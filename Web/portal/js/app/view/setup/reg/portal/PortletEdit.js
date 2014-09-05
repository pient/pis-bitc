Ext.define('PIC.view.setup.reg.portal.PortletEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.RegPortletEditForm',

    requires: [
        'PIC.model.sys.reg.Portlet',
        'PIC.view.setup.reg.portal.PortletCfgEdit'
    ],

    cfgFormWin: null,
    cachedCfgFormWins: [],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation
        }, config);

        var ref_id = $.getQueryString({ ID: "id" });

        me.formPanel = Ext.create("PIC.ExtFormPanel", {
            tbar: {
                items: [{
                    xtype: 'picsavebutton',
                    id: 'btnSubmit',
                    handler: function () {
                        me.formPanel.submit({ id: ref_id });
                    }
                }, '-', {
                    xtype: 'piceditbutton',
                    text: '编辑配置',
                    id: 'btnConfig',
                    handler: function () {
                        me.loadCfgFrmWin();
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: PIC.PortletModel.TypeEnum, allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.PortletModel.StatusEnum, allowBlank: false },
                { fieldLabel: '模块', name: 'DataModule', xtype: 'picenumselect', enumdata: PIC.PortletModel.ModuleEnum, allowBlank: false },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：门户块编辑' },
                { fieldLabel: '标识', name: 'PortletID', hidden: true },
                { fieldLabel: '配置', name: 'Config', hidden: true }
            ]
        });

        me.items = me.formPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    loadCfgFrmWin: function () {
        var me = this;
        var style = { width: 650, height: 550 };

        var cfgType = this.formPanel.getFieldValue('Type');

        switch (cfgType) {
            case "Data":
                style = { width: 650, height: 350 };
                break;
        }

        PICUtil.openDialog({
            url: "PortletCfgEdit.aspx",
            style: style
        });
    },

    getPortletData: function () {
        var pltdata = this.formPanel.form.getValues();

        return pltdata;
    },

    setConfigData: function (cfg) {
        cfg = cfg || "";

        var cfgType = this.formPanel.getFieldValue('Type');

        if ("Custom".equals(cfgType)) {
            cfg = Ext.decode(cfg["Config"]);
        }

        if (typeof cfg === "object") {

            cfg = Ext.encode(cfg);
        }

        this.formPanel.setFieldValue("Config", cfg);
    }
});
