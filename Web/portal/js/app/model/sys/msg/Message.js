
Ext.define('PIC.model.sys.msg.Message', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.MessageModel',

    idProperty: 'MessageID',

    fields: [
		{ name: 'MessageID' },
		{ name: 'Type' },
		{ name: 'SysType' },
		{ name: 'Catalog' },
		{ name: 'Important' },
		{ name: 'Subject' },
		{ name: 'Content' },
		{ name: 'MessageChain' },
		{ name: 'Attachments' },
		{ name: 'OwnerID' },
		{ name: 'OwnerName' },
		{ name: 'OwnerType' },
		{ name: 'FromID' },
		{ name: 'FromName' },
		{ name: 'ToIDs' },
		{ name: 'ToNames' },
		{ name: 'CCIDs' },
		{ name: 'CCNames' },
		{ name: 'Status' },
		{ name: 'Tag' },
		{ name: 'ReadedCount' },
		{ name: 'ExpiredDate' },
		{ name: 'SentDate' },
		{ name: 'CreatedDate' },
		{ name: 'LastModifiedDate' }
    ],

    statics: {
        StatusEnum: PICState["MsgStatusEnum"] || {},
        TypeEnum: PICState["MsgTypeEnum"] || {},
        ImportantEnum: PICState["MsgImportantEnum"] || {},
        PubInfoCatalogEnum: PICState["PubInfoCatalogEnum"] || {},
        PubInfoStatusEnum: PICState["PubInfoStatusEnum"] || {}
    }
});