// 系统常用枚举

Ext.define('PIC.model.com.Enums', {
    extend: 'Ext.data.Model',
    alternateClassName: 'PIC.Enums',

    statics: {
        GenderEnum: { 'M': "男", 'F': "女" },
        EnableStatusEnum: { 'Enabled': '有效', 'Disabled': '无效' }
    }
});