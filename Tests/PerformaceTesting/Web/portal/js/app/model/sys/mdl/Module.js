
Ext.define('PIC.model.sys.mdl.Module', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.ModuleModel',

    idProperty: 'ModuleID',

    fields: [
		{ name: 'ModuleID' },
		{ name: 'Code' },
		{ name: 'Name' },
		{ name: 'Type' },
		{ name: 'ParentID' },
		{ name: 'Path' },
		{ name: 'PathLevel' },
		{ name: 'IsLeaf' },
		{ name: 'SortIndex' },
		{ name: 'Url' },
		{ name: 'Icon' },
		{ name: 'Description' },
		{ name: 'Status', type: 'string' },
		{ name: 'IsSystem' },
		{ name: 'IsEntityPage' },
		{ name: 'IsQuickSearch' },
		{ name: 'IsQuickCreate' },
		{ name: 'IsRecyclable' },
		{ name: 'EditPageUrl' },
		{ name: 'AfterEditScript' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreateDate' },
        { name: 'leaf',
            convert: function (newValue, model) {
                return model.get("IsLeaf");
            }
        }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" },
        TypeEnum: PICState["MdlTypeEnum"] || {}
    }
});