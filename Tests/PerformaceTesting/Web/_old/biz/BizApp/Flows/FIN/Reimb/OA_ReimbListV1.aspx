
<%@ Page Title="行政（月结）费用报销" Language="C#" AutoEventWireup="true" CodeBehind="OA_ReimbListV1.aspx.cs" Inherits="PIC.Biz.Web.Reimbursement.OA_ReimbListV1" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadHolder" runat="server">

<script type="text/javascript">

    var StatusEnum = { New: '新建', AdminSpecialist: '打回', FunctionalManager: '职能经理审批', AdminManager: '行政经理审批', CEO: '董事总经理审批', FinanceSupervisor: '财务主管审批', Accountant: '财务记账', Cashier: '财务报销', Completed: '完成' };

    var EditWinStyle = CenterWin({ width: 700, height: 600, scrollbars: "yes" });
    var EditPageUrl = "OA_ReimbFlow.aspx";

    var PicWinStyle = CenterWin({ width: 880, height: 700, scrollbars: "yes" });
    var PicPageUrl = "FlowPicView.aspx";
    
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
            title: '行政（月结）费用报销',
            editpageurl: EditPageUrl,
            editwinstyle: EditWinStyle,
            tlitems: [{ bttype: 'add', text: '新报销' },
                { bttype: 'edit', text: '操作' }, 
                { bttype: 'delete', handler: onDelete }, '-', {
                iconCls: 'pic-icon-arrow-round',
                text: '流程图',
                id: 'btnFlow',
                handler: function () {
                    OpenWin(PicPageUrl, '_blank', PicWinStyle);
                }
            }, 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
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
					{ id: 'Fee', dataIndex: 'Fee', header: '费用', width: 100, sortable: true },
					{ id: 'FeeDate', dataIndex: 'FeeDate', header: '日期', juncqry: true, width: 100, sortable: true },
					{ id: 'Status', dataIndex: 'Status', header: '状态', enumdata: StatusEnum, juncqry: true, width: 100, sortable: true },
					{ id: 'Comments', dataIndex: 'Comments', header: '注释', juncqry: true, width: 100, sortable: true },
					{ id: 'CreatorName', dataIndex: 'CreatorName', header: '编制人', juncqry: true, width: 100, sortable: true },
					{ id: 'CreatedDate', dataIndex: 'CreatedDate', header: '编制日期', juncqry: true, width: 100, sortable: true }
                    ],
            autoExpandColumn: 'Comments'
        });

        grid.getSelectionModel().on("rowselect", function (sm, ridx, e) {
            var rec = store.getAt(ridx);
            var status = rec.get('Status');
            var btn_edit = grid.getToolButton({ bttype: 'edit' });
            var btn_del = grid.getToolButton({ bttype: 'delete' });

            if (!status || 'New'.equals(status)) {
                btn_edit.setDisabled(false);
                btn_del.setDisabled(false);
            } else {
                //btn_edit.setDisabled(true);
                //btn_del.setDisabled(true);
            }
        });

        // 页面视图
        viewport = new Ext.ux.PICGridViewport({
            items: [{ xtype: 'box', region: 'north', applyTo: 'pic-page-header', height: 30 }, grid]
        });
    }

    function onDelete() {
        var recs = grid.getSelectionModel().getSelections();
        var inc_started = false;    // 是否包含已启动的记录
        var t_recs = [];

        if (!recs || recs.length <= 0) {
            PICDlg.show("请先选择要删除的记录！");
            return;
        }

        $.each(recs, function () {
            var status = this.get('Status');
            if (status && !'New'.equals(status)) {
                inc_started = true;
            } else {
                t_recs.push(this);
            }
        });

        if (!t_recs || t_recs.length <= 0) {
            PICDlg.show("所选择记录中不包含未提交的报销！");
            return;
        }

        if (confirm("确定删除所选记录？已提交的记录将被忽略")) {
            ExtBatchOperate('batchdelete', t_recs, null, null, function () { store.reload(); });
        }
    }
    
    </script>

</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="BodyHolder" runat="server">
    <div id="pic-page-header" class="pic-page-header" style="display:none;"><h1>行政（月结）费用报销</h1></div>
</asp:Content>


