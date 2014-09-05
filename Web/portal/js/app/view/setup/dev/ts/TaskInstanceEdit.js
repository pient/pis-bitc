Ext.define('PIC.view.setup.dev.ts.TaskInstanceEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DevTaskInstanceEditForm',

    requires: [
        'PIC.model.sys.dev.TaskInstance'
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
                items: ['-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.TaskInstanceModel.StatusEnum },
                { fieldLabel: '运行结果', name: 'Result', flex: 2 },
                { fieldLabel: '开始时间', name: 'StartedTime', xtype: 'picdatefield' },
                { fieldLabel: '结束时间', name: 'EndedTime', xtype: 'picdatefield' },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：任务' },
                { fieldLabel: '标识', name: 'TaskInstanceID', hidden: true }
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
