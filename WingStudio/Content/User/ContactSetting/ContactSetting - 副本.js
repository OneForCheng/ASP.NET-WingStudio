/* 检查提交表单是否合格 */
function Checkform() {
   
    var qq = $("#QQ").val();
    var filter = /^[0-9]{5,11}$/;
    if (qq != "") {
        if (!filter.test(qq)) {
            $("#error_msg").html("QQ账号不合法");
            return false;
        }
    }

    var webChat = $("#WeiChat").val();
    filter = /^[a-zA-Z]([0-9a-zA-Z]|_|-){5,19}$/;
    if (webChat != "") {
        if (!filter.test(webChat)) {
            $("#error_msg").html("微信账号不合法");
            return false;
        }
    }

    var blog = $("#Blog").val();
    filter = /^((http|https):\/\/)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(\/[a-zA-Z0-9\&%_\./-~-]*)?$/;
    if (blog != "") {
        if (!filter.test(blog)) {
            $("#error_msg").html("博客主页不合法");
            return false;
        }
    }

    var phone = $("#Phone").val();
    filter = /^1[3|4|5|7|8]\d{9}$/;
    if (phone != "") {
        if (!filter.test(phone)) {
            $("#error_msg").html("手机号码不合法");
            return false;
        }
    }

    return true;
}