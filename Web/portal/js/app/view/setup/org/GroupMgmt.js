Ext.define('PIC.view.setup.org.GroupMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.OrgGroupMgmtPage',

    requires: [
        'PIC.model.sys.org.Group',
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
            model: 'PIC.model.sys.org.Group',
            dsname: 'EntList',
            idProperty: 'GroupID',
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
            width: 340,
            minSize: 200,
            maxSize: 500,
            formparams: { url: "GroupEdit.aspx", style: { width: 600, height: 300} },
            onformsubmit: function (args) {
                me.onFormSubmit(args);
            },
            tlitems: ['-', { xtype: 'tbtext', text: '<span color="yellow">(右键操作节点)</span>' }, '->', '-', 'help'],
            columns: [
				{ id: 'col_Name', dataIndex: 'Name', xtype: 'treecolumn', header: "名称", formlink: true, flex: 1, menuDisabled: true, sortable: true },
				{ id: 'col_Code', dataIndex: 'Code', header: "编号", flex: .5, menuDisabled: true, sortable: true }
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

        var tabArr = [
            { title: "人员", href: "GroupUserView.aspx" },
            { title: "职能", href: "GroupFuncView.aspx" },
            { title: "角色查看", href: "GroupRoleView.aspx" }
        ];

        Ext.each(tabArr, function (i) {
            this.border = false;
            this.listeners = { activate: function (tab) { me.handleActivate(tab) } };
            this.html = "<div style='display:none;'></div>";
        });

        me.tabPanel = Ext.create("PIC.ExtTabPanel", {
            region: 'north',
            activeTab: 0,
            border: false,
            width: document.body.offsetWidth - 5,
            items: tabArr
        });

        me.stagePanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            margins: '0 0 0 0',
            border: false,
            cls: 'empty',
            bodyStyle: 'background:#f1f1f1',
            html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src=""></iframe>',
            listeners: {
                afterrender: function () {
                    me.contentFrame = document.getElementById("frameContent");
                }
            }
        });

        me.subPanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            layout: 'border',
            border: false,
            items: [me.tabPanel, me.stagePanel]
        });

        me.items = [me.mainPanel, me.subPanel];

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
            if (!recs[0].isLeaf() && (!recs[0].isLoaded() || recs[i].childNodes.length > 0)) {
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
    },

    onItemActived: function (rec, mode, params) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (rec) {
            var rid = rec.getId();

            if (frameContent.PICPage && frameContent.PICPage.reloadData) {
                frameContent.PICPage.reloadData({ mode: 'group', id: rid });   // 刷新组视图
            }
        }
    }
});