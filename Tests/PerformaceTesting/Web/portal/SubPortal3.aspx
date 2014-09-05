<%@ Page Title="ÏîÄ¿Ê×Ò³" Language="C#" AutoEventWireup="true" CodeBehind="SubPortal3.aspx.cs" Inherits="PIC.Portal.Web.SubPortal3" %>
    
<%@ OutputCache Duration="1" VaryByParam="None" %>

<asp:Content ID="HeadHolder" ContentPlaceHolderID="HeadHolder" runat="server">

    <style type="text/css">
    
        .x-grid-row 
        {
            border:0px;
            height:25px;
        }
    
        .x-grid-row .x-grid-cell
        {
            border: 1px solid #fafafa;
        }
    
        .x-grid-cell
        {
            background-color:#fafafa !important;
        }
    
        .x-grid-body
        {
            padding-top:0px;
            background-color:#fafafa !important;
        }
    
    </style>
    
    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.portal.default.SubViewport');

            // subFrameContent.location.href = curMdl["Url"] || "about:blank";
        }

    </script>
    
</asp:Content>
