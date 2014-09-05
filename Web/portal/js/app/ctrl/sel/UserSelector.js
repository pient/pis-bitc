Ext.define('PIC.ctrl.sel.UserSelector', {
    extend: 'PIC.ExtGridSelector',
    alternateClassName: 'PIC.UserSelector',
    alias: 'widget.picuserselector',
    gridCfg: null,
    selparams: null,

    requires: [
        'PIC.model.sys.org.User',
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField: 'Name',
            valueField: 'Name',
            url: PICConfig.UserSelectPath,
            selparams: {}
        }, config);

        var qryurl = $.combineQueryUrl(config.url, config.selparams || {});

        config.store = new Ext.create('PIC.PageGridStore', {
            autoLoad: false,
            model: 'PIC.model.sys.org.User',
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
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '姓名', formlink: true, juncqry: true, width: 100, sortable: true, menuDisabled: true },
				{ id: 'col_WorkNo', dataIndex: 'WorkNo', header: '工号', juncqry: true, flex: 1, sortable: true, menuDisabled: true }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});