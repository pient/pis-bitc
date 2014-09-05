<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnumEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Reg.EnumEdit" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {

            Ext.create('PIC.view.setup.reg.enum.EnumEdit');
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>


