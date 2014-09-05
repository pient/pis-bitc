Ext.define('PIC.view.com.bpm.FlowPreview', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.BpmFlowPreview',

    requires: [
        'PIC.view.com.bpm.FlowBus'
    ],

    pageData: {},

    mainPanel: null,
    itemContextMenu: null,

    stage: null,  // 当前阶段
    did: null,  // workflow define id

    flowData: null,
    basicData: null,
    formDefineData: null,
    wfConfig: null,

    constructor: function (config) {
        var me = this;
        me.stage = $.getQueryString('stage', 'start');

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.pageData = config.pageData;

        me.initData();

        me.basicForm = Ext.create("PIC.Bpm.FlowBasicFormPanel", {
            isTabItem: true,
            frmdata: me.basicData
        });

        me.contentForm = null;
        var contentFormPath = PIC.BpmFlowBus.getContentFormPath(me.wfConfig, me.formDefineData);

        if (contentFormPath) {
            me.contentForm = Ext.create(contentFormPath, {
                // title: "审批内容",
                isTabItem: true,
                frmdata: me.contentData,
                currentStage: me.stage || config.currentStage || 'start'
            });
        }

        me.flowContentPanel = Ext.create("PIC.ExtPanel", {
            title: "表单",
            margins: '10 0 0 0',
            align: 'stretch',
            autoScroll: true,
            border: false,
            items: [me.basicForm, me.contentForm]
        });

        // var tabItems = [me.basicForm, me.contentForm];
        var tabItems = [me.flowContentPanel];

        me.tabPanel = Ext.create("PIC.ExtTabPanel", {
            region: 'center',
            activeTab: 0,
            border: false,
            width: document.body.offsetWidth - 5,
            items: tabItems
        });

        me.stageEnum = me.getStageEnum();
        me.stageSelector = Ext.create('PIC.ExtEnumSelect', {
            fieldLabel: '&nbsp;&nbsp;阶段选择',
            labelWidth: 70,
            allowBlank: false,
            enumdata: me.stageEnum,
            value: me.stage,
            listeners: {
                change: function () { me.onStageChange(); }
            }
        });

        me.mainPanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            layout: 'border',
            border: false,
            tbar: ['-', { xtype: 'picclosebutton' }, '-', me.stageSelector,
                '->', '-', { xtype: 'pichelpbutton' }],
            items: [me.tabPanel]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    initData: function () {
        var me = this;

        me.flowData = PICState['FlowData'];

        if (me.flowData) {
            me.defineData = me.flowData['Define'];
            me.basicData = me.flowData['Basic'];
            me.formDefineData = me.flowData['FormDefine'];
            me.wfConfig = me.flowData['Config'];

            if (me.wfConfig && me.wfConfig["FormData"]) {
                me.contentData = me.wfConfig["FormData"];
            }
        }
    },

    validateForms: function (frms) {
        var me = this;
        var flag = true;

        Ext.each(frms, function (frm) {
            flag = frm.isValid();

            if (!flag) {
                if (frm.isTabItem == true) {
                    me.tabPanel.setActiveTab(frm);
                } else {
                    // 向上找到tabItem
                    var upTab = frm.up("[isTabItem=true]");
                    if (upTab) {
                        me.tabPanel.setActiveTab(upTab);

                        return false;
                    }
                }

                return false;
            }
        });

        return flag;
    },

    onStageChange: function () {
        var me = this;
        var _stage = me.stageSelector.getValue();

        var _url = $.combineQueryUrl(window.location.href, { stage: _stage });

        window.location.href = _url;
    },

    getStageEnum: function () {
        var me = this;

        var stageEnum = { 'start': '开始', 'default': '默认' };

        if (me.contentForm && me.contentForm.stageState) {
            for (var key in me.contentForm.stageState) {
                if (!stageEnum[key]) {
                    stageEnum[key] = key;
                }
            }
        }

        return stageEnum;
    }
});

