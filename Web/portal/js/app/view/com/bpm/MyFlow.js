Ext.define('PIC.view.com.bpm.MyFlow', {
    extend: 'PIC.TabFramePage',
    alternateClassName: 'PIC.BpmMyFlowPage',

    constructor: function (config) {
        var me = this;

        me.did = $.getQueryString('did');

        config = Ext.apply({
            layout: 'border',
            activeTab: 0
        }, config);

        config.titbar = [{
            text: '新建流程',
            xtype: 'picaddbutton',
            handler: function () {
                PICUtil.openFlowBusDialog({ op: 'c', did: me.did });
            }
        }, '-', { xtype: 'picclosebutton' }];

        me.tabItems = [
            { title: "待办", href: "MyActionList.aspx?status=new&did=" + me.did },
            { title: "我代理的", href: "MyActionList.aspx?status=agent&did=" + me.did }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});