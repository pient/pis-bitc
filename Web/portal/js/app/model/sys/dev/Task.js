
Ext.define('PIC.model.sys.dev.Task', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.TaskModel',

    idProperty: 'TaskID',

    fields: [
		{ name: 'TaskID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'Catalog' },
		{ name: 'Status' },
		{ name: 'Config' },
		{ name: 'Memo' },
		{ name: 'Tag' },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'CreatedTime' },
		{ name: 'LastModifiedDate' }
    ],

    statics: {
        StatusEnum: PICState["StatusEnum"] || {},
        TypeEnum: PICState["TypeEnum"] || {},
        CatalogEnum: PICState["CatalogEnum"] || {}
    }
});