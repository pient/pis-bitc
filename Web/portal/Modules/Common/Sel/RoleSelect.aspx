<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleSelect.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Sel.RoleSelect" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
<script type="text/javascript">

    function onPgLoad() {

        Ext.create('PIC.view.com.org.RoleSelector');

    }
    
 </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
