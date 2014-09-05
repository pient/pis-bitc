Ext.define('PIC.view.setup.org.UserEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.OrgUserEditForm',

    requires: [
        'PIC.model.sys.org.User',
        'PIC.ctrl.sel.UserSelector',
        'PIC.ctrl.sel.GroupSelector'
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
                { fieldLabel: '姓名', name: 'Name', allowBlank: false },
                { fieldLabel: '登录名', name: 'LoginName', allowBlank: false },
                { fieldLabel: '工号', name: 'WorkNo' },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.UserModel.StatusEnum, enumvaltype: "int", value: 1, allowBlank: false },
                {
                    fieldLabel: '上级', name: 'ReportToName', xtype: 'picuserselector', allowBlank: false,
                    fieldMap: { fld_ReportToID: "UserID" }
                },
                {
                    fieldLabel: '部门', name: 'DeptName', xtype: 'picgroupselector', selparams: { type: 10 }, allowBlank: false,
                    fieldMap: { fld_DeptID: "GroupID" }
                },
                { fieldLabel: '电子邮箱', name: 'Email', flex: 2 },
                { fieldLabel: '备注', name: 'Remark', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统用户编辑' },
                { fieldLabel: '标识', name: 'UserID', hidden: true },
                { fieldLabel: '上级标识', name: 'ReportToID', id: 'fld_ReportToID', hidden: true },
                { fieldLabel: '部门标识', name: 'DeptID', id: 'fld_DeptID', hidden: true }
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
