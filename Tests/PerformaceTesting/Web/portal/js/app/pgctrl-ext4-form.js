
//------------------------PIC ExtJs 表单面板 开始------------------------//

PICFormData = {};
PICForm = null;

function ExtDummyPost(config) {
    if (!Ext.fly('frmDummy')) {
        var frm = document.createElement('form');
        frm.id = 'frmDummy';
        frm.name = config.name || 'frmDummy';
        frm.className = 'x-hidden';
        document.body.appendChild(frm);
    }

    config = Ext.apply({
        url: location.href,
        method: 'POST',
        form: Ext.fly('frmDummy')
    }, config || {});

    Ext.Ajax.request(config);
}

Ext.define('PIC.form.ExtFormPanelMixin', {
    extend: 'Ext.form.FormPanel',
    alias: 'widget.picformpanelmi',

    getAutoLayoutItems: function (items, cfg) {
        var me = this;
        var autoitems = [];
        var colcount = cfg.columns || me.columns || 2;
        var autoitems = [];

        if (items && $.isArray(items)) {
            // 开始自动布局
            var tautoitem;  // autoitem为表单域的容器
            for (var i = 0, pi = 0; i < items.length; i++) {
                var titem = items[i];

                if ((titem.isfield == false)) {
                    autoitems.push(titem);
                    continue;
                }

                var tfitem = { xtype: 'container', layout: 'anchor', flex: titem.flex, items: [titem] };

                var tflexcount = 0;
                if (tautoitem) {
                    // 计算当前autoitem的flex 
                    tflexcount = me.calcAutoItemTotalFlex(tautoitem);
                }

                // 如果当前域段度过大，则自动换行(即构建新的autoitem)
                if (i == 0 || (tflexcount + titem.flex) > pi) {
                    tautoitem = { xtype: 'container', layout: 'hbox', items: [] };
                    tautoitem.anchor = tautoitem.anchor || '-20';
                    autoitems.push(tautoitem);
                    pi = colcount;
                }

                tautoitem.items.push(tfitem);
            }

            $.each(autoitems, function () {
                tautoitem = this;

                if (tautoitem.isfield == false) {
                    return true;
                }

                var tflexcount = me.calcAutoItemTotalFlex(tautoitem);
                if (tautoitem && tflexcount < colcount) {
                    tautoitem.items.push({ unstyled: true, flex: (colcount - tflexcount) });    // 添加panel以维持对齐状态
                }
            });
        } else {
            autoitems = items;
        }

        return autoitems;
    },

    // 计算一个自动生成的HBox Item flex和
    calcAutoItemTotalFlex: function (autoitem) {
        var tflexcount = 0;
        $.each(autoitem.items, function () {
            var tflex = this.flex;
            if (tflex !== 0) { tflex = tflex || 1 };
            tflexcount += tflex;
        });

        return tflexcount;
    }
});

Ext.define('PIC.ExtFormPanel', {
    extend: 'PIC.BaseFormPanel',
    alias: 'widget.picformpanel',
    mixins: ['PIC.form.ExtFormPanelMixin'],

    openerCmp: null,

    constructor: function (config) {
        var me = this;

        me.openerCmp = PICUtil.getOpenerCmp();

        config = Ext.apply({
            region: 'center',
            autoHeight: true,
            autoScroll: true,
            border: false,
            frame: false,
            url: '#',
            labelWidth: 90,
            bodyStyle: 'padding: 5px 5px 0 15px',
            autolayout: true,
            fieldset: true,
            fieldsetTitle: null,
            frmdata: null
        }, config);

        if (config.tbar) {
            config.tbar.formpanel = me;
            config.tbar.xtype = (config.tbar.xtype || "picformtoolbar");
        }

        // 自动布局
        if (config.autolayout) {
            var colcount = config.columns || 2;
            var autoitems = [];
            if (config.items && $.isArray(config.items)) {
                // 预处理
                $.each(config.items, function () {
                    if (this.hidden == true) { this.flex = 0; }
                    if (this.flex !== 0) { this.flex = (this.flex || 1); }
                    if (this.flex > colcount) { this.flex = colcount; } // flex不应当大于colcount
                    this.pichandler = this.pichandler || config.pichandler;
                    this.xtype = this.xtype || "pictextfield";
                    this.labelWidth = this.labelWidth || config.labelWidth;
                    this.anchor = this.anchor || config.anchor || "-10";
                });

                config.items = me.getAutoLayoutItems(config.items, { columns: colcount });
            }
        }

        if (config.fieldset) {
            var _items = config.items;
            config.items = {
                xtype: 'fieldset',
                title: (config.fieldsetTitle || ""),
                padding: '10 10 10 10',
                items: _items
            }
        }

        me.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        window.PICForm = me;

        if (me.openerCmp && me.openerCmp.onforminit) {
            me.openerCmp.onforminit(me, window);
        }

        me.frmdata = window.PICFormData = (me.frmdata || PICState[PIC_FORM_DATA_KEY] || {});

        if (typeof (me.onforminit) == "function") {
            me.onforminit(me);
        }

        me.callParent(arguments);

        me.on('afterrender', function () {
            me.form.setValues(me.frmdata || {});

            if ('r'.equals(pgOp)) {
                me.setReadOnly(true);
            }

            if (typeof (me.onformload) == "function") {
                me.onformload();
            }
        });
    },

    setDisabled: function (b) {
        var me = this;
        var toolbars = me.getToolbars();
        Ext.each(toolbars, function (toolbar) { toolbar.setDisabled(b); });

        var fields = me.getForm().getFields().getRange();
        Ext.each(fields, function (f) {
            f.setDisabled(b);
        });
    },

    setReadOnly: function (b) {
        var me = this;
        var toolbars = me.getToolbars();
        Ext.each(toolbars, function (toolbar) { toolbar.setReadOnly(b); });

        var fields = me.getForm().getFields().getRange();

        Ext.each(fields, function (f) {
            f.setReadOnly(b);
//            var r_flag = true;

//            if (f.onRenderReadView) {
//                r_flag = f.onRenderReadView();
//            }

//            if (r_flag) {
//                var f_child = $(f.bodyEl.dom).children().first();
//                f_child.css("visibility", (b ? "hidden" : "visible"));

//                var text;
//                if (f.getRawValue) { text = f.getRawValue(); }
//                else text = f.getValue();

//                var f_view = f_child.find("span.pic-field-view");
//                if (b == true) {
//                    if (f_view.length > 0) {
//                        f_view.css("display", "");
//                    } else {
//                        f_child.before("<span class='pic-field-view'>" + text + "</span>");
//                    }
//                } else {
//                    if (f_view.length > 0) {
//                        f_view.css("display", "none");
//                    }
//                }
//            }
        });
    },

    getToolbars: function () {
        var toolbars = this.getDockedItems('toolbar[dock="top"]');
        return toolbars;
    },

    submit: function (action, data, params) {
        var me = this;

        // 未提供action时，错位调整
        if (action && typeof (action) != "string") {
            data = action;
            params = data;
            action = params.action || pgAction || pgOp || 'submit';
        } else {
            action = (action || pgAction || pgOp || 'submit');
        }

        if (typeof (me.onBeforeSubmit) === 'function') {
            if (me.onBeforeSubmit(action, data, params) === false) {
                return false;
            }
        }

        if (!me.form.isValid()) {
            return false;
        }

        params = Ext.apply({
            openercallback: true
        }, params);

        // 验证数据
        var frmdata = me.form.getValues();

        params.frmdata = Ext.apply(frmdata, (params.frmdata || {}));

        var funccols = {
            callback: params.callback || me.callback || null,
            onrequest: params.onrequest || me.onrequest || null,
            afterrequest: params.afterrequest || me.afterrequest || null,
            onfailure: params.onfailure || me.onfailure || null,
            afterfailure: params.afterfailure || me.afterfailure || null
        };

        params.callback = function (opts, success, resp) {
            var flag = true;

            me.form.clearInvalid();

            if (funccols.callback) { flag = funccols.callback(opts, success, resp); }

            return flag;
        }

        params.onrequest = function (respdata, opts) {
            var flag = true;
            if (funccols.onrequest) { flag = funccols.onrequest(me, respdata, opts); }

            return flag;
        }

        params.afterexpection = function (respdata, opts) {
            return false
        }

        params.afterrequest = function (respdata, opts) {
            var flag = true;

            if (funccols.afterrequest) { flag = funccols.afterrequest(me, respdata, opts); }

            if (respdata.isexception) {
                return false;
            }

            var callbackparams = {
                action: action,
                data: data || {},
                params: params,
                respdata: respdata,
                options: opts
            };

            if (flag !== false) {
                if (me.openerCmp && me.openerCmp.onformsubmit) {
                    flag = me.openerCmp.onformsubmit(callbackparams);
                }
            }

            if (flag !== false) {
                if (params.openercallback) {
                    PICUtil.invokeDgCallback(callbackparams, true);
                }
            }

            return flag;
        }

        params.onfailure = function (resp, opts) {
            var flag = true;
            if (funccols.onfailure) { flag = funccols.onfailure(me, respdata, opts); }

            return flag;
        }

        params.afterfailure = function (resp, opts) {
            var flag = true;
            if (funccols.afterfailure) { flag = funccols.afterfailure(me, respdata, opts); }

            return flag;
        }

        PICUtil.ajaxRequest(action, params, (data || {}));
    }
});

Ext.define('PIC.ExtFormToolbar', {
    extend: 'PIC.ExtToolbar',
    alias: 'widget.picformtoolbar',
    formpanel: null,
    constructor: function (config) {
        config = Ext.apply({}, config);

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});


Ext.define('PIC.ExtFormDescPanel', {
    extend: 'PIC.ExtPanel',
    alias: 'widget.picformdescpanel',
    constructor: function (config) {
        config = Ext.apply({
            id: 'form-desc',
            height: 80,
            margin: "0",
            border: false,
            frame: false,
            isfield: false,
            html: ""
        }, config);

        config.html = "<hr />" + config.html;

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});

Ext.define('PIC.ExtGridFormPanel', {
    extend: 'PIC.ExtFormPanel',
    alias: 'widget.picgridformpanel',
    grid: null,
    constructor: function (config) {
        var me = this;
        config = Ext.apply({
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIC.ExtFormTabPanel', {
    extend: 'PIC.ExtTabPanel',
    alias: 'widget.pic-formtabpanel',

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
        }, config);

        if (config.items) {
            Ext.each(config.items, function (item) {
                item.isTabItem = item.isTabItem || true;
            });
        }

        me.callParent([config]);
    },

    validateForms: function (frms) {
        var me = this;
        var flag = true;

        Ext.each(frms, function (frm) {
            flag = frm.isValid();

            if (!flag) {
                if (frm.isTabItem == true) {
                    me.setActiveTab(frm);
                } else {
                    // 向上找到tabItem
                    var upTab = frm.up("[isTabItem=true]");
                    if (upTab) {
                        me.setActiveTab(upTab);

                        return false;
                    }
                }

                return false;
            }
        });

        return flag;
    }
});

// 用户密码验证表单
Ext.define('PIC.ExtPwdVerifyForm', {
    extend: 'PIC.BaseFormPanel',
    alternateClassName: 'PIC.PwdVerifyForm',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            border: false,
            bodyPadding: 10,
            fieldDefaults: {
                labelAlign: 'top',
                labelWidth: 200
            }
        }, config);

        me.items = [{
            xtype: 'textfield',
            name: 'pwdField',
            itemId: 'pwdField',
            fieldLabel: '请输入验证密码',
            inputType: "password",
            allowBlank: false
        }];

        me.buttons = [{
            text: '确定',
            handler: function () {
                var _win = me.up("window");
                var val = me.getFieldValue("pwdField");

                me.fireEvent('submit', val, me);
                me.reset();
                _win.hide();
            }
        }, {
            text: '取消',
            handler: function () {
                var _win = me.up("window");

                me.fireEvent('cancel', me);
                me.reset();
                _win.hide();
            }
        }];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        me.addEvents(
            'submit',
            'cancel'
        );

        this.callParent(arguments);
    },

    statics: {
        Win: null,

        show: function (onSubmit, onCancel) {
            if (PIC.PwdVerifyForm.Win == null) {
                var form = Ext.create('PIC.ExtPwdVerifyForm', {
                    listeners: {
                        submit: function (val, frm) {
                            if (typeof onSubmit === 'function') {
                                onSubmit(val, frm);
                            }
                        },

                        cancel: function (frm) {
                            if (typeof onCancel === 'function') {
                                onCancel(val, frm);
                            }
                        }
                    }
                });

                PIC.PwdVerifyForm.Win = Ext.widget('window', {
                    title: '用户密码验证',
                    closeAction: 'hide',
                    width: 300,
                    height: 150,
                    layout: 'fit',
                    resizable: false,
                    modal: true,
                    items: form,
                    defaultFocus: 'pwdField'
                });
            }

            PIC.PwdVerifyForm.Win.show();
        }
    }
});

//------------------------PIC ExtJs 表单面板 结束------------------------//