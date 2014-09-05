
Ext.define('PIC.model.sys.org.GroupRoleNode', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.GroupRoleNode',
    idProperty: 'RoleID',

    fields: [
		{ name: 'GroupID' },
		{ name: 'FunctionID' },
		{ name: 'RoleID' },
		{ name: 'Code' },
		{ name: 'Type' },
		{ name: 'Name' },
		{ name: 'SortIndex' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'Description' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreateDate' },
		{ name: 'GroupCode' },
		{ name: 'GroupName' }
    ]
});