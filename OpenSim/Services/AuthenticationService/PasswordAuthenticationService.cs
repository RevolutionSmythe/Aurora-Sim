/*
 * Copyright (c) Contributors, http://aurora-sim.org/, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the Aurora-Sim Project nor the
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
using OpenMetaverse;
using OpenSim.Services.Interfaces;
using log4net;
using Nini.Config;
using System.Reflection;
using OpenSim.Framework;
using Aurora.Framework;
using Aurora.Simulation.Base;

namespace OpenSim.Services.AuthenticationService
{
    // Generic Authentication service used for identifying
    // and authenticating principals.
    // Principals may be clients acting on users' behalf,
    // or any other components that need 
    // verifiable identification.
    //
    public class PasswordAuthenticationService :
            AuthenticationServiceBase, IAuthenticationService, IService
    {
        private static readonly ILog m_log =
                LogManager.GetLogger(
                MethodBase.GetCurrentMethod().DeclaringType);

        public virtual string Name
        {
            get { return GetType().Name; }
        }

        public void Initialize(IConfigSource config, IRegistryCore registry)
        {
            IConfig handlerConfig = config.Configs["Handlers"];
            if (handlerConfig.GetString("AuthenticationHandler", "") != Name)
                return;

            //
            // Try reading the [AuthenticationService] section first, if it exists
            //
            IConfig authConfig = config.Configs["AuthenticationService"];
            if (authConfig != null)
            {
                m_authenticateUsers = authConfig.GetBoolean("AuthenticateUsers", m_authenticateUsers);
            }

            m_Database = Aurora.DataManager.DataManager.RequestPlugin<IAuthenticationData>();
            registry.RegisterModuleInterface<IAuthenticationService>(this);
        }

        public void Start(IConfigSource config, IRegistryCore registry)
        {
        }

        public void FinishedStartup()
        {
        }

        public string Authenticate(UUID principalID, string authType, string password, int lifetime)
        {
            //Return automatically if we do not auth users
            if(!m_authenticateUsers)
                return GetToken(principalID, lifetime);

            AuthData data = m_Database.Get(principalID, authType);

            if (data != null)
            {
                if (authType != "UserAccount")
                {
                    if (data.PasswordHash == password)
                    {
                        //Really should be moved out in the future
                        if (authType == "WebLoginKey")
                            this.Remove (principalID, authType); //Only allow it to be used once
                        return GetToken (principalID, lifetime);
                    }
                }
                else
                {
                    string hashed = Util.Md5Hash (password + ":" +
                            data.PasswordSalt);

                    m_log.TraceFormat ("[PASS AUTH]: got {0}; hashed = {1}; stored = {2}", password, hashed, data.PasswordHash);

                    if (data.PasswordHash == hashed)
                    {
                        return GetToken (principalID, lifetime);
                    }
                }
            }

            m_log.DebugFormat("[AUTH SERVICE]: PrincipalID {0} or its data not found", principalID);
            return String.Empty;
        }
    }
}
