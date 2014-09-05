Ext.define('PIC.view.setup.bpm.WfInstanceList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.WfInstanceListPage',

    requires: [
        'PIC.model.sys.bpm.WfInstance'
    ],

    mainPanel: null,

    refId: "",

    constructor: function (config) {
        var me = this;

        me.refId = $.getQueryString({ ID: "did" });
        me.mode = $.getQueryString("mode", "");

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfInstance',
            dsname: 'EntList',
            idProperty: 'InstanceID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.did = me.refId;
                params.data.mode = me.mode;
            }
        });

        var actionItems = [{
            iconCls: 'pic-icon-view',
            tooltip: '查看',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.openInstanceInfo(rec);
            }
        }];

        // 新建或草稿状态的流程不在这里显示
        delete (PIC.WfInstanceModel.StatusEnum["New"]);
        delete (PIC.WfInstanceModel.StatusEnum["Draft"]);

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            selectMode: "single",
            formparams: { url: "WfInstanceEdit.aspx", style: { width: 700, height: 550} },
            tlitems: ['-', {
                bttype: 'changeop',
                iconCls: 'pic-icon-exchange',
                text: '换人',
                handler: function () {
                    me.changeOperator();
                }
            }, '-', {
                bttype: 'terminate',
                iconCls: 'pic-icon-close',
                text: '终止',
                handler: function () {
                    me.endFlow();
                }
            }, '->', 'schfield', '-', 'cquery', '-', 'help'],
            schcols: 2,
            schitems: [
                { fieldLabel: '发起人', schopts: { qryopts: "{ mode: 'Like', field: 'CreatorName' }" } },
                { fieldLabel: '状态', id: 'Status', xtype: 'picenumselect', enumdata: PIC.WfInstanceModel.StatusEnum, schopts: { qryopts: "{ mode: 'Eq', field: 'Status' }" } }
            ],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Id', header: '查看', xtype: 'actioncolumn', items: actionItems, width: 60, menuDisabled: true, align: 'center' },
				{ dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex:1, sortable: true },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.WfInstanceModel.StatusEnum, width: 80, sortable: true, align: 'center' },
				{ dataIndex: 'CreatorName', header: '发起人', juncqry: true, width: 80, sortable: true, align: 'center' },
				{ dataIndex: 'StartedTime', header: '发起时间', width: 150, sortable: true, align: 'center' }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        me.mainPanel.on("select", function (cmp, rec, e, a) {
            var status = rec.get("Status");

            var chgOpBtn = PICUtil.getFirstCmp("[bttype=changeop]", me.mainPanel);
            var termiBtn = PICUtil.getFirstCmp("[bttype=terminate]", me.mainPanel);

            chgOpBtn.setDisabled(!(status == "Started"));
            termiBtn.setDisabled(!(status == "Started"));
        });

        this.callParent(arguments);
    },

    reloadData: function (params) {
        var me = this;

        if (params) {
            me.refId = params.did;
        }

        me.dataStore.load();
    },

    openInstanceInfo: function (rec) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要查看的流程实例");

            return;
        }

        PICUtil.openFlowBusDialog({ iid: rec.getId() });
    },

    changeOperator: function (rec) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要换人的流程实例");

            return;
        }

        me.mainPanel.openFormWin({
            url: "WfActionChangeList.aspx",
            params: {
                op: 'chgop',
                iid: rec.getId()
            }
        });
    },

    endFlow: function (rec) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要终止的流程实例");

            return;
        }

        PICUtil.ajaxRequest('endflow', {
            timeout: 30000, // 默认30秒超时，处理Workflow的时间可能稍微有点长
            onsuccess: function (respData, opts) {
                PICMsgBox.alert('成功终止的流程实例！');
            }
        }, { iid: rec.getId() });
    }
});