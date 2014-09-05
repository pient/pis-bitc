Ext.define('PIC.view.setup.dev.ts.SchedulerList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.DevSchedulerListPage',

    requires: [
        'PIC.model.sys.dev.Scheduler'
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
            model: 'PIC.model.sys.dev.Scheduler',
            dsname: 'EntList',
            idProperty: 'SchedulerID'
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "SchedulerEdit.aspx", style: { width: 650, height: 350 } },
            tlitems: ['-', 'add', 'edit', 'delete', '-', {
                bttype: 'run',
                iconCls: 'pic-icon-run',
                text: '启动调度任务',
                handler: function () {
                    me.runSchedule();
                }
            }, {
                bttype: 'reset',
                iconCls: 'pic-icon-reset',
                text: '重启任务调度',
                handler: function () {
                    me.resetSchedule();
                }
            }, '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '编号', name: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }" } },
                { fieldLabel: '名称', name: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }" } }
            ],
            columns: [
                { dataIndex: 'SchedulerID', header: '标识', hidden: true },
				{ dataIndex: 'Code', header: '编号', juncqry: true, width: 150, sortable: true },
				{ dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex: 1, sortable: true },
				{ dataIndex: 'Type', header: '类型', enumdata: PIC.SchedulerModel.TypeEnum, juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'Catalog', header: '类别', enumdata: PIC.SchedulerModel.CatalogEnum, juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.SchedulerModel.StatusEnum, juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'CreatorName', header: '创建人', juncqry: true, width: 100, sortable: true },
				{ dataIndex: 'CreatedDate', header: '创建日期', juncqry: true, width: 100, sortable: true }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    runSchedule: function (rec) {
        var me = this;

        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请选择要启动的调度！");

            return;
        }

        PICUtil.ajaxRequest('run', {
            masktext: '启动调度任务，请稍后...',
            onsuccess: function () {
                PICMsgBox.alert("任务调度启动成功。");

                // me.dataStore.reload();
            }
        }, { id: rec.getId() });
    },

    stopSchedule: function () {
        var me = this;

        PICUtil.ajaxRequest('stop', {
            masktext: '正在停止任务调度，请稍后...',

            onsuccess: function () {
                PICMsgBox.alert("所有任务调度停止成功。");

                // me.dataStore.reload();
            }
        }, {  });
    },

    resetSchedule: function () {
        var me = this;

        PICUtil.ajaxRequest('reset', {
            masktext: '正在重启任务调度，请稍后...',

            onsuccess: function () {
                PICMsgBox.alert("所有任务调度重新启动成功。");

                // me.dataStore.reload();
            }
        }, {  });
    }
});