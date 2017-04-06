//获取指定用户详细信息
function ShowDetailInfo(id) {
    $.ajax({
        url: '/SuperAdmin/GetUserDetailInfo',
        type: 'post',
        async: false,
        data:{id: id},
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                var infos = eval('(' + data + ')');
                var htmlCode = "";
                htmlCode = "<tr><td><span class='blue'>个人信息</span></td><td></td></tr>"
                         + "<tr><td>座右铭:</td><td>" + ((infos.Sign == "") ? "<span class='green'>暂未填写</span>" : infos.Sign) + "</td></tr>"
                         + "<tr><td>家乡:</td><td>" + ((infos.NativeAddr == "") ? "<span class='green'>暂未填写</span>" : infos.NativeAddr) + "</td></tr>"
                         + "<tr><td>现居地址:</td><td>" + ((infos.CurrentAddr == "") ? "<span class='green'>暂未填写</span>" : infos.CurrentAddr) + "</td></tr>"
                         + "<tr><td>生日日期:</td><td>" + ((infos.Birthday == "") ? "<span class='green'>暂未填写</span>" : infos.Birthday) + "</td></tr>"
                         + "<tr><td>班级:</td><td>" + ((infos.Class == "") ? "<span class='green'>暂未填写</span>" : infos.Class) + "</td></tr>"
                         + "<tr><td>爱好:</td><td>" + ((infos.Hobby == "") ? "<span class='green'>暂未填写</span>" : infos.Hobby) + "</td></tr>"
                         + "<tr><td>个人简介:</td><td>" + ((infos.Introduction == "") ? "<span class='green'>暂未填写</span>" : infos.Introduction) + "</td></tr>"
                         + "<tr><td><span class='blue'>联系方式</span></td><td></td></tr>"
                         + "<tr><td>QQ账号:</td><td>" + ((infos.QQ == "") ? "<span class='green'>暂未填写</span>" : infos.QQ) + "</td></tr>"
                         + "<tr><td>微信账号:</td><td>" + ((infos.WeiChat == "") ? "<span class='green'>暂未填写</span>" : infos.WeiChat) + "</td></tr>"
                         + "<tr><td>博客主页:</td><td>" + ((infos.Blog == "") ? "<span class='green'>暂未填写</span>" : infos.Blog) + "</td></tr>"
                         + "<tr><td>手机号码:</td><td>" + ((infos.Phone == "") ? "<span class='green'>暂未填写</span>" : infos.Phone) + "</td></tr>";
                $("#body-data").html(htmlCode);
                $("#panel_title").html("详细信息 - " + infos.Name);
                $("#forcheng-alert").show();
            }
            else {
                $("#forcheng-alert").hide();
            }
        },
        error: function () {
            swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
        }
    });
}