Ext.define('PIC.view.setup.org.RoleAuthView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.RoleAuthViewPage',

    requires: [
        'PIC.model.sys.org.RoleAuthNode',
    ],

    pageData: {},

    mainPanel: null,

    refId: null,
    mode: null, // roleauth
    chkids: [],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            mode: $.getQueryString("mode", "roleauth")
        }, config);

        me.refId = config.id || $.getQueryString('id');

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.org.RoleAuthNode',
            dsname: 'EntList',
            idProperty: 'AuthID',
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
            formparams: { url: "RoleAuthEdit.aspx", style: { width: 600, height: 300} },
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
				{ id: 'col_Code', dataIndex: 'Code', header: "编码", width: 200, sortable: true },
				{ id: 'col_Op', dataIndex: 'Id', header: '操作', xtype: 'actioncolumn', items: actionItems, width: 50, menuDisabled: true, align: 'center' }
            ],
            listeners: {
                checkchange: function (node, checked) {
                    node.setDirty();
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
                Ext.each(me.roleAuths, function (gr) {
                    if (n["AuthID"] === gr["AuthID"]) {
                        n["checked"] = true;
                        n["RoleID"] = gr["RoleID"] || "";

                        return false;
                    }
                });
            });
        }
    },

    reloadData: function (params) {
        var me = this;

        me.mode = params.mode || me.mode;
        me.refId = params.id || me.refId;

        var p = Ext.apply({
            mode: me.mode
        }, params || {});

        PICUtil.ajaxRequest("qryauthlist", {
            afterrequest: function (resp, opts) {
                me.roleAuths = resp["RoleAuthList"] || [];

                me.mainPanel.getRootNode().cascadeBy(function () {
                    var _n = this,
                        _nid = _n.getId();
                    if (_nid && _nid != "root") {
                        // reset data
                        _n.set('checked', false);
                        _n.set('RoleID', "");

                        Ext.each(me.roleAuths, function (ra) {
                            if (_nid === ra["AuthID"]) {
                                // alert("nid: " + _nid + "; AuthID: " + ra["AuthID"])

                                _n.set('checked', true);
                                _n.set('RoleID', ra["RoleID"] || "");

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

        var root = me.mainPanel.getRootNode();

        Ext.each(root.childNodes, function (r) {
            r.expand();
        });

        if (window.parent && window.parent.PICPage) {
            if (typeof (window.parent.PICPage.onItemActived) === "function") {
                window.parent.PICPage.onItemActived(null, "auth", { comp: me });
            }
        }
    },

    saveChanges: function () {
        var me = this;

        if (!me.refId) {
            PICMsgBox.alert("请先选择要操作的角色！");
            return;
        }

        me.fireEvent('save', me);

        var _recs = me.dataStore.getModifiedRecords();

        if (_recs.length <= 0) {
            PICMsgBox.alert("您还未修改任何数据。");
            return;
        }

        var chked = [], unchked = [];

        Ext.each(_recs, function (r) {
            var _rid = r.getId();

            if (!_rid || _rid === 'root') {
                return;
            }

            if (r.get("checked") === true) {
                chked.push(_rid);
            } else {
                unchked.push(_rid);
            }
        });

        PICUtil.ajaxRequest("saveauth", {
            afterrequest: function (resp, opts) {
                // PICMsgBox.alert("保存成功！" + "addIDs: " + chked.length + "; removeIDs:" + unchked.length);
                PICMsgBox.alert("保存成功！");

                Ext.each(_recs, function (r) {
                    r.commit();
                });
            }
        }, { id: me.refId, addIDs: chked, removeIDs: unchked });
    },

    onFormSubmit: function (args) {
        var me = this;

        var id = args.respdata.frmdata.AuthID,
            data = args.respdata.frmdata,
            action = args.action;

        var _n = me.dataStore.getById(id);

        if (_n) {
            _n.set('checked', true);
            _n.set('RoleID', data["RoleID"] || "");
            _n.commit();
        }
    },

    openFormPage: function (op, rec) {
        var me = window.PICPage,
            rec = rec || me.mainPanel.getFirstSelection();

        if (!me.refId) {
            PICMsgBox.alert("请先选择要操作的角色！");
            return;
        }

        var _form_params = Ext.apply({}, me.mainPanel.formparams);
        _form_params.params = { op: op || 'r', mode: 'roleauth', aid: rec.getId(), rid: me.refId }

        PICUtil.openDialog(_form_params);
    }
});