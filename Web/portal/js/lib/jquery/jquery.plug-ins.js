
//
//---------------------------------Cookie操作 插件 开始--------------------------------------------------------------
//

jQuery.cookie = function(name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }

        expires = expires || '; expires=Fri, 31 Dec 2099 23:59:59 GMT;'

        var path = options.path ? '; path=' + options.path : '';
        var domain = options.domain ? '; domain=' + options.domain : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

//
//---------------------------------Cookie操作 插件 结束--------------------------------------------------------------
//

//
//---------------------------------QueryString操作 插件 开始--------------------------------------------------------------
//

/* JQueryString v1.6.1
By James Campbell 
Many thanks to Mike Willis for his suggestions and additions to this jQuery plugin.
*/
(function($) {
    $.getAllQueryStrings = function(options) {
        defaults = { DefaultValue: "undefined", URL: window.location.href };
        options = $.extend(defaults, options);
        var qs;
        var args = new Array();
        if (typeof (options.URL.split("?")[1]) != "undefined") {
            qs = options.URL.split("?")[1].replace(/\+/g, ' ').split('&');
            $.each(qs, function(i) {
                var currentArg = this.split('=');
                if (currentArg.length == 2) {
                    args[i] = { name: currentArg[0], value: currentArg[1] };
                    args[currentArg[0]] = { name: currentArg[0], value: currentArg[1] };
                } else {
                    args[i] = { name: currentArg[0], value: currentArg[1] };
                    args[currentArg[0]] = { name: currentArg[0], value: currentArg[0] };
                }
            });
        }
        if (args.length <= 0) { };
        return args;
    }

    $.getQueryString = function(options) {
        defaults = { DefaultValue: "", URL: window.location.href };
        options = $.extend(defaults, options);
        if (typeof ($.getAllQueryStrings({ URL: options.URL })[options.ID]) == "undefined") {
            return options.DefaultValue;
        } else {
            return $.getAllQueryStrings({ DefaultValue: options.DefaultValue, URL: options.URL })[options.ID].value;
        }
    }

    $.getQueryUrl = function(url) {
        url = url || document.location.href;
        var sepIdx = url.indexOf("?");
        if (sepIdx < 0) return url;

        return url.substr(0, sepIdx);
    }

    $.formatQueryUrl = function(url) {
        url = url || document.location.href;
        // 移除井号（防刷新符）
        return url.trimEnd("#").replace(/#\?/g, "?");
    }

    // 组合url
    $.combineQueryUrl = function(url, param) {
        url = url || document.location.href;

        if (!($.isSetted(param)) || !param) {
            return url;
        }

        var args = $.getAllQueryStrings({ URL: url }) || {};

        var parr = [];
        switch (typeof (param)) {
            case "object":
                if ($.isArray(param)) {
                    parr = param;
                } else if ($.isPlainObject(param)) {
                    for (var key in param) {
                        if (param[key] != undefined && param[key] != null) {
                            parr.push(key + "=" + param[key].toString());
                        }
                    }
                }
                break;
            case "string":
                parr = param.trimStart("&").split("&");
                break;
        }

        $.each(parr, function() {
            var iarr = this.split("=");
            args[iarr[0]] = { name: iarr[0], value: iarr[1] };
        });

        var qryurl = $.getQueryUrl(url) + "?";
        var pstr = "";
        
        for (var key in args) {
            if (args[key] && isNaN(key) && typeof (args[key]) != 'function') {
                pstr += "&" + args[key].name + "=" + args[key].value;
            }
        }

        qryurl += pstr.trimStart("&");

        return qryurl;
    }
    ;
})(jQuery);

//
//---------------------------------QueryString操作 插件 结束--------------------------------------------------------------
//