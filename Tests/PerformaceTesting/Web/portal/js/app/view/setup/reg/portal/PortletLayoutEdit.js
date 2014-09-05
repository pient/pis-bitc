Ext.define('PIC.view.setup.reg.portal.PortletLayoutEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.RegPortletLayoutEditForm',

    requires: [
        'PIC.model.sys.reg.PortletLayout'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation
        }, config);

        var ref_id = $.getQueryString({ ID: "id" })

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        me.formPanel.submit({ id: ref_id });
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '标识', name: 'LayoutID', hidden: true },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: PIC.PortletLayoutModel.TypeEnum, allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.PortletLayoutModel.StatusEnum, allowBlank: false },
                { fieldLabel: '布局配置', name: 'Config', xtype: 'picjsonarea', isformat: true, flex: 2, height: 260 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：门户布局编辑' }
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
