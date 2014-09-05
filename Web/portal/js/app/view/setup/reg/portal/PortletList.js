Ext.define('PIC.view.setup.reg.portal.PortletList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.RegPortletSetupListPage',

    requires: [
        'PIC.model.sys.reg.Portlet'
    ],

    mainPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.reg.Portlet',
            dsname: 'EntList',
            idProperty: 'PortletID'
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "PortletEdit.aspx", style: { width: 650, height: 300} },
            tlitems: ['-', 'add', 'edit', 'delete', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }" } },
                { fieldLabel: '类型', id: 'Type', xtype: 'picenumselect', enumdata: PIC.PortletModel.TypeEnum, schopts: { qryopts: "{ mode: 'Eq', field: 'Type' }" } }
            ],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Code', header: '编号', juncqry: true, width: 150, sortable: true },
				{ dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, width: 200, sortable: true },
				{ dataIndex: 'Type', header: '类型', enumdata: PIC.PortletModel.TypeEnum, width: 80, sortable: true, align: 'center' },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.PortletModel.StatusEnum, width: 80, sortable: true, align: 'center' },
				{ dataIndex: 'DataModule', header: '模块', enumdata: PIC.PortletModel.ModuleEnum, width: 80, sortable: true, align: 'center' },
				{ dataIndex: 'Description', juncqry: true, header: '描述', flex: 1, sortable: true },
				{ dataIndex: 'CreatedDate', header: '创建日期', width: 100, dateonly: true,  sortable: true }
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