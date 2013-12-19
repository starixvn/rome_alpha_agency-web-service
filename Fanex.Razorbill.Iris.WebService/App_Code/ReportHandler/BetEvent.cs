using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for BetEvent
/// </summary>
public class BetEvent
{
    public BetEvent()
    { }
    public string RefNo;
    public string Home;
    public string Away;
    public string League;
    public string Bettype;
    
    
 
}

public class MPBetEvent : BetEvent
{
    public MPBetEvent()
    { 
    }
    public string Choice;
    public decimal Handicap;
    public decimal Odd;
}