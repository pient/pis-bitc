Ext.define('PIC.view.com.msg.PubMsgList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.PubMsgListPage',

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

        me.mode = $.getQueryString('mode', 'read');
        me.type = $.getQueryString('type');
        me.catalog = $.getQueryString('catalog');

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.msg.Message',
            dsname: 'EntList',
            idProperty: 'MessageID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.mode = me.mode;
                params.data.catalog = me.catalog;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            exportTitle: '消息列表',
            formparams: { url: "PubMsgEdit.aspx?catalog=" + me.catalog, style: { width: 750, height: 600 } },
            tlitems: [{
                bttype: 'add',
                text: '新建'
            }, {
                bttype: 'edit'
            }, 'delete', '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '标题', id: 'Subject', schopts: { qryopts: "{ mode: 'Like', field: 'Subject' }" } },
                { fieldLabel: '作者', id: 'FromName', schopts: { qryopts: "{ mode: 'Like', field: 'FromName' }" } },
                { fieldLabel: '状态', id: 'Status', xtype: 'picenumselect', enumdata: PIC.MessageModel.PubInfoStatusEnum, schopts: { qryopts: "{ mode: 'Eq', field: 'Status' }" } }
            ],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Subject', header: '标题', juncqry: true, flex: 1, formlink: true, sortable: true },
				{ dataIndex: 'FromName', header: '作者', width: 100, sortable: true, align: 'center' },
				{ dataIndex: 'Status', header: '状态', enumdata: PIC.MessageModel.PubInfoStatusEnum, width: 80, sortable: true, align: 'center' },
				{ dataIndex: 'CreatedDate', header: '创建时间', width: 150, sortable: true, align: 'center' },
				{ dataIndex: 'SentDate', header: '发布时间', width: 150, sortable: true, align: 'center' },
				{ dataIndex: 'ExpiredDate', header: '过期时间', width: 150, sortable: true, align: 'center' }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    // 链接渲染
    colRender: function (val, p, rec) {
        var me = this;
        var rtn = val;

        return rtn;
    }
});