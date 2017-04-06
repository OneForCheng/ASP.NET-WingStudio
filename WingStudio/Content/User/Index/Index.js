var calendar;
$(document).ready(function () {
    calendar = new Calendar("CalendarValue", {
        SelectDay: null,
        onSelectDay: function (o) { o.className = "onSelect"; },
        onToday: function (o) { o.className = "onToday"; },
        onFinish: function () {
            $Id("CalendarPre").onclick = function () {
                calendar.PreMonth();
                ShowExistBlogsDays();
            }
            $Id("CalendarNext").onclick = function () {
                calendar.NextMonth();
                ShowExistBlogsDays();
            }
            $Id("CalendarYear").innerHTML = this.Year; $Id("CalendarMonth").innerHTML = this.Month;
        }
    });
    ShowExistBlogsDays();

    $(".user-blog").mouseenter(function () {
        var count = $(this).attr("title") + " 篇";
        $(this).addClass("count");
        $(this).html(count);
    }).mouseleave(function () {
        $(this).html("个人博客");
        $(this).removeClass("count");
    });
});

//显示登录用户详细信息
function ShowDetailInfo() {
    $.ajax({
        url: '/User/GetLoginUserInfo',
        type: 'post',
        async: false,
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                var infos = eval('(' + data + ')');
                var htmlCode = "";
                htmlCode = "<tr><td><span class='blue'>个人信息</span></td><td></td></tr>"
                         + "<tr><td>座右铭:</td><td>" + ((infos.Sign == "") ? "<span class='green'>暂未填写</span>" : infos.Sign) + "</td></tr>"
                         + "<tr><td>家乡:</td><td>" + ((infos.NativeAddr == "") ? "<span class='green'>暂未填写</span>" : infos.NativeAddr) + "</td></tr>"
                         + "<tr><td>现居地址:</td><td>" + ((infos.CurrentAddr == "") ? "<span class='green'>暂未填写</span>" : infos.CurrentAddr) + "</td></tr>"
                         + "<tr><td>生日:</td><td>" + ((infos.Birthday == "") ? "<span class='green'>暂未填写</span>" : infos.Birthday) + "</td></tr>"
                         + "<tr><td>班级:</td><td>" + ((infos.Class == "") ? "<span class='green'>暂未填写</span>" : infos.Class) + "</td></tr>"
                         + "<tr><td>爱好:</td><td>" + ((infos.Hobby == "") ? "<span class='green'>暂未填写</span>" : infos.Hobby) + "</td></tr>"
                         + "<tr><td>个人简介:</td><td>" + ((infos.Introduction == "") ? "<span class='green'>暂未填写</span>" : infos.Introduction) + "</td></tr>"
                         + "<tr><td><span class='blue'>联系方式</span></td><td></td></tr>"
                         + "<tr><td>QQ账号:</td><td>" + ((infos.QQ == "") ? "<span class='green'>暂未填写</span>" : infos.QQ) + "</td></tr>"
                         + "<tr><td>微信账号:</td><td>" + ((infos.WeiChat == "") ? "<span class='green'>暂未填写</span>" : infos.WeiChat) + "</td></tr>"
                         + "<tr><td>博客主页:</td><td>" + ((infos.Blog == "") ? "<span class='green'>暂未填写</span>" : "<a href='" + infos.Blog + "' target='_blank'>" + infos.Blog + "</a>") + "</td></tr>"
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

//显示指定用户的个人信息
function ShowUserInfo(id) {
    $.ajax({
        url: '/User/GetUserInfo',
        type: 'post',
        async: false,
        data: { id: id },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                var infos = eval('(' + data + ')');
                var htmlCode = "";
                htmlCode = "<tr><td>座右铭:</td><td>" + ((infos.Sign == "") ? "<span class='green'>暂未填写</span>" : infos.Sign) + "</td></tr>"
                         + "<tr><td>家乡:</td><td>" + ((infos.NativeAddr == "") ? "<span class='green'>暂未填写</span>" : infos.NativeAddr) + "</td></tr>"
                         + "<tr><td>现居地址:</td><td>" + ((infos.CurrentAddr == "") ? "<span class='green'>暂未填写</span>" : infos.CurrentAddr) + "</td></tr>"
                         + "<tr><td>生日:</td><td>" + ((infos.Birthday == "") ? "<span class='green'>暂未填写</span>" : infos.Birthday) + "</td></tr>"
                         + "<tr><td>班级:</td><td>" + ((infos.Class == "") ? "<span class='green'>暂未填写</span>" : infos.Class) + "</td></tr>"
                         + "<tr><td>爱好:</td><td>" + ((infos.Hobby == "") ? "<span class='green'>暂未填写</span>" : infos.Hobby) + "</td></tr>"
                         + "<tr><td>个人简介:</td><td>" + ((infos.Introduction == "") ? "<span class='green'>暂未填写</span>" : infos.Introduction) + "</td></tr>";
                $("#body-data").html(htmlCode);
                $("#panel_title").html("个人信息 - " + infos.Name);
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

//显示指定用户的联系方式
function ShowUserContact(id) {
    $.ajax({
        url: '/User/GetUserContact',
        type: 'post',
        async: false,
        data: { id: id },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                var infos = eval('(' + data + ')');
                var htmlCode = "";
                htmlCode = "<tr><td>QQ账号:</td><td>" + ((infos.QQ == "") ? "<span class='green'>暂未填写</span>" : infos.QQ) + "</td></tr>"
                         + "<tr><td>微信账号:</td><td>" + ((infos.WeiChat == "") ? "<span class='green'>暂未填写</span>" : infos.WeiChat) + "</td></tr>"
                         + "<tr><td>博客主页:</td><td>" + ((infos.Blog == "") ? "<span class='green'>暂未填写</span>" : "<a href='" + infos.Blog + "' target='_blank'>" + infos.Blog + "</a>") + "</td></tr>"
                         + "<tr><td>手机号码:</td><td>" + ((infos.Phone == "") ? "<span class='green'>暂未填写</span>" : infos.Phone) + "</td></tr>";
                $("#body-data").html(htmlCode);
                $("#panel_title").html("联系方式 - " + infos.Name);
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

//获取存在博客的天数信息
function ShowExistBlogsDays() {
    $.ajax({
        url: '/User/GetMonthBlogsCase',
        type: 'post',
        async: true,
        data: { year: calendar.Year, month: calendar.Month },
        success: function (data) {
            if (!$.isEmptyObject(data)) {
                var infos = eval('(' + data + ')');
                var i = 0;
                var targetYear = calendar.Year;
                var targetMonth = calendar.Month;
                for (i = 1; i <= infos.length; i++) {
                    if (infos[i - 1].Count > 0) {
                        calendar.Days[i].innerHTML = "<a href='/User/OneDayBlogs?year=" + targetYear + "&month=" + targetMonth + "&day=" + i + "' title='" + infos[i - 1].Count + "'>" + i + "</a>";
                    }
                }
            }
        },
        error: function (xhr) {
            //$("body").html(xhr.responseText);
        }
    });

}