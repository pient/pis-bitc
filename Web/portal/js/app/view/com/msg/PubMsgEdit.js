Ext.define('PIC.view.com.msg.PubMsgEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.PubMsgEditForm',

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
            subject: $.getQueryString('subject', ''),
            refId: $.getQueryString('id')
        }, config);

        me.catalog = $.getQueryString('catalog', '');

        var frmdata = PICState.frmdata || {};

        var isAttachmentsRequired = 'PicNews'.equals(me.catalog);
        var attachmentsMode = 'PicNews'.equals(me.catalog) ? 'single' : 'multi';
        var isAllowPublish = !'Published'.equals(frmdata.Status);
        var isAllowUnPublish = 'Published'.equals(frmdata.Status);

        var frmItems = [
            { fieldLabel: '标识', name: 'MessageID', hidden: true },
            { fieldLabel: '标题', name: 'Subject', flex: 2, allowBlank: false },
            { fieldLabel: '作者', name: 'FromName', readOnly: true },
            { fieldLabel: '状态', name: 'Status', readOnly: true, xtype: 'picenumselect', enumdata: PIC.MessageModel.PubInfoStatusEnum, allowBlank: false },
            { fieldLabel: '类别', name: 'Catalog', readOnly: true, xtype: 'picenumselect', enumdata: PIC.MessageModel.PubInfoCatalogEnum, allowBlank: false },
            { fieldLabel: '重要度', name: 'Important', xtype: 'picenumselect', enumdata: PIC.MessageModel.ImportantEnum, value: 'Normal' },
            { fieldLabel: '过期时间', name: 'ExpiredDate', xtype: 'picdatefield' }
        ];

        if ('Links'.equals(me.catalog)) {
            frmItems.push({ fieldLabel: 'URL', name: 'Content', flex: 2, value: 'http://', allowBlank: false });
        } else {
            frmItems.push({ fieldLabel: '附件', name: 'Attachments', xtype: 'picfilefield', flex: 2, mode: attachmentsMode, allowBlank: !isAttachmentsRequired });
            frmItems.push({ fieldLabel: '内容', name: 'Content', xtype: 'pichtmleditor', height: 280, flex: 2, allowBlank: false });
        }

        frmItems.push({ xtype: 'picformdescpanel', flex: 2, html: "表单描述：公共信息发布<br />当选择保存时，消息将被保存为草稿，选择草稿界面可查看" });

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    bttype: 'save',
                    handler: function () { me.doSave(); }
                }, {
                    picexecutable: true,
                    hidden: !isAllowPublish,
                    iconCls: 'pic-icon-publish',
                    text: '发布',
                    handler: function () { me.doPublish(); }
                }, {
                    picexecutable: true,
                    hidden: !isAllowUnPublish,
                    iconCls: 'pic-icon-unpublish',
                    text: '撤销发布',
                    handler: function () { me.unPublish(); }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: frmItems,
            listeners: {
                afterrender: function () {
                    if (me.op === 'c') {
                        if (!me.formPanel.getFieldValue('Status')) {
                            me.formPanel.setFieldValue('Status', "New");
                        }

                        me.formPanel.setFieldValue('MessageID', "");
                        me.formPanel.setFieldValue('Important', "Normal");
                    }

                    if (me.op === 'c' || me.op === 'u') {
                        me.formPanel.setFieldValue('FromName', PICState['UserInfo'].Name);
                    }
                    if (me.catalog) {
                        me.formPanel.setFieldValue('Catalog', me.catalog);
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

    doSave: function () {    // 暂存消息
        this.formPanel.submit('save');
    },

    doPublish: function (flag) {    // 发布或者回收消息
        this.formPanel.submit('publish');
    },

    unPublish: function () {    // 发布或者回收消息
        this.formPanel.submit('unpublish');
    }
});
