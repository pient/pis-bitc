
Ext.define('PIC.model.sys.bpm.WfInstance', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.WfInstanceModel',

    idProperty: 'InstanceID',

    fields: [
		{ name: 'InstanceID' },
		{ name: 'DefineID' },
		{ name: 'ApplicationID' },
		{ name: 'ModuleID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'Catalog' },
		{ name: 'Grade' },
		{ name: 'Important' },
		{ name: 'Status' },
		{ name: 'Memo' },
		{ name: 'Config' },
		{ name: 'Data' },
		{ name: 'Tag' },
		{ name: 'StartedTime' },
		{ name: 'EndedTime' },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'CreatedTime' },
		{ name: 'LastModifiedDate' }
    ],

    statics: {
        StatusEnum: PICState["WfInsStatusEnum"] || {}
    }
});