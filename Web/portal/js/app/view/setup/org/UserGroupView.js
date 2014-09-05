Ext.define('PIC.view.setup.org.UserGroupView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.UserGroupViewPage',

    requires: [
        'PIC.model.sys.org.UserGroupNode'
    ],

    pageData: {},

    mainPanel: null,

    refId: null,
    mode: "usergroup", // usergroup
    chkids: [],

    constructor: function (config) {
        var me = this;

        // PICUtil.addModelField(PIC.model.sys.org.Group, { name: 'checked', type: 'boolean' });

        config = Ext.apply({
            layout: 'border',
            mode: $.getQueryString("mode")
        }, config);

        me.refId = config.id || $.getQueryString('id');

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.org.UserGroupNode',
            dsname: 'EntList',
            idProperty: 'GroupID',
            picbeforeload: function (proxy, params, node, operation) {
                if (node) {
                    PICUtil.setReqParams(params, 'querychildren', { pids: [node.getId()] });
                } else {
                    PICUtil.setReqParams(params, 'querychildren');
                }
            },
            picbeforeread: function (data) {
                me.adjustData(data);
            }
        });

        var actionItems = [{
            iconCls: 'pic-icon-edit',
            tooltip: '编辑',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.openFormPage('u', rec);
            }
        }];

        me.mainPanel = Ext.create("PIC.TreeGridPanel", {
            region: 'center',
            store: me.dataStore,
            border: false,
            upcascadedchk: true,
            downcascadedunchk: true,
            selType: 'treemodel',
            formparams: { url: "UserGroupEdit.aspx", style: { width: 600, height: 300} },
            onformsubmit: function (args) {
                me.onFormSubmit(args);
            },
            tlitems: ['-', {
                text: '保存',
                iconCls: 'pic-icon-save',
                handler: function () {
                    me.saveChanges();
                }
            }, '->', '-', 'help'],
            columns: [
				{ id: 'col_SortIndex', dataIndex: 'SortIndex', header: "排序号", width: 60, align: 'center', sortable: true, menuDisabled: true },
				{ id: 'col_Name', xtype: 'treecolumn', dataIndex: 'Name', header: "名称", formlink: { clickfunc: "window.PICPage.openFormPage()" }, flex: 1, sortable: true },
				{ id: 'col_RoleName', dataIndex: 'RoleName', header: "角色", width: 200, sortable: true },
				{ id: 'col_Op', dataIndex: 'Id', header: '操作', xtype: 'actioncolumn', items: actionItems, width: 50, menuDisabled: true, align: 'center' }
            ],
            listeners: {
                checkchange: function (node, checked) {
                    if (checked == true) {
                        if (!node.get("RoleName")) {
                            node.set("RoleName", "默认");
                        } else {
                            node.set("RoleName", node.get("RoleName"));
                        }
                    } else {
                        node.set("RoleName", "");
                    }
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

        me.on("afterrender", function () {
            me.onAfterRender();
        });
    },

    adjustData: function (data) {
        var me = this;

        if (Ext.isArray(data)) {
            Ext.each(data, function (n) {
                Ext.each(me.grpRoles, function (gr) {
                    if (n["GroupID"] === gr["GroupID"]) {
                        n["checked"] = true;
                        n["UserID"] = gr["UserID"] || "";
                        n["RoleID"] = gr["RoleID"] || "";
                        n["RoleCode"] = gr["RoleCode"] || "";
                        n["RoleName"] = gr["RoleName"] || "";

                        return false;
                    }
                });
            });
        }
    },

    reloadData: function (params) {
        var me = this;

        me.refId = params.id || me.refId;

        var p = Ext.apply({
            mode: me.mode
        }, params || {});

        PICUtil.ajaxRequest("qryrolelist", {
            afterrequest: function (resp, opts) {
                me.grpRoles = resp["GroupRoleList"] || [];

                me.mainPanel.getRootNode().cascadeBy(function () {
                    var _n = this,
                        _nid = _n.getId();

                    if (_nid && _nid != "root") {
                        // reset data
                        _n.set('checked', false);
                        _n.set('UserID', "");
                        _n.set('RoleID', "");
                        _n.set('RoleCode', "");
                        _n.set('RoleName', "");

                        Ext.each(me.grpRoles, function (gr) {
                            if (_nid === gr["GroupID"]) {
                                _n.set('checked', true);
                                _n.set('UserID', gr["UserID"] || "");
                                _n.set('RoleID', gr["RoleID"] || "");
                                _n.set('RoleCode', gr["RoleCode"] || "");
                                _n.set('RoleName', gr["RoleName"] || "");

                                return false;
                            }
                        });

                        _n.commit();
                    }
                });
            }
        }, p);
    },

    onAfterRender: function () {
        var me = this;

        if (window.parent && window.parent.PICPage) {
            if (typeof (window.parent.PICPage.onItemActived) === "function") {
                window.parent.PICPage.onItemActived(null, "group", { comp: me });
            }
        }
    },

    saveChanges: function () {
        var me = this;

        me.fireEvent('save', me);

        if (window.parent && window.parent.PICPage) {
            if (typeof (window.parent.PICPage.onDataSave) === "function") {
                window.parent.PICPage.onDataSave("group", { comp: me });
            }
        }
    },

    onFormSubmit: function (args) {
        var me = this;

        var id = args.respdata.frmdata.GroupID,
            data = args.respdata.frmdata,
            action = args.action;

        var _n = me.dataStore.getById(id);

        if (_n) {
            _n.set('checked', true);
            _n.set('UserID', data["UserID"] || "");
            _n.set('RoleID', data["RoleID"] || "");
            _n.set('RoleCode', data["RoleCode"] || "");
            _n.set('RoleName', data["RoleName"] || "");
            _n.commit();
        }
    },

    openFormPage: function (op, rec) {
        var me = window.PICPage,
            rec = rec || me.mainPanel.getFirstSelection();

        if (!me.refId) {
            PICMsgBox.alert("请先选择要操作的用户！");
            return;
        }

        var _form_params = Ext.apply({}, me.mainPanel.formparams);
        _form_params.params = { op: op || 'r', mode: "usergroup", gid: rec.getId(), uid: me.refId }

        PICUtil.openDialog(_form_params);
    }
});