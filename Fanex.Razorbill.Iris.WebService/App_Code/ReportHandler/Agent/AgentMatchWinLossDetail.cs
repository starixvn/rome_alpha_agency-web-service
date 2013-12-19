using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
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

/// <summary>
/// Summary description for MatchWinLossDetail
/// </summary>
public class AgentMatchWinLossDetailRow
{
    public string Total;
    public int BetTypeId;
    public string BetType;
    public int MatchId;
    public string Home;
    public string Away;
    public string LeagueName;    
    public double Turnover = 0;
    public double GrossCommission = 0;
    public double MemberWinLoss = 0;
    public double MemberCommission = 0;
    public double MemberTotal = 0;
    public double AgentWinLoss = 0;
    public double AgentCommission = 0;
    public double AgentTotal = 0;
    public double CompanyCommission = 0;
    public double CompanyTotal = 0;
}

public class AgentMatchWinLossDetail:IbcError
{
    public AgentMatchWinLossDetailRow[] Rows;    
    public static AgentMatchWinLossDetail GetReport(int custId, string fromdate, string todate, string lang)
    {
        double Turnover = 0, GrossCommission = 0, MemberWinLoss = 0, MemberCommission = 0, MemberTotal = 0, AgentWinLoss = 0, AgentCommission = 0, AgentTotal = 0, CompanyCommission = 0, CompanyTotal = 0;
        AgentMatchWinLossDetail report = new AgentMatchWinLossDetail();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate, tdate;
        if (!Validator.ValidDateMMDDYYYY(fromdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(todate, out tdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        if (string.IsNullOrEmpty(lang)) lang = "E";
        Hashtable param = new Hashtable();
        param.Add("custid", custId);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        param.Add("language", lang);
        using (IDataReader dr = DataServices.ExecuteReader("Rpt_MatchWinLostDetail_Ag", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<AgentMatchWinLossDetailRow> reportRows = new List<AgentMatchWinLossDetailRow>();
                AgentMatchWinLossDetailRow totalRow1 = new AgentMatchWinLossDetailRow();
                AgentMatchWinLossDetailRow totalRow2 = new AgentMatchWinLossDetailRow();
                
                while (dr.Read())
                {
                    AgentMatchWinLossDetailRow row = new AgentMatchWinLossDetailRow();
                    if (!dr["BetGroup"].Equals(1))
                    {
                        row.BetTypeId = FormatHandler.GetInt(dr["bettype"]);
                        row.BetType = dr["typenamee"].ToString();
                        row.MatchId = FormatHandler.GetInt(dr["MatchId"]);
                        row.Home = dr["home"].ToString();
                        row.Away = dr["away"].ToString();
                        row.LeagueName = dr["leaguename"].ToString();
                        row.Turnover = FormatHandler.GetDouble(dr["turnover"],2);
                        row.GrossCommission = FormatHandler.GetDouble(dr["GrossComm"],2);
                        row.MemberWinLoss = FormatHandler.GetDouble(dr["MemberWinLost"],2);
                        row.MemberCommission = FormatHandler.GetDouble(dr["MemberComm"],2);
                        row.MemberTotal = FormatHandler.GetDouble(dr["MemberTotal"],2);
                        row.AgentWinLoss = FormatHandler.GetDouble(dr["AgentWinLost"],2);
                        row.AgentCommission = FormatHandler.GetDouble(dr["AgentComm"],2);
                        row.AgentTotal = FormatHandler.GetDouble(dr["AgentTotal"],2);
                        row.CompanyCommission = FormatHandler.GetDouble(dr["CompanyComm"],2);
                        row.CompanyTotal = FormatHandler.GetDouble(dr["CompanyTotal"],2);
                        reportRows.Add(row);

                        totalRow1.Turnover += row.Turnover; Turnover += row.Turnover;
                        totalRow1.GrossCommission += row.GrossCommission; GrossCommission += row.GrossCommission;
                        totalRow1.MemberWinLoss += row.MemberWinLoss; MemberWinLoss += row.MemberWinLoss;
                        totalRow1.MemberCommission += row.MemberCommission; MemberCommission += row.MemberCommission;
                        totalRow1.MemberTotal += row.MemberTotal; MemberTotal += row.MemberTotal;
                        totalRow1.AgentWinLoss += row.AgentWinLoss; AgentWinLoss += row.AgentWinLoss;
                        totalRow1.AgentCommission += row.AgentCommission; AgentCommission += row.AgentCommission;
                        totalRow1.AgentTotal += row.AgentTotal; AgentTotal += row.AgentTotal;
                        totalRow1.CompanyCommission += row.CompanyCommission; CompanyCommission += row.CompanyCommission;
                        totalRow1.CompanyTotal += row.CompanyTotal; CompanyTotal += row.CompanyTotal;

                        totalRow1.Turnover = FormatHandler.GetDouble(totalRow1.Turnover,2);
                        totalRow1.GrossCommission = FormatHandler.GetDouble(totalRow1.GrossCommission,2);
                        totalRow1.MemberWinLoss = FormatHandler.GetDouble(totalRow1.MemberWinLoss,2);
                        totalRow1.MemberCommission = FormatHandler.GetDouble(totalRow1.MemberCommission,2);
                        totalRow1.MemberTotal = FormatHandler.GetDouble(totalRow1.MemberTotal,2);
                        totalRow1.AgentWinLoss = FormatHandler.GetDouble(totalRow1.AgentWinLoss,2);
                        totalRow1.AgentCommission = FormatHandler.GetDouble(totalRow1.AgentCommission,2);
                        totalRow1.AgentTotal = FormatHandler.GetDouble(totalRow1.AgentTotal,2);
                        totalRow1.CompanyCommission = FormatHandler.GetDouble(totalRow1.CompanyCommission,2);
                        totalRow1.CompanyTotal = FormatHandler.GetDouble(totalRow1.CompanyTotal, 2);
                    }
                    else//Mix Parlay
                    {
                        totalRow1.Total = "Total";
                        reportRows.Add(totalRow1);

                        row.BetType = Convert.ToString(dr["typenamee"]);
                        row.Turnover = FormatHandler.GetDouble(dr["turnover"],2); Turnover += FormatHandler.GetDouble(dr["turnover"]);
                        row.GrossCommission = FormatHandler.GetDouble(dr["GrossComm"],2); GrossCommission += FormatHandler.GetDouble(dr["GrossComm"]);
                        row.MemberWinLoss = FormatHandler.GetDouble(dr["MemberWinLost"],2); MemberWinLoss += FormatHandler.GetDouble(dr["MemberWinLost"]);
                        row.MemberCommission = FormatHandler.GetDouble(dr["MemberComm"],2); MemberCommission += FormatHandler.GetDouble(dr["MemberComm"]);
                        row.MemberTotal = FormatHandler.GetDouble(dr["MemberTotal"],2); MemberTotal += FormatHandler.GetDouble(dr["MemberTotal"]);
                        row.AgentWinLoss = FormatHandler.GetDouble(dr["AgentWinLost"],2); AgentWinLoss += FormatHandler.GetDouble(dr["AgentWinLost"]);
                        row.AgentCommission = FormatHandler.GetDouble(dr["AgentComm"],2); AgentCommission += FormatHandler.GetDouble(dr["AgentComm"]);
                        row.AgentTotal = FormatHandler.GetDouble(dr["AgentTotal"],2); AgentTotal += FormatHandler.GetDouble(dr["AgentTotal"]);
                        row.CompanyCommission = FormatHandler.GetDouble(dr["CompanyComm"],2); CompanyCommission += FormatHandler.GetDouble(dr["CompanyComm"]);
                        row.CompanyTotal = FormatHandler.GetDouble(dr["CompanyTotal"],2); CompanyTotal += FormatHandler.GetDouble(dr["CompanyTotal"]);
                        row.Home = "";
                        row.Away = "";
                        row.LeagueName = "";

                        reportRows.Add(row);

                        totalRow2.Turnover = totalRow1.Turnover + row.Turnover;
                        totalRow2.GrossCommission = totalRow1.GrossCommission + row.GrossCommission;
                        totalRow2.MemberWinLoss = totalRow1.MemberWinLoss + row.MemberWinLoss;
                        totalRow2.MemberCommission = totalRow1.MemberCommission + row.MemberCommission;
                        totalRow2.MemberTotal = totalRow1.MemberTotal + row.MemberTotal;
                        totalRow2.AgentWinLoss = totalRow1.AgentWinLoss + row.AgentWinLoss;
                        totalRow2.AgentCommission = totalRow1.AgentCommission + row.AgentCommission;
                        totalRow2.AgentTotal = totalRow1.AgentTotal + row.AgentTotal;
                        totalRow2.CompanyCommission = totalRow1.CompanyCommission + row.CompanyCommission;
                        totalRow2.CompanyTotal = totalRow1.CompanyTotal + row.CompanyTotal;

                        totalRow2.Turnover = FormatHandler.GetDouble( totalRow2.Turnover,2);
                        totalRow2.GrossCommission =  FormatHandler.GetDouble(totalRow2.GrossCommission,2);
                        totalRow2.MemberWinLoss =  FormatHandler.GetDouble(totalRow2.MemberWinLoss,2);
                        totalRow2.MemberCommission =  FormatHandler.GetDouble(totalRow2.MemberCommission,2);
                        totalRow2.MemberTotal = FormatHandler.GetDouble( totalRow2.MemberTotal,2);
                        totalRow2.AgentWinLoss =  FormatHandler.GetDouble(totalRow2.AgentWinLoss,2);
                        totalRow2.AgentCommission = FormatHandler.GetDouble( totalRow2.AgentCommission,2);
                        totalRow2.AgentTotal = FormatHandler.GetDouble( totalRow2.AgentTotal,2);
                        totalRow2.CompanyCommission = FormatHandler.GetDouble( totalRow2.CompanyCommission,2);
                        totalRow2.CompanyTotal = FormatHandler.GetDouble(totalRow2.CompanyTotal, 2);

                        totalRow2.Total = "Total";
                        reportRows.Add(totalRow2);
                    }
                }
                report.Rows = reportRows.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }
        }
        return report;
    }
    //static double getvalue(object value)
    //{
    //    if (String.IsNullOrEmpty(value.ToString()))
    //        return 0;
    //    return Convert.ToDouble(value);
    //}
}
