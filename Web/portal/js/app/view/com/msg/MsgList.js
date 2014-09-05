Ext.define('PIC.view.com.msg.MsgList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.MsgListPage',

    requires: [
        'PIC.model.sys.msg.Message'
    ],

    mainPanel: null,

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.mode = $.getQueryString('mode', 'read');
        me.type = $.getQueryString('type');

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.sys.msg.Message',
            dsname: 'EntList',
            idProperty: 'MessageID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.mode = me.mode;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            exportTitle: '消息列表',
            formparams: { url: "MsgEdit.aspx", style: { width: 750, height: 600 } },
            tlitems: [{
                bttype: 'add',
                text: '新建',
                hidden: (me.type == "Sent") && !(me.mode == 'mgmt')
            }, {
                bttype: 'edit',
                hidden: !(me.type == "Draft") && !(me.mode == 'mgmt')
            }, {
                text: '发送',
                hidden: !(me.type == "Draft") && !(me.mode == 'mgmt'),
                iconCls: 'pic-icon-email-go',
                handler: function () { me.send(); }
            }, {
                text: '回复',
                hidden: !(me.type == "Received") && !(me.mode == 'mgmt'),
                iconCls: 'pic-icon-email-reply',
                handler: function () { me.reply(); }
            }, {
                text: '转发',
                hidden: (me.type == "Draft") && !(me.mode == 'mgmt'),
                iconCls: 'pic-icon-email-go2',
                handler: function () { me.forward(); }
            }, 'delete', '-', 'excel', '->', 'schfield', '-', 'cquery', '-', 'help'],
            schitems: [
                { fieldLabel: '主题', id: 'Subject', schopts: { qryopts: "{ mode: 'Like', field: 'Subject' }" } },
                { fieldLabel: '发送人', id: 'FromName', schopts: { qryopts: "{ mode: 'Like', field: 'FromName' }" } },
                { fieldLabel: '状态', id: 'Status', xtype: 'picenumselect', enumdata: PIC.MessageModel.StatusEnum, schopts: { qryopts: "{ mode: 'Eq', field: 'Status' }" } }
            ],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Subject', dataIndex: 'Subject', header: '主题', juncqry: true, flex: 1, renderer: function (val, p, rec) { return me.colRender(val, p, rec); }, sortable: true },
				{ id: 'col_FromName', dataIndex: 'FromName', header: '发送人', juncqry: true, width: 100, sortable: true, align: 'center' },
				{ id: 'col_ToNames', dataIndex: 'ToNames', header: '接收人', juncqry: true, width: 100, sortable: true, align: 'center' },
				{ id: 'col_Type', dataIndex: 'Type', header: '类型', enumdata: PIC.MessageModel.TypeEnum, width: 100, sortable: true, align: 'center' },
				{ id: 'col_SentDate', dataIndex: 'SentDate', header: '发送时间', width: 150, sortable: true, align: 'center' },
				{ id: 'col_CreatedDate', dataIndex: 'CreatedDate', header: '创建时间', width: 150, sortable: true, align: 'center' }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    // 链接渲染
    colRender: function (val, p, rec) {
        var me = this;
        var rtn = val;

        if (p && p.column) {
            switch (p.column.dataIndex) {
                case "Subject":
                    if ("New".equals(rec.get('Status')) && "Received".equals(rec.get('Type'))) {
                        val = "<b>" + val + "</b>"
                    }

                    rtn = "<a class='pic-ui-link' onclick='PICUtil.openDialog(\"MsgEdit.aspx?op=r&mode=" + me.mode + "&id=" + rec.get("MessageID") + "\", {}, { width: 750, height: 600 })'>" + val + "</a>";
                    break;
            }
        }

        return rtn;
    },

    send: function (rec, sendtype) {
        var me = this;

        rec = rec || me.mainPanel.getFirstSelection();

        if (!rec) {
            PICMsgBox.alert("请先选择要发送的消息！");
            return;
        }

        sendtype = sendtype || 'send';

        var params = { 'type': me.type, sendtype: sendtype, mode: me.mode };

        if (sendtype == 'forward' || sendtype == 'reply') {
            params.op = 'c';
            params.refId = rec.get('MessageID');
        } else {
            params.op = 'u';
            params.id = rec.get('MessageID');
        }

        me.mainPanel.openFormWin({ params: params });
    },

    forward: function (rec) {
        var me = this;

        me.send(rec, 'forward');
    },

    reply: function (rec) {
        var me = this;

        me.send(rec, 'reply');
    }
});