
var StatusEnum = { New: '新建', AdminSpecialist: '打回', FunctionalManager: '职能经理审批', AdminManager: '行政经理审批', CEO: '董事总经理审批', FinanceSupervisor: '财务主管审批', Accountant: '财务记账', Cashier: '财务报销', Completed: '完成' };

var FlowWinParams = { url: "OA_ReimbFlow.aspx", style: { width: 700, height: 600} };
var DiagramWinParams = { url: "FlowPicView.aspx", style: { width: 880, height: 700} };

var ReimbWin = null;

Ext.define('PIC.ReimbGridPanel', {
    extend: 'PIC.grid.ExtPageGridPanel',
    alias: 'widget.picreimbgridpanel',
    reimbWin: null,

    statics: {
        onGridBtnClick: function (colid, recid, val) {
            var rec = PICPgGrid.store.findRecord("Id", recid);
            alert(rec.get("Name"));
        }
    },

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            region: 'center',
            title: '行政（月结）费用报销',
            formparams: { url: "OA_ReimbFlow.aspx", style: { width: 700, height: 600} }
        }, config);

        config.tlitems = [{ bttype: 'add', text: '新报销' }, 'edit', 'delete', '-', {
            text: '流程图',
            iconCls: 'pic-icon-arrow-round',
            id: 'btnFlow',
            handler: function () {

            }
        }, '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'];

        config.schitems = [
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} }
            ];

        config.store = Ext.create('PIC.data.ExtPageGridStore', {
            root: "OA_ReimbursementList",
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

        config.columns = [
            { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
            { id: 'col_Index', dataIndex: 'Index', xtype: 'picrownumberer', width: 25 },
			{ id: 'col_Action', header: '测试', btnparams: { width: 50, handler: "PIC.ReimbGridPanel.onGridBtnClick" }, width: 70, align: 'center' },
			{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 100, sortable: true },
			{ id: 'col_Name', dataIndex: 'Name', header: '名称', formlink: true, juncqry: true, width: 200, sortable: true },
			{ id: 'col_Fee', dataIndex: 'Fee', header: '费用', width: 100, sortable: true },
			{ id: 'col_FeeDate', dataIndex: 'FeeDate', header: '日期', width: 100, sortable: true },
			{ id: 'col_Status', dataIndex: 'Status', header: '状态', enumdata: StatusEnum, width: 100, sortable: true },
			{ id: 'col_Comments', dataIndex: 'Comments', header: '注释', juncqry: true, width: 100, sortable: true },
			{ id: 'col_Attachments', dataIndex: 'Attachments', header: '附件', filelink: true, width: 150, sortable: true },
			{ id: 'col_CreatorName', dataIndex: 'CreatorName', header: '编制人', juncqry: true, width: 100, sortable: true },
			{ id: 'col_CreatedDate', dataIndex: 'CreatedDate', header: '编制日期', dateonly: true, width: 100, sortable: true }
        ];

        me.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});

