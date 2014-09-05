/*
* pgctrl对ext的扩展
* @author Ray Liu
*/

//-----------------------------PIC ExtJs 事件 开始--------------------------//

Ext.define('PIC.ExtObservable', {
    extend: 'Ext.util.Observable',

    constructor: function (config) {
        config = Ext.apply({}, config);
        this.callParent([config]);
    }
});

Ext.define('PIC.ExtPageObservable', {
    extend: 'PIC.ExtObservable',

    constructor: function (config) {
        config = Ext.apply({}, config);
        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    },

    onPgInit: function () { PICPgObserver.fireEvent('pginit'); },
    onFrmLoad: function () { PICPgObserver.fireEvent('frmload'); },
    onPgLoad: function () { PICPgObserver.fireEvent('pgload'); },
    onPgTimer: function () { PICPgObserver.fireEvent('pgtimer'); },
    onPgUnload: function () { PICPgObserver.fireEvent('pgunload'); },
    onDgCallback: function (args) { PICPgObserver.fireEvent('dgcallback', args); },

    // 过时，注册组件，用于打开页面的回调
    registerCmpDgCallback: function (args, callback) {
        callback = callback || args.callback;

        if (typeof (callback) === 'function') {
            PICPgObserver.addListener('dgcallback', function (rtnArgs) {
                if (args && args.cmpid) {
                    if (rtnArgs && rtnArgs.cmpid && args.cmpid === rtnArgs.cmpid) {
                        if (!args.opcode || args.opcode && args.opcode === rtnArgs.opcode) {
                            callback(rtnArgs);
                        }
                    }
                }
            });
        }
    }
}, function () {
    PICPgObserver = PICPageObserver = new this();

    PICPgObserver.addEvents('pginit', 'frmload', 'pgload', 'pgtimer', 'pgunload', 'dgcallback');
});

//-----------------------------PIC ExtJs 事件 结束--------------------------//


//------------------------PIC ExtJs数据组件扩展 开始------------------------//

Ext.define('PIC.data.ExtJsonReader', {
    extend: 'Ext.data.JsonReader',
    alias: 'reader.picjson',
    alternateClassName: ['PIC.ExtJsonReader'],
    dsname: null,
    root: null,
    picbeforeread: null,
    picread: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({ }, config);
        me.picbeforeread = config.picbeforeread || null;
        me.picread = config.picread || null;

        this.callParent([config]);
    }
});

Ext.define('PIC.data.ExtJsonWriter', {
    extend: 'Ext.data.JsonWriter',
    alias: 'writer.picjson',

    constructor: function (config) {
        config = Ext.apply({}, config);

        this.callParent([config]);
    }
});

Ext.define('PIC.data.ExtProxy', {
    extend: 'Ext.data.DataProxy',
    alias: 'proxy.picproxy',
    alternateClassName: ['PIC.data.ExtJsonProxy', 'PIC.ExtJsonProxy'],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            reader: 'picjson',
            writer: 'picjson'
        }, config);

        this.callParent([config]);
    },

    read: function (params, reader, callback, scope, arg) {
        this.callParent(arguments);
    }
});

Ext.define('PIC.data.ExtAjaxProxy', {
    extend: 'Ext.data.proxy.Ajax',
    alias: 'proxy.picajaxproxy',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        this.callParent([config]);
    },

    doRequest: function(operation, callback, scope){
        this.callParent(arguments);
    }
});

Ext.define('PIC.data.ExtStoreMixin', {
    extend: 'Ext.data.Store',
    alias: 'store.picstoremi',
    mixins: ['PIC.ExtExcelMixin'],

    createModel: function (record) {
        if (!record.isModel) {
            var id = null;
            if (this.idProperty) {
                id = record[this.idProperty];
            }
            record = Ext.ModelManager.create(record, this.model, id);
        }

        return record;
    },

    getRangeData: function () {
        var recs = this.getRange();
        var data = [];

        Ext.each(recs, function (rec) {
            data.push(rec.data);
        });

        return data;
    },

    getField: function (f) {
        var fields = this.model.getFields();
        var field = null;

        if (typeof (f) == "string") {
            Ext.each(fields, function (tf) {
                if (tf.name = f) {
                    field = tf;
                    return false;
                }
            });
        } else if (typeof (f) == "number") {
            field = fields[f];
        }

        return field;
    },

    exportExcel: function (config) {
        var me = this;
        config = config || {};

        if (!config.columns) {
            var cols = [];

            var flds = me.model.getFields();
            Ext.each(flds, function (f) {
                var col = Ext.apply({
                    hidden: false,
                    dataIndex: f.name,
                    width: 100,
                    text: f.name
                }, f);

                cols.push(col);
            });

            config.columns = cols;
        }

        config.store = me;

        me.mixins['PIC.ExtExcelMixin'].exportExcel(config);
    }
});

Ext.define('PIC.data.ExtAjaxStoreMixin', {
    extend: 'Ext.data.Store',
    alias: 'store.picajaxstoremi',

    parseLoadArgs: function (args) {
        var _args = { params: { data: {}} };

        if (args) {
            if (!args.params) {
                _args.params.data = args.data || args;
            } else {
                _args.params = args.params;
            }
        }

        return _args;
    }
});

// 修复extjs grouping选择错位bug
Ext.override(Ext.data.Store, {
    fireEvent: function (eventName, store) {
        if (store && store.isStore && eventName === "datachanged") {
            this.sortGroupedStore();
        }

        return this.callParent(arguments);
    },

    sortGroupedStore: function () {
        if (this.isGrouped()) {
            var me = this,
                collection = me.data,
                items = [],
                keys = [],
                groups, length, children, lengthChildren,
                i, j;

            groups = me.getGroups();
            length = groups.length;

            for (i = 0; i < length; i++) {
                children = groups[i].children;
                lengthChildren = children.length;

                for (j = 0; j < lengthChildren; j++) {
                    items.push(children[j]);
                    keys.push(children[j].internalId);
                }
            }

            collection.items = items;
            collection.keys = keys;

            collection.fireEvent('sort', collection, items, keys);
        }
    }
});

Ext.define('PIC.data.ExtStore', {
    extend: 'Ext.data.Store',
    alias: 'store.picstore',
    mixins: ['PIC.data.ExtStoreMixin'],

    constructor: function (config) {
        config = Ext.apply({}, config);
        this.callParent([config]);
    },

    reload: function (args) {
        this.callParent([args]);
    }
});

Ext.define('PIC.data.ExtJsonStore', {
    extend: 'Ext.data.Store',
    alias: 'store.picjson',
    mixins: ['PIC.data.ExtStoreMixin', 'PIC.data.ExtAjaxStoreMixin'],

    constructor: function (config) {
        config = Ext.apply({
            root: 'records',
            idProperty: 'Id',
            totalProperty: 'total',
            remoteSort: true,
            remoteGroup: true,
            autoDestroy: true,
            proxy: { type: 'picproxy' }
        }, config);

        config.root = (config.root || config.dsname || "EntList");

        config.proxy.reader = config.proxy.reader || config.reader || Ext.create('PIC.data.ExtJsonReader', config);
        config.proxy.writer = config.proxy.writer || config.writer || Ext.create('PIC.data.ExtJsonWriter', config);

        this.callParent([config]);
    },

    reload: function (args) {
        var args = this.parseLoadArgs(args);

        this.callParent([args]);
    }
});

Ext.define('PIC.data.ExtArrayStore', {
    extend: 'Ext.data.ArrayStore',
    alias: 'store.picarray',
    mixins: ['PIC.data.ExtStoreMixin'],

    constructor: function (config) {
        this.callParent([config]);
    },

    reload: function (args) {
        this.callParent([args]);
    }
});

Ext.define('PIC.data.ExtEnumStore', {
    extend: 'PIC.data.ExtArrayStore',
    alias: 'store.picenum',
    valtype: 'string',    // int, string, float等

    constructor: function (config) {
        var me = this;

        config = Ext.apply({ enumdata: {}
        }, config);

        me.valtype = config.valtype || "string";

        var tdata = config.enumdata;

        if (config.enumdata) {
            switch (config.enumtype) {
                case 'simple':
                default:
                    config.fields = config.fields || ['value', 'text'];
                    var tdata = [];
                    for (var key in config.enumdata) {
                        var keyval = me.getKeyVal(key);
                        tdata[tdata.length] = [keyval, config.enumdata[key]];
                    }
                    config.data = tdata;
                    break;
            }
        }

        this.callParent([config]);
    },

    getKeyVal: function (key) {
        var me = this;

        var keyVal = key;
        switch (me.valtype) {
            case "int":
                keyVal = parseInt(key);
                break;
            case "float":
                keyVal = parseFloat(key);
                break;
        }

        return keyVal;
    }
});

//------------------------PIC ExtJs数据组件扩展 结束------------------------//

//-----------------------------PIC ExtJs Tip 开始--------------------------//

Ext.define('PIC.ExtQuickTip', {
    extend: 'Ext.QuickTip',
    alias: 'widget.picquicktip',

    constructor: function (config) {
        config = Ext.apply({}, config);

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    }
});

//-----------------------------PIC ExtJs Tip 结束--------------------------//

//-----------------------------PIC ExtJs 按钮 开始--------------------------//

Ext.define('PIC.ExtButton', {
    extend: 'Ext.Button',
    alias: 'widget.picbutton'
}, function () {
    PICUtil.definePICButton('ExtAddButton', 'picaddbutton', 'add', { picexecutable: true, text: '添加', iconCls: 'pic-icon-add' });
    PICUtil.definePICButton('ExtSaveButton', 'picsavebutton', 'save', { picexecutable: true, text: '保存', iconCls: 'pic-icon-save' });
    PICUtil.definePICButton('ExtSubmitButton', 'picsubmitbutton', 'submit', { picexecutable: true, text: '提交', iconCls: 'pic-icon-submit' });
    PICUtil.definePICButton('ExtEditButton', 'piceditbutton', 'edit', { picexecutable: true, text: '编辑', iconCls: 'pic-icon-edit' });
    PICUtil.definePICButton('ExtDeleteButton', 'picdeletebutton', 'delete', { picexecutable: true, text: '删除', iconCls: 'pic-icon-delete' });
    PICUtil.definePICButton('ExtExecButton', 'picexecbutton', 'exec', { picexecutable: true, text: '执行', iconCls: 'pic-icon-exec' });
    PICUtil.definePICButton('ExtSelButton', 'picselbutton', 'select', { text: '选择', iconCls: 'pic-icon-select' });
    PICUtil.definePICButton('ExtViewButton', 'picviewbutton', 'view', { text: '查看', iconCls: 'pic-icon-view' });
    PICUtil.definePICButton('ExtCancelButton', 'piccancelbutton', 'cancel', { text: '取消', iconCls: 'pic-icon-cancel', handler: function () { window.top.close(); } });
    PICUtil.definePICButton('ExtCloseButton', 'picclosebutton', 'close', { text: '关闭', iconCls: 'pic-icon-close', handler: function () { window.top.close(); } });
    PICUtil.definePICButton('ExtHelpButton', 'pichelpbutton', 'help', { text: '帮助', iconCls: 'pic-icon-help', handler: function () { PICUtil.openHelpDialog(); } });
    PICUtil.definePICButton('ExtMoreButton', 'picmorebutton', 'more', { text: '更多', iconCls: 'pic-icon-more' });
    PICUtil.definePICButton('ExtExcelButton', 'picexcelbutton', 'excel', { text: '导出Excel', iconCls: 'pic-icon-xls' });
    PICUtil.definePICButton('ExtWordButton', 'picwordbutton', 'word', { text: '导出Word', iconCls: 'pic-icon-doc' });
    PICUtil.definePICButton('ExtSchButton', 'picschbutton', 'search', { text: '查询', iconCls: 'pic-icon-search' });
    PICUtil.definePICButton('ExtCQryButton', 'piccqrybutton', 'cquery', { text: '复杂查询', iconCls: 'pic-icon-tog-bottom' });
    PICUtil.definePICButton('ExtClrButton', 'picclrbutton', 'clear', { text: '清除', iconCls: 'pic-icon-erase' });
});


//-----------------------------PIC ExtJs 按钮 结束--------------------------//

//---------------------------PIC ExtJs 工具栏 开始--------------------------//

Ext.define('PIC.ExtToolbar', {
    extend: 'Ext.Toolbar',
    alias: 'widget.pictoolbar',

    constructor: function (config) {
        config = Ext.apply({}, config);

        if (config.items) {
            for (var i = 0; i < config.items.length; i++) {
                var item = config.items[i];
                var itemcfg = item;
                var iteminfo;
                if (typeof (item) == "string") {
                    iteminfo = PICUtil.getPICButtonInfoByBtType(item.toLowerCase());
                    if (iteminfo) { itemcfg = iteminfo }
                } else if (typeof (item) == "object" && typeof (item.bttype) == "string") {
                    iteminfo = PICUtil.getPICButtonInfoByBtType(item.bttype.toLowerCase());
                    if (iteminfo) {
                        itemcfg = Ext.apply(iteminfo, itemcfg);
                    }
                }

                config.items[i] = itemcfg;
            }
        }

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('afterrender', function () {
            if ('r'.equals(pgOp)) {
                me.setReadOnly(true);
            }
        })
    },

    setDisabled: function (flag) {
        var me = this;
        me.setStatus({ disabled: flag, visible: true });
    },

    setReadOnly: function (flag) {
        var me = this;
        me.setStatus({ disabled: false, visible: !flag });
    },

    setStatus: function (params, menu) {
        var me = this;
        params = params || {};
        var menu = params.menu;
        params.readOnly = (params.readOnly == true);
        params.disabled = (params.disabled == true);
        params.visible = !(params.readOnly == false);

        if (!menu) {
            me.items.each(function () {
                me.setMenuItemStatus(this, params);

                if (this.menu) {
                    me.setStatus(params, this.menu);
                }
            });
        } else if (menu && menu.items) {
            menu.items.each(function () {
                me.setMenuItemStatus(this, params);
                if (this.menu) { me.setReadOnly(params, this.menu); }
            });
        }
    },

    setMenuItemStatus: function (menuitem, params) {
        if (menuitem && menuitem.picautoset !== false && menuitem.picexecutable === true) {
            if (menuitem.setVisible) { menuitem.setVisible(params.visible); }
            if (menuitem.setDisabled) { menuitem.setDisabled(params.disabled); }
            if (menuitem.setReadOnly) { menuitem.setDisabled(params.readOnly); }
        }
    },

    getButtons: function (params) {
        var btns = PICUtil.findButtons(this, params);

        return btns;
    },

    getButton: function (params) {
        var btns = this.getButtons(params);

        if (btns && btns.length > 0) {
            return btns[0];
        }

        return null;
    }
});

//---------------------------PIC ExtJs 工具栏 结束--------------------------//

//---------------------------PIC ExtJs 蒙版 开始--------------------------//

Ext.define('PIC.ExtLoadMask', {
    extend: 'Ext.LoadMask',
    alias: ['widget.picmask', 'widget.picloadmask'],
    alternateClassName: ['PIC.ExtMask', 'PICMask'],

    constructor: function (config) {
        this.callParent([config]);
    },

    statics: {
        loadBodyMask: function (msg) {
            if (typeof PICBodyMask == "undefined") {
                PICBodyMask = new PIC.ExtLoadMask({
                    target: Ext.getBody(),
                    msg: "请稍候..."
                });
            }

            if (msg === false) {
                PICBodyMask.hide();

                return;
            }

            if (msg) {
                PICBodyMask.update(msg);
            }

            PICBodyMask.show();
        },

        getMask: function (target, msg) {
            var params = {};

            if (target === false) {
                this.hide();

                return;
            } else {
                if (typeof (target) == "string" && !msg) {
                    msg = target;
                    target = Ext.getBody();
                }
            }

            if (typeof (target) != "object") {
                target = Ext.getBody();
            }

            var mask = new PIC.ExtLoadMask(target, msg);

            return mask;
        }
    }
});

//---------------------------PIC ExtJs 蒙版 结束--------------------------//

//-----------------------------PIC ExtJs MessageBox 开始--------------------------//


Ext.define('PIC.ExtMessageBox', {
    extend: 'Ext.window.MessageBox',
    alias: ['widget.picmessagebox', 'widget.picmsgbox'],
    alternateClassName: ['PIC.ExtMsgBox'],

    constructor: function (config) {
        this.callParent([config]);
    },

    showAlert: function (msg, title, fn, scope, icon) {
        Ext.MessageBox.show({
            title: title,
            msg: msg,
            fn: fn,
            buttons: Ext.Msg.OK,
            icon: icon || null
        });
    },

    confirm: function (msg, fn, cfg, scope) {
        cfg = cfg || "确认";
        Ext.MessageBox.confirm(cfg, msg, fn, scope);
    },

    show: function (cfg) {
        Ext.MessageBox.show(cfg)
    },

    alert: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "提示", fn, scope);
    },

    info: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "消息", fn, scope, Ext.Msg.INFO);
    },

    warn: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "警告", fn, scope, Ext.Msg.WARNING);
    },

    error: function (msg, title, fn, scope) {
        this.showAlert(msg, title || "错误", fn, scope, Ext.Msg.ERROR);
    },

    wait: function (progressText, msg) {
        Ext.MessageBox.show({
            msg: msg || "正在处理数据，请等待...",
            progressText: progressText || "处理中...",
            width: 300,
            wait: true,
            waitConfig: { interval: 200 }
        });
    },

    prompt: function (msg, title, fn, scope, value, multiline) {
        Ext.MessageBox.prompt(title || "提示", msg, fn, scope, multiline, value);
    },

    hide: function () { try { Ext.MessageBox.hide(); } catch (e) { } }
},

function () {
    PICExtMsgBox = PICExtMessageBox = new this();
    if (typeof PICMsgBox == "undefined") {
        PICMsgBox = PICExtMsgBox;
    }
});

//-----------------------------PIC ExtJs MessageBox 结束--------------------------//


//-----------------------------PIC ExtJs 表单域 开始--------------------------//

Ext.define('PIC.form.ExtFieldBase', {
    extend: 'Ext.form.field.Base',

    alternateClassName: 'PIC.ExtFieldBase',

    requiredTag: '<span style="color:red">*</span>',

    statics: {
        requiredTag: '<span style="color:red">*</span>',

        initRequiredTag: function (f) {
            if (typeof (f.allowBlank) !== 'undefined' && !f.allowBlank) {
                if (f.fieldLabel) {
                    f.fieldLabel += f.requiredTag || PIC.ExtFieldBase.requiredTag;
                }
            }
        }
    }
});

//为form表单中必填项添加红色*号标志
Ext.override(Ext.form.field.Base, { //针对form中的基本组件
    initComponent: function () {
        PIC.ExtFieldBase.initRequiredTag(this);

        this.callParent(arguments);
    },

    setAllowBlank: function (flag) {
        this.fieldLabel = this.fieldLabel.replace(requiredTag, '');
        this.allowBlank = flag;

        if (flag === true) {
            this.fieldLabel += this.requiredTag;
        }
    }
});

Ext.define('PIC.ExtTextField', {
    extend: 'Ext.form.TextField',
    alias: ['widget.pictextfield', 'widget.picfield'],

    constructor: function (config) {
        this.callParent([config]);
    }
});

Ext.define('PIC.ExtNumberField', {
    extend: 'Ext.form.NumberField',
    alias: ['widget.picnumberfield'],

    constructor: function (config) {
        this.callParent([config]);
    }
});

Ext.define('PIC.ExtTextArea', {
    extend: 'Ext.form.TextArea',
    alias: 'widget.pictextarea',

    constructor: function (config) {
        this.callParent([config]);
    }
});

Ext.define('PIC.ExtJsonArea', {
    extend: 'Ext.form.TextArea',
    alias: 'widget.picjsonarea',

    constructor: function (config) {
        this.callParent([config]);
    },

    getValue: function (isformat) {
        var v = this.callParent([]);

        if (this.isformat == true || isformat == true) {
            v = $.formatJsonString(v);
        }

        return v;
    },

    setValue: function (value, isformat) {
        var v = value;

        if (!v) { return; }

        if (this.isformat == true || isformat == true) {
            v = $.formatJsonString(v);
        }

        this.callParent([v]);
    },

    formatValue: function () {
        var v = this.getValue();

        this.setValue(v, true);
    }
});

Ext.define('PIC.ExtHtmlEditor', {
    extend: 'Ext.form.HtmlEditor',
    alias: 'widget.pichtmleditor',

    constructor: function (config) {
        this.callParent([config]);
    },

    initComponent: function () {
        PIC.ExtFieldBase.initRequiredTag(this);

        this.callParent(arguments);
    },

    getErrors: function (value) {
        var me = this,
            errors = [],
            validator = me.validator,
            allowBlank = me.allowBlank,
            vtype = me.vtype,
            vtypes = Ext.form.field.VTypes,
            msg;

        var value = me.getValue();

        if (Ext.isFunction(validator)) {
            msg = validator.call(me, value);
            if (msg !== true) {
                errors.push(msg);
            }
        }

        if (value.length < 1) {
            if (!allowBlank) {
                errors.push(me.blankText);
            }

            return errors;
        }

        if (vtype) {
            if (!vtypes[vtype](value, me)) {
                errors.push(me.vtypeText || vtypes[vtype + 'Text']);
            }
        }

        return errors;
    }
});

Ext.define('PIC.ExtNumberField', {
    extend: 'Ext.form.NumberField',
    alias: 'widget.picnumberfield',

    constructor: function (config) {
        this.callParent([config]);
    }
});

Ext.define('PIC.ExtDateField', {
    extend: 'Ext.form.DateField',
    alias: 'widget.picdatefield',

    constructor: function (config) {
        config = Ext.apply({
            format: 'Y-m-d'
        }, config);

        this.callParent([config]);
    },

    setValue: function (value, doSelect) {
        var v = value;

        if (!v) {
            return;
        }

        if (typeof (value) == "string") {
            v = $.toDate(value);
            // v = new Date(value);
        }

        this.callParent([v, doSelect]);
    }
});

// TwinTriggerField查询控件
Ext.define('PIC.ExtSearchField', {
    extend: 'Ext.form.TwinTriggerField',
    alias: 'widget.picsearchfield',
    store: null,
    columns: null,

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            fieldLabel: '查询 ',
            hideLabel: false,
            labelWidth: 40,
            clearCls: 'x-form-clear-trigger',
            triggerCls: 'x-form-search-trigger',
            hideTrigger: false
        }, config);

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);

        me.on('specialkey', function (f, e) {
            if (e.getKey() == e.ENTER) {
                this.onTriggerClick();
            }
        }, me);
    },

    onTriggerClick: function () {
        var me = this;

        me.doSearch();
    },

    doSearch: function () {
        var me = this;

        if (typeof (me.onsearch) == "function") {
            var val = me.getValue();

            me.onsearch(me, val);
        }
    }
});

Ext.define('PIC.ExtCheckbox', {
    extend: 'Ext.form.Checkbox',
    alias: 'widget.piccheckbox',

    constructor: function (config) {
        config = Ext.apply({
        }, config);

        this.callParent([config]);
    }
}); 

Ext.define('PIC.ExtCheckboxGroup', {
    extend: 'Ext.form.CheckboxGroup',
    alias: 'widget.piccheckboxgroup',

    constructor: function (config) {
        config = Ext.apply({
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIC.ExtRadioGroup', {
    extend: 'Ext.form.RadioGroup',
    alias: 'widget.picradiogroup',

    constructor: function (config) {
        config = Ext.apply({
        }, config);

        this.callParent([config]);
    }
});

//------------------------PIC ExtJs Picker类控件 开始------------------------//

Ext.define('PIC.ExtComboBox', {
    extend: 'Ext.form.ComboBox',
    alias: 'widget.piccombobox',

    constructor: function (config) {
        config = Ext.apply({
            editable: false,
            displayField: 'text',
            valueField: 'value',
            triggerAction: 'all',
            selectOnFocus: true,
            blankText: "请选择...",
            mode: 'local',
            value: '',
            required: true
        }, config);

        this.callParent([config]);
    },

    setValue: function (value, doSelect) {
        var v = value;

        this.callParent([v, doSelect]);
    }
});

Ext.define('PIC.ExtEnumSelect', {
    extend: 'PIC.ExtComboBox',
    alias: 'widget.picenumselect',

    constructor: function (config) {
        config.required = (config.allowBlank == false);

        if (!config.required && config.enumdata) {
            var tenumdata = { '': config.blankText || '--请选择--' };

            for (var key in config.enumdata) {
                tenumdata[key] = config.enumdata[key];
            }

            config.enumdata = tenumdata;
        }

        if (!config.store && config.enumdata) {
            config.store = Ext.create('PIC.data.ExtEnumStore', {
                valtype: config.enumvaltype,
                enumtype: config.enumtype,
                enumdata: config.enumdata 
            });
        }

        this.callParent([config]);
    }
});

Ext.define('PIC.ExtGridComboBoxList', {
    extend: 'Ext.view.AbstractView',
    alias: 'widget.picgridcombolist',
    alternateClassName: 'PIC.view.ExtGridComboBoxList',
    renderTpl: ['<div class="list-ct" style="border: 1px solid #99BBE8"></div>'],

    initComponent: function () {
        var me = this;
        // 2012-05-07 Ext4.1下的Bug的解决
        me.itemSelector = "div.list-ct";
        //me.itemSelector = ".";
        me.tpl = Ext.create('Ext.XTemplate');

        me.callParent();

        Ext.applyIf(me.renderSelectors, {
            listEl: '.list-ct'
        });

        me.gridCfg.border = false;
        me.gridCfg.store = me.store;
        me.gridCfg.gridType = me.gridCfg.gridType || 'Ext.grid.Panel';

        me.grid = Ext.create(me.gridCfg.gridType, me.gridCfg);

        me.grid.store.addListener({
            beforeload: function () {
                me.owner.loading = true;
            },
            load: function () {
                me.owner.loading = false;
            }
        });

        var sm = me.grid.getSelectionModel();

        sm.addListener('selectionchange', function (a, sl) {
            var cbx = me.owner;
            if (cbx.loading)
                return;
            // var sv = cbx.multiSelect ? cbx.getValue() : {};
            var sv = {}
            var EA = Ext.Array, vf = cbx.valueField;

            if (!sl || !sl.length && !sl[vf]) {
                return
            }

            // al = [ 'G', 'Y', 'B' ]
            var items = me.grid.store.getRange();
            var al = EA.map(items, function (r) {
                return r.data[vf];
            });
            /*var al = EA.map(me.grid.store.data.items, function (r) {
                return r.data[vf];
            });*/
            var cs = EA.map(sl, function (r) {
                var d = r.data;
                if (d) {
                    var k = d[vf];
                    sv[k] = d;
                    return k;
                }
            });

            // cs = [ 'G' ]
            var rl = EA.difference(al, cs);
            EA.each(rl, function (r) {
                delete sv[r];
            });
            cbx.setValue(sv);
        });

        sm.addListener('select', function (m, r, i) {
            var cbx = me.owner;
            if (cbx.loading)
                return;
            if (!cbx.multiSelect)
                cbx.collapse();
        });
    },

    onRender: function () {
        this.callParent(arguments);
        this.grid.render(this.listEl);
    },

    bindStore: function (store, initial) {
        this.callParent(arguments);
        if (this.grid)
            this.grid.bindStore(store, initial);
    },

    onDestroy: function () {
        Ext.destroyMembers(this, 'grid', 'listEl');
        this.callParent();
    },

    highlightItem: function () {
    }
});

Ext.define('PIC.ExtGridComboBox', {
    extend: 'Ext.form.field.Picker',
    requires: ['Ext.util.DelayedTask', 'Ext.EventObject', 'Ext.view.BoundList', 'Ext.view.BoundListKeyNav', 'Ext.data.StoreManager', 'Ext.grid.View'],
    alternateClassName: 'Ext.form.GridComboBox',
    alias: ['widget.picgridcombo', 'widget.picgridcombobox'],
    triggerCls: Ext.baseCSSPrefix + 'form-arrow-trigger',
    multiSelect: false,
    delimiter: ',',
    displayField: 'text',
    triggerAction: 'all',
    allQuery: '',
    queryParam: 'query',
    queryMode: 'remote',
    queryCaching: true,
    pageSize: 0,
    autoSelect: true,
    typeAhead: false,
    typeAheadDelay: 250,
    selectOnTab: true,
    forceSelection: false,

    defaultListConfig: {
        emptyText: '',
        loadingText: '正在加载...',
        loadingHeight: 70,
        minWidth: 70,
        maxHeight: 300,
        shadow: 'sides'
    },

    // private
    ignoreSelection: 0,

    initComponent: function () {
        var me = this, isDefined = Ext.isDefined, store = me.store, transform = me.transform, transformSelect, isLocalMode;
        // <debug>
        if (!store && !transform) {
            Ext.Error.raise('Either a valid store, or a HTML select to transform, must be configured on the combo.');
        }
        if (me.typeAhead && me.multiSelect) {
            Ext.Error.raise('typeAhead and multiSelect are mutually exclusive options -- please remove one of them.');
        }
        if (me.typeAhead && !me.editable) {
            Ext.Error.raise('If typeAhead is enabled the combo must be editable: true -- please change one of those settings.');
        }
        if (me.selectOnFocus && !me.editable) {
            Ext.Error.raise('If selectOnFocus is enabled the combo must be editable: true -- please change one of those settings.');
        }
        // </debug>
        this.addEvents('beforequery', 'select');

        // Build store from 'transform' HTML select element's
        // options

        if (!store && transform) {
            transformSelect = Ext.getDom(transform);
            if (transformSelect) {
                store = Ext.Array.map(Ext.Array.from(transformSelect.options), function (
						option) {
                    return [option.value, option.text];
                });
                if (!me.name) {
                    me.name = transformSelect.name;
                }
                if (!('value' in me)) {
                    me.value = transformSelect.value;
                }
            }
        }

        me.bindStore(store, true);
        store = me.store;

        if (store.autoCreated) {
            me.queryMode = 'local';
            me.valueField = me.displayField = 'field1';
            if (!store.expanded) {
                me.displayField = 'field2';
            }
        }

        if (!isDefined(me.valueField)) {
            me.valueField = me.displayField;
        }

        isLocalMode = me.queryMode === 'local';
        if (!isDefined(me.queryDelay)) {
            me.queryDelay = isLocalMode ? 10 : 500;
        }

        if (!isDefined(me.minChars)) {
            me.minChars = isLocalMode ? 0 : 4;
        }

        if (!me.displayTpl) {
            me.displayTpl = Ext.create('Ext.XTemplate', '<tpl for=".">'
					+ '{[typeof values === "string" ? values : values.' + me.displayField
					+ ']}' + '<tpl if="xindex < xcount">' + me.delimiter + '</tpl>'
					+ '</tpl>');
        } else if (Ext.isString(me.displayTpl)) {
            me.displayTpl = Ext.create('Ext.XTemplate', me.displayTpl);
        }

        me.callParent();

        me.doQueryTask = Ext.create('Ext.util.DelayedTask', me.doRawQuery, me);

        // store has already been loaded, setValue
        if (me.store.getCount() > 0) {
            me.setValue(me.value);
        }

        // render in place of 'transform' select
        if (transformSelect) {
            me.render(transformSelect.parentNode, transformSelect);
            Ext.removeNode(transformSelect);
            delete me.renderTo;
        }
    },

    beforeBlur: function () {
        var me = this;
        me.doQueryTask.cancel();
        if (me.forceSelection) {
            me.assertValue();
        } else {
            me.collapse();
        }
    },

    assertValue: function () {
        var me = this, value = me.getRawValue(), rec;
        rec = me.findRecordByDisplay(value);
        //only allow set single value for now.
        me.setValue(rec ? [rec.raw] : []);
        me.collapse();
    },

    onTypeAhead: function () {
        var me = this, df = me.displayField;
        var st = me.store, rv = me.getRawValue();
        var r = me.store.findRecord(df, rv);
        if (r) {
            var nv = r.get(df), ln = nv.length, ss = rv.length;
            if (ss !== 0 && ss !== ln) {
                me.setRawValue(nv);
                me.selectText(ss, nv.length);
            }
        }
    },

    // invoked when a different store is bound to this combo
    // than the original
    resetToDefault: function () {
    },

    bindStore: function (store, initial) {
        var me = this, oldStore = me.store;
        // this code directly accesses this.picker, bc invoking
        // getPicker
        // would create it when we may be preping to destroy it
        if (oldStore && !initial) {
            if (oldStore !== store && oldStore.autoDestroy) {
                oldStore.destroy();
            } else {
                oldStore.un({
                    scope: me,
                    load: me.onLoad,
                    exception: me.collapse
                });
            }
            if (!store) {
                me.store = null;
                if (me.picker) {
                    me.picker.bindStore(null);
                }
            }
        }
        if (store) {
            if (!initial) {
                me.resetToDefault();
            }
            me.store = Ext.data.StoreManager.lookup(store);
            me.store.on({
                scope: me,
                load: me.onLoad,
                exception: me.collapse
            });
            if (me.picker) {
                me.picker.bindStore(store);
            }
        }
    },

    onLoad: function () {
        var me = this, value = me.value;
        me.syncSelection();
    },

    /**
    * @private Execute the query with the raw contents within
    *		  the textfield.
    */
    doRawQuery: function () {
        this.doQuery(this.getRawValue());
    },

    doQuery: function (queryString, forceAll) {
        queryString = queryString || '';
        // store in object and pass by reference in
        // 'beforequery'
        // so that client code can modify values.
        var me = this, qe = {
            query: queryString,
            forceAll: forceAll,
            combo: me,
            cancel: false
        }, store = me.store, isLocalMode = me.queryMode === 'local';

        if (me.fireEvent('beforequery', qe) === false || qe.cancel) {
            return false;
        }
        // get back out possibly modified values
        queryString = qe.query;
        forceAll = qe.forceAll;
        // query permitted to run
        if (forceAll || (queryString.length >= me.minChars)) {
            // expand before starting query so LoadMask can
            // position itself correctly
            me.expand();
            // make sure they aren't querying the same thing
            if (!me.queryCaching || me.lastQuery !== queryString) {
                me.lastQuery = queryString;
                store.clearFilter(!forceAll);
                if (isLocalMode) {
                    if (!forceAll) {
                        store.filter(me.displayField, queryString);
                    }
                } else {
                    store.load({
                        params: me.getParams(queryString)
                    });
                }
            }
            // Clear current selection if it does not match the
            // current value in the field
            if (me.getRawValue() !== me.getDisplayValue()) {
                me.ignoreSelection++;
                me.picker.getSelectionModel().deselectAll();
                me.ignoreSelection--;
            }
            if (me.typeAhead) {
                me.doTypeAhead();
            }
        }
        return true;
    },

    // private
    getParams: function (queryString) {
        var me = this, p = {}, pageSize = this.pageSize;

        if (me.picker && me.picker.grid && me.picker.grid.getQryOptions) {
            var _qryopts = me.picker.grid.getQryOptions(me.gridCfg);
            var _crit = getSingleSchCriterion(_qryopts, queryString);

            p.schscope = "control";    // 控件级查询，非页面级查询
            p.schcrit = { jcrit: [_crit["crit"]] };
        } else {
            p[this.queryParam] = queryString;
        }

        if (pageSize) {
            p.start = 0;
            p.limit = pageSize;
        }

        return p;
    },

    doTypeAhead: function () {
        if (!this.typeAheadTask) {
            this.typeAheadTask = Ext.create('Ext.util.DelayedTask', this.onTypeAhead, this);
        }
        if (this.lastKey != Ext.EventObject.BACKSPACE
				&& this.lastKey != Ext.EventObject.DELETE) {
            this.typeAheadTask.delay(this.typeAheadDelay);
        }
    },

    onTriggerClick: function () {
        var me = this;
        if (!me.readOnly && !me.disabled) {
            if (me.isExpanded) {
                me.collapse();
            } else {
                me.onFocus({});

                if (me.triggerAction === 'all') {
                    me.doQuery(me.allQuery, true);
                } else {
                    me.doQuery(me.getRawValue());
                }
            }
            me.inputEl.focus();
        }
    },

    // store the last key and doQuery if relevant
    onKeyUp: function (e, t) {
        var me = this, key = e.getKey();

        if (!me.readOnly && !me.disabled && me.editable) {
            me.lastKey = key;
            me.doQueryTask.cancel();

            // perform query w/ any normal key or backspace or
            // delete
            if (!e.isSpecialKey() || key == e.BACKSPACE || key == e.DELETE) {
                if (me.getRawValue() == '') {
                    me.clearValue();
                    return;
                }
                me.doQueryTask.delay(me.queryDelay);
            } else if (key == e.ENTER) {
                this.doQuery(this.getRawValue(), true);
            }
        }
    },

    initEvents: function () {
        var me = this;
        me.callParent();
        // setup keyboard handling
        me.mon(me.inputEl, 'keyup', me.onKeyUp, me);
    },

    createPicker: function () {
        var me = this, menuCls = Ext.baseCSSPrefix + 'menu';

        var opts = Ext.apply({
            selModel: {
                mode: me.multiSelect ? 'SIMPLE' : 'SINGLE'
            },
            floating: true,
            hidden: true,
            ownerCt: me.ownerCt,
            cls: me.el.up('.' + menuCls) ? menuCls : '',
            store: me.store,
            displayField: me.displayField,
            focusOnToFront: false,
            pageSize: me.pageSize,
            gridCfg: me.gridCfg,
            owner: me
        }, me.listConfig, me.defaultListConfig);

        var pk = me.picker = new PIC.ExtGridComboBoxList(opts);

        me.mon(pk, {
            itemclick: me.onItemClick,
            refresh: me.onListRefresh,
            scope: me
        });
        me.mon(pk.getSelectionModel(), {
            selectionChange: me.onListSelectionChange,
            scope: me
        });
        return pk;
    },

    onListRefresh: function () {
        this.alignPicker();
        this.syncSelection();
    },

    onItemClick: function (picker, record) {
        /*
        * If we're doing single selection, the selection change
        * events won't fire when clicking on the selected
        * element. Detect it here.
        */
        var me = this, lastSelection = me.lastSelection, valueField = me.valueField, selected;

        if (!me.multiSelect && lastSelection) {
            selected = lastSelection[0];
            if (record.get(valueField) === selected.get(valueField)) {
                me.collapse();
            }
        }
    },

    onListSelectionChange: function (list, selectedRecords) {
        var me = this;
        // Only react to selection if it is not called from
        // setValue, and if our list is
        // expanded (ignores changes to the selection model
        // triggered elsewhere)
        if (!me.ignoreSelection && me.isExpanded) {
            if (!me.multiSelect) {
                Ext.defer(me.collapse, 1, me);
            }
            me.setValue(selectedRecords, false);
            if (selectedRecords.length > 0) {
                me.fireEvent('select', me, selectedRecords);
            }
            me.inputEl.focus();
        }
    },

    /**
    * @private Enables the key nav for the BoundList when it is
    *		  expanded.
    */
    onExpand: function () {
        var me = this, keyNav = me.listKeyNav;
        var selectOnTab = me.selectOnTab, picker = me.getPicker();

        // redo layout to make size right after reload store
        picker.grid.doLayout();
        // Handle BoundList navigation from the input field.
        // Insert a tab listener specially to enable
        // selectOnTab.
        if (keyNav) {
            keyNav.enable();
        } else {
            keyNav = me.listKeyNav = Ext.create('Ext.view.BoundListKeyNav', this.inputEl, {
                boundList: picker,
                forceKeyDown: true,
                home: function (e) {
                    return true;
                },
                end: function (e) {
                    return true;
                },
                tab: function (e) {
                    if (selectOnTab) {
                        this.selectHighlighted(e);
                        me.triggerBlur();
                    }
                    // Tab key event is allowed to propagate to
                    // field
                    return true;
                }
            });
        }
        // While list is expanded, stop tab monitoring from
        // Ext.form.field.Trigger so it doesn't short-circuit
        // selectOnTab
        if (selectOnTab) {
            me.ignoreMonitorTab = true;
        }
        // Ext.defer(keyNav.enable, 1, keyNav); //wait a bit so
        // it doesn't react to the down arrow opening the picker
        me.inputEl.focus();
        me.syncSelection();
    },

    /**
    * @private Disables the key nav for the BoundList when it
    *		  is collapsed.
    */
    onCollapse: function () {
        var me = this, keyNav = me.listKeyNav;
        if (keyNav) {
            keyNav.disable();
            me.ignoreMonitorTab = false;
        }
    },

    select: function (r) {
        this.setValue(r, true);
    },

    findRecord: function (field, value) {
        var ds = this.store, idx = ds.findExact(field, value);
        return idx !== -1 ? ds.getAt(idx) : false;
    },

    findRecordByValue: function (value) {
        return this.findRecord(this.valueField, value);
    },
    findRecordByDisplay: function (value) {
        return this.findRecord(this.displayField, value);
    },

    setValue: function (value, doSelect) {
        var me = this, txt = me.inputEl;
        me.value = value || {};
        if (me.store.loading)
            return me;
        me.setRawValue(me.getDisplayValue());
        if (txt && me.emptyText && !Ext.isEmpty(value))
            txt.removeCls(me.emptyCls);
        me.checkChange();
        if (doSelect)
            me.syncSelection(); //
        me.applyEmptyText();
        return me;
    },

    getDisplayValue: function () {
        var me = this, dv = [];
        Ext.Object.each(me.value, function (k, v) {
            var a = v[me.displayField];
            if (a)
                dv.push(a);
        });
        return dv.join(',');
    },

    getValue: function () {
        return this.value || [];
    },

    //keys, spliter, doSelect
    setSubmitValue: function (keys, sp, ds) {
        var me = this, v = {}, sp = sp || ',';
        if (keys) {
            Ext.Array.each(keys.split(sp), function (a) {
                var r = me.store.findRecord(me.valueField, a, 0, false, true, true);
                if (r)
                    v[a] = r.data;
            });
        }
        me.setValue(v, ds);
    },

    getSubmitValue: function () {
        var me = this, sv = [];
        Ext.Object.each(me.value, function (k, v) {
            sv.push(v[me.valueField]);
        });
        return sv;
    },

    isEqual: function (v1, v2) {
        var fa = Ext.Array.from, i, len;
        v1 = fa(v1);
        v2 = fa(v2);
        len = v1.length;
        if (len !== v2.length) {
            return false;
        }
        for (i = 0; i < len; i++) {
            if (v2[i] !== v1[i]) {
                return false;
            }
        }
        return true;
    },

    clearValue: function () {
        this.setValue({});
    },

    syncSelection: function () {
        var me = this, pk = me.picker;
        if (pk && pk.grid) {
            var EA = Ext.Array, gd = pk.grid, st = gd.store;
            var cs = [];
            var sv = this.getSubmitValue();
            EA.each(st.data.items, function (r) {
                if (EA.contains(sv, r.data[me.valueField])) {
                    cs.push(r);
                }
            });
            gd.getSelectionModel().select(cs, false, true);
        }
    }
});

Ext.define('PIC.data.ExtSelectStore', {
    extend: 'PIC.data.ExtJsonStore',
    alias: 'store.picselectstore',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            pageSize: 50,
            autoLoad: false
        }, config);

        var url = config.url || location.href;
        url = $.combineQueryUrl(url, { asyncreq: true, qrytype: 'json' });

        if (!config.proxy) {
            config.proxy = {
                type: 'ajax',
                url: url,
                reader: config.reader
            };
        } else {
            if (config.proxy.url) {
                url = $.combineQueryUrl(config.proxy.url, { asyncreq: true, qrytype: 'json' });
            }

            config.proxy.url = url;
        }

        if (!config.proxy.reader) {
            config.proxy.reader = Ext.create('PIC.data.ExtJsonReader');
        }

        this.callParent([config]);

        me.on('beforeload', function (store, op, eOpts) {
            if (typeof (config.picbeforeload == "function")) {
                config.picbeforeload(store, op, eOpts);
            }
        });
    }
});

Ext.define('PIC.ExtGridSelector', {
    extend: 'PIC.ExtGridComboBox',
    alias: ['widget.picgridselect', 'widget.picgridselector'],
    alternateClassName: 'PIC.ExtGridSelect',

    gridCfg: null,
    fieldMap: null, // 值与其他表单域关联({valuekey1: fieldid1, valuekey2: fieldid2...})

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            // labelAlign: 'right',
            // displayField : 'name',
            // valueField : 'value',
            multiSelect: false,
            width: 300,
            labelWidth: 100,
            typeAhead: true,
            queryMode: 'remote',
            matchFieldWidth: false,
            pickerAlign: 'bl'
        }, config);

        var gridCfg = Ext.apply({
        }, config.gridCfg || {});

        if (config.multiSelect === true) {
            config.typeAhead = false;
            config.suspendCheckChange = false;
            // gridCfg.selModel = gridCfg.selModel || new Ext.selection.CheckboxModel({ checkOnly: true });
        }

        config.gridCfg = gridCfg;

        this.callParent([config]);
    },

    onChange: function (args, oldVal) {
        var me = this;

        this.callParent(arguments);

        // 判断store有没有加载,没有则不执行同步操作
        if (me.fieldMap && me.store && me.store.getCount() > 0) {
            var field;
            Ext.Object.each(me.fieldMap, function (fid, kv) {
                field = Ext.getCmp(fid);

                if (field) {
                    var v_arr = [];
                    Ext.Object.each(args, function (k, v) {
                        v_arr.push(v[kv]);
                    });

                    field.setValue(v_arr.join(","));
                }
            });
        }
    },

    setValue: function (value, doSelect) {
        var me = this;
        var val = value;
        if (typeof (val) == "string") {
            vals = val.split(",");
            val = {};
            Ext.each(vals, function (v) {
                var vobj = {};
                vobj[me.valueField] = v;
                val[v] = vobj;
            })
        }
        
        return this.callParent([val, doSelect]);
    },

    getSubmitValue: function () {
        var me = this, sv = [];
        Ext.Object.each(me.value, function (k, v) {
            sv.push(v[me.valueField]);
        });
        return sv.join(',');
    }
});

//------------------------PIC ExtJs Picker类控件 结束------------------------//


//------------------------PIC ExtJs 文件控件 开始------------------------//

Ext.define('PIC.data.ExtFileStore', {
    extend: 'PIC.data.ExtJsonStore',
    alias: 'store.picfilestore',
    mode: 'multi',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            isclient: true,
            idProperty: "id",
            autoLoad: false,
            fields: ['id', 'name', 'fullname', 'size', 'istemp']
        }, config);

        me.mode = config.mode || me.mode;

        if (config.data || config.value) {
            config.data = config.data || config.value;

            if (typeof (config.data) != "object") {
                var frecs = me.getFileRecords(config.data);
                config.data = { total: frecs.length, records: frecs };
            }
        }

        this.callParent([config]);
    },

    getFileRecords: function (value) {
        var me = this;
        var frecords = value;

        if (typeof (value) === "string") {
            value = value.trim();
            var frecs = [];

            // fileobject
            if (value.indexOf("{") === 0) {
                var fobj = $.getJsonObj(value);
                frecs = fobj.files || [];
            } else if (value.indexOf("[") === 0) {
                frecs = $.getJsonObj(value);
            } else {
                var fstrs = value.split(",");

                if ('single'.equals(me.mode) && fstrs.length > 1) {
                    fstrs = [fstrs[0]];
                }

                $.each(fstrs, function () {
                    var fstr = this;
                    if (fstr && fstr.length > 0) {
                        var fname = fstr.substring(fstr.indexOf("_") + 1);
                        var fid = fstr.substring(0, fstr.indexOf("_"));
                        var frec = { id: fid, name: fname, fullname: fstr };
                        frec.size = 0;

                        frecs.push(frec);
                    }
                });
            }

            frecords = frecs;
        } else if ($.isArray(value)) {
            if (value.length > 0) {
                var frecs = [];

                if (value[0].getFileData) {
                    $.each(value, function () {
                        frecs.push(this.getFileData());
                    });

                    frecords = frecs;
                }
            }
        } else if (value && value.files && $.isArray(value.files)) {
            // 针对fileobjectdata
            frecords = value.files;
        }

        return frecords;
    },

    getTotalSize: function () {
        var me = this;
        var tot_size = 0;

        var frecs = me.getRange();
        Ext.each(frecs, function (rec) {
            tot_size += rec.get('size') || 0;
        });
    },

    getMaxSizeRecord: function () {
        var frecs = me.getRange();
        var max_rec = null;
        var max_size = 0;
        Ext.each(frecs, function (rec) {
            if (!max_rec || rec.get('size') > max_size) {
                max_rec = rec;
                max_size = rec.get('size');
            }
        });
    },

    addFiles: function (files) {
        var me = this;

        var recs = me.getFileRecords(files);
        var frecs = [];

        Ext.each(recs, function (rec) {
            me.add(rec);
        });
    },

    getStringValue: function () {
        var me = this;
        var frecs = me.getRange();

        var fObjData = me.getFileObjectData();

        var strval = Ext.encode(fObjData);

        //Ext.each(frecs, function (rec) {
        //    strval += rec.get('fullname').toString() + ",";
        //});

        //if (strval) {
        //    strval = strval.trimEnd(",");
        //}

        return strval;
    },

    getFileObjectData: function () {
        var me = this;
        var frecs = me.getRange();
        var fdata = [];

        Ext.each(frecs, function (rec) {
            fdata.push(rec.getData());
        });

        return { files: fdata };
    }
});

Ext.define('PIC.ExtFileField', {
    extend: 'Ext.form.FieldContainer',
    mixins: { field: 'Ext.form.field.Field' },
    alias: 'widget.picfilefield',
    store: null,
    readOnly: false,
    restoredata: [],
    filepanel: null,
    ctrlpanel: null,
    fileview: null,
    mode: 'multi',
    EditViewTemplate: null,
    ReadViewTemplate: null,
    maxLength: null,
    maxSingleSize: null,
    maxTotalSize: null,
    maxCount: null,
    blankText: '该输入项为必输项',
    maxLengthText: '最大文件名总长度为 {0}',
    maxSingleSizeText: '最大单个文件大小为 {0}',
    maxTotalSizeText: '最大总文件大小为 {0}',
    maxCountText: '最大总文件数为 {0}',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            height: 80,
            border: false,
            readOnly: 'r'.equals(pgOp) || false,
            margin: "0 0 5 0",
            mode: "multi",
            maxCount: 20
        }, config);
        config.layout = 'border';

        me.mode = config.mode;
        me.readOnly = config.readOnly;
        me.allowBlank = config.allowBlank || true;

        var uploadparams = config.uploadparams || {};
        me.store = config.store || Ext.create('PIC.data.ExtFileStore', config);
        me.restoredata = me.store.getRange();

        me.EditViewTemplate = new Ext.XTemplate(
            '<tpl for=".">',
                '<div class="pic-ctrl-file-link" linkfile="{fullname}" style="float:left; height: 20; margin:' + ('single'.equals(me.mode) ? '0' : '2') + 'px; border:0px;">',
                '<span style="border:0px; display:' + (('single'.equals(me.mode) || me.readOnly) ? 'none' : '') + '"><input type="checkbox" value="{id}" /></span>',
                '<a style="margin:5px; cursor: hand; " title="{name}" href="' + PICConfig.FilePagePath + '?id={id}&istemp={istemp}">{name}</a>',
                '</div>',
            '</tpl>'
        );

        me.ReadViewTemplate = new Ext.XTemplate(
            '<tpl for=".">',
                '<div class="pic-ctrl-file-link" linkfile="{fullname}" style="float:left; height: 20; margin:' + ('single'.equals(me.mode) ? '0' : '2') + 'px; border:0px">',
                '<a style="margin:5px; cursor: hand" title="{name}" href="' + PICConfig.FilePagePath + '?id={id}&istemp={istemp}">{name}</a>',
                '</div>',
            '</tpl>'
        );

        var uploadbtn, removebtn, clearbtn, restorebtn;

        if ('single'.equals(me.mode)) {
            config.height = 22;
            uploadbtn = Ext.create("PIC.ExtButton", { text: '上传', flex: 1, handler: function () { me.upload(); } });
            clearbtn = Ext.create("PIC.ExtButton", { text: '清空', flex: 1, handler: function () { me.removeAllFiles(); } });
            me.ctrlpanel = Ext.create("PIC.ExtPanel", {
                region: 'east',
                layout: { type: 'hbox' },
                defaults: { margins: '0 0 0 5' },
                align: 'middle',
                border: false,
                hidden: me.readOnly,
                width: 100,
                items: [uploadbtn, clearbtn]
            });
        } else {
            uploadbtn = Ext.create("PIC.ExtButton", {
                text: '上传',
                handler: function () {
                    me.upload();
                }
            });

            clearbtn = Ext.create("PIC.ExtButton", { text: '清空', handler: function () { me.removeAllFiles(); } });
            restorebtn = Ext.create("PIC.ExtButton", { text: '还原', hidden: true, handler: function () { me.restoreFiles(); } });
            removebtn = Ext.create("PIC.ExtButton", { text: '删除', handler: function () {
                var chkitems = Ext.query("input:checked", me.el.dom);
                $.each(chkitems, function () { me.removeFile(this.value); })
            }
            });

            me.ctrlpanel = Ext.create("PIC.ExtPanel", {
                region: 'east',
                layout: { type: 'vbox', padding: '5', align: 'stretch', pack: 'center' },
                defaults: { margins: '0 0 5 0' },
                align: 'stretch',
                border: false,
                hidden: me.readOnly,
                width: 100,
                items: [uploadbtn, removebtn, clearbtn, restorebtn]
            });
        };

        me.fileview = Ext.create('PIC.ExtDataView', {
            layout: 'fit',
            itemSelector: 'div.pic-ctrl-file-link',
            tpl: me.EditViewTemplate,
            store: me.store
        });

        me.filepanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            autoScroll: true,
            items: me.fileview
        });

        config.items = [me.filepanel, me.ctrlpanel];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        PIC.ExtFieldBase.initRequiredTag(this);

        me.callParent(arguments);

        me.addEvents('change');
    },

    upload: function () {
        var me = this;
        var p = Ext.apply({ sender: this, callback: 'onUploadCompleted', mode: me.mode }, me.uploadparams || {});

        PICUtil.openUploadDialog(p);
    },

    onUploadCompleted: function (args) {
        var me = this;
        var data = args.data || [];

        if ('single'.equals(me.mode)) {
            if (data.length > 0) {
                me.removeAllFiles();
                me.addFiles(data || "");
            }
        } else {
            me.addFiles(data || "");
        }
    },

    restoreFiles: function () {
        var me = this;

        me.store.removeAll();
        me.store.add(me.restoredata);

        me.fireEvent('change');
    },

    removeFile: function (fid) {
        var me = this;

        // var rec = store.getById(fid);    // 不能工作，store的id值总是自动生成
        var idx = me.store.findExact("id", fid);    // 不能工作，store的id值总是自动生成
        me.store.removeAt(idx);

        me.fireEvent('change');
    },

    addFiles: function (files) {
        var me = this;
        me.store.addFiles(files);

        me.fireEvent('change');
    },

    removeAllFiles: function () {
        var me = this;
        me.store.removeAll();
        me.fireEvent('change');
    },

    setValue: function (files) {
        var me = this;
        me.removeAllFiles();
        me.addFiles(files);

        me.fireEvent('change');
    },

    getValue: function () {
        var me = this;

        return me.store.getStringValue();
    },

    setReadOnly: function (b) {
        var me = this;
        me.readonly = b;
        if (me.ctrlpanel) {
            me.ctrlpanel.setVisible(!b);
        }

        $(me.el.dom).find("input").css("display", "none");
    },

    onRenderReadView: function () {
        return false;
    },

    validate: function () {
        var me = this,
            errors,
            isValid,
            wasValid;

        if (me.disabled) {
            isValid = true;
        } else {
            errors = me.getErrors();
            isValid = Ext.isEmpty(errors);
            wasValid = !me.hasActiveError();
            if (isValid) {
                me.unsetActiveError();
            } else {
                me.setActiveError(errors);
            }
        }
        if (isValid !== wasValid) {
            me.fireEvent('validitychange', me, isValid);
            me.updateLayout();
        }

        return isValid;
    },

    renderActiveError: function () {
        var me = this,
            activeError = me.getActiveError(),
            hasError = !!activeError;

        if (activeError !== me.lastActiveError) {
            me.fireEvent('errorchange', me, activeError);
            me.lastActiveError = activeError;
        }

        if (me.rendered && !me.isDestroyed && !me.preventMark) {
            Ext.get(me.fileview.el.dom.parentNode)[hasError ? 'addCls' : 'removeCls']("pic-ctrl-file-invalid");

            me.getActionEl().dom.setAttribute('aria-invalid', hasError);

            if (me.errorEl) {
                me.errorEl.dom.innerHTML = activeError;
            }
        }
    },

    getErrors: function (value) {
        var me = this,
            errors = [],
            validator = me.validator,
            allowBlank = me.allowBlank,
            maxTotalSize = me.maxTotalSize,
            maxSingleSize = me.maxSingleSize,
            maxCount = me.maxCount,
            vtype = me.vtype,
            vtypes = Ext.form.field.VTypes,
            msg;

        var value = me.getValue();

        if (Ext.isFunction(validator)) {
            msg = validator.call(me, value);
            if (msg !== true) {
                errors.push(msg);
            }
        }

        if (!allowBlank) {
            if (!value) {
                errors.push(me.blankText);
            } else {
                var fileVal = Ext.decode(value);

                if (!fileVal || !fileVal.files || fileVal.files.length == 0) {
                    errors.push(me.blankText);
                }
            }

            return errors;
        }

        if (me.maxLength && value.length > me.maxLength) {
            errors.push(Ext.String.format(me.maxLengthText, me.maxLength));
        }

        if (me.maxCount && me.store.getCount() > me.maxCount) {
            errors.push(Ext.String.format(me.maxCountText, me.maxCount));
        }

        if (me.maxSingleSize && me.store.getSingleSize() > me.maxSingleSize) {
            errors.push(Ext.String.format(me.maxSingleSizeText, me.maxSingleSize));
        }

        if (me.maxTotalSize && me.store.getTotalSize() > me.maxTotalSize) {
            errors.push(Ext.String.format(me.maxTotalSizeText, me.maxTotalSize));
        }

        if (vtype) {
            if (!vtypes[vtype](value, me)) {
                errors.push(me.vtypeText || vtypes[vtype + 'Text']);
            }
        }

        return errors;
    }
}, function () {
    this.borrow(Ext.form.field.Base, ['markInvalid', 'clearInvalid']);
});

Ext.define('PIC.ExtWindow', {
    extend: 'Ext.window.Window',
    alias: ['widget.picwin'],

    constructor: function (config) {
        config = Ext.apply({
            closeAction: 'hide',
            resizable: true,
            layout: 'fit',
            renderTo: Ext.getBody(),
            frame: true,
            plain: true,
            resizable: false,
            buttonAlign: "right",
            closeAction: "hide"
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIC.BaseFormPanel', {
    extend: 'Ext.form.FormPanel',
    
    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        this.callParent([config]);
    },

    reset: function () {
        var me = this;
        var fields = me.getForm().reset();
    },

    findField: function (id) {
        return this.getForm().findField(id);
    },

    getFieldByName: function (name) {
        var cmps = this.query("[name=" + name + "]");
        if (cmps.length > 0) {
            return cmps[0];
        }

        return null;
    },

    setFieldsDisabled: function (names, disabled) {
        var me = this;
        Ext.each(names, function (f) {
            me.getFieldByName(f).setDisabled(disabled);
        });
    },

    setFieldsReadOnly: function (names, readOnly) {
        var me = this;

        Ext.each(names, function (f) {
            me.getFieldByName(f).setReadOnly(readOnly);
        });
    },

    getFieldValue: function (name, defval) {
        var cmp = this.getFieldByName(name);

        var val = null;
        if (cmp) {
            val = cmp.getValue();
        }

        if (!val && typeof (defval) != "undefined")
            return defval;

        return val;
    },

    setFieldValue: function (name, val) {
        var cmp = this.getFieldByName(name);

        if (cmp) {
            cmp.setValue(val);
            return true;
        }

        return false;
    },

    getFormData: function () {
        return this.getForm().getValues();
    }
});

Ext.define('PIC.ExtFormWin', {
    extend: 'Ext.window.Window',
    alias: ['widget.picformwin', 'widget.picfrmwin'],

    owner: null,
    action: null,

    constructor: function (config) {
        config = Ext.apply({
            closeAction: 'hide',
            resizable: true,
            layout: 'fit',
            maximizable: true,
            closable: true,
            bodyStyle: "padding:10px",
            lableWidth: 45
        }, config);

        var frmconfig = Ext.apply({
            defaultType: 'textfield',//表单默认类型
            frame: true,
            labelWidth: 75,
            width: 400
        }, config.frmconfig || {});

        config.items = Ext.create('PIC.BaseFormPanel', frmconfig);

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    getForm: function () {
        var me = this;
        var forms = me.query('form');

        if (forms.length > 0) {
            return forms[0];
        }

        return null;
    },

    isValid: function () {
        return this.getForm().isValid();
    },

    getFieldByName: function (name) {
        var cmps = this.getForm().query("[name=" + name + "]");
        if (cmps.length > 0) {
            return cmps[0];
        }

        return null;
    },

    getFieldValue: function (name, defval) {
        var cmp = this.getFieldByName(name);

        var val = null;
        if (cmp) {
            val = cmp.getValue();
        }

        if (!val && typeof (defval) != "undefined")
            return defval;

        return val;
    },

    setFieldValue: function (name, val) {
        var cmp = this.getFieldByName(name);

        if (cmp) {
            cmp.setValue(val);
            return true;
        }

        return false;
    },

    getRecord: function () {
        return this.getForm().getRecord();
    },

    load: function (owner, action, rec) {
        var me = this;

        me.action = action;
        me.owner = owner;

        var _f = this.getForm();

        if (rec) {
            _f.loadRecord(rec);
        } else {
            _f.reset();
        }

        var is_view = ("view" === me.action);

        me.setReadonly(is_view)

        me.show();
    },

    loadRecord: function (rec, action) {
        return this.getForm().loadRecord(rec);
    },

    setReadonly: function (flag) {
        var me = this;

        me.getForm().setReadOnly(flag);
        var btns = me.query(".button");

        Ext.each(btns, function (btn) {
            btn.setVisible(!(flag && btn.pisexecutable));
        });
    },

    reset: function () {
        this.getForm().reset();
    },

    submit: function (params) {
        return this.getForm().submit(params);
    }
});

//------------------------PIC ExtJs 文件控件 结束------------------------//

//------------------------------PIC ExtJs 表单域 结束----------------------------//


//-----------------------------PIC ExtJs 面板 开始--------------------------//

Ext.define('PIC.ExtPanelMixin', {
    extend: 'Ext.Panel',
    alias: 'widget.picpanelmixin',

    getDockedItem: function (selector, beforeBody) {
        var items = this.getDockedItems(selector, beforeBody);
        if (items && items.length > 0) {
            return items[0];
        }

        return null;
    }
});

Ext.define('PIC.ExtPanel', {
    extend: 'Ext.Panel',
    alias: 'widget.picpanel',
    mixins: ['PIC.ExtPanelMixin']
});

Ext.define('PIC.ExtTabPanel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.pictabpanel'
});


Ext.define('PIC.ExtViewport', {
    extend: 'Ext.Viewport',
    alias: 'widget.picviewport',

    constructor: function (config) {
        config = Ext.apply({
            layout: 'border'
        }, config);

        this.callParent([config]);
    }
});

Ext.define('PIC.ExtDataView', {
    extend: 'Ext.DataView',
    alias: 'widget.picdataview'
});

Ext.define('PIC.Page', {
    extend: 'PIC.ExtViewport',
    alias: 'widget.pic-page',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        window.PICPage = me;

        me.callParent([config]);
    }
});

Ext.define('PIC.FormPage', {
    extend: 'PIC.Page',
    alias: 'widget.pic-formpage',

    op: null,   // 操作（c, r, u, d）
    formPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'fit',
            border: false
        }, config);

        me.callParent([config]);
    },

    getFormData: function () {
        var me = this;
        if (me.formPanel && me.formPanel.form && me.formPanel.form.getValues) {
            return me.formPanel.form.getValues();
        }

        return null;
    }
});

Ext.define('PIC.TabFramePage', {
    extend: 'PIC.Page',
    alias: 'widget.pic-tabframepage',

    tabItems: [],
    initpage:'',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        me.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        Ext.each(me.tabItems, function (t) {
            t.border = t.border || false;
            t.html = t.html || "<div style='display:none;'></div>";
            t.listeners = t.listeners || {};
            t.listeners.activate = t.listeners.activate || function (tab) { me.handleActivate(tab) } ;
        });

        me.tabPanel = Ext.create("PIC.ExtTabPanel", {
            region: 'center',
            activeTab: (me.activeTab || 0),
            border: false,
            width: document.body.offsetWidth - 5,
            items: me.tabItems
        });

        me.navPanel = Ext.create("PIC.ExtPanel", {
            region: 'north',
            layout: 'border',
            border: false,
            height: 25,
            items: me.tabPanel
        });

        me.stagePanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            border: false,
            html: '<iframe width="100%" height="100%" id="frameContent" name="frameContent" frameborder="0" src="' + me.initpage + '"></iframe>',
            listeners: {
                afterrender: function () {
                    me.contentFrame = document.getElementById("frameContent");
                }
            }
        });

        me.mainPanel = Ext.create("PIC.ExtPanel", {
            layout: 'border',
            region: 'center',
            margins: '0 0 0 0',
            border: false,
            tbar: me.titbar || false,
            items: [me.navPanel, me.stagePanel]
        });

        me.items = [me.mainPanel];

        me.callParent(arguments);
    },

    handleActivate: function (tab) {
        var me = this;

        var flag = true;
        if (typeof (me.ontabactive) === 'function') {
            flag = me.ontabactive(tab);
        }

        if (flag !== false) {
            me.contentFrame.src = tab.href;
        }
    }
});

//-----------------------------PIC ExtJs 面板 结束--------------------------//

//-----------------------------PIC ExtJs Grid Panel 开始--------------------------//

Ext.define('PIC.grid.ExtRowNumberer', {
    extend: 'Ext.grid.RowNumberer',
    alias: 'widget.picrownumberer',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            width: 25,
            title: '&nbsp;',
            sortable: false
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIC.selection.ExtCheckboxModel', {
    extend: 'Ext.selection.CheckboxModel',
    alias: 'selection.piccheckboxmodel',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            mode: "multi"
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIC.ExtPagingField', {
    extend: 'Ext.form.NumberField',
    alias: 'widget.picpagingfield',

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            width: 50,
            minValue: 1,
            maxValue: 1000,
            value: 20,
            enableKeyEvents: true
        }, config);

        me.callParent([config]);
    }
});

Ext.define('PIC.toolbar.ExtPagingToolbar', {
    extend: 'Ext.PagingToolbar',
    alias: 'widget.picpagingtoolbar',
    alternateClassName: 'PIC.ExtPagingToolbar',
    pgfield: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            displayInfo: true,
            displayMsg: '当前条目 {0} - {1}, 总条目 {2}',
            emptyMsg: '无条目'
        }, config);

        if (!config.items) {
            if (!config.pgfield) {
                me.pgfield = Ext.create("PIC.ExtPagingField", { pgbar: me });
            } else {
                me.pgfield = config.pgfield;
            }
            config.items = ['-', { text: '页面大小：', xtype: 'tbtext' }, me.pgfield]
        }

        me.callParent([config]);
    }
});

Ext.define('PIC.ExtGridPanelMixin', {
    extend: 'Ext.grid.GridPanel',
    alias: 'widget.picgridpanelmixin',

    getSelection: function () {
        return this.getSelectionModel().getSelection();
    },

    getFirstSelection: function () {
        var recs = this.getSelection();
        if (recs.length > 0) {
            return recs[0];
        }
        return null;
    },

    hasSelection: function () {
        var sm = this.getSelectionModel();
        return sm.hasSelection();
    }
});

Ext.define('PIC.grid.ExtGridPanel', {
    extend: 'Ext.grid.GridPanel',
    alias: 'widget.picgridpanel',
    mixins: ['PIC.ExtPanelMixin', 'PIC.ExtGridPanelMixin', 'PIC.ExtExcelMixin'],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);

        config.selModel = config.selModel || Ext.create('PIC.selection.ExtCheckboxModel', {
            mode: config.selectMode || "multi"
        });

        me.callParent([config]);
    }
});

Ext.define('PIC.grid.ExtEditorGridPanel', {
    extend: 'PIC.grid.ExtGridPanel',
    alias: 'widget.piceditorgridpanel',
    mixins: ['PIC.ExtPanelMixin', 'PIC.ExtGridPanelMixin'],
    editing: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            plugins: [],
            columns: []
        }, config);

        config.selModel = config.selModel || Ext.create('PIC.selection.ExtCheckboxModel');

        me.editing = Ext.create('Ext.grid.plugin.CellEditing');
        config.plugins.push(me.editing);

        me.callParent([config]);
    },

    startEditByPosition: function (args) {
        this.editing.startEditByPosition(args);
    }
});

//-----------------------------PIC ExtJs GridPanel 结束--------------------------//


//-----------------------------PIC ExtJs Treeanel 开始--------------------------//

Ext.define('PIC.data.ExtTreeStore', {
    extend: 'Ext.data.TreeStore',
    alias: 'store.pictree',
    alternateClassName: 'PIC.ExtTreeStore',

    mixins: ['PIC.data.ExtStoreMixin'],

    expanded: false,
    rootPathLevel: 0,
    idProperty: null,
    parentIdProperty: null,
    isLeafProperty: null,
    pathLevelProperty: null,
    sortIndexProperty: null,
    iconProperty: null,

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

        if (!config.model && !config.idProperty) {
            config.idProperty = "Id";
        }

        me.callParent([config]);

        if (!config.root && config.data) {
            var rdata = me.adjustRootData(config.data);
            me.setRootNode(rdata);
        }
    },

    adjustRootData: function (data) {
        var me = this;
        var rdata = { text: '.' };
        if (Ext.isArray(data) && data.length > 0) {
            rdata.children = [];

            Ext.each(data, function (node) {
                if (node[me.pathLevelProperty] == me.rootPathLevel) {
                    var p = me.adjustNodeData(data, node);
                    rdata.children.push(p);
                }
            });
        }

        return rdata;
    },

    adjustNodeData: function (data, parent) {
        var me = this;

        if (parent && Ext.isArray(data)) {
            Ext.each(data, function (node) {
                if (parent[me.idProperty] == node[me.parentIdProperty]) {
                    parent.children = parent.children || [];
                    parent.children.push(node);

                    me.adjustNodeData(data, node);
                }
            });

            if (parent.children && parent.children.length > 0) {
                parent.expanded = parent.expanded || me.expanded;
                parent.leaf = false;
            } else {
                parent.leaf = (parent[me.isLeafProperty] === true);
            }

            if (me.iconProperty && parent[me.iconProperty]) {
                parent.icon = parent[me.iconProperty];
            }
        }

        return parent;
    },

    getRange: function () {
        var nodes = this.getAllNodes();

        return nodes;
    },

    getCount: function() {
        var range = this.getRange();

        if(range){
            return range.length;
        }else{
            return 0;
        }
    }
});

Ext.define('PIC.ExtTreePanel', {
    extend: 'Ext.TreePanel',
    alias: 'widget.pictreepanel',
    mixins: ['PIC.ExtPanelMixin', 'PIC.ExtGridPanelMixin'],
    editing: null,

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
        }, config);

        me.callParent([config]);
    }
});

//-----------------------------PIC ExtJs Treeanel 结束--------------------------//