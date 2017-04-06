$(document).ready(function () {
    Init();
});



//初始化页面
function Init() {
    $("nav").height($(document).height());

    // Hide message
    $(".content .message .close").click(function () {
        $(".content .message").fadeOut(500);
    });

    // Mobile navigation
    var toggleFlag = false;
    var clickFlag = false;
    $(".btn-menu").click(function () {
        clickFlag = true;
        if (toggleFlag) {
            $("nav").animate({ "left": "-215px" });
            $("section.content").animate({ marginLeft: 0, marginRight: 0 }, 400);
            toggleFlag = false;
        }
        else {
            $("nav").animate({ "left": 0 }, 20);
            $("section.content").animate({ marginLeft: 215, marginRight: -215 }, 20);
            toggleFlag = true;
        }
    });

    // Sticky sidebar

    $(window).bind("load resize", function () {
        if (clickFlag) {
            $("nav").removeAttr("style");
            $("nav").height($(document).height());
            $("section.content").removeAttr("style");
            clickFlag = false;
        }
    });

}

//选择所有checkbox，class名称为默认值
function SelectedAllBox() {
    if ($("#all-check").is(':checked')) {
        $("[name='checkbox']").prop("checked", true);//全选  
    }
    else {
        $("[name='checkbox']").prop("checked", false);//取消全选
    }
}

//选择所有checkbox，class名称为变量
function CheckedAllBox(id, name) {
    if ($(id).is(':checked')) {
        $("[name='" + name + "']").prop("checked", true);//全选  
    }
    else {
        $("[name='" + name + "']").prop("checked", false);//取消全选
    }
}

//更新图标
var cropperHeader;
var currentId = 0;

function UpdateIcon(id, type, index) {
    if (currentId == id) {
        return true;
    }
    else {
        currentId = id;
    }
    var targetId = (type + "_" + index)
    cropperHeader = new Croppic('icon_box', {
        modal: true,
        uploadUrl: '/Admin/UploadIcon',
        cropUrl: '/Admin/CropIcon',
        cropData: { id: id, type: type },
        customUploadButtonId: targetId,
        rotateControls: false,
        onAfterImgCrop: function () {
            location.reload();//刷新
        },
    });
}