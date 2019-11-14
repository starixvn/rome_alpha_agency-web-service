using System;
using System.Text;
using System.Web;
using UAParser;

namespace Fanex.Razorbill.Iris.WebService.App_Code.UserLogs
{
    public class RequestInfo
    {
        public RequestInfo()
        {
        }

        public RequestInfo(HttpRequest request)
        {
            GetRequestInfo(request);
        }

        public string LoginIP { get; private set; }

        //public string LoginDomain { get; private set; }

        public string BrowserName { get; private set; }

        public string Platform { get; private set; }

        public string CountryCode { get; private set; }

        private void GetRequestInfo(HttpRequest request)
        {
            LoginIP = GetClientIP(request);
            //CountryCode = GetCountryCode(LoginIP);
            //LoginDomain = request.ServerVariables["SERVER_NAME"];

            var browserName = string.Empty;
            var platform = string.Empty;

            try
            {
                var userAgentParser = Parser.GetDefault();
                var clientInfo = userAgentParser.Parse(request.UserAgent ?? string.Empty);

                browserName = GetBrowserName(clientInfo.UserAgent);
                platform = GetPlatform(clientInfo);
            }
            catch (Exception ex)
            {
                Fanex.Logging.LogService.Error("Cannot get request info - " + ex.ToString());
            }
            
            BrowserName = browserName;
            Platform = platform;
        }

        private static string GetClientIP(HttpRequest request)
        {
            var serverVariables = request.ServerVariables;
            var ip = serverVariables["HTTP_X_FORWARDED_FOR"] ?? serverVariables["REMOTE_ADDR"];

            var ips = string.IsNullOrEmpty(ip)
                ? new string[] { string.Empty }
                : ip.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return ips[0];
        }

        private static string GetBrowserName(UserAgent userAgent, string browserSize = null)
        {
            var browserBuilder = new StringBuilder();
            browserBuilder.AppendFormat(
                "{0} {1}.{2}.{3}",
                userAgent.Family,
                userAgent.Major,
                userAgent.Minor,
                userAgent.Patch ?? "0");

            if (!string.IsNullOrWhiteSpace(browserSize))
            {
                browserBuilder.Append(", ").Append(browserSize);
            }

            return browserBuilder.ToString();
        }

        private static string GetPlatform(ClientInfo clientInfo)
        {
            var deviceInfo = clientInfo.Device;
            var device = string.Format("{0}, IsCrawler: {1}", deviceInfo.Family, deviceInfo.IsSpider);

            var os = clientInfo.OS;
            var majorVersion = os.Major ?? "0";
            var minorVersion = os.Minor ?? "0";
            var osVersion = os.Patch ?? "0";
            var operateSystem = string.Format("{0} {1}.{2}.{3}", os.Family, majorVersion, minorVersion, osVersion);

            return string.Format("{0}, Device: {1}", operateSystem, device);
        }

        //private static string GetCountryCode(string ip)
        //{
        //    var country = "Unknown";

        //    try
        //    {
        //        var infoIp = IpUtils.LocationFromIP(ip);
        //        country = infoIp.CountryLong == "Reserved" ? null : infoIp.CountryLong;
        //    }
        //    catch (Exception ex)
        //    {
        //        Fanex.Logging.LogService.Error("Cannot get country info (" + ip + ") - " + ex.ToString());
        //    }

        //    return country;
        //}
    }
}