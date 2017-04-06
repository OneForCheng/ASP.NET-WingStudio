$(document).ready(function () {
    SetGrade();
});


/* 检查提交表单是否合格 */
function Checkform() {
    var name = document.getElementById("Name").value;
    var account = document.getElementById("Account").value;
    var pwd = document.getElementById("Password").value;
    var email = document.getElementById("Email").value;

    if (!IsValidAccount(account)) {
        swal("添加失败", "用户名不合规范!");
        return false;
    }
    if (Trim(name) == "" || Trim(name).length > 20) {
        swal("添加失败", "姓名不合规范!");
        return false;
    }
    if (!IsValidPassword(pwd)) {
        swal("添加失败", "密码不合规范!");
        return false;
    }
    if (!IsValidEmail(email)) {
        swal("添加失败", "邮箱不合规范!");
        return false;
    }
    return true;
}

//设置选择年级
function SetGrade() {
    var select = document.getElementById("Grade");
    var myDate = new Date();
    var str = myDate.getFullYear() + "";
    var end = str.substring(2, 4);
    var start = end - 5;
    var selectValue = "<option value=\"" + start + "\" selected=\"selected\">" + start + "届</option>";
    for (i = start + 1; i <= end; i++) {
        selectValue += "<option value=\"" + i + "\">" + i + "届</option>";
    }
    select.innerHTML = selectValue;
}