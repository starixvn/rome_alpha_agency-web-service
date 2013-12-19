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
/// Summary description for AgentDailySatement
/// </summary>
public class AgentDailyStatementRow
{
    public AgentDailyStatementRow()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Date;// mm//dd//yyyy
    public string Remark;
    public string Type;
    public double Total;
}

public class AgentDailyStatement:IbcError
{
    public AgentDailyStatementRow[] Rows;

    public static AgentDailyStatement GetReport(string username, string fromdate, string todate)
    {
        AgentDailyStatement report = new AgentDailyStatement();
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
        using (IDataReader dr = DataServices.ExecuteReader("rptallstatement", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<AgentDailyStatementRow> reportRow = new List<AgentDailyStatementRow>();
                while (dr.Read())
                {
                    AgentDailyStatementRow row = new AgentDailyStatementRow();
                    row.Date = dr["accdate"].ToString();
                    row.Remark = dr["transdesc"].ToString();
                    row.Type = dr["transtype"].ToString();
                    row.Total = FormatHandler.GetDouble(dr["totalamt"]);
                    reportRow.Add(row);
                }// end while
                report.Rows = reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }// end if(dr==null)
        }// end using
        return report;
    }
}
