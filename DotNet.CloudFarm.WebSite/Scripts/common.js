(function ($) {
    $.QueryString = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=');
            if (p.length != 2) continue;
            b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'))
})(jQuery);

//手机号验证
function isMobile(mobile) {  
    var pattern = /^1[34578]\d{9}$/;  
    if (pattern.test(mobile)) {
        return true;  
    }  
    return false;  
};


function IsNumer(value) {
    var pattern = /^(\d{6})$/g; // 正则表达式
    //var pattern = /^(\d{8})|(\d{9})$/g; // 正则表达式
    return pattern.test(value);
}

//制保留2位小数，如：2，会在2后面补上00.即2.00  
function toDecimal2(x) {
    var f = parseFloat(x);
    if (isNaN(f)) {
        return 0;
    }
    var f = Math.round(x * 100) / 100;
    var s = f.toString();
    var rs = s.indexOf('.');
    if (rs < 0) {
        rs = s.length;
        s += '.';
    }
    while (s.length <= rs + 2) {
        s += '0';
    }
    return parseFloat(s).toFixed(2);
}
