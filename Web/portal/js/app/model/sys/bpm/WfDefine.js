
Ext.define('PIC.model.sys.bpm.WfDefine', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.WfDefineModel',

    idProperty: 'DefineID',

    fields: [
		{ name: 'DefineID' },
		{ name: 'ApplicationID' },
		{ name: 'ModuleID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'Catalog' },
		{ name: 'Status' },
		{ name: 'SortIndex' },
		{ name: 'Config' },
		{ name: 'Data' },
		{ name: 'Memo' },
		{ name: 'Tag' },
		{ name: 'Version' },
		{ name: 'CreatorID' },
		{ name: 'CreatorName' },
		{ name: 'CreatedDate' },
		{ name: 'LastModifiedDate' }
    ],

    statics: {
        StatusEnum: { 'Enabled': "启用", 'Disabled': "停用" },
        OpinionsEnum: PICState["WfOpinionsEnum"] || {}, // 常用意见
        SubmitOpinionsEnum: [ ['Agree', "同意"], ['Received', "已收到通知"], ['Finished', "已处理"], ['Good', "很好"] ], // 提交意见
        RejectOpinionsEnum: [[ 'Rejected', "不同意" ]], // 驳回意见
        CatalogEnum: PICState["WfCatalogEnum"] || {},
        DefaultOpEnum: { 'Submit': "提交", 'Pass': "通过", 'Reject': "驳回" }
    }
});