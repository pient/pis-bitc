
<%@ Page Title="资产登记" Language="C#" AutoEventWireup="true" CodeBehind="OA_PropertyList.aspx.cs" Inherits="PIC.Biz.Web.OA_PropertyList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=650,height=400,scrollbars=yes");
    var EditPageUrl = "OA_PropertyEdit.aspx";
    
    var store, grid, viewport;

    function onPgLoad() {
        setPgUI();
    }

    function setPgUI() {

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'OA_PropertyList',
            idProperty: 'Id',
            fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'Type' },
			{ name: 'Spec' },
			{ name: 'Price' },
			{ name: 'Status' },
			{ name: 'Supplier' },
			{ name: 'Contact' },
			{ name: 'ContactTel' },
			{ name: 'Description' },
			{ name: 'CreatorId' },
			{ name: 'CreatorName' },
			{ name: 'CreatedDate' }
			]
        });

        grid = new Ext.ux.grid.PICGridPanel({
            id: 'grid',
            xtype: 'picgridpanel',
            store: store,
            region: 'center',
            title: '资产登记',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['add', 'edit', 'delete', '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '供应商', id: 'Supplier', schopts: { qryopts: "{ mode: 'Like', field: 'Supplier' }"} },
                { fieldLabel: '类型', id: 'Type', xtype: 'piccombo', enumdata: PICState['TypeEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Type' }"} },
                { fieldLabel: '状态', id: 'Status', xtype: 'piccombo', enumdata: PICState['StatusEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Status' }"} },
                { fieldLabel: '登记人', id: 'CreatorName', schopts: { qryopts: "{ mode: 'Like', field: 'CreatorName' }"} },
                { fieldLabel: '登记时间', id: 'CreatedDateFrom', xtype: 'datefield', vtype: 'daterange', endDateField: 'CreatedDateTo', schopts: { qryopts: "{ mode: 'GreaterThanEqual', datatype:'Date', field: 'CreatedDate' }"} },
                { fieldLabel: '至', id: 'CreatedDateTo', xtype: 'datefield', vtype: 'daterange', startDateField: 'CreatedDateFrom', schopts: { qryopts: "{ mode: 'LessThanEqual', datatype:'Date', field: 'CreatedDate' }"} }
            ],
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 80, sortable: true },
					{ id: 'Name', dataIndex: 'Name', linkparams: { url: EditPageUrl, style: EditWinStyle }, header: '名称', juncqry: true, width: 100, sortable: true },
					{ id: 'Spec', dataIndex: 'Spec', header: '规格', juncqry: true, width: 100, sortable: true },
					{ id: 'Price', dataIndex: 'Price', header: '总价', width: 100, sortable: true },
					{ id: 'Supplier', dataIndex: 'Supplier', header: '供应商', juncqry: true, width: 100, sortable: true },
					{ id: 'Type', dataIndex: 'Type', header: '类型', enumdata: PICState['TypeEnum'], width: 80, sortable: true },
					{ id: 'Status', dataIndex: 'Status', header: '状态', enumdata: PICState['StatusEnum'], width: 80, sortable: true },
					{ id: 'ContactTel', dataIndex: 'ContactTel', header: '联系电话', width: 100, sortable: true },
					{ id: 'CreatorName', dataIndex: 'CreatorName', header: '登记人', juncqry: true, width: 80, sortable: true },
					{ id: 'CreatedDate', dataIndex: 'CreatedDate', header: '登记日期', width: 100, sortable: true },
					{ id: 'Description', dataIndex: 'Description', header: '备注', juncqry: true, enumdata: PICState['TypeEnum'], width: 100, sortable: true }
                    ],
            autoExpandColumn: 'Description'
        });

        // 页面视图
        viewport = new Ext.ux.PICGridViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none"><h1>资产登记</h1></div>
</asp:Content>


