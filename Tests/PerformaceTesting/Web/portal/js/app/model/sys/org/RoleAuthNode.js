
Ext.define('PIC.model.sys.org.RoleAuthNode', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.RoleAuthNode',
    idProperty: 'AuthID',

    fields: [
		{ name: 'RoleID' },
		{ name: 'Tag' },
		{ name: 'Description' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' },
		{ name: 'RoleName' },
		{ name: 'RoleCode' },
		{ name: 'AuthID' },
		{ name: 'Name' },
		{ name: 'Code' },
		{ name: 'Type' },
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