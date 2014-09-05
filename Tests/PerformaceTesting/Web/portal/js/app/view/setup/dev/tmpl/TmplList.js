Ext.define('PIC.view.setup.dev.tmpl.TmplList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.DevTmplListPage',

    requires: [
        'PIC.model.sys.dev.TemplateCatalog',
        'PIC.model.sys.dev.Template'
    ],

    mainPanel: null,

    catalogId: "",
    currentCatalog: null,

    constructor: function (config) {
        var me = this;

        me.catalogId = $.getQueryString({ ID: "cid" });

        if (PICState["Catalog"]) {
            me.currentCatalog = Ext.create('PIC.model.sys.dev.TemplateCatalog', PICState["Catalog"]);
        }

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.dev.Template',
            dsname: 'EntList',
            idProperty: 'TemplateID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.cid = me.catalogId;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "TmplEdit.aspx", style: { width: 650, height: 320} },
            tlitems: ['-', 'add', 'edit', 'delete', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '编号', id: 'Code', schopts: { qryopts: "{ mode: 'Like', field: 'Code' }"} },
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }"} }
            ],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'SortIndex', header: '排序号', width: 60, sortable: true, align: 'center' },
				{ dataIndex: 'Code', header: '编号', juncqry: true, width: 180, sortable: true },
				{ dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, width: 180, sortable: true },
				{ dataIndex: 'Status', header: '状态', width: 80, enumdata: PIC.TemplateModel.StatusEnum, sortable: true, align: 'center' },
				{ dataIndex: 'Description', header: '描述', juncqry: true, flex: 1, sortable: true }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    reloadPage: function (currentCatalog) {
        var me = this;

        if (me.currentCatalog) {
            me.currentCatalog = currentCatalog;
            me.catalogId = currentCatalog.getId();
        }

        me.dataStore.load();
    }
});