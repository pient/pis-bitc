Ext.define('PIC.view.setup.auth.AuthMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.AuthMgmtPage',

    requires: [
        'PIC.model.sys.auth.Auth',
    ],

    pageData: {},

    navPanel: null,
    mainPanel: null,
    itemContextMenu: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.auth.Auth',
            dsname: 'EntList',
            idProperty: 'AuthID',
            picbeforeload: function (proxy, params, node, operation) {
                if (node) {
                    PICUtil.setReqParams(params, 'querychildren', { pids: [node.getId()] });
                } else {
                    PICUtil.setReqParams(params, 'querychildren');
                }
            }
        });

        me.mainPanel = Ext.create("PIC.TreeGridPanel", {
            store: me.dataStore,
            region: 'center',
            border: false,
            formparams: { url: "AuthEdit.aspx", style: { width: 600, height: 330} },
            onformsubmit: function (args) {
                me.onFormSubmit(args);
            },
            tlitems: ['-', { xtype: 'tbtext', text: '<span color="yellow">(右键操作节点)</span>' }, '->', '-', 'help'],
            columns: [
				{ id: 'col_Name', dataIndex: 'Name', xtype: 'treecolumn', header: "名称", formlink: true, width: 200, menuDisabled: true, sortable: true },
				{ id: 'col_Code', dataIndex: 'Code', header: "编号", width: 240, menuDisabled: true, sortable: true },
				{ id: 'col_Description', dataIndex: 'Description', header: "描述", flex: 1, menuDisabled: true }
            ],
            listeners: {
                afterrender: function () {
                    var root = me.mainPanel.getRootNode();

                    Ext.each(root.childNodes, function (r) {
                        r.expand();
                    });
                },

                itemcontextmenu: function (v, rec, item, rowIdx, e) {
                    return; // 暂不提供右键操作

                    Ext.EventObject.preventDefault();

                    if (rec) {
                        var notleaf = (rec.childNodes.length > 0) || (!rec.get("IsLeaf") && !rec.isLoaded());
                        me.miDelete.setDisabled(notleaf);

                        var editStatus = rec.get("EditStatus") || "C,CS,R,U,D";

                        me.miAddSibling.setVisible((editStatus.indexOf("C") >= 0));
                        me.miAddSub.setVisible((editStatus.indexOf("CS") >= 0));
                        me.miModify.setVisible((editStatus.indexOf("U") >= 0));
                        me.miDelete.setDisabled((editStatus.indexOf("D") < 0));
                    }

                    var xy = e.getXY();
                    me.itemContextMenu.showAt(xy);
                }
            }
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.miDelete = Ext.create('Ext.menu.Item', {
            text: '删除',
            iconCls: 'pic-icon-delete',
            handler: function () {
                me.deleteItem();
            }
        });

        me.miAddSibling = Ext.create('Ext.menu.Item', {
            text: '新增同级节点',
            handler: function () {
                me.addSiblingItem();
            }
        });

        me.miAddSub = Ext.create('Ext.menu.Item', {
            text: '新增子节点',
            handler: function () {
                me.addSubItem();
            }
        });

        me.miModify = Ext.create('Ext.menu.Item', {
            text: '修改',
            iconCls: 'pic-icon-edit',
            handler: function () {
                me.modifyItem();
            }
        });

        me.itemContextMenu = Ext.create('Ext.menu.Menu', {
            items: [me.miDelete, '-', me.miAddSibling, me.miAddSub, me.miModify]
        });
    },

    handleActivate: function (tab, rec) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();
        var url = tab.href;

        if (rec) {
            url = $.combineQueryUrl(url, { id: rec.getId() });
        }

        me.contentFrame.src = url;
    },

    modifyItem: function (rec) {
        var me = this;

        var _rec = rec || me.mainPanel.getFirstSelection();

        me.mainPanel.openFormWin('u', _rec.getId());
    },

    addSubItem: function (rec) {
        var me = this;
        var _rec = rec || me.mainPanel.getFirstSelection();

        me.mainPanel.openFormWin('createsub', _rec.getId());
    },

    addSiblingItem: function (rec) {
        var me = this;
        var _rec = rec || me.mainPanel.getFirstSelection();

        me.mainPanel.openFormWin('createsib', _rec.getId());
    },

    deleteItem: function (recs) {
        var me = this;
        recs = recs || me.mainPanel.getSelection();

        if (!recs || recs.length <= 0) {
            PICMsgBox.alert("请先选择要操作的记录！");
            return;
        }

        for (var i = 0; i < recs.length; i++) {
            if (recs[i].childNodes.length > 0) {
                PICMsgBox.warn("存在非叶节点，请删除叶节点子节点再作删除操作！");
                return;
            }
        }

        PICMsgBox.confirm("确定执行删除操作？", function (val) {
            if ('yes' === val) {
                me.mainPanel.batchOperate("batchdelete", recs, null, {
                    afterrequest: function (response, opts) {
                        Ext.each(recs, function (r) {
                            r.remove();
                        });
                    }
                });
            }
        });
    },

    onFormSubmit: function (args) {
        var me = this,
            store = me.dataStore;

        var id = args.params.frmdata.EnumerationID,
            action = args.action;

        var rec, new_rec, expnode;

        if (id) { rec = store.getById(id); }
        rec = rec || me.mainPanel.getFirstSelection();
        new_rec = Ext.create('PIC.model.sys.org.Group', args.respdata.frmdata);

        switch (action) {
            case 'create':
            case 'createsib':
                if (rec && rec.get('ParentID')) {
                    var pnode = store.getById(rec.get('ParentID'));
                    pnode.insertChild(0, new_rec);
                } else {
                    store.reload();
                }
                break;
            case 'createsub':
            case 'update':
                var p = rec.parentNode;
                var idx = p.indexOf(rec);

                rec.remove();

                p.insertChild(idx, new_rec);

                if (action == 'createsub') {
                    expnode = rec;
                }
                break;
        }

        if (expnode && expnode.id) {
            var _expnode = store.getById(expnode.getId());

            if (_expnode) {
                _expnode.expand();
            }
        }

        store.sort('SortIndex', 'ASC');
    }
});