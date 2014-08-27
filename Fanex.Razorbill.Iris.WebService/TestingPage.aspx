<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestingPage.aspx.cs" Inherits="TestingPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .defaultpage
        {
            width: 300px;
            float: left;
        }
        .leftBox
        {
            width: 75px;
            float: left;
        }
        .righttBox
        {
            width: 200px;
            float: left;
        }
        .nextBox
        {
            float: left;
            padding-top: 5px;
        }
    </style>
</head>
<body>
    <div class="defaultpage">
        <div class = "nextBox">
            <div class="leftBox">CustName</div>
            <div class="righttBox"><input type="text" id="txtCustID" name="txtCustID" value="agent01sub00" /></div>
        </div>
        <div class = "nextBox">
            <div class="leftBox">Password</div>
            <div class="righttBox"><input type="text" id="txtPassword" name="txtPassword"  value="1234aaAA"/></div>
        </div>
        <div class = "nextBox">
            <div class="leftBox"></div>
            <div class="righttBox"><input type="button" value="GetAgentBetListRunning" id="btnGetRunning" /></div>
        </div>
        
        <div class = "nextBox">
            <div style ="width: 170px; float:left;">
                <div style="float:left; width:35px;">Date:</div>
                
                <input type="text" value="" id="txtReportDate" style="width:120px;" />
            </div>
            <div class="righttBox">
                <input type="button" value="GetAgentBetListWinLoss" id="btnGetWinLoss" />
            </div>
        </div>

        <div class = "nextBox" id="divResult">
        </div>
    </div>

    <script src="Js/JQuery.js" type="text/javascript" language="javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#btnGetRunning").bind("click", function () {
                getAgentBetListRunning();
            });
            $("#btnGetWinLoss").bind("click", function () {
                getAgentBetListWinLoss();
            });
        });

        function getAgentBetListRunning() {
            $('#divResult').html('');
            if ($('#txtCustID').val().length == 0) {
                alert('CustID cannot null');
                return false;
            }
            if ($('#txtPassword').val().length == 0) {
                alert('Password cannot null');
                return false;
            }

            var soapMessage = '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">\
                                  <soap:Header>\
                                    <AuthHeader xmlns="http://schemas.xmlsoap.org/wsdl/">\
                                      <Id>' + $("#txtCustID").val() + '</Id>\
                                      <Password>' + $("#txtPassword").val() + '</Password>\
                                    </AuthHeader>\
                                  </soap:Header>\
                                  <soap:Body>\
                                    <GetAgentBetListRunning xmlns="http://schemas.xmlsoap.org/wsdl/" />\
                                  </soap:Body>\
                                </soap:Envelope>';
            $.ajax({
                url: "AgentReport.asmx?op=GetAgentBetListRunning",
                type: "POST",
                dataType: "xml",
                data: soapMessage,
                success: function (data, status, req) {
                    if (status == "success"){
                        $('#divResult').html($(req.responseXML).find("GetAgentBetListRunningResult").text());
                    }
                    
                },
                contentType: "text/xml; charset=\"utf-8\""
            });
        }

        function getAgentBetListWinLoss() {
            $('#divResult').html('');
            if ($('#txtCustID').val().length == 0) {
                alert('CustID cannot null');
                return false;
            }
            if ($('#txtPassword').val().length == 0) {
                alert('Password cannot null');
                return false;
            }
            if ($('#txtReportDate').val().length == 0) {
                alert('Report date cannot null');
                return false;
            }

            var soapMessage = '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">\
                                  <soap:Header>\
                                    <AuthHeader xmlns="http://schemas.xmlsoap.org/wsdl/">\
                                      <Id>' + $("#txtCustID").val() + '</Id>\
                                      <Password>' + $("#txtPassword").val() + '</Password>\
                                    </AuthHeader>\
                                  </soap:Header>\
                                  <soap:Body>\
                                    <GetAgentBetListWinLoss xmlns="http://schemas.xmlsoap.org/wsdl/">\
                                      <date>' + $("#txtReportDate").val() + '</date>\
                                    </GetAgentBetListWinLoss>\
                                  </soap:Body>\
                                </soap:Envelope>';
            $.ajax({
                url: "AgentReport.asmx?op=GetAgentBetListWinLoss",
                type: "POST",
                dataType: "xml",
                data: soapMessage,
                success: function (data, status, req) {
                    if (status == "success"){
                        $('#divResult').html($(req.responseXML).find("GetAgentBetListWinLossResult").text());
                    }
                },
                contentType: "text/xml; charset=\"utf-8\""
            });
        }
    </script>
</body>
</html>
