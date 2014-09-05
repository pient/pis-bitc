<%@ Page Title="系统模块管理" Language="C#" AutoEventWireup="true" CodeBehind="ModuleMgmt.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Mdl.ModuleMgmt" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <style type="text/css">
        .x-tree-icon { display: none !important; }
    </style>

    <script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.mdl.ModuleMgmt');
    }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
