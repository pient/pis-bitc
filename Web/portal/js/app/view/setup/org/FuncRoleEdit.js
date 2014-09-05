Ext.define('PIC.view.setup.org.FuncRoleEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.FuncRoleEditForm',

    requires: [
        'PIC.model.sys.org.FuncRole',
        'PIC.ctrl.sel.RoleSelector'
    ],

    refId: null,
    mode: 'funcrole',

    constructor: function (config) {
        var me = this;

        me.refId = $.getQueryString("id") || $.getQueryString("fid");
        me.mode = $.getQueryString("mode", "funcrole");

        config = Ext.apply({
            pgAction: pgAction
        }, config);

        var allowChangeFunc = !(me.mode === "func");
        var allowRoleMultiSelect = (config.pgAction === "create");

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
                { fieldLabel: '标识', name: 'FunctionRoleID', hidden: true },
                { fieldLabel: '职能标识', name: 'FunctionID', hidden: true, allowBlank: false },
                { fieldLabel: '角色标识', name: 'RoleID', id: 'fld_RoleID', hidden: true, allowBlank: false },
                { fieldLabel: '职能', name: 'FuncName', disabled: !allowChangeFunc, allowBlank: false },
                { fieldLabel: '角色', name: 'RoleName', xtype: 'picroleselector', selparams: { mode: "func" }, multiSelect: allowRoleMultiSelect, allowBlank: false,
                    fieldMap: { fld_RoleID: "RoleID" },
                    picbeforeload: function (proxy, params, node, operation) {
                        if (me.pgAction == "create") {
                            params.data.fid = me.refId;
                        } else {
                            params.data.id = me.formPanel.getFieldValue("FunctionRoleID");
                            params.data.fid = me.formPanel.getFieldValue("FunctionID");
                        }
                    }
                },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.FuncRoleModel.StatusEnum, enumvaltype: "int", value: 1, allowBlank: false },
                { fieldLabel: '扩展信息', name: 'Tag', flex: 2 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：职能角色编辑' }
            ],
            listeners: {
                afterrender: function () {
                    // 设置默认值
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
