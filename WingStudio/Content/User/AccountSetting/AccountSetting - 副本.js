//交替显示修改用户名框
function LoginNameToggle() {
    $("#loginName_display_block").toggle();
    $("#loginName_edit_block").toggle();
    $("#txt_loginName").val('');
    $("#tip_loginName").html('');
}

//交替显示修改密码框
function PasswordToggle() {
    $("#password_display_block").toggle();
    $("#password_edit_block").toggle();
    $("#txt_oldpwd").val('');
    $("#txt_newpwd").val('');
    $("#txt_confirmpwd").val('');
    $("#tip_password").html('');
}

//交替显示修改安全提问框
function SecQusetionToggle() {
    $("#secQusetion_display_block").toggle();
    $("#secQusetion_edit_block").toggle();
    $("#txt_answer").val('');
    $("#tip_secQusetion").html('');
    $('#secQusetion').val(0);
    $('#txt_answer_box').css('display', 'none');
}

//交替显示修改姓名框
function NameToggle() {
    $("#name_display_block").toggle();
    $("#name_edit_block").toggle();
    $("#txt_name").val('');
    $("#tip_name").html('');
}

//交替显示修改通知邮箱框
function EmailToggle() {
    $("#email_display_block").toggle();
    $("#email_edit_block").toggle();
    $("#txt_email").val('');
    $("#tip_email").html('');
}

//修改登录用户名
function ChangeLoginName() {
    var value = Trim($("#txt_loginName").val());
    if (value == '') {
        $("#tip_loginName").html('请输入新登录用户名');
        return;
    }
    var oldLoginName = $('#loginName').html();
    if (value == oldLoginName) {
        $("#tip_loginName").html('新登录用户名不能与原登录用户名相同');
        return;
    }
    if (value.length < 4 || value.length > 20) {
        $("#tip_loginName").html('登录用户名长度不合法');
        return;
    }

    if (!IsValidAccount(value)) {
        $("#tip_loginName").html('登录用户名不合法');
        return;
    }

    $("#tip_loginName").html("修改操作中，请稍候...");

    $.ajax({
        url: '/User/ChangeAccount',
        type: 'post',
        async: true,
        data: { account: value },
        success: function (data) {
            if (data.title == '修改成功') {
                $("#tip_loginName").html(data.title + "，立即<a href='/Logoff' >重新登录</a>");
                $('#loginName').html(Trim($("#txt_loginName").val()));
                $("#loginName_edit_block").hide();
                $("#loginName_display_block").show();
            }
            else {
                $("#tip_loginName").html(data.message);
            }
        },
        error: function () {
            $("#tip_loginName").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
        }
    });

}

//修改密码
function ChangePassword() {
    var value = Trim($("#txt_newpwd").val());
    if (value == '') {
        $("#tip_password").html('请输入密码');
        return;
    }

    if (!IsValidPassword(value)) {
        $("#tip_password").html("新密码不合法");
        return;
    }

    if (value != $("#txt_confirmpwd").val()) {
        $("#tip_password").html('两次密码输入不一致');
        return;
    }
    var oldpwd = $("#txt_oldpwd").val();
    if (value == oldpwd) {
        $("#tip_password").html("新密码不能与旧密码相同");
        return;
    }

    if (!IsValidPassword(oldpwd)) {
        $("#tip_password").html("旧密码错误");
        return;
    }

    $("#tip_password").html("修改操作中，请稍候...");

    $.ajax({
        url: '/User/ChangePassword',
        type: 'post',
        async: true,
        data: { oldPassword: oldpwd, newPassword: value },
        success: function (data) {
            if (data.title == '修改成功') {
                $("#tip_password").html(data.title + "，立即<a href='/Logoff' >重新登录</a>");
                $("#password_edit_block").hide();
                $("#password_display_block").show();
            }
            else {
                $("#tip_password").html(data.message);
            }
        },
        error: function () {
            $("#tip_password").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
        }
    });

}

//修改安全提问
function ChangeSecQusetion() {
    var selectValue = $("#secQusetion").val();
    var answer = $("#txt_answer").val();

    if (selectValue < 0 || selectValue > 6) {
        $("#tip_secQusetion").html("选择的安全问题不存在");
        return;
    }

    $("#tip_secQusetion").html("修改操作中，请稍候...");

    $.ajax({
        url: '/User/ChangeSecQusetion',
        type: 'post',
        async: true,
        data: { qusetion: selectValue, answer: answer },
        success: function (data) {
            if (data.title == '修改成功') {
                $("#tip_secQusetion").html(data.title + "，立即<a href='/Logoff' >重新登录</a>");
                $("#secQusetion_edit_block").hide();
                $("#secQusetion_display_block").show();
                if ($("#secQusetion").val() == 0) {
                    $("#secQusetionState").html("未设置");
                }
                else {
                    $("#secQusetionState").html("已开启");
                }
            }
            else {
                $("#tip_secQusetion").html(data.message);
            }
        },
        error: function () {
            $("#tip_secQusetion").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
        }
    });
}

//修改姓名
function ChangeName() {
    var value = Trim($("#txt_name").val());
    if (value == '') {
        $("#tip_name").html('请输入新姓名');
        return;
    }
    var oldname = $('#name').html();
    if (value == oldname) {
        $("#tip_name").html('新姓名不能与原姓名相同');
        return;
    }
    if (value.length == 0 || value.length > 20) {
        $("#tip_name").html('姓名长度不合法');
        return;
    }


    $("#tip_name").html("修改操作中，请稍候...");

    $.ajax({
        url: '/User/ChangeName',
        type: 'post',
        async: true,
        data: { name: value },
        success: function (data) {
            if (data.title == '修改成功') {
                $("#tip_name").html("成功修改姓名");
                $('#name').html(Trim($("#txt_name").val()));
                $("#name_edit_block").hide();
                $("#name_display_block").show();
            }
            else {
                $("#tip_name").html(data.message);
            }
        },
        error: function () {
            $("#tip_name").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
        }
    });

}

//修改通知邮箱
function ChangeEmail() {
    var value = Trim($("#txt_email").val());
    if (value == '') {
        $("#tip_email").html('请输入新通知邮箱');
        return;
    }
    var oldemail = $('#email').html();
    if (value == oldemail) {
        $("#tip_email").html('新通知邮箱不能与原通知邮箱相同');
        return;
    }
    if (!IsValidEmail(value)) {
        $("#tip_email").html('通知邮箱不合法');
        return;
    }

    $("#tip_email").html("修改操作中，请稍候...");

    $.ajax({
        url: '/User/ChangeEmail',
        type: 'post',
        async: true,
        data: { email: value },
        success: function (data) {
            if (data.title == '修改成功') {
                $("#tip_email").html("成功修改通知邮箱");
                $('#email').html(Trim($("#txt_email").val()));
                $("#email_edit_block").hide();
                $("#email_display_block").show();
            }
            else {
                $("#tip_email").html(data.message);
            }
        },
        error: function () {
            $("#tip_email").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
        }
    });

}