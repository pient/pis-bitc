/**
 * @class PIC.view.portal.home.PortalColumn
 * @extends Ext.container.Container
 * A layout column class used internally be {@link PIC.view.portal.home.PortalPanel}.
 */
Ext.define('PIC.view.portal.home.PortalColumn', {
    extend: 'Ext.container.Container',
    alias: 'widget.portalcolumn',

    requires: [
        'Ext.layout.container.Anchor',
        'PIC.view.portal.home.Portlet'
    ],

    layout: 'anchor',
    defaultType: 'portlet',
    cls: 'x-portal-column'

    // This is a class so that it could be easily extended
    // if necessary to provide additional behavior.
});