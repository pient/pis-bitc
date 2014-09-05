
<%@ Page Title="招聘需求申请" Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Flow.aspx.cs" Inherits="PIC.Biz.Web.RecruitNeeds.Flow" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <script type="text/javascript" src="/portal/js/pgctrl-ext4-3rd.js"></script>
    <script type="text/javascript" src="/portal/js/pgctrl-ext4.js"></script>
    <script type="text/javascript" src="/portal/js/pgctrl-ext4-form.js"></script>
    <script type="text/javascript" src="/portal/js/pgfunc-ext4.js"></script>
    <script type="text/javascript" src="./js/Flow.js"></script>

    <script type="text/javascript">

        function onPgLoad() {
            setPgUI();
        }

        function setPgUI() {
            // 页面视图
            viewport = Ext.create('PIC.ExtViewport', {
                items: { xtype: 'picrecruitneedsapyformpanel' }
            });
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">

</asp:Content>


