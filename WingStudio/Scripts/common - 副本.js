//有关返回顶部的按钮
var myToTopToggle = setInterval(ToTopToggle, 50);//控制显示
var myBackTop;//控制返回顶部
var toTopFlag = true;
var scrollTop;//滚动条距离顶部的距离

//显示或隐藏返回顶部按钮
function ToTopToggle() {
    //兼容各大浏览器
    scrollTop = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
    if (scrollTop <= 200) {
        document.getElementById("toTop").style.display = "none";
    }
    else {
        document.getElementById("toTop").style.display = "block";
    }
}

//设置平滑滚动到顶部的触发器
function IntervalBackTop() {
    if (toTopFlag) {
        toTopFlag = false;
        myBackTop = setInterval(ScrollBackTop, 50);
    }
}

//平滑滚动到顶部
function ScrollBackTop() {
    scrollTop = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
    if (scrollTop <= 20) {
        clearInterval(myBackTop);
        if (document.body.scrollTop != 0) {
            document.body.scrollTop = 0;
        }
        else if (document.documentElement.scrollTop != 0) {
            document.documentElement.scrollTop = 0;
        }
        else {
            window.pageYOffset = 0;
        }
        toTopFlag = true;
    }
    else {
        if (document.body.scrollTop != 0) {
            document.body.scrollTop = document.body.scrollTop - 100;
        }
        else if (document.documentElement.scrollTop != 0) {
            document.documentElement.scrollTop = document.documentElement.scrollTop - 100;
        }
        else {
            window.pageYOffset = window.pageYOffset - 100;
        }

    }
}


//=============自定义函数=================

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
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$/;
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
