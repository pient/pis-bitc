
<%@ Page Title="公共消息列表" Language="C#" AutoEventWireup="true" CodeBehind="PubMsgListView.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Msg.PubMsgListView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.com.msg.PubMsgListView');
    }
    
    </script>

</asp:Content>

