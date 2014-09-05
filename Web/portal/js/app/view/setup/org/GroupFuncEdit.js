Ext.define('PIC.view.setup.org.GroupFuncEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.GroupFuncEditForm',

    requires: [
        'PIC.model.sys.org.GroupFunc',
        'PIC.ctrl.sel.FuncSelector'
    ],

    refId: null,
    mode: 'groupfunc',

    constructor: function (config) {
        var me = this;

        me.refId = $.getQueryString("id") || $.getQueryString("gid");
        me.mode = $.getQueryString("mode", "groupfunc");

        config = Ext.apply({
            pgAction: pgAction
        }, config);

        var allowChangeGroup = !(me.mode === "groupfunc");
        var allowChangeFunc = (config.pgAction === "create");

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
                { fieldLabel: '标识', name: 'GroupFunctionID', hidden: true },
                { fieldLabel: '组标识', name: 'GroupID', hidden: true, allowBlank: false },
                { fieldLabel: '职能标识', name: 'FunctionID', id: 'fld_FunctionID', hidden: true, allowBlank: false },
                { fieldLabel: '组', name: 'GroupName', disabled: !allowChangeGroup, allowBlank: false },
                { fieldLabel: '职能', name: 'FuncName', xtype: 'picfuncselector', selparams: { mode: "group" }, disabled: !allowChangeFunc, multiSelect: false, allowBlank: false,
                    fieldMap: { fld_FunctionID: "FunctionID" },
                    picbeforeload: function (proxy, params, node, operation) {
                        if (me.pgAction == "create") {
                            params.data.gid = me.refId;
                        } else {
                            params.data.id = me.formPanel.getFieldValue("GroupFunctionID");
                            params.data.gid = me.formPanel.getFieldValue("GroupID");
                        }
                    }
                },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.GroupFuncModel.StatusEnum, enumvaltype: "int", value: 1, allowBlank: false },
                { fieldLabel: '扩展信息', name: 'Tag', flex: 2 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：组职能编辑' }
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
