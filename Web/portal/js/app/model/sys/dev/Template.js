
Ext.define('PIC.model.sys.dev.Template', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.TemplateModel',

    idProperty: 'TemplateID',

    fields: [
		{ name: 'TemplateID' },
		{ name: 'CatalogID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'Config' },
		{ name: 'Tag' },
		{ name: 'Status' },
		{ name: 'EditStatus' },
		{ name: 'SortIndex' },
		{ name: 'Description' },
		{ name: 'CreaterID' },
		{ name: 'CreaterName' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreatedDate' }
    ],

    getConfig: function () {
        var cfgStr = this.get("Config");
        var cfgObj = null;

        if (cfgStr) {
            cfgObj = $.getJsonObj(cfgStr);
        }

        return cfgObj;
    },

    statics: {
        StatusEnum: { 'Enabled': '启用', 'Disabled': '停用' },
        SNIncTypeEnum: PICState["SNIncTypeEnum"] || {},
        TypeEnum: { Standard: '标准', Data: '数据', Code: '编码', SN: '序列号', Custom: '自定义' }
    }
});