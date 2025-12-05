$(document).ready(function () {
    // .net core jquery ajax fill select

    LoadMasterOnLoad();
    LoadUserInDropDown();
    loadMneu();


    //$.ajax({
    //    type: "POST",
    //    url: "/NewUser/GetDataUser",
    //    data: "{}",
    //    success: function (data) {
    //        $("#lblNotes").text(data);
    //    },
    //    error: function (x) {
    //        alert('message');
    //    }
    //}


});
function LoadMasterOnLoad() {
    $('#gvMaster').DataTable({
        "destroy": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        "pageLength": 10,
        "ajax": {
            "url": "/v2/Admin/NewUser/GetNewUser",
            "type": "POST",
            "datatype": "json",

            data: function (dtParms) {

                var info = $('#gvMaster').DataTable().page.info();

                var JsonData = JSON.stringify(
                    {
                        draw: info.draw, PageNo: info.page, PageSize: info.length, SearchColumn: "UserName", SearchValue: $("input[type='search']").val(),
                        SortColumn: "UserName", SortType: "ASC",
                    });

                return JsonData;

            },
            "dataSrc": function (json) {
                json.d = json;
                json.draw = json.d.draw;
                json.recordsTotal = json.d.recordsTotal;
                json.recordsFiltered = json.d.recordsFiltered;
                json.data = JSON.parse(json.d.data);
                var return_data = json;
                return return_data.data;
            },
        },

        "columns": [
            { "data": "USERNAME", "name": "USERNAME", "autoWidth": true, "searchable": false },
            { "data": "USERCODE", "name": "USERCODE", "autoWidth": true, "searchable": false },



        ],
    });
}

function LoadUserInDropDown() {
    ////username: "Rahul"
    ////value: 0
    //var username = "Rahul";
    //var value = 0;

    $.ajax({
        type: "POST",
        url: "/v2/Admin/NewUser/GetDataUser",
        data: "{}",
        success: function (data) {
            $('#ddlUser').empty();
            $('#ddlUser').append("<option value='0'>--Select--</option>");
            $.each(data, function (key, value) {
                $("#ddlUser").append($("<option></option>").val(value.username).html(value.username));
            });
        },
        error: function (response) {

        }
    });
    //$(document).ready(function () {
    //    $("#btnLdmodel").click(function () {
    //        var data = $("#ddlUser option:selected");
    //        alert($("#ddlUser").val());
    //    });
    //});
    //$(document).ready(function () {
    //    $("#btnLdmodel").click(function () {
    //        var value = $("#ddlUser option:selected");
    //        alert(value.text());
    //    });

    //});

    //$(document).ready(function () {
    //    $('#btnLdmodel').click(function () {
    //        var ("username") = $('#ddlUser').val();

    //    });

    //});
    //$('#btnLdmodel').click(function () {
    //    alert('Alter with jQuery Button Clicked');
    //});

}

function loadMneu() {


    $(".drpdown").change(function () {
        var username = ($('#ddlUser option:selected').text());

        //var username = "";
        //alert('1');
        $.getJSON('/v2/Admin/NewUser/GetNew',
            {
                value: "1",
                username: username
            },

            function success(data) {
                var responseData = $.parseJSON(data);
                var UserDetails = "";
                UserDetails = responseData.Table[0].USERNAME + "-" + responseData.Table[0].FULLNAME + "-" + responseData.Table[0].MOBILENO + "-" + responseData.Table[0].GENDER;
                $('#lblresponsedata').text(UserDetails);
                /*  alert(UserDetails);*/
                $('#exampleModal').modal('show');
            },


        ).fail(function (res) {
            toastr["error"]("", "Error occurred while fetching template table details");
        });
    });
}
