
Ext.define('PIC.view.portal.default.MenuPanel', {
    extend: 'PIC.ExtTreePanel',
    alias: 'widget.picportalmenupanel',
    alternateClassName: 'PIC.PortalMenuPanel',

    requires: [
        'PIC.model.portal.ModuleItem',
        'PIC.view.portal.default.ContentPanel'
    ],

    headerPanel: null,
    menuPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            hideHeaders: true,
            rootVisible: false,
            border: false,
            noline: true,
            collapsible: false,
            useArrows: true,
            split: false,
            width: 180,
            expanded: true,
            minSize: 150,
            maxSize: 250,
            margins: '0 0 0 0'
        }, config);

        var data = me.adjustNodeData(config.data || PICState["MdlList"] || []);

        config.store = Ext.create('PIC.ExtTreeStore', {
            model: 'PIC.model.portal.ModuleItem',
            rootPathLevel: 1,
            idProperty: 'ModuleID',
            expanded: true,
            folderSort: true,
            proxy: Ext.create("PIC.data.ExtJsonProxy"),
            data: data
        });
        
        config.tbar = Ext.create("PIC.ExtPanel", {
            border: false,
            height: 68,
            contentEl: 'left_header',
            bodyStyle: "background:#fafafa; padding: 5px;"
        });

        config.columns = [
            { id: 'col_SortIndex', dataIndex: 'SortIndex', header: '排序号', hidden: true },
            { id: 'col_Name', dataIndex: 'Name', flex: 1, header: "名称", xtype: 'treecolumn' }
        ];

        me.callParent([config]);

        /*me.on("afterrender", function (cmp, e) {
        nodes = Ext.getDom(cmp).getView().getNodes();
        Ext.Object.each(nodes, function (key, item, myself) {
        item.style.height = '23px';
        });
        });*/
    },

    adjustNodeData: function (data) {
        var me = this;
        if (Ext.isArray(data) && data.length > 0) {
            Ext.each(data, function (node) {
                node['Icon'] = me.getIconUrl(node['Icon']);
            });
        }

        return data;
    },

    getIconUrl: function (icon, baseurl) {
        var baseurl = baseurl || ICON_IMG_BASE;
        var iconurl = icon;

        if (iconurl) {
            if (iconurl.indexOf('/') < 0) {
                iconurl = baseurl + iconurl;
            }
        }

        return iconurl;
    },

    statics: {
        RootPathLevel: 1,

        NameRenderer: function (val, meta, rec, rowIdx, colIdx, store, view) {
            var rtn = val || "";

            if (rec.get("PathLevel") == PIC.PortalMenuPanel.RootPathLevel) {
                rtn = '<span style="font-weight: bold; margin-top:10px; color:gray">' + val + '</span>';
            }

            if ('M_SYS_FAV_MSG'.equals(rec.get('Code'))) {
                rtn += '<span style="font-weight:bold; color: red; margin:2px;">(<span id="mitem_msgnew">0</span>)</span>'
            }

            if ('M_SYS_FAV_TASK'.equals(rec.get('Code'))) {
                rtn += '<span style="font-weight:bold; color: red; margin:2px;">(<span id="mitem_tasknew">0</span>)</span>'
            }


            return rtn;
        }
    }
});
