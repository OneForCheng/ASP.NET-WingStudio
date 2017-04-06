$(document).ready(function () {
    Window.UEDITOR_HOME_URL = "/Scripts/UEditor/";
    var editor = new baidu.editor.ui.Editor({
        autoHeightEnabled: false,
        initialFrameHeight: 300,
        initialFrameWidth: '100%',
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
    var theme = $("#Theme").val();
    if (Trim(theme).length == 0 || Trim(theme).length > 40) {
        swal("错误提示", "主题不能为空或超过40个字!");
        return false;
    }
    var content = $("textarea[name='Content']").val();
    if (Trim(content) == "") {
        swal("错误提示", "消息正文不能为空!");
        return false;
    }
    var target = $("#target").val();
    if (Trim(target) == "") {
        return true;
    }
    var accounts = target.split(";");
    var filter = /^[a-zA-Z0-9]{4,20}$/;
    var fail = false, failAccount = "";
    $.each(accounts, function (index, account) {
        account = account.replace(/(^\s*)|(\s*$)/g, "");
        if (account != "") {
            if (!filter.test(account)) {
                failAccount = account;
                fail = true;
                return false;
            }
        }
    });
    if (fail) {
        swal({ title: "错误提示", text: "存在不合法的收信人账号:<span style='color:red;'>" + failAccount + "</span>", html: true });
        return false;
    }
    return true;
}


//切换显示添加邮箱弹窗
function AddAccountToggle() {
    $("#forcheng-alert").toggle();
    $("#all-check").prop("checked", false);
    $("[name='checkbox']").prop("checked", false);//取消全选
}

//添加邮箱到输入框中
function AddAccountToInput() {
    var accounts = "";
    $("[name='checkbox']").each(function () {
        if ($(this).is(':checked')) {
            accounts += $(this).val() + ";";
        }
    });
    if (accounts != "") {
        $("#target").val(accounts);
    }
    AddAccountToggle();
}
