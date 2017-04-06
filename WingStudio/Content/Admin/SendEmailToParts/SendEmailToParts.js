var isLock = false;
var count = 0;
var editor, validEmails;

$(document).ready(function () {
    Window.UEDITOR_HOME_URL = "/Scripts/UEditor/";
    editor = new baidu.editor.ui.Editor({
        autoHeightEnabled: false,
        initialFrameHeight: 300,
        autoFloatEnabled: false,
        toolbars: [['source', 'fullscreen', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline', 'strikethrough', '|', 'superscript', 'subscript', '|', 'forecolor', 'backcolor', '|',
                    'insertorderedlist', 'insertunorderedlist', '|', 'insertcode', 'paragraph', '|', 'fontfamily', 'fontsize',
                    '|', 'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|',
                    'link', 'unlink', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|', 'horizontal', 'spechars', 'emotion', '|', 'simpleupload', 'insertvideo', 'map',
                    '|', 'preview']],
    });
    editor.render("Content");
});


/* 检查提交表单是否合格 */
function Checkform() {
    if (isLock)
    {
        return false;
    }
    var theme = $("#Theme").val();
    if (Trim(theme).length == 0 || Trim(theme).length > 50) {
        swal("错误提示", "主题不能为空或超过50个字!");
        return false;
    }
    var content = $("textarea[name='Content']").val();
    if (Trim(content) == "") {
        swal("错误提示", "邮件正文不能为空!");
        return false;
    }
    var toEmail = $("#ToEmail").val();
    if (Trim(toEmail) == "")
    {
        swal("错误提示", "收件人不能为空!");
        return false;
    }
    var emails = toEmail.split(";");
    var fail = false, failEmail = "";
    validEmails = [];
    $.each(emails, function (index, email) {
        email = email.replace(/(^\s*)|(\s*$)/g, "");
        if(email != "")
        {
            if (IsValidEmail(email))
            {
                validEmails.push(email);
            }
            else
            {
                failEmail = email;
                fail = true;
                return false;
            }
        }
    });
    if (fail)
    {
        swal({ title: "错误提示", text: "存在不合法的收件人邮箱:<span style='color:red;'>" + failEmail + "</span>" , html: true});
        return false;
    }
    return true;
}

//发送邮件
function SendEmail()
{
    if (validEmails.length == 0)
    {
        return false;
    }
    isLock = true;
    count = 0;
    $('#result-alert').show();
    var theme = $("#Theme").val();
    var content = $("textarea[name='Content']").val();
    $("#ToEmail").val('');
    $("#Theme").val('');
    editor.setContent("");
    $.each(validEmails, function (index, email) {
        $.ajax({
            url: '/Admin/SendEmail',
            type: 'post',
            async: true,
            data: { ToEmail: email, Theme: theme, Content: content },
            success: function (data) {
                count++;
                if (count == 1) {
                    $("#result-data").html('');
                }
                if (data.state == "发送成功")
                {
                    $("#result-data").append("<tr><td>" + count + "</td><td>" + data.email + "</td><td class='green'>" + data.state + "</td></tr>");
                }
                else if (data.state == "发送失败")
                {
                    $("#result-data").append("<tr><td>" + count + "</td><td>" + data.email + "</td><td class='red'>" + data.state + "</td></tr>");
                }
                else
                {
                    $("#result-data").append("<tr><td>" + count + "</td><td>" + data.email + "</td><td class='blue'>" + data.state + "</td></tr>");
                }
                if (count == validEmails.length) {
                    $("#result-data").append("<tr><td class='red'>邮件已发送完毕!</td><td></td><td></td></tr>");
                    isLock = false;
                }
            },
            error: function () {
                swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
                isLock = false;
            }
        });
    });
}

//切换显示添加邮箱弹窗
function AddEmailToggle() {
    $("#addemail-alert").toggle();
    $("#all-check").prop("checked", false);
    $("[name='checkbox']").prop("checked", false);//取消全选
}

//添加邮箱到输入框中
function AddEmailToInput()
{
    var emails = "";
    $("[name='checkbox']").each(function () {
        if ($(this).is(':checked')) {
            emails += $(this).val() + ";";
        }
    });
    if(emails != "")
    {
        $("#ToEmail").val(emails);
    }
    AddEmailToggle();
}
