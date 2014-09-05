Ext.define('PIC.view.com.msg.MyPubMsg', {
    extend: 'PIC.TabFramePage',
    alternateClassName: 'PIC.MyPubMsgPage',

    requires: [
        'PIC.model.sys.msg.Message',
    ],

    mainPanel: null,
    catalog: null,

    constructor: function (config) {
        var me = this;

        me.catalog = $.getQueryString('catalog');

        config = Ext.apply({
            layout: 'border',
            activeTab: 1,
            initpage: 'PubMsgList.aspx'
        }, config);

        me.tabItems = [
            { title: "新建草稿", href: PICConfig.PubMsgListPagePath + "?status=New&catalog=" + me.catalog },
            { title: "已发布", href: PICConfig.PubMsgListPagePath + "?status=Published&catalog=" + me.catalog },
            { title: "已回收", href: PICConfig.PubMsgListPagePath + "?status=UnPublished&catalog=" + me.catalog },
            { title: "过期", href: PICConfig.PubMsgListPagePath + "?status=Expired&catalog=" + me.catalog }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});