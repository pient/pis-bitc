Ext.define('PIC.view.com.usr.MyProfile', {
    extend: 'PIC.Page',
    alternateClassName: 'PIC.MyProfilePage',

    requires: [
        'PIC.view.com.usr.MyBasicInfoPanel'
    ],

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            layout: 'border'
        }, config);

        me.initData();

        me.basicForm = Ext.create("PIC.MyBasicInfoPanel", {
            basicData: me.basicData,
            configData: me.configData
        });

        var tabItems = [me.basicForm];

        me.tabPanel = Ext.create("PIC.ExtTabPanel", {
            region: 'center',
            activeTab: 0,
            border: false,
            width: document.body.offsetWidth - 5,
            items: tabItems
        });

        me.mainPanel = Ext.create("PIC.ExtPanel", {
            region: 'center',
            layout: 'border',
            border: false,
            tbar: ['-', {
                xtype: 'picsavebutton',
                handler: function () {
                    me.saveData();
                }
            }, '-', {
                text: '修改密码',
                iconCls: 'pic-icon-key',
                handler: function () {
                    PICUtil.changeUserPwd(PICState["UserInfo"]['UserID']);
                }
            }, '-', { xtype: 'picclosebutton' }, '->', '-', { xtype: 'pichelpbutton' }],
            items: [me.tabPanel]
        });

        me.items = me.mainPanel;

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);
    },

    initData: function () {
        var me = this;

        me.basicData = PICState["UserData"] || {};
        me.configData = PICState["ConfigData"] || {};
    },

    saveData: function () {
        var me = this;

        var email = me.basicForm.innerBasicForm.getFieldValue('Email');

        var picture = {
            FileID: me.basicForm.getFieldValue('PictureID'),
            Name: me.basicForm.getFieldValue('PictureName')
        };

        var signature = {
            FileID: me.basicForm.getFieldValue('SignatureID'),
            Name: me.basicForm.getFieldValue('SignatureName')
        };

        var bpmcfg = me.basicForm.innerBpmConfigForm.getValues();

        PICUtil.ajaxRequest('save', { }, {
            email: email,
            picture: picture,
            signature: signature,
            bpmcfg: bpmcfg
        });
    }
});