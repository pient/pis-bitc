
<%@ Page Title="流程图查看" Language="C#" AutoEventWireup="true" CodeBehind="FlowPicView.aspx.cs" Inherits="PIC.Biz.Web.DailyBudget.FlowPicView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">
        
        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
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
        <img src="./images/Flow.jpg" />
    </div>
</asp:Content>


