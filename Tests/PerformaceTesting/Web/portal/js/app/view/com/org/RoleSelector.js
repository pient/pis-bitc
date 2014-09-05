Ext.define('PIC.view.com.org.RoleSelector', {
    extend: 'PIC.SelectorPage',
    alias: 'widget.picroleselector',

    alternateClassName: 'PIC.RoleSelectorPage',

    mode: null,
    id: null,
    refId: null,

    requires: [
        'PIC.model.sys.org.Role',
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.mode = config.mode || $.getQueryString("mode");
        me.id = config.mode || $.getQueryString("id");
        me.refId = config.mode || $.getQueryString("refid");
        
        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.org.Role',
            dsname: 'EntList',
            idProperty: 'RoleID',
            groupField: 'Type',
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
            features: [{
                ftype: 'grouping',
                groupHeaderTpl: ['{columnName}: {name:this.formatName} ({rows.length} 项)', {
                    formatName: function (name) { return PIC.RoleModel.TypeEnum[name]; }
                }],
                id: 'userGrouping'
            }],
            formparams: { url: "RoleEdit.aspx", style: { width: 600, height: 300} },
            tlitems: ['-', 'select', 'cancel', '->', 'schfield'],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
                { id: 'col_Type', dataIndex: 'Type', header: '类型', hidden: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '名称', juncqry: true, formlink: true, flex: 1, sortable: true, menuDisabled: true },
				{ id: 'col_Code', dataIndex: 'Code', header: '编号', juncqry: true, width: 120, sortable: true, menuDisabled: true }
            ]
        });

        me.items = [me.mainPanel];

        this.callParent([config]);
    }
});