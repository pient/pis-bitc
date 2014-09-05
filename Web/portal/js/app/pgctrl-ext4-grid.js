
//------------------------PIC ExtJs PageGridPanel 开始------------------------//

Ext.define('PIC.data.PageReader', {
    extend: 'Ext.data.JsonReader',
    alias: 'reader.picpage',
    alternateClassName: 'PIC.PageReader',

    root: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            idProperty: "Id",
            totalProperty: "total",
            root: "records"
        }, config);

        this.callParent([config]);
    },

    read: function (resp) {
        var me = this,
            data = resp.data || {},
            schcirt = 0,
            records = [],
            recordCount = 0,
            total = 0;

        var root = Ext.isArray(data) ? data : me.getRoot(data);
        if (root) {
            total = root.length;

            if (typeof (me.picbeforeread) === "function") {
                if (root) {
                    me.picbeforeread(root);
                }
            }
        }

        if (resp.data && resp.data.SearchCriterion) {
            schcirt = resp.data.SearchCriterion;
            total = schcirt["RecordCount"] || 0;
        }

        if (root) {
            records = me.extractData(root);
            recordCount = records.length;
        }

        return new Ext.data.ResultSet({
            total: total || recordCount || 0,
            count: recordCount || 0,
            records: records,
            success: true,
            message: null
        });
    }
});

Ext.define('PIC.PageProxy', {
    extend: 'PIC.data.ExtAjaxProxy',
    alias: 'proxy.picpage',
    start: 0,
    limit: 0,

    constructor: function (config) {
        config = Ext.apply({
        }, config);
        if (!config.reader) {
            config.reader = Ext.create("PIC.PageReader", config);
        }

        this.callParent([config]);
    },

    doRequest: function (operation, callback, scope) {
        var me = this;
        var params = operation.params || {};
        params.data = params.data || {};

        params.start = operation.start;
        params.limit = operation.limit;

        if (me.picbeforeload && me.picbeforeload(scope, params, operation.node, operation) === false) {
            return;
        }

        var opts = [];
        for (var key in params) {
            if (typeof (params[key]) == "string" || typeof (params[key]) == "number") {
                params["data"][key] = params[key];
            }
        }

        params.sorters = operation.sorters;

        if (params.schscope === "control") {
            // 默认页面级查询，若控件级查询不作处理
        } else {
            if (params.schtype == "field" && params.schdom || params.schtype == "group" && params.grpname) {
                if (params.schtype == "field") {
                    params.schcrit = getSchCriterion(params.schdom);
                } else if (params.schtype == "group") {
                    params.schcrit = getSchCriterionByGroup(params.grpname);
                }

                if (params.start == undefined) {
                    params.start = 1;
                    PICSearchCrit["CurrentPageIndex"] = 1;
                }
            }
        }

        params[PIC_QUERY_CRIT_KEY] = params[PIC_QUERY_CRIT_KEY] || PICSearchCrit || {};

        params = getRequestCriterion(params);

        if (params.start !== undefined && params.limit !== undefined) {
            me.start = params.start;
            me.limit = params.limit;
        }

        params.afterrequest = function (respData, opts) {
            var records = respData;
            var result = { status: "success", data: respData };
            var request = {};
            me.fireEvent("load", me, respData, opts);
            me.fireEvent("picload", me, respData, opts, result);

            me.processResponse(true, operation, request, result, callback, scope);
        }

        params.afterfailure = function (respData, opts) {
            me.fireEvent("loadexception", me, respData, opts);
        }

        params.url = me.url || params.url || null;

        PICDataAdapter.request(params);

        operation.setStarted();
    }
});


Ext.define('PIC.GridStoreMixin', {
    extend: 'Ext.data.Store'
});

Ext.define('PIC.PageGridStore', {
    extend: 'PIC.data.ExtJsonStore',
    alias: 'store.picpagegridstore',
    mixins: ['PIC.ExtExcelMixin'],

    beforeload: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            remoteGroup: true,
            idProperty: "Id",
            pageSize: PICSearchCrit["PageSize"],
            totalCount: PICSearchCrit["RecordCount"]
        }, config);

        config.root = (config.root || config.dsname || "EntList");

        if (!config.data) {
            var data = config.data || PICState[config.root] || [];
            if (config.picread) { config.picread.call(this, this, null, data) };

            config.data = PICState[config.root] || [];
        } else {
            if (config.picread) { config.picread.call(this, this, null, data) };
        }

        if (!config.proxy) {
            config.proxy = Ext.create("PIC.PageProxy", config);
        }

        me.beforeload = config.beforeload;
        me.picbeforeload = config.picbeforeload || me.picbeforeload;

        me.callParent([config]);
    },

    doSearch: function (cmp) {
        var me = this;
        var schcrit = me.schcrit || PICSearchCrit || {};
        var pgSize = schcrit["PageSize"] || 20;
        var params = null;

        if (cmp.schopts) {
            params = cmp.schopts.params;
        }

        if (!params) {
            params = {};
            if (typeof (cmp) == "string") {
                params["schtype"] = "group";
                params["grpname"] = cmp;
            } else {
                params["schtype"] = "field";
                params["schdom"] = cmp.el.dom;
            }
        }

        me.loadPage(schcrit['CurrentPageIndex'], { params: params });
    }
});

Ext.define('PIC.GridPagingField', {
    extend: 'PIC.ExtPagingField',
    alias: 'widget.picgridpagingfield',
    pgbar: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({}, config);

        me.pgbar = config.pgbar;
        config.value = config.pageSize || me.pgbar.pageSize;

        me.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('keyup', function (f, e) {
            if (e.keyCode == "13") { // 回车
                me.onPageSizeChange();
            }
        })
    },

    onPageSizeChange: function () {
        var me = this;
        var currPageSize = me.getValue();
        if (currPageSize > me.maxValue || currPageSize < me.minValue) {
            return;
        }

        if (me.pgbar) {
            var schcirt = me.pgbar.grid.schcirt || PICSearchCrit;
            if (schcirt) {
                schcirt['PageSize'] = currPageSize;
            }
            me.pgbar.store.pageSize = currPageSize;
            me.pgbar.store.loadPage(schcirt['CurrentPageIndex'], { start: 0, pageSize: me.pgbar.pageSize, limit: me.pgbar.pageSize });
        }
    }
});

Ext.define('PIC.toolbar.ExtGridPagingToolbar', {
    extend: 'PIC.toolbar.ExtPagingToolbar',
    alias: 'widget.picgridpagingtoolbar',
    alternateClassName: 'PIC.GridPagingToolbar',
    grid: null,
    store: null,
    pgfield: null,

    constructor: function (config) {
        var me = this;
        me.grid = config.grid;
        me.store = config.store;

        config = Ext.apply({}, config);

        if (!config.pgfield) {
            config.pgfield = Ext.create("PIC.GridPagingField", { pgbar: me, pageSize: config.pageSize || me.store.pageSize });
        }

        me.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});

Ext.define('PIC.PageGridSchToolbar', {
    extend: 'PIC.ExtToolbar',
    alias: 'widget.picpagegridschtoolbar',

    constructor: function (config) {
        this.callParent([config]);
    }
});

Ext.define('PIC.PageGridSchPanel', {
    extend: 'PIC.ExtFormPanel',
    alias: 'widget.picpagegridschpanel',
    grid: null,
    store: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            hideCollapseTool: true,
            labelWidth: 60,
            collapsed: false,
            picgrp: config.picgrp || 'defgrp'
        }, config);
        
        config.columns = config.columns || 3;
        me.grid = config.grid;
        me.store = config.store || config.grid.store || null;

        if (!me.tbar) {
            var schtbar = { xtype: 'picpagegridschtoolbar' };

            title = config.title || '<b class="x-grid-cquery-title">&nbsp;复杂查询框&nbsp;</b>';
            schtbar.items = [title, '->', '-'];
            schtbar.items.push({ xtype: 'picschbutton', handler: function () { me.doSearch(); } });
            schtbar.items.push('-');
            schtbar.items.push({ xtype: 'picclrbutton', text: '清除查询条件', handler: function () { me.doClear(); } });
            config.tbar = schtbar;
        }

        if (config.items && Ext.isArray(config.items)) {
            // 预处理
            Ext.each(config.items, function (it) {
                it.pichandler = it.pichandler || config.pichandler;
                if (it.schopts) {
                    it.schstore = it.schopts.schstore || config.store || it.schopts.store;    // 查询store
                    it.schopts.picgrp = it.schopts.picgrp || config.picgrp; // 查询组
                }

            });
        }

        me.callParent([config]);

        me.on('afterrender', function () {
            me.initSchItem(me);
        })
    },

    initSchItem: function (item) {
        var me = this;
        if (item.schopts) {
            item.on('specialkey', function (f, e) {
                if (e.getKey() == e.ENTER) {
                    me.store.doSearch(item);
                }
            }, item);

            if (item.el && item.el.dom) {
                $(item.el.dom).attr('qryopts', item.schopts.qryopts);
                if (item.schopts.picgrp) { $(item.el.dom).attr('picgrp', item.schopts.picgrp); }
            }
        } else {
            if (item.items) {
                var p = this;
                if ($.isArray(item.items)) {
                    $.each(item.items, function () {
                        p.initSchItem(this);
                    })
                } else if (item.items.items) {
                    if ($.isArray(item.items.items)) {
                        $.each(item.items.items, function () {
                            p.initSchItem(this);
                        })
                    }
                }
            }
        }
    },

    doSearch: function () {
        this.store.doSearch(this.picgrp);
    },

    doClear: function () {
        var me = this;
        var fields = me.form.getFields();
        fields.each(function (f) {
            if (!f.disabled && !f.readOnly && !f.hidden) {
                f.setValue("");
            }
        });
    },

    doCollapse: function () {
        if (me.grid.schpanel) {
            me.schpanel.toggleCollapse(true);
        }

        Ext.defer(function () {
            me.grid.doLayout();
        }, 100);
    }
});

Ext.define('PIC.PageGridButton', {
    extend: 'PIC.ExtButton',
    alias: 'widget.picpagegridbutton',
    bttype: null,
    grid: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({}, config);

        me.grid = config.grid;
        config.handler = config.handler || me.onbtnclick;

        me.bttype = config.bttype;
        this.callParent([config]);
    },

    onbtnclick: function () {
        var me = this;
        var grid = me.grid;

        switch (me.bttype) {
            case "add":
                grid.openFormWin("c");
                break;
            case "edit":
            case "view":
                var rec = grid.getFirstSelection();
                if (!rec) {
                    PICMsgBox.alert("请先选择要操作的记录！");
                    return;
                }

                if (me.bttype === "edit") {
                    grid.openFormWin("u", rec.getId());
                } else {
                    grid.openFormWin("r", rec.getId());
                }
                break;
            case "delete":
                var recs = grid.getSelection();
                if (!recs || recs.length <= 0) {
                    PICMsgBox.alert("请先选择要删除的记录！");
                    return;
                }

                PICMsgBox.confirm("确定删除所选记录？", function (btn) {
                    if ('yes'.equals(btn)) {
                        var params = {
                            afterrequest: function () {
                                grid.store.reload();
                            }
                        };

                        grid.batchOperate('batchdelete', recs, {}, params);
                    }
                });
                break;
            case "excel":
                var title = me.title || grid.exportTitle || grid.pictitle || grid.title;

                grid.exportExcel({ title: title });
                break;
            case "cquery":
                grid.toggleSchPanel();
                break;
            case "select":  // 选择面板时, 必须有doSelect方法
                var _recs = grid.getSelection();
                if (!_recs || _recs.length <= 0) {
                    PICMsgBox.alert("请先选择记录！");
                    return;
                }

                grid.doSelect(_recs);
                break;
        }
    }
});

// TwinTriggerFiled查询控件
Ext.define('PIC.PageGridSearchField', {
    extend: 'PIC.ExtSearchField',
    alias: 'widget.picpagegridsearchfield',
    store: null,
    columns: null,

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            picgrp: 'defgrp',
            pichandler: null
        }, config);

        me.grid = config.grid;
        me.store = config.store || config.grid.store;
        
        config.qryopts = config.qryopts || Ext.encode(me.grid.getQryOptions(config));

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('afterrender', function (f, e) {
            var field = this;

            if (field.el && field.el.dom) {
                $(field.el.dom).attr('qryopts', field.qryopts);
                if (field.picgrp) { $(field.el.dom).attr('picgrp', field.picgrp); }
            }

        }, this);
    },

    onTriggerClick: function () {
        var me = this;
        if (!me.pichandler) {
            me.store.doSearch(me);
        } else {
            me.pichandler.call(me, me.el.dom.value);
        }
    }
});

Ext.define('PIC.PageGridToolbar', {
    extend: 'PIC.ExtToolbar',
    alias: 'widget.picpagegridtoolbar',
    grid: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.grid = config.grid;

        if (config.items) {
            Ext.each(config.items, function (it, i, its) {
                if (!it) return true;
                var itemcfg = it;

                if (typeof (it) == "string") {
                    switch (it) {
                        case "add":
                        case "save":
                        case "edit":
                        case "view":
                        case "delete":
                        case "excel":
                        case "word":
                        case "cquery":
                        case "help":
                        case "select":
                            itemcfg = { xtype: 'picpagegridbutton', bttype: it, grid: me.grid };
                            break;
                        case "schfield":
                            itemcfg = { xtype: 'picpagegridsearchfield', columns: config.fldcols || [], schbutton: true, clrbutton: true, grid: me.grid };
                            break;
                    }
                } else {
                    if (!it.xtype) {
                        itemcfg.xtype = 'picpagegridbutton';
                        itemcfg.grid = itemcfg.grid || me.grid;
                    }
                }

                its[i] = itemcfg;
            });
        }

        this.callParent([config]);
    }
});

Ext.define('PIC.PageGridTitlePanel', {
    extend: 'PIC.ExtPanel',
    alias: 'widget.picpagegridtitlepanel',
    grid: null,
    schpanel: null,

    constructor: function (config) {
        config = Ext.apply({
            border: false
        }, config);
        config.tbar = config.tbar || config.tlbar;

        if (!config.items) {
            if (config.schpanel) {
                config.items = [config.schpanel];
            }
        }

        this.callParent([config]);
    }
});

Ext.define('PIC.GridPanelMixin', {
    extend: 'Ext.grid.GridPanel',
    alias: 'widget.pic-gridpanelmixin',

    beforeGridConfigInit: function (config) {
        var me = this;

        me.formparams = config.formparams || {};
        me.formparams.params = me.formparams.params || {};
    },

    afterGridConfigInit: function (config) {
        var me = this;

        me.onformsubmit = me.onformsubmit || function () { me.store.reload(); }

        // 注册dialog page callback
        //PICPgObserver.addListener('dgcallback', function (args) {
        //    me.onformsubmit(args);
        //});
    },

    initToolbar: function (config) {
        var me = this;

        if (!config.tbar) {
            if (!config.titbar && (config.tlbar || config.tlitems || config.schpanel || config.schitems)) {
                config.titbar = { xtype: 'picpagegridtitlepanel', grid: me };

                if (!config.tlbar && config.tlitems) {
                    config.tlbar = { xtype: 'picpagegridtoolbar', items: config.tlitems, fldcols: config.columns, grid: me }
                }

                config.titbar.tbar = config.tlbar;

                if (config.schpanel !== false) {
                    if (!config.schpanel) {
                        if (config.schitems) {
                            config.schpanel = { xtype: 'picpagegridschpanel', items: config.schitems, columns: config.schcols, grid: me }
                        }
                    }

                    if (config.schpanel) {
                        config.schpanel.store = config.schpanel.store || config.store;

                        if (config.schpanel) {
                            // 判断是否Ext组件
                            if ("Ext.Component" != config.schpanel.ctype) {
                                config.schpanel.hidden = config.schpanel.hidden || true;
                                config.schpanel.items = config.schpanel.items || config.schpanel.schitems;
                                config.schpanel.columns = config.schpanel.columns || config.schpanel.schcols;
                                config.titbar.schpanel = Ext.create('PIC.PageGridSchPanel', config.schpanel);
                            } else {
                                config.titbar.schpanel = config.schpanel;
                            }
                        }
                    } else {
                        config.titbar.schpanel = Ext.create('PIC.PageGridSchPanel', { hidden: true });
                    }
                }
            }

            config.tbar = config.titbar;
        }
    },

    processColumns: function (config) {
        var me = this;
        if (Ext.isArray(config.columns)) {
            Ext.each(config.columns, function (col, i, cols) {
                if (typeof (col.menuDisabled) === 'undefined') {
                    col.menuDisabled = true;
                }

                if (col.enumdata) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        return col.enumdata[val];
                    }
                } else if (col.formlink) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        var _params = me.getFormWinParams('r', rec.getId());

                        if (typeof col.formlink === "object") {
                            _params = Ext.apply(_params, col.formlink || {});
                        }

                        var rtn = PICUtil.renderFuncLink({ text: val, params: _params });

                        return rtn;
                    };
                } else if (col.filelink) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        var rtn = PICUtil.renderFileLink({ name: val, id: rec.getId() });

                        return rtn;
                    };
                } else if (col.btnparams) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        var params = Ext.apply({
                            recid: rec.internalId,
                            colid: col.id,
                            id: "btn_" + col.id,
                            width: "100%",
                            value: val,
                            text: val || col.header
                        }, col.btnparams);

                        var rtn = PICUtil.applyXTemplate('<button id={id} style="width: {width};" onclick="{handler}(\'{colid}\', \'{recid}\', \'{value}\')" class="pic-grid-button">{text}</button>', params);

                        return rtn;
                    };
                } else if (col.renderparams) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        return PICUtil.applyXTemplate(col.renderparams, { data: rec.getData(), params: col.renderparams.params, value: val });
                    }
                } else if (col.dateonly) {
                    col.renderer = col.renderer || function (val, p, rec) {
                        return $.dateOnly(val);
                    }
                }
            })
        }
    },

    openFormWin: function (cfg, id, args, params) {
        var me = this;

        if (typeof (me.beforeformload) == "function") {
            if (me.beforeformload(cfg, id, args, params) === false) {
                return;
            }
        }

        var _cfg = this.getFormWinParams(cfg, id, args, params);

        PICUtil.openDialog(_cfg);
    },

    getFormWinParams: function (cfg, id, args, params) {
        var me = this;
        var _params = {};
        var _cfg = {};

        cfg = cfg || {};
        var frmparams = Ext.apply({}, me.formparams || {});

        if (typeof (cfg) == "string") {
            _params.op = cfg;
            if (id) { _params.id = id; }
        } else {
            frmparams = Ext.apply(frmparams, cfg);

            Ext.apply(_cfg, cfg);
        }

        var _t_p = Ext.apply({}, me.formparams.params || {});
        _params = Ext.apply(_t_p, _params);

        Ext.apply(_params, args || {});
        Ext.apply(_params, cfg.params || {});

        Ext.apply(_cfg, frmparams || {});

        _cfg.params = _params;

        if (!_cfg.params[PIC_PG_OPENER_CMP_KEY]) {
            _cfg.params[PIC_PG_OPENER_CMP_KEY] = me.getId();
        }

        return _cfg;
    },

    batchOperate: function (action, recs, data, params) {
        params = params || {};
        params.url = params.url || null;

        data = data || {};

        var idlist = [],
            dtlist = [];

        if (recs != null) {
            Ext.each(recs, function (r) {
                idlist.push(r.getId());
                dtlist.push(r.getData());
            })
        }

        data["IdList"] = data["IdList"] || idlist;
        if (params.uploaddata == true) { data["DtList"] = (data["DtList"] || idlist); }

        PICUtil.ajaxRequest(action, params, data);
    },

    getQryOptions: function (config) {
        var schopts = config.schopts || { juncmode: 'Or' };
        var cols = config.fldcols || config.columns || [];

        if (!schopts.items) {
            schopts.items = [];
            $.each(cols, function (i) {
                var col = this;
                var jq = col.juncqry;

                if (typeof (jq) == "object") {
                    jq.field = jq.field || col.dataIndex;
                    schopts.items.push(jq);
                } else if (jq == true && col.dataIndex) {
                    schopts.items.push({ mode: 'Like', field: col.dataIndex });
                }
            });
        }

        return schopts;
    }
});


Ext.define('PIC.PageGridPanel', {
    extend: 'PIC.grid.ExtGridPanel',
    alias: 'widget.picpagegridpanel',

    mixins: ['PIC.GridPanelMixin'],

    beforeload: null,
    schcrit: null,
    titlepanel: null,
    schpanel: null,
    formparams: null,
    onformload: null,
    onformsubmit: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            border: false,
            columns: [],
            singleSelect: false,
            viewConfig: { loadMask: false }
        }, config);

        me.beforeGridConfigInit(config);

        me.schcrit = PICSearchCrit || {};

        me.processColumns(config); // 域处理columns

        me.initToolbar(config);

        if (!config.bbar) {
            if (!config.pgbar && config.pgbar != false) {
                config.pgbar = Ext.create('PIC.GridPagingToolbar', {
                    grid: me,
                    store: config.store
                });
                config.bbar = config.pgbar;
            }
        }

        me.callParent([config]);

        me.titlepanel = me.getDockedItem('picpagegridtitlepanel');
        if (me.titlepanel) {
            me.schpanel = me.titlepanel.schpanel;
        }

        me.afterGridConfigInit(config);
    },

    toggleSchPanel: function () {
        var me = this;

        if (me.schpanel) {
            me.schpanel.setVisible(me.schpanel.hidden);
        }
    }
});

//------------------------PIC ExtJs PageGridPanel 结束------------------------//


//------------------------PIC ExtJs TreeGridPanel 开始------------------------//


Ext.define('PIC.data.TreeGridReader', {
    extend: 'Ext.data.JsonReader',
    alias: 'reader.picpage',
    alternateClassName: 'PIC.TreeGridReader',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            idProperty: "Id"
        }, config);

        this.callParent([config]);
    },

    read: function (resp) {
        var me = this;
        var data = resp.data || {};
        var records = [];

        if (me.dsname) {
            data = data[me.dsname] || data;
        }

        if (me.isLeafProperty) {
            Ext.each(data, function (it) {
                it["leaf"] = it["leaf"] || (it[me.isLeafProperty] === true);
            });
        }

        if (data) {
            if (typeof (me.picbeforeread) === "function") {
                me.picbeforeread(data);
            }
        }

        var root = Ext.isArray(data) ? data : me.getRoot(data);
        if (root) { total = root.length; }

        if (root) {
            records = me.extractData(root);
        }

        return new Ext.data.ResultSet({
            records: records,
            success: true,
            message: null
        });
    }
});

Ext.define('PIC.data.TreeGridProxy', {
    extend: 'PIC.data.ExtAjaxProxy',
    alias: 'proxy.pic-treegridproxy',
    alternateClassName: 'PIC.TreeGridProxy',

    constructor: function (config) {
        config = Ext.apply({
        }, config);

        config.reader = config.reader || Ext.create("PIC.TreeGridReader", config);

        this.callParent([config]);
    },

    doRequest: function (operation, callback, scope) {
        var me = this;
        var params = operation.params || {};
        params.data = params.data || {};

        if (me.picbeforeload && me.picbeforeload(scope, params, operation.node, operation) === false) {
            return;
        }

        var opts = [];

        for (var key in params) {
            if (typeof (params[key]) == "string" || typeof (params[key]) == "number") {
                params["data"][key] = params[key];
            }
        }

        params.afterrequest = function (respData, opts) {
            var records = respData;
            var result = { status: "success", data: respData };
            var request = {};
            me.processResponse(true, operation, request, result, callback, scope);

            me.fireEvent("load", me, respData, opts);
            me.fireEvent("picload", me, respData, opts, result);
        }

        params.afterfailure = function (respData, opts) {
            me.fireEvent("loadexception", me, respData, opts);
        }

        params.url = me.url || params.url || null;

        PICDataAdapter.request(params);

        operation.setStarted();
    }
});

Ext.define('PIC.TreeGridStore', {
    extend: 'PIC.data.ExtTreeStore',
    alias: 'store.pic-treegridstore',
    mixins: ['PIC.ExtExcelMixin', 'PIC.data.ExtAjaxStoreMixin'],
    dsname: null,

    beforeload: null,

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            rootPathLevel: 0,
            parentIdProperty: 'ParentID',
            isLeafProperty: 'IsLeaf',
            pathLevelProperty: 'PathLevel',
            sortIndexProperty: 'SortIndex',
            iconProperty: 'Icon'
        }, config);

        if (!config.data && typeof (config.dsname) == "string") {
            config.data = PICState[config.dsname] || [];
        }

        if (typeof (config.picbeforeread) === "function") {
            if (config.data) {
                config.picbeforeread(config.data);
            }
        }

        me.beforeload = config.beforeload;
        me.picbeforeload = config.picbeforeload || me.picbeforeload;

        config.proxy = config.proxy || Ext.create("PIC.TreeGridProxy", config);

        me.callParent([config]);
    },

    load: function (options) {
        options = options || {};
        options.params = options.params || {};

        var me = this, node = options.node || me.tree.getRootNode(), root;

        // If there is not a node it means the user hasnt defined a rootnode
        // yet. In this case lets just
        // create one for them.
        if (!node) {
            node = me.setRootNode({
                expanded: true
            });
        }

        if (me.clearOnLoad) {
            node.removeAll(false);
        }

        Ext.applyIf(options, {
            node: node
        });

        options.params[me.nodeParam] = node ? node.getId() : 'root';

        if (node) {
            node.set('loading', true);
        }

        return me.callParent([options]);
    },

    getAllNodes: function (rnode, cnodes) {
        var me = this;
        rnode = rnode || me.getRootNode();
        cnodes = cnodes || [];
        var nodes = rnode.childNodes || [];
        $.merge(cnodes, nodes);

        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i];

            if (node.childNodes.length > 0) {
                me.getAllNodes(node, cnodes);
            }
        }

        return cnodes;
    }
});

Ext.define('PIC.TreeGridPanel', {
    extend: 'PIC.ExtTreePanel',
    alias: 'widget.pic-treegridpanel',

    mixins: ['PIC.GridPanelMixin'],

    cascadedchk: null,
    beforeload: null,
    formparams: null,

    onformsubmit: null,
    clipBoard: { records: [] },   // 剪切板

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            selType: 'checkboxmodel',
            multiSelect: true,
            border: false,
            rootVisible: false,
            columnLines: false,
            rowLines: true,
            icon: '',
            columns: [],
            viewConfig: { loadMask: false },
            downcascadedchk: false, // 向下级联选择
            downcascadedunchk: false, // 向下级联不选择
            upcascadedchk: false, // 向上级联选择
            upcascadedunchk: false // 向上级联不选择
        }, config);

        me.beforeGridConfigInit(config);

        me.processColumns(config); // 域处理columns

        config.schpanel = false;    // treepanel不现实SchPanel
        me.initToolbar(config);

        me.callParent([config]);

        me.addEvents('copy', 'cut', 'paste');

        me.afterGridConfigInit(config);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        if (me.downcascadedchk === true || me.upcascadedchk === true
            || me.downcascadedunchk === true || me.upcascadedunchk === true) {
            me.initCheckbox();
        }

        //if (me.store) {
        //    me.store.on("load", function () {
        //        var root = me.store.getRootNode();

        //        if (root.childNodes && root.childNodes.length == 1) {
        //            var firstNode = root.childNodes[0];
        //            if (!firstNode.isExpanded() && !firstNode.isLoaded()) {
        //                firstNode.expanded();
        //            }
        //        }
        //    })
        //}
    },

    initCheckbox: function () {
        var me = this;

        var nodep = function (node) {
            var allunchked = true;
            Ext.Array.each(node.childNodes, function (v) {
                if (v.data.checked === true) {
                    allunchked = false;
                    return;
                }
            });

            var checked = !allunchked;
            return checked;
        };

        var parentnode = function (node) {
            if (node.parentNode != null) {
                var checked = nodep(node.parentNode);

                if (node.parentNode.get('checked') != checked) {
                    node.parentNode.set('checked', checked);

                    me.fireEvent('checkchange', node.parentNode, checked);
                };

                // parentnode(node.parentNode);
            }
        };

        /*遍历子结点 选中 与取消选中操作*/
        var chd = function (node, checked) {
            node.set('checked', checked);
            if (node.isNode) {
                node.eachChild(function (child) {
                    chd(child, checked);
                });
            }
        };

        me.on('checkchange', function (node, checked) {
            node.set('checked', checked);

            if (me.downcascadedchk === true && checked === true
                || me.downcascadedunchk === true && checked === false) {
                node.expand();
                node.eachChild(function (child) {
                    child.set('checked', checked);
                    child.fireEvent('checkchange', child, checked);
                });

                node.eachChild(function (child) {
                    chd(child, checked);
                });
            }

            if (me.upcascadedchk === true && checked === true
                || me.upcascadedunchk === true && checked === false) {
                parentnode(node);
            }
        }, me);
    },

    getChecked: function () {
        var me = this;
        var chked = [];

        var allrecs = me.store.getAllNodes();

        Ext.each(allrecs, function (r) {
            if (r.get("checked") === true) {
                chked.push(r);
            }
        });

        return chked;
    },

    getUnchecked: function () {
        var me = this;
        var unchked = [];

        var allrecs = me.store.getAllNodes();

        Ext.each(allrecs, function (r) {
            if (!r.get("checked")) {
                unchked.push(r);
            }
        });

        return unchked;
    },

    resetChecked: function () {
        var me = this;
        me.getRootNode().cascadeBy(function () {
            this.set('checked', false);
        });
    },

    copy: function (recs) {
        // 复制
        var me = this;
        me.fireEvent('copy', me, recs);

        recs = recs || me.getSelection();
        me.clearClipBoard();

        if (recs) {
            me.clipBoard.type = 'copy';
            me.clipBoard.records = recs;

            Ext.each(recs, function (r) {
                me.addRowClass(r, 'x-grid3-row-copy');
            })
        }
    },

    cut: function (recs) {
        // 剪切
        var me = this;
        me.fireEvent('cut', me, recs);
        recs = recs || me.getSelection();

        me.clearClipBoard();

        if (recs) {
            me.clipBoard.type = 'cut';
            me.clipBoard.records = recs;

            Ext.each(recs, function (r) {
                me.addRowClass(r, 'x-grid3-row-cut');
            })
        }
    },

    paste: function (type, rec) {
        // 粘贴
        var me = this;
        type = type || 'sib';   // 默认粘贴为兄弟节点(选中节点之下)
        rec = rec || me.getFirstSelection();

        var store = me.store;
        if (!rec || !me.clipBoard.records || me.clipBoard.records.length <= 0) {
            return;
        }

        me.fireEvent('paste', me, type, rec, me.clipBoard);

        // 清空剪切板
        me.clearClipBoard();
    },

    clearClipBoard: function () {
        var me = this;
        var recs = me.clipBoard.records;

        if (recs && recs.length > 0) {
            Ext.each(recs, function (r) {
                me.removeRowClass(r, 'x-grid3-row-copy');
                me.removeRowClass(r, 'x-grid3-row-cut');
            });

            me.clipBoard.type = null;
            me.clipBoard.records = [];
        }
    },

    addRowClass: function (nodeInfo, cls) {
        var node = this.getView().getNode(nodeInfo);
        if (node) {
            $(node).addClass(cls);
        }
    },

    removeRowClass: function (nodeInfo, cls) {
        var node = this.getView().getNode(nodeInfo);
        if (node) {
            $(node).removeClass(cls);
        }
    }
});


//------------------------PIC ExtJs TreeGridPanel 结束------------------------//
