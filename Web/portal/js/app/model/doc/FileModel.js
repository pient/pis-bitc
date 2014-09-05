Ext.define("PIC.model.doc.FileModel", {
    extend: "Ext.data.Model",
    fields: ['id', 'name', 'type', 'size', 'filestatus', 'percent', 'fid', 'istemp'],

    getFileData: function () {   // 转换为文件数据，供文件上传下载控件使用
        var me = this;
        var data = { id: me.get('fid'), name: me.get('name'), istemp: me.get('istemp') || true };
        data.fullname = data.id + "_" + data.name;

        return data;
    }
});