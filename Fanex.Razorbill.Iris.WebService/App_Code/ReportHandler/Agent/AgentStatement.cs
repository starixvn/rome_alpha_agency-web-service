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
/// Summary description for AgentStatement
/// </summary>
public class AgentStatementRow
{
    public AgentStatementRow()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Date;
    public string Remark;
    public string Type;
    public double Total;
}
public class AgentStatement:IbcError
{
    public AgentStatementRow[] Rows;
    public static AgentStatement GetReport(int custId, string fromdate, string todate)
    {
        AgentStatement report = new AgentStatement();
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
        param.Add("fromdate", fdate);
        param.Add("todate", tdate);
        param.Add("custid", custId);
        using (IDataReader dr = DataServices.ExecuteReader("RptAllStatement", param, DatabaseType.ARCHIVE))
        {
            if(dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<AgentStatementRow> reportRow = new List<AgentStatementRow>();
               
                while (dr.Read())
                {
                    AgentStatementRow row = new AgentStatementRow();
                    row.Date = dr["accdate"].ToString();
                    row.Remark = dr["transdesc"].ToString();
                    row.Type = dr["transtype"].ToString();
                    if (dr["totalamt"] != DBNull.Value)
                    {
                        row.Total = FormatHandler.GetDouble(dr["totalamt"],2);
                    }
                    else
                    {
                        row.Total = 0;
                    }
                    reportRow.Add(row);
                }// end while
                report.Rows= reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }//end if (dr == null)
        }// end using
        return report;
    }
}
