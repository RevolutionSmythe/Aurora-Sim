﻿using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Nini.Config;
using OpenMetaverse;
using OpenSim.Framework;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using OpenSim.Services.Interfaces;
using GridRegion = OpenSim.Services.Interfaces.GridRegion;
using Aurora.Simulation.Base;

namespace OpenSim.Services.Connectors.Simulation
{
    public class LocalSimulationServiceConnector : IService, ISimulationService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<Scene> m_sceneList = new List<Scene>();
        private IEntityTransferModule m_AgentTransferModule;
        protected IEntityTransferModule AgentTransferModule
        {
            get
            {
                if (m_AgentTransferModule == null)
                    m_AgentTransferModule = m_sceneList[0].RequestModuleInterface<IEntityTransferModule>();
                return m_AgentTransferModule;
            }
        }

        #region IService Members

        public void Initialize(IConfigSource config, IRegistryCore registry)
        {
            IConfig handlers = config.Configs["Handlers"];
            if (handlers.GetString("SimulationHandler", "") == "LocalSimulationServiceConnector")
                registry.RegisterInterface<ISimulationService>(this);
        }

        public void PostInitialize(IConfigSource config, IRegistryCore registry)
        {
        }

        public void Start(IConfigSource config, IRegistryCore registry)
        {
        }

        public void AddNewRegistry(IConfigSource config, IRegistryCore registry)
        {
            IConfig handlers = config.Configs["Handlers"];
            if (handlers.GetString("SimulationHandler", "") == "LocalSimulationServiceConnector")
                registry.RegisterInterface<ISimulationService>(this);
        }

        #endregion

        #region ISimulation

        public IScene GetScene(ulong regionhandle)
        {
            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionHandle == regionhandle)
                    return s;
            }
            // ? weird. should not happen
            return m_sceneList[0];
        }

        public ISimulationService GetInnerService()
        {
            return this;
        }

        /// <summary>
        /// Can be called from other modules.
        /// </summary>
        /// <param name="scene"></param>
        public void RemoveScene(IScene sscene)
        {
            Scene scene = (Scene)sscene;
            lock (m_sceneList)
            {
                if (m_sceneList.Contains(scene))
                {
                    m_sceneList.Remove(scene);
                }
            }
        }

        /// <summary>
        /// Can be called from other modules.
        /// </summary>
        /// <param name="scene"></param>
        public void Init(IScene sscene)
        {
            Scene scene = (Scene)sscene;
            if (!m_sceneList.Contains(scene))
            {
                lock (m_sceneList)
                {
                    m_sceneList.Add(scene);
                }
            }
        }

        /**
         * Agent-related communications
         */

        public bool CreateAgent(GridRegion destination, AgentCircuitData aCircuit, uint teleportFlags, out string reason)
        {
            if (destination == null)
            {
                reason = "Given destination was null";
                m_log.DebugFormat("[LOCAL SIMULATION CONNECTOR]: CreateAgent was given a null destination");
                return false;
            }

            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionID == destination.RegionID)
                {
                    //m_log.DebugFormat("[LOCAL SIMULATION CONNECTOR]: Found region {0} to send SendCreateChildAgent", destination.RegionName);
                    return s.NewUserConnection(aCircuit, teleportFlags, out reason);
                }
            }

            m_log.DebugFormat("[LOCAL SIMULATION CONNECTOR]: Did not find region {0} for SendCreateChildAgent", destination.RegionName);
            reason = "Did not find region " + destination.RegionName;
            return false;
        }

        public bool UpdateAgent(GridRegion destination, AgentData cAgentData)
        {
            if (destination == null)
                return false;

            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionHandle == destination.RegionHandle)
                {
                    //m_log.DebugFormat(
                    //    "[LOCAL SIMULATION CONNECTOR]: Found region {0} {1} to send AgentUpdate",
                    //    s.RegionInfo.RegionName, destination.RegionHandle);

                    s.IncomingChildAgentDataUpdate(cAgentData);
                    return true;
                }
            }

            //            m_log.DebugFormat("[LOCAL COMMS]: Did not find region {0} for ChildAgentUpdate", regionHandle);
            return false;
        }

        public bool UpdateAgent(GridRegion destination, AgentPosition cAgentData)
        {
            if (destination == null)
                return false;

            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionHandle == destination.RegionHandle)
                {
                    //m_log.Debug("[LOCAL COMMS]: Found region to send ChildAgentUpdate");
                    s.IncomingChildAgentDataUpdate(cAgentData);
                    return true;
                }
            }
            //m_log.Debug("[LOCAL COMMS]: region not found for ChildAgentUpdate");
            return false;
        }

        public bool RetrieveAgent(GridRegion destination, UUID id, out IAgentData agent)
        {
            agent = null;

            if (destination == null)
                return false;

            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionHandle == destination.RegionHandle)
                {
                    //m_log.Debug("[LOCAL COMMS]: Found region to send ChildAgentUpdate");
                    return s.IncomingRetrieveRootAgent(id, out agent);
                }
            }
            //m_log.Debug("[LOCAL COMMS]: region not found for ChildAgentUpdate");
            return false;
        }

        public bool ReleaseAgent(UUID origin, UUID id, string uri)
        {
            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionID == origin)
                {
                    //m_log.Debug("[LOCAL COMMS]: Found region to SendReleaseAgent");
                    AgentTransferModule.AgentArrivedAtDestination(id);
                    return true;
                    //                    return s.IncomingReleaseAgent(id);
                }
            }
            //m_log.Debug("[LOCAL COMMS]: region not found in SendReleaseAgent " + origin);
            return false;
        }

        public bool CloseAgent(GridRegion destination, UUID id)
        {
            if (destination == null)
                return false;

            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionID == destination.RegionID)
                {
                    //m_log.Debug("[LOCAL COMMS]: Found region to SendCloseAgent");
                    return s.IncomingCloseAgent(id);
                }
            }
            //m_log.Debug("[LOCAL COMMS]: region not found in SendCloseAgent");
            return false;
        }

        /**
         * Object-related communications
         */

        public bool CreateObject(GridRegion destination, ISceneObject sog, bool isLocalCall)
        {
            if (destination == null)
                return false;

            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionHandle == destination.RegionHandle)
                {
                    //m_log.Debug("[LOCAL COMMS]: Found region to SendCreateObject");
                    if (isLocalCall)
                    {
                        // We need to make a local copy of the object
                        ISceneObject sogClone = sog.CloneForNewScene(s);
                        sogClone.SetState(sog.GetStateSnapshot(), s);
                        return s.IncomingCreateObject(sogClone);
                    }
                    else
                    {
                        // Use the object as it came through the wire
                        return s.IncomingCreateObject(sog);
                    }
                }
            }
            return false;
        }

        public bool CreateObject(GridRegion destination, UUID userID, UUID itemID)
        {
            if (destination == null)
                return false;

            foreach (Scene s in m_sceneList)
            {
                if (s.RegionInfo.RegionHandle == destination.RegionHandle)
                {
                    return s.IncomingCreateObject(userID, itemID);
                }
            }
            return false;
        }

        #endregion /* ISimulationService */

        #region Misc

        public bool IsLocalRegion(ulong regionhandle)
        {
            foreach (Scene s in m_sceneList)
                if (s.RegionInfo.RegionHandle == regionhandle)
                    return true;
            return false;
        }

        public bool IsLocalRegion(UUID id)
        {
            foreach (Scene s in m_sceneList)
                if (s.RegionInfo.RegionID == id)
                    return true;
            return false;
        }

        #endregion
    }
}