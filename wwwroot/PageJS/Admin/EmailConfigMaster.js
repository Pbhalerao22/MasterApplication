
$(document).ready(function () {
    LoadMasterOnLoad();
    function LoadMasterOnLoad() {
        $('#gvMaster').DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
            "pageLength": 10,
            "ajax": {
                "url": "/v2/v2/Admin/EmailConfigMaster/GetEmail",
                "type": "POST",
                "datatype": "json",

                data: function (dtParms) {

                    var info = $('#gvMaster').DataTable().page.info();

                    var JsonData = JSON.stringify(
                        {
                            draw: info.draw, PageNo: info.page, PageSize: info.length, SearchColumn: "PROCESS_TYPE", SearchValue: $("input[type='search']").val(),
                            SortColumn: "PROCESS_TYPE", SortType: "ASC"
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
            "columnDefs": [
                { targets: [1], visible: false },
                {
                    render: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    },
                    targets: 3
                },
                {
                    render: function (data, type, full, meta) {
                        return "<div class='text-wrap width-200'>" + data + "</div>";
                    },
                    targets: 4
                }
            ],

            "columns": [
                { "data": "PROCESS_TYPE", "name": "Process_Type", "autoWidth": true, "searchable": false },
                { "data": "CODE", "name": "CODE", "autoWidth": true, "searchable": true },
                { "data": "PROCESS_SUB_TYPE", "name": "Process_Sub_Type", "autoWidth": true, "searchable": true },
                { "data": "EMAIL_CONTENT", "name": "Email_Content", "autoWidth": true, "searchable": true },
                { "data": "ATTACHMENT_PATH", "name": "Attachment_Path", "autoWidth": true, "searchable": true },
              //  { "data": "ATTACHMENT_NAME", "name": "Attachment_Name", "autoWidth": true, "searchable": true },




                {
                    "data": "CODE",
                    render: function (data, type, row) {
                        //return '<a asp-action="Info" asp-route-EmpID=' + data + '>Info</a>  |<a asp-action="Edit" asp-route-EmpID=' + data + '>Edit</a>&nbsp;|&nbsp;<a asp-action="Delete" asp-route-EmpID=' + data + '>Delete</a>';
                        return  '<a class="btn btn-outline-primary btn-sm mb-1" style="text-decoration:none" href="/v2/Admin/EmailConfigMaster/Edit?Code=' + data + '"  style="cursor: pointer;"><i class="fe fe-edit me-2"></i>Edit</a> '  +
                            '<a class="btn btn-outline-danger btn-sm mb-1 MasterDelete" id="' + data + '"   style="cursor: pointer;"><i class="fe fe-trash me-2"></i>Delete</a>'

                    }
                },


            ], "fnInitComplete": function (response) {
                console.log(response);
                $('#gvMaster').css('width', '100%')
            }, "error": function (response) {
                console.log("error");
                console.log(response);
                console.log("----------------");
            }

        });
    }
    $('#gvMaster').on('click', '.MasterDelete', function () {
        var code = ($(this).attr('id'));
        Swal.fire({
            title: 'Do you want to delete?',
            showDenyButton: true,
            confirmButtonText: 'Delete',
            denyButtonText: `Cancel`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                window.location.href = '/v2/Admin/EmailConfigMaster/Delete?Code=' + code;
            } else if (result.isDenied) {

            }
        })


    })


})