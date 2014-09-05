Ext.define('PIC.view.setup.dev.ts.TaskEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DevTaskEditForm',

    requires: [
        'PIC.model.sys.dev.Task'
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
                    bttype: 'save',
                    handler: function () {
                        me.formPanel.submit({ id: me.refId });
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: PIC.TaskModel.TypeEnum, allowBlank: false },
                { fieldLabel: '类别', name: 'Catalog', xtype: 'picenumselect', enumdata: PIC.TaskModel.CatalogEnum, allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.TaskModel.StatusEnum, allowBlank: false },
                { fieldLabel: '配置', name: 'Config', xtype: 'picjsonarea', isformat: true, height: 200, flex: 2 },
                { fieldLabel: '备注', name: 'Memo', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：任务' },
                { fieldLabel: '标识', name: 'TaskID', hidden: true }
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
