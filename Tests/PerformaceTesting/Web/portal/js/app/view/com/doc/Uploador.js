Ext.define('PIC.view.com.doc.Uploador', {
    extend: 'PIC.Page',
    alias: 'widget.picupoloador',

    alternateClassName: 'PIC.UploadorPage',

    mode: null,
    fileTypes: null,
    dirId: null,

    requires: [
        'PIC.model.doc.File',
        'PIC.ctrl.doc.UploadPanel'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
        }, config);
        
        me.mode = config.mode || $.getQueryString("mode", "multi");
        me.fileTypes = $.getQueryString("fileTypes", '*.*');
        me.fileTypesDesc = $.getQueryString("fileTypesDesc");
        me.dirId = $.getQueryString("dirid") || $.getQueryString("did");

        me.mainPanel = Ext.create('PIC.ctrl.doc.UploadPanel', {
            region: 'center',
            border: false,
            filePostName: 'uploads', //后台接收参数
            fileTypes: me.fileTypes,//可上传文件类型
            fileTypesDesc: me.fileTypesDesc,//可上传文件类型
            fileUploadLimit: (me.mode === 'single' ? '1' : '0'),
            postParams: PICUtil.setReqParams({ dirid: me.dirId }, 'upload'), //http请求附带的参数
            onUploadCompleted: function (store) {
                var recs = store.getRange();

                // 为兼容到IE6，这里需要重新构建参数
                var fdata = [];

                Ext.each(recs, function (rec) {
                    fdata.push(rec.getFileData());
                });

                PICUtil.invokePgCallback({ data: fdata }, true);
            }
        });

        me.items = [me.mainPanel];

        this.callParent([config]);
    }
});