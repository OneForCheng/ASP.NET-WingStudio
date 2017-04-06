var ApplyLock = false;
var isFormal = true;

$(document).ready(function () {
    var content = $("#html_content").text();
    $("#html_content").html(filterXSS(content)).show();
});

//检查报名申请表单
function CheckApplyForm() {
    if (ApplyLock) {
        return false;
    }
    var name = Trim($("#apply_name").val());
    if (name == "" || name.length > 20) {
        $("#apply_error_msg").html('姓名不能为空或超过20个字!');
        return false;
    }
    var stuNo = $("#apply_no").val();
    if (stuNo == "") {
        $("#apply_error_msg").html('学号不能为空!');
        return false;
    }
    var filter = /^20[0-9]{6,7}$/;
    if (!filter.test(stuNo)) {
        $("#apply_error_msg").html('学号不合法!');
        return false;
    }
    var stuClass = Trim($("#apply_class").val());
    if (stuClass == "" || stuClass.length > 30) {
        $("#apply_error_msg").html('班级不能为空或超过30个字!');
        return false;
    }
    var email = $("#apply_email").val();
    if (email == "") {
        $("#apply_error_msg").html('邮箱不能为空!');
        return false;
    }
    if (!IsValidEmail(email)) {
        $("#apply_error_msg").html('邮箱不合法!');
        return false;
    }
    if (isFormal) {
        var phone = $("#apply_phone").val();
        if (phone == "") {
            $("#apply_error_msg").html('电话不能为空!');
            return false;
        }
        if (!IsValidPhone(phone)) {
            $("#apply_error_msg").html('电话不合法!');
            return false;
        }

    }
    if (!IsValidCode($("#apply_code").val())) {
        $("#apply_error_msg").html('验证码错误!');
        RefreshValiCode('apply_valicode', 'apply');
        return false;
    }
    return true;
}

//提交报名申请表单
function PostApplyForm(id) {
    ApplyLock = true;
    $("#apply_error_msg").html("表单提交中，请稍候...");
    $.ajax({
        url: '/RepeatedApply',
        type: 'post',
        async: false,
        data: { id: id, sno: $("#apply_no").val() },
        success: function (data) {
            if (data.Repeated) {
                swal({ title: "温馨提示", text: "已报名人员中存在类似报名信息，疑似重复报名，是否继续？", type: "warning", showCancelButton: true, confirmButtonColor: "#DD6B55", confirmButtonText: "继续", cancelButtonText: '取消', closeOnConfirm: false }, function (isConfirm) {
                    if (isConfirm) {
                        Apply(data.Id);
                    }
                    else {
                        ApplyLock = false;
                        ApplyToggle(isFormal);
                    }
                });
            }
            else {
                Apply(data.Id);
            }
        },
        error: function () {
            $("#apply_error_msg").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            ApplyLock = false;
        }
    });
}

//提交报名
function Apply(id) {
    ApplyLock = true;
    var phone = "", sex = "";
    if (isFormal) {
        phone = $("#apply_phone").val();
        sex = $("#apply_sex").val();
    }
    $.ajax({
        url: '/Apply',
        type: 'post',
        async: true,
        data: { id: id, __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(), Name: $("#apply_name").val(), StudentNo: $("#apply_no").val(), StudentClass: $("#apply_class").val(), Email: $("#apply_email").val(), Phone: phone, Sex: sex, code: $("#apply_code").val() },
        success: function (data) {
            if (data.title == "申请成功") {
                $("#apply_error_msg").html("申请成功!");
                swal({ title: data.title, text: data.message, type: "success" });
                $("#apply_count").html(data.count);
                $("#apply_panel").hide();
                $("#apply_name").val('');
                $("#apply_no").val('');
                $("#apply_class").val('');
                $("#apply_email").val('');
                $("#apply_code").val('');
                if (isFormal) {
                    $("#apply_phone").val('');
                }
                ApplyLock = false;
            }
            else {
                $("#apply_error_msg").html(data.message);
                swal({ title: data.title, text: data.message, type: "error" });
                $("#apply_code").val('');
                RefreshValiCode("apply_valicode", "apply");
                ApplyLock = false;
            }
        },
        error: function () {
            $("#apply_error_msg").html("提交失败，请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            ApplyLock = false;
        }
    });
}

function ApplyToggle(flag) {
    isFormal = flag;
    $("#apply_panel").toggle();
    $("#apply_error_msg").html('');
    $("#apply_name").val('');
    $("#apply_no").val('');
    $("#apply_class").val('');
    $("#apply_email").val('');
    $("#apply_code").val('');
    if (isFormal) {
        $("#apply_phone").val('');
    }
    RefreshValiCode('apply_valicode', 'apply');
}