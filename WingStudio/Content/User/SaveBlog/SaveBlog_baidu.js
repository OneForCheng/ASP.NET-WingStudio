$(document).ready(function () {
    Window.UEDITOR_HOME_URL = "/Scripts/UEditor/";
    var editor = new baidu.editor.ui.Editor({
        autoHeightEnabled: false,
        initialFrameHeight: 400,
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