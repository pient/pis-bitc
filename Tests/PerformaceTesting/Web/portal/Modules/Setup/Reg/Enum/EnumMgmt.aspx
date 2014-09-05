<%@ Page Title="系统枚举" Language="C#" AutoEventWireup="true" CodeBehind="EnumMgmt.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Reg.EnumMgmt" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <style type="text/css">
        .x-tree-icon { display: none !important; }
    </style>

    <script type="text/javascript">

        function onPgLoad() {

            Ext.create('PIC.view.setup.reg.enum.EnumMgmt');
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
