Ext.define('PIC.view.setup.bpm.WfTaskList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.WfTaskListPage',

    requires: [
        'PIC.model.sys.bpm.WfInstance',
        'PIC.model.sys.bpm.WfTask'
    ],

    mainPanel: null,

    refId: "",

    constructor: function (config) {
        var me = this;
        me.refId = $.getQueryString({ ID: "iid" });

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfTask',
            dsname: 'EntList',
            idProperty: 'TaskID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.iid = me.refId;
            }
        });

        var actionItems = [{
            iconCls: 'pic-icon-action',
            tooltip: '活动信息',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);
                me.loadActionList(rec);
            }
        }];

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "WfTaskEdit.aspx", style: { width: 700, height: 550} },
            tlitems: ['-', {
                iconCls: 'pic-icon-task',
                text: '任务信息'
            }, 'cancel', '->', '-', 'help'],
            schitems: [],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Op', dataIndex: 'Id', header: '活动', xtype: 'actioncolumn', items: actionItems, width: 60, menuDisabled: true, align: 'center' },
				{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 100, sortable: true },
				{ id: 'col_Title', dataIndex: 'Title', header: '标题', juncqry: true, formlink: true, flex: 1, sortable: true },
				{ id: 'col_Status', dataIndex: 'Status', header: '状态', enumdata: PIC.WfTaskModel.StatusEnum, width: 60, sortable: true, align: 'center' },
				{ id: 'col_CreatorName', dataIndex: 'CreatorName', header: '发起人', juncqry: true, width: 80, sortable: true, align: 'center' },
				{ id: 'col_StartedTime', dataIndex: 'StartedTime', header: '发起时间', width: 135, sortable: true, align: 'center' },
				{ id: 'col_EndedTime', dataIndex: 'EndedTime', header: '结束时间', width: 135, sortable: true, align: 'center' }
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

    loadActionList: function (rec) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要查看的流程任务");
        }

        // PICUtil.openFlowActionDialog({ tid: rec.getId() });
    }
});