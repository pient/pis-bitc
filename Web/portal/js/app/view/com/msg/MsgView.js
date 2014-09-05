Ext.define('PIC.view.com.msg.MsgView', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.MsgViewForm',

    requires: [
        'PIC.model.sys.msg.Message',
        'PIC.ctrl.sel.UserSelector'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation,
            type: $.getQueryString('type', 'Draft'),
            toids: $.getQueryString('toids', PICState['ToIDs']),
            tonames: $.getQueryString('tonames', PICState['ToNames']),
            subject: $.getQueryString('subject', ''),
            sendtype: $.getQueryString('sendtype', ''),
            refId: $.getQueryString('id')
        }, config);

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    text: '回复',
                    iconCls: 'pic-icon-email-reply',
                    handler: function () { me.process('reply'); }
                }, {
                    text: '转发',
                    iconCls: 'pic-icon-email-go2',
                    handler: function () { me.process('forward'); }
                }, 'delete', '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '标识', name: 'MessageID', hidden: true },
                { fieldLabel: '消息组标识', name: 'GroupID', hidden: true },
                { fieldLabel: '接收人标识', name: 'ToIDs', id: 'fld_ToIDs', hidden: true },
                { fieldLabel: '发送人', name: 'FromName', disabled: true },
                { fieldLabel: '重要度', name: 'Important', xtype: 'picenumselect', enumdata: PIC.MessageModel.ImportantEnum, value: 'Normal' },
                {
                    fieldLabel: '接收人', name: 'ToNames', xtype: 'picuserselector', flex: 2, multiSelect: true, allowBlank: false,
                    fieldMap: { fld_ToIDs: "UserID" }
                },
                { fieldLabel: '主题', name: 'Subject', flex: 2, allowBlank: false },
                { fieldLabel: '附件', name: 'Attachments', xtype: 'picfilefield', flex: 2, allowBlank: false },
                { fieldLabel: '内容', name: 'Content', xtype: 'pichtmleditor', height: 280, flex: 2, allowBlank: false },
                { xtype: 'picformdescpanel', flex: 2, html: "表单描述：发送消息<br />当选择保存时，消息将被保存为草稿，选择草稿界面可查看" }
            ]
        });

        me.items = me.formPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    process: function (sendtype) {
        var me = this;

        var url = $.combineQueryUrl("MsgEdit.aspx", { op: 'c', sendtype: sendtype, refId: me.refId });

        window.location.href = url;
    }
});
