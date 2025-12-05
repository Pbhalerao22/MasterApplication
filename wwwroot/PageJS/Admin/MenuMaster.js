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
                "url": "/v2/Admin/MenuMaster/GetMenus",
                "type": "POST",
                "datatype": "json",

                data: function (dtParms) {

                    var info = $('#gvMaster').DataTable().page.info();

                    var JsonData = JSON.stringify(
                        {
                            draw: info.draw, PageNo: info.page, PageSize: info.length, SearchColumn: "MENUNAME", SearchValue: $("input[type='search']").val(),
                            SortColumn: "MENUNAME", SortType: "ASC"
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
                { targets: [1], visible: false }
            ],

            "columns": [
                { "data": "MENUNAME", "name": "MENUNAME", "autoWidth": true, "searchable": false },
                { "data": "CODE", "name": "CODE", "autoWidth": true, "searchable": true },
                { "data": "MENUDESC", "name": "MENUDESC", "autoWidth": true, "searchable": false },
                { "data": "CONTROLLERNAME", "name": "CONTROLLERNAME", "autoWidth": true, "searchable": false },
                { "data": "ACTIONNAME", "name": "ACTIONNAME", "autoWidth": true, "searchable": false },
                { "data": "LOCKED", "name": "LOCKED", "autoWidth": true, "searchable": false },


                {
                    "data": "CODE",
                    render: function (data, type, row) {
                        return '<a class="btn btn-outline-primary btn-sm mb-1" style="text-decoration:none" href="/v2/Admin/MenuMaster/Edit?Code=' + data + '"  style="cursor: pointer;"><i class="fe fe-edit me-2"></i>Edit</a> ' +
                            '<a class="btn btn-outline-danger MasterDelete btn-sm mb-1" id="' + data + '"  style="cursor: pointer;"><i class="fe fe-trash me-2"></i>Delete</a>'

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
                window.location.href = '/v2/Admin/MenuMaster/Delete?Code=' + code;
            } else if (result.isDenied) {

            }
        })


    })


})