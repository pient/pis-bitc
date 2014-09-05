
var TypeEnum = { Admin: '综合', Functional: '分摊' };

var HistoryWinParams = { url: "OA_ReimbHistoryList.aspx", style: { width: 950, height: 500} };
var DiagramWinParams = { url: "FlowPicView.aspx", style: { width: 880, height: 700} };

var ItemGrid = null;

Ext.define('PIC.ReimbFormPanel', {
    extend: 'PIC.ExtFormPanel',
    alias: 'widget.picreimbformpanel',

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            title: '行政（月结）费用报销',
            tbar: { xtype: 'picreimbformtoolbar' }
        }, config);

        config.items = [
            { fieldLabel: '标识', name: 'Id', hidden: true },
            { fieldLabel: '费用条目', name: 'Items', hidden: true },
            { fieldLabel: '编号', name: 'Code', allowBlank: false },
            { fieldLabel: '名称', name: 'Name', allowBlank: false },
            { fieldLabel: '日期', name: 'FeeDate', xtype: 'picdatefield', allowBlank: false },
            { fieldLabel: '费用', name: 'Fee', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: TypeEnum, allowBlank: false },
            { xtype: 'container' },
            { fieldLabel: '编制人', name: 'CreatorName', xtype: 'picuserselect', multiSelect: true, fieldMap: { fld_CreatorId: "UserID", fld_Department: "UserID"} },
            { fieldLabel: '人员标识', id: 'fld_CreatorId', name: 'CreatorId', hidden: true },
            { fieldLabel: '部门', id: 'fld_Department', name: 'Department', xtype: 'picgroupselect' },
            { fieldLabel: '注释', name: 'Comments', xtype: 'pictextarea', flex: 2, allowBlank: false },
        // { fieldLabel: '注释', name: 'Comments', xtype: 'pichtmleditor', flex: 2 },
            {fieldLabel: '附件', name: 'Attachments', xtype: 'picfilefield', flex: 2, maxCount: 2, allowBlank: false },
            { xtype: 'picreimbitemgrid', name: 'ItemGrid', id: 'ItemGrid', height: 300, anchor: '-30', margin: "0 0 10 0", flex: 2, isfield: false },
        // { fieldLabel: '编制人', name: 'CreatorName', disabled: true },
        // { fieldLabel: '编制时间', name: 'CreatedDate', xtype: 'picdatefield', disabled: true },
            {xtype: 'picformdescpanel', flex: 2, html: '表单描述：行政（月结）费用报销' }
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


Ext.define('PIC.ReimbFormToolbar', {
    extend: 'PIC.ExtFormToolbar',
    alias: 'widget.picreimbformtoolbar',
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

                if ("Functional".equals(type)) {
                    if (recs.length <= 0) {
                        PICMsgBox.alert("分摊报销，必须提供包含部门的条目信息。");
                        return;
                    }

                    // 若为分摊审批，则所有条目部门项不能为空
                    for (var i = 0; i < recs.length; i++) {
                        var trec = recs[i];
                        if (!trec.get('DepartmentId')) {
                            PICMsgBox.alert("分摊报销，所有条目必须提供部门信息。", null, function () {
                                var idx = ItemGrid.store.indexOf(trec);
                                ItemGrid.startEditByPosition({ row: idx, column: 6 });
                            });
                            return false;
                        }
                    }
                } else {
                    for (var i = 0; i < recs.length; i++) {
                        var trec = recs[i];
                        if (trec.get('DepartmentId')) {
                            PICMsgBox.confirm("条目中有部门信息，确定为综合报销？", null, function (btn) {
                                if (!'yes'.equals(btn)) {
                                    typefield.focus();
                                    return false;
                                } else {
                                    ItemGrid.startEditByPosition({ row: idx, column: 6 });
                                }
                            });
                        }
                    }
                }

                me.formpanel.submit('submit', {});
            }
        }, {
            iconCls: 'pic-icon-delete',
            picexecutable: true,
            text: '删除',
            hidden: (pgOp == "c"),
            name: 'btnDelete',
            handler: function (opts) {
                // PICAjaxRequest('delete', { id: '' });

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
            title: "费用分摊明细",
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
			    { name: 'Title' },
			    { name: 'ItemFee' },
			    { name: 'Date' },
			    { name: 'DepartmentId' },
			    { name: 'DepartmentName' },
			    { name: 'IsReceipt' },
			    { name: 'Comments' }
			]
        });

        config.columns = [
        { id: 'col_Index', dataIndex: 'Index', xtype: 'picrownumberer', width: 25 },
        { id: 'col_Title', dataIndex: 'Title', header: "标题", editor: { xtype: 'pictextfield' }, width: 150 },
        { id: 'col_ItemFee', dataIndex: 'ItemFee', header: "费用", editor: { xtype: 'picnumberfield' }, width: 80 },
        { id: 'col_Date', dataIndex: 'Date', header: "日期", editor: { xtype: 'picdatefield' }, width: 100, renderer: Ext.util.Format.dateRenderer('Y-m-d') },
        { id: 'col_DepartmentName', dataIndex: 'DepartmentName', header: "部门", editor: { xtype: 'pictextfield' }, width: 80 },
        { id: 'col_Comments', dataIndex: 'Comments', header: "注释", editor: { xtype: 'pictextarea', height: 50 }, flex: 1, width: 180 }
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
