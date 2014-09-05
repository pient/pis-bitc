Ext.define('PIC.view.setup.org.GroupUserView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.GroupUserViewPage',

    requires: [
        'PIC.model.sys.org.UserGroup',
    ],

    pageData: {},

    mainPanel: null,

    refId: null,
    mode: "groupuser", // groupuser

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.refId = config.id || $.getQueryString('id');

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.org.UserGroup',
            dsname: 'EntList',
            idProperty: 'UserID',
            remoteSort: true,
            remoteGroup: true,
            picbeforeload: function (proxy, params, node, operation) {
                params.data.id = me.refId;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            border: false,
            store: me.dataStore,
            formparams: { url: "UserGroupEdit.aspx?mode=groupuser", style: { width: 650, height: 400} },
            tlitems: ['-', 'add', 'edit', {
                bttype: 'delete',
                handler: function () {
                    me.doDelete();
                }
            }, '->', 'schfield', '-', 'help'],
            schpanel: false,
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_WorkNo', dataIndex: 'WorkNo', header: '工号', juncqry: true, width: 150, sortable: true },
				{ id: 'col_UserName', dataIndex: 'UserName', header: '用户名', formlink: { clickfunc: "window.PICPage.openFormPage()" }, juncqry: true, width: 150, sortable: true },
				{ id: 'col_RoleName', dataIndex: 'RoleName', header: '角色', juncqry: true, flex: 1, sortable: true }
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

    openFormPage: function (op, rec) {
        var me = window.PICPage,
            rec = rec || me.mainPanel.getFirstSelection();

        if (!me.refId) {
            PICMsgBox.alert("请先选择要操作的组！");
            return;
        }

        var _form_params = Ext.apply({}, me.mainPanel.formparams);
        _form_params.params = { op: op || 'r', id: rec.getId(), gid: me.refId }

        PICUtil.openDialog(_form_params);
    }
});