Ext.define('PIC.view.setup.dev.ts.TaskList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.DevTaskListPage',

    requires: [
        'PIC.model.sys.dev.Task',
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
            model: 'PIC.model.sys.dev.Task',
            dsname: 'EntList',
            idProperty: 'TaskID'
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "TaskEdit.aspx", style: { width: 650, height: 500 } },
            tlitems: ['-', 'add', 'edit', 'delete', '-', {
                bttype: 'run',
                text: '运行任务',
                iconCls: 'pic-icon-run',
                handler: function () {
                    me.runTask();
                }
            }, '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '编号', name: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }" } },
                { fieldLabel: '名称', name: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }" } }
            ],
            columns: [
                { dataIndex: 'TaskID', header: '标识', hidden: true },
				{ dataIndex: 'Code', header: '编号', juncqry: true, width: 150, sortable: true },
				{ dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex: 1, sortable: true },
				{ dataIndex: 'Type', header: '类型', enumdata: PIC.TaskModel.TypeEnum, juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'Catalog', header: '类别', enumdata: PIC.TaskModel.CatalogEnum, juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.TaskModel.StatusEnum, juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'CreatorName', header: '创建人', juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'CreatedTime', header: '创建日期', juncqry: true, width: 100, sortable: true }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    runTask: function (rec) {
        var me = this;

        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要测试的模版！");

            return;
        }

        PICUtil.ajaxRequest('run', {
            masktext: '正在执行任务，请稍后...'
        }, { id: rec.getId() });
    }
});