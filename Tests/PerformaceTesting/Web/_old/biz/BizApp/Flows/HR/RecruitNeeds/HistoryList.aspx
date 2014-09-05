
<%@ Page Title="招聘需求申请" Language="C#" AutoEventWireup="true" CodeBehind="HistoryList.aspx.cs" Inherits="PIC.Biz.Web.RecruitNeeds.HistoryList" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    var EditWinStyle = CenterWin("width=650,height=600,scrollbars=yes");
    var EditPageUrl = "OA_ReimbursementFlow.aspx";
    
    var store, grid, viewport;

    function onPgLoad() {
        setPgUI();
    }

    function setPgUI() {

        // 表格数据源
        store = new Ext.ux.PICGridStore({
            dsname: 'OA_ReimbursementList',
            idProperty: 'Id',
            fields: [
			{ name: 'Id' },
			{ name: 'Code' },
			{ name: 'Name' },
			{ name: 'Type' },
			{ name: 'Category' },
			{ name: 'Fee' },
			{ name: 'FeeDate' },
			{ name: 'Status' },
			{ name: 'Comments' },
			{ name: 'Attachments' },
			{ name: 'Tag' },
			{ name: 'Items' },
			{ name: 'LastModifiedDate' },
			{ name: 'CreatorId' },
			{ name: 'CreatorName' },
			{ name: 'CreatedDate' }
			]
        });

        grid = new Ext.ux.grid.PICGridPanel({
            id: 'grid',
            store: store,
            region: 'center',
            title: '行政（月结）费用报销 - <span style="font-size: 14px">上月份报销费用&nbsp;<b style="color:red;">(' + (PICState["LastMonthExpense"] || "0.00") + ')</b>，去年本月度报销费用&nbsp;<b style="color:red">(' + (PICState["LastYearExpense"] || "0.00") + ')</b></font>',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: ['excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} }
            ],
            columns: [
                    { id: 'Id', dataIndex: 'Id', header: '标识', hidden: true },
                    new Ext.ux.grid.PICRowNumberer(),
                    new Ext.ux.grid.PICCheckboxSelectionModel(),
					{ id: 'Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 100, sortable: true },
					{ id: 'Name', dataIndex: 'Name', header: '名称', juncqry: true, linkparams: { url: EditPageUrl, style: EditWinStyle }, width: 200, sortable: true },
					{ id: 'Fee', dataIndex: 'Fee', header: '费用', juncqry: true, width: 100, sortable: true },
					{ id: 'FeeDate', dataIndex: 'FeeDate', header: '日期', juncqry: true, width: 100, sortable: true },
					{ id: 'Status', dataIndex: 'Status', header: '状态', juncqry: true, width: 100, sortable: true },
					{ id: 'Comments', dataIndex: 'Comments', header: '注释', juncqry: true, width: 100, sortable: true },
					{ id: 'CreatorName', dataIndex: 'CreatorName', header: '编制人', juncqry: true, width: 100, sortable: true },
					{ id: 'CreatedDate', dataIndex: 'CreatedDate', header: '编制日期', juncqry: true, width: 100, sortable: true }
                    ],
            autoExpandColumn: 'Comments'
        });

        // 页面视图
        viewport = new Ext.ux.PICGridViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>行政（月结）费用报销</h1></div>
</asp:Content>


