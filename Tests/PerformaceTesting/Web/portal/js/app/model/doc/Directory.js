
Ext.define('PIC.model.doc.Directory', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.DocDirectoryModel',

    idProperty: 'DirectoryID',

    fields: [
		{ name: 'DirectoryID' },
		{ name: 'ModuleID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'Tag' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
		{ name: 'Description' },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' },
        { name: 'leaf',
            convert: function (newValue, model) {
                return model.get("IsLeaf");
            }
        }
    ],

    statics: {
    }
});