
Ext.define('PIC.model.sys.reg.ParameterCatalog', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.ParameterCatalogModel',

    idProperty: 'ParameterCatalogID',

    fields: [
		{ name: 'ParameterCatalogID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'EditStatus' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
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