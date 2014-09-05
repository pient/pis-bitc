<%@ Page Title="模版配置编辑" Language="C#" AutoEventWireup="true" CodeBehind="TmplCfgEdit.aspx.cs" Inherits="PIC.Portal.Web.Modules.Setup.Dev.TmplCfgEdit" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.setup.dev.tmpl.TmplCfgEdit');
        }

    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>


