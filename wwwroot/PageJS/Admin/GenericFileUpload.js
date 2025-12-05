$(document).ready(function () {


    $('#drpXMLType').on('change', function () {

        var v_XMLTYPE = $('#drpXMLType :selected').val();
        $.ajax({
            type: "POST",
            url: "/v2/Admin/GenericFileUpload/BindPath",
            data: { XMLTYPE: v_XMLTYPE },
            dataType: "json",
            success: function (json) {
                var strFilePath = json;
                if (strFilePath != "") {
                    $("#txtFilePath").val(strFilePath);

                }
                else {
                    $("#txtFilePath").val("");
                }
                return false;
            },
            error: function (response) {
                toastr.error('Error Occurred fetching XML Path, Please contact Administrator');
                return true;
            }
        });
  
    });
});
