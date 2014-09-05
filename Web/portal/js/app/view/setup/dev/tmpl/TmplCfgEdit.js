Ext.define('PIC.view.setup.dev.tmpl.TmplCfgEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DevTmplCfgEditForm',

    requires: [
        'PIC.model.sys.dev.Template',
        'PIC.model.sys.dev.TemplateCatalog'
    ],

    ownerForm: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            tmplModel: null
        }, config);

        me.ownerForm = config.ownerForm || window.opener.PICPage;
        me.cfgData = {};

        me.editorCfg = config.editorCfg || me.ownerForm.getCfgEditorConfig();  // 配置信息元数据(其中包括用于描述编辑器相关属性信息)
        me.tmplModel = config.tmplModel || me.ownerForm.getTmplModel();  // 模版数据

        if (me.tmplModel && me.tmplModel.getConfig) {
            me.cfgData = me.tmplModel.getConfig() || {};

            me.tmplType = me.tmplModel.get("Type"); // 模版类型
            me.tmplConfig = me.tmplModel.get("Config"); // 模版类型
        }

        me.editorCfg = me.cfgData.editorCfg || me.editorCfg;

        // 若me.editorCfg.Type得类型为空或Custom，则设置编辑器类型为模版类型
        if (me.tmplType && !"Custom".equals(me.tmplType)) {
            me.editorCfg.Type = me.tmplType
        }

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.loadForm();
    },

    loadForm: function () {
        var me = this;

        me.removeAll();

        var frmItems = [];

        switch (me.editorCfg.Type) {
            case "Standard":
                me.addStandardForm();
                break;
            case "Data":
                me.addDataForm();
                break;
            case "Code":
                me.addCodeForm();
                break;
            case "SN":
                me.addSNForm();
                break;
            case "Template":
                me.addTemplateForm();
                break;
            case "Custom":
            default:
                me.addCustomForm();
                break;
        }
    },

    addTemplateForm: function () {
        var me = this;

        var tmplCode = me.editorCfg.Code;

        if (me.editorCfg.FormCfg) {
            me.addForm(me.editorCfg.FormCfg);
        } else if (me.editorCfg.Code) {
            PICUtil.ajaxRequest('geteditortmpl', {
                onsuccess: function (respData, opts) {
                    if (respData.TmplData) {
                        var editorTmplModel = Ext.create('PIC.model.sys.dev.Template', respData.TmplData);
                        var cfg = editorTmplModel.getConfig();

                        if (cfg.formCfg) {
                            me.addForm(cfg.formCfg);
                        }
                    }
                }
            }, { code: tmplCode });
        } else {
            me.addCustomForm();
        }
    },

    addStandardForm: function () {
        var me = this;

        var frmItems = [
            { fieldLabel: "模版", name: "TemplateString", xtype: "pictextarea", flex: 2, height: 280, allowBlank: false },
            { fieldLabel: "数据上下文", name: "DataContextString", xtype: "picjsonarea", flex: 2, isformat: true, height: 100 },
            { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于标准配置信息的编辑' }
        ];

        me.addForm({
            items: frmItems
        });
    },
    
    addDataForm: function () {
        var me = this;

        var frmItems = [
            { fieldLabel: "数据上下文", name: "DataContextString", xtype: "picjsonarea", flex: 2, isformat: true, height: 300, allowBlank: false },
            { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于数据配置信息的编辑' }
        ];

        me.addForm({
            items: frmItems
        });
    },

    addCodeForm: function () {
        var me = this;

        var frmItems = [
            { fieldLabel: "模版", name: "TemplateString", xtype: "pictextarea", flex: 2, height: 100, allowBlank: false },
            { fieldLabel: "数据上下文", name: "DataContextString", xtype: "picjsonarea", flex: 2, isformat: true, height: 100 },
            { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于编码配置信息的编辑' }
        ];

        me.addForm({
            items: frmItems
        });
    },

    addSNForm: function () {
        var me = this;

        var frmItems = [
            { fieldLabel: '增长类型', name: 'IncreaseType', xtype: 'picenumselect', enumdata: PIC.TemplateModel.SNIncTypeEnum, allowBlank: false },
            { fieldLabel: '长度', name: 'Length', xtype: 'picnumberfield' },
            { fieldLabel: '当前序列号', name: 'CurrentSN', allowBlank: false },
            { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于序列号配置信息的编辑' }
        ];

        me.addForm({
            items: frmItems
        });
    },

    addCustomForm: function () {
        var me=this;

        var frmItems = [
            { fieldLabel: '配置脚本', name: 'Config', xtype: 'picjsonarea', isformat: true, flex: 2, height: 380 },
            { xtype: 'picformdescpanel', flex: 2, html: '表单描述：用于自定义配置信息的编辑' }
        ];

        me.addForm({
            items: frmItems
        });
    },

    addForm: function (cfg) {
        var me = this;

        var _cfg = Ext.apply({
            tbar: {
                items: [{
                    xtype: 'picsubmitbutton',
                    text: '确定',
                    handler: function () {
                        me.saveData();
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [],
            listeners: {
                afterrender: function () {
                    me.loadData();
                }
            }
        }, cfg || {});

        me.formPanel = Ext.create("PIC.ExtFormPanel", _cfg);

        me.add(me.formPanel);
    },

    loadData: function () {
        var me = this;

        var cfgdata = me.cfgData || {};

        switch (me.editorCfg.Type) {
            case "Custom":
                cfgdata = { Config: me.tmplConfig };
                break;
        }

        me.formPanel.form.setValues(cfgdata);
    },

    saveData: function () {
        var me = this;

        var cfgdata = "";

        switch (me.editorCfg.Type) {
            case "Custom":
                cfgdata = me.formPanel.getFieldValue("Config");
                break;
            default:
                cfgdata = Ext.encode(me.formPanel.getFormData());
                break;
        }

        me.ownerForm.setConfigData(cfgdata);

        window.close();
    }
});
