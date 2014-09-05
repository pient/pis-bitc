Ext.define('PIC.view.com.bpm.FlowBasicFormPanel', {
    extend: 'PIC.BpmFormPanel',
    alternateClassName: 'PIC.Bpm.FlowBasicFormPanel',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            // title: '基本信息'
            currentStage: 'start'
        }, config);

        config.items = [
                { fieldLabel: '流程名称', name: 'DefineName', readOnly: true, allowBlank: false },
                { fieldLabel: '流程定义编号', name: 'DefineCode', readOnly: true, allowBlank: false },
                { fieldLabel: '编号', name: 'Code', readOnly: true, value: '提交后生成' },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.WfInstanceModel.StatusEnum, readOnly: true },
                { fieldLabel: '申请人', name: 'ApplicantName', readOnly: true, allowBlank: false },
                { fieldLabel: '部门名称', name: 'DeptName', readOnly: true },
                { fieldLabel: '创建时间', name: 'CreatedTime', xtype: 'picdatefield', readOnly: true },
                { fieldLabel: '启动时间', name: 'StartedTime', xtype: 'picdatefield', readOnly: true },
                { fieldLabel: '标题', name: 'Title', flex: 2, readOnly: !"start".equals(config.currentStage), allowBlank: false },
                { fieldLabel: '表单路径', name: 'FormPath', hidden: true },
                { fieldLabel: '部门标识', name: 'DeptID', hidden: true },
                { fieldLabel: '实例标识', name: 'InstanceID', hidden: true },
                { fieldLabel: '申请人标识', name: 'ApplicantID', hidden: true }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});