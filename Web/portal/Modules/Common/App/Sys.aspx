<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Sys.aspx.cs" Inherits="PIC.Portal.Web.Default" %>

<%@ Import Namespace="PIC.Portal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadHolder" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyHolder" runat="server">
    <table>
        <tr>
            <td>当前在线用户数：</td>
            <td><%= PortalService.GetOnlineUserCount() %></td>
        </tr>
    </table>
</asp:Content>