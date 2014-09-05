
Ext.define('PIC.view.portal.Viewport', {
    extend: 'PIC.ExtViewport',
    alias: 'widget.picportalviewport',
    alternateClassName: ['PIC.PortalViewport', 'PICPortalViewport'],

    modules: null,
    basicCfg: null,

    funcPanel: null,
    menuPanel: null,
    contentPanel: null,
    mainPanel: null,
    footerPanel: null,

    constructor: function (config) {
        var me = this;
        PICPortal = me;

        me.modules = PICState["Modules"] || [];
        me.basicCfg = PICState["BasicCfg"] || [];

        if(me.basicCfg){
            var usrpic = me.basicCfg["Picture"];
            if (usrpic && usrpic["FileID"]) {
                var usrpicurl = PICUtil.getFileUrlPath(usrpic["FileID"]);
                $('.usr-pic').attr("src", usrpicurl);
            }
        }

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.menuPanel = Ext.create('PIC.view.portal.default.MenuPanel', {
            region: 'west',
            collapsible: true,
            collapseDirection: 'left',
            split: true,
            listeners: {
                afterrender: function () {
                    Ext.defer(function () {
                        // PIC.PortalViewport.LoadWorkbench();
                        PICPortalViewport.LoadAction();
                    }, 100);
                },
                cellclick: function (cmp, td, cellIdx, rec, tr, rowIdx, e, eOpts) {
                    me.loadPage(rec.get('Url'), rec.get('Name'));
                },
                select: function (cmp, rec, e, a) {
                    me.loadPage(rec.get('Url'), rec.get('Name'));
                }
            }
        });

        PICPortalContent = me.contentPanel = Ext.create('PIC.view.portal.default.ContentPanel', {
            region: 'center'
        });

        me.funcPanel = Ext.create("PIC.ExtPanel", {
            applyTo: 'func_bar',
            margins: '0 0 0 0',
            bodyStyle: "background:transparent; margin: 5px;",
            width: 350,
            border: false,
            contentEl: 'func_content'
        });

        me.mainPanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            layout: 'border',
            margins: '40 0 0 0',
            border: false,
            cls: 'empty',
            bodyStyle: 'background:#dce2e7;',
            items: [me.menuPanel, me.contentPanel]
        });

        me.footerPanel = Ext.create("PIC.ExtPanel", {
            region: 'south',
            margins: '0 0 0 0',
            cls: 'empty',
            border: false,
            bodyStyle: 'background:#f1f1f1',
            contentEl: 'footer_content'
        });

        config.items = [me.mainPanel, me.footerPanel]

        this.callParent([config]);
    },
    
    loadPage: function (url, name) {
        var me = this;

        var url = $.combineQueryUrl(url, PIC.PortalViewport.pageParams);

        me.contentPanel.loadPage(url, name);
    },

    statics: {
        pageParams: {},

        setPageParams: function (params) {
            PIC.PortalViewport.pageParams = params || {};
        },

        LoadMyProfile: function () {
            PICUtil.openMyProfileDialog();
        },

        LoadWorkbench: function (params) {
            PICPortalViewport.LoadByPath("//M_SYS_FAV/M_SYS_FAV_WORK", params);
        },

        LoadHome: function (params) {
            PICPortalViewport.LoadByPath("//M_SYS_FAV/M_SYS_FAV_HOME", params);
        },

        LoadMessage: function (params) {
            PICPortalViewport.LoadByPath("//M_SYS_FAV/M_SYS_FAV_MSG", params);
        },

        LoadAction: function (params) {
            PICPortalViewport.LoadByPath("//M_SYS_FAV/M_SYS_FAV_TASK", params);
        },

        // params将用作加载页面的额外参数
        LoadByPath: function (path, params, pathField) {
            pathField = pathField || "Code";

            if (params) {
                PICPortalViewport.setPageParams(params);
            }

            PICPortal.menuPanel.selectPath("");
            PICPortal.menuPanel.selectPath(path, pathField);

            PICPortalViewport.setPageParams(null);
        },

        LoadCalendar: function () {
            PICUtil.openCalendarDialog();
        },

        LoadHelp: function () {
            PICUtil.openHelpDialog();
        },

        RefreshTaskMsg: function () {
            PICUtil.ajaxRequest('msgtask', {
                nomask: true,

                afterrequest: function (respData, opts) {
                    $("#lbl_msgnew").html(respData.NewMsgCount);
                    $("#lbl_tasknew").html(respData.NewTaskCount);

                    $("#mitem_msgnew").html(respData.NewMsgCount);
                    $("#mitem_tasknew").html(respData.NewTaskCount);
                }
            });
        },

        DoRelogin: function () {
            Ext.defer(function () {
                location.href = '/portal/Unlogin.aspx?action=relogin';
            }, 200);
        },

        DoExit: function () {
            if (!PICPortal.exitting) {
                Ext.defer(function () {
                    location.href = '/portal/Unlogin.aspx?action=exit';
                }, 200);

                PICPortal.exitting = true;
            }
        },

        OnFuncClick: function (arg, params) {
            switch (arg) {
                case "msg":
                    onPgTimer();
                    PIC.PortalViewport.LoadMessage();
                    break;
                case "task":
                case "action":
                    onPgTimer();
                    PIC.PortalViewport.LoadAction();
                    break;
                case "calendar":
                    PIC.PortalViewport.LoadCalendar(params);
                    break;
                case "home":
                    PIC.PortalViewport.LoadHome();
                    break;
                case "workbench":
                    PIC.PortalViewport.LoadWorkbench();
                    break;
            }
        }
    }
});
