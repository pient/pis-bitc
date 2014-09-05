
Ext.define('PIC.model.sys.reg.Portlet', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.PortletModel',

    idProperty: 'PortletID',

    fields: [
		{ name: 'PortletID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'DataModule' },
		{ name: 'Description' },
		{ name: 'Config' },
		{ name: 'Status' },
		{ name: 'Config' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' }
    ],

    statics: {
        TypeEnum: PICState["TypeEnum"] || {},
        StatusEnum: PICState["StatusEnum"] || {},
        ModuleEnum: PICState["ModuleEnum"] || {}
    }
});