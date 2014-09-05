<%@ Page Title="系统参数" Language="C#" AutoEventWireup="true" CodeBehind="DocMgmt.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Doc.DocMgmt" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
    <style type="text/css">
        .x-tree-icon { display: none !important; }
    </style>

    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.setup.doc.DocMgmt');
        }
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
