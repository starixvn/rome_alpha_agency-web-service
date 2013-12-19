using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using RemotingRef;

/// <summary>
/// Summary description for CacheRef
/// </summary>
public static class CacheRef
{
    private static string TWLang(string lang)
    {
        switch (lang)
        {
            case "en-US": return "en";
            case "zh-TW": return "ch"; // Traditional
            case "zh-CN": return "cs"; // Simplified
            case "ja-JP": return "jp";
            case "th-TH": return "th"; // Thai
            default: return "en";
        }
    }
    public static string GetBettype(object bettypeID)
    {
        return RefData.GetBetTypeName(bettypeID.ToString(), "en", string.Empty);
    }

    public static string GetSportName(object sportID)
    {
        return RefData.GetSportName(sportID.ToString(), "en", string.Empty);
    }

    public static string GetTeamName(object teamID)
    {
        return RefData.GetTeamName(teamID.ToString(), "en", string.Empty);
    }

    public static string GetLeagueName(object leagueID)
    {
        return RefData.GetLeagueName(leagueID.ToString(), "en", string.Empty);
    }
    public static string GetOddsTypeName(object oddsTypeId)
    {
        return RefData.GetOddsTypeName(oddsTypeId.ToString(), "en",string.Empty);        
    }    
}
