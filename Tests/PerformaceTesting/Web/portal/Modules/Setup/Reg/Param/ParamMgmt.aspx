<%@ Page Title="系统参数" Language="C#" AutoEventWireup="true" CodeBehind="ParamMgmt.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Reg.ParamMgmt" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
    <style type="text/css">
        .x-tree-icon { display: none !important; }
    </style>

    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.setup.reg.param.ParamMgmt');
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
