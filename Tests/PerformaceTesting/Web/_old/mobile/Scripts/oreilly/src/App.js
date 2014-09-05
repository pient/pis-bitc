oreilly.cfg = {};

oreilly.OfflineStore = new Ext.data.Store({
    model: 'OfflineData',
    autoLoad: true
});

oreilly.SpeakerStore = new Ext.data.Store({
    model: 'Speaker',
    
    getGroupString: function(r){
        return r.get('last_name')[0]
    }
});

oreilly.App = Ext.extend(Ext.TabPanel, {
    
    fullscreen: true,
    
    tabBar: {
        ui: 'gray',
        dock: 'bottom',
        layout: { pack: 'center' }
    },
    
    cardSwitchAnimation: false,
    
    initComponent: function() {

        if (navigator.onLine) {
            this.items = [{
                title: '最新消息',
                iconCls: 'chat',
                xtype: 'tweetlist',
                hashtag: '最新消息'
            }, {
                title: '预约保养',
                iconCls: 'locate',
                xtype: 'location',
                coords: this.gmapCoords,
                mapText: this.gmapText,
                permLink: this.gmapLink,
            }, {
                title: '救援服务',
                iconCls: 'locate',
                xtype: 'location',
                coords: this.gmapCoords,
                mapText: this.gmapText,
                permLink: this.gmapLink,
            }, {
                title: '地图',
                iconCls: 'locate',
                xtype: 'location',
                coords: this.gmapCoords,
                mapText: this.gmapText,
                permLink: this.gmapLink,
            }, {
                title: '会员中心',
                iconCls: 'team1',
                xtype: 'speakerlist'
            }, {
                title: '关于',
                iconCls: 'info',
                xtype: 'aboutlist'
            }];
        } else {
            this.on('render', function(){
                this.el.mask('No internet connection.');
            }, this);
        }
        
        oreilly.cfg = {};
        oreilly.cfg.shortUrl = this.shortUrl;
        oreilly.cfg.title = this.title;
        
        oreilly.App.superclass.initComponent.call(this);
    }
    
});