
Ext.define('PIC.model.sys.org.RoleAuth', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.RoleAuthModel',

    idProperty: 'RoleAuthID',

    fields: [
		{ name: 'RoleAuthID' },
		{ name: 'RoleID' },
		{ name: 'AuthID' },
		{ name: 'Tag' },
		{ name: 'Description' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' },
		{ name: 'RoleCode' },
		{ name: 'RoleName' },
		{ name: 'AuthCode' },
		{ name: 'AuthName' },
		{ name: 'AuthType' }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" }
    }
});