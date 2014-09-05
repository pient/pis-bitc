
Ext.define('PIC.model.sys.org.Group', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.GroupModel',

    idProperty: 'GroupID',

    fields: [
		{ name: 'GroupID' },
		{ name: 'Name' },
		{ name: 'Code' },
		{ name: 'Attr' },
		{ name: 'PrincipalID' },
		{ name: 'PrincipalName' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'SortIndex' },
		{ name: 'Description' },
		{ name: 'Tag' },
		{ name: 'Status' },
		{ name: 'ModifiedSortIndex' },
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
        AttrEnum: { '校区': "校区", '部门': "部门" }
    }
});