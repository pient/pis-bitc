
//------------------------PIC ExtJs 流程表单面板 开始------------------------//

Ext.define('PIC.BpmFormPage', {
    extend: 'PIC.FormPage',
    alias: 'widget.pic-bpmformpage',

    stageState: {},
    currentStage: null,

    flowData: null,
    wfConfig: null,

    op: null,  // 操作
    did: null,  // workflow define id
    iid: null,  //  workflow instance id

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: $.getQueryString('op', pgOperation),
            did: $.getQueryString('did', ''),
            iid: $.getQueryString('iid', '')
        }, config);

        me.flowData = config.flowData || PICState['FlowData'];

        if (me.flowData['Define']) {
            me.wfConfig = $.getJsonObj(me.flowData['Define']['Config']) || {};
        } else if (me.flowData['Instance']) {
            me.wfConfig = $.getJsonObj(me.flowData['Instance']['Config']) || {};
        }

        me.callParent([config]);
    }
});

Ext.define('PIC.BpmFormPanel', {
    extend: 'PIC.ExtFormPanel',
    alias: 'widget.pic-bpmformpanel',

    stageStateApplyPhase: 'init',   // 应用状态时机，init(初始化), afterrender(afterrender事件内), 其他则不应用状态

    constructor: function (config) {
        var me = this;

        if (me.requireCls) {
            Ext.syncRequire(me.requireCls);
        }

        config = Ext.apply({
            stageStateApplyPhase: me.stageStateApplyPhase
        }, config);

        if (config.stageStateApplyPhase === 'init') {
            me.applyStageState(config);
        }

        if (typeof (me.onAfterInit) === "function") {
            me.onAfterInit();
        }

        me.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        me.callParent(arguments);

        if (me.stageStateApplyPhase === 'afterrender') {
            me.on('afterrender', function () {
                me.applyStageState('form');
            });
        }

        me.on('afterrender', function () {
            if (typeof (me.onAfterRender) === "function") {
                me.onAfterRender();
            }
        });
    },

    applyStageState: function (mode, stage, cfgitems) {
        // mode (config，form) 是对配置文件进行处理，还是对表单进行处理，默认配置文件，mode为config时，可能会用到cfgitems
        var me = this;

        mode = mode || 'config';

        if (typeof (mode) !== "string") {
            if ($.isArray(mode)) {
                // mode为config.items
                cfgitems = mode;
            } else {
                // mode为config
                if (mode.items) {
                    cfgitems = mode.items;
                }

                stage = stage || mode.currentStage;
            }

            mode = 'config';
        }

        me.stageState = me.stageState || {};

        stage = stage || me.currentStage || "";
        
        if (stage.toLowerCase() !== "start") {
            // default或Default都使用默认项
            if (me.stageState['default']) {
                me.doApplyStageState("default", mode, cfgitems);
            }
        }

        me.doApplyStageState(stage, mode, cfgitems);
    },

    doApplyStageState: function (stage, mode, cfgitems) {
        var me = this;
        if (!me.stageState) return;

        mode = mode || 'config';

        if (stage.toLowerCase() === "default") {
            if (mode === 'form') {
                me.setReadOnly(true);
            } else {
                // 处理默认config
                var _items = cfgitems || me.getConfigItems();

                Ext.each(_items, function (i) {
                    i.readOnly = (i.readOnly !== false);    // 默认全设置为只读
                });
            }
        }

        var state = me.stageState[stage];
        if (state) {
            for (var k in state) {
                me.doApplyStageItemState(k, state[k], mode, cfgitems);
            }
        }
    },

    doApplyStageItemState: function (fname, istate, mode, cfgitems) {
        var me = this, s = istate;

        var f;

        if (mode === 'form') {
            f = me.getFieldByName(fname);
        } else {
            f = me.getConfigItem(fname, cfgitems);
        }

        if (f) {
            if (typeof (istate) === 'string') {
                s = { Status: istate };
            }

            var _status = s.Status || '';

            if (_status) {
                switch (_status) {
                    case 'R':
                    case 'Readonly':
                    case 'ReadOnly':
                        if (mode === 'form') {
                            f.setVisible(true);
                            f.setReadOnly(true);
                        } else {
                            f.hidden = false;
                            f.readOnly = true;
                        }
                        break;
                    case 'D':
                    case 'Disabled':
                        if (mode === 'form') {
                            f.setDisabled(true);
                        } else {
                            f.disabled = true;
                        }
                        break;
                    case 'H':
                    case 'Hidden':
                        if (mode === 'form') {
                            f.setVisible(false);
                        } else {
                            f.hidden = true;
                        }
                        break;
                    case 'E':
                    case 'Edit':
                    case 'Editable':
                        if (mode === 'form') {
                            f.setVisible(true);
                            f.setReadOnly(false);
                            f.setDisabled(false);
                        } else {
                            f.hidden = false;
                            f.readOnly = false;
                            f.disabled = false;
                        }
                        break;
                }
            }
        }
    },

    getConfigItem: function (name, items) {
        var me = this;

        items = items || me.getConfigItems();

        var rtns = $.grep(items, function (n, i) {
            return (n["name"] === name) || (!n["name"] && n["dataIndex"] === name);
        });

        if (rtns.length > 0) {
            return rtns[0];
        }

        return null;
    },

    getConfigItems: function () {
        var me = this;

        // 处理默认config
        var _items = me.items;

        if (!_items && me.config && me.config.items) {
            _items = me.config.items;
        }

        return _items;
    }
});

//------------------------PIC ExtJs 流程表单面板 结束------------------------//