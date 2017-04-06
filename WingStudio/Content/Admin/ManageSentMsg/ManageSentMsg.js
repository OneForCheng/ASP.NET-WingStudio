//切换显示收件人弹窗
function ReceiverToggle(id) {
    if (id == 0) {
        $("#forcheng-alert").hide();
    }
    else {
        $.ajax({
            url: '/Admin/ReceiverInfos',
            type: 'post',
            async: true,
            data: { id: id },
            success: function (data) {
                if (!$.isEmptyObject(data)) {
                    var blogInfos = eval('(' + data + ')');
                    var htmlCode = "";
                    var i = 0;
                    for (i = 0; i < blogInfos.length; i++) {
                        htmlCode += "<tr><td>" + (i + 1) + "</td><td>" + blogInfos[i].Account + "</td><td>" + blogInfos[i].Name + "</td></tr>";
                    }
                    $("#body-data").html(htmlCode);
                }
                else {
                    $("#body-data").html("<tr><td class='red'>无任何收件人!</td></tr>");
                }
                $("#forcheng-alert").show();
            },
            error: function () {
                swal("请求失败", "请刷新重新提交！若一直出现此问题，请联系网站负责人。");
            }
        });
    }
}