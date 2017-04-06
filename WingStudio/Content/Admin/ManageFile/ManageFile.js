//切换显示添加文件所属组弹窗
function AddGroupToggle(id) {
    $("#del-alert").hide();
    if (id == 0) {
        $("#add-alert").hide();
        $("#add-all-check").prop("checked", false);
        $("[name='add-checkbox']").prop("checked", false);//取消全选
    }
    else {
        $("#add-all-check").val(id);
        $.ajax({
            url: '/Admin/EnableAddFileGroups',
            type: 'post',
            async: true,
            data: { id: id },
            success: function (data) {

                if (!$.isEmptyObject(data)) {
                    var blogInfos = eval('(' + data + ')');
                    var htmlCode = "";
                    var i = 0;
                    for (i = 0; i < blogInfos.length; i++) {
                        htmlCode += "<tr><td><div class='checkbox'><label><input type='checkbox' name='add-checkbox' value='" + blogInfos[i].Id + "'></label></div></td><td>" + (i + 1) + "</td><td title='" + blogInfos[i].Theme + "'>" + blogInfos[i].Theme + "</td><td title='" + blogInfos[i].Description + "'>" + blogInfos[i].Description + "</td></tr>";
                    }
                    $("#add-body-data").html(htmlCode);
                }
                else {
                    $("#add-body-data").html("<tr><td class='red'>无可添加文件组!</td></tr>");
                }
                $("#add-alert").show();
                $("#add-all-check").prop("checked", false);
                $("[name='add-checkbox']").prop("checked", false);//取消全选
            },
            error: function () {
                swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            }
        });
    }
}

//添加文件所属组
function AddGroupToFile() {
    var id = parseInt($("#add-all-check").val());
    var fail = false;
    var isPost = false;
    $("[name='add-checkbox']").each(function () {
        if ($(this).is(':checked')) {
            isPost = true;
            var groupId = parseInt($(this).val());
            $.ajax({
                url: '/Admin/AddFileToGroup',
                type: 'post',
                async: false,
                data: { id: id, groupId: groupId },
                success: function (data) {
                    if (data.title == "添加失败") {
                        fail = true;
                    }
                },
                error: function () {
                    swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
                }
            });
        }
    });
    $("#add-alert").hide();
    $("#add-all-check").prop("checked", false);
    $("[name='add-checkbox']").prop("checked", false);//取消全选
    if (isPost) {
        if (fail) {
            swal("操作提示", "存在一个或多个非法数据!");
        }
        else {
            swal("添加成功", "成功添加指定文件到指定组!");
        }
    }
}


//切换显示删除文件所属组弹窗
function DelGroupToggle(id) {
    $("#add-alert").hide();
    if (id == 0) {
        $("#del-alert").hide();
        $("#del-all-check").prop("checked", false);
        $("[name='del-checkbox']").prop("checked", false);//取消全选
    }
    else {
        $("#del-all-check").val(id);
        $.ajax({
            url: '/Admin/EnableDelFileGroups',
            type: 'post',
            async: true,
            data: { id: id },
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    var blogInfos = eval('(' + data + ')');
                    var htmlCode = "";
                    var i = 0;
                    for (i = 0; i < blogInfos.length; i++) {
                        htmlCode += "<tr><td><div class='checkbox'><label><input type='checkbox' name='del-checkbox' value='" + blogInfos[i].Id + "'></label></div></td><td>" + (i + 1) + "</td><td title='" + blogInfos[i].Theme + "'>" + blogInfos[i].Theme + "</td><td title='" + blogInfos[i].Description + "'>" + blogInfos[i].Description + "</td></tr>";
                    }
                    $("#del-body-data").html(htmlCode);
                }
                else {
                    $("#del-body-data").html("<tr><td class='red'>无可移除文件组!</td></tr>");
                }
                $("#del-alert").show();
                $("#del-all-check").prop("checked", false);
                $("[name='del-checkbox']").prop("checked", false);//取消全选
            },
            error: function () {
                swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            }
        });
    }
}

//删除文件所属组
function DelFileFromGroup() {
    var id = parseInt($("#del-all-check").val());
    var fail = false;
    var isPost = false;
    $("[name='del-checkbox']").each(function () {
        if ($(this).is(':checked')) {
            isPost = true;
            var groupId = parseInt($(this).val());
            $.ajax({
                url: '/Admin/DelFileFromGroup',
                type: 'post',
                async: false,
                data: { id: id, groupId: groupId },
                success: function (data) {
                    if (data.title == "删除失败") {
                        fail = true;
                    }
                },
                error: function () {
                    swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
                }
            });
        }
    });
    $("#del-alert").hide();
    $("#del-all-check").prop("checked", false);
    $("[name='del-checkbox']").prop("checked", false);//取消全选
    if (isPost) {
        if (fail) {
            swal("操作提示", "存在一个或多个非法数据!");
        }
        else {
            swal("移除成功", "成功移除指定文件的指定所有组!");
        }
    }
}