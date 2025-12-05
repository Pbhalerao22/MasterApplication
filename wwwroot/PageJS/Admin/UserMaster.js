$(document).ready(function () {
    LoadMasterOnLoad();
    function LoadMasterOnLoad() {
        $('#gvMaster').DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "lengthMenu": [[10, 25, 50, 100], [ 10, 25, 50, 100]],
            "pageLength": 10,
            "ajax": {
                "url": "/v2/Admin/UserMaster/GetUser",
                "type": "POST",
                "datatype": "json",

                data: function (dtParms) {

                    var info = $('#gvMaster').DataTable().page.info();
                    
                    var JsonData = JSON.stringify(
                        {
                            draw: info.draw, PageNo: info.page, PageSize: info.length, SearchColumn: "UserName", SearchValue: $("input[type='search']").val(),
                            SortColumn: "UserName", SortType: "ASC"
                        });
                    return JsonData;


                },
                "dataSrc": function (json) {
                    json.d = json;
                    json.draw = json.d.draw;
                    json.recordsTotal = json.d.recordsTotal;
                    json.recordsFiltered = json.d.recordsFiltered;
                    json.data = JSON.parse( json.d.data);
                    var return_data = json;
                    return return_data.data;
                },
            },
            "columnDefs": [
                { targets: [1], visible: false }
                ],

            "columns": [
                { "data": "USERNAME", "name": "USERNAME", "autoWidth": true, "searchable": false },
                { "data": "USERCODE", "name": "USERCODE", "autoWidth": true, "searchable": true },
                { "data": "FULLNAME", "name": "FULLNAME", "autoWidth": true, "searchable": false },
                {
                    "data": "ISADMIN",
                    render: function (data, type, row) {
                        
                        return data == "0" ? "No" : "Yes";

                    }
                },
                {
                    "data": "ISADUSER",
                    render: function (data, type, row) {

                        return data == "0" ? "No" : "Yes";

                    }
                },
                { "data": "LOCKED", "name": "LOCKED", "autoWidth": true, "searchable": false },
                
                
                {
                    "data": "USERCODE",
                    render: function (data, type, row) {
                        return '<a class="btn btn-outline-primary btn-sm mb-1" style="text-decoration:none" href="/v2/Admin/UserMaster/Edit?Code=' + data + '" style="cursor: pointer;"><i class="fe fe-edit me-2"></i>Edit</a> ' +
                            '<a class="btn btn-outline-danger btn-sm mb-1 MasterDelete" id="' + data + '"  style="cursor: pointer;"><i class="fe fe-trash me-2"></i>Delete</a>'
                        
                    }
                },


            ], "fnInitComplete": function (response) {
                console.log(response);
                $('#gvMaster').css('width','100%')
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
                window.location.href = '/v2/Admin/UserMaster/Delete?Code=' + code;
            } else if (result.isDenied) {
                
            }
        })

       
    })

})