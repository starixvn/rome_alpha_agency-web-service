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
/// Row in AgentMemberWinLoss report
/// </summary>
public class MemberWinLossRow
{
    public MemberWinLossRow()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string MemberId;
    public double Turnover;
    public double MemberWinLoss;
    public double MemberCommission;
    public double MemberTotal;
    public double AgentWinLoss;
    public double AgentCommission;
    public double AgentTotal;
    public double CompanyTotal;

}
/// <summary>
/// Agent_MemberWinLoss report
/// </summary>
public class AgentMemberWinLoss:IbcError
{
    public MemberWinLossRow[] Rows;
    public static AgentMemberWinLoss GetReport(string username,string fromdate,string todate)
    {
        AgentMemberWinLoss report = new AgentMemberWinLoss();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;
        DateTime fdate,tdate;
        if (!Validator.ValidDateMMDDYYYY(fromdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(todate, out tdate).IsSuccess())
        {
            report.ErrorCode = IbcError.DATE_INVALID_FORMAT;
            report.ErrorMsg = IbcError.DATE_INVALID_FORMAT_MSG;
            return report;
        }
        Hashtable param=new Hashtable();
        param.Add("username",username);
        param.Add("fromdate",fdate);
        param.Add("todate",tdate);
        
        using (IDataReader dr = DataServices.ExecuteReader("CustomerSelAllWinLoss",param,DatabaseType.ARCHIVE))
        {
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else
            {
                List<MemberWinLossRow> reportRows = new List<MemberWinLossRow>();
                MemberWinLossRow totalRow = new MemberWinLossRow();
                totalRow.MemberId = "Total";
                totalRow.Turnover = totalRow.MemberWinLoss = totalRow.MemberCommission = totalRow.MemberTotal
                    = totalRow.AgentWinLoss = totalRow.AgentCommission = totalRow.AgentTotal = totalRow.CompanyTotal = 0;
                while (dr.Read())
                {
                    MemberWinLossRow row = new MemberWinLossRow();
                    row.MemberId = dr["username"].ToString();
                    row.Turnover = FormatHandler.GetDouble(dr["turnover"],2);
                    row.MemberWinLoss = FormatHandler.GetDouble(dr["winlost"],2);
                    row.MemberCommission = FormatHandler.GetDouble(dr["playercomm"],2);
                    row.MemberTotal = row.MemberWinLoss + row.MemberCommission;
                    
                    row.MemberTotal = FormatHandler.GetDouble( row.MemberTotal,2);

                    row.AgentWinLoss = FormatHandler.GetDouble(dr["awinlost"],2);
                    row.AgentCommission = FormatHandler.GetDouble(dr["comm"]) - row.MemberCommission;

                    row.AgentCommission = FormatHandler.GetDouble( row.AgentCommission,2);

                    row.AgentTotal = row.AgentCommission + row.AgentWinLoss;
                    row.CompanyTotal = -(row.MemberWinLoss + FormatHandler.GetDouble(dr["comm"]) + row.AgentWinLoss);

                    row.CompanyTotal =  FormatHandler.GetDouble( row.CompanyTotal,2);

                    reportRows.Add(row);

                    totalRow.Turnover += row.Turnover;
                    totalRow.MemberWinLoss += row.MemberWinLoss;
                    totalRow.MemberTotal += row.MemberTotal;
                    totalRow.MemberCommission += row.MemberCommission;
                    totalRow.AgentWinLoss += row.AgentWinLoss;
                    totalRow.AgentCommission += row.AgentCommission;
                    totalRow.AgentTotal += row.AgentTotal;
                    totalRow.CompanyTotal += row.CompanyTotal;

                    totalRow.Turnover = FormatHandler.GetDouble(totalRow.Turnover,2);
                    totalRow.MemberWinLoss = FormatHandler.GetDouble(totalRow.MemberWinLoss,2);
                    totalRow.MemberTotal = FormatHandler.GetDouble(totalRow.MemberTotal,2);
                    totalRow.MemberCommission = FormatHandler.GetDouble(totalRow.MemberCommission,2);
                    totalRow.AgentWinLoss = FormatHandler.GetDouble(totalRow.AgentWinLoss,2);
                    totalRow.AgentCommission = FormatHandler.GetDouble(row.AgentCommission,2);
                    totalRow.AgentTotal = FormatHandler.GetDouble(totalRow.AgentTotal,2);
                    totalRow.CompanyTotal = FormatHandler.GetDouble(totalRow.CompanyTotal, 2);
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