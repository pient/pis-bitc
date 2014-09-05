
Ext.define('PIC.model.sys.org.OrgType', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.OrgTypeModel',

    idProperty: 'OrgTypeID',

    fields: [
		{ name: 'OrgTypeID' },
		{ name: 'Name' },
		{ name: 'Tag' },
		{ name: 'Description' }
    ],

    statics: {
        TypeEnum: PICState["OrgTypeEnum"]
    }
});