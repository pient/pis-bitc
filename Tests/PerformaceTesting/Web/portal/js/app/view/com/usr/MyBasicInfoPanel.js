Ext.define('PIC.view.com.usr.MyBasicInfoPanel', {
    extend: 'PIC.ExtFormPanel',
    alternateClassName: 'PIC.MyBasicInfoPanel',

    requires: [
        'PIC.model.sys.org.User',
        'PIC.ctrl.sel.UserSelector'
    ],

    defaultUserPicture: '/portal/images/thumbnail/def_usr.png',
    defaultSignaturePicture: '/portal/images/thumbnail/def_sign.png',

    basicData: null,
    configData: null,

    ispicturetemp: false,   // 图片是否为临时文件

    constructor: function (config) {
        var me = this;

        config = Ext.apply({
            title: '个人信息',
            columns: 2,
            autolayout: false,
            fieldset: false,
            labelWidth: 60
        }, config);

        me.basicData = me.basicData || config.basicData;
        me.configData = me.configData || config.configData;

        me.innerBasicForm = Ext.create('PIC.ExtFormPanel', {
            columns: 1,
            fieldset: false,
            frmdata: me.basicData,
            items: [
                { fieldLabel: '姓名', name: 'Name', readOnly: true },
                { fieldLabel: '登录名', name: 'LoginName', readOnly: true },
                { fieldLabel: '工号', name: 'WorkNo', readOnly: true },
                { fieldLabel: '部门', name: 'DeptName', readOnly: true },
                { fieldLabel: '上级', name: 'ReportToName', readOnly: true },
                { fieldLabel: '状态', name: 'Status', xtype: 'picenumselect', enumdata: PIC.UserModel.StatusEnum, enumvaltype: "int", readOnly: true },
                { fieldLabel: '邮件', name: 'Email' },
                { fieldLabel: '标识', name: 'UserID', hidden: true }
            ]
        });

        me.innerBpmConfigForm = Ext.create('PIC.ExtFormPanel', {
            frmdata: me.configData["Bpm"] || {},
            columns: 3,
            fieldset: false,
            labelWidth: 60,
            items: [
                { fieldLabel: '代理人', name: 'AgentName', xtype: 'picuserselector', fieldMap: { fld_AgentID: "UserID" } },
                { fieldLabel: '开始日期', name: 'AgentStartDate', xtype: 'picdatefield' },
                { fieldLabel: '结束日期', name: 'AgentEndDate', xtype: 'picdatefield' },
                { fieldLabel: '代理人标识', name: 'AgentID', id: 'fld_AgentID', hidden: true }
            ]
        });

        me.innerSignatureConfigForm = Ext.create('PIC.ExtFormPanel', {
            frmdata: me.configData["Signature"] || {},
            fieldset: false,
            labelWidth: 60,
            items: [{
                    xtype: 'container',
                    height: 60,
                    flex:2,
                    html: '<div align="center"><input style="float:left;" type="button" onclick="window.PICPage.basicForm.changeSignature()" value="上传签名 (180x80)" />'
                        + '<img id="imgSignaturePic" src="' + me.defaultSignaturePicture + '" style="float:left; height:50px;" /></div>'
                }
            ]
        });

        config.items = [{
            xtype: 'fieldset',
            title: '<b style="font-size:12">基本信息</b>',
            layout: 'column',
            items: [{
                xtype: 'container',
                height: 200,
                columnWidth: .4,
                html: '<div align="center"><img id="imgUserPic" src="' + me.defaultUserPicture + '" height="150px;" /></div>'
                    + '<div align="center" style="padding:10px;"><input type="button" onclick="window.PICPage.basicForm.changePicture()" value="更换照片 (180x180)"></div>'
            }, {
                xtype: 'container',
                columnWidth: .6,
                items: me.innerBasicForm
            }]
        }, {
            xtype: 'fieldset',
            flex: 2,
            title: '<b style="font-size:12">签名</b>',
            items: {
                xtype: 'container',
                items: me.innerSignatureConfigForm
            }
        }, {
            xtype: 'fieldset',
            flex: 2,
            title: '<b style="font-size:12">流程配置</b>',
            items: {
                xtype: 'container',
                items: me.innerBpmConfigForm
            }
        }, {
            fieldLabel: '照片',
            name: 'PictureID',
            xtype: 'pictextfield',
            hidden: true,
            listeners: {
                'change': function (f, newValue, oldValue) {
                    var url = PICUtil.getFileUrlPath(newValue, me.ispicturetemp) || me.defaultUserPicture;

                    $("#imgUserPic").attr('src', url);
                }
            }
        }, {
            fieldLabel: '照片', name: 'PictureName', xtype: 'pictextfield', hidden: true
        }, {
            fieldLabel: '签名',
            name: 'SignatureID',
            xtype: 'pictextfield',
            hidden: true,
            listeners: {
                'change': function (f, newValue, oldValue) {
                    var url = PICUtil.getFileUrlPath(newValue, me.issignaturetemp) || me.defaultSignaturePicture;

                    $("#imgSignaturePic").attr('src', url);
                }
            }
        }, {
            fieldLabel: '照片', name: 'SignatureName', xtype: 'pictextfield', hidden: true
        }];

        this.callParent([config]);
    },

    initComponent: function () {
        var me = this;

        this.callParent(arguments);

        me.on('afterrender', function () {
            if (me.configData['Basic']) {
                if (me.configData['Basic']['Picture']) {
                    var pic = me.configData['Basic']['Picture'];
                    me.setFieldValue('PictureID', pic.FileID);
                    me.setFieldValue('PictureName', pic.Name);
                }

                if (me.configData['Basic']['Signature']) {
                    var sign = me.configData['Basic']['Signature'];
                    me.setFieldValue('SignatureID', sign.FileID);
                    me.setFieldValue('SignatureName', sign.Name);
                }
            }

            me.doLayout();
        });
    },

    changePicture: function () {
        PICUtil.openUploadDialog({
            mode: 'single',
            sender: this,
            fileTypes: '*.jpg;*.jpeg;*.gif;*.png;*.bmp',
            fileTypesDesc: '图片文件',
            callback: 'onAfterPictureUploaded'
        });
    },

    changeSignature: function () {
        PICUtil.openUploadDialog({
            mode: 'single',
            sender: this,
            fileTypes: '*.jpg;*.jpeg;*.gif;*.png;*.bmp',
            fileTypesDesc: '图片文件',
            callback: 'onAfterSignatureUploaded'
        });
    },

    onAfterPictureUploaded: function (args) {
        var me = this;

        if (args.data.length > 0) {
            var picobj = args.data[0];

            me.ispicturetemp = true;
            me.setFieldValue('PictureID', picobj["id"] || "");
            me.setFieldValue('PictureName', picobj["name"] || "");
        }
    },

    onAfterSignatureUploaded: function (args) {
        var me = this;

        if (args.data.length > 0) {
            var picobj = args.data[0];

            me.issignaturetemp = true;
            me.setFieldValue('SignatureID', picobj["id"] || "");
            me.setFieldValue('SignatureName', picobj["name"] || "");
        }
    }
});