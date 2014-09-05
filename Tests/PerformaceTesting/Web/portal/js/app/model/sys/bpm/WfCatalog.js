
Ext.define('PIC.model.sys.bpm.WfCatalog', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.WfCatalogModel',

    idProperty: 'Index',

    fields: [
		{ name: 'Index' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'FlowList' }
    ],

    statics: {
        CatalogEnum: PICState["WfCatalogEnum"] || {}
    }
});