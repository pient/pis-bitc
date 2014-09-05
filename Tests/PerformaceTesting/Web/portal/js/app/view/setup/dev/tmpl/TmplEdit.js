Ext.define('PIC.view.setup.dev.tmpl.TmplEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.DevTmplEditForm',

    requires: [
        'PIC.model.sys.dev.Template',
        'PIC.model.sys.dev.TemplateCatalog',
        'PIC.view.setup.dev.tmpl.TmplCfgEdit'
    ],

    cfgFormWin: null,
    cachedCfgFormWins: [],

    refId: null,
    catalogId: null,
    currentCatalog: null,

    constructor: function (config) {
        var me = this;

        me.catalogId = $.getQueryString("cid") || opener.PICPage.catalogId;
        me.currentCatalog = opener.PICPage.currentCatalog || null;

        if (me.currentCatalog) {
            me.tmplItemConfig = me.currentCatalog.getItemConfig();
        }

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
                }, {
                    xtype: 'piceditbutton',
                    text: '编辑配置',
                    id: 'btnConfig',
                    handler: function () {
                        me.loadCfgFrmWin();
                    }
                }, '-', {
                    text: '测试',
                    iconCls: 'pic-icon-test',
                    handler: function () {
                        me.doTest();
                    }
                }, '-', 'cancel', '->', '-', 'help']
            },
            items: [
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', allowBlank: false },
                { fieldLabel: '类型', name: 'Type', xtype: 'picenumselect', enumdata: PIC.TemplateModel.TypeEnum, value: 'Custom' },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.TemplateModel.StatusEnum, value: 'Enabled', allowBlank: false },
                { fieldLabel: '排序号', name: 'SortIndex', xtype: 'picnumberfield', value: 100 },
                { fieldLabel: '描述', name: 'Description', xtype: 'pictextarea', flex: 2 },
                { xtype: 'picformdescpanel', flex: 2, html: '表单描述：系统模版编辑' },
                { fieldLabel: '标识', name: 'TemplateID', hidden: true },
                { fieldLabel: '分类标识', name: 'CatalogID', hidden: true, allowBlank: false },
                { fieldLabel: '配置', name: 'Config', hidden: true }
            ],
            listeners: {
                afterrender: function () {
                    me.onFormLoad();
                }
            }
        });

        me.items = me.formPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    onFormLoad: function () {
        var me = this;

        me.formPanel.setFieldValue("CatalogID", me.catalogId);

        if (me.currentCatalog) {
            var catalogCode = me.currentCatalog.get("Code");

            if (catalogCode) {
                me.formPanel.setFieldValue("Code", catalogCode + ".");
            }

            if (me.tmplItemConfig && me.tmplItemConfig["DefTmplType"]) {
                me.formPanel.setFieldValue("Type", me.tmplItemConfig["DefTmplType"]);
            }
        }
    },

    doTest: function (tmplId, tmplType) {
        var me = this;

        tmplId = tmplId || me.formPanel.getFieldValue("TemplateID");

        if (!tmplId) {
            PICMsgBox.warn("表单数据未创建，请先提交当前数据再执行模版生成测试操作。");
            return false;
        }

        var tmplType = tmplType || me.formPanel.getFieldValue("Type") || "Custom";
        var isPrompt = !"sn".equals(tmplType);
        var isAlert = "sn".equals(tmplType) || "code".equals(tmplType); // 是否弹出窗口显示数据，默认否

        var submitFunc = function (ctx_params) {
            var _params = PICUtil.getReqParams('update');

            _params.onsuccess = function (respData, opts) {
                if (isAlert) {
                    PICUtil.getTmplData('renderstr', {
                        onsuccess: function (respData, opts, resp) {
                            PICMsgBox.alert(resp.responseText);
                        }
                    }, { tid: tmplId, ctxparams: ctx_params });
                } else {
                    PICUtil.openTmplViewPage({
                        tid: tmplId,
                        ctxparams: $.getJsonString(ctx_params)
                    });
                }

                return false;
            }

            me.formPanel.submit(_params);
        };

        // 先更新保存数据，再进行测试
        if (isPrompt) {
            PICMsgBox.prompt('请输入数据上下文查询参数（Json）:', '参数输入', function (btn, text) {
                if (btn === "ok") {
                    var ctx_params = $.getJsonObj(text);

                    submitFunc(ctx_params);
                }
            }, this, "{}", true);
        } else {
            submitFunc({});
        }
    },

    doSave: function (afterreq) {
        var me = this;

        me.formDefine = me.formDefineEditor.getFieldValue('FormDefine');

        var afterreq = afterreq || function () {
            if (pgAction != 'create') {
                return false;
            }
        };

        if (me.tabPanel.validateForms([me.basicForm, me.formDefineEditor])) {
            var _params = {
                id: me.refId,
                formDefine: me.formDefine,
                afterrequest: afterreq
            };

            if (me.op === 'copy') {
                _params = PICUtil.getReqParams('copy', _params);
            }

            me.basicForm.submit(_params);
        }
    },

    loadCfgFrmWin: function () {
        var me = this;
        var style = { width: 650, height: 550 };

        if (me.tmplItemConfig && me.tmplItemConfig.CfgEditor) {
            style = me.tmplItemConfig.CfgEditor.Style || style;
        }

        PICUtil.openDialog({
            url: "TmplCfgEdit.aspx",
            style: style
        });
    },

    getTmplModel: function () {
        var tmpldata = this.getFormData();

        return Ext.create('PIC.model.sys.dev.Template', tmpldata);
    },

    getCfgEditorConfig: function () {
        var me = this;

        if (me.currentCatalog) {
            var itemCfg = me.currentCatalog.getItemConfig();

            return itemCfg.CfgEditor || null;
        }

        return null;
    },

    setConfigData: function (cfg) {
        var cfgstr = "";

        if (typeof (cfg) == "string") {
            cfgstr = cfg;
        } else {
            cfgstr = Ext.encode(cfg);
        }

        this.formPanel.setFieldValue("Config", cfg);
    }
});
