Ext.define('PIC.view.setup.reg.param.ParamList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.RegParamListPage',

    requires: [
        'PIC.model.sys.reg.Parameter'
    ],

    mainPanel: null,

    catalogId: "",
    currentCatalog: null,

    constructor: function (config) {
        var me = this;
        me.catalogId = $.getQueryString({ ID: "cid" });
        me.currentCatalog = PICState["Catalog"];

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.reg.Parameter',
            dsname: 'EntList',
            idProperty: 'ParameterID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.cid = me.catalogId;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "ParamEdit.aspx", style: { width: 650, height: 350} },
            tlitems: ['-', 'add', 'edit', 'delete', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} }
            ],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 200, sortable: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, width: 200, sortable: true },
				{ id: 'col_Value', dataIndex: 'Value', header: '值', juncqry: true, flex: 1, sortable: true }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    reloadPage: function (cid, currentCatalog) {
        var me = this;

        me.catalogId = cid;
        me.currentCatalog = currentCatalog;

        me.dataStore.load();
    }
});