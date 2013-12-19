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


public class AgentBalanceRow
{
    public string AgentId;
    public double AgentGivenCredit = 0;
    public double AgentUsedCredit = 0;
    public double AgentOutstanding = 0;
    public double AgentCashBalance = 0;
    public double AgentTotalBalance = 0;
    public string FirstName;
    public string LastName;
}

public class MasterAgentBalance : IbcError
{
    public AgentBalanceRow[] Rows;

    public static MasterAgentBalance GetReport(int custId, bool Closed)
    {
        MasterAgentBalance report = new MasterAgentBalance();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        Hashtable param = new Hashtable();
        param.Add("masterid", custId);
        if (Closed) param.Add("closed", 1);
        using (IDataReader dr = DataServices.ExecuteReader("MasterAgentBalanceList", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<AgentBalanceRow> reportRow = new List<AgentBalanceRow>();
                AgentBalanceRow totalRow = new AgentBalanceRow();
                totalRow.AgentId = "Total";
                while (dr.Read())
                {
                    AgentBalanceRow row = new AgentBalanceRow();

                    row.AgentId = dr["username"].ToString();
                    row.AgentGivenCredit = FormatHandler.GetDouble(dr["AgentCredit"],2);
                    row.AgentUsedCredit = FormatHandler.GetDouble(dr["AgentUsedCredit"],2);
                    row.AgentOutstanding = FormatHandler.GetDouble(dr["AgentOutstanding"],2);
                    row.AgentCashBalance = FormatHandler.GetDouble(dr["AgentCashBalance"],2);
                    row.AgentTotalBalance = FormatHandler.GetDouble(dr["AgentBalance"],2);
                    row.FirstName = dr["FirstName"].ToString();
                    row.LastName = dr["LastName"].ToString();

                    reportRow.Add(row);

                    totalRow.AgentGivenCredit += row.AgentGivenCredit;
                    totalRow.AgentUsedCredit += row.AgentUsedCredit;
                    totalRow.AgentOutstanding += row.AgentOutstanding;
                    totalRow.AgentCashBalance += row.AgentCashBalance;
                    totalRow.AgentTotalBalance += row.AgentTotalBalance;

                    totalRow.AgentGivenCredit = FormatHandler.GetDouble(totalRow.AgentGivenCredit,2);
                    totalRow.AgentUsedCredit = FormatHandler.GetDouble(totalRow.AgentUsedCredit,2);
                    totalRow.AgentOutstanding = FormatHandler.GetDouble(totalRow.AgentOutstanding,2);
                    totalRow.AgentCashBalance = FormatHandler.GetDouble(totalRow.AgentCashBalance,2);
                    totalRow.AgentTotalBalance = FormatHandler.GetDouble(totalRow.AgentTotalBalance, 2);
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