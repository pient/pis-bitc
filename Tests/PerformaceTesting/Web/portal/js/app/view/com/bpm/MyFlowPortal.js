Ext.define('PIC.view.com.bpm.MyFlowPortal', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.BpmMyFlowPortal',

    requires: [
        'PIC.model.sys.bpm.WfCatalog',
        'PIC.view.portal.home.PortalPanel',
        'PIC.view.portal.home.PortalColumn'
    ],

    layoutdef: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.flowList = PICState["FlowList"] || [];
        me.flowStatList = PICState["FlowStatList"] || [];
        me.flowFreqList = PICState["FlowFreqList"] || [];   // 流程使用频率信息

        me.freqPanel = Ext.create("PIC.ExtPanel", {
            title: '常用流程',
            region: 'east',
            bodyStyle: 'background:#fff;',
            frame: true,
            border: false,
            margin: '5 5 5 5',
            width: 220
        });

        me.renderFreqPanel();

        me.mainPanel = Ext.create("PIC.view.com.bpm.MyFlowPortalPanel", {
            region: 'center',
            margin: '2 2 2 2',
            tbar: ['-', {
                xtype: 'picsearchfield',
                fieldLabel: '&nbsp;&nbsp;&nbsp;&nbsp;流程查询',
                labelWidth: 70,
                onsearch: function (fld, val) {
                    me.doSearch(fld, val);
                }
            }],
            items: []
        });

        me.renderMainPanel();

        me.items = [me.mainPanel, me.freqPanel]

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },
    
    renderMainPanel: function () {
        var me = this;
        var catalogData = me.getFlowCatalogData();

        me.mainPanel.removeAll();   // 清空原组件中内容

        var mainItems = [];

        // 默认禁止动画，否则会导致页面重新获取
        Ext.each(catalogData, function (cdata) {
            var p = me.generatePortlet(cdata);

            mainItems.push(p);
        });

        me.mainPanel.add(Ext.create('PIC.ExtPanel', {
            columnWidth: 1,
            border: false,
            margin: '5 5 5 5',
            items: mainItems
        }));

        me.mainPanel.doLayout();
    },

    renderFreqPanel: function () {
        var me = this;

        var html = me.generatePortletHtml({
            FlowList: me.flowFreqList
        });

        var dom = $(html).addClass('freq-item');
        html = dom[0].outerHTML;

        me.freqPanel.update(html);
    },

    generatePortlet: function (cdata) {
        var me = this;

        var html = me.generatePortletHtml(cdata);

        var p = {
            title: cdata.Name,
            bodyStyle: 'background:#fff; ',
            collapsible: true,
            frame: true,
            margin: '2 2 2 2',
            animate: true,
            animCollapse: true,
            html: html
        };

        return p;
    },

    generatePortletHtml: function (cdata) {
        var me = this;

        var html = new Ext.XTemplate(
        '<tpl for=".">',
            '<div class="item-outer">',
                '<ul class="item-outer-body">',
                    '<tpl for="FlowList">',
                        '<li class="item-inner" nodeIndex={[xindex-1]}>',
                            '<a onclick="PICPage.onFlowLinkClick(\'{DefineID}\')">{Name}</a>&nbsp;',
                            '(<a class="item-inner-start" onclick="PICPage.onFlowCountLinkClick(\'{DefineID}\')">{FreqCount}</a>)',
                        '</li>',
                    '</tpl>',
                '</ul>',
            '</div>',
        '</tpl>'
        ).apply(cdata);

        return html;
    },

    getFlowCatalogData: function () {
        var me = this;

        var catalogData = [];
        var catalogEnum = PIC.WfCatalogModel.CatalogEnum;

        var i = 0;
        for (var key in catalogEnum) {
            var fList = [];

            Ext.each(me.flowList, function (f) {
                if (f["Catalog"] === key) {
                    fList.push(f);
                }

                Ext.each(me.flowFreqList, function (ff) {
                    if (f["DefineID"] === ff["DefineID"]) {
                        f["FreqCount"] = ff["FreqCount"];  // 待办数

                        return false;
                    }
                });

                f["FreqCount"] = f["FreqCount"] || 0;

                //Ext.each(me.flowStatList, function (s) {
                //    if (s["DefineID"] === f["DefineID"]) {
                //        f["NewCount"] = s["NewCount"];  // 待办数
                //        f["NewMyCount"] = s["NewMyCount"];  // 自己的待办数
                //        f["NewAgentCount"] = s["NewAgentCount"];  // 代理的待办数

                //        return false;
                //    }
                //});

                //f["NewCount"] = f["NewCount"] || 0;
                //f["NewMyCount"] = f["NewMyCount"] || 0;
                //f["NewAgentCount"] = f["NewAgentCount"] || 0;
            });

            if (fList.length > 0) {
                catalogData.push({
                    Index: ++i,
                    Code: key,
                    Name: catalogEnum[key],
                    FlowList: fList
                });
            }
        }

        return catalogData;
    },

    getFlowByCode: function (code) {
        var me = this;
        var flow = null;

        Ext.each(me.flowList, function (f) {
            if (f["Code"] == code) {
                flow = f;
                return false;
            }
        });

        return flow;
    },

    onFlowLinkClick: function (did) {
        PICUtil.openFlowBusDialog({
            op: 'c',
            did: did
        });
    },

    onFlowCountLinkClick: function (did) {
        var me = this;

        // var f = me.getFlowByCode(code);
        PICUtil.openDialog("MyFlow.aspx", { did: did }, { height: 400, width: 750 });
    },

    doSearch: function (fld, val) {
        var me = this;

        PICUtil.ajaxRequest('query', {
            onsuccess: function (respData, opts) {
                if (respData) {
                    me.flowList = respData["FlowList"];

                    me.renderMainPanel();
                }
            }
        }, { qrytext: val });
    }
});
