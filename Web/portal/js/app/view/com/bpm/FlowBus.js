Ext.define('PIC.view.com.bpm.FlowBus', {
    extend: 'PIC.Page',
    alternateClassName: ['PIC.BpmFlowBus'],

    requires: [
        'PIC.model.sys.bpm.WfDefine',
        'PIC.model.sys.bpm.WfInstance',

        'PIC.view.com.bpm.FlowBasicFormPanel',
        'PIC.view.com.bpm.FlowTrackingPanel',
        'PIC.view.com.bpm.FlowRunningPanel',
        'PIC.view.com.bpm.FlowApproveFormPanel'
    ],

    pageData: {},

    mainPanel: null,
    itemContextMenu: null,

    stage: null,  // 当前阶段
    did: null,  // workflow define id
    iid: null,  //  workflow instance id
    aid: null,  //  workflow action id

    wfConfig: null,
    flowData: null,
    basicData: null,
    instanceData: null,
    controlPoints: null,    // 控制点信息
    nextTasks: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.op = pgOperation;
        me.did = $.getQueryString('did', '');
        me.iid = $.getQueryString('iid', $.getQueryString('id', ''));
        me.aid = $.getQueryString('aid', '');

        me.pageData = config.pageData;

        me.initData();

        if (me.instanceData) {
            me.instanceStatus = me.instanceData.Status;
            me.iid = me.iid || me.instanceData["InstanceID"];
        }

        if (pgAction == 'update' || pgAction == 'create') {
            me.stage = 'start';
        }

        if (me.flowStateData) {
            if (me.flowStateData["Current"]) {
                me.stage = me.flowStateData["Current"]["TaskCode"];
            }

            if (me.flowStateData["Request"] && me.flowStateData["Request"]["ActionInfo"]) {
                me.aid = me.aid || me.flowStateData["Request"]["ActionInfo"]["ActionID"];
            }
        }

        me.displayDelete = (me.iid && ("update".equals(pgAction)) && me.instanceData && ("New".equals(me.instanceData.Status) || "Draft".equals(me.instanceData.Status)));
        me.displaySave = me.displayDelete || ((me.did || me.aid) && !me.isFlowReadonly() && !me.isFlowEnd());
        me.displaySubmit = me.displaySave;
        me.displayDiscard = me.iid && !!me.allowDiscard;    // 被驳回到开始节点时，申请人可以作废此流程

        // 基本信息表单
        me.basicForm = Ext.create("PIC.Bpm.FlowBasicFormPanel", {
            fieldsetTitle: '基本信息',
            frmdata: me.basicData,
            currentStage: me.stage || config.currentStage
        });

        // 流程表单
        me.contentForm = null;
        var contentFormPath = PIC.BpmFlowBus.getContentFormPath(me.wfConfig, me.formDefineData);
        if (contentFormPath) {
            me.contentForm = Ext.create(contentFormPath, {
                fieldsetTitle: "审批内容",
                frmdata: me.contentData,
                busPage: me,
                currentStage: me.stage || config.currentStage
            });
        }

        // 流程审批表单
        me.approveForm = Ext.create("PIC.Bpm.FlowApproveFormPanel", {
            title: false,
            busPage: me
        });

        var flowContentItems =[me.basicForm, me.contentForm];

        // 构建FlowInfoContainer
        var flowInfoItems = [], flowDetailItems = [];

        if (me.isFlowStarted()) {
            // 展示审批表单
            if (me.aid && !me.isFlowEnd() && !me.isFlowReadonly()) {
                flowContentItems.push(me.approveForm);
            }

            // 展示流程流水信息
            me.runningPanel = Ext.create("PIC.view.com.bpm.FlowRunningPanel", {
                busPage: me
            });

            flowDetailItems.push(me.runningPanel);

            // 展示流程跟踪板块
            me.trackingPanel = Ext.create("PIC.view.com.bpm.FlowTrackingPanel", {
                busPage: me
            });

            flowDetailItems.push(me.trackingPanel);
        }

        me.flowContentPanel = Ext.create("PIC.ExtPanel", {
            title: "表单",
            margins: '10 0 0 0',
            align: 'stretch',
            autoScroll: true,
            border: false,
            items: flowContentItems
        });

        // var tabItems = [me.basicForm, me.contentForm];
        var tabItems = [me.flowContentPanel];

        // 展示流程图
        me.diagramPanel = Ext.create("PIC.ExtPanel", {
            title: '流程图',
            border: false,
            autoScroll: true,
            html: '<div><img src="' + me.diagramUrl + '" style="width:680px" /></div>'
        });

        flowDetailItems.push(me.diagramPanel);

        me.flowDetailContainer = Ext.create('PIC.ExtFormTabPanel', {
            activeTab: 0,
            border: false,
            flex: 1,
            items: flowDetailItems
        });

        flowInfoItems.push(me.flowDetailContainer);

        // 流程信息内容
        me.flowInfoContainer = Ext.create('PIC.ExtPanel', {
            title: '流转',
            isTabItem: true,
            border: false,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: flowInfoItems
        });

        tabItems.push(me.flowInfoContainer);

        me.tabPanel = Ext.create("PIC.ExtFormTabPanel", {
            region: 'center',
            activeTab: 0,
            border: false,
            width: document.body.offsetWidth - 5,
            items: tabItems,
            listeners: {
                afterrender: function () {
                    // 所有tabItem都遍历激活一下，以使其自动渲染
                    Ext.each(tabItems, function (ti) {
                        me.tabPanel.setActiveTab(ti);
                    });

                    me.tabPanel.setActiveTab(me.flowContentPanel);
                }
            }
        });

        me.mainPanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            layout: 'border',
            border: false,
            tbar: me.getBarButtons({ tbar: true }),
            // bbar: me.getBarButtons({ bbar: true }),
            items: [me.tabPanel]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.on('afterrender', function () {
            if ('update'.equals(pgAction)) {
                var task = Ext.create('Ext.util.DelayedTask', function () {
                    me.showControlPointWin();
                }, me);

                task.delay(1000);
            }
        });
    },

    initData: function () {
        var me = this;

        me.flowData = PICState['FlowData'];

        if (me.flowData) {
            me.allowDiscard = me.flowData['allowDiscard'] || false; // 是否允许作为当前流程
            me.defineData = me.flowData['Define'];
            me.basicData = me.flowData['Basic'];
            me.formDefineData = me.flowData['FormDefine'];
            me.wfConfig = me.flowData['Config'];
            me.instanceData = me.flowData['Instance'];
            me.flowStateData = me.flowData['FlowState'];
            me.actionData = me.flowData['Action'];
            me.nextTasks = me.flowData['NextTasks'];

            if (me.wfConfig) {
                me.diagramPath = me.wfConfig["FlowDiagramPath"] || "";
                me.diagramUrl = PIC.BpmFlowBus.getDiagramUrl(me.diagramPath);
                me.docTemplates = me.wfConfig["DocTemplates"] || "";

                if (me.wfConfig["Tag"] != null) {
                    me.controlPoints = me.wfConfig["Tag"]["ControlPoints"] || [];
                }
            }
        }

        if ('c' === me.op) {
            if (me.basicData) {
                if (!me.basicData.CreatedTime) {
                    me.basicData.CreatedTime = PICState["SystemInfo"]["Date"];
                }

                if (!me.basicData.Status) {
                    me.basicData.Status = "New";
                }
            }

            if (me.op == "c") {
                if (me.wfConfig && me.wfConfig["FormData"]) {
                    me.contentData = me.wfConfig["FormData"];
                }
            }
        } else {
            if(!me.basicData){
                if (me.flowStateData) {
                    me.basicData = me.flowStateData["Request"]["BasicInfo"];
                }
            }

            if (me.instanceData) {
                if (!me.basicData.Code) {
                    me.basicData.Code = me.instanceData.Code;
                }
                if (!me.basicData.CreatedTime) {
                    me.basicData.CreatedTime = me.instanceData.CreatedTime;
                }
                if (!me.basicData.StartedTime) {
                    me.basicData.StartedTime = me.instanceData.StartedTime;
                }
            }

            if(!me.contentData){
                me.contentData = me.flowStateData["Request"]["FormData"];
            }
        }
    },

    doSave: function () {
        var me = this;

        var flag = me.tabPanel.validateForms([me.basicForm]);
        if (!flag) return;

        var basicData = me.basicForm.getValues();
        var formData = me.contentForm.getValues();

        var postData = { Basic: basicData, Form: formData }

        PICUtil.ajaxRequest('save', {
            onsuccess: function (respData, opts) {
                PICMsgBox.alert('保存成功！');

                PICUtil.invokePgCallback(respData);
            }
        }, postData);
    },

    doSubmit: function () {
        var me = this;
        var flag = false;

        if (!me.aid) {
            flag = me.tabPanel.validateForms([me.basicForm, me.contentForm]);
        } else {
            flag = me.tabPanel.validateForms([me.basicForm, me.contentForm, me.approveForm]);
        }

        if (!flag) return;

        var basicData = me.basicForm.getValues();
        var formData = me.contentForm.getValues();
        var approveData = me.approveForm.getValues();

        var postData = { Basic: basicData, Form: formData, Approve: approveData }
        
        PICUtil.ajaxRequest('submit', {
            timeout: 30000, // 默认30秒超时，处理Workflow的时间可能稍微有点长
            onsuccess: function (respData, opts) {
                var vp = PICUtil.getPortalViewPort();   // 获取门户

                if (!me.aid && vp) {
                    PICMsgBox.confirm("发起流程成功，是否转到我发起的流程页面查看所有您已发起的流程？", function (btn) {
                        if ('yes'.equals(btn) && !me.aid) {
                            try {
                                vp.LoadAction({ "status": "mine" });
                                window.close();
                            } catch (e) { }
                        }

                        PICUtil.invokePgCallback(respData);
                    });
                } else {
                    PICMsgBox.alert("提交成功!");
                    PICUtil.invokePgCallback(respData);
                }
            }
        }, postData);
    },

    // 作废当前流程，只有流程发起人，且流程被打回到开始节点时，才可以作废
    doDiscard: function () {
        var me = this;

        PICMsgBox.confirm("作废流程将无法恢复，确定作废当前流程？", function (btn) {
            if ('yes'.equals(btn)) {
                PICUtil.ajaxRequest('discard', {
                    onsuccess: function (respData, opts) {
                        PICMsgBox.alert('作废成功！');

                        PICUtil.invokePgCallback(respData);
                    }
                }, { iid: me.iid });
            }
        });
    },

    doExport: function (tmpl) {
        var me = this;

        var params = { iid: me.iid, tmpl: tmpl };
        if (tmpl) {
            params.IsCheckAuth = tmpl.IsCheckAuth || true;  // 默认需要检查密码
        }
        PICUtil.exportFlowDoc(params);
    },

    doDelete: function () {
        var me = this;

        PICMsgBox.confirm("删除流程将无法恢复，确定删除当前流程？", function (btn) {
            if ('yes'.equals(btn)) {
                PICUtil.ajaxRequest('delete', {
                    onsuccess: function (respData, opts) {
                        PICMsgBox.alert('删除成功！');

                        PICUtil.invokePgCallback(respData);
                    }
                }, { iid: me.iid });
            }
        });
    },

    getBarButtons: function (params) {
        var me = this;

        params = params || {};
        var is_tbar = !(params.tbar === false);
        var is_bbar = (params.bbar === true);

        var btns = [{
            text: '保存',
            hidden: !me.displaySave,
            xtype: 'picsavebutton',
            handler: function () {
                me.doSave();
            }
        }, '-', {
            xtype: 'picsubmitbutton',
            text: '提交',
            hidden: !me.displaySubmit,
            handler: function () {
                me.doSubmit();
            }
        }, '-', {
            text: '删除',
            hidden: !me.displayDelete,
            xtype: 'picdeletebutton',
            handler: function () {
                me.doDelete();
            }
        }, {
            text: '作废',
            hidden: !me.displayDiscard,
            xtype: 'picdeletebutton',
            handler: function () {
                me.doDiscard();
            }
        }];

        if (me.iid && is_tbar && !is_bbar) {
            var docExpMenu = { text: "导出", iconCls: "pic-icon-doc", hidden: true, menu: [] };

            if (me.docTemplates && me.docTemplates.length > 0) {
                docExpMenu.hidden = false;
                Ext.each(me.docTemplates, function (t) {
                    if (t.Name) {
                        docExpMenu.menu.push({
                            text: t.Name,
                            handler: function () {
                                me.doExport(t);
                            }
                        });
                    }
                });
            }

            btns.push('-', docExpMenu);
        }

        btns.push('-', { xtype: 'picclosebutton' }, '->',
            '-', {
                xtype: 'pichelpbutton',
                handler: function () {
                    me.onHelp();
                }
            });

        return btns;
    },

    // 获取当前控制点
    getControlPoint: function (stage) {
        var me = this;
        stage = stage || me.stage || me.currentStage;

        var cp = null;
        if (stage && me.controlPoints) {
            Ext.each(me.controlPoints, function (_cp) {
                if (stage.equals(_cp.TaskCode)) {
                    cp = _cp;
                    return false;
                }
            });
        }

        return cp;
    },

    getNextTaskState: function (routeCode) {
        // 获取下一步任务状态
        var me = this;

        if (me.nextTasks) {
            return me.nextTasks[routeCode] || null;
        }

        return null;
    },

    getRouteEnum: function () {
        var me = this;

        // 获取路径枚举
        var routeEnum = {};

        if (me.flowStateData && me.flowStateData.Current) {
            // 根据操作字符串获取枚举
            var actionOpString = me.flowStateData.Current.ActionOpString;

            if (actionOpString) {
                var actionOp = $.getJsonObj(actionOpString);

                if ($.isArray(actionOp)) {
                    Ext.each(actionOp, function (op) {
                        if (op.Code) {
                            if (op.Name) {
                                routeEnum[op.Code] = op.Name;
                            } else {
                                routeEnum[op.Code] = PIC.WfDefineModel.DefaultOpEnum[op.Code] || op.Code;
                            }
                        }
                    });
                }
            }
        }

        if ($.isEmptyObject(routeEnum)) {
            // 根据任务获取枚举
            if (me.nextTasks) {
                for (var key in me.nextTasks) {
                    routeEnum[key] = PIC.WfDefineModel.DefaultOpEnum[key] || key;
                }
            }
        }

        if ($.isEmptyObject(routeEnum)) {
            routeEnum['Submit'] = PIC.WfDefineModel.DefaultOpEnum['Submit'];    // 默认提交按钮
        }

        return routeEnum;
    },

    showControlPointWin: function (cp) {
        var me = this;
        cp = cp || me.getControlPoint();

        if (cp && cp.Desc) {
            PICMsgBox.warn(cp.Desc, "当前流程控制点:");
        }
    },

    isFlowReadonly: function () {
        return pgAction == 'read' || pgAction == 'view';
    },

    isFlowNew: function (status) {
        var me = this;
        status = status || me.instanceStatus;

        if ("Draft".equals(status)
            || "New".equals(status)) {
            return true;
        }

        return false;
    },

    isFlowStarted: function (status) {
        var me = this;
        status = status || me.instanceStatus;

        if (!status
            || "Draft".equals(status)
            || "New".equals(status)) {
            return false;
        }

        return true;
    },

    isFlowEnd: function (status) {    // 判断流程是否已经结束
        var me = this;
        status = status || me.instanceStatus;

        if ("Completed".equals(status)
            || "Closed".equals(status)) {
            return true;
        }

        return false;
    },

    isTaskEnd: function (status) {
        status = status || null;

        if ("Completed".equals(status)
            || "Closed".equals(status)) {
            return true;
        }

        return false;
    },

    onHelp: function () {
        var me = this;

        me.showControlPointWin();
    },

    statics: {
        getContentFormPath: function (cfg, formDefine) {
            if (cfg) {
                var formPath = (cfg.FormPath || "").trim();

                if (formPath === ".") {
                    if (formDefine) {
                        PICUtil.execScript(formDefine);
                    }

                    formPath = PIC_LOCAL_BPM_FORM_NAME;
                } else if (formPath.indexOf("PIC.") != 0) {
                    formPath = "PIC." + formPath;
                }

                return formPath;
            }

            return null;
        },

        getDiagramUrl: function (path) {
            var url = $.combineQueryUrl(PICConfig.FilePagePath, {
                type: 'local',
                code: 'flow',
                subpath: path
            });

            return url;
        }
    }
});

