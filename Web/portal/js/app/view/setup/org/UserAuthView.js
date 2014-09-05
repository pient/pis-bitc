Ext.define('PIC.view.setup.org.UserAuthView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.UserAuthViewPage',

    requires: [
        'PIC.model.sys.org.UserAuthNode',
    ],

    pageData: {},

    mainPanel: null,

    refId: null,
    mode: "userauth",
    chkids: [],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            mode: $.getQueryString("mode", "userauth")
        }, config);

        me.refId = config.id || $.getQueryString('id');

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.org.UserAuthNode',
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
            formparams: { url: "../auth/AuthEdit.aspx", style: { width: 600, height: 300} },
            onformsubmit: function (args) {
                me.onFormSubmit(args);
            },
            tlitems: ['-', 'view', '->', '-', 'help'],
            columns: [
				{ id: 'col_SortIndex', dataIndex: 'SortIndex', header: "排序号", width: 60, align: 'center', sortable: true, menuDisabled: true },
				{ id: 'col_Name', xtype: 'treecolumn', dataIndex: 'Name', header: "名称", formlink: true, flex: 1, sortable: true },
				{ id: 'col_Code', dataIndex: 'Code', header: "编码", width: 200, sortable: true }
            ]
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
                        n["UserID"] = gr["UserID"] || "";

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
                me.roleAuths = resp["UserAuthList"] || [];

                me.mainPanel.getRootNode().cascadeBy(function () {
                    var _n = this,
                        _nid = _n.getId();
                    if (_nid && _nid != "root") {
                        // reset data
                        _n.set('checked', false);
                        _n.set('UserID', "");

                        Ext.each(me.roleAuths, function (ra) {
                            if (_nid === ra["AuthID"]) {
                                // alert("nid: " + _nid + "; AuthID: " + ra["AuthID"])

                                _n.set('checked', true);
                                _n.set('UserID', ra["UserID"] || "");

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
    }
});