// 系统变量定义，非常重要！
PIC_PAGE_STATE_KEY = "__PAGESTATE";
PIC_PAGE_SCH_CRIT_KEY = "SearchCriterion";
PIC_QUERY_CRIT_KEY = "qrycrit";
PIC_ASYNC_REQ_KEY = "asyncreq";
PIC_FORM_DATA_KEY = "frmdata";
PIC_REQ_DATA_KEY = "reqdata";
PIC_REQ_ACTION_KEY = "reqaction";

PIC_RESP_SEX_KEY = "__SEXCEPTION";
PIC_RESP_EX_KEY = "__EXCEPTION";
PIC_RESP_MSG_KEY = "__MESSAGE";

PIC_PG_OPENER_CMP_KEY = "__cmpid";
PIC_APP_REQ_CODE = "__code";

PIC_LOCAL_BPM_FORM_NAME = 'PIC.local.BpmFormPanel';  // 系统默认流程表单对象名

var PICAppRootPath = "/portal";
var PICMdlRootPath = PICAppRootPath + "/Modules";
var PICComMdlRootPath = PICMdlRootPath + "/Common";

if (Ext && Ext.BLANK_IMAGE_URL) {
    Ext.BLANK_IMAGE_URL = PICAppRootPath + "/js/ext4/s.gif";
}

var ICON_IMG_BASE = PICAppRootPath + "/images/icons/";
var BLANK_PIC_URL = PICAppRootPath + "/images/portal/blank.gif";
var UPLOAD_SWF_URL = PICAppRootPath + '/js/lib/swfupload/swfupload.swf';

var MSGSEND_PAGE_URL = PICAppRootPath + "/CommonPages/Util/MsgSend.aspx";
var INFO_VIEWER_PAGE_URL = PICAppRootPath + "/CommonPages/Portal/InfoViewer.aspx";
var FLOT_CHART_PAGE_URL = PICAppRootPath + "/CommonPages/Chart/FlotChart.aspx";
var PORTAL_DATA_PAGE_URL = PICAppRootPath + "/CommonPages/Data/PageData.aspx";
var BIZ_DATA_PAGE_URL = "/biz/CommonPages/PageData.aspx";


//-----------------------------PIC ExtJs 配置信息 开始--------------------------//

var PIC = PIC || {};

PIC.ExtButtonDict = {};

PICPortal = null;
PICPage = null;

Ext.define('PICConfig', {
    statics: {
        AppPagePath: PICComMdlRootPath + "/App/App.aspx",
        HelpPagePath: PICComMdlRootPath + "/App/Help.aspx",
        CalendarPagePath: PICComMdlRootPath + "/App/Calendar/Calendar.htm",
        VersionPagePath: PICComMdlRootPath + "/App/Version.aspx",

        MyProfilePath: PICComMdlRootPath + '/Usr/MyProfile.aspx',

        UploadPagePath: PICComMdlRootPath + "/Doc/Upload.aspx",
        FilePagePath: PICComMdlRootPath + "/Doc/File.ashx",

        ImportPagePath: PICComMdlRootPath + "/Data/DataImport.aspx",
        ExportPagePath: PICComMdlRootPath + "/Data/DataExport.aspx",

        MsgListPagePath: PICComMdlRootPath + "/Msg/MsgList.aspx",
        MsgViewPagePath: PICComMdlRootPath + "/Msg/MsgEdit.aspx",
        PubMsgListPagePath: PICComMdlRootPath + "/Msg/PubMsgList.aspx",

        UserSelectPath: PICComMdlRootPath + '/Sel/UserSelect.aspx',
        GroupSelectPath: PICComMdlRootPath + '/Sel/GroupSelect.aspx',
        RoleSelectPath: PICComMdlRootPath + '/Sel/RoleSelect.aspx',
        AuthSelectPath: PICComMdlRootPath + '/Sel/AuthSelect.aspx',
        FuncSelectPath: PICComMdlRootPath + '/Sel/FuncSelect.aspx',
        EnumSelectPath: PICComMdlRootPath + '/Sel/EnumSelect.aspx',

        TmplViewPagePath: PICComMdlRootPath + '/Tmpl/TmplView.aspx',

        MsgPagePath: PICComMdlRootPath + '/Msg/MsgView.aspx',

        FlowBusPagePath: PICComMdlRootPath + '/Bpm/FlowBus.aspx',
        FlowPreviewPagePath: PICComMdlRootPath + '/Bpm/FlowPreview.aspx',
        MyBpmActionListPagePath: PICComMdlRootPath + '/Bpm/MyActionList.aspx',

        Init: function () {
            window.PICPortal = window.top.PICPortal;

            Ext.Loader.setConfig({
                enabled: true,
                paths: {
                    'PIC': '/portal/js/app',
                    'PIC.biz': '/biz/js/app'
                }
            });

            Ext.tip.QuickTipManager.init();

            Ext.Ajax.defaultHeaders = {
                'Accept': 'application/json'
            };
        }
    }
});

PICConfig.Init();

//-----------------------------PIC ExtJs 配置信息 结束--------------------------//

//-----------------------------PIC ExtJs 实用方法 开始--------------------------//

Ext.define('PICUtil', {
    statics: {
        ajaxRequest: function (action, params, data) {
            if (typeof action == "string") {
                params = params || {};
                params.data = Ext.applyIf((data || {}), params.data || {});
                params[PIC_REQ_ACTION_KEY] = params[PIC_REQ_ACTION_KEY] || action || 'default';
            } else {
                params = action;
            }

            PICDataAdapter.request(params);
        },

        setReqParams: function (params, reqaction, data) {
            params = params || {};

            if (typeof reqaction === "string") {
                params[PIC_REQ_ACTION_KEY] = reqaction;
            } else if ($.isPlainObject(reqaction)) {
                data = reqaction;
            }
            
            if ($.isPlainObject(data)) {
                params.data = params.data || {};
                Ext.apply(params.data, data || {});
            }

            return params;
        },

        getReqParams: function (reqaction, data) {
            var params = PICUtil.setReqParams({}, reqaction, data);

            return params;
        },

        getOpenerCmp: function () {
            // 获取与大开此窗口相关的控件
            var cmpid = $.getQueryString(PIC_PG_OPENER_CMP_KEY);
            var cmp = null;

            if (window.opener && window.opener.Ext) {
                cmp = window.opener.Ext.getCmp(cmpid);
            }

            return cmp;
        },

        getFirstCmp: function (selector, root) {
            var cmps = Ext.ComponentQuery.query(selector, root);

            if (cmps && cmps.length > 0) {
                return cmps[0];
            }

            return null;
        },

        addModelField: function (type, fld) {
            if (!(fld instanceof Ext.data.Field)) {
                fld = new Ext.data.Field(fld)
            }

            type.prototype.fields.add(fld);
        },

        removeModelField: function (type, fldname) {
            Ext.each(type.prototype.fields.items, function (f) {
                if (f.name === fldname) {
                    type.prototype.fields.remove(f);

                    return false;
                }
            });
        },

        getModelIds: function (recs, store) {
            if (!recs && store) {
                recs = recs = recs || store.getRange();
            }

            var ids = [];

            Ext.each(recs, function (rec) {
                ids.push(rec.getId());
            });

            return ids;
        },

        applyXTemplate: function (xtmpl, data) {
            var xtmpl = xtmpl;
            if (typeof xtmpl == "string") {
                xtmpl = new Ext.XTemplate(xtmpl);
            }

            var rtn = xtmpl.apply(data);
            return rtn;
        },

        renderFuncLink: function (data) {
            var t_params = (data.params || "");
            var clickfunc = "";

            if (typeof t_params === "object") {
                if (t_params.clickfunc) {
                    if (t_params.clickfunc.indexOf("(") > 0) {  // 已设置参数，则直接赋值
                        clickfunc = t_params.clickfunc;
                    } else {
                        if (t_params.params) {
                            if (t_params.params["id"]) {
                                clickfunc = t_params.clickfunc + '("' + t_params.params["id"] + '")';
                            }
                        }

                        if (!clickfunc) {
                            clickfunc = t_params.clickfunc + '()';
                        }
                    }
                } else {
                    clickfunc = "PICUtil.openDialog(" + Ext.encode(t_params) + ")";
                }
            }

            var rtn = new Ext.XTemplate(
                "<span class='pic-formpage-link' style='margin:2px; border:0px'>",
                "<a class='pic-ui-link' onclick='{clickfunc}'>{text}</a>",
                "</span>").apply({ text: data.text, clickfunc: clickfunc });

            return rtn;
        },

        renderFileLink: function (data) {
            if (typeof data == "string") {
                data = PICUtil.parseFileFullName(data);
            }

            data.url = data.url || $.combineQueryUrl(PICConfig.FilePagePath, { id: data.id });

            var rtn = new Ext.XTemplate(
            '<tpl for=".">',
                '<a class="pic-ctrl-file-link" style="cursor: hand; float:left; margin-left:2px; margin-right:2px; border:0px" title="{name}" href="{url}">{name}</a>',
            '</tpl>').apply(data);
            return rtn;
        },

        parseFileFullName: function (ffname) {
            var tffname = ffname.trimEnd(",");
            var fnames = ffname.split(",");
            var data = [];
            Ext.each(fnames, function (fname) {
                var id_pos_end = fname.indexOf("_");
                var id = fname.substring(0, id_pos_end);
                var name = fname.substring(id_pos_end + 1);
                data.push({ id: id, name: name, fullname: fname });
            });

            return data;
        },

        sleep: function (milliseconds) {
            var start = new Date().getTime();
            for (var i = 0; i < 1e7; i++) {
                if ((new Date().getTime() - start) > milliseconds) {
                    break;
                }
            }
        },

        //-----------------------------PIC ExtJs 窗口操作 开始--------------------------//

        openSelDialog: function (args, params, style) {
            PICUtil.openDialog(args, params, style);
        },

        openDialog: function (args, params, style, onProcessFinish) {
            if (typeof (args) == "string") {
                var url = args;
                args = { url: url, params: params, style: style, onProcessFinish: onProcessFinish };
            }

            var tstyle = PICUtil.getDialogStyle(args.style);
            var tparams = Ext.apply(args.params || {}, params || {});

            var senderid = tparams[PIC_PG_OPENER_CMP_KEY];

            if (!senderid) {
                var sender = args.sender || tparams.sender;

                if (sender) {
                    if (typeof (sender) == "string") {
                         senderid= sender;
                    } else if (sender.getId) {
                        senderid = sender.getId();
                    }
                }
            }

            if (senderid) {
                tparams[PIC_PG_OPENER_CMP_KEY] = senderid;
            }

            var turl = $.combineQueryUrl(args.url, tparams);

            var win = null;

            if (tparams.modalDialog == true) {
                win = PICUtil.openModelWin(turl, {}, tstyle, onProcessFinish);
            } else {
                // win = window.showModelessDialog(turl, window, tstyle);
                win = PICUtil.openWin(turl, args.target || "_blank", tstyle);

                if (win) {
                    if (typeof (args.onProcessFinish) == "function") {
                        
                        // args.onProcessFinish.call(win, args.params);
                    }
                }
            }

            return win;
        },

        openModelWin: function (url, params, style, onProcessFinish) {
            var ModelStyle = "dialogWidth:750px; dialogHeight:550px; scroll:yes; center:yes; status:no; resizable:yes;";

            params.rtntype = params.rtntype || "array";
            style = style || ModelStyle;
            url = $.combineQueryUrl(url, params);

            rtn = window.showModalDialog(url, window, style);

            if (rtn) {
                if (typeof (onProcessFinish) == "function") {
                    onProcessFinish.call(rtn, params);
                }
            }
        },

        openWin: function (url, target, style) {
            var desurl = url;
            var win = null;

            if (desurl == null || desurl == "") {
                return false;
            }

            // try { var target = eval(target); } catch (e) { }

            if (!target || target == "null") {
                window.location.href = desurl;
            } else if (typeof (target) == "object") {
                target.location.href = desurl;
                win = target;
            } else if (typeof (target) == "string") {
                if (!style) style = "compact";
                else {
                    if (style.indexOf('resizable') < 0) {
                        style += ",resizable=yes";
                    }
                    if (style.indexOf('location') < 0) {
                        style += ",location=no";
                    }
                }

                if (target == "")
                    target = "_SELF";
                if (target.toUpperCase() == "_BLANK") {
                    win = window.open(desurl, "", style);
                }
                else {
                    win = window.open(desurl, target, style);
                }
                if (!win) alert("弹出窗口被阻止！");
            }

            if (win) return win;
        },

        getDialogStyle: function (style) {
            style = style || { width: 750, height: 550, location: 'no' };

            if (typeof (style) != "string") {
                if (style.type === "dialog") {
                    style.dialogWidth = (style.dialogWidth || style.width || "750px").toString();
                    style.dialogHeight = (style.dialogHeight || style.height || "550px").toString();
                    if (style.dialogWidth.indexOf("px") < 0) style.dialogWidth += "px";
                    if (style.dialogHeight.indexOf("px") < 0) style.dialogHeight += "px";
                    style.scroll = style.scroll || "yes";
                    style.center = style.center || "yes";
                    style.status = style.status || "no";
                    style.resizable = style.resizable || "yes";

                    style = $.getStrByJsonObj(style, ":", ";");
                } else {
                    style.width = style.width || (window.screen.width / 2);
                    style.height = style.height || (window.screen.height / 2);
                    style.left = (window.screen.width - style.width) / 2;
                    style.top = (window.screen.height - style.height) / 2;

                    style = $.getStrByJsonObj(style, "=", ",");
                }
            }

            return style;
        },

        openLink: function (url) {
            if (Ext.isIE) {
                window.open(url, "_BLANK", "width=100, height=10, top=20, left=40, location=no");
            } else {
                window.location.href = url;
            }
        },

        invokeDgCallback: function (params, closeFlag) {
            var flag = true;
            var opener = window.opener || window.dialogArguments;

            if (opener && opener.pgDialogCallback) {
                flag = opener.pgDialogCallback(params);  // 调用opener页面回调函数
            }

            if (flag != false && closeFlag === true) {
                // 延迟关闭本页面
                Ext.defer(function () {
                    window.close();
                }, 100);
            }

            return flag;
        },

        invokePgCallback: function (params, closeFlag) {    // 采用统一模式调用callback
            params = params || {};

            var openerCmp = PICUtil.getOpenerCmp();
            var callback = $.getQueryString("callback", null);

            if (typeof (callback) == "string" && callback != "") {
                if (openerCmp) {
                    if (openerCmp[callback]) {
                        openerCmp[callback](params)
                    }
                } else if (window.opener) {
                    if (window.opener.PICPage) {
                        if (window.opener.PICPage[callback]) {
                            window.opener.PICPage[callback](params)
                        }
                    } else if (window.opener[callback]) {
                        window.opener[callback](params)
                    }
                }
            }

            if (closeFlag !== false) {
                window.close();
            }
        },

        loadScript: function (options) {
            Ext.Loader.loadScript(options);
        },

        execScript: function (script) {
            Ext.globalEval(script);
        },

        //-----------------------------PIC ExtJs 窗口操作 结束--------------------------//

        //-----------------------------PIC ExtJs 应用操作 开始--------------------------//

        // 获取页面主窗口
        getPortalViewPort: function () {
            var vp = null;
            var topwin = window;

            // 循环三次, 防止过多循环影响性能
            for (var i = 0; i < 3; i++) {
                vp = topwin.PICPortalViewport || topwin.top.PICPortalViewport;

                if (!vp && topwin.opener) {
                    topwin = topwin.opener;
                } else {
                    break;
                }
            }

            return vp;
        },

        verifyPwd: function (args, onsuccess, onfail) {
            var params = { data: args || {} };

            params.url = PICConfig.AppPagePath;

            params.afterrequest = function (respData, opts) {
                if (respData.result == 'success') {
                    if (typeof (onsuccess) === 'function') {
                        onsuccess();
                    }
                } else {
                    if (typeof (onfail) === 'function') {
                        onfail();
                    } else {
                        PICMsgBox.warn("密码错误！");

                        return false;
                    }
                }
            };

            if (args.nowindow != false) {
                PIC.PwdVerifyForm.show(function (val, frm) {
                    params.data.pwd = val;
                    PICUtil.execFunc('verifypwd', params);
                });
            } else {
                PICUtil.execFunc('verifypwd', params);
            }
        },

        changeUserPwd: function (usrid) {
            if (!usrid) {
                PICMsgBox.alert("请先指定要修改密码的用户！");

                return false;
            }

            var pwdChangWin = Ext.create('PIC.ExtFormWin', {
                title: "修改密码",
                width: 310,
                frmconfig: {
                    items: [
                    { name: 'oldPwd', fieldLabel: "原密码", inputType: 'password', allowBlank: false },
                    { name: 'newPwd', fieldLabel: "新密码", inputType: 'password', allowBlank: false },
                    { name: 'confirmPwd', fieldLabel: "确认密码", inputType: 'password', allowBlank: false }],
                    buttons: [{
                        text: "确定",
                        handler: function () {
                            if (pwdChangWin.isValid()) {
                                var oldPwd = pwdChangWin.getFieldValue('oldPwd');
                                var newPwd = pwdChangWin.getFieldValue('newPwd');
                                var confirmPwd = pwdChangWin.getFieldValue('confirmPwd');

                                if (newPwd === confirmPwd) {
                                    PICUtil.execFunc('chgpwd', {
                                        onsuccess: function (resp, opts) {
                                            PICMsgBox.alert("修改成功，请使用新密码！");
                                            pwdChangWin.hide();
                                        }
                                    }, {
                                        id: usrid,
                                        oldpwd: oldPwd,
                                        newpwd: newPwd
                                    });
                                } else {
                                    PICMsgBox.alert("密码不匹配！");
                                }
                            }
                        }
                    }, {
                        text: "取消",
                        handler: function () { pwdChangWin.hide(); }
                    }]
                },
                listeners: {
                    hide: function () {
                        pwdChangWin.reset();
                    }
                }
            });

            pwdChangWin.show();
        },

        execFunc: function (code, params, data) {
            PICUtil.execAppDataRequest('exec', code, params, data);
        },

        getTmplData: function (code, params, data) {
            PICUtil.execAppDataRequest('tmpl', code, params, data);
        },

        execAppDataRequest: function (action, code, params, data) {
            params = Ext.apply({
                nomask: true,
                url: PICConfig.AppPagePath
            }, params || {});

            data = data || {};
            data[PIC_APP_REQ_CODE] = code;

            // 获取应用数据
            PICUtil.ajaxRequest(action, params, data);
        },

        openMyProfileDialog: function (params, style) {
            PICUtil.openDialog(PICConfig.MyProfilePath, params, style || { height: 500, width: 700, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        openVersionDialog: function (params, style) {
            PICUtil.openDialog(PICConfig.VersionPagePath, params, style || { height: 600, width: 465, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        openHelpDialog: function (params, style) {
            PICUtil.openDialog(PICConfig.HelpPagePath, params, style || { height: 600, width: 465, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        openCalendarDialog: function (params) {
            PICUtil.openDialog(PICConfig.CalendarPagePath, params, { height: 450, width: 800, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        openUploadDialog: function (params, onProcessFinish) {
            var p = Ext.apply({}, params || {});

            var url = (p.url || PICConfig.UploadPagePath);
            var style = (p.style || { height: 350, width: 600, help: 0, resizable: 0, status: 0, scroll: 0 }); // 上传文件页面弹出样式

            PICUtil.openDialog({ url: url, params: params, style: style, onProcessFinish: onProcessFinish });
        },

        openFileDialog: function (fid, istemp) {
            var url = $.combineQueryUrl(PICConfig.FilePagePath, { id: fid, istemp: istemp || false });

            PICUtil.openLink(url);
        },

        // 打开模版查看也卖弄
        openTmplViewPage: function (params) {
            params = Ext.apply({ op: 'view' }, params || {});

            PICUtil.openDialog(PICConfig.TmplViewPagePath, params, { height: 500, width: 750, help: 1, resizable: 1, status: 1, scroll: 1 });
        },

        openMsgDialog: function (params) {
            params = Ext.apply({ op: 'view' }, params || {});

            PICUtil.openDialog(PICConfig.MsgPagePath, params, { height: 600, width: 750, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        openFlowBusDialog: function (params) {
            params = Ext.apply({ op: 'view' }, params || {});

            PICUtil.openDialog(PICConfig.FlowBusPagePath, params, { height: 550, width: 750, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        openFlowPreviewDialog: function (params) {
            var _params = {};

            if (typeof (params) == 'string') {
                _params.did = params;
            } else {
                _params = params || {};
            }

            _params = Ext.apply({ op: 'view' }, _params);
            
            PICUtil.openDialog(PICConfig.FlowPreviewPagePath, _params, { height: 600, width: 750, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        openFlowTrackingDialog: function (params) {
            params = Ext.apply({ op: 'view' }, params || {});

            PICUtil.openDialog(PICConfig.FlowTrackingPath, params, { height: 500, width: 750, help: 0, resizable: 0, status: 0, scroll: 0 });
        },

        exportFlowDoc: function (params) {
            var _params = params || {};

            function doExport() {
                var urlargs = Ext.apply({ reqaction: 'export', iid: _params.iid }, _params.tmpl || {});

                var url = $.combineQueryUrl(PICConfig.FlowBusPagePath, urlargs);
                PICUtil.openLink(url);
            }

            if (_params && _params.IsCheckAuth) {
                PICUtil.verifyPwd({}, doExport);
            } else {
                doExport();
            }
        },

        getAppFullPath: function (path) {
            if (path.indexOf(PICAppRootPath) == 0) {
                return path;
            }

            if (path.indexOf("/") !== 0) {
                path = "/" + path;
            }

            return PICAppRootPath + path;
        },

        getFileUrlPath: function (args, istemp) {
            var _args = {};
            var url = PICAppRootPath + "/images/thumbnail/def_usr.png";

            if (typeof (args) == "string") {
                _args = { fid: args, istemp: istemp || false }
            } else if (typeof (args) == "object") {
                _args = $.apply({ istemp: false }, args || {});
            }

            if (_args.fid || _args.id) {
                url = $.combineQueryUrl(PICConfig.FilePagePath, _args);
            }

            return url;
        },

        loadUserTip: function (el, userid, userTip, tmpl) {
            if (!userid) {
                return;
            }

            if (!userTip) {
                userTip = Ext.create('PIC.ExtQuickTip', {
                    renderTo: Ext.getBody(),
                    width: 200,
                    title: '用户信息：'
                });
            }

            if (!tmpl) {
                tmpl = new Ext.XTemplate(
                    "<span style='margin:2px; border:0px'>",
                    "姓名：{Name}<br/>",
                    "工号：{WorkNo}<br/>",
                    "部门：{DeptName}<br/>",
                    "邮件：{Email}<br/>",
                    "</span>");
            }

            userTip.showBy(el);
            userTip.setLoading("加载中...");

            PICUtil.getTmplData('renderstr', {
                onsuccess: function (respData, opts, resp) {
                    userTip.setLoading(false);

                    if (respData && respData.User && respData.User.length > 0) {
                        var htmlstr = tmpl.apply(respData.User[0]);
                        userTip.update(htmlstr);
                    } else {
                        userTip.update("暂无...");
                    }
                }
            }, { tcode: 'Sys.Data.UserInfo', ctxparams: { id: userid } });
        },

        //-----------------------------PIC ExtJs 应用操作 结束--------------------------//

        //-----------------------------PIC ExtJs 按钮操作 开始--------------------------//

        // Ext findByType 不支持按钮, 这里是查找按钮方法
        findButtonsBase: function (c, params, array) {
            array = array || [];

            var bttype, xtype;

            if (typeof (params) == 'string') {
                bttype = params;
            } else {
                bttype = params.bttype;
                xtype = params.xtype;
            }

            if (c.toolbars) {
                Ext.each(c.toolbars, function (i) {
                    var j = i.items.items;
                    Ext.each(j, function (k) {
                        if (bttype) {
                            if (k.bttype == bttype) {
                                array.push(k);
                            }
                        } else if (xtype) {
                            if (k.xtype == xtype) {
                                array.push(k);
                            }
                        } else if (k.xtype == 'button' || k.type == 'button') {
                            array.push(k);
                        }
                    })
                });
            }

            if (c.items && c.items.items) {
                var i = c.items.items;
                Ext.each(i, function (k) {
                    if (bttype) {
                        if (k.bttype == bttype) {
                            array.push(k);
                        }
                    } else if (xtype) {
                        if (k.xtype == xtype) {
                            array.push(k);
                        }
                    } else if (k.xtype == 'button' || k.type == 'button') {
                        array.push(k);
                    }
                });
            }

            return array;
        },

        findFirstButton: function (c, params) {
            var btns = PICUtil.findButtons(c, params);

            if (btns != null && btns.length > 0) {
                return btns[0];
            }

            return null;
        },

        // 获取当前容器，以及下级所有容器的所有按钮（递归）
        findButtons: function (c, params, array) {
            array = array || [];

            //如果c不是叶子节点
            if ((c.items && c.items.items)) {
                PICUtil.findButtonsBase(c, params, array);

                if (c.items && c.items.items) {
                    Ext.each(c.items.items, function (i) {
                        PICUtil.findButtons(i, params, array);
                    });
                }
            }

            return array;
        },

        regPICBtType: function (bttype, type, xtype, config) {
            config = Ext.apply({
                type: type,
                xtype: xtype,
                bttype: bttype,
                picexecutable: false
            }, config);

            if ('string' == typeof (bttype)) {
                if (PIC.ExtButtonDict[bttype]) { throw "bttype已注册！"; }
                else { PIC.ExtButtonDict[bttype] = config; }
            }
        },

        getPICButtonInfoByBtType: function (bttype, field) {
            if (!field) { return PIC.ExtButtonDict[bttype]; }
            else if (PIC.ExtButtonDict[bttype]) {
                return PIC.ExtButtonDict[bttype][field];
            } else {
                return null;
            }
        },

        definePICButton: function (type, xtype, bttype, config) {
            var defname = ('PIC.' + type);

            config = Ext.apply({
                extend: 'PIC.ExtButton',
                alias: ('widget.' + xtype),
                type: type,
                bttype: bttype,
                picexecutable: false,
                text: '',
                iconCls: '',
                name: defname
            }, config);


            PICUtil.regPICBtType(bttype, type, xtype, config);

            Ext.define(defname, config);
        }

        //-----------------------------PIC ExtJs 按钮操作 结束--------------------------//
    }
});

//-----------------------------PIC ExtJs 实用方法 结束--------------------------//