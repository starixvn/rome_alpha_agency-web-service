using System;
using System.Collections.Generic;
using System.Text;


public class AuthHeader : System.Web.Services.Protocols.SoapHeader 
{
    public string Id;
    public string Password;
}

public class IbcWebserviceAgent:System.Web.Services.WebService
{
    public AuthHeader AuthHeaderValue;
    
    public bool  IsValidAgent(int agentType,out int custId)
    {
        if (System.Configuration.ConfigurationManager.AppSettings["testing"] == "1")
        {
            custId = 68289;
            AuthHeaderValue = new AuthHeader();
            AuthHeaderValue.Id = IBCDLL.UserController.GetUsernameById(custId.ToString());
            return true;
        }
        return (ValidateAgent(AuthHeaderValue.Id, AuthHeaderValue.Password,out custId) == agentType);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>RoleId</returns>
    private int ValidateAgent(string username, string password,out int custId)
    {
        int serverId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["server_id"]);
        string site = System.Configuration.ConfigurationManager.AppSettings["site"];
        string ip = Context.Request.UserHostAddress;
        int roleId;
        IBCDLL.DAL.DataResult rs = IBCDLL.UserController.AuthenticateAgent(username, password, serverId, ip, site, out roleId, out custId);
        return roleId;
    }

    public bool IsValidMember(int custId, string username, int roleId)
    {
        int roleIdOutput;
        roleIdOutput = IBCDLL.UserController.AuthenticateMember(custId, username);
        return roleIdOutput == roleId;
    }
}


