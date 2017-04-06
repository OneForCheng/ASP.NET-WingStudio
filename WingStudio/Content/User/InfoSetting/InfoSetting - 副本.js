$(document).ready(function () {
    jeDate({
        dateCell: "#Birthday",
        format: "YYYY-MM-DD",
        isinitVal: false,
        isTime: true, //isClear:false,
        minDate: "1990-01-01",
    });
    $("#NativeAddr").click(function (e) {
        SelCity(this, e);
    });
    $("#CurrentAddr").click(function (e) {
        SelCity(this, e);
    });
});

/* 检查提交表单是否合格 */
function Checkform() {
    var sign = $("#Sign").val();
    if (sign != "") {
        if (sign.length > 50)
        {
            $("#error_msg").html("座右铭超过指定长度");
            return false;
        }
    }
 
    var nativeAddr = $("#NativeAddr").val();
    if (nativeAddr != "") {
        if (!IsValidAddress(nativeAddr)) {
            $("#error_msg").html("家乡地址不合法");
           
            return false;
        }
    }

    var currentAddr = $("#CurrentAddr").val();
    if (currentAddr != "") {
        if (!IsValidAddress(currentAddr)) {
            $("#error_msg").html("现居地址不合法");
           
            return false;
        }
    }

    var birthday = $("#Birthday").val();
    if (birthday != "")
    {
        var filter = /^(\d{4})-(\d{1,2})-(\d{1,2})$/
        if (!filter.test(birthday))
        {
            $("#error_msg").html("出生日期不合法");
            
            return false;
        }
        birthday = Date.parse(birthday);
        if (isNaN(birthday)) {
            $("#error_msg").html("出生日期不合法");
            return false;
        }
    }

    var stuClass = $("#Class").val();
    if (stuClass != "") {
        if (stuClass.length > 20) {
            $("#error_msg").html("班级超过指定长度");
            return false;
        }
    }

    var hobby = $("#Hobby").val();
    if (hobby != "") {
        if (hobby.length > 50) {
            $("#error_msg").html("爱好超过指定长度");
            return false;
        }
    }

    var intro = $("#Introduction").val();
    if (intro != "") {
        if (intro.length > 100) {
            $("#error_msg").html("个人简介超过指定长度");
            return false;
        }
    }

    return true;
}