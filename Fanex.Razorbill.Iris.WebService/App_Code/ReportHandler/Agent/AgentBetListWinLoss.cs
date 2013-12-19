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
/// Summary description for AgentBetList
/// </summary>
public class AgentBetListWinLossRow
{
    public string MemberId;
    public long BetId;
    public string TransTime;
    public string BetType;
    public string Sport;
    public int GameId;
    public string Home;
    public string Away;
    public string League;
    public string Choice;
    public string Handicap;
    public string OddsType;
    public double Odds;
    public double Stake;
    public int LiveAScore;
    public int LiveBScore;
    //public string MatchDate;

    public float MemWL;
    public float MemComm;
    public float AgentWL;
    public float AgentComm;
}
public class AgentBetListWinLoss:IbcError
{
    public AgentBetListWinLossRow[] Rows;
    public static AgentBetListWinLoss GetReports(string date, int agentId)
    {
        AgentBetListWinLoss report = new AgentBetListWinLoss();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate;
        if (!Validator.ValidDateMMDDYYYY(date, out fdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        Hashtable param = new Hashtable();
        param.Add("custid", agentId);
        param.Add("fromdate", fdate);
        param.Add("todate", fdate);
        param.Add("language", "E");
        param.Add("movebetsday", common.MoveBetDate);
        try
        {
            using (IDataReader dr = DataServices.ExecuteReader("Age_BetList_SelWinLossAll_API", param, DatabaseType.ARCHIVE))
            {
                if (dr == null)
                {
                    report.ErrorCode = IbcError.INTERNAL_ERROR;
                    report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                }
                else
                {
                    List<AgentBetListWinLossRow> reportRow = new List<AgentBetListWinLossRow>();
                    while (dr.Read())
                    {
                        AgentBetListWinLossRow row = new AgentBetListWinLossRow();
                        row.MemberId = dr["username"].ToString();
                        row.BetId = (dr["transid"] == null ? 0 : (long)(dr["transid"]));
                        row.TransTime = dr["transdate"].ToString();
                        row.BetType = CacheRef.GetBettype(dr["Bettype"]);
                        row.Sport = CacheRef.GetSportName(dr["Sporttype"]);
                        row.GameId = FormatHandler.GetInt(dr["matchid"]);

                        row.Home = CacheRef.GetTeamName(dr["HomeID"]);
                        row.Away = CacheRef.GetTeamName(dr["AwayID"]);

                        row.League = CacheRef.GetLeagueName(dr["LeagueID"]);
                        //row.MatchDate = IBCUtils.FormatHandler.FormatDateTimeEn24(dr["kickofftime"]);

                        #region bettype
                        int bettype = FormatHandler.GetInt(dr["bettype"]);
                        double hdp1 = FormatHandler.GetDouble(dr["hdp1"], 2);
                        double hdp2 = FormatHandler.GetDouble(dr["hdp2"], 2);
                        double hdp = 0;
                        string betteam = dr["betteam"].ToString();
                        switch (bettype)
                        {
                            case 1:
                            case 7:
                                if (betteam == "h")
                                {
                                    row.Choice = row.Home;
                                    hdp = hdp2 - hdp1;
                                    row.Handicap = FormatHandler.GetDouble(hdp, 2).ToString();
                                }
                                else
                                {
                                    row.Choice = row.Away;
                                    hdp = hdp1 - hdp2;
                                    row.Handicap = FormatHandler.GetDouble(hdp, 2).ToString();
                                }
                                break;
                            case 2:
                                if (betteam == "h")
                                {
                                    row.Choice = "Odd";
                                    row.Handicap = "-";
                                }
                                else
                                {
                                    row.Choice = "Even";
                                    row.Handicap = "-";
                                }
                                break;
                            case 3:
                            case 8:
                                if (betteam == "h")
                                {
                                    row.Choice = "Over";
                                    hdp = hdp1 - hdp2;
                                    row.Handicap = FormatHandler.GetDouble(hdp, 2).ToString();
                                }
                                else
                                {
                                    row.Choice = "Under";
                                    row.Handicap = FormatHandler.GetDouble(hdp1, 2).ToString();
                                }
                                break;
                            case 4:
                                row.Choice = "Correct Score";
                                row.Handicap = hdp1.ToString() + "-" + hdp2.ToString();
                                break;
                            case 6:
                            case 16:
                            case 5:
                            case 15:
                                row.Choice = betteam;
                                row.Handicap = "-";
                                break;
                            case 10:
                                row.Choice = row.Home;
                                row.Handicap = "-";
                                break;
                            case 9:
                                row.Choice = "-";
                                row.Handicap = "-";
                                break;
                        }
                        #endregion

			row.OddsType =CacheRef.GetOddsTypeName(dr["OddsType"]);
                        row.Odds = FormatHandler.GetDouble(dr["Odds"], 3);
                        row.Stake = FormatHandler.GetDouble(dr["Stake"], 2);
                        if (FormatHandler.GetBool(dr["liveindicator"]))
                        {
                            row.LiveAScore = FormatHandler.GetInt(dr["livehomescore"]);
                            row.LiveBScore = FormatHandler.GetInt(dr["liveawayscore"]);
                        }
                        else
                        {
                            row.LiveAScore = -1;
                            row.LiveBScore = -1;
                        }
                        row.MemWL = FormatHandler.GetFloat(dr["Winlost"]);
                        row.MemComm = FormatHandler.GetFloat(dr["Comm"]);
                        row.AgentWL = FormatHandler.GetFloat(dr["AWinlost"]);
                        row.AgentComm = FormatHandler.GetFloat(dr["AComm"]);
                        reportRow.Add(row);
                    }

                    report.Rows = reportRow.ToArray();
                    report.ErrorCode = IbcError.SUCCESS;
                    report.ErrorMsg = IbcError.SUCCESS_MSG;
                }
            }
        }
        catch
        {
            report.ErrorCode = IbcError.INTERNAL_ERROR;
            report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
        }
        return report;
    }
}
