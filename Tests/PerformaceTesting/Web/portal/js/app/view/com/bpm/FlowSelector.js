Ext.define('PIC.view.com.bpm.FlowSelector', {
    extend: 'PIC.SelectorPage',

    alternateClassName: 'PIC.FlowSelectorPage',

    mode: null,
    id: null,
    refId: null,

    requires: [
        'PIC.model.sys.bpm.WfDefine',
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.id = config.mode || $.getQueryString("id");
        me.mode = config.mode || $.getQueryString("mode");
        me.refId = config.mode || $.getQueryString("refid");
        
        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.bpm.WfDefine',
            dsname: 'EntList',
            idProperty: 'DefineID',
            groupField: 'Catalog',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.id = me.id;
                params.data.mode = me.mode;
                params.data.refid = me.refId;
            }
        });

        me.mainPanel = Ext.create('PIC.GridSelectorPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            schpanel: false,
            features: [{
                ftype: 'grouping',
                groupHeaderTpl: ['{columnName}: {name:this.formatName} ({rows.length} 项)', {
                    formatName: function (name) { return PIC.WfDefineModel.CatalogEnum[name] || name; }
                }]
            }],
            formparams: { url: PICMdlRootPath + "/Setup/Bpm/WfDefineEdit.aspx", style: { width: 700, height: 500 } },
            tlitems: ['-', 'select', 'cancel', '->', 'schfield'],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
                { dataIndex: 'Catalog', header: '类别', hidden: true },
				{ dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex: 1, sortable: true, menuDisabled: true },
				{ dataIndex: 'Code', header: '编号', juncqry: true, width: 180, sortable: true, menuDisabled: true }
            ]
        });

        me.items = [me.mainPanel];

        this.callParent([config]);
    }
});