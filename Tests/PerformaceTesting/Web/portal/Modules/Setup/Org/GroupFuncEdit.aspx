<%@ Page Title="组职能编辑" Language="C#" AutoEventWireup="true" CodeBehind="GroupFuncEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.GroupFuncEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.GroupFuncEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
