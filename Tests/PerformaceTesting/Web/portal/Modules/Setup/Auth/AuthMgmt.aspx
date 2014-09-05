<%@ Page Title="权限管理" Language="C#" AutoEventWireup="true" CodeBehind="AuthMgmt.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Auth.AuthMgmt" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <style type="text/css">
        .x-tree-icon { display: none !important; }
    </style>

    <script type="text/javascript">

        function onPgLoad() {

            Ext.create('PIC.view.setup.auth.AuthMgmt');

        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
