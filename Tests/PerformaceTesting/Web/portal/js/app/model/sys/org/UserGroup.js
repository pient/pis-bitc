
Ext.define('PIC.model.sys.org.UserGroup', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.UserGroupModel',

    fields: [
		{ name: 'UserID' },
		{ name: 'GroupID' },
		{ name: 'RoleID' },
		{ name: 'Status', type: 'string' },
		{ name: 'EditStatus' },
		{ name: 'Description' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' },
		{ name: 'WorkNo' },
		{ name: 'UserName' },
		{ name: 'GroupName' },
		{ name: 'GroupCode' },
		{ name: 'GroupType' },
		{ name: 'RoleName' },
		{ name: 'RoleCode' },
		{ name: 'RoleType' }
    ],

    statics: {
    }
});