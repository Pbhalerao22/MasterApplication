$(document).ready(function () {


    $('#ddlUsers').change(function () {
        var id = $('#ddlUsers :selected').val();
        window.location.href = "/v2/Admin/UserRoleMapping/GetUserRoleDetails?UserCode=" + id;

    })
    $(':radio[name=RoleOpt]').change(function () {

        var V_UserCode = $('#ddlUsers :selected').val();
        if (V_UserCode == "0") {
            toastr.warning('Please Select Valid User');
            return false;
        }

        var ClassObj = {};
        var userRoleArr = [];
        $('#gvUserRoleMapping tbody tr').each(function () {
            var V_RoleCode = $(this).find('td:eq(2)').text();
            var V_RoleName = $(this).find('td:eq(0)').text();
            var V_IsAssigned = $(this).find('td:eq(1)').find('input[type="radio"]');
            V_IsAssigned = $(V_IsAssigned)[0].checked;
            userRoleArr.push({ RoleCode: V_RoleCode, RoleName: V_RoleName, IsAssigned: V_IsAssigned, UserCode: V_UserCode });
        });

        $.ajax({
            type: "POST",
            url: "/v2/Admin/UserRoleMapping/UpdateUserRoleMapping",
            data: JSON.stringify(userRoleArr),
            // data: dataObject,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                window.location.href = "/v2/Admin/UserRoleMapping/GetUserRoleDetails?UserCode=" + V_UserCode;
            },
            error: function (response) {
                window.location.href = "/v2/Admin/UserRoleMapping/Index";
            }
        });


    })

    $(':checkbox[name=chkRole]').change(function () {
        if ($(this).prop('checked') == true) {
            $(this).parent().parent().find('td:eq(3) input').removeAttr('disabled')
        }
        else {
            $(this).parent().parent().find('td:eq(3) input').attr('disabled','disabled')
        }
        return false;
        var V_UserCode = $('#ddlUsers :selected').val();
        if (V_UserCode == "0") {
            toastr.warning('Please Select Valid User');
            return false;
        }

        var ClassObj = {};
        var userRoleArr = [];
        $('#gvUserRoleMapping tbody tr').each(function () {
            var V_RoleCode = $(this).find('td:eq(2)').text();
            var V_RoleName = $(this).find('td:eq(0)').text();
            var V_IsAssigned = $(this).find('td:eq(1)').find('input[type="checkbox"]');
            V_IsAssigned = $(V_IsAssigned)[0].checked;
            if (V_IsAssigned == true) {
                $(this).find('td:eq(3) input').removeAttr('disabled')
            }
            userRoleArr.push({ RoleCode: V_RoleCode, RoleName: V_RoleName, IsAssigned: V_IsAssigned, UserCode: V_UserCode });
        });

        $.ajax({
            type: "POST",
            url: "/UserRoleMapping/UpdateUserRoleMapping",
            data: JSON.stringify(userRoleArr),
            // data: dataObject,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                window.location.href = "/UserRoleMapping/GetUserRoleDetails?UserCode=" + V_UserCode;
            },
            error: function (response) {
                window.location.href = "/UserRoleMapping/Index";
            }
        });


    })
    $('#btnSubmit').click(function () {
        var V_UserCode = $('#ddlUsers :selected').val();
        if (V_UserCode == "0") {
            toastr.warning('Please Select Valid User');
            return false;
        }

        var ClassObj = {};
        var userRoleArr = [];
        var isValidData = false;
        $('#gvUserRoleMapping tbody tr').each(function () {
            var V_RoleCode = $(this).find('td:eq(2)').text();
            var V_RoleName = $(this).find('td:eq(0)').text();
            var V_IsAssigned = $(this).find('td:eq(1)').find('input[type="checkbox"]');
            var V_DefaultRole = $(this).find('td:eq(3)').find('input[type="radio"]');
            V_IsAssigned = $(V_IsAssigned)[0].checked;
            V_DefaultRole = $(V_DefaultRole)[0].checked;
            if (V_DefaultRole == true && V_IsAssigned ==true) {
                isValidData = true;
            }
            userRoleArr.push({ RoleCode: V_RoleCode, RoleName: V_RoleName, IsAssigned: V_IsAssigned, UserCode: V_UserCode, DefaultRole: V_DefaultRole });
        });
        if (isValidData == false) {
            toastr.warning('Kindly select default role for the user');
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/v2/Admin/UserRoleMapping/UpdateUserRoleMapping",
            data: JSON.stringify(userRoleArr),
            // data: dataObject,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                window.location.href = "/v2/Admin/UserRoleMapping/GetUserRoleDetails?UserCode=" + V_UserCode;
            },
            error: function (response) {
                window.location.href = "/v2/Admin/UserRoleMapping/Index";
            }
        });
    })
    
})