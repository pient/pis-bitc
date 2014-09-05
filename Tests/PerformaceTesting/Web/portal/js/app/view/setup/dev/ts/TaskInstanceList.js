Ext.define('PIC.view.setup.dev.ts.TaskInstanceList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.DevTaskInstanceListPage',

    requires: [
        'PIC.model.sys.dev.TaskInstance'
    ],

    mainPanel: null,

    refId: "",

    constructor: function (config) {
        var me = this;
        me.refId = $.getQueryString({ ID: "id" });

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.dev.TaskInstance',
            dsname: 'EntList',
            idProperty: 'TaskID'
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "TaskInstanceEdit.aspx", style: { width: 650, height: 600 } },
            tlitems: ['-', 'add', 'edit', 'delete', '-', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '编号', name: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }" } },
                { fieldLabel: '名称', name: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }" } }
            ],
            columns: [
                { dataIndex: 'InstanceID', header: '标识', hidden: true },
                { dataIndex: 'TaskID', header: '任务标识', hidden: true },
				{ dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex: 1, sortable: true },
				{ dataIndex: 'Code', header: '编号', juncqry: true, width: 150, sortable: true },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.TaskInstanceModel.StatusEnum, juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'Result', header: '运行结果', juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'StartedTime', header: '开始时间', juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'EndedTime', header: '结束时间', juncqry: true, width: 100, sortable: true }
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