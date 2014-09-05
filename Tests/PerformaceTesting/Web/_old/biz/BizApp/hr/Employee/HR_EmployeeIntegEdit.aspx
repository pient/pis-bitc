<%@ Page Title="员工信息维护" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeIntegEdit.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeIntegEdit" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
<script type="text/javascript">
    var tabArr = [{ title: "基本信息", href: "HR_EmployeeEdit.aspx" },
    //{ title: "员工合同", href: "HR_EmployeeContractEdit.aspx" },
    {title: "员工经历", href: "HR_EmployeeCareerList.aspx" },
    { title: "职称", href: "HR_EmployeeTitleList.aspx" },
    { title: "从业资质", href: "HR_EmployeeJobTitleList.aspx"}];
    
    var viewport, tab;
    var eid, op;

    function onPgLoad() {
        eid = $.getQueryString({ "ID": "id" });
    
        setPgUI();
    }

    function setPgUI() {
        $.each(tabArr, function(i) {
            this.href = $.combineQueryUrl(this.href, { op: pgOperation, eid: eid });

            this.listeners = { activate: handleActivate };
            this.html = "<div style='display:none;'></div>";
        });

        tab = new Ext.ux.PICTabPanel({
            region: 'center',
            activeTab: 0,
            border: 0,
            width: document.body.offsetWidth - 5,
            items: tabArr
        });

        tpanel = {
            title: '员工信息维护',
            border: 0,
            height: 50,
            layout: 'border',
            region: 'north',
            items: [tab]
        }

        viewport = new Ext.ux.PICViewport({
            layout: 'border',
            items: [
                { xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 },
                    tpanel, {
                        region: 'center',
                        margins: '0 0 0 0',
                        cls: 'empty',
                        bodyStyle: 'background:#f1f1f1',
                        html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src=""></iframe>'
}]
        });

        handleActivate(tabArr[0]);
    }

    function handleActivate(tab) {
        if (document.getElementById("frameContent")) {
            document.getElementById("frameContent").contentWindow.location.href = tab.href;
        }
    }

</script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none"><h1>员工信息维护</h1></div>
</asp:Content>
