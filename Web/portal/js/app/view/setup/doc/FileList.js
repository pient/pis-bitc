Ext.define('PIC.view.setup.doc.FileList', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.DocFileListPage',

    requires: [
        'PIC.model.doc.File'
    ],

    mainPanel: null,

    refId: "",

    constructor: function (config) {
        var me = this;
        me.refId = $.getQueryString({ ID: "did" });

        config = Ext.apply({
            layout: 'border',
            border: false
        }, config);

        me.dataStore = new Ext.create('PIC.PageGridStore', {
            model: 'PIC.model.doc.File',
            dsname: 'EntList',
            idProperty: 'FileID',
            picbeforeload: function (proxy, params, node, operation) {
                params.data.did = me.refId;
            }
        });

        me.mainPanel = Ext.create('PIC.PageGridPanel', {
            region: 'center',
            store: me.dataStore,
            border: false,
            formparams: { url: "FileEdit.aspx", style: { width: 650, height: 300 } },
            tlitems: ['-', {
                text: '上传',
                iconCls: 'pic-icon-upload',
                handler: function () { me.upload(); }
            }, 'delete',
                '->', 'schfield', '-', 'cquery', '-', 'help'],
            schcols: 2,
            schitems: [
                { fieldLabel: '名称', id: 'Name', schopts: { qryopts: "{ mode: 'Like', field: 'Name' }" } }
            ],
            columns: [
                { id: 'col_Id', dataIndex: 'Id', header: '标识', hidden: true },
				{ id: 'col_Name', dataIndex: 'Name', header: '名称', juncqry: true, filelink: true, flex: 1, sortable: true },
				{ id: 'col_SizeStr', dataIndex: 'SizeStr', header: '文件大小', width: 100, sortable: true },
				{ id: 'col_OwnerName', dataIndex: 'OwnerName', header: '上传人', width: 100, sortable: true },
				{ id: 'col_CreatedDate', dataIndex: 'CreatedDate', header: '上传时间', width: 150, sortable: true }
            ]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    reloadPage: function (did) {
        var me = this;

        me.refId = did;
        me.dataStore.load();
    },

    upload: function () {
        var me = this;

        if (!me.refId) {
            PICMsgBox.alert("请先选择对应文档目录");

            return false;
        }

        PICUtil.openUploadDialog({
            did: me.refId,
            sender: me,
            callback: 'onUploadCompleted'
        });
    },

    onUploadCompleted: function () {
        this.dataStore.reload();
    }
});