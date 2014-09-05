Ext.define('PIC.view.setup.org.FuncRoleView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.FuncRoleViewPage',

    requires: [
        'PIC.model.sys.org.FuncRole',
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
            model: 'PIC.model.sys.org.FuncRole',
            dsname: 'EntList',
            idProperty: 'FunctionRoleID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.id = me.refId;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            border: false,
            store: me.dataStore,
            formparams: { url: "FuncRoleEdit.aspx", style: { width: 650, height: 300} },
            tlitems: ['-', 'add', {
                xtype: 'picaddbutton',
                hidden: true,
                iconCls: 'pic-icon-batchadd',
                text: '批量添加',
                handler: function () {
                    me.batchAddRole();
                }
            }, '-', 'edit', 'delete', '->', 'schfield', '-', 'help'],
            schpanel: false,
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_RoleName', dataIndex: 'RoleName', header: '角色名称', juncqry: true, formlink: true, width: 120, sortable: true },
				{ id: 'col_Status', dataIndex: 'Status', header: "状态", enumdata: PIC.FuncRoleModel.StatusEnum, width: 75, sortable: true, align: 'center' },
                { id: "col_Description", dataIndex: 'Description', header: "描述", juncqry: true, flex: 1, sortable: true }
            ],
            beforeformload: function (cfg, id, args, params) {
                me.mainPanel.formparams.params.fid = me.refId;

                if (!me.refId) {
                    PICMsgBox.alert("请先选择对应职能");

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
        me.mode = params.mode || me.mode;
        me.refId = params.id || me.refId;

        me.dataStore.load();
    },

    batchAddRole: function () {
        var me = this;

        PICUtil.openRoleSelectDialog({ callback: "onRoleSelected", mode: "func", refid: me.refId });
    },

    onRoleSelected: function (rtns) {
        var me = this;

        var ids = [];
        if (rtns) {
            Ext.each(rtns, function (r) {
                ids.push(r["RoleID"]);
            });
        }

        me.doSaveRoles(ids);
    },

    doSaveRoles: function (ids) {
        var me = this;

        if (ids && ids.length > 0) {
            PICUtil.ajaxRequest('batchaddroles', {
                afterrequest: function () {
                    me.dataStore.reload();
                }
            }, { fid: me.refId, rids: ids });
        }
    }
});