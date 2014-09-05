<%@ Page Title="职能角色编辑" Language="C#" AutoEventWireup="true" CodeBehind="FuncRoleEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.FuncRoleEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.FuncRoleEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
