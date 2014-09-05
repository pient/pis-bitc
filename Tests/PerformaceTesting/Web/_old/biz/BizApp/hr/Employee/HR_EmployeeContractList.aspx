
<%@ Page Title="标题" Language="C#" AutoEventWireup="true" CodeBehind="HR_EmployeeContractList.aspx.cs" Inherits="PIC.Biz.Web.HR_EmployeeContractList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
    var EditPageUrl = "HR_EmployeeContractEdit.aspx";
    
    var store, myData;
    var pgBar, schBar, tlBar, titPanel, grid, viewport;

    function onPgLoad() {
        setPgUI();
    }

    function setPgUI() {

        // 表格数据
        myData = {
            total: PICSearchCrit["RecordCount"],
            records: PICState["HR_EmployeeContractList"] || []
        };

        // 表格数据源
        store = new Ext.ux.data.PICJsonStore({
            dsname: 'HR_EmployeeContractList',
            idProperty: 'Id',
            data: myData,
            fields: [
			{ name: 'Id' },
			{ name: 'EmployeeId' },
			{ name: 'Type' },
			{ name: 'Code' },
			{ name: 'Status' },
			{ name: 'BeginDate' },
			{ name: 'EndDate' },
			{ name: 'TrialBeginDate' },
			{ name: 'TrialEndDate' },
			{ name: 'JobContent' },
			{ name: 'Laborage' },
			{ name: 'Bonus' },
			{ name: 'LastModifiedDate' },
			{ name: 'CreatedDate' },
			{ name: 'CreatorId' },
			{ name: 'CreatorName' }
			]
        });

        // 分页栏
        pgBar = new Ext.ux.PICPagingToolbar({
            pageSize: PICSearchCrit["PageSize"],
            store: store
        });

        // 搜索栏
        schBar = new Ext.ux.PICSchPanel({
			store: store,
            items: [
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '编码', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '创建人', id: 'CreatorName', schopts: { qryopts: "{ mode: 'Like', field: 'CreatorName' }"} }]
            });

            // 工具栏
            tlBar = new Ext.ux.PICToolbar({
                items: [{
                    text: '添加',
                    iconCls: 'pic-icon-add',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "c", EditWinStyle);
                    }
                }, {
                    text: '修改',
                    iconCls: 'pic-icon-edit',
                    handler: function() {
                        ExtOpenGridEditWin(grid, EditPageUrl, "u", EditWinStyle);
                    }
                }, {
                    text: '删除',
                    iconCls: 'pic-icon-delete',
                    handler: function() {
						var recs = grid.getSelectionModel().getSelections();
						if (!recs || recs.length <= 0) {
							PICDlg.show("请先选择要删除的记录！");
							return;
						}
						
                        if (confirm("确定删除所选记录？")) {
                            ExtBatchOperate('batchdelete', recs, null, null, onExecuted);
                        }
                    }
                }, '-', { 
					text: '导出Excel', 
					iconCls: 'pic-icon-xls', 
					handler: function() {
                        ExtGridExportExcel(grid, { store: null, title: '标题' });
                    }
                }, '->', { text: '查询:' },
                new Ext.app.PICSearchField({ store: store, schbutton: true, qryopts: "{ type: 'fulltext' }" }),
                '-',
                {
                    text: '复杂查询',
                    iconCls: 'pic-icon-search',
                    handler: function() {
                        schBar.toggleCollapse(false);

                        setTimeout("viewport.doLayout()", 50);
                    }
}]
                });

                // 工具标题栏
                titPanel = new Ext.ux.PICPanel({
                    tbar: tlBar,
                    items: [schBar]
                });

                // 表格面板
                grid = new Ext.ux.grid.PICGridPanel({
                    store: store,
                    region: 'center',
                    autoExpandColumn: 'Name',
                    columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Code', dataIndex: 'Code', header: '编号', width: 100, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '名称', linkparams: { url: EditPageUrl, style: EditWinStyle }, width: 100, sortable: true },
					{ id: 'CreatorName', dataIndex: 'CreatorName', header: '创建人', width: 100,  sortable: true },
					{ id: 'CreatedDate', dataIndex: 'CreatedDate', header: '创建日期', width: 100, renderer: ExtGridDateOnlyRender,  sortable: true }
                    ],
                    bbar: pgBar,
                    tbar: titPanel
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
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>标题</h1></div>
</asp:Content>


