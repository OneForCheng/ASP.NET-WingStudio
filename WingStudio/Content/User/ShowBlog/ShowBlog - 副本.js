$(document).ready(function () {
    var content = $("#html_content").text();
    $("#html_content").html(filterXSS(content)).show();
    SyntaxHighlighter.config.bloggerMode = true;
    SyntaxHighlighter.highlight();
});