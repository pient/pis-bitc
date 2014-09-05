
<%@ Page Title="企业黄页" Language="C#" AutoEventWireup="true" CodeBehind="OA_YellowPageList.aspx.cs" Inherits="PIC.Biz.Web.OA_YellowPageList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
    var EditPageUrl = "OA_YellowPageEdit.aspx";
    
    var store, grid, viewport;

    function onPgLoad() {
        setPgUI();
    }

    function setPgUI() {

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'OA_YellowPageList',
            idProperty: 'Id',
            fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'ParentID' },
			{ name: 'Path' },
			{ name: 'PathLevel' },
			{ name: 'IsLeaf' },
			{ name: 'SortIndex' },
			{ name: 'Phone' },
			{ name: 'Mobile' },
			{ name: 'Email' },
			{ name: 'Fax' },
			{ name: 'Position' },
			{ name: 'Description' },
			{ name: 'Tag' },
			{ name: 'LastModifiedDate' },
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
            title: '企业黄页',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['add', 'edit', 'delete', '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} }
            ],
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Code', dataIndex: 'Code', header: '编号', width: 100, sortable: true },
					{ id: 'Name', dataIndex: 'Name', linkparams: { url: EditPageUrl, style: EditWinStyle }, header: '名称', width: 100, sortable: true },
					{ id: 'ParentID', dataIndex: 'ParentID', header: 'ParentID', width: 100, sortable: true },
					{ id: 'Path', dataIndex: 'Path', header: 'Path', width: 100, sortable: true },
					{ id: 'PathLevel', dataIndex: 'PathLevel', header: 'PathLevel', width: 100, sortable: true },
					{ id: 'IsLeaf', dataIndex: 'IsLeaf', header: 'IsLeaf', width: 100, sortable: true },
					{ id: 'SortIndex', dataIndex: 'SortIndex', header: 'SortIndex', width: 100, sortable: true },
					{ id: 'Phone', dataIndex: 'Phone', header: 'Phone', width: 100, sortable: true },
					{ id: 'Mobile', dataIndex: 'Mobile', header: 'Mobile', width: 100, sortable: true },
					{ id: 'Email', dataIndex: 'Email', header: 'Email', width: 100, sortable: true },
					{ id: 'Fax', dataIndex: 'Fax', header: 'Fax', width: 100, sortable: true },
					{ id: 'Position', dataIndex: 'Position', header: 'Position', width: 100, sortable: true },
					{ id: 'Description', dataIndex: 'Description', header: '描述', width: 100, sortable: true },
					{ id: 'Tag', dataIndex: 'Tag', header: 'Tag', width: 100, sortable: true },
					{ id: 'LastModifiedDate', dataIndex: 'LastModifiedDate', header: 'LastModifiedDate', width: 100, sortable: true },
					{ id: 'CreatedDate', dataIndex: 'CreatedDate', header: 'CreatedDate', width: 100, sortable: true },
					{ id: 'CreatorId', dataIndex: 'CreatorId', header: 'CreatorId', width: 100, sortable: true },
					{ id: 'CreatorName', dataIndex: 'CreatorName', header: 'CreatorName', width: 100, sortable: true }
                    ],
            autoExpandColumn: 'Name'
        });

        // 页面视图
        viewport = new Ext.ux.PICGridViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>企业黄页</h1></div>
</asp:Content>


