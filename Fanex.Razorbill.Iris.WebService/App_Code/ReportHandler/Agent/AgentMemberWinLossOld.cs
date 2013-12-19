using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using IBCUtils;
using IBCDLL.DAL;
using IBCDLL.AgentReport;

/// <summary>
/// Summary description for MemberWinLossOld
/// </summary>
public class MemberWinLossOldRow
{
    public string MemberId;
    public double Turnover;
    public double GrossCommission;
    public double MemberWinLoss;
    public double MemberCommission;
    public double MemberTotal;
    public double AgentWinLoss;
    public double AgentCommission;
    public double AgentTotal;
}
public class AgentMemberWinLossOld : IbcError
{
    public MemberWinLossOldRow[] Rows;
    public static AgentMemberWinLossOld GetReport(string username, string fromdate, string todate)
    {
        AgentMemberWinLossOld report = new AgentMemberWinLossOld();
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

        try
        {
            using (IDataReader dr = DataServices.ExecuteReader("Rpt_MemberWinlostDetail_Old", param, DatabaseType.WAREHOUSE))
            {
                if (dr == null)
                {
                    report.ErrorCode = IbcError.INTERNAL_ERROR;
                    report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                }
                else
                {
                    List<MemberWinLossOldRow> reportRows = new List<MemberWinLossOldRow>();
                    MemberWinLossOldRow totalRow = new MemberWinLossOldRow();
                    totalRow.MemberId = "Total";
                    totalRow.Turnover = totalRow.GrossCommission = totalRow.MemberWinLoss = totalRow.MemberCommission
                        = totalRow.MemberTotal = totalRow.AgentWinLoss = totalRow.AgentCommission = totalRow.AgentTotal = 0;
                    while (dr.Read())
                    {
                        MemberWinLossOldRow row = new MemberWinLossOldRow();
                        row.MemberId = dr["username"].ToString();
                        row.Turnover = FormatHandler.GetDouble(dr["turnover"],2);
                        row.GrossCommission = FormatHandler.GetDouble(dr["GrossComm"],2);
                        row.MemberWinLoss = FormatHandler.GetDouble(dr["Memberwin"],2);
                        row.MemberCommission = FormatHandler.GetDouble(dr["Membercomm"],2);
                        row.MemberTotal = FormatHandler.GetDouble(dr["MemberTotal"],2);
                        row.AgentWinLoss = FormatHandler.GetDouble(dr["AgentWin"],2);
                        row.AgentCommission = FormatHandler.GetDouble(dr["AgentComm"],2);
                        row.AgentTotal = FormatHandler.GetDouble(dr["AgentTotal"],2);

                        reportRows.Add(row);

                        totalRow.Turnover += row.Turnover;
                        totalRow.GrossCommission += row.GrossCommission;
                        totalRow.MemberWinLoss += row.MemberWinLoss;
                        totalRow.MemberCommission += row.MemberCommission;
                        totalRow.MemberTotal += row.MemberTotal;
                        totalRow.AgentWinLoss += row.AgentWinLoss;
                        totalRow.AgentCommission += row.AgentCommission;
                        totalRow.AgentTotal += row.AgentTotal;

                        totalRow.Turnover = FormatHandler.GetDouble(totalRow.Turnover,2);
                        totalRow.GrossCommission = FormatHandler.GetDouble(totalRow.GrossCommission,2);
                        totalRow.MemberWinLoss =FormatHandler.GetDouble( totalRow.MemberWinLoss,2);
                        totalRow.MemberCommission =FormatHandler.GetDouble( totalRow.MemberCommission,2);
                        totalRow.MemberTotal = FormatHandler.GetDouble(totalRow.MemberTotal,2);
                        totalRow.AgentWinLoss = FormatHandler.GetDouble(totalRow.AgentWinLoss,2);
                        totalRow.AgentCommission = FormatHandler.GetDouble(totalRow.AgentCommission,2);
                        totalRow.AgentTotal = FormatHandler.GetDouble(totalRow.AgentTotal, 2);
                    }
                    reportRows.Add(totalRow);
                    report.Rows = reportRows.ToArray();
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
