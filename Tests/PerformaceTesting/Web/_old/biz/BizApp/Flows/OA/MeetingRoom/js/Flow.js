
var PurposeEnum = { Meeting: '开会', Presentation: '展示', Explain: '说明会', Education: '视听教育', Training: '训练', Other: '其他' };

var HistoryWinParams = { url: "HistoryList.aspx", style: { width: 950, height: 500} };
var DiagramWinParams = { url: "FlowPicView.aspx", style: { width: 880, height: 700} };

Ext.define('PIC.MeetingRoomApyFormPanel', {
    extend: 'PIC.ExtFormPanel',
    alias: 'widget.picmeetingroomapyformpanel',

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            title: '办公用品领用',
            tbar: { xtype: 'picmeetingroomapyformtoolbar' }
        }, config);

        config.items = [
            { fieldLabel: '标识', name: 'Id', hidden: true },
            { fieldLabel: '编号', name: 'Code', allowBlank: false },
            { fieldLabel: '名称', name: 'Name', allowBlank: false },
            { fieldLabel: '编制人', name: 'CreatorName', xtype: 'picuserselect', multiSelect: true, fieldMap: { fld_CreatorId: "UserID" } },
            { fieldLabel: '人员标识', id: 'fld_CreatorId', name: 'CreatorId', hidden: true },
            { fieldLabel: '部门', id: 'fld_Department', name: 'Department', xtype: 'picgroupselect' },
            { fieldLabel: '使用时间（起）', name: 'StartTime', xtype: 'picdatefield' },
            { fieldLabel: '使用时间（至）', name: 'EndTime', xtype: 'picdatefield' },
            { fieldLabel: '是否使用视听设备', name: 'IsUseAudioVisual', xtype: 'checkbox', allowBlank: false },
            { fieldLabel: '是否需要备用茶水', name: 'IsPrepareTea', xtype: 'checkbox', allowBlank: false },
            { fieldLabel: '使用人数', name: 'UserNumber', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '使用目的', name: 'Purpose', xtype: 'picenumselect', enumdata: PurposeEnum, multiSelect: true, allowBlank: false },
            { fieldLabel: '说明', name: 'Comments', xtype: 'pictextarea', flex: 2, allowBlank: false },
            { fieldLabel: '附件', name: 'Attachments', xtype: 'picfilefield', flex: 2, maxCount: 2, allowBlank: false },
            { xtype: 'picformdescpanel', flex: 2, height: 150, html: '注意事项：<br />1．请注意不受理口头方式借用会场。 <br />2．如需使用视听设备，请由专业管理人员操作，非经同意，请勿擅自使用。 <br />3．使用完毕请关闭空调及灯光，如有使用白板，请擦拭干净。 <br />4．会场内座椅如有移动，请回复原状。 <br />5．使用完毕后，请立即通知专业管理单位人员。 <br />6．敬请爱惜使用，谢谢您的合作<br />' }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    },

    submit: function (action, data, params) {
        params = Ext.apply({}, params);

        this.callParent([action, data, params]);
    }
});


Ext.define('PIC.MeetingRoomApyFormToolbar', {
    extend: 'PIC.ExtFormToolbar',
    alias: 'widget.picmeetingroomapyformtoolbar',
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
