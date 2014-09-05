<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="FlowPreview.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Bpm.FlowPreview" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.com.bpm.FlowPreview');
        }

    </script>

</asp:Content>
