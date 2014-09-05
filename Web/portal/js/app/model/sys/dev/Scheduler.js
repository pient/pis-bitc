
Ext.define('PIC.model.sys.dev.Scheduler', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.SchedulerModel',

    idProperty: 'SchedulerID',

    fields: [
		{ name: 'SchedulerID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'Catalog' },
		{ name: 'Status' },
		{ name: 'Config' },
		{ name: 'Memo' },
		{ name: 'Tag' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'CreatedDate' },
		{ name: 'LastModifiedDate' }
    ],

    statics: {
        StatusEnum: PICState["StatusEnum"] || {},
        TypeEnum: PICState["TypeEnum"] || {},
        CatalogEnum: PICState["CatalogEnum"] || {}
    }
});