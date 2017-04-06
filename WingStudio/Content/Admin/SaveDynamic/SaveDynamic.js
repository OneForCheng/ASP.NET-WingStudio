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
                    'link', 'unlink', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|', 'horizontal', 'spechars', 'emotion', '|', 'simpleupload', 'insertvideo', 'map', '|',
                    'inserttable', 'deletetable', 'insertparagraphbeforetable', 'insertrow', 'deleterow', 'insertcol', 'deletecol', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols', 'charts', '|',
                    'preview']],
    });
    editor.render("Content");
});

/* 检查提交表单是否合格 */
function Checkform() {
    var theme = $("#Theme").val();
    var content = $("textarea[name='Content']").val();
    if (Trim(theme).length == 0 || Trim(theme).length > 40) {
        swal("错误提示", "主题不能为空或超过40个字!");
        return false;
    }
    if (Trim(content) == "") {
        swal("错误提示", "动态内容不能为空!");
        return false;
    }
    return true;
}
