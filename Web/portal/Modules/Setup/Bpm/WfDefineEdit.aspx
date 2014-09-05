<%@ Page Title="流程定义" Language="C#" AutoEventWireup="true" CodeBehind="WfDefineEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Bpm.WfDefineEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.bpm.WfDefineEdit');
    }
    
</script>

</asp:Content>
