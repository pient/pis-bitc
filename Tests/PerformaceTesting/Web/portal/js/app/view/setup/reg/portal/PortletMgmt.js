Ext.define('PIC.view.setup.reg.portal.PortletMgmt', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.RegPortletMgmtPage',

    navPanel: null,
    mainPanel: null,
    contentFrame: null,

    constructor: function (config) {
        var me = this;

        var tabArr = [
            { title: "门户块", href: "PortletList.aspx" },
            { title: "门户块布局", href: "PortletLayoutList.aspx" }
        ];

        config = Ext.apply({
            layout: 'border'
        }, config);

        Ext.each(tabArr, function (i) {
            this.border = false;
            this.listeners = { activate: function (tab) { me.handleActivate(tab) } };
            this.html = "<div style='display:none;'></div>";
        });

        me.tabPanel = Ext.create("PIC.ExtTabPanel", {
            region: 'center',
            activeTab: 0,
            border: false,
            width: document.body.offsetWidth - 5,
            items: tabArr
        });

        me.navPanel = Ext.create("PIC.ExtPanel", {
            region: 'north',
            layout: 'border',
            border: false,
            height: 25,
            items: me.tabPanel
        });

        me.mainPanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            margins: '0 0 0 0',
            border: false,
            cls: 'empty',
            bodyStyle: 'background:#f1f1f1',
            html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src="PortletList.aspx"></iframe>',
            listeners: {
                afterrender: function () {
                    me.contentFrame = document.getElementById("frameContent");
                }
            }
        });

        me.items = [me.navPanel, me.mainPanel];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    handleActivate: function (tab) {
        var me = this;

        me.contentFrame.src = tab.href;
    }
});