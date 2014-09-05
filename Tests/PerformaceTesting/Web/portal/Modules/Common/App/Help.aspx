
<%@ Page Title="帮助" Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.App.Help" %>

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
                title: "帮助",
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
                <tr style="display: none">
                    <td>
                        <input id="HelpID" name="HelpID" />
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="middle">
                        <img src="/portal/images/portal/PIC-LOGO.png" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>


