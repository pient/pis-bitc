<%@ Page Title="职能编辑" Language="C#" AutoEventWireup="true" CodeBehind="FuncEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.FuncEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.FuncEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
