<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FuncSelect.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Sel.FuncSelect" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
<script type="text/javascript">

    function onPgLoad() {

        Ext.create('PIC.view.com.sel.FuncSelector');

    }
    
 </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
