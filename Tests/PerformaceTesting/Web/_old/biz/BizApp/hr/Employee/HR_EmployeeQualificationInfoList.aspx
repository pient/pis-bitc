
<%@ Page Title="人员资质" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeQualificationInfoList.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeQualificationInfoList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
    var EditPageUrl = "HR_EmployeeIntegEdit.aspx";

    var typeenum = {};
    var qtype;
    
    var store, grid, viewport;

    function onPgLoad() {
        typeenum = PICState["TypeEnum"];
        qtype = $.getQueryString({ ID: 'qtype' });

        setPgUI();
    }

    function setPgUI() {

        var fields = [
			{ name: 'EmployeeId' },
			{ name: 'UserId' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'Qualification' },
			{ name: 'Memo' },
			{ name: 'QualificationId' },
			{ name: 'DepartmentId' },
			{ name: 'DepartmentName' },
			{ name: 'SupervisorId' },
			{ name: 'SupervisorName' },
			{ name: 'Sex' },
			{ name: 'IDNumber' },
			{ name: 'Type' },
			{ name: 'Status' },
			{ name: 'Test' }
			];

        // 创建编辑列
        var columns = [{ id: 'EmployeeId', dataIndex: 'EmployeeId', header: '标识', hidden: true },
					{ id: 'Qualification', dataIndex: 'Qualification', header: 'Qualification', hidden: true, width: 100, sortable: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Code', dataIndex: 'Code', header: '工号', juncqry: true, width: 80, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '姓名', linkparams: { url: EditPageUrl, style: EditWinStyle }, juncqry: true, width: 80, sortable: true },
					{ id: 'DepartmentName', dataIndex: 'DepartmentName', header: '部门', juncqry: true, width: 100, sortable: true },
					{ id: 'Sex', dataIndex: 'Sex', header: '性别', enumdata: PICState['SexEnum'], width: 50, align: 'center', sortable: true },
					{ id: 'Type', dataIndex: 'Type', header: '类别', enumdata: PICState['EmployeeTypeEnum'], width: 80, align: 'center', sortable: true}];

        var chkcols = [];
        for (var code in typeenum) {
            var f = QFieldName(code);
            fields.push({ name: f });

            var col = new Ext.ux.grid.PICCheckColumn({ id: f, dataIndex: f, header: typeenum[code], tooltip: typeenum[code], exptrenderer: exportRender, DefaultValue: true, align: 'center' });

            chkcols.push(col);
            columns.push(col);
        }

        columns.push({ id: 'Memo', dataIndex: 'Memo', header: '备注', editor: { xtype: 'textfield' }, width: 100, menuDisabled: true, sortable: true });

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'HR_EmployeeQualificationInfoList',
            idProperty: 'EmployeeId',
            fields: fields,
            picread: function (rd, resp, dt) {
                if (dt) {
                    dt.reocrds = adjustData(dt.records);
                }
            }
        });

        grid = new Ext.ux.grid.PICEditorGridPanel({
            id: 'grid',
            plugins: chkcols,
            store: store,
            region: 'center',
            // title: '人员资质',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: [{ bttype: 'save', handler: DoSave }, '-', { xtype: 'picgridbutton', bttype: 'excel', title: '人员资质' }, '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '工号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '姓名', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '所属部门', id: 'DepartmentName', xtype: 'picgroupselector', params: { seltype: 'single' }, schopts: { qryopts: "{ mode: 'Like', field: 'DepartmentName' }"} },
                { fieldLabel: '性别', id: 'Sex', xtype: 'piccombo', enumdata: PICState['SexEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Sex' }"} },
                { fieldLabel: '类别', id: 'Type', xtype: 'piccombo', enumdata: PICState['EmployeeTypeEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Type' }"} }
            ],
            columns: columns,
            autoExpandColumn: 'Memo'
        });

        // 页面视图
        viewport = new Ext.ux.PICGridViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }

    // 数据适配
    function adjustData(recs) {
        var q = {};

        $.each(recs, function () {
            var rec = this;
            if (rec.Qualification) {
                q = $.getJsonObj(rec.Qualification);
                var qs = q[qtype] || {};

                for (var code in typeenum) {
                    rec[QFieldName(code)] = ($.inArray(code, qs) >= 0);
                }
            }
        });

        return recs;
    }

    function DoSave() {
        var dt = [];
        var recs = store.getModifiedRecords();

        $.each(recs, function () {
            dt.push($.getJsonString(FormatQData(this.data)));
        });

        $.ajaxExec('save', { qtype: qtype, "data": dt }, onExecuted);
    }

    function QFieldName(code) {
        return "Q_" + code;
    }

    function IsQField(field) {
        return (field.dataIndex && field.dataIndex.indexOf("Q_") == 0);
    }

    function FormatQData(rec) {
        var q = $.getJsonObj(rec.Qualification) || {};
        var subq = [];

        for (var code in typeenum) {
            if (rec[QFieldName(code)] == true) {
                subq.push(code);
            }
        }
        
        q[qtype] = subq;

        rec.Qualification = $.getJsonString(q);

        return rec;
    }

    function exportRender(v, p, rec) {
        var rtn = v;

        if (IsQField(this)) {
            rtn = v ? "√ " : "";
        }

        return rtn;
    }

    // 提交数据成功后
    function onExecuted() {
        store.commitChanges();
        store.removeAll();
        store.load();
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>人员资质</h1></div>
</asp:Content>


