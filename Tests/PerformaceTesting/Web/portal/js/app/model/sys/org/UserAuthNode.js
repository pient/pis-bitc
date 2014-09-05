
Ext.define('PIC.model.sys.org.UserAuthNode', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.UserAuthNode',
    idProperty: 'AuthID',

    fields: [
		{ name: 'UserID' },
		{ name: 'WorkNo' },
		{ name: 'UserName' },
		{ name: 'LoginName' },
		{ name: 'AuthID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'ModuleID' },
		{ name: 'Data' },
		{ name: 'Description' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreateDate' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
        { name: 'checked', type: 'boolean' },
        { name: 'leaf',
            convert: function (newValue, model) {
                return model.get("IsLeaf");
            }
        }
    ]
});