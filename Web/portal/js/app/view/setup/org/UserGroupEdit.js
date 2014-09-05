Ext.define('PIC.view.setup.org.UserGroupEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.UserGroupEditForm',

    requires: [
        'PIC.model.sys.org.UserGroupNode',
        'PIC.ctrl.sel.UserSelector'
    ],

    refId: null,
    mode: 'usergroup',
    roleList: [],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            pgAction: pgAction,
            mode: $.getQueryString("mode", "usergroup"),
            refId: $.getQueryString("id"),
            uid: $.getQueryString("uid"),
            gid: $.getQueryString("gid"),
            roleList: PICState["RoleList"] || []
        }, config);

        var allowChangeUser = !(config.mode === "usergroup") && (pgAction === "create");
        var allowChangeGroup = (!(config.mode === "usergroup") && !(config.mode === "groupuser"));
        
        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        var rids = [];
                        var checked = me.ckgRole.getChecked();

                        Ext.each(checked, function (chk) {
                            rids.push(chk.inputValue);
                        });

                        me.formPanel.submit({ uid: me.uid, gid: me.gid, rids: rids, mode: me.mode });
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '用户标识', name: 'UserID', id: 'fld_UserID', hidden: true, allowBlank: false },
                { fieldLabel: '组标识', name: 'GroupID', id: 'fld_GroupID', hidden: true, allowBlank: false },
                { fieldLabel: '角色标识', name: 'RoleID', id: 'fld_RoleID', hidden: true },
                { fieldLabel: '用户', name: 'UserName', xtype: 'picuserselector', disabled: !allowChangeUser, allowBlank: false,
                    fieldMap: { fld_UserID: "UserID" },
                    selparams: { mode: me.mode },
                    multiSelect: true 
                },
                { fieldLabel: '组', name: 'GroupName', disabled: !allowChangeGroup, allowBlank: false },
                { xtype: 'container', flex: 2, height: 10 },
                {
                    xtype: 'fieldset',
                    title: '角色',
                    flex: 2,
                    items: {
                        xtype: 'checkboxgroup',
                        id: 'ckg_RoleName',
                        flex: 2,
                        columns: 4,
                        vertical: true,
                        items: []
                    }
                },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用户在指定组的角色信息' }
            ],
            listeners: {
                afterrender: function () {
                    me.ckgRole = Ext.getCmp("ckg_RoleName");
                    me.ckgRole.removeAll();

                    var rids = (me.formPanel.frmdata["RoleID"] || "").split(",");

                    Ext.each(me.roleList, function (r) {
                        var _cb = { boxLabel: r["Name"], name: r["Code"], inputValue: r["RoleID"] };
                        _cb.checked = ($.inArray(r["RoleID"], rids) > -1);

                        me.ckgRole.add(_cb);
                    });
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
