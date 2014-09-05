Ext.define('PIC.view.setup.bpm.WfInstanceEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.WfInstanceEditForm',

    requires: [
        'PIC.model.sys.bpm.WfInstance'
    ],

    refId: null,

    constructor: function (config) {
        var me = this;

        me.refId = $.getQueryString("id");

        config = Ext.apply({
            op: pgOperation
        }, config);

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        me.formPanel.submit({ id: me.refId });
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '标识', name: 'DefineID', hidden: true },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '所有人', name: 'OwnerName', allowBlank: false },
                { fieldLabel: '发起人', name: 'CreatorName', allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.WfInstanceModel.StatusEnum, value: "Enabled", allowBlank: false },
                { fieldLabel: '附加信息', name: 'Tag', xtype: 'picjsonarea', isformat: true, flex: 2, height: 300 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：流程实例表单' }
            ]
        });

        me.items = me.formPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});
