$(document).ready(function () {
    Window.UEDITOR_HOME_URL = "/Scripts/UEditor/";
    var editor = new baidu.editor.ui.Editor({
        autoHeightEnabled: false,
        initialFrameHeight: 300,
		initialFrameWidth: '100%',
        autoFloatEnabled: false,
		saveInterval: 10000,
        toolbars: [['source', 'fullscreen', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline', 'strikethrough', '|', 'superscript', 'subscript', '|', 'forecolor', 'backcolor', '|',
                    'insertorderedlist', 'insertunorderedlist', '|', 'insertcode', 'paragraph', '|', 'fontfamily', 'fontsize',
                    '|', 'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|',
                    'link', 'unlink', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|', 'horizontal', 'spechars', 'emotion', '|', 'simpleupload', 'insertvideo', 'map',
                    '|', 'searchreplace', 'preview']],
    });
    editor.render("Content");
    //创建离开提示框
    //绑定beforeunload事件
    //$(window).bind('beforeunload', function () {
    //    return '您输入的内容尚未保存，确定离开此页面吗？';
    //});
    //解除绑定
    //$(window).unbind('beforeunload');
    //window.onbeforeunload = null;
});

/* 检查提交表单是否合格 */
function Checkform() {
    var theme = $("#Theme").val();
    if (Trim(theme).length == 0 || Trim(theme).length > 40) {
        swal("错误提示", "标题不能为空或超过40个字!");
        return false;
    }
    var content = $("textarea[name='Content']").val();
    if (Trim(content) == "") {
        swal("错误提示", "博客正文不能为空!");
        return false;
    }
    return true;
}