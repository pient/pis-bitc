Ext.define('PIC.view.setup.reg.portal.PortletCfgEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.RegPortletCfgEditForm',

    ownerForm: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            pltdata: null
        }, config);

        me.ownerForm = window.opener.PICPage;

        var pltdata = config.pltdata || me.ownerForm.getPortletData();
        var cfgdata = null;

        var frmItems = [];

        switch (pltdata.Type) {
            case "Custom":
                cfgdata = { Config: pltdata.Config };
                frmItems = [
                    { fieldLabel: '配置脚本', name: 'Config', xtype: 'picjsonarea', isformat: true, flex: 2, height: 400 },
                    { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于自定义型门户块配置信息的编辑' }
                ];
                break;
            case 'Data':
                cfgdata = Ext.decode(pltdata.Config || "{}");
                frmItems = [
                   { fieldLabel: '获取条数', name: 'rows', xtype: 'picnumberfield', allowBlank: false },
                   { fieldLabel: '参数类型信息', name: 'dttypes', flex: 2 },
                   { fieldLabel: '数据HQL', name: 'hql', xtype: 'pictextarea', flex: 2 },
                   { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于获取服务器数据配置信息的编辑' }
                ];
                break;
            case 'Template':
                cfgdata = Ext.decode(pltdata.Config || "{}");
                frmItems = [
                    { fieldLabel: '标题', name: 'title', flex: 2, allowBlank: false },
                   { fieldLabel: '获取条数', name: 'rows', xtype: 'picnumberfield', allowBlank: false },
                   { fieldLabel: '参数类型信息', name: 'dttypes', flex: 2 },
                   { fieldLabel: '数据HQL', name: 'hql', xtype: 'pictextarea', flex: 2, height: 80 },
                    { fieldLabel: '模版', name: 'tmpl', xtype: 'pictextarea', flex: 2, height: 230 },
                   { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于根据模版展示配置信息的编辑' }
                ];
                break;
            default:
                cfgdata = Ext.decode(pltdata.Config || "{}");
                frmItems = [
                    { fieldLabel: '标题', name: 'title', flex: 2, allowBlank: false },
                    { fieldLabel: '显示条数', name: 'rows', xtype: 'picnumberfield', allowBlank: false },
                    { fieldLabel: '高度', name: 'height', xtype: 'picnumberfield' },
                    { fieldLabel: '宽度', name: 'width', xtype: 'picnumberfield', allowBlank: false },
                    { fieldLabel: '数据HQL', name: 'hql', xtype: 'pictextarea', flex: 2 },
                    { fieldLabel: '标题头', name: 'header', xtype: 'pictextarea', flex: 2, height: 40 },
                    { fieldLabel: '列表项', name: 'item', xtype: 'pictextarea', flex: 2, height: 120 },
                    { fieldLabel: '页脚', name: 'footer', xtype: 'pictextarea', flex: 2, height: 40 },
                    { fieldLabel: '相关脚本', name: 'script', xtype: 'pictextarea', flex: 2, height: 80 },
                    { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于自定义型门户块配置信息的编辑' }
                ];
                break;
        }

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        var frmdata = me.formPanel.form.getValues();

                        me.ownerForm.setConfigData(frmdata);

                        window.close();
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: frmItems,
            listeners: {
                afterrender: function () {
                    me.formPanel.form.setValues(cfgdata || {});
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
