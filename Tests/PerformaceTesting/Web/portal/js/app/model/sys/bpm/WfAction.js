
Ext.define('PIC.model.sys.bpm.WfAction', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.WfActionModel',

    idProperty: 'ActionID',

    fields: [
		{ name: 'ActionID' },
		{ name: 'TaskID' },
		{ name: 'InstanceID' },
		{ name: 'Title' },
		{ name: 'Type' },
		{ name: 'Catalog' },
		{ name: 'Grade' },
		{ name: 'Rate' },
		{ name: 'Important' },
		{ name: 'Status' },
		{ name: 'Deadline' },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'AgentID' },
		{ name: 'AgentName' },
		{ name: 'OpenedTime' },
		{ name: 'OpenorID' },
		{ name: 'OpenorName' },
		{ name: 'StartedTime' },
		{ name: 'ExecutorID' },
		{ name: 'ExecutorName' },
		{ name: 'ClosedTime' },
		{ name: 'CloserID' },
		{ name: 'CloserName' },
		{ name: 'Description' },
		{ name: 'Tag' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'CreatedTime' },
		{ name: 'LastModifiedDate' }
    ],

    statics: {
        StatusEnum: PICState["WfActStatusEnum"] || {}
    }
});