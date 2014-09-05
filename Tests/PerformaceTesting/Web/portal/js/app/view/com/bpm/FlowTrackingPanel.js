Ext.define('PIC.view.com.bpm.FlowTrackingPanel', {
    extend: 'PIC.PageGridPanel',

    requires: [
        'PIC.model.sys.bpm.WfInstance',
        'PIC.model.sys.bpm.WfTask'
    ],

    busPage: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation,
            title: '节点信息',
            border: false,
            pgbar: false,
            schitems: [],
            formparams: { url: "WfTaskEdit.aspx", style: { width: 760, height: 450 } }
        }, config);

        me.busPage = config.busPage;

        config.tlitems = ['-', {
            text: '刷新',
            iconCls: 'pic-icon-refresh',
            handler: function () {
                me.reloadData();
            }
        }, '->'];

        config.store = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfTask',
            dsname: 'TaskList',
            idProperty: 'TaskID',
            picbeforeload: function (proxy, params, node, operation) {
                PICUtil.setReqParams(params, 'qrytask', { iid: me.busPage.iid });
            }
        });

        config.columns = [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Code', header: '编号', juncqry: true, hidden: true, width: 100, sortable: false },
				{ dataIndex: 'Title', header: '标题', juncqry: true, renderer: me.titleRenderer, flex: 1, sortable: false },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.WfTaskModel.StatusEnum, width: 60, sortable: false, align: 'center' },
				{ dataIndex: 'StartedTime', header: '发起时间', width: 135, sortable: false, align: 'center' },
				{ dataIndex: 'EndedTime', header: '结束时间', width: 135, sortable: false, align: 'center' }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.on('afterrender', function () {
            me.reloadData();
        });
    },

    titleRenderer: function (val, p, rec) {
        var cfg = null,
            rtn = val;

        if (val) {
            if (!this.busPage.isTaskEnd(rec.get('Status'))) {
                val = '<b>' + val + '</b>';
            }
            rtn = PICUtil.renderFuncLink({ text: val, params: { clickfunc: 'window.PICPage.trackingPanel.onTitleClick(this, "' + rec.getId() + '")' } });
        }

        return rtn;
    },

    onTitleClick: function (el, recid) {
        var me = this;

        if (!recid) {
            return;
        }

        if (!me.titleTip) {
            me.titleTip = Ext.create('PIC.ExtQuickTip', {
                renderTo: Ext.getBody(),
                autoHide: false,
                width: 300,
                title: '<b style="font-size:14px;">操作信息：</b>'
            });
        }

        me.titleTip.showBy(el);
        me.titleTip.setLoading("加载中...");

        PICUtil.ajaxRequest('qryacts', {
            nomask: true,
            onsuccess: function (respData, opts) {
                me.titleTip.setLoading(false);

                var actList = respData.ActionList;

                if (actList && actList.length > 0) {
                    var htmlstr = me.getTitleTipHtml(recid, actList);
                    me.titleTip.update(htmlstr);
                } else {
                    me.titleTip.update("暂无审批信息...");
                }
            }
        }, { tid: recid });
    },

    getTitleTipHtml: function (recid, actData) {
        var me = this;
        var rec = me.store.getById(recid);

        Ext.each(actData, function (act) {
            act.TagObj = $.getJsonObj(act.Tag) || {};
        });

        // new Ext.XTemplate('<tpl for=".">审批人：{CreatorID}<br />审批意见：{TagObj.Comments}</tpl>').apply(actData)

        var rtn = new Ext.XTemplate(
            '<hr/><span style="border:0px">',
            '<tpl for=".">',
            '<b>审批人：</b>{OwnerName}<br />',
            '<b>审批意见：</b>{TagObj.Request.ActionInfo.Comments}<br />',
            '<b>提交选项：</b>{TagObj.Request.ActionInfo.RouteName}<br />',
            '<b>目标节点：</b>{TagObj.Request.ActionInfo.TargetName}<br />',
            '</tpl>',
            '<b>发起人：</b>' + rec.get("CreatorName"),
            '</span>').apply(actData);

        return rtn;
    },

    loadGridTip: function () {
        var me = this;

        var view = me.getView();

        me.gridTip = Ext.create('Ext.tip.ToolTip', {
            target: view.el,
            delegate: view.itemSelector,
            autoHide: false,
            trackMouse: true,
            renderTo: Ext.getBody(),
            listeners: {
                beforeshow: function (tip) {
                    var rec = view.getRecord(tip.triggerElement);
                    var tagObj = $.getJsonObj(rec.get('Tag'));

                    if (tagObj && tagObj["TaskState"] && tagObj["TaskState"]["Request"]) {
                        var userActorList = tagObj["TaskState"]["Request"]["UserActorList"];

                        if (userActorList && userActorList.length > 0) {
                            var usrListStr = $.map(userActorList, function (u) {
                                return (u["Name"] || "");
                            }).join(',');

                            if (usrListStr) {
                                usrListStr = "审批人：" + usrListStr;
                                me.gridTip.update(usrListStr);
                            }
                        } else {
                            me.gridTip.update("暂无...");
                        }
                    }
                }
            }
        });
    },

    reloadData: function () {
        this.store.reload();
    }
});