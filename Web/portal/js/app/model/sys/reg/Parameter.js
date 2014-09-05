
Ext.define('PIC.model.sys.reg.Parameter', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.ParameterModel',

    idProperty: 'ParameterID',

    fields: [
		{ name: 'ParameterID' },
		{ name: 'CatalogID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Value' },
		{ name: 'Type' },
		{ name: 'RegexStr' },
		{ name: 'EditStatus' },
		{ name: 'SortIndex' },
		{ name: 'Description' },
		{ name: 'CreaterID' },
		{ name: 'CreaterName' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' }
    ],

    statics: {
        DataTypeEnum: {
            'String': '字符串',
            'Int32': '整型',
            'Boolean': '布尔型',
            'Decimal': 'Decimal型',
            'Double': 'Double型',
            'DateTime': '日期时间'
        },

        StatusEnum: {
            'Enabled': '有效',
            'Disabled': '无效'
        }
    }
});