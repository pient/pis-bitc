
Ext.define('PIC.view.portal.default.SubMenuPanel', {
    extend: 'PIC.ExtTreePanel',
    alias: 'widget.picsubportalmenupanel',
    alternateClassName: 'PIC.SubPortalMenuPanel',

    menuPanel: null,

    currentModule: null,
    modules: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            hideHeaders: true,
            collapsible: true,
            collapseDirection: 'left',
            border: false,
            split: true,
            noline: true,
            useArrows: true,
            rootVisible: false,
            width: 180,
            expanded: false,
            minSize: 150,
            maxSize: 260,
            title: '<div class="pic-icon-help" style="cursor:hand" onclick="PICPortal.LoadHelp()" title="帮助">&nbsp;</div>'
        }, config);

        me.modules = config.modules || [];
        me.currentModule = config.currentModule || {};

        var data = me.adjustNodeData(me.modules);
        
        config.store = Ext.create('PIC.ExtTreeStore', {
            model: 'PIC.model.portal.ModuleItem',
            rootPathLevel: 1,
            idProperty: 'ModuleID',
            data: data
        });
        
        config.columns = [
            { id: 'col_SortIndex', dataIndex: 'SortIndex', header: '排序号', hidden: true },
            { id: 'col_Name', xtype: 'treecolumn', dataIndex: 'Name', flex: 1, header: "名称" }
        ];

        me.callParent([config]);
    },

    adjustNodeData: function (data) {
        var me = this;
        if (Ext.isArray(data) && data.length > 0) {
            Ext.each(data, function (node) {
                if (me.currentModule.ID.equals(this.ParentID)) {
                    this.ParentID = null;
                }

                node['Icon'] = me.getIconUrl(node);
            });
        }

        return data;
    },

    getIconUrl: function (node, baseurl) {
        var baseurl = baseurl || ICON_IMG_BASE;
        var iconurl = node['Icon'];
        var nodetype = node['Type'];

        if (!iconurl) {
            switch (nodetype) {
                case 2:
                    iconurl = "addrbook.png";
                    break;
                case 1:
                    iconurl = "html.png";
                    break;
            }
        }

        if (iconurl.indexOf('/') < 0) {
            iconurl = baseurl + iconurl;
        }

        return iconurl;
    },

    statics: {
    }
});
