
Ext.define('PIC.model.doc.File', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.DocFileModel',

    idProperty: 'FileID',

    fields: [
		{ name: 'FileID' },
		{ name: 'ModuleID' },
		{ name: 'DirectoryID' },
		{ name: 'GroupID' },
		{ name: 'VersionGroupID' },
		{ name: 'Name' },
		{ name: 'Tag' },
		{ name: 'Description' },
		{ name: 'ExtName' },
		{ name: 'Size', type: 'int' },
        {
            name: 'SizeStr',
            convert: function (newValue, model) {
                var size = model.get("Size") || 0;
                var kbSize = model.get("Size") / 1024;

                if (kbSize < 1000) {
                    return kbSize.toFixed(1) + ' KB';
                } else {
                    var mbSize = kbSize / 1024;
                    return mbSize.toFixed(2) + ' MB';
                }
            }
        },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'CreatedDate' },
		{ name: 'LastModifiedDate' },
        { name: 'leaf',
            convert: function (newValue, model) {
                return model.get("IsLeaf");
            }
        }
    ],

    statics: {
    }
});