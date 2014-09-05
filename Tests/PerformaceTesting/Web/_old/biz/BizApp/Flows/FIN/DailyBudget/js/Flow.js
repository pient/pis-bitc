
var TypeEnum = { Admin: '综合', Functional: '分摊' };

var HistoryWinParams = { url: "HistoryList.aspx", style: { width: 950, height: 500} };
var DiagramWinParams = { url: "FlowPicView.aspx", style: { width: 880, height: 700} };

var ItemGrid = null;

Ext.define('PIC.DailyBudgetApyFormPanel', {
    extend: 'PIC.ExtFormPanel',
    alias: 'widget.picdailybudgetapyformpanel',

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            title: '日常预算申请',
            tbar: { xtype: 'picdailybudgetapyformtoolbar' }
        }, config);

        config.items = [
            { fieldLabel: '标识', name: 'Id', hidden: true },
            { fieldLabel: '编号', name: 'Code', allowBlank: false },
            { fieldLabel: '名称', name: 'Name', allowBlank: false },
            { fieldLabel: '编制人', name: 'CreatorName', xtype: 'picuserselect', multiSelect: true, fieldMap: { fld_CreatorId: "UserID" } },
            { fieldLabel: '人员标识', id: 'fld_CreatorId', name: 'CreatorId', hidden: true },
            { fieldLabel: '部门', id: 'fld_Department', name: 'Department', xtype: 'picgroupselect' },
            { fieldLabel: '注释', name: 'Comments', xtype: 'pictextarea', flex: 2, allowBlank: false },
            {fieldLabel: '附件', name: 'Attachments', xtype: 'picfilefield', flex: 2, maxCount: 2, allowBlank: false },
            { xtype: 'picreimbitemgrid', name: 'ItemGrid', id: 'ItemGrid', height: 300, anchor: '-30', margin: "0 0 10 0", flex: 2, isfield: false },
            { xtype: 'picformdescpanel', flex: 2, html: '注：申请人填好部门，办公用品名称数量，申请人，日期；再请部室负责人、办公室负责人签字；再到保管室领取。此表可复印' }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        ItemGrid = Ext.getCmp('ItemGrid');
    },

    submit: function (action, data, params) {
        params = Ext.apply({}, params);

        this.callParent([action, data, params]);
    }
});


Ext.define('PIC.DailyBudgetApyFormToolbar', {
    extend: 'PIC.ExtFormToolbar',
    alias: 'widget.picdailybudgetapyformtoolbar',
    constructor: function (config) {
        var me = this;

        config = Ext.apply({}, config);

        config.items = config.items || [{
            xtype: 'picsavebutton',
            name: 'btnSave',
            handler: function (obj) {
                ItemGrid.commit();
                me.formpanel.submit('update', {});
            }
        }, {
            iconCls: 'pic-icon-submit',
            picexecutable: true,
            text: '提交',
            name: 'btnSubmit',
            handler: function (obj) {
                var recs = ItemGrid.store.getRange();
                var typefield = me.formpanel.findField("Type");
                var type = typefield.getValue();

                me.formpanel.submit('submit', {});
            }
        }, {
            iconCls: 'pic-icon-delete',
            picexecutable: true,
            text: '删除',
            hidden: (pgOp == "c"),
            name: 'btnDelete',
            handler: function (opts) {
                me.formpanel.submit('delete', {});
            }
        }, '-', 'close', '->', '-', {
            iconCls: 'pic-icon-time',
            text: '历史查看',
            name: 'btnHistory',
            handler: function () {
                PICOpenDialog(HistoryWinParams);
            }
        }, {
            iconCls: 'pic-icon-arrow-round',
            text: '流程图',
            name: 'btnFlow',
            handler: function () {
                PICOpenDialog(DiagramWinParams);
            }
        }, '-', 'help'
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});

Ext.define('PIC.ReimbItemGrid', {
    extend: 'PIC.grid.ExtEditorGridPanel',
    alias: 'widget.picreimbitemgrid',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            title: "办公用品",
            height: 200,
            autoExpandMin: 150,
            tbar: { xtype: 'pictoolbar',
                items: [{
                    xtype: 'picaddbutton',
                    handler: function () {
                        me.editing.cancelEdit();
                        var insRowIdx = me.store.data.length;
                        me.store.insert(insRowIdx, { Fee: 0, Date: new Date() });
                        me.editing.startEditByPosition({ row: insRowIdx, column: 2 });
                    }
                }, {
                    xtype: 'picdeletebutton',
                    handler: function () {
                        var recs = me.getSelectionModel().getSelection();
                        var d_recs = [];

                        if (!recs || recs.length <= 0) {
                            PICMsgBox.alert("提示", "请先选择要删除的记录！");
                            return;
                        }

                        PICMsgBox.confirm("请确认", "确定删除所选记录？", function (btn) {
                            if ('yes'.equals(btn)) {
                                Ext.each(recs, function (r) {
                                    me.store.remove(r);
                                });
                            }
                        });
                    }
                }, '-', 'excel']
            }
        }, config);

        var itemdatastr = PICPgFormData["Items"];
        var itemdata = ($.getJsonObj(itemdatastr) || {}).Items || [];

        config.store = Ext.create('PIC.data.ExtJsonStore', {
            idProperty: 'Index',
            data: itemdata,
            fields: [
			    { name: 'Index' },
			    { name: 'Code' },
			    { name: 'Name' },
			    { name: 'Quantity' },
			    { name: 'Unit' }
			]
        });

        config.columns = [
        { id: 'col_Index', dataIndex: 'Index', xtype: 'picrownumberer', width: 25 },
        { id: 'col_Code', dataIndex: 'Code', header: "编号", editor: { xtype: 'pictextfield' }, width: 150 },
        { id: 'col_Name', dataIndex: 'Name', header: "名称", editor: { xtype: 'pictextfield' }, width: 150 },
        { id: 'col_Quantity', dataIndex: 'Quantity', header: "数量", editor: { xtype: 'picnumberfield' }, width: 80 },
        { id: 'col_Unit', dataIndex: 'Unit', header: "单位", editor: { xtype: 'pictextfield' }, width: 80 }
        ];

        me.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    },

    commit: function () {
        var me = this;
        var itemsdata = me.store.getRangeData();
        var itemsdatastr = $.getJsonString({ Items: itemsdata });
        PICPgForm.findField("Items").setValue(itemsdatastr);

        me.store.commitChanges();
    }

});
