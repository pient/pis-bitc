
Ext.define('PIC.model.sys.org.User', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.UserModel',

    idProperty: 'UserID',

    fields: [
		{ name: 'UserID' },
		{ name: 'LoginName' },
		{ name: 'WorkNo' },
		{ name: 'Name' },
		{ name: 'DeptID' },
		{ name: 'DeptCode' },
		{ name: 'DeptName' },
		{ name: 'Email' },
		{ name: 'Remark' },
		{ name: 'Status' },
		{ name: 'ReportTo' },
		{ name: 'ReportToName' },
		{ name: 'Tag' },
		{ name: 'LastLogIP' },
		{ name: 'LastLogDate' },
		{ name: 'SortIndex' },
		{ name: 'LastModifiedDate' },
		{ name: 'CreateDate' }
    ],

    statics: {
        StatusEnum: { 1: "启用", 0: "停用" }
    }
});