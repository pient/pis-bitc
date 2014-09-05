
Ext.define('PIC.model.sys.dev.TaskInstance', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.TaskInstanceModel',

    idProperty: 'InstanceID',

    fields: [
		{ name: 'InstanceID' },
		{ name: 'TaskID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Status' },
		{ name: 'Result' },
		{ name: 'StartedTime' },
		{ name: 'EndedTime' },
		{ name: 'Tag' },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'CreatedTime' },
		{ name: 'LastModifiedDate' }
    ],

    statics: {
        StatusEnum: PICState["StatusEnum"] || {}
    }
});