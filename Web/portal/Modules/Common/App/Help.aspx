<%@ Page Title="帮助" Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.App.Help" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="editDiv" align="center">
        <table class="pic-ui-table-edit" style="margin:100px;">
            <tbody>
                <tr style="display: none">
                    <td>
                        <input id="HelpID" name="HelpID" />
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="left">
                        <p><a href="mailto:nkxt@bitc.edu.cn">您好各位老师，新系统已经上线试运行，以下为相关链接，如果有问题，请发送邮件至nkxt@bitc.edu.cn</a>：</p>
                        <p> 相关链接：</p>
                        <p>
                            <a href="clientbin/用户手册.doc">用户手册</a>&nbsp;&nbsp;|&nbsp;&nbsp; 
                            <a href="clientbin/Flash修复工具.zip">Flash修复工具下载</a>&nbsp;&nbsp;|&nbsp;&nbsp; 
                            <a href="clientbin/chrome_installer.zip">谷歌浏览器下载</a>&nbsp;&nbsp;|&nbsp;&nbsp; 
                            <a href="http://10.0.7.154:9345">测试网站</a>&nbsp;&nbsp;|&nbsp;&nbsp; 
                            <a href="clientbin/测试附件.doc">测试附件下载</a>&nbsp;&nbsp;|&nbsp;&nbsp; 
                        </p>
                        <p>&nbsp;</p>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>


