Ext.define('PIC.view.com.bpm.FlowRunningPanel', {
    extend: 'PIC.PageGridPanel',

    requires: [
        'PIC.model.sys.bpm.WfInstance',
        'PIC.model.sys.bpm.WfAction'
    ],

    busPage: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation,
            title: '流转信息',
            border: false,
            pgbar: false,
            schitems: [],
            formparams: { url: "WfActionEdit.aspx", style: { width: 760, height: 450 } }
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
            model: 'PIC.model.sys.bpm.WfAction',
            dsname: 'ActionList',
            idProperty: 'ActionID',
            picbeforeload: function (proxy, params, node, operation) {
                PICUtil.setReqParams(params, 'qryacts', { iid: me.busPage.iid });
            }
        });

        config.columns = [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Title', header: '标题', juncqry: true, width: 200, sortable: false },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.WfActionModel.StatusEnum, width: 60, sortable: false, align: 'center' },
				{ dataIndex: 'OwnerName', header: '任务负责人', width: 80, renderer: me.ownerNameRenderer, sortable: false, align: 'center' },
				{ dataIndex: 'CloserName', header: '任务处理人', width: 80, renderer: me.closerNameRenderer, sortable: false, align: 'center' },
				{ dataIndex: 'Opinion', header: '意见', renderer: me.opinionRenderer, flex: 1, minWidth: 200, sortable: false, align: 'center' },
				{ dataIndex: 'CreatedTime', header: '发起时间', width: 135, sortable: false, align: 'center' },
				{ dataIndex: 'ClosedTime', header: '结束时间', width: 135, sortable: false, align: 'center' }
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

    opinionRenderer: function (val, p, rec) {
        var rtn = "";

        var tag = rec.get("Tag");
        if (tag) {
            var tagObj = $.getJsonObj(tag);

            if (tagObj && tagObj["Request"]) {
                var req = tagObj["Request"];

                if (req && req["ActionInfo"]) {
                    rtn = req["ActionInfo"]["Comments"] || "";
                }
            }
        }

        return rtn;
    },

    ownerNameRenderer: function (val, p, rec) {
        var cfg = null,
            rtn = "";

        rtn = PICUtil.renderFuncLink({ text: rec.get("OwnerName"), params: { clickfunc: 'PICUtil.loadUserTip(this, "' + rec.get("OwnerID") + '")' } });

        return rtn;
    },

    closerNameRenderer: function (val, p, rec) {
        var cfg = null,
            rtn = "";

        rtn = PICUtil.renderFuncLink({ text: rec.get("CloserName"), params: { clickfunc: 'PICUtil.loadUserTip(this, "' + rec.get("CloserID") + '")' } });

        return rtn;
    },

    reloadData: function () {
        this.store.reload();
    }
});