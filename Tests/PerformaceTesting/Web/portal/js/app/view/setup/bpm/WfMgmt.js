Ext.define('PIC.view.setup.bpm.WfMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.WfMgmtPage',

    requires: [
        'PIC.model.sys.bpm.WfDefine'
    ],

    pageData: {},

    mainPanel: null,
    itemContextMenu: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.mode = $.getQueryString('mode', '');

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfDefine',
            dsname: 'EntList',
            idProperty: 'DefineID',
            groupField: 'Catalog'
        });

        var actionItems = [{
            iconCls: 'pic-icon-edit',
            tooltip: '编辑',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.mainPanel.openFormWin("u", rec.getId());
            }
        }, {
            iconCls: 'pic-icon-copy',
            tooltip: '复制',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.doCopy(rec);
            }
        }, {
            iconCls: 'pic-icon-run',
            tooltip: '启动',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.startFlow(rec);
            }
        }];

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'west',
            border: false,
            split: true,
            width: 300,
            store: me.dataStore,
            features: [{
                ftype: 'grouping',
                groupHeaderTpl: ['{columnName}: {name:this.formatName} ({rows.length} 项)', {
                    formatName: function (name) { return PIC.WfDefineModel.CatalogEnum[name] || name; }
                }]
            }],
            schpanel: false,
            formparams: { url: "WfDefineEdit.aspx", style: { width: 750, height: 550} },
            tlitems: ['-', 'add', '-', '->', 'schfield'],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
                { id: 'col_Catalog', dataIndex: 'Catalog', header: '类别', hidden: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex:1, sortable: true, menuDisabled: true },
				{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, hidden: true, width: 150, sortable: true, menuDisabled: true },
				{ id: 'col_Op', dataIndex: 'Id', header: '操作', xtype: 'actioncolumn', items: actionItems, width: 80, menuDisabled: true, align: 'center' }
            ],
            listeners: {
                select: function (cmp, rec, idx, eOpts) {
                    me.onItemActived(rec);
                }
            }
        });

        var tabArr = [
            { title: "流程实例", href: "WfInstanceList.aspx?mode=" + me.mode }
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

    onItemActived: function (rec, mode, params) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (rec) {
            var rid = rec.getId();

            if (frameContent.PICPage && frameContent.PICPage.reloadData) {
                frameContent.PICPage.reloadData({ did: rid });   // 刷新组视图
            }
        }
    },

    doCopy: function (rec) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要复制的流程。");
            return;
        }

        me.mainPanel.openFormWin({
            params: {
                op: 'copy',
                id: rec.getId()
            }
        });
    },

    startFlow: function (rec) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要启动的流程定义");

            return null;
        }

        var did = rec.getId();

        PICUtil.openFlowBusDialog({ op: 'c', did: did });
    }
});