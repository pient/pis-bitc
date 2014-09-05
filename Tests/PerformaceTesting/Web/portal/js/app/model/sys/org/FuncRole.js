
Ext.define('PIC.model.sys.org.FuncRole', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.FuncRoleModel',

    idProperty: 'FunctionRoleID',

    fields: [
		{ name: 'FunctionRoleID' },
		{ name: 'FunctionID' },
		{ name: 'RoleID' },
		{ name: 'Tag' },
		{ name: 'Description' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' },
		{ name: 'FuncName' },
		{ name: 'FuncCode' },
		{ name: 'RoleName' },
		{ name: 'RoleCode' },
		{ name: 'RoleType' }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" }
    }
});