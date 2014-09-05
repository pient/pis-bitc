Ext.define('PIC.view.setup.bpm.WfActionChangeList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.WfActionChangeListPage',

    requires: [
        'PIC.model.sys.bpm.WfInstance',
        'PIC.model.sys.bpm.WfTask',
        'PIC.model.sys.bpm.WfAction'
    ],

    mainPanel: null,

    mode: "",

    constructor: function (config) {
        var me = this;
        me.iid = $.getQueryString({ ID: "iid" });

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfAction',
            dsname: 'EntList',
            idProperty: 'TaskID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.iid = me.iid;
            }
        });

        var actionItems = [{
            iconCls: 'pic-icon-exchange',
            tooltip: '换人',
            isDisabled: function (view, rowIdx, colIdx, item, rec) {
                if ("new".equals(rec.get('Status'))) {
                    return false;
                }

                return true;
            },
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.changeOperator(rec);
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
				{ dataIndex: 'Id', header: '换人', xtype: 'actioncolumn', items: actionItems, width: 60, menuDisabled: true, align: 'center' },
				{ dataIndex: 'Title', header: '标题', juncqry: true, flex: 1, minWidth: 200, sortable: true },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.WfActionModel.StatusEnum, width: 60, sortable: true, align: 'center' },
				{ dataIndex: 'OwnerName', header: '处理', juncqry: true, width: 80, sortable: true, align: 'center' },
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

    changeOperator: function (rec) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要换人的活动");

            return;
        }

        PICUtil.openSelDialog({
            url: PICConfig.UserSelectPath,
            sender: me,
            style: { width: 390, height: 500 },
            params: { callback: "doChangeOperator", mode: "func", aid: rec.getId() }
        });
    },

    doChangeOperator: function (rtn) {
        var me = this;

        if (rtn) {
            var aid, usr;

            if (rtn["params"] && rtn["params"]["aid"] && rtn["data"] && rtn["data"].length > 0) {
                aid = rtn["params"]["aid"].value;
                usr = rtn["data"][0];

                if (aid && usr) {
                    PICUtil.ajaxRequest('chgop', {
                        onsuccess: function (respData, opts) {
                            me.dataStore.reload();
                        }
                    }, { aid: aid, uid: usr["UserID"] });
                }
            }
        }
    }
});