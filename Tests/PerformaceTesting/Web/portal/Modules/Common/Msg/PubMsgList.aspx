
<%@ Page Title="公共消息列表" Language="C#" AutoEventWireup="true" CodeBehind="PubMsgList.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Msg.PubMsgList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.com.msg.PubMsgList');
    }
    
    </script>

</asp:Content>

