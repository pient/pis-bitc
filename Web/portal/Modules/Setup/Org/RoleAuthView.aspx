<%@ Page Title="角色权限" Language="C#" AutoEventWireup="true" CodeBehind="RoleAuthView.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.RoleAuthView" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
<style type="text/css">
    .x-tree-icon { display: none !important; }
</style>

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.RoleAuthView');
    }

</script>

</asp:Content>
