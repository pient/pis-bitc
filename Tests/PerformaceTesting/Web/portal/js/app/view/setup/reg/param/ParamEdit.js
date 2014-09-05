Ext.define('PIC.view.setup.reg.param.ParamEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.RegParamEditForm',

    requires: [
        'PIC.model.sys.reg.Parameter'
    ],

    refId: null,
    catalogId: null,
    currentCatalog: null,

    constructor: function (config) {
        var me = this;

        me.catalogId = $.getQueryString("cid") || opener.PICPage.catalogId;
        me.currentCatalog = opener.PICPage.currentCatalog || null;

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
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.ParameterModel.StatusEnum, value: 'Enabled', allowBlank: false },
                { fieldLabel: '值类型', name: 'Type', xtype: 'picenumselect', hidden: true, enumdata: PIC.ParameterModel.DataTypeEnum, value: 'String' },
                { fieldLabel: '值', name: 'Value', xtype: 'pictextarea', flex: 2, allowBlank: false },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统参数编辑' },
                { fieldLabel: '标识', name: 'ParameterID', hidden: true },
                { fieldLabel: '分类标识', name: 'CatalogID', hidden: true, allowBlank: false }
            ],
            listeners: {
                afterrender: function () {
                    me.formPanel.setFieldValue("CatalogID", me.catalogId);

                    if (me.currentCatalog && me.currentCatalog["Code"]) {
                        me.formPanel.setFieldValue("Code", me.currentCatalog["Code"] + ".");
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
