
<%@ Page Title="消息列表" Language="C#" AutoEventWireup="true" CodeBehind="MsgList.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Msg.MsgList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        Ext.create('PIC.view.com.msg.MsgList');
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
</asp:Content>


