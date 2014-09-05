Ext.define('PIC.view.setup.msg.MsgMgmt', {
    extend: 'PIC.TabFramePage',
    alternateClassName: 'PIC.MsgMgmtPage',

    requires: [
        'PIC.model.sys.msg.Message',
    ],

    pageData: {},

    mainPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.tabItems = [
            { title: "消息列表", href: PICConfig.MsgListPagePath + "?mode=mgmt" },
            { title: "公共消息", href: PICConfig.PubMsgListPagePath + "?mode=mgmt" }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});