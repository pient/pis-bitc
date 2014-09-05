Ext.define('PIC.view.setup.bpm.WfDefineEdit', {
    extend: 'PIC.FormPage',
    alternateClassName: 'PIC.WfDefineEditForm',

    requires: [
        'PIC.model.sys.bpm.WfDefine',
        'PIC.view.com.bpm.FlowBus'
    ],

    refId: null,
    formDefine: null,
    diagramPath: null,
    diagramUrl: null,

    constructor: function (config) {
        var me = this;

        me.refId = $.getQueryString("id");

        config = Ext.apply({
            op: pgOperation
        }, config);

        me.initData();

        me.basicForm = Ext.create("PIC.ExtFormPanel", {
            title: '基本信息',
            items: [
                { fieldLabel: '标识', name: 'DefineID', hidden: true },
                { fieldLabel: '名称', name: 'Name', allowBlank: false },
                { fieldLabel: '编号', name: 'Code', readOnly: false, emptyText: '提交后生成' },
                { fieldLabel: '分类号', name: 'Catalog', xtype: 'picenumselect', enumdata: PIC.WfDefineModel.CatalogEnum, allowBlank: false },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.WfDefineModel.StatusEnum, value: "Enabled", allowBlank: false },
                { fieldLabel: '排序号', name: 'SortIndex', xtype: 'picnumberfield', value: 100 },
                { fieldLabel: '配置', name: 'Config', xtype: 'picjsonarea', isformat: true, flex: 2, height: 300 },
                { fieldLabel: '备注', name: 'Memo', xtype: 'pictextarea', flex: 2 }
            ]
        });

        me.formDefineEditor = Ext.create("PIC.ExtFormPanel", {
            title: '流程表单',
            frmdata: { FormDefine: me.formDefine },
            tbar: {
                items: [{
                    text: '预览',
                    iconCls: 'pic-icon-preview',
                    handler: function () {
                        me.doPreview();
                    }
                }, '-', {
                    text: '测试',
                    iconCls: 'pic-icon-test',
                    handler: function () {
                        me.doTest();
                    }
                }]
            },
            items: [
                { name: 'FormDefine', xtype: 'picjsonarea', isformat: true, allowBlank: false, flex: 2, height: 400 }
            ]
        });

        me.tabItems = [me.basicForm, me.formDefineEditor];

        // 当流程不是创建状态时，显示流程图
        if (pgAction !== 'create' && me.diagramUrl) {
            me.flowDiagramPanel = Ext.create("PIC.ExtPanel", {
                title: '流程图',
                fieldset: false,
                autoScroll: true,
                html: '<div><img src="' + me.diagramUrl + '" style="width: 700px;" /></div>'
            });

            me.tabItems.push(me.flowDiagramPanel);
        }

        me.tabPanel = Ext.create("PIC.ExtFormTabPanel", {
            region: 'center',
            activeTab: 0,
            border: false,
            width: document.body.offsetWidth - 5,
            items: me.tabItems
        });

        me.items = Ext.create("PIC.ExtPanel", {
            region: 'center',
            layout: 'border',
            border: false,
            tbar: [{
                xtype: 'picsavebutton',
                handler: function () {
                    me.doSave();
                }
            }, {
                xtype: 'picdeletebutton',
                hidden: (me.hasInstance || 'copy'.equals(config.op) || 'c'.equals(config.op)),
                handler: function () {
                    me.doDelete();
                }
            }, '-', { xtype: 'picclosebutton' }, '->', '-', { xtype: 'pichelpbutton' }],
            items: [me.tabPanel]
        });

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.on('afterrender', function () {
            // 所有tabItem都遍历激活一下，以使其自动渲染
            Ext.each(me.tabItems, function (ti) {
                me.tabPanel.setActiveTab(ti);
            });

            me.tabPanel.setActiveTab(me.tabItems[0]);

            if (me.op === 'copy') {
                me.basicForm.setFieldValue('DefineID', '');
                me.basicForm.setFieldValue('Code', '');
                me.basicForm.setFieldValue('Name', '');
            }
        })
    },

    initData: function () {
        var me = this;

        me.formDefine = PICState["FormDefine"] || "";
        me.hasInstance = !!PICState["HasInstance"] || false;

        me.diagramPath = PICState["DiagramPath"] || "";
        me.diagramUrl = PIC.BpmFlowBus.getDiagramUrl(me.diagramPath);
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

    doDelete: function () {
        this.basicForm.submit('delete');
    },

    doPreview: function () {
        var me = this;

        me.doSave(function () {
            PICUtil.openFlowPreviewDialog(me.refId);
            return false;
        });
    },

    doTest: function () {
        var me = this;

        me.doSave(function () {
            PICUtil.openFlowBusDialog({ op: 'c', did: me.refId });

            return false;
        });
    }
});
