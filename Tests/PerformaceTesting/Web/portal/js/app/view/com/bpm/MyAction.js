Ext.define('PIC.view.com.bpm.MyAction', {
    extend: 'PIC.TabFramePage',
    alternateClassName: 'PIC.BpmMyActionPage',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            activeTab: 1
        }, config);

        var status = $.getQueryString('status', 'new');

        me.tabItems = [
            { title: "新建", code:'draft', href: "MyActionList.aspx?status=draft" },
            { title: "待办", code: 'new', href: "MyActionList.aspx?status=new" },
            { title: "我发起的", code: 'mine', href: "MyActionList.aspx?status=mine" },
            { title: "已处理", code: 'closed', href: "MyActionList.aspx?status=closed" },
            { title: "我代理的", code: 'agent', href: "MyActionList.aspx?status=agent" }
        ];

        for (var i = 0; i < me.tabItems.length; i++) {
            if (me.tabItems[i].code.equals(status)) {
                config.activeTab = i;
                break;
            }
        }

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});