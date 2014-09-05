
<%@ Page Title="员工信息" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeList.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=750,height=600,scrollbars=yes");
    var EditPageUrl = "HR_EmployeeEdit.aspx";

    var IntegEditPageUrl = "HR_EmployeeIntegEdit.aspx";
    var IntegEditWinStyle = CenterWin("width=750,height=600,scrollbars=yes");
    
    var store, grid, viewport;

    function onPgLoad() {
        setPgUI();
    }

    function setPgUI() {
        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'HR_EmployeeList',
            idProperty: 'Id',
            fields: [
			{ name: 'Id' },
			{ name: 'UserId' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'Status' },
			{ name: 'Type' },
			{ name: 'UsedName' },
			{ name: 'IDNumber' },
			{ name: 'Birthday' },
			{ name: 'Sex' },
			{ name: 'People' },
			{ name: 'Email' },
			{ name: 'Phone' },
			{ name: 'Phone2' },
			{ name: 'HomePhone' },
			{ name: 'Mobile' },
			{ name: 'FileCode' },
			{ name: 'AttendanceType' },
			{ name: 'Photo' },
			{ name: 'Region' },
			{ name: 'Post' },
			{ name: 'NativePlace' },
			{ name: 'ContactAddress' },
			{ name: 'HouseholdAddress' },
			{ name: 'SupervisorId' },
			{ name: 'SupervisorName' },
			{ name: 'DepartmentId' },
			{ name: 'DepartmentName' },
			{ name: 'MajorCode' },
			{ name: 'MajorName' },
			{ name: 'ContractStartDate' },
			{ name: 'ContractEndDate' },
			{ name: 'ProbationStartDate' },
			{ name: 'ProbationEndDate' },
			{ name: 'LastModifiedDate' },
			{ name: 'RegularizedDate' },
			{ name: 'InauguralDate' },
			{ name: 'IDPhotoFront' },
			{ name: 'IDPhotoBack' },
			{ name: 'Signature' },
			{ name: 'PoliticalStatus' },
			{ name: 'MarriageStatus' },
			{ name: 'SpouseName' },
			{ name: 'Memo' },
			{ name: 'DutyForShow' },
			{ name: 'PostQualification' },
			{ name: 'School' },
			{ name: 'SchoolMajor' },
			{ name: 'GraduatedDate' },
			{ name: 'EducationHistory' },
			{ name: 'Title' },
			{ name: 'OrigDepartment' },
			{ name: 'InnerTitle' },
			{ name: 'TrailPay' },
			{ name: 'FormalPay' },
			{ name: 'PayAdjustment' },
			{ name: 'GeneralSafety' },
			{ name: 'Safety' },
			{ name: 'SafetyCode' },
			{ name: 'Fund' },
			{ name: 'FundCode' },
			{ name: 'ManualLabor' },
			{ name: 'ResidenceValid' },
			{ name: 'HireStatus' },
			{ name: 'Tag' },
			{ name: 'TagA' },
			{ name: 'TagB' },
			{ name: 'TagC' },
			{ name: 'CreatedDate' },
			{ name: 'CreatorId' },
			{ name: 'CreatorName' }
			]
        });

        grid = new Ext.ux.grid.PICGridPanel({
            id: 'grid',
            xtype: 'picgridpanel',
            store: store,
            region: 'center',
            title: '员工信息',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['add', {
                bttype: 'edit',
                handler: function () {
                    ExtOpenGridEditWin(grid, IntegEditPageUrl, "u", IntegEditWinStyle);
                }
            }, 'delete', '-', {
                iconCls: 'pic-icon-xls',
                text: '导出Excel',
                handler: function () {
                    ExtGridExportExcel(grid)
                }
            }, '-', {
                text: '更多',
                iconCls: 'pic-icon-cog',
                menu: { items: [{
                    iconCls: 'pic-icon-arrow-recycle',
                    text: '同步人员信息',
                    handler: function () {
                        var recs = grid.getSelectionModel().getSelections();
                        if (!recs || recs.length <= 0) {
                            PICDlg.show("请先选择要同步的人员信息！");
                            return;
                        }

                        if (confirm("确定同步所选人员信息？")) {
                            ExtBatchOperate('sync', recs, {}, null, function () { PICDlg.show("数据同步完成。"); });
                        }
                    }
                }/*, { iconCls: 'pic-icon-db-import',
                    text: '数据导入',
                    handler: function () {
                        var filter = "Excel文件 (*.xls)|*.xls";

                        OpenUploadWin({ Filter: filter, IsSingle: true }, function () {
                            var val = this;
                            if (val) {
                                var ffid = val.toString().trimEnd(',');

                                var smask = new Ext.LoadMask(document.body, { msg: "数据导入..." });

                                $.ajaxExec('import', { ffid: ffid, code: 'T_HR_EMPLOYEE' }, function (args) {
                                    PICDlg.show(args.data.message);
                                    smask.hide();
                                });
                            }
                        })
                    }
                }, '-', { text: '数据导入模板',
                    handler: function () {
                        DownloadTemplate("T_HR_EMPLOYEE");
                    }
                }*/]
                }
            }, '->', 'schfield', '-', 'cquery', '-', 'help'],
            schpanel: {
                schcols: 4,
                schitems: [
                { fieldLabel: '工号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '姓名', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '性别', id: 'Sex', xtype: 'piccombo', enumdata: PICState['SexEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Sex' }"} },
                { fieldLabel: '所属部门', id: 'DepartmentName', xtype: 'picgroupselector', params: { seltype: 'single' }, schopts: { qryopts: "{ mode: 'Like', field: 'DepartmentName' }"} },
                { fieldLabel: '合同终止日期', id: 'ContractStartDateFrom', xtype: 'datefield', vtype: 'daterange', endDateField: 'ContractStartDateTo', schopts: { qryopts: "{ mode: 'GreaterThanEqual', datatype:'Date', field: 'ContractStartDate' }"} },
                { fieldLabel: '至', id: 'ContractStartDateTo', xtype: 'datefield', vtype: 'daterange', startDateField: 'ContractStartDateFrom', schopts: { qryopts: "{ mode: 'LessThanEqual', datatype:'Date', field: 'ContractStartDate' }"} },
                { fieldLabel: '报到日期', id: 'InauguralDateFrom', xtype: 'datefield', vtype: 'daterange', endDateField: 'InauguralDateTo', schopts: { qryopts: "{ mode: 'GreaterThanEqual', datatype:'Date', field: 'InauguralDate' }"} },
                { fieldLabel: '至', id: 'InauguralDateTo', xtype: 'datefield', vtype: 'daterange', startDateField: 'InauguralDateFrom', schopts: { qryopts: "{ mode: 'LessThanEqual', datatype:'Date', field: 'InauguralDate' }"} },
                { fieldLabel: '转正日期', id: 'RegularizedDateFrom', xtype: 'datefield', vtype: 'daterange', endDateField: 'RegularizedDateTo', schopts: { qryopts: "{ mode: 'GreaterThanEqual', datatype:'Date', field: 'RegularizedDate' }"} },
                { fieldLabel: '至', id: 'RegularizedDateTo', xtype: 'datefield', vtype: 'daterange', startDateField: 'RegularizedDateFrom', schopts: { qryopts: "{ mode: 'LessThanEqual', datatype:'Date', field: 'RegularizedDate' }"} },
                { fieldLabel: '出生年月', id: 'BirthdayFrom', xtype: 'datefield', vtype: 'daterange', endDateField: 'BirthdayTo', schopts: { qryopts: "{ mode: 'GreaterThanEqual', datatype:'Date', field: 'Birthday' }"} },
                { fieldLabel: '至', id: 'BirthdayTo', xtype: 'datefield', vtype: 'daterange', startDateField: 'BirthdayFrom', schopts: { qryopts: "{ mode: 'LessThanEqual', datatype:'Date', field: 'Birthday' }"} },
                { fieldLabel: '学历', id: 'EducationHistory', xtype: 'piccombo', enumdata: PICState['EducationEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'EducationHistory' }"} },
                { fieldLabel: '职称', id: 'InnerTitle', xtype: 'piccombo', enumdata: PICState['InnerTitleEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'InnerTitle' }"} },
                { fieldLabel: '所学专业', id: 'SchoolMajor', schopts: { qryopts: "{ mode: 'Like', field: 'SchoolMajor' }"} }
            ]
            },
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Code', dataIndex: 'Code', header: '工号', juncqry: true, width: 80, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '姓名', linkparams: { url: IntegEditPageUrl, style: IntegEditWinStyle }, juncqry: true, width: 100, sortable: true },
					{ id: 'Sex', dataIndex: 'Sex', header: '性别', enumdata: PICState['SexEnum'], width: 100, sortable: true },
					{ id: 'DepartmentName', dataIndex: 'DepartmentName', header: '所属部门', width: 100, juncqry: true, sortable: true },
					{ id: 'Region', dataIndex: 'Region', header: '工作地点', enumdata: PICState['RegionEnum'], width: 100, sortable: true },
					{ id: 'IDNumber', dataIndex: 'IDNumber', header: '身份证号码', juncqry: true, width: 100, sortable: true },
					{ id: 'School', dataIndex: 'School', header: '毕业院校', juncqry: true, width: 100, sortable: true },
					{ id: 'SchoolMajor', dataIndex: 'SchoolMajor', header: '所学专业', juncqry: true, width: 100, sortable: true },
					{ id: 'GraduatedDate', dataIndex: 'GraduatedDate', header: '毕业时间', width: 100, sortable: true },
					{ id: 'EducationHistory', dataIndex: 'EducationHistory', header: '学历', enumdata: PICState['EducationEnum'], width: 100, sortable: true },
					{ id: 'InnerTitle', dataIndex: 'InnerTitle', header: '职称', enumdata: PICState['InnerTitleEnum'], width: 100, sortable: true },
					{ id: 'InauguralDate', dataIndex: 'InauguralDate', header: '报到日期', width: 100, sortable: true },
					{ id: 'OrigDepartment', dataIndex: 'OrigDepartment', header: '原工作单位', juncqry: true, width: 100, sortable: true },
					{ id: 'Birthday', dataIndex: 'Birthday', header: '出生年月', width: 100, sortable: true },
					{ id: 'Mobile', dataIndex: 'Mobile', header: '移动电话', juncqry: true, width: 100, sortable: true },
					{ id: 'Email', dataIndex: 'Email', header: '邮件', juncqry: true, width: 100, sortable: true },
					{ id: 'ContractStartDate', dataIndex: 'ContractStartDate', header: '合同开始日期', width: 100, sortable: true },
					{ id: 'ContractEndDate', dataIndex: 'ContractEndDate', header: '合同终止日期', width: 100, sortable: true }
                    ]
        });

        // 页面视图
        viewport = new Ext.ux.PICViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }
    
    // 提交数据成功后
    function onExecuted() {
        store.reload();
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>员工信息</h1></div>
</asp:Content>


