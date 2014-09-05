
Ext.define('PIC.view.portal.home.Viewport', {
    extend: 'PIC.ExtViewport',
    alias: 'widget.picportalviewport',
    alternateClassName: 'PIC.PortalViewport',

    requires: [
        'PIC.view.portal.home.PortalPanel',
        'PIC.view.portal.home.PortalColumn'
    ],

    layoutdef: null,

    constructor: function (config) {
        var me = this;

        me.layoutdef = PICState["Layout"];
        me.sysdatamodules = PICState["SysDataModule"];

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.layout = me.generateLayout();

        me.items = Ext.create("PIC.view.portal.home.PortalPanel", {
            region: 'center',
            margins: '2 2 2 2',
            items: me.layout
        });

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        me.on('afterrender', function () {
            me.renderLayout();
        });

        this.callParent(arguments);
    },

    generateLayout: function () {
        var me = this;

        var cfg = $.getJsonObj(me.layoutdef['Config']);
        var layout = cfg;

        var tools = me.getTools();

        // 默认禁止动画，否则会导致页面重新获取
        $.each(layout, function () {
            var col = this;
            if (col && col.items) {
                $.each(col.items, function () {
                    var item = this;
                    item.tools = tools;
                    item.animate = (item.animate == true);
                    item.animCollapse = (item.animCollapse == true);
                })
            }
        });

        return layout;
    },

    renderLayout: function () {
        var me = this;
        var portlets = me.query("portlet[code]");

        Ext.each(portlets, function (p) {
            p.setLoading('正在加载...');

            if (p.code) {
                if (p.code.indexOf(".") < 0) {
                    p.code = "Sys.Portal.Portlet." + p.code;
                }
            }

            PICUtil.getTmplData('renderstr', {
                onsuccess: function (respData, opts, resp) {
                    p.setLoading(false);

                    me.renderPortlet(p, resp.responseText);
                }
            }, { tcode: p.code, ctxparams: p.ctxparams });
        });
    },

    renderPortlet: function (p, respText) {
        var me = this;

        if (respText) {
            p.body.update(respText);

            // 调整viewport,防止出现横向滚动条
            me.doLayout();
        }

        // 图片新闻需要特殊处理
        if ("Sys.Portal.Portlet.PicNews".equals(p.code)) {
            $(".ppy").popeye({
                autoslide: true,
                slidespeed: 5000,
                caption: 'permanent'
            });
        }
    },

    // 替换相关值
    translatePICVar: function (expr) {
        var rtnstr = expr;

        rtnstr = expr.replace(/\#{([\w,.,_]+)\}/g, function (match, value) {
            var str = value;

            if (value) {
                var vals = value.split('.');
                if (vals.length == 2) {
                    str = PICVar[vals[0]][vals[1]];
                } else if (vals.length == 1 && PICVar[vals[0]]) {
                    str = PICVar[vals[0]];
                }
            }

            return str;
        });

        return rtnstr;
    },

    getTools: function () {
        return [{
            xtype: 'tool',
            type: 'restore',
            handler: function (e, target, header, tool) {
                var p = header.ownerCt;

                var scriptStr = $(p.el.dom).find("script[class=more]").text();

                if (scriptStr) {
                    eval(scriptStr);
                }
            }
        }];
    },

    showMsg: function(msg) {
        var el = Ext.get('app-msg'),
            msgId = Ext.id();

        this.msgId = msgId;
        el.update(msg).show();

        Ext.defer(this.clearMsg, 3000, this, [msgId]);
    },

    clearMsg: function(msgId) {
        if (msgId === this.msgId) {
            Ext.get('app-msg').hide();
        }
    },

    statics: {
    }
});
