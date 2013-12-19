using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using IBCUtils;
using IBCUtils.Objects;


/// <summary>
/// Summary description for MasterReport
/// </summary>
[WebService(Namespace = "http://schemas.xmlsoap.org/wsdl/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class MasterReport : IbcWebserviceAgent
{

    public MasterReport()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    /// <summary>
    /// CITS
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterAgentWinLoss AgentWinLoss(string fromdate, string todate)
    {
        int masterId;
        MasterAgentWinLoss result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterAgentWinLoss();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MasterAgentWinLoss.GetReport(AuthHeaderValue.Id, fromdate, todate);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// CITS
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterAgentWinLossDetail AgentWinLossDetail(string fromdate, string todate)
    {
        int masterId;
        MasterAgentWinLossDetail result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterAgentWinLossDetail();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MasterAgentWinLossDetail.GetReport(AuthHeaderValue.Id, fromdate, todate);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// CITS
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterAgentWinLossOld AgentWinLossOld(string fromdate, string todate)
    {
        int masterId;
        MasterAgentWinLossOld result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterAgentWinLossOld();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MasterAgentWinLossOld.GetReport(AuthHeaderValue.Id, fromdate, todate);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// CITS
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterMatchWinLossDetail MatchWinLossDetail(string fromdate, string todate, string language)
    {
        int masterId;
        MasterMatchWinLossDetail result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterMatchWinLossDetail();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MasterMatchWinLossDetail.GetReport(masterId, fromdate, todate, language);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterStatement Statement()
    {
        int masterId;
        MasterStatement result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterStatement();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MasterStatement.GetReport(masterId);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }


    /// <summary>
    /// CITS ITMASTER/68287
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterAgentBalance AgentBalanceActive()
    {
        int masterId;
        MasterAgentBalance result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterAgentBalance();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MasterAgentBalance.GetReport(masterId, false);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// CITS ITMASTER/68287
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterAgentBalance AgentBalanceClosed()
    {
        int masterId;
        MasterAgentBalance result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterAgentBalance();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MasterAgentBalance.GetReport(masterId, true);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MemberCancelledList GetMemberCancelledList(string agentUsername)
    {
        int masterId;
        MemberCancelledList result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MemberCancelledList();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                if (IsValidMember(masterId, agentUsername, UserInfo.ROLE_AGENT))
                {
                    result = MemberCancelledList.GetMemberList(agentUsername, UserInfo.ROLE_MEMBER);
                }
                else
                {
                    result = new MemberCancelledList();
                    result.ErrorCode = IbcError.USERID_INVALID;
                    result.ErrorMsg = IbcError.USERID_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MemberCancelledList GetAgentCancelledList()
    {
        int masterId;
        MemberCancelledList result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MemberCancelledList();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MemberCancelledList.GetMemberList(AuthHeaderValue.Id, UserInfo.ROLE_AGENT);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// username = all => List cancelled bets of all agent
    /// username = agent => List cancelled bets of one agent
    /// </summary>
    /// <param name="username"></param>
    /// <param name="userType">1:customer; 2: agent; 3: master</param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public CancelledBets CancelledBet(string username, int userType)
    {
        int masterId;
        CancelledBets result = null;

        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new CancelledBets();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                if (username.ToLower().Equals("all"))
                {
                    result = CancelledBets.GetCancelledList(AuthHeaderValue.Id, UserInfo.ROLE_MASTER);
                }
                else if (userType == UserInfo.ROLE_MEMBER)
                {
                    if (IsValidMember(masterId, username, UserInfo.ROLE_MEMBER))
                    {
                        result = CancelledBets.GetCancelledList(username, UserInfo.ROLE_MEMBER);
                    }
                    else
                    {
                        result = new CancelledBets();
                        result.ErrorCode = IbcError.USERID_INVALID;
                        result.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                    }
                }
                else if (userType == UserInfo.ROLE_AGENT)
                {
                    if (IsValidMember(masterId, username, UserInfo.ROLE_AGENT))
                    {
                        result = CancelledBets.GetCancelledList(username, UserInfo.ROLE_AGENT);
                    }
                    else
                    {
                        result = new CancelledBets();
                        result.ErrorCode = IbcError.USERID_INVALID;
                        result.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                    }
                }
                else
                {
                    result = new CancelledBets();
                    result.ErrorCode = IbcError.AGENT_ROLE_INVALID;
                    result.ErrorMsg = IbcError.AGENT_ROLE_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
    
        return result;
    }

    /// <summary>
    /// CITS - ITMASTER/68287
    /// </summary>
    /// <param name="FromUserName"></param>
    /// <param name="ToUserName"></param>
    /// <param name="Amount"></param>
    /// <param name="lang"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MasterTransfer Transfer(string AgentUserName, double amount, string language)
    {
        int masterId;
        MasterTransfer result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MasterTransfer();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                if (IsValidMember(masterId, AgentUserName, UserInfo.ROLE_AGENT))
                {
                    result = MasterTransfer.Transfer(masterId, AgentUserName, amount, language);
                }
                else
                {
                    result = new MasterTransfer();
                    result.ErrorCode = IbcError.USERID_INVALID;
                    result.ErrorMsg = IbcError.USERID_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }
    /// <summary>
    /// CITS imbu02/405006
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public LeagueForResult League(string fromdate, string language)
    {
        int masterId;
        LeagueForResult result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new LeagueForResult();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = LeagueForResult.GetReport(fromdate, language);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// CITS imbu02/405006
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="leagueId"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public GameResult Result(string eventdate, int leagueId, string language)
    {
        int masterId;
        GameResult result = null;
        try
        {

            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new GameResult();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = GameResult.GetReport(eventdate, leagueId, language);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }


    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MemberBetLists3 GetAgentWinLossMemberBetList(string fromdate, string todate, string username, string language)
    {
        int masterId;
        MemberBetLists3 result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MemberBetLists3();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                if (IsValidMember(masterId, username, UserInfo.ROLE_MEMBER))
                {
                    result = MemberBetLists3.GetReport(username, fromdate, todate, language, 1, "", 0);
                }
                else
                {
                    result = new MemberBetLists3();
                    result.ErrorCode = IbcError.USERID_INVALID;
                    result.ErrorMsg = IbcError.USERID_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }


    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MemberBetLists3 GetAgentWinLossMemberBetListOld(string fromdate, string todate, string username, string language)
    {
        int masterId;
        MemberBetLists3 result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MemberBetLists3();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                if (IsValidMember(masterId, username, UserInfo.ROLE_MEMBER))
                {
                    result = MemberBetLists3.GetReport(username, fromdate, todate, language, 2, "", 0);
                }
                else
                {
                    result = new MemberBetLists3();
                    result.ErrorCode = IbcError.USERID_INVALID;
                    result.ErrorMsg = IbcError.USERID_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }
    
    /// <summary>
    /// khoa remove fromdatem todate at 12-21-2007
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <param name="matchId"></param>
    /// <param name="bettype"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public MemberBetLists3 GetMemberBetListByMatch( string matchId, int bettype, string language)
    {
        string fromdate = DateTime.Today.ToShortDateString(), todate = DateTime.Today.ToShortDateString();
        
        int masterId;
        MemberBetLists3 result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new MemberBetLists3();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = MemberBetLists3.GetReport(AuthHeaderValue.Id, fromdate, todate, language, 4, matchId, bettype);
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <param name="username">memberId</param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public AgentMemberWinLoss MemberWinLoss( string fromdate, string todate,string username )
    {
        int masterId;
        AgentMemberWinLoss result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new AgentMemberWinLoss();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                if (IsValidMember(masterId, username, UserInfo.ROLE_AGENT))
                {
                    result = AgentMemberWinLoss.GetReport(username, fromdate, todate);
                }
                else
                {
                    result = new AgentMemberWinLoss();
                    result.ErrorCode = IbcError.USERID_INVALID;
                    result.ErrorMsg = IbcError.USERID_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <param name="username">memberId</param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public AgentMemberWinLossOld MemberWinLossOld(string fromdate, string todate,string username)
    {
        int masterId;
        AgentMemberWinLossOld result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new AgentMemberWinLossOld();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                if (IsValidMember(masterId, username, UserInfo.ROLE_AGENT))
                {
                    result = AgentMemberWinLossOld.GetReport(username, fromdate, todate);
                }
                else
                {
                    result = new AgentMemberWinLossOld();
                    result.ErrorCode = IbcError.USERID_INVALID;
                    result.ErrorMsg = IbcError.USERID_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public BetSlip GetBetSlip(string refno, string language)
    {
        int masterId;
        BetSlip result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new BetSlip();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                result = BetSlip.GetReport(refno, language);
                if (!IsValidMember(masterId, result.Username, UserInfo.ROLE_MEMBER))
                {
                    result = new BetSlip();
                    result.ErrorCode = IbcError.TRANS_ID_INVALID;
                    result.ErrorMsg = IbcError.TRANS_ID_INVALID_MSG;
                }

            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fromdate"></param>
    /// <param name="todate"></param>
    /// <param name="username">memberId</param>
    /// <returns></returns>
    [WebMethod]
    [SoapHeader("AuthHeaderValue")]
    public AgentMemberWinLossDetail MemberWinLossDetail(string fromdate, string todate,string username)
    {
        int masterId;
        AgentMemberWinLossDetail result = null;
        try
        {
            if (!IsValidAgent(UserInfo.ROLE_MASTER, out masterId))
            {
                result = new AgentMemberWinLossDetail();
                result.ErrorCode = IbcError.USERID_INVALID;
                result.ErrorMsg = IbcError.USERID_INVALID_MSG;
            }
            else
            {
                if (IsValidMember(masterId, username, UserInfo.ROLE_AGENT))
                {
                    result = AgentMemberWinLossDetail.GetReport(username, fromdate, todate);
                }
                else
                {
                    result = new AgentMemberWinLossDetail();
                    result.ErrorCode = IbcError.USERID_INVALID;
                    result.ErrorMsg = IbcError.USERID_INVALID_MSG;
                }
            }
        }
        catch (Exception e)
        {
            LogService.WriteLog(e);
        }
        return result;
    }
}

