
Ext.define('PIC.model.sys.dev.TemplateCatalog', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.TemplateCatalogModel',

    idProperty: 'CatalogID',

    fields: [
		{ name: 'CatalogID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
		{ name: 'EditStatus' },
		{ name: 'Config' },
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

    getConfig: function () {
        var cfgStr = this.get("Config");
        var cfgObj = null;

        if (cfgStr) {
            cfgObj = $.getJsonObj(cfgStr);
        }

        return cfgObj;
    },

    getItemConfig: function () {
        var cfg = this.getConfig();

        var itemCfg = {
            CfgEditor: {
                Type: "Custom",
                Style: { width: 650, height: 550 }
            }
        };

        if (cfg && cfg.ItemCfg) {
            itemCfg = Ext.apply(itemCfg, cfg.ItemCfg || {});
        }

        return itemCfg;
    },

    statics: {
    }
});