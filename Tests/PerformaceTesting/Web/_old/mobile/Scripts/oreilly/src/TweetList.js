oreilly.views.TweetList = Ext.extend(Ext.Panel, {
    hashtag: '',
    layout: 'fit',
    initComponent: function () {

        var toolbarBase = {
            xtype: 'toolbar',
            title: this.hashtag
        };

        if (this.prevCard !== undefined) {
            toolbarBase.items = [{
                ui: 'back',
                text: this.prevCard.title,
                scope: this,
                handler: function () {
                    this.ownerCt.setActiveItem(this.prevCard, { type: 'slide', reverse: true });
                }
            }, { xtype: 'spacer', flex: 1 }, {
                iconCls: 'action',
                iconMask: true,
                scope: this,
                ui: 'plain',
                handler: function () {
                    Ext.Msg.confirm('External Link', 'Open search in Twitter?', function (res) {
                        if (res == 'yes') {
                            window.location = 'http://search.twitter.com/search?q=' + escape(this.hashtag);
                        }
                    }, this);
                }
            }]
        }

        this.dockedItems = toolbarBase;

        this.list = new Ext.List({
            itemTpl: new Ext.XTemplate('<div class="avatar"<tpl if="profile_image_url"> style="background-image: url({profile_image_url})"</tpl>></div> <div class="tweet"><strong>{from_user}</strong><tpl if="to_user"> &raquo; {to_user}</tpl><br />{text:this.linkify}</div>', {
                linkify: function (value) {
                    return value.replace(/(http:\/\/[^\s]*)/g, "<span class=\"link\" href=\"$1\">$1</span>");
                }
            }),
            loadingText: false,
            store: new Ext.data.Store({
                model: 'Tweet',
                data: [{}]
            }),
            listeners: {
                selectionchange: { fn: this.selectTweet, scope: this }
            }
        });

        this.items = [this.list];

        oreilly.views.TweetList.superclass.initComponent.call(this);
    },

    selectTweet: function (sel, records) {
        if (records[0]) {
            Ext.Msg.confirm('外部链接', '打开Tweet?', function (res) {
                if (res == 'yes') {
                    
                }

                sel.deselectAll();
            }, this);
        }
    }
});

Ext.reg('tweetlist', oreilly.views.TweetList);