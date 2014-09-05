<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Doc.Upload" %>
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

    <script src="/portal/js/lib/swfupload/swfupload.js"></script>
    <script src="/portal/js/lib/swfupload/plugins/swfupload.speed.js"></script>
    <script src="/portal/js/lib/swfupload/plugins/swfupload.queue.js"></script>

    <script type="text/javascript">

        function onPgLoad() {

            Ext.create('PIC.view.com.doc.Uploador');

        }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>
