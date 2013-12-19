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
/// Summary description for MasterStatement
/// </summary>
public class MasterStatementRow
{
    public MasterStatementRow()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Date;
    public string Type;
    public string Remark;
    public double TotalAmount;
    public double Total;
}

public class MasterStatement:IbcError
{
    public MasterStatementRow[] Rows;
    public static MasterStatement GetReport(int custId)
    {
        MasterStatement report = new MasterStatement();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        Hashtable param = new Hashtable();
        param.Add("custid", custId);
        using (IDataReader dr = DataServices.ExecuteReader("rpt_mastatement", param, DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MasterStatementRow> reportRow = new List<MasterStatementRow>();
                double total = 0;
                while (dr.Read())
                {
                    MasterStatementRow row = new MasterStatementRow();
                    row.Date = dr["matchdate"].ToString();
                    row.Type = dr["transtype"].ToString();
                    row.Remark = dr["transdesc"].ToString();
                    row.TotalAmount = FormatHandler.GetDouble(dr["totalamt"]);
                    row.Total = total + FormatHandler.GetDouble(dr["totalamt"]);
                    reportRow.Add(row);
                }// end while
                report.Rows = reportRow.ToArray();
                report.ErrorCode = IbcError.SUCCESS;
                report.ErrorMsg = IbcError.SUCCESS_MSG;
            }// end if(dr == null)
        }// end using
        return report;
    }
}