Ext.define('PIC.ctrl.sel.AuthSelector', {
    extend: 'PIC.ExtGridSelector',
    alternateClassName: 'PIC.AuthSelector',
    alias: 'widget.picauthselector',
    gridCfg: null,
    selparams: null,

    requires: [
        'PIC.model.sys.auth.Auth',
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField: 'Name',
            valueField: 'Name',
            queryMode: 'local', // 树形节点等只需要一次查询的此设为true
            url: PICConfig.AuthSelectPath,
            selparams: {},
            multiSelect: true
        }, config);

        var qryurl = $.combineQueryUrl(config.url, config.selparams || {});

        config.store = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.auth.Auth',
            dsname: 'EntList',
            idProperty: 'AuthID',
            url: qryurl,
            picbeforeload: config.picbeforeload || function (proxy, params, node, operation) {
                PICUtil.setReqParams(params, 'querychildren');
                if (node) {
                    var nid = node.getId();
                    if (nid != "root") {
                        params.data.pids = [node.getId()];
                    }
                }

                if (typeof (config.selbeforeload) == "function") {
                    config.selbeforeload(proxy, params, node, operation);
                }
            }
        });

        config.gridCfg = Ext.apply({
            gridType: 'PIC.TreeGridPanel',
            height: 200,
            width: 325,
            // formparams: { url: "AuthEdit.aspx", style: { width: 600, height: 330} },
            columns: [
				{ id: 'col_Name', dataIndex: 'Name', xtype: 'treecolumn', header: "名称", formlink: false, width: 200, menuDisabled: true, sortable: true },
				{ id: 'col_Code', dataIndex: 'Code', header: "编号", flex: 1, menuDisabled: true, sortable: true }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});