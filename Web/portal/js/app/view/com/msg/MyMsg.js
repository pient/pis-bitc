Ext.define('PIC.view.com.msg.MyMsg', {
    extend: 'PIC.TabFramePage',
    alternateClassName: 'PIC.MyMsgPage',

    requires: [
        'PIC.model.sys.msg.Message',
    ],

    mainPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            activeTab: 1,
            initpage: 'MsgList.aspx'
        }, config);

        me.tabItems = [
            { title: "新建草稿", href: PICConfig.MsgListPagePath + "?type=Draft" },
            { title: "收件箱", href: PICConfig.MsgListPagePath + "?type=Received" },
            { title: "已发送", href: PICConfig.MsgListPagePath + "?type=Sent" }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});