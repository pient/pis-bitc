Ext.define('PIC.view.setup.org.GroupFuncView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.FuncFuncViewPage',

    requires: [
        'PIC.model.sys.org.GroupFunc',
    ],

    pageData: {},

    mainPanel: null,

    refId: null,
    mode: null, // user

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.refId = config.id || $.getQueryString('id');

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.org.GroupFunc',
            dsname: 'EntList',
            idProperty: 'GroupFunctionID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.id = me.refId;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            border: false,
            store: me.dataStore,
            formparams: { url: "GroupFuncEdit.aspx", style: { width: 650, height: 300} },
            tlitems: ['-', 'add', {
                xtype: 'picaddbutton',
                hidden: true,
                iconCls: 'pic-icon-batchadd',
                text: '批量添加',
                handler: function () {
                    me.batchAddFunc();
                }
            }, '-', 'edit', {
                bttype: 'delete',
                handler: function () {
                    me.doDelete();
                }
            }, '->', 'schfield', '-', 'help'],
            schpanel: false,
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_FuncName', dataIndex: 'FuncName', header: '职能名称', juncqry: true, formlink: true, width: 120, sortable: true },
				{ id: 'col_Status', dataIndex: 'Status', header: "状态", enumdata: PIC.GroupFuncModel.StatusEnum, width: 75, sortable: true, align: 'center' },
                { id: "col_Description", dataIndex: 'Description', header: "描述", juncqry: true, flex: 1, sortable: true }
            ],
            beforeformload: function (cfg, id, args, params) {
                me.mainPanel.formparams.params.gid = me.refId;

                if (!me.refId) {
                    PICMsgBox.alert("请先选择对应组");

                    return false;
                }
            }
        });

        me.items = me.mainPanel;

        this.callParent([config]);

        me.addEvents('save');
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    reloadData: function (params) {
        var me = this;
        me.refId = params.id || me.refId;

        me.dataStore.load();
    },

    doDelete: function (recs) {
        var me = this;
        recs = recs || me.mainPanel.getSelection();

        if (!recs || recs.length <= 0) {
            PICMsgBox.alert("请先选择要操作的记录！");
            return;
        }

        PICMsgBox.confirm("确定执行删除操作？", function (val) {
            if ('yes' === val) {
                me.mainPanel.batchOperate("batchdelete", recs, { gid: me.refId }, {
                    afterrequest: function () {
                        me.dataStore.reload();
                    }
                });
            }
        });
    },

    batchAddFunc: function () {
        var me = this;

        PICUtil.openFuncSelectDialog({ callback: "onFuncSelected", mode: "func", refid: me.refId });
    },

    onFuncSelected: function (rtns) {
        var me = this;

        var ids = [];
        if (rtns) {
            Ext.each(rtns, function (r) {
                ids.push(r["FuncID"]);
            });
        }

        me.doSaveFuncs(ids);
    },

    doSaveFuncs: function (ids) {
        var me = this;

        if (ids && ids.length > 0) {
            PICUtil.ajaxRequest('batchaddfuncs', {
                afterrequest: function () {
                    me.dataStore.reload();
                }
            }, { fid: me.refId, rids: ids });
        }
    }
});