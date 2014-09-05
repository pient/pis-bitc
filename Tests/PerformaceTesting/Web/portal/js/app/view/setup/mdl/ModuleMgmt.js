Ext.define('PIC.view.setup.mdl.ModuleMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.ModuleMgmtPage',

    requires: [
        'PIC.model.sys.mdl.Module',
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
            model: 'PIC.model.sys.mdl.Module',
            dsname: 'MdlList',
            idProperty: 'ModuleID',
            picbeforeload: function (proxy, params, node, operation) {
                PICUtil.setReqParams(params, 'querychildren', { mid: node.getId() });
            }
        });

        me.mainPanel = Ext.create("PIC.TreeGridPanel", {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "ModuleEdit.aspx", style: { width: 600, height: 350} },
            onformsubmit: function (args) {
                me.onFormSubmit(args);
            },
            tlitems: ['-', {
                text: '刷新应用模块',
                iconCls: 'pic-icon-refresh',
                handler: function () {
                    me.reloadSysModule();
                }
            }, '->', '-', 'help'],
            columns: [
				{ id: 'col_SortIndex', dataIndex: 'SortIndex', header: "排序号", width: 60, align: 'center', sortable: true, menuDisabled: true },
				{ id: 'col_Name', xtype: 'treecolumn', dataIndex: 'Name', header: "名称", formlink: false, width: 160, sortable: true },
				{ id: 'col_Code', dataIndex: 'Code', header: "编号", width: 100, sortable: true },
                { id: 'col_Type', dataIndex: 'Type', header: "类型", enumdata: PIC.ModuleModel.TypeEnum, width: 75, align: 'center', sortable: true },
                { id: 'col_Url', dataIndex: 'Url', header: "URL", renderer: function (val, p, rec) { return PICUtil.renderFuncLink({ text: val, params: { url: val } }) }, width: 200, sortable: true },
                { id: 'col_Status', dataIndex: 'Status', header: "状态", enumdata: PIC.ModuleModel.StatusEnum, width: 75, sortable: true, align: 'center' },
                { id: "col_Description", dataIndex: 'Description', header: "描述", flex: 1, sortable: true },
                { id: 'col_CreateDate', dataIndex: 'CreateDate', header: "创建日期", width: 85, sortable: true, dateonly: true }
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
                    afterrequest: function (response, opts) {
                        Ext.each(recs, function (r) {
                            r.remove();
                        });
                    }
                });
            }
        });
    },

    reloadSysModule: function () {
        PICUtil.ajaxRequest('refreshsys', {
            afterrequest: function (resp, opts) {
                PICMsgBox.alert("成功刷新应用模块, 重新登录，模块修改将生效！");
            }
        });
    },

    onFormSubmit: function (args) {
        var me = this,
            store = me.dataStore;

        var id = args.params.frmdata.ModuleID,
            action = args.action;

        var rec, new_rec, expnode;

        if (id) { rec = store.getById(id); }
        rec = rec || me.mainPanel.getFirstSelection();
        new_rec = Ext.create('PIC.model.sys.mdl.Module', args.respdata.frmdata);

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