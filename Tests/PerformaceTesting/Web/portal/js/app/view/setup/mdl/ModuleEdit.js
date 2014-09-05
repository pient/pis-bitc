Ext.define('PIC.view.setup.mdl.ModuleEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.ModuleEditForm',

    requires: [
        'PIC.model.sys.mdl.Module'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation
        }, config);

        var ref_id = $.getQueryString({ ID: "id" });
        var allowChangeCode = (pgOperation == "createsub" || pgOperation == "createsib");

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        me.formPanel.submit({ id: ref_id });
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '标识', name: 'ModuleID', hidden: true },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', readOnly: !allowChangeCode, allowBlank: false },
                { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: PIC.ModuleModel.TypeEnum, enumvaltype: "int", allowBlank: false },
                { fieldLabel: '排序号', name: 'SortIndex', xtype: 'picnumberfield' },
                { id: 'fld_Status', fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.ModuleModel.StatusEnum, enumvaltype: "int", allowBlank: false },
                { fieldLabel: '图标', name: 'Icon' },
                { fieldLabel: 'URL', name: 'Url', flex: 2 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统模块编辑' }
            ],
            listeners: {
                afterrender: function () {
                    // 设置默认值
                    var code = me.formPanel.getFieldValue("Code", "");

                    switch (pgOp) {
                        case "createsub":
                        case "createsib":
                            if (pgOp == "createsub") {
                                me.formPanel.setFieldValue("Code", code + ".");
                            } else if (pgOp == "createsib") {
                                var _code = code.substr(0, code.lastIndexOf("."))
                                if (_code) { _code = _code + "."; }
                                me.formPanel.setFieldValue("Code", _code);
                            }
                            break;
                    }
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
