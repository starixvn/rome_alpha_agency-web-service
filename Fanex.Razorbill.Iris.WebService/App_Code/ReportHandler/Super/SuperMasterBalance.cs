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

public class MasterBalanceRow
{
    public string MasterId;
    public double MasterGivenCredit = 0;
    public double MasterUsedCredit = 0;
    public double MasterOutstanding = 0;
    public double MasterCashBalance = 0;
    public double MasterTotalBalance = 0;
    public string FirstName;
    public string LastName;
}
public class SuperMasterBalance : IbcError
{
    public MasterBalanceRow[] Rows;

    public static SuperMasterBalance GetReport(int custId, bool Closed)
    {
        SuperMasterBalance report = new SuperMasterBalance();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;        
        Hashtable param = new Hashtable();
        param.Add("sagid", custId);
        if (Closed) param.Add("closed", 1);
        try
        {
            using (IDataReader dr = DataServices.ExecuteReader("CustomerSelSAgBalance", param, DatabaseType.ARCHIVE))
            {
                if (dr == null)
                {
                    report.ErrorCode = IbcError.INTERNAL_ERROR;
                    report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
                }
                else
                {
                    List<MasterBalanceRow> reportRow = new List<MasterBalanceRow>();
                    MasterBalanceRow totalRow = new MasterBalanceRow();
                    totalRow.MasterId = "Total";
                    while (dr.Read())
                    {
                        MasterBalanceRow row = new MasterBalanceRow();

                        row.MasterId = dr["username"].ToString();
                        row.MasterGivenCredit = FormatHandler.GetDouble(dr["MAgentCredit"],2);
                        row.MasterUsedCredit = FormatHandler.GetDouble(dr["MAgentUsedCredit"],2);
                        row.MasterOutstanding = FormatHandler.GetDouble(dr["MAgentOutstanding"],2);
                        row.MasterCashBalance = FormatHandler.GetDouble(dr["MAgentCashBalance"],2);
                        row.MasterTotalBalance = FormatHandler.GetDouble(dr["MAgentBalance"],2);
                        row.FirstName = dr["FirstName"].ToString();
                        row.LastName = dr["LastName"].ToString();

                        reportRow.Add(row);

                        totalRow.MasterGivenCredit += row.MasterGivenCredit;
                        totalRow.MasterUsedCredit += row.MasterUsedCredit;
                        totalRow.MasterOutstanding += row.MasterOutstanding;
                        totalRow.MasterCashBalance += row.MasterCashBalance;
                        totalRow.MasterTotalBalance += row.MasterTotalBalance;

                        totalRow.MasterGivenCredit = FormatHandler.GetDouble(totalRow.MasterGivenCredit,2);
                        totalRow.MasterUsedCredit = FormatHandler.GetDouble(totalRow.MasterUsedCredit,2);
                        totalRow.MasterOutstanding = FormatHandler.GetDouble(totalRow.MasterOutstanding,2);
                        totalRow.MasterCashBalance = FormatHandler.GetDouble(totalRow.MasterCashBalance,2);
                        totalRow.MasterTotalBalance = FormatHandler.GetDouble(totalRow.MasterTotalBalance,2);
                    }// end while
                    reportRow.Add(totalRow);
                    report.Rows = reportRow.ToArray();
                    report.ErrorCode = IbcError.SUCCESS;
                    report.ErrorMsg = IbcError.SUCCESS_MSG;
                }//  if (dr == null)
            }//end using
        }
        catch
        {
            report.ErrorCode = IbcError.INTERNAL_ERROR;
            report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
        }

        return report;
    }
}