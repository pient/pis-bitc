
Ext.define('PIC.model.sys.org.Function', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.FunctionModel',

    idProperty: 'FunctionID',

    fields: [
		{ name: 'FunctionID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'SortIndex' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'Description' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreateDate' }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" }
    }
});