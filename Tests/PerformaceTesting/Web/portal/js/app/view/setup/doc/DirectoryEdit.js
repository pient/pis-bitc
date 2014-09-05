Ext.define('PIC.view.setup.doc.DirectoryEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DocDirectoryEditForm',

    requires: [
        'PIC.model.doc.Directory'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation
        }, config);

        var ref_id = $.getQueryString({ ID: "id" })

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
                { fieldLabel: '标识', name: 'DirectoryID', hidden: true },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '扩展信息', name: 'Tag', flex: 2 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：文档目录' }
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
