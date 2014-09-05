Ext.define('PIC.view.setup.reg.enum.EnumMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.RegEnumMgmtPage',

    requires: [
        'PIC.model.sys.reg.Enumeration',
    ],

    pageData: {},

    mainPanel: null,
    itemContextMenu: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.reg.Enumeration',
            dsname: 'EntList',
            idProperty: 'EnumerationID',
            picbeforeload: function (proxy, params, node, operation) {
                if (node) {
                    PICUtil.setReqParams(params, 'querychildren', { pids: [node.getId()] });
                } else {
                    PICUtil.setReqParams(params, 'querychildren');
                }
            }
        });

        me.mainPanel = Ext.create("PIC.TreeGridPanel", {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "EnumEdit.aspx", style: { width: 600, height: 300} },
            onformsubmit: function (args) {
                me.onFormSubmit(args);
            },
            tlitems: ['-', { xtype: 'tbtext', text: '<span color="yellow">(右键操作节点)</span>' }, '->', '-', 'help'],
            columns: [
				{ id: 'col_SortIndex', dataIndex: 'SortIndex', header: "排序号", width: 60, sortable: true, align: 'center' },
				{ id: 'col_Name', dataIndex: 'Name', xtype: 'treecolumn', header: "名称", formlink: true, width: 250, sortable: true },
				{ id: 'col_Code', dataIndex: 'Code', header: "编码", width: 200, sortable: true },
				{ id: 'col_Value', dataIndex: 'Value', header: "枚举值", width: 120, sortable: true },
				{ id: 'col_Tag', dataIndex: 'Tag', header: "附加信息", width: 120, sortable: true },
				{ id: 'col_Description', dataIndex: 'Description', header: "描述", flex: 1, sortable: true }
            ],
            listeners: {
                itemcontextmenu: function (v, rec, item, rowIdx, e) {
                    Ext.EventObject.preventDefault();

                    if (rec) {
                        var notleaf = (rec.childNodes.length > 0) || (!rec.get("IsLeaf") && !rec.isLoaded());

                        me.miDelete.setDisabled(notleaf);
                    }

                    var xy = e.getXY();
                    me.itemContextMenu.showAt(xy);
                },

                afterrender: function () {
                    var root = me.mainPanel.getRootNode();

                    Ext.each(root.childNodes, function (r) {
                        r.expand();
                    });
                },

                paste: function (scope, type, rec, cb) {
                    me.onPaste(type, rec, cb);
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
            text: '新增同级模块',
            handler: function () {
                me.addSiblingItem();
            }
        });

        me.miAddSub = Ext.create('Ext.menu.Item', {
            text: '新增子模块',
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

        me.miCut = Ext.create('Ext.menu.Item', {
            id: 'menuCut',
            iconCls: 'pic-icon-cut',
            text: '剪切',
            handler: function () {
                me.mainPanel.cut();
            }
        });

        me.miPasteAsSib = Ext.create('Ext.menu.Item', {
            id: 'menuPasteAsSib',
            text: '粘贴为同级',
            handler: function () {
                me.mainPanel.paste('sib');
            }
        });

        me.miPasteAsSub = Ext.create('Ext.menu.Item', {
            id: 'menuPasteAsSub',
            text: '粘贴为子级',
            handler: function () {
                me.mainPanel.paste('sub');
            }
        });

        me.miPasteCancel = Ext.create('Ext.menu.Item', {
            id: 'menuPasteCancel',
            text: '取消',
            handler: function () {
                me.mainPanel.clearClipBoard();
            }
        });

        me.miPaste = Ext.create('Ext.menu.Item', {
            id: 'menuPaste',
            iconCls: 'pic-icon-paste',
            text: '粘贴',
            menu: {
                items: [me.miPasteAsSib, me.miPasteAsSub, me.miPasteCancel]
            }
        });

        me.itemContextMenu = Ext.create('Ext.menu.Menu', {
            items: [me.miDelete, '-', me.miAddSibling, me.miAddSub, me.miModify, '-', me.miCut, me.miPaste]
        });
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

    onPaste: function (type, rec, cb) {
        var me = this,
            store = me.dataStore;

        var pdstype = cb.type;
        var recs = me.mainPanel.clipBoard.records;
        var idlist = [];

        if (recs != null) {
            Ext.each(recs, function (r) {
                idlist.push(r.getId());
            });
        }

        var data = { IdList: idlist, type: type, pdstype: pdstype, tid: rec.getId() };

        var pnode = rec.parentNode;

        PICUtil.ajaxRequest("paste", {
            afterrequest: function (resp, opts) {
                if (pdstype == 'cut') {
                    // 删除所有复制节点
                    Ext.each(recs, function (r) {
                        r.remove();
                    });
                }

                if (type === "sub") {
                    rec.remove();
                }

                if (resp.EntList) {
                    Ext.each(resp.EntList, function (ent) {
                        if (!store.getById(ent.ID)) {
                            var r = Ext.create('PIC.model.sys.reg.Enumeration', ent);
                            pnode.insertChild(0, r);
                        }
                    });
                }

                store.sort('SortIndex', 'ASC');
            }
        }, data);
    },

    onFormSubmit: function (args) {
        var me = this,
            store = me.dataStore;

        var id = args.params.frmdata.EnumerationID,
            action = args.action;

        var rec, new_rec, expnode;

        if (id) { rec = store.getById(id); }
        rec = rec || me.mainPanel.getFirstSelection();
        new_rec = Ext.create('PIC.model.sys.reg.Enumeration', args.respdata.frmdata);

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