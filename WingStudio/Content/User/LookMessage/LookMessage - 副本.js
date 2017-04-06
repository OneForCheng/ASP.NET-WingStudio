$(document).ready(function () {
    var content = $("#html_content").text();
    $("#html_content").html(filterXSS(content)).show();
});