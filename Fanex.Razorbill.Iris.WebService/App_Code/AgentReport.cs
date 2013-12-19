using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using IBCUtils;
using IBCUtils.Objects;

/// <summary>
/// Summary description for AgentReport
/// </summary>
[WebService(Namespace = "http://schemas.xmlsoap.org/wsdl/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class AgentReport : IbcWebserviceAgent
{

    public AgentReport()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// CITS
    /// </summary>
    /// <param name="AgentUserName"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public AgentBetListRunning GetAgentBetListRunning()
    {
        int agentId;
        AgentBetListRunning result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_AGENT, out agentId))
            {
                result = new AgentBetListRunning();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = AgentBetListRunning.GetReports(agentId);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }
	/// <summary>
    /// Get list of win/loss - settled bets of all members belonging to an agent
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public AgentBetListWinLoss GetAgentBetListWinLoss(string date)
    {
        int agentId;
        AgentBetListWinLoss result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_AGENT, out agentId))
            {
                result = new AgentBetListWinLoss();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = AgentBetListWinLoss.GetReports(date, agentId);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

}

