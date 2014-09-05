
Ext.define('PIC.model.sys.bpm.WfTask', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.WfTaskModel',

    idProperty: 'TaskID',

    fields: [
		{ name: 'TaskID' },
		{ name: 'DefineID' },
		{ name: 'Code' },
		{ name: 'Type' },
		{ name: 'Catalog' },
		{ name: 'Important' },
		{ name: 'Title' },
		{ name: 'Status' },
		{ name: 'Description' },
		{ name: 'Config' },
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
        StatusEnum: { 'Started': "开始", 'Completed': "完成" }
    }
});