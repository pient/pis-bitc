Ext.define('PIC.view.com.bpm.MyFlowPortalPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.myflow-portalpanel',

    requires: [
        'PIC.view.portal.home.PortalColumn'
    ],

    cls: 'x-portal',
    bodyCls: 'x-portal-body',
    defaultType: 'picpanel',
    autoScroll: true,

    manageHeight: false,

    initComponent: function () {
        this.layout = {
            type: 'column'
        };

        this.callParent();
    }
});