
Ext.define('PIC.view.setup.auth.AuthEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.AuthEditForm',

    requires: [
        'PIC.model.sys.auth.Auth'
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
                { fieldLabel: '标识', name: 'GroupID', hidden: true },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.AuthModel.StatusEnum, enumvaltype: "int", value: 1, allowBlank: false },
                { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: PIC.AuthModel.TypeEnum, enumvaltype: "int", value: 1, allowBlank: false },
                { fieldLabel: '数据', name: 'Data', flex: 2 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统权限查看' }
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
