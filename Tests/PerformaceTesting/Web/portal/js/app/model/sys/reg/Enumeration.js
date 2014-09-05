
Ext.define('PIC.model.sys.reg.Enumeration', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.EnumerationModel',

    idProperty: 'EnumerationID',

    fields: [
		{ name: 'EnumerationID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Value' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
		{ name: 'EditStatus' },
		{ name: 'Tag' },
		{ name: 'Description' },
		{ name: 'CreaterID' },
		{ name: 'CreaterName' },
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