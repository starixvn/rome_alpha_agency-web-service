using System;
using System.Collections;
using Fanex.Data;

namespace Fanex.Razorbill.Iris.WebService.App_Code.UserLogs
{
    public class UserLogDataAccess
    {
        public int WriteLog(string custName, string actionName, RequestInfo requestInfo, DateTime requestTime)
        {
            using (var db = new ObjectDb("API_WriteLog_UserLog_APILog_RunSP"))
            {
                db.AssignParamValues(new Hashtable
                {
                    { "username", custName },
                    { "loginip", requestInfo.LoginIP },
                    { "browsername", requestInfo.BrowserName },
                    { "platform", requestInfo.Platform },
                    { "countrycode", requestInfo.CountryCode },
                    { "actionname", actionName },
                    { "requestime", requestTime }
                });

                return db.ExecuteNonQuery();
            }
        }
    }
}