Ext.define('PIC.ctrl.bpm.FlowFormPreviewWin', {
	extend: 'Ext.window.Window',

	owner: null,
	action: null,

	constructor: function (config) {
		config = Ext.apply({
			closeAction: 'hide',
			resizable: true,
			layout: 'fit',
			maximizable: true,
			closable: true,
			bodyStyle: "padding:10px",

			definecode: null,
			formurl: null,
			formscript: null
		}, config);

		this.callParent([config]);
	},

	initComponent: function () {
		var me = this;

		if (me.formscript) {
			PICUtil.execScript(me.formscript);
		} else {
		    if (!formurl && me.definecode) {
		        var params = PICUtil.getReqParams('qrytext', {
		            dcode: me.definecode
		        });
		        params[PIC_APP_REQ_CODE] = "wfformdef";

		        me.formurl = $.combineQueryUrl(PICConfig.AppPagePath, params);
		    }

		    if (me.formurl) {
		        PICUtil.loadScript({
		            url: me.formurl
		        });
		    }
		}

		me.items = Ext.create(PIC_LOCAL_BPM_FORM_NAME, {
		});

		this.callParent(arguments);
	},

	getForm: function () {
		var me = this;
		var forms = me.query('form');

		if (forms.length > 0) {
			return forms[0];
		}

		return null;
	},

	isValid: function () {
		return this.getForm().isValid();
	}
});