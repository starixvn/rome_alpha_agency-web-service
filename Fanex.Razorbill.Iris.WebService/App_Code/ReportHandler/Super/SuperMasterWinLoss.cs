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


public class MasterWinLossRow
{
    public string MasterId;
    public double Turnover = 0;
    public double MasterTotal = 0;
    public double SuperWinloss = 0;
    public double SuperCommission = 0;
    public double SuperTotal = 0;
    public double CompanyTotal = 0;
}
public class SuperMasterWinLoss: IbcError
{
    public MasterWinLossRow[] Rows;
    public static SuperMasterWinLoss GetReport(int custId, string fromdate, string todate)
    {
        SuperMasterWinLoss report = new SuperMasterWinLoss();
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
        param.Add("custid", custId);
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        using (IDataReader dr = DataServices.ExecuteReader("Rpt_MAgentWinlost", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MasterWinLossRow> reportRows = new List<MasterWinLossRow>();
                MasterWinLossRow totalRow = new MasterWinLossRow();
                totalRow.MasterId = "Total";
                while (dr.Read())
                {
                    MasterWinLossRow row = new MasterWinLossRow();
                    
                    row.MasterId = dr["username"].ToString();
                    row.Turnover = FormatHandler.GetDouble(dr["turnover"], 2);
                    row.MasterTotal = FormatHandler.GetDouble(dr["MasterTotal"], 2);
                    row.SuperWinloss = FormatHandler.GetDouble(dr["SAgWinlost"], 2);
                    row.SuperCommission = FormatHandler.GetDouble(dr["SComm"], 2);
                    row.SuperTotal = FormatHandler.GetDouble(dr["SAgTotal"], 2);
                    row.CompanyTotal = FormatHandler.GetDouble(dr["Company"], 2);

                    reportRows.Add(row);

                    totalRow.Turnover += row.Turnover;
                    totalRow.MasterTotal += row.MasterTotal;
                    totalRow.SuperWinloss += row.SuperWinloss;
                    totalRow.SuperCommission += row.SuperCommission;
                    totalRow.SuperTotal += row.SuperTotal;
                    totalRow.CompanyTotal += row.CompanyTotal;

                    totalRow.Turnover = FormatHandler.GetDouble(totalRow.Turnover,2);
                    totalRow.MasterTotal = FormatHandler.GetDouble(totalRow.MasterTotal,2);
                    totalRow.SuperWinloss = FormatHandler.GetDouble(totalRow.SuperWinloss,2);
                    totalRow.SuperCommission = FormatHandler.GetDouble(totalRow.SuperCommission,2);
                    totalRow.SuperTotal = FormatHandler.GetDouble(totalRow.SuperTotal,2);
                    totalRow.CompanyTotal = FormatHandler.GetDouble(totalRow.CompanyTotal,2);

                }
                reportRows.Add(totalRow);
                report.Rows = reportRows.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }
        }
        return report;
    }
}