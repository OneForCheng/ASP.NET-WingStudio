function Checkform() {
    var theme = Trim($("#Theme").val());
    if (theme.length == 0 || theme.length > 20) {
        swal("添加失败", "组名不能为空或超过20个字!");
        return false;
    }
    var des = Trim($("#Description").val());
    if (des.length == 0 || des.length > 40) {
        swal("添加失败", "描述不能为空或超过40个字!");
        return false;
    }
    var acce = 0;
    $("[name='checkbox']").each(function () {
        if ($(this).is(':checked')) {
            acce += parseInt($(this).val());
        }
    });
    $("#Accessible").val(acce);
    return true;
}