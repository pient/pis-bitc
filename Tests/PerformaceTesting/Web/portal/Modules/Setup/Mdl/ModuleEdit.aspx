<%@ Page Title="系统模块" Language="C#" AutoEventWireup="true" CodeBehind="ModuleEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Mdl.ModuleEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
<%@ OutputCache Duration="1" VaryByParam="None" %>

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.mdl.ModuleEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
