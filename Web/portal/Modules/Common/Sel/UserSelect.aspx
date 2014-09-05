<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSelect.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Sel.UserSelect" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.com.org.UserSelector');
    }
    
 </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
