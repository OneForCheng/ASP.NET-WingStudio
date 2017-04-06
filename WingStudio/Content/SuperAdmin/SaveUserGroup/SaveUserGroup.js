function Checkform()
{
    var theme = Trim($("#Theme").val());
    if(theme.length == 0 || theme.length > 20)
    {
        swal("添加失败", "组名不能为空或超过20个字!");
        return false;
    }
    var auth = 0;
    $("[name='checkbox']").each(function () {
        if ($(this).is(':checked'))
        {
            auth += parseInt($(this).val());
        }
    });
    $("#Authority").val(auth);
    return true;
}