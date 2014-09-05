Ext.define('PIC.view.setup.dev.tmpl.TmplMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.DevTmplMgmtPage',

    requires: [
        'PIC.model.sys.dev.TemplateCatalog',
    ],

    pageData: {},

    mainPanel: null,
    stagePanel: null,
    itemContextMenu: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.dev.TemplateCatalog',
            dsname: 'EntList',
            idProperty: 'CatalogID',
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
            region: 'west',
            border: false,
            split: true,
            width: 300,
            minSize: 240,
            maxSize: 500,
            formparams: { url: "TmplCatalogEdit.aspx", style: { width: 700, height: 500} },
            onformsubmit: function (args) {
                me.onFormSubmit(args);
            },
            tlitems: ['-', { xtype: 'tbtext', text: '<span color="yellow">(右键操作节点)</span>' }, '->', '-', 'help'],
            columns: [
				{ dataIndex: 'Name', xtype: 'treecolumn', header: "名称", formlink: true, flex: 1, menuDisabled: true, sortable: true }
            ],
            listeners: {
                select: function (cmp, rec, idx, eOpts) {
                    me.onItemActived(rec);
                },

                afterrender: function () {
                    var root = me.mainPanel.getRootNode();

                    Ext.each(root.childNodes, function (r) {
                        r.expand();
                    });
                },

                itemcontextmenu: function (v, rec, item, rowIdx, e) {
                    Ext.EventObject.preventDefault();

                    if (rec) {
                        var notleaf = (rec.childNodes.length > 0) || (!rec.get("IsLeaf") && !rec.isLoaded());
                        me.miDelete.setDisabled(notleaf);

                        var editStatus = rec.get("EditStatus") || "C,CS,R,U,D,";

                        me.miAddSibling.setVisible((editStatus.indexOf("C,") >= 0));
                        me.miAddSub.setVisible((editStatus.indexOf("CS,") >= 0));
                        me.miModify.setVisible((editStatus.indexOf("U,") >= 0));
                        me.miDelete.setDisabled((editStatus.indexOf("D,") < 0));
                    }

                    var xy = e.getXY();
                    me.itemContextMenu.showAt(xy);
                }
            }
        });

        me.stagePanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            border: false,
            bodyStyle: 'background:#f1f1f1',
            html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src=""></iframe>'
        });

        me.items = [me.mainPanel, me.stagePanel];

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
                    onsuccess: function (response, opts) {
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

        var id = args.params.frmdata.TemplateID,
            action = args.action;

        var rec, new_rec, expnode;

        if (id) { rec = store.getById(id); }
        rec = rec || me.mainPanel.getFirstSelection();
        new_rec = Ext.create('PIC.model.sys.dev.TemplateCatalog', args.respdata.frmdata);

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
    },

    onItemActived: function (rec, mode, params) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (rec) {
            if (!frameContent.PICPage) {
                frameContent.location.href = "TmplList.aspx?cid=" + rec.getId();
            } else if (frameContent.PICPage.reloadPage) {
                frameContent.PICPage.reloadPage(rec);   // 异步加载
            }
        }
    }
});