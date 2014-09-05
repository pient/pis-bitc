Ext.define('PIC.view.setup.dev.ts.SchedulerCfgEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DevSchedulerCfgEditForm',

    refId: null,
    
    requires: [
        'PIC.model.sys.dev.Task',
        'PIC.view.setup.dev.ts.TaskSelector'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            schdata: null
        }, config);

        me.ownerForm = window.opener.PICPage;

        var schdata = config.schdata || me.ownerForm.getSchedulerData();
        var cfgdata = Ext.decode(schdata.Config || "{}");

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        var frmdata = me.formPanel.form.getValues();

                        me.ownerForm.setConfigData(frmdata);

                        window.close();
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                {
                    fieldLabel: '调度任务', name: 'TaskName', xtype: 'pic-tstaskselector', allowBlank: false,
                    fieldMap: { fld_TaskID: "TaskID" }
                },
                { fieldLabel: '开始时间', name: 'StartTime', xtype: 'picdatefield', showToday: true, allowBlank: false },
                { fieldLabel: '结束时间', name: 'EndTime', xtype: 'picdatefield', allowBlank: false },
                { fieldLabel: '执行次数', name: 'RepeatCount', xtype: 'picnumberfield', value: 1, allowBlank: false },
                { fieldLabel: '执行间隔', name: 'RepeatInterval', xtype: 'picnumberfield', flex: .6, value: 60, allowBlank: false },
                { xtype: 'container', padding:'2', html: '(分钟)' },
                { fieldLabel: 'Cron表达式', name: 'CronString', flex: 2 },
                { fieldLabel: '备注', name: 'Memo', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统将优先将<a href="CornExpression.htm" target="_blank">Cron表达式</a>作为执行根据' },
                { id: 'fld_TaskID', fieldLabel: '标识', name: 'TaskID', hidden: true }
            ],
            listeners: {
                afterrender: function () {
                    me.formPanel.form.setValues(cfgdata || {});
                }
            }
        });

        me.items = me.formPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});
