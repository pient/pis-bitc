/*
* pgfunc对ext选择页面的扩展
* @author Ray Liu
*/
Ext.define('PIC.SelectorPage', {
    extend: 'PIC.Page',
    alias: 'widget.pic-selectorpage',

    selPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIC.SelectorPanelMixin', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.pic-selectorpanelmixin',

    seltype: 'multi',  // multi(多选), single(单选)
    rtntype: 'array', // string, json(json字符串), record(Ext DataRecord), array(数组)
    openerCmp: null,

    returnValue: function (rtns, params) {
        var rtnData = rtns || {},
            fieldmap, strs,
            callback;

        if (typeof (dialogArguments) != "undefined" && dialogArguments) {
            fieldmap = dialogArguments.fieldmap || "";
        } else {
            fieldmap = $.getQueryString("fieldmap", "");
            callback = $.getQueryString("callback", null);
        }

        strs = fieldmap.split(";");

        var _params = $.getAllQueryStrings() || {};
        _params = Ext.apply(_params, params || {});

        var rtndata = { result: "success", data: rtns || {}, params: _params };

        if (typeof (dialogArguments) != "undefined" && dialogArguments) {
            window.returnValue = rtndata;
        } else if (window.opener) {
            var tstr = [];

            if (window.opener.Ext) {
                Ext.each(strs, function (s) {
                    tstr = s.split(":");

                    if (tstr[0] && tstr[1]) {
                        var f = window.opener.Ext.getCmp(tstr[0]);
                        var v = rtnData[tstr[1]] || "";

                        if (f.setValue) {
                            f.setValue(v);
                        }
                    }
                });
            }

            PICUtil.invokePgCallback(rtndata);
        }

        window.close();
    }
});

Ext.define('PIC.GridSelectorPanelMixin', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.pic-gridselectorpanelmixin',

    mixins: ['PIC.SelectorPanelMixin'],

    beforeSelConfigInit: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.seltype = config.rtntype || $.getQueryString("seltype", me.seltype).toLowerCase();
        me.rtntype = config.rtntype || $.getQueryString("rtntype", me.rtntype).toLowerCase();
    },

    afterSelConfigInit: function (config) {
        var me = this;

        me.on("celldblclick", function (grid, td, cellIndex, rec, tr, rowIndex, e, eOpts) {
            me.doSelect([rec]);
        });

        me.addEvents('select');
    },

    doSelect: function (recs) {
        var me = this,
            rtnval = null;

        if (!recs) {
            recs = me.getSelection();
        }

        if (me.seltype == "multi") {
            rtnval = me.getSelValues(recs, me.rtntype);
        } else {
            if (recs && recs.length > 0) {
                rtnval = recs[0].getData();
            }
        }

        me.fireEvent('select', me, recs, rtnval);

        // 返回数据
        me.returnValue(rtnval);
    },

    getSelValues: function (recs, type) {
        var me = this;

        switch (type) {
            case "record":
                rtns = recs;
                break;
            case "array":
                rtns = me.getSelArrayValue(recs);
                break;
            case "json":
            case "string":
            default:
                rtns = me.getSelStringValue(recs);
                break;
        }

        return rtns;
    },

    getSelArrayValue: function (recs) {
        var me = this;
        var arr = [];

        if (recs && Ext.isArray(recs)) {
            Ext.each(recs, function (r) {
                arr.push(r.getData());
            });
        }

        return arr;
    },

    getSelStringValue: function (recs) {
        var me = this;
        var strjson = {};

        if (recs && Ext.isArray(recs)) {
            var data_arr = me.getSelArrayValue(recs);

            strjson = Ext.encode(data_arr);
        }

        return strjson;
    }
});

Ext.define('PIC.GridSelectorPanel', {
    extend: 'PIC.PageGridPanel',
    alias: 'widget.pic-gridselectorpanelmixin',
    mixins: ['PIC.GridSelectorPanelMixin'],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.beforeSelConfigInit(config);

        me.callParent([config]);

        me.afterSelConfigInit(config);
    }
});

Ext.define('PIC.data.ExtUserSelectStore', {
    extend: 'PIC.data.ExtSelectStore',
    alias: 'store.picuserselectstore',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            fields: [{ name: 'UserID' }, { name: 'Name' }, { name: 'WorkNo' }, { name: 'LoginName'}],
            proxy: {
                type: 'ajax',
                url: PICConfig.UserSelectPath,
                reader: { type: 'json', root: 'DtList', totalProperty: 'RecordCount' }
            }
        }, config);

        me.callParent([config]);

        me.on("change", function () {

        })
    }
});

Ext.define('PIC.ExtUserSelect', {
    extend: 'PIC.ExtGridSelect',
    alias: 'widget.picuserselect',
    gridCfg: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayField: 'Name',
            valueField: 'Name'
        }, config);

        if (!config.store) {
            config.store = Ext.create('PIC.data.ExtUserSelectStore');
        }

        config.gridCfg = Ext.apply({
            viewConfig: { loadMask: false },
            height: 200,
            width: 240,
            columns: [
                { text: '姓名', width: 80, dataIndex: 'Name' },
                { text: '工号', width: 80, dataIndex: 'WorkNo' }
            ]
        }, config.gridCfg || {});

        this.callParent([config]);
    }
});