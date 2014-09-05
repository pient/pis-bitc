Ext.define('PIC.view.setup.org.UserMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.OrgUserMgmtPage',

    requires: [
        'PIC.model.sys.org.User',
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
            model: 'PIC.model.sys.org.User',
            dsname: 'EntList',
            idProperty: 'UserID'
        });

        var actionItems = [{
            iconCls: 'pic-icon-key',
            tooltip: '重置密码 ',
            handler: function (grid, rowIndex, colIndex) {
                var rec = me.dataStore.getAt(rowIndex);

                me.resetPwd(rec);
            }
        }, {
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
            store: me.dataStore,
            border: false,
            split: true,
            width: 390,
            schpanel: false,
            formparams: { url: "UserEdit.aspx", style: { width: 600, height: 330} },
            tlitems: ['-', 'add', '->', 'schfield'],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '姓名', juncqry: true, formlink: true, flex: 1, sortable: true, menuDisabled: true },
				{ id: 'col_WorkNo', dataIndex: 'WorkNo', header: '工号', juncqry: true, width: 100, sortable: true, menuDisabled: true },
				{ id: 'col_Op', dataIndex: 'Id', header: '操作', xtype: 'actioncolumn', items: actionItems, width: 80, menuDisabled: true, align: 'center' }
            ],
            listeners: {
                select: function (cmp, rec, idx, eOpts) {
                    me.onItemActived(rec);
                }
            }
        });

        var tabArr = [
            { title: "组", href: "UserGroupView.aspx" },
            { title: "权限查看", href: "UserAuthView.aspx" },
            { title: "设置", href: "UserConfig.aspx" }
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

    resetPwd: function (rec) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要重设密码的用户！");
            return;
        }

        PICMsgBox.prompt('请输入新密码:', '新密码', function (btn, text) {
            if (btn === "ok") {
                PICUtil.ajaxRequest('resetpwd', {
                    afterrequest: function (resp, opts) {
                        PICMsgBox.alert("已设置新密码！");
                    }
                }, { id: rec.getId(), pwd: (text || "") });
            }
        });
    },

    deleteItem: function (rec) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要删除的用户！");
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

    saveGroup: function (rec, params) {
        var me = this;
        var comp = params.comp;
        var _recs = comp.dataStore.getModifiedRecords();

        if (_recs.length <= 0) {
            PICMsgBox("您还未修改任何数据。");
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

        PICUtil.ajaxRequest("savegroup", {
            afterrequest: function (resp, opts) {
                // PICMsgBox.alert("保存成功！" + "joinIDs: " + chked.length + "; quitIDs:" + unchked.length);
                PICMsgBox.alert("保存成功！");

                Ext.each(_recs, function (r) {
                    r.commit();
                });
            }
        }, { id: rec.getId(), joinIDs: chked, quitIDs: unchked });
    },

    onItemActived: function (rec, mode, params) {
        var me = this;
        var rec = rec || me.mainPanel.getFirstSelection();

        if (rec) {
            var rid = rec.getId();

            if (frameContent.PICPage && frameContent.PICPage.reloadData) {
                frameContent.PICPage.reloadData({ mode: 'user', id: rid });   // 刷新组视图
            }
        }
    },

    onDataSave: function (mode, params) {
        var me = this;
        var rec = me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要操作的用户！");
            return;
        }

        switch (mode) {
            case "group":
                me.saveGroup(rec, params);
                break;
        }
    }
});