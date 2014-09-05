<%@ Page Title="用户权限" Language="C#" AutoEventWireup="true" CodeBehind="UserAuthView.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.UserAuthView" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
<style type="text/css">
    .x-tree-icon { display: none !important; }
</style>

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.UserAuthView');
    }

</script>

</asp:Content>
