// 用户角色组，UserGroupView和UserGroupEdit视图专用

Ext.define('PIC.model.sys.org.UserGroupNode', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.UserGroupNode',
    idProperty: 'GroupID',

    fields: [
		{ name: 'UserID' },
		{ name: 'RoleID' },
		{ name: 'RoleCode' },
		{ name: 'RoleName' },
		{ name: 'UserName' },
		{ name: 'WorkNo' },
		{ name: 'GroupID' },
		{ name: 'Name' },
		{ name: 'Code' },
		{ name: 'Type' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
        { name: 'checked', type: 'boolean' },
        { name: 'leaf',
            convert: function (newValue, model) {
                return model.get("IsLeaf");
            }
        }
    ],

    statics: {
    }
});