Ext.define('PIC.view.com.msg.MsgEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.MsgEditForm',

    requires: [
        'PIC.model.sys.msg.Message',
        'PIC.model.sys.org.User',
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
                    id: 'btnSend',
                    picexecutable: true,
                    iconCls: 'pic-icon-email-go',
                    text: '发送',
                    hidden: (config.type != "Sent" && config.type != "Draft"),
                    handler: function () { me.send(); }
                }, {
                    bttype: 'save',
                    hidden: (config.type != "Sent" && config.type != "Draft"),
                    handler: function () { me.save(); }
                }, '-', 'cancel', '->', /*'excel', 'word', */'-', 'help']
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
                { fieldLabel: '内容', name: 'Content', xtype: 'pichtmleditor', height: 280, flex: 2, allowBlank: false },
                { fieldLabel: '附件', name: 'Attachments', xtype: 'picfilefield', flex: 2, allowBlank: false },
                { xtype: 'picformdescpanel', flex: 2, html: "表单描述：发送消息<br />当选择保存时，消息将被保存为草稿，选择草稿界面可查看" }
            ],
            listeners: {
                afterrender: function () {
                    if (me.op === 'c') {
                        me.formPanel.setFieldValue('MessageID', "");
                        me.formPanel.setFieldValue('Important', "Normal");

                        var refMsg = PICState['RefMessage'];

                        if (refMsg) {
                            var subject = refMsg['Subject'] || "";

                            switch (me.sendtype) {
                                case 'forward':
                                    subject = "转发：" + subject;

                                    me.formPanel.setFieldValue('Attachments', refMsg['Attachments']);
                                    break;
                                case 'reply':
                                    subject = "回复：" + subject;

                                    me.formPanel.setFieldValue('ToIDs', refMsg['FromID']);
                                    me.formPanel.setFieldValue('ToNames', refMsg['FromName']);
                                    break;
                            }

                            me.formPanel.setFieldValue('Subject', subject);
                            me.formPanel.setFieldValue('Content', '&nbsp;<br/><hr />' + refMsg['Content']);
                        }
                    }

                    if (me.op === 'c' || me.op === 'u') {
                        me.formPanel.setFieldValue('FromName', PICState['UserInfo'].Name);
                    }
                }
            }
        });

        me.items = me.formPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    send: function () {
        var me = this;
        me.formPanel.submit('send', { sendtype: me.sendtype, refId: me.refId });
    },

    save: function () {
        var me = this;
        me.formPanel.submit('save');
    }
});
