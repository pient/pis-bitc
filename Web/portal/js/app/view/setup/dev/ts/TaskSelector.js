Ext.define('PIC.view.setup.dev.ts.TaskSelector', {
    extend: 'PIC.ExtGridSelector',
    alternateClassName: 'PIC.TSTaskSelector',
    alias: 'widget.pic-tstaskselector',
    gridCfg: null,
    selparams: null,

    requires: [
        'PIC.model.sys.dev.Task'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField: 'Name',
            valueField: 'Name',
            url: PICUtil.getAppFullPath('/Modules/Setup/Dev/TS/TaskList.aspx'),
            selparams: {}
        }, config);

        var qryurl = $.combineQueryUrl(config.url, config.selparams || {});

        config.store = new Ext.create('PIC.PageGridStore', {
            autoLoad: false,
            model: 'PIC.model.sys.dev.Task',
            dsname: 'EntList',
            idProperty: 'UserID',
            url: qryurl,
            picbeforeload: config.picbeforeload || config.selbeforeload || Ext.emptyFn
        });

        config.gridCfg = Ext.apply({
            gridType: 'PIC.PageGridPanel',
            height: 200,
            width: 225,
            schpanel: false,
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Name', header: '名称', formlink: true, juncqry: true, width: 100, sortable: true, menuDisabled: true },
				{ dataIndex: 'Code', header: '编码', juncqry: true, flex: 1, sortable: true, menuDisabled: true }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});