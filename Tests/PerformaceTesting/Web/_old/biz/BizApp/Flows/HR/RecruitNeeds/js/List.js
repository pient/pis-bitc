
var StatusEnum = { New: '新建', AdminSpecialist: '打回', FunctionalManager: '职能经理审批', AdminManager: '行政经理审批', CEO: '董事总经理审批', FinanceSupervisor: '财务主管审批', Accountant: '财务记账', Cashier: '财务报销', Completed: '完成' };

var FlowWinParams = { url: "Flow.aspx", style: { width: 700, height: 600} };
var DiagramWinParams = { url: "FlowPicView.aspx", style: { width: 880, height: 700} };

var ReimbWin = null;

Ext.define('PIC.RecruitNeedsGridPanel', {
    extend: 'PIC.grid.ExtPageGridPanel',
    alias: 'widget.picrecruitneedsgridpanel',
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
            title: '招聘需求申请',
            formparams: { url: "Flow.aspx", style: { width: 700, height: 600} }
        }, config);

        config.tlitems = [{ bttype: 'add', text: '新申请' }, 'edit', 'delete', '-', {
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
            root: "DataList",
            fields: [
			{ name: 'InstanceID' },
			{ name: 'FormID' },
			{ name: 'FormCode' },
			{ name: 'Data' },
			{ name: 'Tag' },
			{ name: 'Status' },
			{ name: 'CreatorId' },
			{ name: 'CreatorName' },
			{ name: 'CreatedDate' },
			{ name: 'LastModifiedDate' }
			]
        });

        config.columns = [
            { id: 'col_Id', dataIndex: 'InstanceID', header: '标识', hidden: true },
            { id: 'col_Index', dataIndex: 'Index', xtype: 'picrownumberer', width: 25 },
			{ id: 'col_Action', header: '测试', btnparams: { width: 50, handler: "PIC.ReimbGridPanel.onGridBtnClick" }, width: 70, align: 'center' },
			{ id: 'col_FormCode', dataIndex: 'FormCode', header: '编号', juncqry: true, width: 100, sortable: true },
			{ id: 'col_Title', dataIndex: 'Data', header: '标题', formlink: true, width: 200, sortable: true },
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

