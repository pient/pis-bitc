<%@ Page Title="流程实例" Language="C#" AutoEventWireup="true" CodeBehind="WfInstanceEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Bpm.WfInstanceEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.bpm.WfInstanceEdit');
    }
    
</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
