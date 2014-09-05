<%@ Page Title="消息管理" Language="C#" AutoEventWireup="true" CodeBehind="MyMsg.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Msg.MyMsg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

    <script type="text/javascript">

        function onPgLoad() {
            Ext.create('PIC.view.com.msg.MyMsg');
        }

    </script>

</asp:Content>
