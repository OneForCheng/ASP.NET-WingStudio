var cropperHeader;
$(document).ready(function () {
	$("button.navbar-toggle").blur(function () {
		setTimeout(function () {
			$("#navbar_collapse").collapse('hide');
		}, 100);
	});
	
    cropperHeader = new Croppic('avatar_box', {
        modal: true,
        uploadUrl: '/User/UploadAvatar',
        cropUrl: '/User/CropAvatar',
        customUploadButtonId: 'upload_avatar',
        rotateControls: false,
        onAfterImgCrop: function () {
            location.href = "/User";
        },
    });
});

//切换搜索目标
function SearchTarget(target, action, placeholder) {
    if (target == "blog") {
        $("#search_form").attr("action", action);
        $("#search_input").attr("placeholder", placeholder);
    }
    else {
        $("#search_form").attr("action", action);
        $("#search_input").attr("placeholder", placeholder);
    }
}
