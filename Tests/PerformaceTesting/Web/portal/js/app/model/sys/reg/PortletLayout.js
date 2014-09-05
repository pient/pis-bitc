
Ext.define('PIC.model.sys.reg.PortletLayout', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.PortletLayoutModel',

    idProperty: 'LayoutID',

    fields: [
		{ name: 'LayoutID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'Config' },
		{ name: 'Status' },
		{ name: 'Description' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' }
    ],

    statics: {
        TypeEnum: PICState["TypeEnum"] || {},
        StatusEnum: PICState["StatusEnum"] || {}
    }
});