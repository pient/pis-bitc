<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Default.aspx.cs" Inherits="PIC.Portal.Web.Default" %>

<%@ Import Namespace="PIC.Portal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <style type="text/css">
        
    body{
	    margin:0;
	    padding:0;
	    text-align:center;
	    font-size:12px;
	    font-family:Arial, Helvetica, sans-serif,'宋体';
	    background: url(images/portal/main_bg.jpg) repeat-x 0 0 #dce2e7;
    }
    
    .x-grid-row 
    {
        border:0px;
        height:25px;
    }
    
    .x-grid-row .x-grid-cell
    {
        border: 1px solid #fafafa;
    }
    
    .x-grid-cell
    {
        background-color:#fafafa !important;
    }
    
    .x-grid-body
    {
        padding-top:0px;
        background-color:#fafafa !important;
    }

    .symbol-img
    {
        vertical-align:middle;
    }

    .symbol-icon
    {
        vertical-align:middle;
        width:16px;
    }
    
    #top
    {
        background-color:#2672ec;
        color:White;
        height:40px;
        padding: 2px;
    }
    
    </style>

    <script type="text/javascript">
        var exitting = false;   // 正在退出标识

        var tYear = <%= DateTime.Now.Year%>;
        var tMonth = <%= DateTime.Now.Month%>;
        var tDay = <%= DateTime.Now.Day%>;
        var tWeek = <%= (int)DateTime.Now.DayOfWeek%>;

        var funcPanel, contentPanel, footerPanel;

        function onPgTimer(){
            PIC.PortalViewport.RefreshTaskMsg();
        }

        function onPgUnload(){
            // DoExit();
        }

        function onPgLoad() {
            mdls = PICState["Modules"] || [];
            
            Ext.create('PIC.view.portal.Viewport');
            
            PIC.PortalViewport.RefreshTaskMsg();

            enablePgTimer();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
<div id="top">
    <table style="width:100%; border-collapse:collapse;">
        <tr>
            <td style="height:40px; font:italic bold 25px arial,sans-serif; color:white ">
                <span style="margin-left:10px"><%=PortalService.SystemInfo.SystemName%></span>
            </td>
            <td align="right" valign="top">
                <div id='func_bar' style="color: white; margin:0px; padding:0px; float:right; font-size:12px; font-weight:bold; color:#eff6f2; margin-right:5px; margin-top:2px; text-align:left;">
                    <span id='func_content'>
                        <!--&nbsp;&nbsp;&nbsp;&nbsp;新任务：条-->
                        <span style="cursor:hand;" onclick='PIC.PortalViewport.LoadMyProfile()'>
                            <span style="vertical-align:middle">
                                <img class="symbol-img usr-pic" src="images/thumbnail/def_usr.png" height="30px;" />
                            </span>
                            <%=UserInfo.Name %>
                        </span>
                        &nbsp;&nbsp;|&nbsp;&nbsp;
                        <span id='unread_content' style='cursor:hand' onclick='PIC.PortalViewport.OnFuncClick("msg")' title="未读消息">
                            <span style="vertical-align:middle">
                                <img class="symbol-icon" src="images/icons/message.png" />
                            </span>
                            <label id="lbl_msgnew" style="color:Red; margin:2px;">
                            0
                            </label>
                        </span>|&nbsp;&nbsp;
                        <span id='newtask_content' style='cursor:hand' onclick='PIC.PortalViewport.OnFuncClick("action")' title="新任务">
                            <span style="vertical-align:middle">
                                <img class="symbol-icon" src="images/icons/task_4.png" />
                            </span>
                            <label id="lbl_tasknew" style="color:Red; margin:2px;">
                            0
                            </label>
                        </span>|&nbsp;&nbsp;
                        <span style='cursor:hand' onclick='PIC.PortalViewport.DoRelogin()'>退出</span>&nbsp;&nbsp;|&nbsp;&nbsp;
                        <span><a href="./help.html" target="_blank" style="color:white">帮助</a></span>&nbsp;&nbsp;
                        <!-- |&nbsp;&nbsp;<span style='cursor:hand' onclick='PIC.PortalViewport.DoExit()'>退出</span>&nbsp;&nbsp; -->
                    </span>
                </div>
            </td>
        </tr>
    </table>
</div>

<div  style="display:none">
    <div id="left_header" style="padding:10px; padding-top:15px; font-weight:bold; color:#095b91; font-size:12px;" align="left">
        <span style="cursor:hand;" onclick='PIC.PortalViewport.OnFuncClick("home")'>
            <span style="vertical-align:middle">
                <img class="symbol-img usr-pic" src="images/thumbnail/def_usr.png" height="45px;" />
            </span>
            &nbsp;&nbsp;<%= UserInfo.Name%>
        </span>
    </div>
    <div id="main" align="center" style="display:none">
        <div align="center">
            <div id='footer_content' style="background-color:#2672ec; padding:5px;">
                <table style="font-size:12px; color:white; padding:5px; width:100%;">
                    <tr>
                        <td>
                        <span id="date_content" style="cursor:hand" onclick="PIC.PortalViewport.OnFuncClick('calendar', {tYear: tYear, tMonth: (tMonth-1), tDay: tDay})">
                            <script language="javascript">
                                var html = '今天是 ' + tYear + '年' + tMonth + '月' + tDay + '日, ' + '星期' + CHS_WEEK_DICT[tWeek];
                                $("#date_content").text(html);
                            </script>
                        </span>
                        &nbsp;|&nbsp;
                        <a href="<%=PortalService.SystemInfo.CompanyWebsite%>" style="color:white; text-decoration:none;" target="_blank">公司主页</a>
                        &nbsp;|&nbsp;
                        <a href="http://www.baidu.com/" style="color:white; text-decoration:none;" target="_blank">百度</a>
                        </td>
                        <td align="right">
                            <span style="cursor:hand; margin-right:20px;" onclick="PICUtil.openVersionDialog()"><%=PortalService.SystemInfo.CompanyName%> 版本信息</span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
</asp:Content>