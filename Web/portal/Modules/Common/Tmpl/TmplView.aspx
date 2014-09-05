<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="TmplView.aspx.cs" Inherits="PIC.Portal.Web.Modules.Common.Tmpl.TmplView" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    function onPgLoad() {
        renderTmplHtml();
    }

    function renderTmplHtml() {
        var tmplObj = PICState["Template"];
        var renderStr = PICState["RenderStr"];

        var jsonObj = $.getJsonObj(renderStr);

        if (jsonObj) {
            renderStr = $.formatJson(jsonObj);
        }

        $("body").html('<textarea style="width:100%; height:100%" readonly=true>' + renderStr + '</textarea>');
    }

</script>

</asp:Content>


