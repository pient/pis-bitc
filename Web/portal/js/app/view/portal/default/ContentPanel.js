

Ext.define('PIC.view.portal.default.ContentPanel', {
    extend: 'PIC.ExtPanel',
    alias: 'widget.picportalcontentpanel',
    alternateClassName: 'PIC.PortalContentPanel',

    frameContent: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            title: '&nbsp;'
        }, config);

        var frame_id = me.id + "_frame";
        var frame_html = '<iframe width="100%" height="100%" id="' + frame_id + '" name="' + frame_id + '" contentid="' + me.id + '" frameborder="0"></iframe>'

        me.callParent([config]);

        me.on('afterrender', function () {
            me.body.update(frame_html);
            frameContent = Ext.get(frame_id);
        })
    },

    loadPage: function (url, title) {
        if (url && url !== frameContent.dom.src) {
            frameContent.dom.src = url;

            this.loadTitle({ url: url, title: title });
        }
    },

    loadTitle: function (tit_data) {
        var me = this;

        if (tit_data && tit_data.title && tit_data.url) {
            me.setTitle('<div align="left"><a href="' + tit_data.url + '" target="' + frameContent.dom.id + '">' + tit_data.title + '</a></div>');
        } else {
            me.setTitle('<div align="left"><a href="/portal/home.aspx" target="' + frameContent.dom.id + '">主页</a></div>');
        }
    },

    statics: {
        OnContentLoad: function (frame) {
            // 加载路径
            if (!frame.contentWindow.PICPath) {
                PICPortal.contentPanel.loadTitle();
            }
        }
    }
});