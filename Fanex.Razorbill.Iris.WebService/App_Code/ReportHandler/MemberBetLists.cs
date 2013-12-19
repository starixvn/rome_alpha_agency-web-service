using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IBCUtils;
using IBCDLL.DAL;
using IBCDLL.AgentReport;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Summary description for MemberBetLists
/// </summary>
public class MemberBetListsRow
{
    public MemberBetListsRow()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string MemberId;
    public string TransactionTime;
    public BetEvent Match = new BetEvent();
    public MPBetEvent[] Matchs;
    public string Choice;
    public string Handicap;
    public string LiveScore;
    public double Odds;
    public double Stake;
    public string Status;
    public double AgentPosition;
    
}
public class MemberBetLists:IbcError
{
    public MemberBetListsRow[] Rows;
   public static MPBetEvent[] GetMixParlayReport(string refno, string language)
    {
        List<MPBetEvent> reportMP = new List<MPBetEvent>();
        Hashtable paramMP = new Hashtable();
        paramMP.Add("refno", refno);
        paramMP.Add("Language", language);
        using (IDataReader drMP = DataServices.ExecuteReader("BetsSelMTeamList", paramMP, DatabaseType.ARCHIVE))
        {
            if (drMP != null)
            {

                int bettypeMP;
                string betteamMP;
                while (drMP.Read())
                {
                    MPBetEvent rowMP = new MPBetEvent();
                    rowMP.RefNo = drMP["refno"].ToString();
                    bettypeMP = FormatHandler.GetInt(drMP["bettype"]);
                    betteamMP = drMP["betteam"].ToString();
                    rowMP.Home = drMP["home"].ToString();
                    rowMP.Away = drMP["away"].ToString();
                    rowMP.League = drMP["leaguename"].ToString();
                    rowMP.Bettype = drMP["typenamee2"].ToString();
                    rowMP.Odd = FormatHandler.GetDecimal(drMP["odds"]);
                    if (bettypeMP == 1)
                    {
                        if (betteamMP == "h")
                        {
                            rowMP.Choice = drMP["home"].ToString();
                            rowMP.Handicap = FormatHandler.GetDecimal(drMP["hdp2"]) - FormatHandler.GetDecimal(drMP["hdp1"]);
                        }
                        else
                        {
                            rowMP.Choice = drMP["away"].ToString();
                            rowMP.Handicap = FormatHandler.GetDecimal(drMP["hdp1"]) - FormatHandler.GetDecimal(drMP["hdp2"]);
                        }

                    }
                    else if (bettypeMP == 2)
                    {

                        if (betteamMP == "h")
                        {
                            rowMP.Choice = "Odd";
                        }
                        else
                        {
                            rowMP.Choice = "Even";
                        }
                    }
                    else if (bettypeMP == 3)
                    {
                        rowMP.Handicap = FormatHandler.GetDecimal(drMP["hdp1"]);
                        if (betteamMP == "h")
                        {
                            rowMP.Choice = "Over";
                        }
                        else
                        {
                            rowMP.Choice = "Under";
                        }
                    }
                    else if (bettypeMP == 4)
                    {
                        rowMP.Choice = drMP["hdp1"].ToString() + ":" + drMP["hdp2"].ToString();

                    }
                    else if (bettypeMP == 5 || bettypeMP == 6)
                    {
                        rowMP.Choice = drMP["betteam"].ToString();

                    }
                    else if (bettypeMP == 7)
                    {
                        if (betteamMP == "h")
                        {
                            rowMP.Choice = drMP["home"].ToString();
                            rowMP.Handicap = FormatHandler.GetDecimal(drMP["hdp2"]) - FormatHandler.GetDecimal(drMP["hdp1"]);
                        }
                        else
                        {
                            rowMP.Choice = drMP["away"].ToString();
                            rowMP.Handicap = FormatHandler.GetDecimal(drMP["hdp1"]) - FormatHandler.GetDecimal(drMP["hdp2"]);
                        }
                    }
                    reportMP.Add(rowMP);
                }// end while (drMP.Read())
            }// end  if (drMP == null)
        }// end using
        return reportMP.ToArray();
    }

    public static MemberBetLists GetReport(string username, string fromdate, string todate, string language)
    {
        MemberBetLists report = new MemberBetLists();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate, tdate;
        if (!Validator.ValidDateMMDDYYYY(fromdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(todate, out tdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        Hashtable param = new Hashtable();
        //fromdate = fromdate != "" ? fromdate : DateTime.Now.ToShortDateString();
        //todate = todate != "" ? todate : DateTime.Now.ToShortDateString();
        param.Add("username", username);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        param.Add("language", language);
        using (IDataReader dr = DataServices.ExecuteReader("BetsSelBetListRunningAg", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MemberBetListsRow> reportRow = new List<MemberBetListsRow>();
                int bettype;
                string betteam = "";
                bool isLive = false;
                while (dr.Read())
                {
                    MemberBetListsRow row = new MemberBetListsRow();
                    row.MemberId = username;
                    row.Match.League = dr["leaguename"].ToString();
                   
                    row.TransactionTime = dr["transdate"].ToString();
                    bettype = FormatHandler.GetInt(dr["bettype"]);
                    betteam = dr["betteam"].ToString();
                    row.Match.RefNo = dr["refno"].ToString();
                    row.Match.Home = dr["home"].ToString();
                    row.Match.Away = dr["away"].ToString();
                    row.Match.Bettype = dr["typenamee"].ToString();
                    row.Odds = FormatHandler.GetDouble(dr["odds"],2);
                    row.Stake = FormatHandler.GetDouble(dr["stake"],2);
                    row.AgentPosition = FormatHandler.GetDouble(dr["apositiontaking"]) * 100;
                    row.AgentPosition = FormatHandler.GetDouble(row.AgentPosition, 2);

                    if (DBNull.Value == dr["status"])
                    {
                        row.Status = "-";
                    }
                    else
                    {
                        row.Status = dr["status"].ToString();
                    }
                    if (bettype == 1 || bettype == 7)
                    {
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }

                        if (betteam == "h")
                        {
                            row.Choice = dr["home"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp2"]) - FormatHandler.GetDouble(dr["hdp1"]));
                        }
                        else
                        {
                            row.Choice = dr["away"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp1"]) - FormatHandler.GetDouble(dr["hdp2"]));
                        }
                    }
                    else if (bettype == 2)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Odd";
                        }
                        else
                        {
                            row.Choice = "Even";
                        }
                        row.Handicap = "0";
                    }
                    else if (bettype == 3 || bettype == 8)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Over";
                        }
                        else
                        {
                            row.Choice = "Under";
                        }
                        row.Handicap = dr["hdp1"].ToString();
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }
                    }
                    else if (bettype == 4)
                    {
                        row.Choice = "Correct Score";
                        row.Handicap = dr["hdp1"].ToString() + ":" + dr["hdp2"].ToString();
                    }
                    else if (bettype == 6 || bettype == 5 || bettype == 16)
                    {
                        row.Choice = betteam;
                    }
                    else if (bettype == 10)
                    {
                        row.Choice = dr["home"].ToString();
                        row.Match.Away = "";
                        row.Match.Home = "";
                        row.Match.League = dr["leaguename"].ToString();
                        row.Handicap = "0";
                    }
                    else if (bettype == 9)
                    {
                        try
                        {
                            row.Matchs = GetMixParlayReport(dr["refno"].ToString(), language);
                        }
                        catch
                        {
                            report.ErrorCode = IbcError.INTERNAL_ERROR;
                            report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                        }

                    }// end bettype = 9
                    reportRow.Add(row);
                }//end while
                report.Rows = reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }// end if(dr == null)
        }// end using
        return report;
    }
    public static MemberBetLists BetList(string username, int agentId, string fromdate, string todate, string language)
    {
        MemberBetLists report = new MemberBetLists();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate, tdate;
        if (!Validator.ValidDateMMDDYYYY(fromdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(todate, out tdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        Hashtable param = new Hashtable();
        param.Add("username", username);
        param.Add("agid", agentId);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        param.Add("language", language);
        using (IDataReader dr = DataServices.ExecuteReader("BetsSelBetListEnglishAgent2", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MemberBetListsRow> reportRow = new List<MemberBetListsRow>();
                string betteam;
                int bettype;
                bool isLive = false;
                while (dr.Read())
                {
                    MemberBetListsRow row = new MemberBetListsRow();
                    row.MemberId = username;
                    row.TransactionTime = dr["transdate"].ToString();
                    row.Match.RefNo = dr["refno"].ToString();
                    row.Match.Home = dr["home"].ToString();
                    row.Match.Away = dr["away"].ToString();
                    row.Match.Bettype = dr["typename2"].ToString();
                    row.Match.League = dr["leaguename2"].ToString();
                    row.Status = dr["status"].ToString();
                    betteam = dr["betteam"].ToString();
                    bettype = FormatHandler.GetInt(dr["bettype"]);
                    row.Odds = FormatHandler.GetDouble(dr["odds"]);
                    row.Stake = FormatHandler.GetDouble(dr["stake"]);
                    if (bettype == 1 || bettype == 7)
                    {
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }
                        if (betteam == "h")
                        {
                            row.Choice = dr["home"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp2"]) - FormatHandler.GetDouble(dr["hdp1"]));
                        }
                        else
                        {
                            row.Choice = dr["away"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp1"]) - FormatHandler.GetDouble(dr["hdp2"]));
                        }
                    }
                    else if (bettype == 2)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Odd";
                        }
                        else
                        {
                            row.Choice = "Even";
                        }
                        row.Handicap = "0";
                    }
                    else if (bettype == 3 || bettype == 8)
                    {
                        row.Handicap = dr["hdp1"].ToString();
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }
                        if (betteam == "h")
                        {
                            row.Choice = "Over";
                        }
                        else
                        {
                            row.Choice = "Under";
                        }
                    }
                    else if (bettype == 4)
                    {
                        row.Choice = "Correct Score";
                        row.Handicap = dr["hdp1"].ToString() + ":" + dr["hdp2"].ToString();
                    }
                    else if (bettype == 6 || bettype == 5 || bettype == 16)
                    {
                        row.Choice = betteam;
                    }
                    else if (bettype == 10)
                    {
                        row.Choice = dr["home"].ToString();
                    }
                    else if (bettype == 9)
                    {
                        try
                        {
                            row.Matchs = GetMixParlayReport(dr["refno"].ToString(), language);
                        }
                        catch
                        {
                            report.ErrorCode = IbcError.INTERNAL_ERROR;
                            report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                        }
                    }
                    reportRow.Add(row);
                }// end while
                report.Rows = reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }// end if(dr == null)
        }// end using
        return report;
    }

}

public class MemberBetListsRow2 : MemberBetListsRow
{
    public MemberBetListsRow2()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public double MemberWinLoss;
    public double MemberCommission;
}

public class MemberBetLists2 : IbcError
{
    public MemberBetListsRow2[] Rows;
    /// <summary>
    /// spNo = 1 => spName = "BetsSelMWinLoss", dbType = DatabaseType.ARCHIVE
    /// use case: memberwinloss-> memberbetlist, AgentWinloss -> memberbetlist,masterwinloss -> memberbetlist
    /// spNo = 2 => spName = "BetsSelMWinLossOld", dbType = DatabaseType.WAREHOUSE
    /// use case: memberwinlossOld -> memberbetlist, AgentwinlossOld --> memberbetlist
    /// spNo = 3 => spName = "BetsSelBetListMatchWinLoss_Ag", dbType = DatabaseType.ARCHIVE
    /// use case: matchwinloss -> Member betlist
    /// spNo = 4 => spName = "BetsSelBetListMatchWinLoss_MAg",dbType = DatabaseType.ARCHIVE
    /// use case: match winloss detail-> member betlist
    /// spNo = 5 => spName = "BetsSelBetListMatchWinLoss_SAg", dbType = DatabaseType.ARCHIVE
    /// </summary>
    /// <param name="spNo"></param>
    /// <param name="dbType"></param>
    /// <returns></returns>
    public static string GetStoreProcedureName(int spNo, out int dbType)
    {
        dbType = DatabaseType.ARCHIVE;
        string spName = "";
        if (spNo == 1)
        {
            spName = "BetsSelMWinLoss";
        }
        else if (spNo == 2)
        {
            spName = "BetsSelMWinLossOld";
            dbType = DatabaseType.WAREHOUSE;
        }
        else if (spNo == 3)
        {
            spName = "BetsSelBetListMatchWinLoss_Ag";
        }
        else if (spNo == 4)
        {
            spName = "BetsSelBetListMatchWinLoss_MAg";
        }
        else if (spNo == 5)
        {
            spName = "BetsSelBetListMatchWinLoss_SAg";
        }
        return spName;
    }
    public static MemberBetLists2 GetReport(string username, string fromdate, string todate, string language, int spNo, string matchId, int Bettype)
    {
        MemberBetLists2 report = new MemberBetLists2();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate, tdate;
        if (!Validator.ValidDateMMDDYYYY(fromdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(todate, out tdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        Hashtable param = new Hashtable();
        param.Add("username", username);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        param.Add("language", language);
        param.Add("getolddata", 1);
        param.Add("matchid", matchId);
        param.Add("bettype", Bettype);
        int dbType;
        string spName = GetStoreProcedureName(spNo, out dbType);
          using (IDataReader dr = DataServices.ExecuteReader(spName, param, dbType))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MemberBetListsRow2> reportRow = new List<MemberBetListsRow2>();
                MemberBetListsRow2 totalRow = new MemberBetListsRow2();
                totalRow.MemberId = "Total";
                totalRow.MemberWinLoss = 0;
                totalRow.MemberCommission = 0;
                int bettype;
                string betteam = "";
                bool isLive = false;
                while (dr.Read())
                {
                    MemberBetListsRow2 row = new MemberBetListsRow2();
                        row.MemberId = dr["username"].ToString();
                        row.Match.League = dr["typenamee"].ToString();
                    row.TransactionTime = dr["transdate"].ToString();
                    bettype = FormatHandler.GetInt(dr["bettype"]);
                    betteam = dr["betteam"].ToString();
                    row.Match.RefNo = dr["refno"].ToString();
                    row.Match.Home = dr["home"].ToString();
                    row.Match.Away = dr["away"].ToString();
                    //row.Match.Bettype = dr["typenamee2"].ToString();
                    row.Odds = FormatHandler.GetDouble(dr["odds"],2);
                    row.Stake = FormatHandler.GetDouble(dr["stake"],2);
                        row.MemberWinLoss = FormatHandler.GetDouble(dr["winlost"]);
                        row.MemberCommission = FormatHandler.GetDouble(dr["playercomm"],2);
                    row.AgentPosition = FormatHandler.GetDouble(dr["apositiontaking"],2) * 100;
                    row.AgentPosition = FormatHandler.GetDouble(row.AgentPosition, 2);
                    if (DBNull.Value == dr["status"])
                    {
                        row.Status = "-";
                    }
                    else
                    {
                        row.Status = dr["status"].ToString();
                    }
                    if (bettype == 1 || bettype == 7)
                    {
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }

                        if (betteam == "h")
                        {
                            row.Choice = dr["home"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp2"]) - FormatHandler.GetDouble(dr["hdp1"]));
                        }
                        else
                        {
                            row.Choice = dr["away"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp1"]) - FormatHandler.GetDouble(dr["hdp2"]));
                        }
                    }
                    else if (bettype == 2)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Odd";
                        }
                        else
                        {
                            row.Choice = "Even";
                        }
                        row.Handicap = "0";
                    }
                    else if (bettype == 3 || bettype == 8)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Over";
                        }
                        else
                        {
                            row.Choice = "Under";
                        }
                        row.Handicap = dr["hdp1"].ToString();
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }
                    }
                    else if (bettype == 4)
                    {
                        row.Choice = "Correct Score";
                        row.Handicap = dr["hdp1"].ToString() + ":" + dr["hdp2"].ToString();
                    }
                    else if (bettype == 6 || bettype == 5 || bettype == 16)
                    {
                        row.Choice = betteam;
                    }
                    else if (bettype == 10)
                    {
                        row.Choice = dr["home"].ToString();
                        row.Match.Away = "";
                        row.Match.Home = "";
                        row.Match.League = dr["typenamee"].ToString();
                        row.Handicap = "0";
                    }
                    else if (bettype == 9)
                    {
                        try
                        {
                            row.Matchs = MemberBetLists.GetMixParlayReport(dr["refno"].ToString(), language);
                        }
                        catch
                        { 
                            report.ErrorCode = IbcError.INTERNAL_ERROR;
                            report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                        }

                    }// end bettype = 9
                    reportRow.Add(row);
                    totalRow.MemberWinLoss += row.MemberWinLoss;
                    totalRow.MemberCommission += row.MemberCommission;

                    totalRow.MemberWinLoss = FormatHandler.GetDouble(totalRow.MemberWinLoss,2);
                    totalRow.MemberCommission = FormatHandler.GetDouble(totalRow.MemberCommission,2);
                }//end while
                reportRow.Add(totalRow);
                report.Rows = reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }// end if(dr == null)
        }// end using
        return report;
    }
}


public class MemberBetListsRow3 : MemberBetListsRow
{
    public MemberBetListsRow3()
    { 
    
    }
    public double MemberWinLoss;
    public double MemberCommission;
    public double MasterPosition;
}

public class MemberBetLists3 : IbcError
{
    public MemberBetListsRow3[] Rows;
    public static MemberBetLists3 GetReport(string username, string fromdate, string todate, string language, int spNo, string matchId, int Bettype)
    {
        MemberBetLists3 report = new MemberBetLists3();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate, tdate;
        if (!Validator.ValidDateMMDDYYYY(fromdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(todate, out tdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        Hashtable param = new Hashtable();
        param.Add("username", username);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        param.Add("language", language);
        param.Add("matchid", matchId);
        param.Add("bettype", Bettype);
        int dbType;
        string spName = MemberBetLists2.GetStoreProcedureName(spNo, out dbType);
        using (IDataReader dr = DataServices.ExecuteReader(spName, param, dbType))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MemberBetListsRow3> reportRow = new List<MemberBetListsRow3>();
                MemberBetListsRow3 totalRow = new MemberBetListsRow3();
                totalRow.MemberId = "Total";
                totalRow.MemberWinLoss = 0;
                totalRow.MemberCommission = 0;
                int bettype;
                string betteam = "";
                bool isLive = false;
                while (dr.Read())
                {
                    MemberBetListsRow3 row = new MemberBetListsRow3();
                    row.MemberId = username;
                    if (spNo == 4)
                    {
                        row.MemberId = dr["username"].ToString();
                    }
                    row.Match.League = dr["typenamee"].ToString();

                    row.TransactionTime = dr["transdate"].ToString();
                    bettype = FormatHandler.GetInt(dr["bettype"]);
                    betteam = dr["betteam"].ToString();
                    row.Match.RefNo = dr["refno"].ToString();
                    row.Match.Home = dr["home"].ToString();
                    row.Match.Away = dr["away"].ToString();
                    //row.Match.Bettype = dr["typenamee2"].ToString();
                    row.Odds = FormatHandler.GetDouble(dr["odds"],3);
                    row.Stake = FormatHandler.GetDouble(dr["stake"],2);

                    row.AgentPosition = FormatHandler.GetDouble(dr["apositiontaking"]) * 100;
                    row.AgentPosition = FormatHandler.GetDouble(row.AgentPosition,2);

                    row.MemberWinLoss = FormatHandler.GetDouble(dr["winlost"],2);
                    row.MemberCommission = FormatHandler.GetDouble(dr["playercomm"],2);
                    row.MasterPosition = FormatHandler.GetDouble(dr["mpositiontaking"]) * 100;

                    row.MasterPosition =  FormatHandler.GetDouble( row.MasterPosition,2);

                    if (DBNull.Value == dr["status"])
                    {
                        row.Status = "-";
                    }
                    else
                    {
                        row.Status = dr["status"].ToString();
                    }
                    if (bettype == 1 || bettype == 7)
                    {
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }

                        if (betteam == "h")
                        {
                            row.Choice = dr["home"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp2"]) - FormatHandler.GetDouble(dr["hdp1"]));
                        }
                        else
                        {
                            row.Choice = dr["away"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp1"]) - FormatHandler.GetDouble(dr["hdp2"]));
                        }
                    }
                    else if (bettype == 2)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Odd";
                        }
                        else
                        {
                            row.Choice = "Even";
                        }
                        row.Handicap = "0";
                    }
                    else if (bettype == 3 || bettype == 8)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Over";
                        }
                        else
                        {
                            row.Choice = "Under";
                        }
                        row.Handicap = dr["hdp1"].ToString();
                        isLive = FormatHandler.GetBool(dr["liveindicator"]);
                        if (isLive)
                        {
                            row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                        }
                    }
                    else if (bettype == 4)
                    {
                        row.Choice = "Correct Score";
                        row.Handicap = dr["hdp1"].ToString() + ":" + dr["hdp2"].ToString();
                    }
                    else if (bettype == 6 || bettype == 5 || bettype == 16)
                    {
                        row.Choice = betteam;
                    }
                    else if (bettype == 10)
                    {
                        row.Choice = dr["home"].ToString();
                        row.Match.Away = "";
                        row.Match.Home = "";
                        row.Match.League = dr["typenamee"].ToString();
                        row.Handicap = "0";
                    }
                    else if (bettype == 9)
                    {
                        try
                        {
                            row.Matchs = MemberBetLists.GetMixParlayReport(dr["refno"].ToString(), language);
                        }
                        catch
                        {
                            report.ErrorCode = IbcError.INTERNAL_ERROR;
                            report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                        }

                    }// end bettype = 9
                    reportRow.Add(row);
                    totalRow.MemberWinLoss += row.MemberWinLoss;
                    totalRow.MemberCommission += row.MemberCommission;

                    totalRow.MemberWinLoss = FormatHandler.GetDouble(totalRow.MemberWinLoss,2);
                    totalRow.MemberCommission = FormatHandler.GetDouble(totalRow.MemberCommission,2);

                }//end while
                reportRow.Add(totalRow);
                report.Rows = reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }// end if(dr == null)
        }// end using
        return report;
    }
}

public class MemberBetListsRow4 : MemberBetListsRow
{
    public MemberBetListsRow4()
    { }
    public double MemberWinLoss;
    public double MemberCommission;
    public double MasterPosition;
    public double SuperPosition;
}

public class MemberBetLists4:IbcError
{
    public MemberBetListsRow4[] Rows;
    public static MemberBetLists4 GetReport(string username, string fromdate, string todate, string language, int spNo, string matchId, int Bettype)
    {
        MemberBetLists4 report = new MemberBetLists4();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate, tdate;
        if (!Validator.ValidDateMMDDYYYY(fromdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(todate, out tdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        Hashtable param = new Hashtable();
        param.Add("username", username);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        param.Add("language", language);
        param.Add("matchid", matchId);
        param.Add("bettype", Bettype);
        int dbType;
        string spName = MemberBetLists2.GetStoreProcedureName(spNo, out dbType);

        try
        {
            using (IDataReader dr = DataServices.ExecuteReader(spName, param, dbType))
            {
                if (dr == null)
                {
                    report.ErrorCode = IbcError.INTERNAL_ERROR;
                    report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                }
                else
                {
                    List<MemberBetListsRow4> reportRow = new List<MemberBetListsRow4>();
                    MemberBetListsRow4 totalRow = new MemberBetListsRow4();
                    totalRow.MemberId = "Total";
                    totalRow.MemberWinLoss = 0;
                    totalRow.MemberCommission = 0;
                    int bettype;
                    string betteam = "";
                    bool isLive = false;
                    while (dr.Read())
                    {
                        MemberBetListsRow4 row = new MemberBetListsRow4();
                        row.MemberId = dr["username"].ToString();
                        row.Match.League = dr["typenamee"].ToString();
                        row.TransactionTime = dr["transdate"].ToString();
                        bettype = FormatHandler.GetInt(dr["bettype"]);
                        betteam = dr["betteam"].ToString();
                        row.Match.RefNo = dr["refno"].ToString();
                        row.Match.Home = dr["home"].ToString();
                        row.Match.Away = dr["away"].ToString();
                        //row.Match.Bettype = dr["typenamee2"].ToString();
                        row.Odds = FormatHandler.GetDouble(dr["odds"],2);
                        row.Stake = FormatHandler.GetDouble(dr["stake"],2);
                        row.AgentPosition = FormatHandler.GetDouble(dr["apositiontaking"]) * 100;
                        row.MemberWinLoss = FormatHandler.GetDouble(dr["winlost"]);
                        row.MemberCommission = FormatHandler.GetDouble(dr["playercomm"]);
                        row.MasterPosition = FormatHandler.GetDouble(dr["mpositiontaking"]) * 100;
                        row.SuperPosition = FormatHandler.GetDouble(dr["spositiontaking"]) * 100;

                        row.AgentPosition = FormatHandler.GetDouble( row.AgentPosition,2);
                        row.MemberWinLoss = FormatHandler.GetDouble( row.MemberWinLoss,2);
                        row.MemberCommission = FormatHandler.GetDouble( row.MemberCommission,2);
                        row.MasterPosition = FormatHandler.GetDouble( row.MasterPosition,2);
                        row.SuperPosition = FormatHandler.GetDouble( row.SuperPosition,2);

                        if (DBNull.Value == dr["status"])
                        {
                            row.Status = "-";
                        }
                        else
                        {
                            row.Status = dr["status"].ToString();
                        }
                        if (bettype == 1 || bettype == 7)
                        {
                            isLive = FormatHandler.GetBool(dr["liveindicator"]);
                            if (isLive)
                            {
                                row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                            }

                            if (betteam == "h")
                            {
                                row.Choice = dr["home"].ToString();
                                row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp2"]) - FormatHandler.GetDouble(dr["hdp1"]));
                            }
                            else
                            {
                                row.Choice = dr["away"].ToString();
                                row.Handicap = Convert.ToString(FormatHandler.GetDouble(dr["hdp1"]) - FormatHandler.GetDouble(dr["hdp2"]));
                            }
                        }
                        else if (bettype == 2)
                        {
                            if (betteam == "h")
                            {
                                row.Choice = "Odd";
                            }
                            else
                            {
                                row.Choice = "Even";
                            }
                            row.Handicap = "0";
                        }
                        else if (bettype == 3 || bettype == 8)
                        {
                            if (betteam == "h")
                            {
                                row.Choice = "Over";
                            }
                            else
                            {
                                row.Choice = "Under";
                            }
                            row.Handicap = dr["hdp1"].ToString();
                            isLive = FormatHandler.GetBool(dr["liveindicator"]);
                            if (isLive)
                            {
                                row.LiveScore = dr["livehomescore"].ToString() + "-" + dr["liveawayscore"].ToString();
                            }
                        }
                        else if (bettype == 4)
                        {
                            row.Choice = "Correct Score";
                            row.Handicap = dr["hdp1"].ToString() + ":" + dr["hdp2"].ToString();
                        }
                        else if (bettype == 6 || bettype == 5 || bettype == 16)
                        {
                            row.Choice = betteam;
                        }
                        else if (bettype == 10)
                        {
                            row.Choice = dr["home"].ToString();
                            row.Match.Away = "";
                            row.Match.Home = "";
                            row.Match.League = dr["typenamee"].ToString();
                            row.Handicap = "0";
                        }
                        else if (bettype == 9)
                        {
                            try
                            {
                                row.Matchs = MemberBetLists.GetMixParlayReport(dr["refno"].ToString(), language);
                            }
                            catch
                            {
                                report.ErrorCode = IbcError.INTERNAL_ERROR;
                                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                            }

                        }// end bettype = 9
                        reportRow.Add(row);
                        totalRow.MemberWinLoss += row.MemberWinLoss;
                        totalRow.MemberCommission += row.MemberCommission;

                        totalRow.MemberWinLoss = FormatHandler.GetDouble( totalRow.MemberWinLoss,2);
                        totalRow.MemberCommission = FormatHandler.GetDouble( totalRow.MemberCommission,2);
                    }//end while
                    reportRow.Add(totalRow);
                    report.Rows = reportRow.ToArray();
                    report.ErrorCode = IbcError.SUCCESS;
                    report.ErrorMsg = IbcError.SUCCESS_MSG;
                }// end if(dr == null)
            }// end using
        }
        catch
        {
            report.ErrorCode = IbcError.INTERNAL_ERROR;
            report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;        
        }

        return report;
    }
}