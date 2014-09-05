
<%@ Page Title="行政（月结）费用报销" Language="C#" AutoEventWireup="true" Async="true" CodeBehind="OA_ReimbFlow.aspx.cs" Inherits="PIC.Biz.Web.OA_ReimbFlow" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript" src="/portal/js/pgctrl-ext4-3rd.js"></script>
    <script type="text/javascript" src="/portal/js/pgctrl-ext4.js"></script>
    <script type="text/javascript" src="/portal/js/pgctrl-ext4-form.js"></script>
    <script type="text/javascript" src="/portal/js/pgfunc-ext4.js"></script>
    <script type="text/javascript" src="./js/OA_ReimbFlow.js"></script>

    <script type="text/javascript">

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            // 页面视图
            viewport = Ext.create('PIC.ExtViewport', {
                items: { xtype: 'picreimbformpanel' }
            });
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">

</asp:Content>


