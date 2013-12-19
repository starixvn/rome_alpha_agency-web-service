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
/// Summary description for BetSlip
/// </summary>

public class BetSlipEvent
{
    public BetSlipEvent()
    { }
    public int Code;
    public string Match;
    public string Choice;
    public string Handicap;
    public double Odds;
}
public class BetSlip:IbcError
{
    public string BetId;
    public string TransTime;
    public BetSlipEvent BetDetail = new BetSlipEvent();
    public BetSlipEvent[] BetDetails;
    public double Stake;
    public string Status;
    public double PotentialWinning;
    public double Winning;
    public bool IsLive;
    public string LiveScore;
    public string ResultTime;
    public string FinalScore;
    public string Username;


    public static BetSlip GetReport(string refno, string language)
    {
        BetSlip report = new BetSlip();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        Hashtable param = new Hashtable();
        param.Add("refno", refno);
        param.Add("language", language);
        using (IDataReader dr = DataServices.ExecuteReader("BetTransSelBetSlip", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                int bettype ;
                string betteam;
                while (dr.Read())
                {

                    bettype = FormatHandler.GetInt(dr["bettype"]);
                    betteam = dr["betteam"].ToString();
                    
                    report.BetId = dr["transid"].ToString();
                    report.TransTime = dr["transdate"].ToString();
                    report.BetDetail.Code = FormatHandler.GetInt(dr["matchcode"]);
                    report.BetDetail.Match = dr["home"].ToString() + "-vs-" + dr["away"].ToString();
                    report.BetDetail.Odds = FormatHandler.GetDouble(dr["odds"],2);
                    report.Stake = FormatHandler.GetDouble(dr["stake"],2);
                    report.Status = dr["status"].ToString();
                    if (report.BetDetail.Odds > 0)
                    {
                        report.PotentialWinning = report.BetDetail.Odds * report.Stake + report.Stake;
                        report.PotentialWinning = FormatHandler.GetDouble(report.PotentialWinning,2);
                    }
                    else
                    {
                        report.PotentialWinning = -1 * report.BetDetail.Odds * report.Stake + report.Stake;
                        report.PotentialWinning = FormatHandler.GetDouble(report.PotentialWinning, 2);
                    }
                    report.Username = dr["username"] + "";
                    report.Winning = FormatHandler.GetDouble(dr["winlost"]);
                    report.IsLive = FormatHandler.GetBool(dr["liveindicator"]);
                    report.LiveScore = dr["livehomescore"].ToString() + " - " + dr["liveawayscore"].ToString();
                    report.ResultTime = FormatHandler.FormatDateTimeEn12NoSec(dr["winlostdate"]);
                    report.FinalScore = dr["finalhomescore"].ToString() + " - " + dr["finalawayscore"].ToString();
                    double hdp1, hdp2, hdp;
                    hdp2 = FormatHandler.GetDouble(dr["hdp2"]);
                    hdp1 = FormatHandler.GetDouble(dr["hdp1"]);
                    if(bettype == 1 || bettype == 7)
                    {
                        if(betteam == "h")
                        {
                            report.BetDetail.Choice = dr["home"].ToString();
                            hdp = hdp2 - hdp1;
                            report.BetDetail.Handicap = Convert.ToString(FormatHandler.GetDouble(hdp,2));
                        }
                        else
                        {
                            report.BetDetail.Choice = dr["away"].ToString();
                            hdp = hdp1 - hdp2;
                            report.BetDetail.Handicap = Convert.ToString(FormatHandler.GetDouble(hdp,2));
                        }
                       
                    }
                    else if (bettype == 2)
                    {
                        if (betteam == "h")
                        {
                            report.BetDetail.Choice = "Odd";
                        }
                        else
                        {
                            report.BetDetail.Choice = "Even";
                        }
                    }
                    else if (bettype == 3 || bettype == 8)
                    {
                        if (betteam == "h")
                        {
                            report.BetDetail.Choice = "Over";
                        }
                        else
                        {
                            report.BetDetail.Choice = "Under";
                        }
                        report.BetDetail.Handicap = FormatHandler.GetDouble(dr["hdp1"],2).ToString();
                    }
                    else if (bettype == 4)
                    {
                        report.BetDetail.Choice = "Correct Score";
                        report.BetDetail.Handicap = FormatHandler.GetDouble(dr["hdp1"],2).ToString() + "-" + FormatHandler.GetDouble(dr["hdp2"],2).ToString();
                        report.PotentialWinning = report.BetDetail.Odds * report.Stake;
                        report.PotentialWinning = FormatHandler.GetDouble(report.PotentialWinning,2);
                    }
                    else if (bettype == 5 || bettype == 6)
                    {
                        report.BetDetail.Choice = betteam;
                        report.PotentialWinning = report.BetDetail.Odds * report.Stake;
                        report.PotentialWinning = FormatHandler.GetDouble(report.PotentialWinning, 2);
                    }
                    //Now is not use
                    //else if (bettype == 9)
                    //{
                    //    report.BetDetails = GetMixParlayReport(refno, language);
                    //}
                     if (bettype == 7 || bettype == 8)
                     {
                         report.FinalScore = dr["hthomescore"].ToString() + " - " + dr["htawayscore"].ToString();
                     }
                    
                }// end while
            }// end if(dr == null)
        }// end using
        return report;
    }
    static BetSlipEvent[] GetMixParlayReport(string refno, string language)
    {
        List<BetSlipEvent> report = new List<BetSlipEvent>();
        Hashtable param = new Hashtable();
        param.Add("refno", refno);
        param.Add("language", language);
        using (IDataReader dr = DataServices.ExecuteReader("BetsSelMTeamList", param, DatabaseType.ARCHIVE))
        {
            if (dr != null)
            {
                 int bettype;
                string betteam;
                while (dr.Read())
                {
                    BetSlipEvent row = new BetSlipEvent();
                    row.Code = FormatHandler.GetInt(dr["matchcode"]);
                    row.Match = dr["home"].ToString() + "-vs-" + dr["away"].ToString();
                    row.Odds = FormatHandler.GetDouble(dr["odds"]);
                    bettype = FormatHandler.GetInt(dr["bettype"]);
                    betteam = dr["betteam"].ToString();
                    if (bettype == 1)
                    {
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
                    else if(bettype == 2)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = "Odd";
                        }
                        else
                        {
                            row.Choice = "Even";
                        }
                    }
                    else if (bettype == 3)
                    {
                        row.Handicap = dr["hdp1"].ToString();
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
                        row.Choice = dr["hdp1"].ToString() + ":" + dr["hdp2"].ToString();

                    }
                    else if (bettype == 5 || bettype == 6)
                    {
                        row.Choice = dr["betteam"].ToString();

                    }
                    else if (bettype == 7)
                    {
                        if (betteam == "h")
                        {
                            row.Choice = dr["home"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDecimal(dr["hdp2"]) - FormatHandler.GetDecimal(dr["hdp1"]));
                        }
                        else
                        {
                            row.Choice = dr["away"].ToString();
                            row.Handicap = Convert.ToString(FormatHandler.GetDecimal(dr["hdp1"]) - FormatHandler.GetDecimal(dr["hdp2"]));
                        }
                    }
                    report.Add(row);
                }// end while
            }// end if(drMP != null)
        }// end using 
        return report.ToArray() ;
    }
}