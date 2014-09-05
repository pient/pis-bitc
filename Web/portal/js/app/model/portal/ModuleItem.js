
Ext.define('PIC.model.portal.ModuleItem', {
    extend: 'Ext.data.Model',

    idProperty: 'ModuleID',

    fields: [
            { name: 'ModuleID', type: 'string' },
            { name: 'Code', type: 'string' },
            { name: 'Name', type: 'string' },
            { name: 'Type', type: 'string' },
            { name: 'ParentID', type: 'string' },
            { name: 'Path', type: 'string' },
            { name: 'PathLevel', type: 'string' },
            { name: 'IsLeaf', type: 'string' },
            { name: 'SortIndex', type: 'string' },
            { name: 'Url', type: 'string' },
            { name: 'Icon', type: 'string' },
            { name: 'Description', type: 'string' },
            { name: 'Status', type: 'string' },
            { name: 'IsSystem', type: 'string' },
            { name: 'IsEntityPage', type: 'string' }
    ]
});