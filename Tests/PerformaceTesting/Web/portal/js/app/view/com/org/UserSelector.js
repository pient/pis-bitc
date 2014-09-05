Ext.define('PIC.view.com.org.UserSelector', {
    extend: 'PIC.SelectorPage',

    alternateClassName: 'PIC.UserSelectorPage',

    mode: null,
    id: null,
    refId: null,

    requires: [
        'PIC.model.sys.org.User',
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.id = config.mode || $.getQueryString("id");
        me.mode = config.mode || $.getQueryString("mode");
        me.refId = config.mode || $.getQueryString("refid");
        
        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.org.User',
            dsname: 'EntList',
            idProperty: 'UserID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.mode = me.mode;
                params.data.id = me.id;
                params.data.refid = me.refId;
            }
        });

        me.mainPanel = Ext.create('PIC.GridSelectorPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            schpanel: false,
            formparams: { url: PICMdlRootPath + "/Setup/Org/UserEdit.aspx", style: { width: 600, height: 330 } },
            tlitems: ['-', 'select', 'cancel', '->', 'schfield'],
            columns: [
                { dataIndex: 'Id', header: '标识', hidden: true },
				{ dataIndex: 'Name', header: '姓名', juncqry: true, formlink: true, flex: 1, sortable: true, menuDisabled: true },
				{ dataIndex: 'WorkNo', header: '工号', juncqry: true, width: 180, sortable: true, menuDisabled: true }
            ]
        });

        me.items = [me.mainPanel];

        this.callParent([config]);
    }
});