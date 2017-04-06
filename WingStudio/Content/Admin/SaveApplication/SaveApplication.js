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
    InitDate();
});

//初始化日期
function InitDate() {
    var now = new Date();
    var nowStr = now.getFullYear() + "-" + (now.getMonth()+1) + "-" + now.getDate();
    jeDate({
        dateCell: "#StartTime",
        format: "YYYY-MM-DD hh:mm:ss",
        isinitVal: false,
        isTime: true,
        //minDate: nowStr,
    });
    jeDate({
        dateCell: "#EndTime",
        format: "YYYY-MM-DD hh:mm:ss",
        isinitVal: false,
        isTime: true, //isClear:false,
        //minDate: nowStr,
    });
}

/* 检查提交表单是否合格 */
function Checkform() {
    var theme = $("#Theme").val();
    var content = $("textarea[name='Content']").val();
    if (Trim(theme).length == 0 || Trim(theme).length > 40) {
        swal("错误提示", "主题不能为空或超过40个字!");
        return false;
    }
    if (Trim(content) == "") {
        swal("错误提示", "报名内容不能为空!");
        return false;
    }
    var startTime = $("#StartTime").val();
    if (startTime == "")
    {
        swal("错误提示", "开始时间不能为空!");
        return false;
    }
    startTime = Date.parse(startTime);
    if (isNaN(startTime))
    {
        swal("错误提示", "开始时间不合法!");
        return false;
    }
    var endTime = $("#EndTime").val();
    if (endTime == "") {
        swal("错误提示", "截止时间不能为空!");
        return false;
    }
    endTime = Date.parse(endTime);
    if (isNaN(endTime)) {
        swal("错误提示", "截止时间不合法!");
        return false;
    }
    if (startTime - endTime > 0) {
        swal("错误提示", "开始时间不能大于截止时间!");
        return false;
    }
    return true;
}