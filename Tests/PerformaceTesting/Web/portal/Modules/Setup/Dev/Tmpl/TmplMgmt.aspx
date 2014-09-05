<%@ Page Title="系统模版" Language="C#" AutoEventWireup="true" CodeBehind="TmplMgmt.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Dev.Tmpl.TmplMgmt" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
    <style type="text/css">
        .x-tree-icon { display: none !important; }
    </style>

    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.setup.dev.tmpl.TmplMgmt');
        }
    </script>

</asp:Content>
