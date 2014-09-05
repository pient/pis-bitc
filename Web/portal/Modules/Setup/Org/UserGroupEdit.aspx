<%@ Page Title="编辑" Language="C#" AutoEventWireup="true" CodeBehind="UserGroupEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Org.UserGroupEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.setup.org.UserGroupEdit');
    }

    </script>

</asp:Content>
