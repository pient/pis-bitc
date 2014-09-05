Ext.define('PIC.view.setup.org.GroupRoleView', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.GroupRoleViewPage',

    requires: [
        'PIC.model.sys.org.GroupRoleNode',
    ],

    pageData: {},

    mainPanel: null,

    refId: null,
    mode: null, // group

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.refId = config.id || $.getQueryString('id');

        me.pageData = config.pageData;

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.org.GroupRoleNode',
            dsname: 'EntList',
            idProperty: 'RoleID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.id = me.refId;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            border: false,
            store: me.dataStore,
            formparams: { url: "RoleEdit.aspx", style: { width: 650, height: 300} },
            tlitems: ['-', 'view', '->', 'schfield', '-', 'help'],
            schpanel: false,
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '角色名称', juncqry: true, formlink: true, width: 150, sortable: true },
                { id: "col_Code", dataIndex: 'Code', header: "角色编号", juncqry: true, width: 150, sortable: true },
                { id: "col_Description", dataIndex: 'Description', header: "描述", juncqry: true, flex: 1, sortable: true }
            ],
            beforeformload: function (cfg, id, args, params) {
                me.mainPanel.formparams.params.gid = me.refId;

                if (!me.refId) {
                    PICMsgBox.alert("请先选择对应组");

                    return false;
                }
            }
        });

        me.items = me.mainPanel;

        this.callParent([config]);

        me.addEvents('save');
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    reloadData: function (params) {
        var me = this;
        me.refId = params.id || me.refId;

        me.dataStore.load();
    }
});