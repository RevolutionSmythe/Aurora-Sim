/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Nini.Config;
using OpenMetaverse;
using OpenSim.Framework;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;

namespace OpenSim.Region.CoreModules.Agent.AssetTransaction
{
    public class AssetTransactionModule : ISharedRegionModule, IAgentAssetTransactions
    {
//        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly Dictionary<UUID, Scene> RegisteredScenes = new Dictionary<UUID, Scene>();
        private bool m_dumpAssetsToFile = false;
        private Scene m_scene = null;

        //[Obsolete] //As long as this is being used to get objects that are not region specific, this is fine to use
        public Scene MyScene
        {
            get{ return m_scene;}
        }

        /// <summary>
        /// Each agent has its own singleton collection of transactions
        /// </summary>
        private Dictionary<UUID, AgentAssetTransactions> AgentTransactions =
            new Dictionary<UUID, AgentAssetTransactions>();
        

        public AssetTransactionModule()
        {
            //m_log.Debug("creating AgentAssetTransactionModule");
        }

        #region IRegionModule Members

        public void Initialise(IConfigSource config)
        {
        }

        public void AddRegion(Scene scene)
        {
            if (!RegisteredScenes.ContainsKey(scene.RegionInfo.RegionID))
            {
                // m_log.Debug("initialising AgentAssetTransactionModule");
                RegisteredScenes.Add(scene.RegionInfo.RegionID, scene);
                scene.RegisterModuleInterface<IAgentAssetTransactions>(this);

                scene.EventManager.OnNewClient += NewClient;
                scene.EventManager.OnClosingClient += OnClosingClient;
                scene.EventManager.OnClientClosed += OnClientClosed;
            }

            // EVIL HACK!
            // This needs killing!
            //
            if (m_scene == null)
                m_scene = scene;
        }

        public void RemoveRegion(Scene scene)
        {
            if (RegisteredScenes.ContainsKey(scene.RegionInfo.RegionID))
            {
                // m_log.Debug("initialising AgentAssetTransactionModule");
                RegisteredScenes.Remove(scene.RegionInfo.RegionID);
                scene.UnregisterModuleInterface<IAgentAssetTransactions>(this);

                scene.EventManager.OnNewClient -= NewClient;
                scene.EventManager.OnClosingClient -= OnClosingClient;
                scene.EventManager.OnClientClosed -= OnClientClosed;
            }
        }

        public void RegionLoaded(Scene scene)
        {

        }

        public Type ReplaceableInterface
        {
            get { return null; }
        }

        public void PostInitialise()
        {
        }

        public void Close()
        {
        }

        public string Name
        {
            get { return "AgentTransactionModule"; }
        }

        public bool IsSharedModule
        {
            get { return true; }
        }

        #endregion

        public void NewClient(IClientAPI client)
        {
            client.OnAssetUploadRequest += HandleUDPUploadRequest;
            client.OnXferReceive += HandleXfer;
        }

        private void OnClosingClient(IClientAPI client)
        {
            client.OnAssetUploadRequest -= HandleUDPUploadRequest;
            client.OnXferReceive -= HandleXfer;
        }

        private void OnClientClosed(UUID clientID, Scene scene)
        {
            ScenePresence SP = scene.GetScenePresence(clientID);
            if(SP != null && !SP.IsChildAgent)
                RemoveAgentAssetTransactions(clientID);
        }

        #region AgentAssetTransactions
        /// <summary>
        /// Get the collection of asset transactions for the given user.  If one does not already exist, it
        /// is created.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private AgentAssetTransactions GetUserTransactions(UUID userID)
        {
            lock (AgentTransactions)
            {
                if (!AgentTransactions.ContainsKey(userID))
                {
                    AgentAssetTransactions transactions  = new AgentAssetTransactions(userID, this, m_dumpAssetsToFile);
                    AgentTransactions.Add(userID, transactions);
                }

                return AgentTransactions[userID];
            }
        }

        /// <summary>
        /// Remove the given agent asset transactions.  This should be called when a client is departing
        /// from a scene (and hence won't be making any more transactions here).
        /// </summary>
        /// <param name="userID"></param>
        public void RemoveAgentAssetTransactions(UUID userID)
        {
            // m_log.DebugFormat("Removing agent asset transactions structure for agent {0}", userID);

            lock (AgentTransactions)
            {
                AgentTransactions.Remove(userID);
            }
        }

        /// <summary>
        /// Create an inventory item from data that has been received through a transaction.
        ///
        /// This is called when new clothing or body parts are created.  It may also be called in other
        /// situations.
        /// </summary>
        /// <param name="remoteClient"></param>
        /// <param name="transactionID"></param>
        /// <param name="folderID"></param>
        /// <param name="callbackID"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="invType"></param>
        /// <param name="type"></param>
        /// <param name="wearableType"></param>
        /// <param name="nextOwnerMask"></param>
        public void HandleItemCreationFromTransaction(IClientAPI remoteClient, UUID transactionID, UUID folderID,
                                                      uint callbackID, string description, string name, sbyte invType,
                                                      sbyte type, byte wearableType, uint nextOwnerMask)
        {
            //            m_log.DebugFormat(
            //                "[TRANSACTIONS MANAGER] Called HandleItemCreationFromTransaction with item {0}", name);

            AgentAssetTransactions transactions = GetUserTransactions(remoteClient.AgentId);

            IMonitorModule monitorModule = m_scene.RequestModuleInterface<IMonitorModule>();
            if (monitorModule != null)
            {
                INetworkMonitor networkMonitor = (INetworkMonitor)monitorModule.GetMonitor(m_scene.RegionInfo.RegionID.ToString(), "Network Monitor");
                networkMonitor.AddPendingUploads(1);
            }

            transactions.RequestCreateInventoryItem(
                remoteClient, transactionID, folderID, callbackID, description,
                name, invType, type, wearableType, nextOwnerMask);
        }

        /// <summary>
        /// Update an inventory item with data that has been received through a transaction.
        ///
        /// This is called when clothing or body parts are updated (for instance, with new textures or
        /// colours).  It may also be called in other situations.
        /// </summary>
        /// <param name="remoteClient"></param>
        /// <param name="transactionID"></param>
        /// <param name="item"></param>
        public void HandleItemUpdateFromTransaction(IClientAPI remoteClient, UUID transactionID,
                                                    InventoryItemBase item)
        {
            //            m_log.DebugFormat(
            //                "[TRANSACTIONS MANAGER] Called HandleItemUpdateFromTransaction with item {0}",
            //                item.Name);

            AgentAssetTransactions transactions = GetUserTransactions(remoteClient.AgentId);

            IMonitorModule monitorModule = m_scene.RequestModuleInterface<IMonitorModule>();
            if (monitorModule != null)
            {
                INetworkMonitor networkMonitor = (INetworkMonitor)monitorModule.GetMonitor(m_scene.RegionInfo.RegionID.ToString(), "Network Monitor");
                networkMonitor.AddPendingUploads(1);
            }

            transactions.RequestUpdateInventoryItem(remoteClient, transactionID, item);
        }

        /// <summary>
        /// Update a task inventory item with data that has been received through a transaction.
        ///
        /// This is currently called when, for instance, a notecard in a prim is saved.  The data is sent
        /// up through a single AssetUploadRequest.  A subsequent UpdateTaskInventory then references the transaction
        /// and comes through this method.
        /// </summary>
        /// <param name="remoteClient"></param>
        /// <param name="transactionID"></param>
        /// <param name="item"></param>
        public void HandleTaskItemUpdateFromTransaction(
            IClientAPI remoteClient, SceneObjectPart part, UUID transactionID, TaskInventoryItem item)
        {
            //            m_log.DebugFormat(
            //                "[TRANSACTIONS MANAGER] Called HandleTaskItemUpdateFromTransaction with item {0}",
            //                item.Name);

            AgentAssetTransactions transactions = GetUserTransactions(remoteClient.AgentId);

            IMonitorModule monitorModule = m_scene.RequestModuleInterface<IMonitorModule>();
            if (monitorModule != null)
            {
                INetworkMonitor networkMonitor = (INetworkMonitor)monitorModule.GetMonitor(m_scene.RegionInfo.RegionID.ToString(), "Network Monitor");
                networkMonitor.AddPendingUploads(1);
            }

            transactions.RequestUpdateTaskInventoryItem(remoteClient, part, transactionID, item);
        }

        /// <summary>
        /// Request that a client (agent) begin an asset transfer.
        /// </summary>
        /// <param name="remoteClient"></param>
        /// <param name="assetID"></param>
        /// <param name="transaction"></param>
        /// <param name="type"></param>
        /// <param name="data"></param></param>
        /// <param name="tempFile"></param>
        public void HandleUDPUploadRequest(IClientAPI remoteClient, UUID assetID, UUID transaction, sbyte type,
                                           byte[] data, bool storeLocal, bool tempFile)
        {
//            m_log.Debug("HandleUDPUploadRequest - assetID: " + assetID.ToString() + " transaction: " + transaction.ToString() + " type: " + type.ToString() + " storelocal: " + storeLocal + " tempFile: " + tempFile);
            
            if (((AssetType)type == AssetType.Texture ||
                (AssetType)type == AssetType.Sound ||
                (AssetType)type == AssetType.TextureTGA ||
                (AssetType)type == AssetType.Animation) &&
                tempFile == false)
            {
                Scene scene = (Scene)remoteClient.Scene;
                IMoneyModule mm = scene.RequestModuleInterface<IMoneyModule>();

                if (mm != null)
                {
                    if (!mm.UploadCovered(remoteClient, mm.UploadCharge))
                    {
                        remoteClient.SendAgentAlertMessage("Unable to upload asset. Insufficient funds.", false);
                        return;
                    }
                }
            }

            AgentAssetTransactions transactions = GetUserTransactions(remoteClient.AgentId);

            IMonitorModule monitorModule = m_scene.RequestModuleInterface<IMonitorModule>();
            if (monitorModule != null)
            {
                INetworkMonitor networkMonitor = (INetworkMonitor)monitorModule.GetMonitor(m_scene.RegionInfo.RegionID.ToString(), "Network Monitor");
                networkMonitor.AddPendingUploads(1);
            }

            AssetXferUploader uploader = transactions.RequestXferUploader(transaction);
            if (uploader != null)
            {
                uploader.Initialise(remoteClient, assetID, transaction, type, data, storeLocal, tempFile);
            }
        }

        /// <summary>
        /// Handle asset transfer data packets received in response to the asset upload request in
        /// HandleUDPUploadRequest()
        /// </summary>
        /// <param name="remoteClient"></param>
        /// <param name="xferID"></param>
        /// <param name="packetID"></param>
        /// <param name="data"></param>
        public void HandleXfer(IClientAPI remoteClient, ulong xferID, uint packetID, byte[] data)
        {
            //m_log.Debug("xferID: " + xferID + "  packetID: " + packetID + "  data!");
            AgentAssetTransactions transactions = GetUserTransactions(remoteClient.AgentId);

            IMonitorModule monitorModule = m_scene.RequestModuleInterface<IMonitorModule>();
            if (monitorModule != null)
            {
                INetworkMonitor networkMonitor = (INetworkMonitor)monitorModule.GetMonitor(m_scene.RegionInfo.RegionID.ToString(), "Network Monitor");
                networkMonitor.AddPendingUploads(1);
            }

            transactions.HandleXfer(remoteClient, xferID, packetID, data);
        }

        #endregion
    }
}
