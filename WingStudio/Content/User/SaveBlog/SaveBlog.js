/* 检查提交表单是否合格 */
function Checkform() {
    var theme = $("#Theme").val();
    if (Trim(theme).length == 0 || Trim(theme).length > 40) {
        swal("错误提示", "标题不能为空或超过40个字!");
        return false;
    }
    var content = $("textarea[name='Content']").val();
    if (Trim(content) == "") {
        swal("错误提示", "博客正文不能为空!");
        return false;
    }
    return true;
}

//博客入榜
function BlogToTopList(id)
{
    if (id != 0)
    {
        $('#SaveBlogForm').attr("action", "/User/ModBlogToTopList/" + id);
        $('#SaveBlogForm').attr("method", "post");
        $('#SaveBlogForm').submit();
    }
    else
    {
        $('#SaveBlogForm').attr("action", "/User/AddBlogToTopList");
        $('#SaveBlogForm').attr("method", "post");
        $('#SaveBlogForm').submit();
    }
}