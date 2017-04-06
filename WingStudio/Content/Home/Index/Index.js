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

    $('.dynamic-panel .owl-carousel').owlCarousel({
        loop: true,
        margin: 5,
        responsiveClass: true,
        nav: false,
        autoplay:true,
        autoplayTimeout:3000,
        autoplayHoverPause:true,
        responsive: {
            0: {
                items: 1,
                loop: false,
            },
            400: {
                items: 2
            },
            600:{
                items: 3
            },
            800: {
                items: 4
            },
            1000: {
                items: 5
            }
        }
    });
    $('.share-panel .owl-carousel').owlCarousel({
        loop: true,
        margin: 5,
        responsiveClass: true,
        nav: false,
        responsive: {
            0: {
                items: 1,
                loop: false,
            },
            400: {
                items: 2
            },
            600: {
                items: 3
            },
            800: {
                items: 4
            },
            1000: {
                items: 5
            }
        }
    });
    $("#resource_share_info").hide();
});

//切换公告显示
function NoticeToggle(isLong) {
    if (isLong) {
        $("#notice_tab").removeClass("tabactive");
        $("#long_notice_tab").addClass("tabactive");
        $("#long_notice_info").show();
        $("#notice_info").hide();
    }
    else {
        $("#long_notice_tab").removeClass("tabactive");
        $("#notice_tab").addClass("tabactive");
        $("#notice_info").show();
        $("#long_notice_info").hide();

    }
}

//切换动态显示
function DynamicToggle(isFormal) {
    if (isFormal) {
        $("#unformal_dynamic_tab").removeClass("tabactive");
        $("#formal_dynamic_tab").addClass("tabactive");
        $("#formal_dynamic_info").show();
        $("#unformal_dynamic_info").hide();
    }
    else {
        $("#formal_dynamic_tab").removeClass("tabactive");
        $("#unformal_dynamic_tab").addClass("tabactive");
        $("#unformal_dynamic_info").show();
        $("#formal_dynamic_info").hide();
    }
}

//切换分享区显示
function ShareToggle(isResource) {
    if (isResource) {
        $("#blog_share_tab").removeClass("tabactive");
        $("#resource_share_tab").addClass("tabactive");
        $("#resource_share_info").show();
        $("#blog_share_info").hide();
    }
    else {
        $("#resource_share_tab").removeClass("tabactive");
        $("#blog_share_tab").addClass("tabactive");
        $("#resource_share_info").hide();
        $("#blog_share_info").show();
    }
}

//切换搜索目标
function SearchTarget(target, action, placeholder)
{
    if(target == "blog")
    {
        $("#search_form").attr("action", action);
        $("#search_input").attr("placeholder", placeholder);
    }
    else
    {
        $("#search_form").attr("action", action);
        $("#search_input").attr("placeholder", placeholder);
    }
}

//获取存在博客的天数信息
function ShowExistBlogsDays()
{
    $.ajax({
        url: '/GetMonthBlogsCase',
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
                    if(infos[i - 1].Count > 0)
                    {
                        calendar.Days[i].innerHTML = "<a href='/OneDayBlogs?year=" + targetYear + "&month=" + targetMonth + "&day=" + i + "' title='" + infos[i - 1].Count + "'>" + i + "</a>";
                    }
                }
            }
        },
        error: function (xhr) {
            //$("body").html(xhr.responseText);
        }
    });
   
}