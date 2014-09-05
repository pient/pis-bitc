
<%@ Page Title="版本信息" Language="C#" AutoEventWireup="true" CodeBehind="Version.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.App.Version" %>

<%@ OutputCache Duration="1" VaryByParam="None" %>
<%@ Import Namespace="PIC.Portal" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        
        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {

            var tlBar = new Ext.ux.PICFrmToolbar({
                items: [{
                    text: '退出',
                    id: 'btnCancel',
                    iconCls: 'pic-icon-cancel',
                    handler: function () {
                        window.close();
                    }
                }]
            });

            // 页面视图
            viewport = new Ext.ux.PICFrmViewport({
                title: "版本信息",
                tbar: tlBar
            });
        }

        //验证成功执行保存方法
        function SuccessSubmit() {
            PICFrm.submit(pgAction, {}, null, SubFinish);
        }

        function SubFinish(args) {
            RefreshClose();
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="editDiv" align="center">
        <table class="pic-ui-table-edit">
            <tbody>
                <tr>
                    <td align="center" valign="middle">
                        <img src="/portal/images/portal/PIC-LOGO.png" />
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="middle">
                    <b>版本信息： </b><br />
                    <div style="margin:5px; margin-left:20px; ">
                        版本说明：<%=PortalService.SystemInfo.SystemName%> V2.1<br />
                        可用语言：中文简体 <br />
                        最大用户数限：300<br />
                    </div>
                    
                    <!--<b>警告： </b><br />
                    <div style="margin:5px; margin-left:20px;">
                        本计算机程序受到著作权法和国际公约的保护。未经授权擅自复制或传播本程序的部分或全部，可能受到严厉的民事及刑事制裁，并将在法律许可的范围内受到最大可能的起诉。 
                    </div>-->
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>


