

Ext.ux.PICReimbFormPanel = Ext.extend(Ext.ux.PICFormPanel, {
    constructor: function (config) {
        config = config || {};
        config.region = config.region || 'center';
        config.title = config.title || '行政（月结）费用报销';
        config.autoHeight = !(config.autoHeight == false);
        config.bodyStyle = config.bodyStyle || 'padding: 5px';
        config.tbar = new Ext.ux.PICReimbFormToolbar();
        config.autoScroll = (config.autoScroll != false);

        config.items = config.items || [{
            layout: 'column',
            border: false,
            defaults: { border: false, anchor: '-20' },
            items: [{
                layout: 'column',
                defaults: { border: false },
                items: [{
                    columnWidth: .5,
                    layout: 'form',
                    defaults: { width: 150 },
                    items: [{
                        xtype: 'textfield',
                        name: 'Name',
                        fieldLabel: '名称'
                    }, {
                        xtype: 'datefield',
                        name: 'FeeDate',
                        fieldLabel: '日期'
                    }, {
                        xtype: 'piccombo',
                        enumdata: TypeEnum,
                        required: true,
                        name: 'Type',
                        fieldLabel: '类型'
                    }]
                }, {
                    columnWidth: .5,
                    layout: 'form',
                    defaults: { width: 150 },
                    items: [{
                        xtype: 'textfield',
                        name: 'Code',
                        fieldLabel: '编号'
                    }, {
                        xtype: 'textfield',
                        name: 'Fee',
                        fieldLabel: '费用'
                    }]
                }]
            }, {
                layout: 'column',
                defaults: { border: false },
                items: [{
                    layout: 'form',
                    items: [{
                        xtype: 'textarea',
                        name: 'Comments',
                        width: 500,
                        fieldLabel: '注释'
                    }]
                }, {
                    layout: 'form',
                    items: [{
                        xtype: 'textarea',
                        name: 'Attachments',
                        width: 500,
                        fieldLabel: '附件'
                    }]
                }]
            }]
        }, {
            xtype: 'panel',
            html: 'xxxxxxxxxxxx'
        }, {
            layout: 'column',
            border: false,
            defaults: { border: false, anchor: '-20' },
            items: [{
                layout: 'column',
                defaults: { border: false },
                items: [{
                    layout: 'column',
                    padding: '5 0 0 0',
                    defaults: { border: false },
                    items: [{
                        columnWidth: .5,
                        layout: 'form',
                        defaults: { width: 150 },
                        items: [{
                            xtype: 'textfield',
                            name: 'CreatorName',
                            fieldLabel: '编制人'
                        }]
                    }, {
                        columnWidth: .5,
                        layout: 'form',
                        defaults: { width: 150 },
                        items: [{
                            xtype: 'datefield',
                            name: 'CreatedDate',
                            fieldLabel: '编制时间'
                        }]
                    }]
                }]
            }]
        }, {
            xtype: 'panel',
            height: 80,
            border: false,
            html: '<hr />表单描述：行政（月结）费用报销'
        }];

        Ext.ux.PICReimbFormPanel.superclass.constructor.call(this, Ext.apply(config, {
        }));
    },

    initComponent: function () {
        Ext.ux.PICReimbFormPanel.superclass.initComponent.call(this);
    }
});

Ext.reg('picreimbformpanel', Ext.ux.PICReimbFormPanel);

Ext.ux.PICReimbFormToolbar = Ext.extend(Ext.ux.PICToolbar, {
    store: null,
    formpanel: null,

    constructor: function (config) {
        config = config || {};

        config.items = config.items || [{
            xtype: 'picsavebutton',
            id: 'btnSave'
            }, {
                iconCls: 'pic-icon-submit',
                picexecutable: true,
                text: '提交',
                id: 'btnSubmit'
            }, {
                iconCls: 'pic-icon-delete',
                picexecutable: true,
                text: '删除',
                hidden: (pgOperation == "c"),
                id: 'btnDelete'
            }, '-', 'cancel', '->', '-', {
                iconCls: 'pic-icon-time',
                text: '历史查看',
                id: 'btnHistory',
                handler: function () {
                    // OpenWin(HistoryPageUrl, '_blank', HistoryWinStyle);
                }
            }, {
                iconCls: 'pic-icon-arrow-round',
                text: '流程图',
                id: 'btnFlow',
                handler: function () {
                    // OpenWin(PicPageUrl, '_blank', PicWinStyle);
                }
            }, '-', 'help'
        ];

        Ext.ux.PICReimbFormToolbar.superclass.constructor.call(this, Ext.apply(config, {
        }));
    },

    initComponent: function () {
        Ext.ux.PICReimbFormToolbar.superclass.initComponent.call(this);
    }
});

Ext.reg('picreimbformtoolbar', Ext.ux.PICReimbFormToolbar);

Ext.ux.PICReimbItemGrid = Ext.extend(Ext.ux.grid.PICEditorGridPanel, {
    constructor: function (config) {
        config = config || {};

        Ext.ux.PICReimbItemGrid.superclass.constructor.call(this, Ext.apply(config, {
        }));
    },

    initComponent: function () {
        Ext.ux.PICReimbItemGrid.superclass.initComponent.call(this);
    }
});

Ext.reg('picreimbitemgrid', Ext.ux.PICReimbItemGrid);
