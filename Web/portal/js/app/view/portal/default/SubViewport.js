
Ext.define('PIC.view.portal.default.SubViewport', {
    extend: 'PIC.ExtViewport',
    alias: 'widget.picsubportalviewport',
    alternateClassName: 'PIC.SubPortalViewport',

    requires: [
        'PIC.model.portal.ModuleItem'
    ],

    currentModule: null,
    modules: null,

    menuPanel: null,
    contentPanel: null,
    mainPanel: null,

    constructor: function (config) {
        var me = this;

        currentModule = PICState["Module"] || {};
        modules = PICState["Modules"] || [];

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.menuPanel = Ext.create('PIC.view.portal.default.SubMenuPanel', {
            region: 'west',
            split: true,
            currentModule: currentModule,
            modules: modules,
            listeners: {
                afterrender: function () {
                    Ext.defer(function () {
                        // PIC.PortalViewport.LoadHome();
                    }, 100);
                },
                cellclick: function (cmp, td, cellIdx, rec, tr, rowIdx, e, eOpts) {
                    var url = rec.get('Url');
                    var title = rec.get('Name');
                    if (url) { me.contentPanel.loadPage(url, title); }
                }
            }
        });

        me.contentPanel = Ext.create('PIC.view.portal.default.SubContentPanel', {
            region: 'center'
        });

        me.mainPanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            layout: 'border',
            border: false,
            cls: 'empty',
            bodyStyle: 'background:#dce2e7;',
            items: [me.menuPanel, me.contentPanel]
        });

        config.items = me.mainPanel;

        this.callParent([config]);
    },

    statics: {
    }
});
