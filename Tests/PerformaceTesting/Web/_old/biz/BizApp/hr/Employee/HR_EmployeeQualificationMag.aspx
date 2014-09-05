<%@ Page Title="人员资质" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeQualificationMag.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeQualificationMag" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">
    
<script type="text/javascript">
    var ListPageUrl = "HR_EmployeeQualificationInfoList.aspx";

    var tabArr = [];
    var typeenum = {};
    
    var viewport, tab;

    function onPgLoad() {
        typeenum = PICState['TypeEnum'] || {};

        for (var key in typeenum) {
            var tab = { qtype: key, title: typeenum[key] };
            tab.href = $.combineQueryUrl(ListPageUrl, { op: pgOperation, qtype: tab.qtype });
            tab.listeners = { activate: handleActivate };
            tab.html = "<div style='display:none;'></div>";
            tabArr.push(tab);
        }

        setPgUI();
    }

    function setPgUI() {

        tab = new Ext.ux.PICTabPanel({
            region: 'center',
            activeTab: 0,
            border: 0,
            width: document.body.offsetWidth - 5,
            items: tabArr
        });

        tpanel = {
            title: '人员资质',
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
    <div id="pic-page-header" class="pic-page-header" style="display:none"><h1>人员资质</h1></div>
</asp:Content>
