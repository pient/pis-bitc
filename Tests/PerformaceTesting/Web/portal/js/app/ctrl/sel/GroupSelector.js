Ext.define('PIC.ctrl.sel.GroupSelector', {
    extend: 'PIC.ExtGridSelector',
    alternateClassName: 'PIC.GroupSelector',
    alias: 'widget.picgroupselector',
    gridCfg: null,
    selparams: null,

    requires: [
        'PIC.model.sys.org.Group',
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField: 'Name',
            valueField: 'Name',
            queryMode: 'local', // 树形节点等只需要一次查询的此设为true
            url: PICConfig.GroupSelectPath,
            selparams: {}
        }, config);

        config.selparams.type = config.selparams.type || $.getQueryString('type');

        var qryurl = $.combineQueryUrl(config.url, config.selparams || {});

        config.store = new Ext.create('PIC.TreeGridStore', {
            model: 'PIC.model.sys.org.Group',
            dsname: 'EntList',
            idProperty: 'GroupID',
            url: qryurl,
            picbeforeload: config.picbeforeload || function (proxy, params, node, operation) {
                PICUtil.setReqParams(params, 'querychildren', { type: me.selparams.type || '' });

                if (node) {
                    var nid = node.getId();
                    if (nid != "root") {
                        PICUtil.setReqParams(params, { pids: [nid] });
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
            // formparams: { url: "GroupEdit.aspx", style: { width: 600, height: 330} },
            columns: [
				{ id: 'col_Name', dataIndex: 'Name', xtype: 'treecolumn', header: "名称", formlink: false, width: 200, menuDisabled: true, sortable: true },
				{ id: 'col_Code', dataIndex: 'Code', header: "编号", flex: 1, menuDisabled: true, sortable: true }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});