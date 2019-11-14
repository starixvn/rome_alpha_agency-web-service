using System;

namespace Fanex.Razorbill.Iris.WebService.App_Code.UserLogs
{
    public class UserLogService
    {
        public void WriteLog(string custName, string actionName, RequestInfo requestInfo)
        {
            var dataAccess = new UserLogDataAccess();
            dataAccess.WriteLog(custName, actionName, requestInfo, DateTime.Now);
        }
    }
}