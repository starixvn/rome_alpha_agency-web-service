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


public class MasterTransfer:IbcError
{
    public static MasterTransfer Transfer(int MasterID, string ToUserName, double Amount, string lang)
    {
        MasterTransfer report = new MasterTransfer();
        report.ErrorCode = IbcError.SUCCESS;
        report.ErrorMsg = IbcError.SUCCESS_MSG;

        if (string.IsNullOrEmpty(ToUserName) || string.IsNullOrEmpty(Amount.ToString()))
        {
            report.ErrorCode = IbcError.INVALID_USERNAME_0R_PASSWORD;
            report.ErrorMsg = IbcError.INVALID_USERNAME_0R_PASSWORD_MSG;
            return report;
        }
        if (string.IsNullOrEmpty(lang)) lang = "E";
        Hashtable param = new Hashtable();
        param.Add("masterid", MasterID);
        param.Add("tousername", ToUserName);
        param.Add("creator", MasterID);
        param.Add("amttrans", Amount);
        param.Add("language", lang);
        param.Add("errcode", 0);
        param.Add("errmsg", "");
        DataResult dr = null;
        try
        {
            dr = DataServices.ExecuteCommand("BettransInsClearBalance_MAg_API", param, DatabaseType.MAIN);
            if (dr == null)
            {
                report.ErrorCode = IbcError.INTERNAL_ERROR;
                report.ErrorMsg = IbcError.INTERNAL_ERROR_MSG;
            }
            else if (!dr.OutputResults["errcode"].Equals(0))
            {
                report.ErrorCode = FormatHandler.GetInt(dr.OutputResults["errcode"]);
                report.ErrorMsg = dr.OutputResults["errmsg"].ToString();
            }
        }
        catch (Exception ex)
        {
            report.ErrorCode = FormatHandler.GetInt(dr.OutputResults["errcode"]);
            report.ErrorMsg = dr.OutputResults["errmsg"].ToString();
        }
        return report;
    }
}
