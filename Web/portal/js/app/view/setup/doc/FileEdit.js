Ext.define('PIC.view.setup.doc.FileEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DocFileEditForm',

    requires: [
        'PIC.model.doc.File'
    ],

    refId: null,
    catalogId: null,

    constructor: function (config) {
        var me = this;
        me.catalogId = $.getQueryString("cid") || opener.PICPage.catalogId;

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
                { fieldLabel: '标识', name: 'ParameterID', hidden: true },
                { fieldLabel: '分类标识', name: 'CatalogID', hidden: true, allowBlank: false },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '值', name: 'Value', allowBlank: false },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统参数编辑' }
            ],
            listeners: {
                afterrender: function () {
                    me.formPanel.setFieldValue("CatalogID", me.catalogId);
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
