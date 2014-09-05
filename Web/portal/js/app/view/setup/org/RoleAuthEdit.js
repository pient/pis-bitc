Ext.define('PIC.view.setup.org.RoleAuthEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.RoleAuthEditForm',

    requires: [
        'PIC.model.sys.org.RoleAuth',
        'PIC.ctrl.sel.AuthSelector'
    ],

    refId: null,
    mode: 'user',

    constructor: function (config) {
        var me = this;

        me.refId = $.getQueryString("id") || $.getQueryString("rid");
        me.mode = $.getQueryString("mode", "roleauth");

        config = Ext.apply({
            pgAction: pgAction
        }, config);

        var allowChangeRole = !(me.mode == "roleauth");
        var allowChangeAuth = !(me.mode == "roleauth");

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        me.formPanel.submit();
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '标识', name: 'RoleAuthID', hidden: true },
                { fieldLabel: '角色标识', name: 'RoleID', hidden: true, allowBlank: false },
                { fieldLabel: '权限标识', name: 'AuthID', id: 'fld_AuthID', hidden: true, allowBlank: false },
                { fieldLabel: '角色', name: 'RoleName', disabled: !allowChangeRole, allowBlank: false },
                { fieldLabel: '权限', name: 'AuthName', disabled: !allowChangeAuth, allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.RoleAuthModel.StatusEnum, enumvaltype: "int", value: 1, allowBlank: false },
                { fieldLabel: '扩展信息', name: 'Tag', flex: 2 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：角色权限编辑' }
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
