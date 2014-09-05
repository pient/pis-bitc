<%@ Page Title="角色编辑" Language="C#" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.RoleEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.RoleEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
