Ext.define('PIC.view.com.bpm.MyActionList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.BpmMyActionListPage',

    requires: [
        'PIC.model.sys.bpm.WfInstance',
        'PIC.model.sys.bpm.WfTask',
        'PIC.model.sys.bpm.WfAction'
    ],

    mainPanel: null,
    tip: null,

    mode: "",
    refId: "",

    constructor: function (config) {
        var me = this;
        me.refId = $.getQueryString({ ID: "tid" });
        me.did = $.getQueryString({ ID: "did" });
        me.status = $.getQueryString({ ID: "status" });

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfInstance',
            dsname: 'EntList',
            idProperty: 'InstanceID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.status = me.status;
            }
        });

        me.displayCRUD = ('draft' === me.status);
        me.displayExec = ('new' === me.status);
        me.displayOperator = !('new' === me.status) && !('draft' === me.status) || true;
        me.displayStatus = !('new' === me.status) && !('draft' === me.status);
        me.displayEndedTime = !('new' === me.status) && !('draft' === me.status);

        var actionItems = [{
            iconCls: 'pic-icon-exec',
            tooltip: '执行',
            handler: function (grid, rowIndex, colIndex) {
                var rec = grid.getStore().getAt(rowIndex);

                me.doExec(rec);
            }
        }];

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            exportTitle: '流程实例',
            border: false,
            formparams: { url: "FlowBus.aspx", style: { width: 700, height: 550} },
            tlitems: ['-', {
                bttype: 'add',
                text: '新建流程',
                hidden: (!me.displayCRUD),
                handler: function () {
                    PICUtil.openSelDialog({
                        url: "FlowSelect.aspx",
                        sender: me,
                        style: { width: 450, height: 500 },
                        params: { callback: "createFlowInstance", mode: "func" }
                    });
                }
            }, {
                bttype: 'edit',
                hidden: !me.displayCRUD,
                handler: function () {
                    me.doEdit();
                }
            }, '-', 'excel', '->', '-', 'help'],
            schitems: [],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'ApplicantID', hidden: true },
				{ dataIndex: 'Id', header: '操作', xtype: 'actioncolumn', items: actionItems, hidden: !me.displayExec, width: 60, menuDisabled: true, align: 'center' },
				{ dataIndex: 'Name', header: '标题', juncqry: true, formlink: { clickfunc: 'PICPage.onFormlinkClick' }, flex: 1, minWidth: 150, sortable: true },
				{ dataIndex: 'Id', name: 'ApplicantName', header: '申请人', juncqry: true, width: 80, renderer: me.applicantNameRenderer, sortable: true, align: 'center' },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.WfActionModel.StatusEnum, hidden: !me.displayStatus, width: 60, sortable: true, align: 'center' },
				{ dataIndex: 'Id', name: 'TaskName', header: '当前环节', juncqry: true, width: 120, renderer: me.taskNameRenderer, sortable: true, align: 'center' },
				{ dataIndex: 'Id', name: 'OperatorName', header: '当前处理人', juncqry: true, hidden: !me.displayOperator, width: 120, renderer: me.operatorNameRenderer, sortable: true, align: 'center' },
				{ dataIndex: 'CreatedTime', header: '创建时间', width: 150, sortable: true, align: 'center' },
				{ dataIndex: 'StartedTime', header: '开始时间', width: 150, sortable: true, align: 'center' },
				{ dataIndex: 'EndedTime', header: '结束时间', hidden: !me.displayEndedTime, width: 150, sortable: true, align: 'center' }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    applicantNameRenderer: function (val, p, rec) {
        var cfg = null,
            rtn = "",
            tag = rec.get('Tag');

        cfg = $.getJsonObj(tag) || "";

        if (cfg && cfg.WfFlowInfo && cfg.WfFlowInfo.FlowState && cfg.WfFlowInfo.FlowState.Request && cfg.WfFlowInfo.FlowState.Request.BasicInfo) {
            rtn = cfg.WfFlowInfo.FlowState.Request.BasicInfo["ApplicantName"] || "";
        }

        return rtn;
    },

    taskNameRenderer: function (val, p, rec) {
        var cfg = null,
            rtn = "",
            tag = rec.get('Tag');

        cfg = $.getJsonObj(tag) || "";

        if (cfg && cfg.WfFlowInfo && cfg.WfFlowInfo.FlowState && cfg.WfFlowInfo.FlowState.Current) {
            rtn = cfg.WfFlowInfo.FlowState.Current["ActionTitle"] || cfg.WfFlowInfo.FlowState.Current["Name"] || "";
        }

        return rtn;
    },

    operatorNameRenderer: function (val, p, rec) {
        var cfg = null,
            rtn = "",
            tag = rec.get('Tag');

        cfg = $.getJsonObj(tag) || "";

        if (cfg && cfg.WfFlowInfo && cfg.WfFlowInfo.FlowState && cfg.WfFlowInfo.FlowState.Current && cfg.WfFlowInfo.FlowState.Current.Request) {
            var usrs = cfg.WfFlowInfo.FlowState.Current.Request["UserActorList"];
            if ($.isArray(usrs)) {
                rtn = $.map(usrs, function (u, i) {
                    if (u["Name"]) {
                        return PICUtil.renderFuncLink({ text: u["Name"], params: { clickfunc: 'window.PICPage.onOperatorNameClick(this, "' + u["UserID"] + '")' } });
                    }

                    return '';
                }).join(',');
            } else {
                rtn = "";
            }
        }

        return rtn;
    },

    createFlowInstance: function (rtn) { // 选择返回执行
        if (rtn && rtn["data"] && rtn["data"].length > 0) {
            var f = rtn["data"][0];

            if (f && f["DefineID"]) {
                PICUtil.openFlowBusDialog({ op: 'c', did: f["DefineID"], callback: 'onFlowSubmit' });
            }
        }
    },

    onFlowSubmit: function () {
        this.dataStore.load();
    },

    onFormlinkClick: function (recid) {
        var me = this;
        var rec = me.dataStore.getById(recid);

        if (me.displayExec) {
            me.doExec(rec);
        } else if (me.displayCRUD) {
            me.mainPanel.openFormWin('u', recid);
        } else {
            me.mainPanel.openFormWin('r', recid);
        }
    },

    doExec: function (rec) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要执行的流程实例");

            return;
        }

        var aid = rec.raw["ActionID"];

        if (aid) {
            PICUtil.openFlowBusDialog({ op: 'u', aid: aid, callback: 'onFlowSubmit' });
        }
    },

    doEdit: function (rec) {
        var me = this;
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要编辑的流程实例");
        }

        PICUtil.openFlowBusDialog({ op: 'u', iid: rec.getId(), callback: 'onFlowSubmit' });
    },

    loadTrackingInfo: function (rec) {
        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要查看的流程实例");
        }

        PICUtil.openFlowBusDialog({ op: 'start', did: rec.getId() });
    },

    reloadData: function (params) {
        var me = this;

        if (params) {
            me.refId = params.did;
        }

        me.dataStore.load();
    },

    onOperatorNameClick: function (el, recid) {
        var me = this;

        if (!recid) {
            return;
        }

        if (!me.operatorTip) {
            me.operatorTip = Ext.create('PIC.ExtQuickTip', {
                renderTo: Ext.getBody(),
                width: 200,
                title: '用户信息：'
            });
        }

        me.operatorTip.showBy(el);
        me.operatorTip.setLoading("加载中...");

        PICUtil.getTmplData('renderstr', {
            onsuccess: function (respData, opts, resp) {
                me.operatorTip.setLoading(false);

                if (respData && respData.User && respData.User.length > 0) {
                    var htmlstr = me.getOperatorTipHtml(respData.User[0]);
                    me.operatorTip.update(htmlstr);
                } else {
                    me.operatorTip.update("暂无...");
                }
            }
        }, { tcode: 'Sys.Data.UserInfo', ctxparams: { id: recid } });
    },

    getOperatorTipHtml: function (usrData) {
        var rtn = new Ext.XTemplate(
                "<span style='margin:2px; border:0px'>",
                "姓名：{Name}<br/>",
                "工号：{WorkNo}<br/>",
                "部门：{DeptName}<br/>",
                "邮件：{Email}<br/>",
                "</span>").apply(usrData);

        return rtn;
    }
});