
<%@ Page Title="员工经历" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeCareerList.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeCareerList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=650,height=400,scrollbars=yes");
    var EditPageUrl = "HR_EmployeeCareerEdit.aspx";
    
    var store, grid, viewport;
    var eid;

    function onPgLoad() {
        eid = $.getQueryString({ "ID": "eid" });
        EditPageUrl = $.combineQueryUrl(EditPageUrl, { eid: eid });
    
        setPgUI();
    }

    function setPgUI() {

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'HR_EmployeeCareerList',
            idProperty: 'Id',
            loadargs: { eid: eid },
            fields: [
			{ name: 'Id' },
			{ name: 'EmployeeId' },
			{ name: 'Company' },
			{ name: 'InDate' },
			{ name: 'OutDate' },
			{ name: 'Department' },
			{ name: 'Position' },
			{ name: 'Description' },
			{ name: 'Achivement' },
			{ name: 'Status' },
			{ name: 'CreatedDate' },
			{ name: 'LastModifiedDate' },
			{ name: 'CreatorId' },
			{ name: 'CreatorName' }
			]
        });

        grid = new Ext.ux.grid.PICGridPanel({
            id: 'grid',
            xtype: 'picgridpanel',
            store: store,
            region: 'center',
            title: '员工经历',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['add', 'edit', 'delete', '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '编码', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '创建人', id: 'CreatorName', schopts: { qryopts: "{ mode: 'Like', field: 'CreatorName' }"} }
            ],
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Company', dataIndex: 'Company', header: '公司名称', linkparams: { url: EditPageUrl, style: EditWinStyle }, width: 100, sortable: true },
					{ id: 'Department', dataIndex: 'Department', header: '部门', width: 100, sortable: true },
					{ id: 'Position', dataIndex: 'Position', header: '职位', width: 100, sortable: true },
					{ id: 'InDate', dataIndex: 'InDate', header: '入职时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'OutDate', dataIndex: 'OutDate', header: '离职时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true },
					{ id: 'CreatorName', dataIndex: 'CreatorName', header: '创建人', width: 100, sortable: true },
					{ id: 'CreatedDate', dataIndex: 'CreatedDate', header: '创建日期', width: 100, renderer: ExtGridDateOnlyRender, sortable: true }
                    ],
            autoExpandColumn: 'Company'
        });

        // 页面视图
        viewport = new Ext.ux.PICViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>员工经历</h1></div>
</asp:Content>


