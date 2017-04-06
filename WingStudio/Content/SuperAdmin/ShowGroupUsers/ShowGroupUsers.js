//切换显示添加成员弹窗
function AddMemberToggle(id) {
    if (id == 0) {
        $("#forcheng-alert").hide();
        $("#all-check").prop("checked", false);
        $("[name='checkbox']").prop("checked", false);//取消全选
    }
    else {
        $("#all-check").val(id);
        $.ajax({
            url: '/SuperAdmin/GetBaseUserInfos',
            type: 'post',
            async: true,
            data: { id: id },
            success: function (data) {

                if (!$.isEmptyObject(data)) {
                    var userInfos = eval('(' + data + ')');
                    var htmlCode = "";
                    var i = 0;
                    for (i = 0; i < userInfos.length; i++) {
                        htmlCode += "<tr><td><div class='checkbox'><label><input type='checkbox' name='checkbox' value='" + userInfos[i].Id + "'></label></div></td><td>" + (i + 1) + "</td><td>" + userInfos[i].Account + "</td><td>" + userInfos[i].Name + "</td></tr>";
                    }
                    $("#body-data").html(htmlCode);
                }
                else {
                    $("#body-data").html("<tr><td class='red'>无可添加用户!</td></tr>");
                }
                $("#forcheng-alert").show();
                $("#all-check").prop("checked", false);
                $("[name='checkbox']").prop("checked", false);//取消全选
            },
            error: function () {
                swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            }
        });
    }
}

//添加成员到指定组
function AddMemberToGroup() {
    var groupId = parseInt($("#all-check").val());
    var fail = false;
    var isPost = false;
    $("[name='checkbox']").each(function () {
        if ($(this).is(':checked')) {
            isPost = true;
            var id = parseInt($(this).val());
            $.ajax({
                url: '/SuperAdmin/AddUserToGroup',
                type: 'post',
                async: false,
                data: { id: id, groupId: groupId },
                success: function (data) {
                    if (data.title.indexOf('添加失败') > -1) {
                        fail = true;
                    }
                },
                error: function () {
                    swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
                }
            });
        }
    });
    $("#forcheng-alert").hide();
    $("#all-check").prop("checked", false);
    $("[name='checkbox']").prop("checked", false);//取消全选
    if (isPost) {
        if (fail) {
            swal("操作提示", "存在一个或多个非法数据!");
        }
        else {
            swal("添加成功", "成功添加指定用户到指定组!");
        }
    }
}