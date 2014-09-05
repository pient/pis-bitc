
Ext.define('PIC.model.sys.org.GroupFunc', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.GroupFuncModel',

    idProperty: 'GroupFunctionID',

    fields: [
		{ name: 'GroupFunctionID' },
		{ name: 'GroupID' },
		{ name: 'FunctionID' },
		{ name: 'Tag' },
		{ name: 'Description' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' },
		{ name: 'GroupCode' },
		{ name: 'GroupName' },
		{ name: 'FuncCode' },
		{ name: 'FuncName' },
		{ name: 'FuncType' }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" }
    }
});