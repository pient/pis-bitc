<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="UserGroupView.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.UserGroupView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
<style type="text/css">
    .x-tree-icon { display: none !important; }
</style>

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.UserGroupView');
    }

</script>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
