Ext.define('PIC.view.setup.dev.ts.TSMgmt', {
    extend: 'PIC.TabFramePage',
    alternateClassName: 'PIC.TSMgmtPage',

    pageData: {},

    mainPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.tabItems = [
            { title: "调度列表", href: "SchedulerList.aspx" },
            { title: "任务列表", href: "TaskList.aspx" }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    }
});