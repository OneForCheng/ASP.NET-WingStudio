$(document).ready(function () {
    Window.UEDITOR_HOME_URL = "/Scripts/UEditor/";
    var editor = new baidu.editor.ui.Editor({
        autoHeightEnabled: false,
        initialFrameHeight: 180,
		initialFrameWidth: '100%',
        autoFloatEnabled:false,
		saveInterval: 10000,
        toolbars: [[ 'undo', 'redo', '|', 'bold', 'italic', 'underline', '|', 'forecolor', 'backcolor', '|',
                    'link', 'unlink', '|', 'horizontal', 'spechars', 'emotion',
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
    return true;
}