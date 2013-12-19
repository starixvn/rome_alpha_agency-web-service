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

public class MasterWinLossDetailRow
{
    public string MasterId;
    public double Turnover = 0;
    public double GrossCommission = 0;
    public double MemberWinLoss = 0;
    public double MemberCommission = 0;
    public double MemberTotal = 0;
    public double AgentWinLoss = 0;
    public double AgentCommission = 0;
    public double AgentTotal = 0;
    public double MasterWinLoss = 0;
    public double MasterCommission = 0;
    public double MasterTotal = 0;
    public double SuperWinLoss = 0;
    public double SuperCommission = 0;
    public double SuperTotal = 0;
    public double CompanyTotal = 0;
}
public class SuperMasterWinLossDetail : IbcError
{
    public MasterWinLossDetailRow[] Rows;

    public static SuperMasterWinLossDetail GetReport(int custId, string fromdate, string todate)
    {
        SuperMasterWinLossDetail report = new SuperMasterWinLossDetail();
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
        param.Add("srecommend", custId);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);

        using (IDataReader dr = DataServices.ExecuteReader("Rpt_MAgentWinlostDetail", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MasterWinLossDetailRow> reportRow = new List<MasterWinLossDetailRow>();
                MasterWinLossDetailRow totalRow = new MasterWinLossDetailRow();
                totalRow.MasterId = "Total";
                while (dr.Read())
                {
                    MasterWinLossDetailRow row = new MasterWinLossDetailRow();
                    row.MasterId = dr["username"].ToString();
                    row.Turnover = FormatHandler.GetDouble(dr["turnover"]);
                    row.GrossCommission = FormatHandler.GetDouble(dr["GrossComm"]);
                    row.MemberWinLoss = FormatHandler.GetDouble(dr["MemberWin"]);
                    row.MemberCommission = FormatHandler.GetDouble(dr["MemberComm"]);
                    row.MemberTotal = FormatHandler.GetDouble(dr["MemberTotal"]);
                    row.AgentWinLoss = FormatHandler.GetDouble(dr["AgentWin"]);
                    row.AgentCommission = FormatHandler.GetDouble(dr["AgentComm"]);
                    row.AgentTotal = FormatHandler.GetDouble(dr["AgentTotal"]);
                    row.MasterWinLoss = FormatHandler.GetDouble(dr["MasterWin"]);
                    row.MasterCommission = FormatHandler.GetDouble(dr["MasterComm"]);
                    row.MasterTotal = FormatHandler.GetDouble(dr["MasterTotal"]);
                    row.SuperWinLoss = FormatHandler.GetDouble(dr["SAgWin"]);
                    row.SuperCommission = FormatHandler.GetDouble(dr["SAgComm"]);
                    row.SuperTotal = FormatHandler.GetDouble(dr["SAgTotal"]);
                    row.CompanyTotal = FormatHandler.GetDouble(dr["Company"]);

                    reportRow.Add(row);

                    totalRow.MasterId = "Total";
                    totalRow.Turnover += row.Turnover;
                    totalRow.GrossCommission += row.GrossCommission;
                    totalRow.MemberWinLoss += row.MemberWinLoss;
                    totalRow.MemberCommission += row.MemberCommission;
                    totalRow.MemberTotal += row.MemberTotal;
                    totalRow.AgentWinLoss += row.AgentWinLoss;
                    totalRow.AgentCommission += row.AgentCommission;
                    totalRow.AgentTotal += row.AgentTotal;
                    totalRow.MasterWinLoss += row.MasterWinLoss;
                    totalRow.MasterCommission += row.MasterCommission;
                    totalRow.MasterTotal += row.MasterTotal;
                    totalRow.SuperWinLoss += row.SuperWinLoss;
                    totalRow.SuperCommission += row.SuperCommission;
                    totalRow.SuperTotal += row.SuperTotal;
                    totalRow.CompanyTotal += row.CompanyTotal;
                }// end while
                reportRow.Add(totalRow);
                report.Rows = reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }//  if (dr == null)
        }//end using


        return report;
    }
}