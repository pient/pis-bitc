Ext.setup({
    icon: 'icon.png',
    tabletStartupScreen: 'tablet_startup.png',
    phoneStartupScreen: 'phone_startup.png',
    glossOnIcon: false,
    onReady: function () {
        var tabpanel = new Ext.TabPanel({
            tabBar: {
                dock: 'bottom',
                layout: {
                    pack: 'center'
                }
            },
            fullscreen: true,
            ui: 'light',
            cardSwitchAnimation: {
                type: 'slide',
                cover: true
            },

            defaults: {
                scroll: 'vertical'
            },
            items: [{
                title: '主页',
                html: '<h1>Bottom Tabs</h1><p>Docking tabs to the bottom will automatically change their style. The tabs below are type="light", though the standard type is dark. Badges (like the 4 &amp; Long title below) can be added by setting <code>badgeText</code> when creating a tab/card or by using <code>setBadge()</code> on the tab later.</p>',
                iconCls: 'info',
                cls: 'card1'
            }, {
                title: '收藏夹',
                html: '<h1>Favorites Card</h1>',
                iconCls: 'favorites',
                cls: 'card2',
                badgeText: '4'
            }, {
                title: '行车手册',
                id: 'tab3',
                html: '<h1>Downloads Card</h1>',
                badgeText: 'Text can go here too, but it will be cut off if it is too long.',
                cls: 'card3',
                iconCls: 'download'
            }, {
                title: '在线预约',
                html: '<h1>Settings Card</h1>',
                cls: 'card4',
                iconCls: 'settings'
            }, {
                title: '救援服务',
                html: '<h1>Settings Card</h1>',
                cls: 'card4',
                iconCls: 'settings'
            }, {
                title: '会员中心',
                html: '<h1>User Card</h1>',
                cls: 'card5',
                iconCls: 'user'
            }]
        });
    }
});