Ext.define('PIC.ctrl.sel.FuncSelector', {
    extend: 'PIC.ExtGridSelector',
    alternateClassName: 'PIC.FuncSelector',
    alias: 'widget.picfuncselector',
    gridCfg: null,
    selparams: null,

    requires: [
        'PIC.model.sys.org.Function',
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField: 'Name',
            valueField: 'Name',
            url: PICConfig.FuncSelectPath,
            selparams: {}
        }, config);

        var qryurl = $.combineQueryUrl(config.url, config.selparams || {});

        config.store = new Ext.create('PIC.PageGridStore', {
            autoLoad: false,
            model: 'PIC.model.sys.org.Function',
            dsname: 'EntList',
            idProperty: 'FunctionID',
            url: qryurl,
            picbeforeload: config.picbeforeload || config.selbeforeload || Ext.emptyFn
        });

        config.gridCfg = Ext.apply({
            gridType: 'PIC.PageGridPanel',
            height: 200,
            width: 225,
            schpanel: false,
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
                { id: 'col_Type', dataIndex: 'Type', header: '类型', hidden: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, width: 100, sortable: true, menuDisabled: true },
				{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, flex: 1, sortable: true, menuDisabled: true }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});