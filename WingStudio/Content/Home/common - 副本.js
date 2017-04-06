var isLock = false;
var secanswer_mask_value = "";

//事情焦点时隐藏下拉菜单
$("button.navbar-toggle").blur(function () {
    setTimeout(function () {
        $("#navbar_collapse").collapse('hide');
    }, 100);
});

//显示登录模块
function ShowLoginBlock() {
    $("#login_panel").show();
    $("#login_form").show();
    $("#answer_box").hide();
    $("#forgot_pwd_form").hide();
    $("#panel_title").html("用户登录");
    $("#IsAdmin").val("False");
    $('#SecQusetion').val(0);
    $("#SecAnswer").val('');
    $("#login_error_msg").html('');
}

//显示忘记密码模块
function ShowForgotPwdBlock() {
    $("#login_form").hide();
    $("#panel_title").html("找回密码");
    $("#forgot_pwd_form").show();
    $("#fpwd_account").val('');
    $("#fpwd_name").val('');
    $("#fpwd_code").val('');
    $("#login_error_msg").html('');
    RefreshValiCode("forgot_pwd_valicode", "forget");
}

//切换安全提问
function SecQusetionToggle() {
    secanswer_mask_value = "";
    if ($('#SecQusetion').val() > 0) {
        $('#answer_box').show();
    }
    else {
        $('#answer_box').hide();
    }
}

//刷新验证码
function RefreshValiCode(id, type) {
    document.getElementById(id).src = "/GetValidateCode?type=" + type + "&time=" + (new Date()).getTime();
}

//用户登录
function UserLogin() {
    $("#panel_title").html("用户登录");
    $("#IsAdmin").val("False");
}

//管理员登录
function AdminLogin() {
    $("#panel_title").html("管理员登录");
    $("#IsAdmin").val("True");
}

//检查登录表单
function CheckLoginForm() {
    if (isLock) {
        return false;
    }
    var pwd = $("#Password").val();
    var account = $("#Account").val();
    var secQusetion = $("#SecQusetion").val();
    var secAnswer = $("#SecAnswer").val();
    if (account == "") {
        $("#login_error_msg").html('用户名不能为空!');
        return false;
    }
    if (pwd == "") {
        $("#login_error_msg").html('密码不能为空!');
        return false;
    }
    if (secQusetion != "0") {
        if (secAnswer == "") {
            $("#login_error_msg").html('提问答案不能为空!');
            return false;
        }
        if (Trim(secAnswer).length > 50) {
            $("#login_error_msg").html('提问答案超出指定长度!');
            return false;
        }
    }
    if (!IsValidAccount(account)) {
        $("#login_error_msg").html('用户名不合法!');
        return false;
    }
    if (!IsValidPassword(pwd)) {
        $("#login_error_msg").html('密码不合法!');
        return false;
    }
    return true;
}

//提交登录表单
function PostLoginForm() {
    isLock = true;
    $("#login_error_msg").html("表单提交中，请稍候...");
    $.ajax({
        url: '/Login',
        type: 'post',
        async: true,
        data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), Account: $("#Account").val(), Password: $("#Password").val(), SecQuestion: $("#SecQusetion").val(), SecAnswer: $("#SecAnswer").val(), IsAdmin: $("#IsAdmin").val() },
        success: function (data) {
            if (data.title == "登录成功") {
                $("#login_error_msg").html("登录成功，正在跳转...");
                location.href = data.href;
            }
            else {
                $("#login_error_msg").html(data.error);
                isLock = false;
            }
        },
        error: function (xhr) {
            //$("body").html(xhr.responseText);
            $("#login_error_msg").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            isLock = false;
        }
    });
}

//检查忘记密码表单
function CheckForgotPwdForm() {
    if (isLock) {
        return false;
    }
    var account = $("#fpwd_account").val();
    if (account == "") {
        $("#login_error_msg").html('用户名不能为空!');
        return false;
    }
    var name = $("#fpwd_name").val();
    if (name == "") {
        $("#login_error_msg").html('姓名不能为空!');
        return false;
    }
    if (!IsValidAccount(account)) {
        $("#login_error_msg").html('用户名不合法!');
        return false;
    }
    if (Trim(name).length > 20) {
        $("#login_error_msg").html('姓名超出指定长度!');
        return false;
    }
    var code = $("#fpwd_code").val();
    if (!IsValidCode(code)) {
        $("#login_error_msg").html('验证码错误!');
        RefreshValiCode("forgot_pwd_valicode", "forget");
        return false;
    }
    return true;
}

//提交找回密码表单
function PostForgotPwdForm() {
    isLock = true;
    $("#login_error_msg").html("表单提交中，请稍候...");
    $.ajax({
        url: '/ForgotPassword',
        type: 'post',
        async: true,
        data: { account: $("#fpwd_account").val(), name: $("#fpwd_name").val(), code: $("#fpwd_code").val() },
        success: function (data) {
            if (data.title == "申请成功") {
                $("#login_error_msg").html("申请成功!");
                swal({ title: data.title, text: data.message, type: "success" });
                isLock = false;
                ShowLoginBlock();
            }
            else {
                $("#login_error_msg").html(data.message);
                //swal({ title: data.title, text: data.message, type: "error" });
                $("#fpwd_code").val('');
                RefreshValiCode("forgot_pwd_valicode", "forget");
                isLock = false;
            }
        },
        error: function (xhr) {
            $("body").html(xhr.responseText);
            $("#login_error_msg").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            isLock = false;
        }
    });
}

//安全提问答案输入时间绑定
$('#secanswer_mask').bind('input propertychange', function () {

    var value1 = $(this).val().trim();
    var target = $("#SecAnswer");
    if (value1 == "") {
        secanswer_mask_value = "";
        target.val("");
        return true;
    }
    var value2 = target.val() + "";
    var len1 = value1.length;
    var len2 = value2.length;
    if (len1 < len2) {
        target.val(value2.substr(0, len1));
        return true;
    }
    if (secanswer_mask_value != value1) {
        var tmpStr = value1.substr(secanswer_mask_value.length, len1 - secanswer_mask_value.length);
        if (tmpStr != "") {
            target.val(value2 + tmpStr);
            tmpStr = "";
            for (i = 0; i < len1; i++) {
                tmpStr += "•";
            }
            $(this).val(tmpStr);
        }
        secanswer_mask_value = value1;
    }

    return true;
});