Ext.define('PIC.view.com.bpm.FlowApproveFormPanel', {
    extend: 'PIC.BpmFormPanel',
    alternateClassName: 'PIC.Bpm.FlowApproveFormPanel',

    requires: [
        'PIC.model.sys.bpm.WfInstance',
        'PIC.model.sys.bpm.WfDefine'
    ],

    busPage: null,
    routeEnum: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            op: pgOperation,
            title: '审批',
            fieldsetTitle: '审批',
            height: 220
        }, config);

        me.busPage = config.busPage;

        me.routeEnum = me.busPage.getRouteEnum(me.busPage);
        me.routeRadioItems = [];
        for (var _key in me.routeEnum) {
            me.routeRadioItems.push({
                "boxLabel": me.routeEnum[_key],
                "name": "RouteCode",
                "inputValue": _key
            });
        }

        /* me.routeComboBox = Ext.create('PIC.ExtEnumSelect', {
            fieldLabel: '下一步', name: 'RouteCode', xtype: 'picenumselect', enumdata: me.routeEnum,
            listeners: {
                'change': function () {
                    me.onRouteCodeChange(this);
                }
            }, allowBlank: false
        }); */

        me.routeRadioGroup = Ext.create('PIC.ExtRadioGroup', {
            fieldLabel: '下一步' + PIC.ExtFieldBase.requiredTag,
            name: 'RouteCodeGroup',
            columns: 5,
            flex: 2,
            items: me.routeRadioItems,
            listeners: {
                'change': function () {
                    me.onRouteCodeChange(this);
                }
            }, allowBlank: false
        });

        me.opinionComboBox = Ext.create('PIC.ExtEnumSelect', {
            fieldLabel: '常用意见', name: 'CommonOpinion', xtype: 'picenumselect', enumdata: PIC.WfDefineModel.OpinionsEnum,
            listeners: {
                'change': function () {
                    me.onOpinionChange(this);
                }
            }
        });

        config.items = [
                me.routeRadioGroup,
                { xtype: 'container', name: 'ActorContainer', flex: .8, padding: '5 5 5 5' },
                { xtype: 'container', flex: 2 },
                me.opinionComboBox,
                { fieldLabel: '处理意见', name: 'Comments', xtype: 'pictextarea', flex: 2, allowBlank: false },
                {
                    xtype: 'container',
                    flex: 2,
                    layout: {
                        type: 'table',
                        columns: 4,
                        tdAttrs: { style: 'padding: 5px 10px;' }
                    },
                    items: [{ xtype: 'component', width: 70 }, {
                            xtype: 'picsubmitbutton',
                            text: '提交',
                            hidden: !me.busPage.displaySubmit,
                            handler: function () {
                                me.busPage.doSubmit();
                            }
                        },
                        { xtype: 'picclosebutton' }
                    ]
                },
                { fieldLabel: '路径名称', name: 'RouteName', hidden: true },
                { fieldLabel: '目标编号', name: 'TargetCode', hidden: true },
                { fieldLabel: '目标名称', name: 'TargetName', hidden: true },
                { fieldLabel: '活动标识', name: 'ActionID', hidden: true, value: me.busPage.aid || '' }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    onRouteCodeChange: function (f) {
        var me = this;
        var routeCode = f.getValue();
        var routeName = f.getDisplayValue();

        me.setFieldValue('RouteName', routeName);

        var tstate = me.busPage.getNextTaskState(routeCode);

        var actorContainer = PICUtil.getFirstCmp("[name=ActorContainer]", me);

        // 重新加载意见
        var opinionEnumStore = me.opinionComboBox.getStore();

        var enumData = {};
        
        if ("Submit".equals(routeCode)) {
            opinionEnumStore.removeAll();
            enumData = PIC.WfDefineModel.SubmitOpinionsEnum;
        } else if ("Reject".equals(routeCode)) {
            opinionEnumStore.removeAll();
            enumData = PIC.WfDefineModel.RejectOpinionsEnum;
        }

        for (var key in enumData) {
            opinionEnumStore.loadData(enumData);
        }
        
        if (tstate) {
            me.setFieldValue('TargetCode', tstate['TaskCode']);
            me.setFieldValue('TargetName', tstate['Name']);

            var actorHtml = PICUtil.renderFuncLink({
                text: tstate['Name'],
                params: { clickfunc: 'window.PICPage.approveForm.onQueryActorUsersTip(this, "' + tstate['TaskCode'] + '")' }
            })

            actorContainer.update(actorHtml);
        } else {
            actorContainer.update("待定...");
        }
    },

    onOpinionChange: function (f) {
        var me = this;
        var commentsField = me.getFieldByName('Comments');
        var opinionCode = f.getValue();
        var opinion = f.getRawValue();

        var comments = commentsField.getValue();

        if (!comments || comments.length < 10) {
            commentsField.setValue(opinion);
        }
    },

    onQueryActorUsersTip: function (el, taskCode) {
        var me = this;

        if (!taskCode) return;

        if (!me.actorTip) {
            me.actorTip = Ext.create('PIC.ExtQuickTip', {
                renderTo: Ext.getBody(),
                width: 200,
                title: '审批人信息：'
            });
        }

        me.actorTip.showBy(el);
        me.actorTip.setLoading("加载中...");

        PICUtil.ajaxRequest('qryactorusers', {
            nomask: true,
            onsuccess: function (respData, opts) {
                me.actorTip.setLoading(false);

                var usrList = respData['ActorUserList'];

                if (usrList.length > 0) {
                    var htmlstr = me.getActorUsersTipHtml(usrList);
                    me.actorTip.update(htmlstr);
                } else {
                    me.actorTip.update("待定...");
                }
            }
        }, { taskCode: taskCode, iid: me.busPage.iid });
    },

    getActorUsersTipHtml: function (usrList) {
        var rtn = new Ext.XTemplate(
            '<tpl for=".">',
                "<span style='margin:2px; border:0px'>",
                "{Name}; 工号：{WorkNo}",
                "</span><br/>",
            '</tpl>').apply(usrList);

        return rtn;
    }
});