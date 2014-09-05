<%@ Page Title="组管理" Language="C#" AutoEventWireup="true" CodeBehind="GroupMgmt.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.GroupMgmt" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <style type="text/css">
        .x-tree-icon { display: none !important; }
    </style>

    <script type="text/javascript">

        function onPgLoad() {

            Ext.create('PIC.view.setup.org.GroupMgmt');
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
