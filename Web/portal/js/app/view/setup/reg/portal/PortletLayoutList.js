Ext.define('PIC.view.setup.reg.portal.PortletLayoutList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.RegPortletSetupLayoutListPage',

    requires: [
        'PIC.model.sys.reg.PortletLayout'
    ],

    mainPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.reg.PortletLayout',
            dsname: 'EntList',
            idProperty: 'LayoutID'
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "PortletLayoutEdit.aspx", style: { width: 700, height: 550} },
            tlitems: ['-', 'add', 'edit', 'delete', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} },
                { fieldLabel: '状态', id: 'Status', xtype: 'picenumselect', enumdata: PIC.PortletLayoutModel.StatusEnum, schopts: { qryopts: "{ mode: 'Eq', field: 'Status' }"} }
            ],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 150, sortable: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, width: 200, sortable: true },
				{ id: 'col_Type', dataIndex: 'Type', header: '类型', enumdata: PIC.PortletLayoutModel.TypeEnum, width: 100, sortable: true },
				{ id: 'col_Status', dataIndex: 'Status', header: '状态', enumdata: PIC.PortletLayoutModel.StatusEnum, width: 100, sortable: true },
				{ id: 'col_OwnerName', dataIndex: 'OwnerName', header: '所有者', juncqry: true, width: 100, sortable: true },
				{ id: 'col_Description', dataIndex: 'Description', juncqry: true, header: '描述', flex: 1, sortable: true },
				{ id: 'col_CreatedDate', dataIndex: 'CreatedDate', header: '创建日期', width: 100, dateonly: true,  sortable: true }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});