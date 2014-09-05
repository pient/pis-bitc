<%@ Page Title="系统用户" Language="C#" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.UserEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.UserEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
