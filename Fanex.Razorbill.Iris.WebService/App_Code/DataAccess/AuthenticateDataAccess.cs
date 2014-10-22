// --------------------------------------------------------------------------
// <copyright file="AuthenticateDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Iris.Webservice
{
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;
    using IBCUtils;

    /// <summary>
    /// Authenticate Data Access.
    /// </summary>
    public class AuthenticateDataAccess
    {
        public AuthenticateDataAccess()
        {            
        }

        /// <summary>
        /// Validate Agent Level.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">User Password.</param>
        /// <param name="serverId">Server ID.</param>
        /// <param name="clientIP">Client IP.</param>
        /// <param name="site">System name.</param>
        public ValidationResult ValidateAgentLevel(string username, string password, int serverId, string clientIP, string site)
        {
            using (ObjectDb objDb = new ObjectDb("Age_LoginDownline_API"))
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("username", username);
                parameters.Add("userpwd", Encryption.CfsEnCode(password));
                parameters.Add("loginip", clientIP);
                parameters.Add("sessionid", 0);
                parameters.Add("serverid", serverId);
                parameters.Add("roleid", 0);
                parameters.Add("custid", 0);
                parameters.Add("on_userid", 0);
                parameters.Add("currency", string.Empty);
                parameters.Add("masterusername", string.Empty);
                parameters.Add("subaccid", 0);
                parameters.Add("site", site);
                parameters.Add("errcode", 0);
                parameters.Add("errmsg", string.Empty);

                objDb.AssignParamValues(parameters);
                objDb.ExecuteNonQuery();

                var output = objDb.GetOuputParams();
                return new ValidationResult()
                {
                    Error = new ErrorResult()
                    {
                        ErrorCode = Converter.ToInt(output["errcode"]),
                        ErrorMessage = Converter.ToString2(output["errmsg"])
                    },
                    Customer = new CustomerInfo()
                    {
                        CustId = Converter.ToInt(output["custid"]),
                        SubAccountId = Converter.ToInt(output["subaccid"]),
                        RoleId = Converter.ToInt(output["roleid"]),
                        OnUserId = Converter.ToInt(output["on_userid"]),
                        Currency = Converter.ToString2(output["currency"]),
                        Language = string.Empty,
                        MasterUsername = Converter.ToString2(output["masterusername"]),
                        Site = Converter.ToString2(output["site"])
                    }
                };
            }
        }

        public SMAInfo GetSMAInformation(int custId)
        {
            var result = new SMAInfo(); 
            using (ObjectDb objDb = new ObjectDb("DBA_GetAgent"))
            {
                objDb.AssignParamValues(new Hashtable()
                {
                    {"inputuser", custId}
                });

                var dataReader = objDb.ExecuteReader();
                while (dataReader.Read())
                {
                    result.SuperId = Converter.ToInt(dataReader["superid"]);
                    result.SuperName = Converter.ToString2(dataReader["supername"]);
                    result.MasterId = Converter.ToInt(dataReader["masterid"]);
                    result.MasterName = Converter.ToString2(dataReader["mastername"]);
                    result.AgentId = Converter.ToInt(dataReader["agentid"]);
                    result.AgentName = Converter.ToString2(dataReader["agentname"]);
                }
            }

            return result;
        }
    }
}