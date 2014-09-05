
Ext.define('PIC.view.setup.org.GroupEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.OrgGroupEditForm',

    requires: [
        'PIC.model.sys.org.Group',
        'PIC.model.sys.org.User',
        'PIC.ctrl.sel.UserSelector'
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
                { fieldLabel: '负责人标识', name: 'PrincipalID', id: 'fld_PrincipalID', hidden: true },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '属性', name: 'Attr', xtype: 'picenumselect', enumdata: PIC.GroupModel.AttrEnum, allowBlank: false },
                {
                    fieldLabel: '负责人', name: 'PrincipalName', xtype: 'picuserselector',
                    fieldMap: { fld_PrincipalID: "UserID" }
                },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.GroupModel.StatusEnum, enumvaltype: "int", value: 1, allowBlank: false },
                { fieldLabel: '排序号', name: 'SortIndex', xtype: 'picnumberfield' },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统用户组编辑' }
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
