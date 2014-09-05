<%@ Page Title="编辑" Language="C#" AutoEventWireup="true" CodeBehind="AuthEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Auth.AuthEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {

        Ext.create('PIC.view.setup.auth.AuthEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
