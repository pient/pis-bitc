Ext.define('PIC.view.com.msg.PubMsgListView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.PubMsgListViewPage',

    requires: [
        'PIC.model.sys.msg.Message'
    ],

    mainPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.catalog = $.getQueryString('catalog');

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.msg.Message',
            dsname: 'EntList',
            idProperty: 'MessageID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.catalog = me.catalog;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            exportTitle: '消息列表',
            tlitems: false,
            schitems: false,
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Subject', header: '标题', juncqry: true, flex: 1, mixWidth: 200, formlink: { clickfunc: 'window.PICPage.onFormlinkClick' }, sortable: true },
				{ dataIndex: 'FromName', header: '作者', width: 90, sortable: true, align: 'center' },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.MessageModel.PubInfoStatusEnum, width: 90, sortable: true, align: 'center' },
				{ dataIndex: 'SentDate', header: '发布时间', width: 140, sortable: true, align: 'center' }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    onFormlinkClick: function (recid) {
        var me = this;
        var rec = me.dataStore.getById(recid);

        if (rec) {
            var diagUrl = "PubMsgView.aspx?catalog=" + me.catalog;

            if ("Links".equals(me.catalog)) {
                diagUrl = rec.get('Content');
            }

            PICUtil.openDialog({ url: diagUrl, style: { width: 980, height: 550 } });
        }
    },

    // 链接渲染
    colRender: function (val, p, rec) {
        var me = this;
        var rtn = val;

        return rtn;
    }
});