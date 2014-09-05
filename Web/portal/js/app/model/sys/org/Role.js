
Ext.define('PIC.model.sys.org.Role', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.RoleModel',

    idProperty: 'RoleID',

    fields: [
		{ name: 'RoleID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'Description' },
		{ name: 'SortIndex' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreateDate' }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" }
    }
});