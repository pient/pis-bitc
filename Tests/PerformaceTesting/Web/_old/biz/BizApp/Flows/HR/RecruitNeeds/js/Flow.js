
var GenderEnum = { None: '不限', Male: '男', Female: '女' };
var PurposeEnum = { KDBZ: '扩大编制', CBRL: '储备人力', CZBC: '辞职补充', LSXY: '临时需要', OTHER: '其他原因' };
var ComputerSkillsEnum = { JT: '精通', LH: '良好', LD: '略懂' };
var LanguageReqEnum = { PTHJT: '普通话精通', PTHLH: '普通话良好' };
var ChannelEnum = { LB: '内部招聘', WB: '外部招聘' };

var HistoryWinParams = { url: "HistoryList.aspx", style: { width: 950, height: 500} };
var DiagramWinParams = { url: "FlowPicView.aspx", style: { width: 880, height: 700} };

var ItemGrid = null;

Ext.define('PIC.RecruitNeedsApyFormPanel', {
    extend: 'PIC.ExtFormPanel',
    alias: 'widget.picrecruitneedsapyformpanel',

    constructor: function (config) {
        var me = this;
        config = Ext.apply({
            title: '招聘需求申请',
            tbar: { xtype: 'picrecruitneedsapyformtoolbar' }
        }, config);

        config.items = [
            { fieldLabel: '标识', name: 'Id', hidden: true },
            { fieldLabel: '编号', name: 'Code', allowBlank: false },
            { fieldLabel: '名称', name: 'Name', allowBlank: false },
            { fieldLabel: '编制人', name: 'CreatorName', xtype: 'picuserselect', multiSelect: true, fieldMap: { fld_CreatorId: "UserID" } },
            { fieldLabel: '人员标识', id: 'fld_CreatorId', name: 'CreatorId', hidden: true },
            { fieldLabel: '部门', id: 'fld_Department', name: 'Department', xtype: 'picgroupselect' },
            { fieldLabel: '招聘岗位', name: 'Post', allowBlank: false },
            { fieldLabel: '招聘人数', name: 'RecruitNumber', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '定编人数', name: 'DelimitNumber', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '现有人数', name: 'CurrentNumber', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '最迟到岗日期', name: 'LastEate', xtype: 'picdatefield' },
            { fieldLabel: '招聘原因', name: 'Purpose', xtype: 'picenumselect', enumdata: PurposeEnum, multiSelect: true, allowBlank: false },
            { fieldLabel: '岗位职责及工作内容', name: 'Duty', xtype: 'pictextarea', flex: 2, allowBlank: false },
            { fieldLabel: '性别', name: 'Gender', xtype: 'picenumselect', enumdata: GenderEnum, allowBlank: false },
            { fieldLabel: '年龄', name: 'Age', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '学历', name: 'EdiHistory', allowBlank: false },
            { fieldLabel: '专业', name: 'Major', allowBlank: false },
            { fieldLabel: '最低工作年限', name: 'MinWorkYears', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '从事行业要求', name: 'IndustryReq', xtype: 'pictextarea', flex: 2, allowBlank: false },
            { fieldLabel: '计算机能力', name: 'ComputerSkills', xtype: 'picenumselect', enumdata: ComputerSkillsEnum, allowBlank: false },
            { fieldLabel: '语言种类及要求', name: 'LanguageReq', xtype: 'picenumselect', enumdata: LanguageReqEnum, allowBlank: false },
            { fieldLabel: '其他要求', name: 'OtherReq', xtype: 'pictextarea', flex: 2, allowBlank: false },
            { fieldLabel: '招聘渠道', name: 'Channel', xtype: 'picenumselect', enumdata: ChannelEnum, allowBlank: false },
            { fieldLabel: '预算招聘费用', name: 'Fee', xtype: 'picnumberfield', allowBlank: false },
            { fieldLabel: '招聘完成计划时间', name: 'PlanEndDate', xtype: 'picdatefield' },
            { fieldLabel: '附件', name: 'Attachments', xtype: 'picfilefield', flex: 2, maxCount: 2, allowBlank: false },
            { xtype: 'picformdescpanel', flex: 2, html: '注：招聘需求表' }
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    },

    submit: function (action, data, params) {
        params = Ext.apply({}, params);

        this.callParent([action, data, params]);
    }
});


Ext.define('PIC.RecruitNeedsApyFormToolbar', {
    extend: 'PIC.ExtFormToolbar',
    alias: 'widget.picrecruitneedsapyformtoolbar',
    constructor: function (config) {
        var me = this;

        config = Ext.apply({}, config);

        config.items = config.items || [{
            xtype: 'picsavebutton',
            name: 'btnSave',
            handler: function (obj) {
                ItemGrid.commit();
                me.formpanel.submit('update', {});
            }
        }, {
            iconCls: 'pic-icon-submit',
            picexecutable: true,
            text: '提交',
            name: 'btnSubmit',
            handler: function (obj) {
                var recs = ItemGrid.store.getRange();
                var typefield = me.formpanel.findField("Type");
                var type = typefield.getValue();

                me.formpanel.submit('submit', {});
            }
        }, {
            iconCls: 'pic-icon-delete',
            picexecutable: true,
            text: '删除',
            hidden: (pgOp == "c"),
            name: 'btnDelete',
            handler: function (opts) {
                me.formpanel.submit('delete', {});
            }
        }, '-', 'close', '->', '-', {
            iconCls: 'pic-icon-time',
            text: '历史查看',
            name: 'btnHistory',
            handler: function () {
                PICOpenDialog(HistoryWinParams);
            }
        }, {
            iconCls: 'pic-icon-arrow-round',
            text: '流程图',
            name: 'btnFlow',
            handler: function () {
                PICOpenDialog(DiagramWinParams);
            }
        }, '-', 'help'
        ];

        this.callParent([config]);
    },

    initComponent: function () {
        this.callParent(arguments);
    }
});
