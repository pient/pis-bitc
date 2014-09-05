
Ext.define('PIC.model.sys.auth.Auth', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.AuthModel',

    idProperty: 'AuthID',

    fields: [
		{ name: 'AuthID' },
		{ name: 'Name' },
		{ name: 'Code' },
		{ name: 'Type' },
		{ name: 'ModuleID' },
		{ name: 'Data' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
		{ name: 'Description' },
		{ name: 'Tag' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreateDate' },
        { name: 'leaf',
            convert: function (newValue, model) {
                return model.get("IsLeaf");
            }
        }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" },
        TypeEnum: PICState["AuthTypeEnum"] || {}
    }
});