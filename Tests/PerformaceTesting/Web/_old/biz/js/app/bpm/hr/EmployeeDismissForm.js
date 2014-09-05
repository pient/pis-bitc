Ext.define('PIC.biz.bpm.hr.EmployeeDismissForm', {
    extend: 'PIC.BpmFormPanel',
    alternateClassName: 'PIC.Biz.EmployeeDismissForm',

    requires: [
        'PIC.model.com.Enums',
        'PIC.ctrl.sel.UserSelector',
        'PIC.ctrl.sel.RoleSelector',
        'PIC.ctrl.sel.GroupSelector'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.stageState = {
            'Default': { SBSJ: 'E', YGXM: 'E', XB: 'R' },
            'ZGFYZ_SP': { SBSJ: 'H', YGXM: 'D', XB: 'D' }   // 主管业务副院长审批
        };

        config.items = [
                { fieldLabel: '收表时间', name: 'SBSJ', xtype: 'picdatefield', showToday: true, allowBlank: false },
                {
                    fieldLabel: '姓名', name: 'YGXM', xtype: 'picuserselector', allowBlank: false,
                    fieldMap: { fld_YGID: "UserID" }
                },
                { fieldLabel: '性别', name: 'XB', xtype: 'picenumselect', enumdata: PIC.Enums["GenderEnum"] },
                { fieldLabel: '出生年月', name: 'CSNY', xtype: 'picdatefield' },
                { fieldLabel: '编制类别', name: 'BZLB' },
                {
                    fieldLabel: '部门', name: 'BM', xtype: 'picgroupselector', selparams: { type: 10 }, allowBlank: false,
                    fieldMap: { fld_BMID: "GroupID", fld_BMCode: "Code" }
                },
                { fieldLabel: '岗位', name: 'GW', xtype: 'picroleselector', selparams: { type: 10 } },
                { fieldLabel: '擅自脱岗时间', name: 'TGSJ', xtype: 'picdatefield' },
                { fieldLabel: '所在部门呈报时间', name: 'CBSJ', xtype: 'picdatefield' },
                { fieldLabel: '备注', name: 'BZ', xtype: 'pictextarea', flex: 2 },
                { fieldLabel: '解聘理由', name: 'JPLY', xtype: 'pictextarea', flex: 2 },
                { fieldLabel: '所在部门领导意见', name: 'BMLDYJ', xtype: 'pictextarea', flex: 2 },
                { fieldLabel: '主管领导意见', name: 'ZGLDYJ', xtype: 'pictextarea', flex: 2 },
                { fieldLabel: '附件', name: 'FJ', xtype: 'picfilefield', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：北京信息职业技术学院解聘人员呈报表' },

                { fieldLabel: '标识', name: 'ID', hidden: true },
                { fieldLabel: '姓名标识', name: 'YGID', id: 'fld_YGID', hidden: true },
                { fieldLabel: '部门编号', name: 'BMCode', id: 'fld_BMCode', hidden: true },
                { fieldLabel: '部门标识', name: 'BMID', id: 'fld_BMID', hidden: true }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});