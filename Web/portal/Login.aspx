<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PIC.Portal.Web.Login" %>

<%@ Import Namespace="PIC" %>
<%@ Import Namespace="PIC.Portal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title><%=PortalService.SystemInfo.SystemName %></title>
<meta http-equiv="Content-Type" />
    <script src="/portal/js/lib/jquery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="/portal/js/app/common.js" type="text/javascript"></script>
    <script src="/portal/js/lib/jquery/jquery.form.js" type="text/javascript"></script>
    <script src="/portal/js/lib/jquery/jquery.plug-ins.js" type="text/javascript"></script>
    <style type="text/css">
	    td img {display: block; }
	    
	    body{
	        margin:0;
	        padding:0;
	        text-align:center;
	        font-size:12px;
	        font-family:Arial, Helvetica, sans-serif,'宋体';
	        background: url(/portal/images/portal/main_bg.jpg) repeat-x 0 0 #dce2e7;
        }
        	
	    #main{
	    }
	    
	    .text-input
	    {
	        border:solid 1px #8FAACF;
	    }
	    
	    .lbl-message
	    {
	        color:Red;
	    }
    	
    </style>
    
    <script language="javascript" type="text/javascript">
        var islogining = false;

        function onPgLoad() {
            $(document).bind("keydown", function(e) {
                // 回车
                if (e.keyCode == 13 && !islogining) {
                    DoLogin();
                }
            });
            
            adjustMainHeight();
        }

        function onPgUnload() {
            $.unloadPage();
        }

        function onPgResize(){
	        adjustMainHeight();
        }

        function adjustMainHeight(){
	        var headerHeight = parseInt(document.getElementById("header").clientHeight) || 0;
	        var mainHeight = parseInt(document.documentElement.clientHeight) - headerHeight;
        	
	        var mainTableHeight = parseInt(document.getElementById("mainTable").clientHeight) || 0;

	        $("#mainTable").css("height", mainHeight + 50);
	        // $("#main").css("height", mainHeight + 50);
        }

        function DoLogin() {
            if (islogining) {
                return;
            }
            
            setLoginStatus(true);

            var uname = $("#uname").val(), pwd = $("#pwd").val();

            if (!uname) {
                if (!uname) {
                    $("#message").text("请输入用户名。");
                    $("#uname").focus();
                } else if (!pwd) {
                    $("#message").text("请输入密码。");
                    $("#uname").focus();
                }

                setLoginStatus(false);
                return;
            } else {
                $("#message").text("");
            }

            $("form").ajaxSubmit({ data: { 'reqaction': 'login', 'asyncreq': true }, success: function(resp) {
                setLoginStatus(false);

                if (resp) {
                    if (resp.indexOf("success") == 0) {
                        var redurl = resp.substr("success".length + 1);
                        location.href = redurl;
                    } else {
                        $("#message").html(resp);
                    }
                }
            }
            });
        }

        function OpenPwdForgotPage() {
            
        }

        function setLoginStatus(flag) {
            if (flag) {
                islogining = true;
                $("input").attr("disabled", true);
                $("#imgDoLogin").attr("disabled", true);

                $("#span-loading").css("display", ""); // 显示进度条
            } else {
                islogining = false;
                $("input").attr("disabled", false);
                $("#imgDoLogin").attr("disabled", false);
                $("#span-loading").css("display", "none"); // 隐藏进度条
            }
        }
        
    </script>
</head>
<body onload="onPgLoad()" onbeforeunload="onPgUnload()" onresize="onPgResize()">
<form id="Form1" runat="server">
<div id="main" style="vertical-align:middle;" align="center">
    <table id="mainTable" style="height:260px;" border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td>
            <div style="background-color:rgb(223, 232, 246); color:rgb(36, 94, 180); border:1px solid; padding:10px; padding-left:20px; padding-right:20px;">
            <table id="innerTable" style="height:260px;" border="0" cellpadding="0" cellspacing="0">
              <tr>
               <td style="width:400px;" align="center">
                    <div id="header" style="margin:20px; height:50px;" >
                        <div style="font-size:29px; font-weight:bold; font-style:italic; white-space:nowrap;">
                            <%=PortalService.SystemInfo.SystemName%>
                        </div>
                    </div>
                   <img src="/portal/images/portal/login/login_bg.jpg" alt="" width="320" />
                   <div style="height:50px; width:300px; padding:10px; background-color:#2672ec; color:White">
                        <!-- 每日图片及说明 -->
                   </div>
                   <br /><br /><br /><br />
                </td>
                <td align="left" style="width:210px;" >
                    <div style="font-weight:bold;">
                        用户名 : <br /><input id="uname" name="uname" value="<%=PersistedLoginPkg.uname %>" class="text-input" style="width:200px;" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <br /><br />
                        密码: <br /><input id="pwd" name="pwd" value="<%=PersistedLoginPkg.pwd %>" class="text-input" type="password" style="width:200px;" />
                        &nbsp;&nbsp;
                        <br /><br />
                        <input type="checkbox" runat="server" name="saveAcountName" id="saveAcount" value="true" />
                        <label for="checkbox" style="font-weight:normal">记住用户名</label>
                        &nbsp;&nbsp;
                        <input type="checkbox" runat="server" name="savePassword" id="savePassword" value="true" />
                        <label for="checkbox" style="font-weight:normal">记住密码</label>
                        <br /><br />
                        <table border="0" cellpadding="2" cellspacing="2" >
                            <tr>
                                <td valign="baseline" style="width:50px;">
                                    <input id="btnDoLogin" type="button" onclick="DoLogin();" value="登录" style="cursor:hand;" />
                                </td>
                                <td valign="middle" style="width:100px;">
                                    <span onclick="OpenPwdForgotPage()" style="cursor:hand; margin-left:15px; display:none;">忘记密码</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan=2 align="center" style="height:15px;">
                                    <label class="lbl-message" id="message" name="message"></label>
                                    <span id="span-loading" style="display:none;">
                                        <img src="images/portal/loading.gif" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
               </td>
              </tr>
              <tr>
                <td colspan=2>
                    <hr />
                    <span style="float:left">当前在线用户数：<%= PortalService.GetOnlineUserCount() %></span>
                    <!--<span style="float:left">©2012 PIC</span>-->
                    <span style="float:right">
						<a href="./Help.html" target="_blank">帮助</a>
					</span>
                </td>
              </tr>
            </table>
            </div>
        </td>
        </tr>
        </table>
     </div>
 </form>
</body>
</html>
