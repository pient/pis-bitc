
Ext.define('PIC.view.portal.default.SubContentPanel', {
    extend: 'PIC.view.portal.default.ContentPanel',
    alias: 'widget.picportalcontentpanel',
    alternateClassName: 'PIC.SubPortalContentPanel',

    frameContent: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            title: null
        }, config);

        me.callParent([config]);
    }
});