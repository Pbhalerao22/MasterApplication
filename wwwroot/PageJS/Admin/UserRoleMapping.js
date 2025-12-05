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

        //$.ajax({
        //    type: "POST",
        //    url: "/UserRoleMapping/UserRoleMapping",
        //    data: JSON.stringify(userRoleArr),
        //    // data: dataObject,
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (response) {
        //        window.location.href = "/UserRoleMapping/GetUserRoleDetails?UserCode=" + V_UserCode;
        //    },
        //    error: function (response) {
        //        window.location.href = "/UserRoleMapping/Index";
        //    }
        //});
        $.ajax({
            type: "POST",
            url: "/v2/Admin/UserRoleMapping/UserRoleMapping",
            data: '{testvalue : "TEst"}', //JSON.stringify(userRoleArr),
            contentType: "application/json",
            dataType: "json",
            success: function (response) {
                if (response.status.toLowerCase() == "saved") {
                    toastr.success('Data Updated Successfully!');
                }
                else {
                    toastr.warning('Could not save the information, Contact Administration!!');
                }
                return false;

            },
            error: function (res) {
                toastr.error('Error Occurred while updating data, Please contact Administrator');
                return true;
            }
        });

    })

})