Ext.define('PIC.view.setup.bpm.WfActionList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.WfActionListPage',

    requires: [
        'PIC.model.sys.bpm.WfInstance',
        'PIC.model.sys.bpm.WfTask',
        'PIC.model.sys.bpm.WfAction'
    ],

    mainPanel: null,

    mode: "",
    refId: "",

    constructor: function (config) {
        var me = this;
        me.refId = $.getQueryString({ ID: "tid" });

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfAction',
            dsname: 'EntList',
            idProperty: 'TaskID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.tid = me.refId;
            }
        });

        var actionItems = [{
            iconCls: 'pic-icon-run',
            tooltip: '执行',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                // me.mainPanel.openFormWin("u", rec.getId());
            }
        }];

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "WfActionEdit.aspx", style: { width: 700, height: 550} },
            tlitems: ['-', 'cancel', '->', '-', 'help'],
            schitems: [],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Id', header: '操作', xtype: 'actioncolumn', items: actionItems, width: 60, menuDisabled: true, align: 'center' },
				{ dataIndex: 'Title', header: '标题', juncqry: true, formlink: true, flex: 1, sortable: true },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.WfActionModel.StatusEnum, width: 60, sortable: true, align: 'center' },
				{ dataIndex: 'OwnerName', header: '所有人', juncqry: true, width: 80, sortable: true, align: 'center' },
				{ dataIndex: 'CreatedTime', header: '发起时间', width: 150, sortable: true, align: 'center' },
				{ dataIndex: 'ClosedTime', header: '结束时间', width: 150, sortable: true, align: 'center' }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    reloadData: function (params) {
        var me = this;

        if (params) {
            me.refId = params.did;
        }

        me.dataStore.load();
    },

    loadTrackingInfo: function (rec) {
        var rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要查看的流程流程");
        }

        PICUtil.openFlowBusDialog({ op: 'start', did: rec.getId() });
    }
});