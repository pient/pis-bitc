
<%@ Page Title="公共信息查询" Language="C#" AutoEventWireup="true" CodeBehind="OA_PublicInfoQry.aspx.cs" Inherits="PIC.Biz.Web.OA_PublicInfoQry" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">
    var EditWinStyle = CenterWin("width=700,height=650,scrollbars=yes");
    var EditPageUrl = "OA_PublicInfoEdit.aspx";

    var store, grid, viewport;
    
    var type;

    function onPgLoad() {        
        setPgUI();
    }

    function setPgUI() {

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'OA_PublicInfoList',
            idProperty: 'Id',
            fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'Title' },
			{ name: 'Type' },
			{ name: 'Keywords' },
			{ name: 'AuthorId' },
			{ name: 'AuthorName' },
			{ name: 'Content' },
			{ name: 'Grade' },
			{ name: 'PublisherId' },
			{ name: 'PublisherName' },
			{ name: 'GroupId' },
			{ name: 'GroupName' },
			{ name: 'Status' },
			{ name: 'Clicks' },
			{ name: 'IsPopup' },
			{ name: 'PopupEndDate' },
			{ name: 'PublishDate' },
			{ name: 'ExpireDate' },
			{ name: 'Picture' },
			{ name: 'Attachment' },
			{ name: 'LastModifiedDate' },
			{ name: 'CreatedDate' }
			]
        });

        grid = new Ext.ux.grid.PICGridPanel({
            id: 'grid',
            xtype: 'picgridpanel',
            store: store,
            region: 'center',
            title: '公共信息查询',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '栏目名称', name: 'Type', xtype: 'piccombo', enumdata: PICState['TypeEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Type' }"} },
                { fieldLabel: '标题', id: 'Title', schopts: { qryopts: "{ mode: 'Like', field: 'Title' }"} },
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '状态', name: 'Status', xtype: 'piccombo', enumdata: PICState['StatusEnum'], schopts: { qryopts: "{ mode: 'Eq', field: 'Status' }"} },
                { fieldLabel: '发布时间', id: 'PublishDateFrom', xtype: 'datefield', vtype: 'daterange', endDateField: 'PublishDateTo', schopts: { qryopts: "{ mode: 'GreaterThanEqual', datatype:'Date', field: 'PublishDate' }"} },
                { fieldLabel: '至', id: 'PublishDateTo', xtype: 'datefield', vtype: 'daterange', startDateField: 'PublishDateFrom', schopts: { qryopts: "{ mode: 'LessThanEqual', datatype:'Date', field: 'PublishDate' }"} },
                { fieldLabel: '关键字', id: 'Keywords', schopts: { qryopts: "{ mode: 'Like', field: 'Keywords' }"} },
                { fieldLabel: '作者', id: 'AuthorName', schopts: { qryopts: "{ mode: 'Like', field: 'AuthorName' }"} }
            ],
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Code', header: '编号', juncqry: true, width: 100, sortable: true, dataIndex: 'Code' },
					{ id: 'Title', header: '标题', juncqry: true, width: 100, linkparams: { url: EditPageUrl, style: EditWinStyle }, sortable: true, dataIndex: 'Title' },
					{ id: 'AuthorName', header: '作者', juncqry: true, width: 100, sortable: true, dataIndex: 'AuthorName' },
					{ id: 'Clicks', header: '点击数', width: 100, sortable: true, dataIndex: 'Clicks' },
					{ id: 'Status', header: '状态', width: 100, enumdata: PICState['StatusEnum'], sortable: true, dataIndex: 'Status' },
					{ id: 'IsPopup', header: '是否弹出', width: 100, enumdata: PICState['BooleanEnum'], sortable: true, dataIndex: 'IsPopup' },
					{ id: 'PublishDate', header: '发布时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true, dataIndex: 'PublishDate' },
					{ id: 'CreatedDate', header: '创建时间', width: 100, renderer: ExtGridDateOnlyRender, sortable: true, dataIndex: 'CreatedDate' }
                    ],
            autoExpandColumn: 'Title',
            autoExpandMin: 100
        });

        // 页面视图
        viewport = new Ext.ux.PICGridViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });

        // grid.collapseSchPanel();
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>标题</h1></div>
</asp:Content>
