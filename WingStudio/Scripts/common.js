eval(function (p, a, c, k, e, r) { e = function (c) { return c.toString(36) }; if ('0'.replace(0, e) == 0) { while (c--) r[e(c)] = k[c]; k = [function (e) { return r[e] || e }]; e = function () { return '[1-9a-m]' }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]); return p }('9 myToTopToggle=e(f,g);9 b;9 a=h;9 1;c f(){1=2.3.1||7.8||2.4.1;5(1<=200){2.i("j").k.l="none"}6{2.i("j").k.l="block"}}c IntervalBackTop(){5(a){a=false;b=e(m,g)}}c m(){1=2.3.1||7.8||2.4.1;5(1<=20){clearInterval(b);5(2.4.1!=0){2.4.1=0}6 5(2.3.1!=0){2.3.1=0}6{7.8=0}a=h}6{5(2.4.1!=0){2.4.1=2.4.1-d}6 5(2.3.1!=0){2.3.1=2.3.1-d}6{7.8=7.8-d}}}', [], 23, '|scrollTop|document|documentElement|body|if|else|window|pageYOffset|var|toTopFlag|myBackTop|function|100|setInterval|ToTopToggle|50|true|getElementById|toTop|style|display|ScrollBackTop'.split('|'), 0, {}))





 


































































































































































































String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//去除字符串的前后空白
function Trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

//检查账号是否合法
function IsValidAccount(account) {
    var filter = /^[0-9a-zA-Z]{4,20}$/;
    if (filter.test(account))
    {
        return true;
    }
    else {
        return false;
    }
}

//检查密码是否合法
function IsValidPassword(pwd) {
    var filter = /^[0-9a-zA-Z]{6,20}$/;
    if (filter.test(pwd))
    {
        return true;
    }
    else {
        return false;
    }
}

//检查邮箱是否合法
function IsValidEmail(email) {
    var filter = /^([\w-\.]+)@(([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$/;
    if (filter.test(email))
    {
        return true;
    }
    else {
        return false;
    }
}

//检查电话号码是否合法
function IsValidPhone(phone) {
    var filter = /^1[3|4|5|7|8]\d{9}$/;
    if (filter.test(phone))
    {
        return true;
    }
    else {
        return false;
    }
}

//检查验证码是否合法
function IsValidCode(code) {
    var filter = /^[0-9a-zA-Z]{5}$/;
    if (filter.test(code))
    {
        return true;
    }
    else {
        return false;
    }
}

//检查地址是否合法
function IsValidAddress(address) {
    var filter = /^[\u4e00-\u9fa5]{2,4}-[\u4e00-\u9fa5]{2,8}-[\u4e00-\u9fa5]{2,10}$/;
    if (filter.test(address))
    {
        return true;
    }
    else {
        return false;
    }
}
