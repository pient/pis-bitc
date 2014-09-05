Ext.define('PIC.view.setup.dev.ts.SchedulerEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DevSchedulerEditForm',

    requires: [
        'PIC.model.sys.dev.Scheduler'
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
                    bttype: 'save',
                    handler: function () {
                        me.formPanel.submit({ id: me.refId });
                    }
                }, {
                    xtype: 'piceditbutton',
                    text: '编辑配置',
                    id: 'btnConfig',
                    handler: function () {
                        me.loadCfgFrmWin();
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: PIC.SchedulerModel.TypeEnum, allowBlank: false },
                { fieldLabel: '类别', name: 'Catalog', xtype: 'picenumselect', enumdata: PIC.SchedulerModel.CatalogEnum, allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.SchedulerModel.StatusEnum, allowBlank: false },
                { fieldLabel: '配置', name: 'Config', xtype: 'picjsonarea', isformat: true, hidden: true, height: 300, flex: 2 },
                { fieldLabel: '备注', name: 'Memo', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：任务计划' },
                { fieldLabel: '标识', name: 'SchedulerID', hidden: true }
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
        var style = { width: 650, height: 350 };

        PICUtil.openDialog({
            url: "SchedulerCfgEdit.aspx",
            style: style
        });
    },

    getSchedulerData: function () {
        var schdata = this.formPanel.form.getValues();

        return schdata;
    },

    setConfigData: function (cfg) {
        cfg = cfg || "";

        if (typeof cfg === "object") {
            cfg = Ext.encode(cfg);
        }

        this.formPanel.setFieldValue("Config", cfg);
    }
});
