
//------------------------PIC 页面处理 开始------------------------//

var PIC_OP_ACTION = {
    c: 'create',
    cs: 'createsub',
    cp: 'copy',
    r: 'read',
    u: 'update',
    d: 'delete',
    exec: 'execute'
};

// 操作类型
var OP_TYPE = {
    c: { name: "c", code: "create", text: "创建", action: "create" },
    r: { name: "r", code: "read", text: "读取", action: "read" },
    u: { name: "u", code: "update", text: "更新", action: "update" },
    d: { name: "d", code: "delete", text: "删除", action: "delete" },
    o: { name: "o", code: "other", text: "其他", action: "other" }
}

var PIC_TIMER_INTERVAL = 600000;  // 60s

var PICState = {};
var PICPath = null;
var PICVar = {};
var PICSearchCrit = {}; // 数据查询规则

var pgTimerEnabled = false; // 是否pgTimer生效

var pgOp, pgOperation, pgOpType, pgOperationType, pgAction;

// 页面初始化
Ext.onReady(function () {
    PICState = getPageState() || PICState;

    PICVar["UserInfo"] = PICState["UserInfo"];
    PICVar["SystemInfo"] = PICState["SystemInfo"];

    pgOp = pgOperation = $.getQueryString({ ID: "op" });   // 操作
    pgOpType = pgOperationType = $.getQueryString({ ID: "optype", DefaultValue: "s" });   // 操作执行位置(s, c)服务器端客户端
    pgAction = PIC_OP_ACTION[pgOp] || pgOp;    // 页面活动

    if (PICState) { PICSearchCrit = PICState[PIC_PAGE_SCH_CRIT_KEY] || {}; }
    
    pgInit();
});

// 获取PageState
function getPageState() {
    var pgState = null;
    var psstr = $("#" + PIC_PAGE_STATE_KEY).val();
    if (psstr != null) { pgState = $.getJsonObj(psstr); }

    return pgState;
}

// 初始化PIC页面
function pgInit() {
    initPICEvt();   // 初始化事件
    initPICUI();    // 初始化PIC界面
    initPICQry();   // 初始化PIC查询控件

    PICPgObserver.onPgLoad(); // 触发pgload事件
}

// 由页面打开的窗口回吊使用
function pgDialogCallback(args) {
    return PICPgObserver.onDgCallback(args);
}

function pgTimeElapsed() {
    if (pgTimerEnabled == true) {
        PICPgObserver.onPgTimer(); // 触发pgtimer事件

        PICPgTimer = setTimeout(pgTimeElapsed, PIC_TIMER_INTERVAL);
    }
}

function enablePgTimer() {
    pgTimerEnabled = true;

    PICPgTimer = setTimeout(pgTimeElapsed, PIC_TIMER_INTERVAL);
}

function disablePgTimer() {
    pgTimerEnabled = false;

    if (PICPgTimer) { clearTimeout(PICPgTimer); }
}

// 初始化事件
function initPICEvt() {
    if (typeof (onPgInit) == "function") { PICPgObserver.addListener('pginit', onPgInit); }
    if (typeof (onFrmLoad) == "function") { PICPgObserver.addListener('frmload', onFrmLoad); }
    if (typeof (onPgLoad) == "function") { PICPgObserver.addListener('pgload', onPgLoad); }
    if (typeof (onPgTimer) == "function") { PICPgObserver.addListener('pgtimer', onPgTimer); }    // onPgTimer shouldn't include time-consuming or user interacted function
    if (typeof (onPgUnload) == "function") { PICPgObserver.addListener('pgunload', onPgUnload); }
    if (typeof (onDgCallback) == "function") { PICPgObserver.addListener('dgcallback', onDgCallback); }

    $(window).bind("beforeunload", function () {
        // 触发pgunload事件
        PICPgObserver.onPgUnload();
    });

    $(window).bind("beforeprint", function() {
        // 隐藏按钮（并保存当前按钮状态以便于还原）
    });

    $(window).bind("afterprint", function() {
        // 还原按钮
    });

    PICPgObserver.onPgInit(); // 触发pginit事件
}

// 初始化PIC样式
function initPICUI() {
    PICPgObserver.onFrmLoad(); // 触发frmload事件
}

// 初始化PIC查询控件
function initPICQry() {
    $("[picqry], [qryopts], [qrygrp]").each(function (i) {
        var qryopts = $.getJsonObj($(this).attr("qryopts")) || {};
        var qryevent = qryopts["event"] || "keyup";
        if (qryevent && !$(this).attr(qryevent)) {
            $(this).bind(qryevent, function (event) {
                var event = event || window.event;
                if (qryevent != "keyup" || (qryevent == "keyup" && event.keyCode == 13)) {
                    qryEventHandler(this, event);
                }
            });
        }
    });
}

function getPICOpType (code) {
    var op = {};

    for (var c in OP_TYPE) {
        var _op = OP_TYPE[c];

        if (_op.name === code || _op.code === code || _op.text === code || _op.action === code) {
            op = _op;
            break;
        }
    }

    return op;
}

//------------------------PIC 页面处理 结束------------------------//


//------------------------PIC 查询规则处理 开始------------------------//

// 查询方式
var SEARCH_MODE = {
    Equal: ["C", 0],
    NotEqual: ["C", 1],
    In: ["C", 2],
    NotIn: ["C", 3],
    Like: ["C", 4],
    NotLike: ["C", 5],
    GreaterThan: ["C", 6],
    GreaterThanEqual: ["C", 7],
    LessThan: ["C", 8],
    LessThanEqual: ["C", 9],
    StartWith: ["C", 10],
    EndWith: ["C", 11],
    NotStartWith: ["C", 12],
    NotEndWith: ["C", 13],
    UnSettled: ["C", 14],
    IsEmpty: ["S", 0],    // 查询集合时使用
    IsNotEmpty: ["S", 1],    // 查询集合时使用
    IsNull: ["S", 2],
    IsNotNull: ["S", 3],
    UnSettled: ["S", 4]   // 未设定
}

// 数据类型
var DATA_TYPE = {
    Integer: "Int32",
    Int: "Int32",
    Date: "DateTime"
}

// 获取查询模式值
function getSearchModeValue(str) {
    return SEARCH_MODE[str] || SEARCH_MODE["Equal"];    // 默认等于查询
}

// 处理查询
function qryEventHandler(obj, event) {
    var qryopts = $.getJsonObj($(obj).attr("qryopts")) || {};
    var qryTarget = ($(qryopts["target"])[0] || {}).PIC || PICDefQryTarget;
    if (qryTarget) {
        var schCrit = getSchCriterion(obj);
        if (qryTarget.query) {
            qryTarget.query(schCrit["ccrit"], schCrit["ftcrit"]);
        }
    }
}

function getRequestCriterion(reqparams) {
    var params = reqparams || {};

    params[PIC_QUERY_CRIT_KEY] = params[PIC_QUERY_CRIT_KEY] || {};

    if (params.schcrit !== undefined) {
        params[PIC_QUERY_CRIT_KEY]["Searches"] = params[PIC_QUERY_CRIT_KEY]["Searches"] || {};

        params[PIC_QUERY_CRIT_KEY]["Searches"]["Searches"] = params.schcrit["ccrit"] || [];
        params[PIC_QUERY_CRIT_KEY]["Searches"]["FTSearches"] = params.schcrit["ftcrit"] || [];
        params[PIC_QUERY_CRIT_KEY]["Searches"]["JuncSearches"] = params.schcrit["jcrit"] || [];
    }

    if (params.start !== undefined && params.limit !== undefined) {
        params[PIC_QUERY_CRIT_KEY]["CurrentPageIndex"] = parseInt(params.start / params.limit) + 1;
        params[PIC_QUERY_CRIT_KEY]["PageSize"] = params.limit;
    }

    if (Ext.isArray(params.sorters)) {
        var orders = [];
        Ext.each(params.sorters, function (s) {
            orders.push({ "PropertyName": s.property, "Ascending": (s.direction == "ASC") });
        });

        params[PIC_QUERY_CRIT_KEY]["Orders"] = orders;
    }

    return params;
}

function getSchCriterionByGroup(picgrp, crit) {
    var tcrit = { ccrit: [], ftcrit: [], jcrit: [] };
    
    crit = crit || tcrit;
    if (!picgrp) return crit;

    var qryinputs = $("[picgrp=" + picgrp + "]");


    // 普通检索
    $.each(qryinputs, function (i) {
        var val;

        if (Ext && Ext.getCmp && this.id) {
            var cmp = Ext.getCmp(this.id);
            val = cmp.getValue();
        } else {
            val = $(this).val();
        }

        var _crit = getSingleSchCriterion($(this).attr("qryopts"), val);

        switch (_crit["type"]) {
            case "fulltext":
                crit["ftcrit"].push(_crit["crit"]);
                break;
            case "junc":
                crit["jcrit"].push(_crit["crit"]);
                break;
            default:
                crit["ccrit"].push(_crit["crit"]);
                break;
        }
    });

    return crit;
}

// 由Html元素和查询标准模板获取查询标准对象
function getSchCriterion(htmlele, crit) {
    
    var tcrit = { ccrit: [], ftcrit: [], jcrit: [] };

    if (!htmlele) return crit || tcrit;
    crit = crit || tcrit;

    var picgrp = $(htmlele).attr("picgrp"); // 查询组

    var qryinputs = [];
    if (!picgrp) {  // 没有组，则为单字段查询
        qryinputs[qryinputs.length] = htmlele;
    } else {
        qryinputs = $("[picgrp=" + picgrp + "]");
    }

    // 普通检索
    $.each(qryinputs, function (i) {
        var val;

        if (Ext && Ext.getCmp && this.id) {
            var cmp = Ext.getCmp(this.id);
            val = cmp.getValue();
        } else {
            val = $(this).val();
        }

        var _crit = getSingleSchCriterion($(this).attr("qryopts"), val);

        switch (_crit["type"]) {
            case "fulltext":
                crit["ftcrit"].push(_crit["crit"]);
                break;
            case "junc":
                crit["jcrit"].push(_crit["crit"]);
                break;
            default:
                crit["ccrit"].push(_crit["crit"]);
                break;
        }
    });

    return crit;
}

function getSingleSchCriterion(obj, val) {
    var opts = {};

    if (typeof (obj) == "string") {
        opts = $.getJsonObj(obj) || {};
    } else if (typeof (obj) == "object") {
        opts = obj;
    }
    
    var tcrit = {}
    var isft = false;

    // 查询类型 fulltext或junc或default(全文查询或默认查询)
    var type = opts["type"] || "default";

    // junc查询
    if (isJuncSchOptions(opts)) {
        type = "junc";
    }

    if (type.toLowerCase() == "fulltext") {
        tcrit["Value"] = val;
        tcrit["ColumnList"] = (opts["field"] ? opts["field"].split(",") : null);
    } else if (type.toLowerCase() == "junc") {
        tcrit = getJuncSchCriterion(opts, val);
    } else {
        tcrit["PropertyName"] = opts["field"] || obj.id || $(obj).attr("name");
        tcrit["Value"] = val;
        if (opts["datatype"]) {
            tcrit["Type"] = DATA_TYPE[opts["datatype"]] || opts["datatype"];
        }
        var schMode = getSearchModeValue(opts["mode"]);
        if (schMode[0] == "C") {
            tcrit["SearchMode"] = schMode[1]
        } else {
            tcrit["SingleSearchMode"] = schMode[1];
        }
    }

    return {crit:tcrit, type:type};
}

function isJuncSchOptions(opts) {
    return !!(opts["juncmode"] || opts["items"] && $.isArray(typeof (opts["items"])) || $.isArray(opts));
}

// 获取连接查询
function getJuncSchCriterion(opts, val) {
    var tcrit = { JunctionMode: 'Or', Searches: [] };
    var items = opts["items"] || [];
    tcrit["JunctionMode"] = opts["juncmode"] || "Or";

    if ($.isArray(opts)) {
        items = opts;
    }

    $.each(items, function() {
        var _tcrit = getSingleSchCriterion(this, val);
        if (_tcrit["crit"]) {
            tcrit["Searches"].push(_tcrit["crit"]);
        }
    });

    return tcrit;
}

//------------------------PIC 查询规则处理 结束------------------------//

//------------------------PIC 查询适配器 开始------------------------//

PIC.DataAdapter = (function () {
    return {
        getRequest: function (params) {
            var params = params || {};
            var method = params.method || params.posttype || 'POST';
            params.asyncreq = true;
            var dt = params.data || {};

            var url = params.url || $.getQueryUrl();
            url = url.trimEnd("#").replace(/#\?/g, "?");

            // 处理查询条件（一般列表页面需要）
            var qrycrit = params[PIC_QUERY_CRIT_KEY] || dt[PIC_QUERY_CRIT_KEY] || {};
            var postdata = { };
            postdata[PIC_ASYNC_REQ_KEY] = true;
            postdata[PIC_PAGE_SCH_CRIT_KEY] = $.getJsonString(qrycrit);
            if ($.isSetted(params[PIC_REQ_ACTION_KEY])) { postdata[PIC_REQ_ACTION_KEY] = params[PIC_REQ_ACTION_KEY]; }

            // 处理表单数据
            var frmdata;
            if (!params[PIC_FORM_DATA_KEY] && dt[PIC_FORM_DATA_KEY]) {
                frmdata = dt[PIC_FORM_DATA_KEY];
                delete (dt[PIC_FORM_DATA_KEY]);
            } else {
                frmdata = params[PIC_FORM_DATA_KEY];
            }

            if (typeof (frmdata) == "string") {
                postdata[PIC_FORM_DATA_KEY] = frmdata;
            } else if (typeof (frmdata) == "object") {
                postdata[PIC_FORM_DATA_KEY] = $.getJsonString(frmdata, true);
            }

            // 由于可能存在textarea编辑回车，需要两次转换
            if (postdata[PIC_FORM_DATA_KEY]) {
                postdata[PIC_FORM_DATA_KEY] = postdata[PIC_FORM_DATA_KEY];
            }

            // 提交的数据
            postdata[PIC_REQ_DATA_KEY] = $.getJsonString(dt, true) || "";
            if (postdata[PIC_REQ_DATA_KEY]) {
                postdata[PIC_REQ_DATA_KEY] = postdata[PIC_REQ_DATA_KEY];
            }

            // 简单编码处理，防止乱码
            for (var key in postdata) {
                if (typeof (postdata[key]) == 'string') {
                    var tpdt = postdata[key];
                    /*tpdt = tpdt.replace(/\xB7/g, "%B7"); //由于服务器端Request处理中点，解码时会变成问号
                    tpdt = tpdt.replace(/\xD7/g, "%D7"); //×
                    tpdt = tpdt.replace(/\xF7/g, "%F7"); //÷
                    tpdt = tpdt.replace(/\xB1/g, "%B1"); //±
                    tpdt = tpdt.replace(/\xB0/g, "%B0"); //°
                    tpdt = tpdt.replace(/\+/g, "%2B")//由于服务器端Request处理"+"解码时会变成空格*/
                    postdata[key] = tpdt;
                }
            }

            var request = {
                url: url,
                method: method,
                params: postdata,
                callback: params.callback,
                success: function (resp, opts) {
                    Ext.defer(function () {
                        if (params.nomask !== true) {
                            PICMsgBox.hide();
                        }

                        var flag = true,
                        respData = null;
                        if (resp && resp.responseText) { respData = $.getJsonObj(resp.responseText) || {}; }

                        if (params.onrequest === false) { flag = false; }
                        else if (params.onrequest) { flag = params.onrequest.call(this, respData, opts, resp); }

                        if (flag !== false && respData) {
                            if (respData[PIC_RESP_SEX_KEY]) {
                                respData.isexception = true;

                                // 安全异常处理
                                PICMsgBox.alert(respData[PIC_RESP_SEX_KEY]["Content"], null, function () {
                                    if (params.onsexception === false) { flag = false; }
                                    else if (params.onsexception) { flag = params.onsexception.call(this, respData[PIC_RESP_SEX_KEY], respData, opts, resp); }

                                    if (flag !== false) {
                                        window.location.reload();
                                    }
                                });
                            } else if (respData[PIC_RESP_EX_KEY]) {
                                respData.isexception = true;

                                if (params.onexception === false) { flag = false; }
                                else if (params.onexception) { flag = params.onexception.call(this, respData[PIC_RESP_EX_KEY], respData, opts, resp); }

                                if (flag !== false) {
                                    PICMsgBox.warn(respData[PIC_RESP_EX_KEY]["Content"], (respData[PIC_RESP_EX_KEY]["Title"] || "异常"), function () {
                                        if (params.afterexception === false) { flag = false; }
                                        else if (params.afterexception) { flag = params.afterexception.call(this, respData[PIC_RESP_EX_KEY], respData, opts, resp); }

                                        if (params.afterrequest === false) { flag = false; }
                                        else if (flag && params.afterrequest) { params.afterrequest.call(this, respData, opts, resp); }
                                    });
                                }
                            } else if (respData[PIC_RESP_MSG_KEY]) {
                                if (params.onmessage === false) { flag = false; }
                                else if (params.onmessage) { flag = params.onmessage.call(this, respData[PIC_RESP_SEX_KEY], respData, opts, resp); }

                                if (flag !== false) {
                                    PICMsgBox.show({
                                        msg: respData[PIC_RESP_MSG_KEY]["Content"],
                                        title: respData[PIC_RESP_MSG_KEY]["Title"] || "消息",
                                        buttons: Ext.Msg.OK,
                                        icon: Ext.Msg.INFO,
                                        fn: function () {
                                            if (params.aftermessage === false) { flag = false; }
                                            else if (params.aftermessage) { flag = params.aftermessage.call(this, respData[PIC_RESP_SEX_KEY], respData, opts, resp); }

                                            if (params.afterrequest === false) { flag = false; }
                                            else if (flag && params.afterrequest) { params.afterrequest.call(this, respData, opts, resp); }
                                        }
                                    });
                                }
                            } else {
                                if (params.onsuccess === false) { flag = false; }
                                else if (params.onsuccess) { flag = params.onsuccess.call(this, respData, opts, resp); }

                                if (params.afterrequest === false) { flag = false; }
                                else if (flag && params.afterrequest) { flag = params.afterrequest.call(this, respData, opts, resp); }
                            }
                        } else {
                            if (params.onsuccess === false) { flag = false; }
                            else if (params.onsuccess) { flag = params.onsuccess.call(this, respData, opts, resp); }

                            if (params.afterrequest === false) { flag = false; }
                            else if (flag && params.afterrequest) { flag = params.afterrequest.call(this, respData, opts, resp); }
                        }
                    }, 100);
                },
                failure: function (resp, opts) {
                    Ext.defer(function () {
                        if (params.nomask !== true) {
                            PICMsgBox.hide();
                        }

                        var flag = true;
                        if (params.onfailure === false) { flag = false; }
                        else if (params.onfailure) { flag = params.onfailure.call(this, resp, opts); }
                        debugger
                        if (flag !== false) {
                            Ext.Msg.show({
                                title: "错误",
                                msg: "提交数据出错",
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.ERROR,
                                fn: function () {
                                    if (params.afterfailure == false) { flag = false; }
                                    else if (params.afterfailure) { flag = params.afterfailure.call(this, resp, opts); }
                                }
                            });
                        } else {
                            if (params.afterfailure == false) { flag = false; }
                            else if (params.afterfailure) { flag = params.afterfailure.call(this, resp, opts); }
                        }
                    }, 100);
                }
            };

            return request;
        },

        request: function (params) {
            if (params.nomask !== true) {
                PICMsgBox.wait(params.masktext || null, params.maskmsg || null);
            }

            // 开始提交数据(利用Ext.ajax)
            var request = this.getRequest(params);
            Ext.Ajax.request(request);
        }
    }
} ());

PICDataAdapter = PIC.DataAdapter;

//------------------------PIC 查询适配器 结束------------------------//