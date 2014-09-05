Ext.define('PIC.view.setup.org.RoleMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.AuthRoleMgmtPage',

    requires: [
        'PIC.model.sys.org.OrgType',
        'PIC.model.sys.org.Role'
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

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.org.Role',
            dsname: 'EntList',
            idProperty: 'RoleID',
            groupField: 'Type'
        });

        var actionItems = [{
            iconCls: 'pic-icon-edit',
            tooltip: '编辑',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.mainPanel.openFormWin("u", rec.getId());
            }
        }, {
            iconCls: 'pic-icon-delete',
            tooltip: '删除 ',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.deleteItem(rec);
            }
        }];

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'west',
            border: false,
            split: true,
            width: 390,
            store: me.dataStore,
            features: [{
                ftype: 'grouping',
                groupHeaderTpl: ['{columnName}: {name:this.formatName} ({rows.length} 项)', {
                    formatName: function (name) { return PIC.OrgTypeModel.TypeEnum[name]; }
                }],
                id: 'userGrouping'
            }],
            schpanel: false,
            formparams: { url: "RoleEdit.aspx", style: { width: 600, height: 300} },
            tlitems: ['-', 'add', '->', 'schfield'],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
                { id: 'col_Type', dataIndex: 'Type', header: '类型', hidden: true },
                { id: 'col_SortIndex', dataIndex: 'SortIndex', header: '排序号', width: 50, align:'center' },
				{ id: 'col_Name', dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex:1, sortable: true, menuDisabled: true },
				{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 120, sortable: true, menuDisabled: true },
				{ id: 'col_Op', dataIndex: 'Id', header: '操作', xtype: 'actioncolumn', items: actionItems, width: 80, menuDisabled: true, align: 'center' }
            ],
            listeners: {
                select: function (cmp, rec, idx, eOpts) {
                    me.onItemActived(rec);
                }
            }
        });

        var tabArr = [
            { title: "权限", href: "RoleAuthView.aspx" }
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

    deleteItem: function (rec) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要删除的角色！");
            return;
        }

        PICMsgBox.confirm("确定执行删除操作？", function (val) {
            if ('yes' === val) {
                me.mainPanel.batchOperate("batchdelete", [rec], null, {
                    afterrequest: function (response, opts) {
                        me.dataStore.load();
                    }
                });
            }
        });
    },

    onItemActived: function (rec, mode, params) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (rec) {
            var rid = rec.getId();

            if (frameContent.PICPage && frameContent.PICPage.reloadData) {
                frameContent.PICPage.reloadData({ mode: 'role', id: rid });   // 刷新组视图
            }
        }
    }
});