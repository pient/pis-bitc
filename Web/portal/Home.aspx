<%@ Page Title="首页" Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PIC.Portal.Web.Home" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    <link href="/portal/css/portal/portal.css" rel="stylesheet" />

    <!--用于支持图片新闻-->
    <link href="/portal/js/lib/jQuery.popeye/css/popeye/jquery.popeye.css" rel="stylesheet" />
    <link href="/portal/js/lib/jQuery.popeye/css/popeye/jquery.popeye.style.css" rel="stylesheet" />
    <script src="/portal/js/lib/jQuery.popeye/lib/popeye/jquery.popeye-2.1.js"></script>

    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.portal.home.Viewport');
        }

    </script>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <span id="app-msg" style="display:none;"></span>
    
</asp:Content>
