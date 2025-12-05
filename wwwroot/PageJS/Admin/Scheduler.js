$(document).ready(function () {
    $('#btnUpdate').click(function () {
        var Time = $('#EXtime').val();
        var regex = /^((0[1-9])|(1[0-2])):[0-5][0-9]:(A|P|a|p)(M|m)$/;
        if (!regex.test(Time)) {
            toastr.warning('Kindly Enter Valid Time');
            return false;
        }

    });
    $(".C360-datepicker").datepicker({ dateFormat: 'dd-mm-yy' });
    LoadMasterOnLoad();
    function LoadMasterOnLoad() {
        $('#gvMaster').DataTable({
            "processing": true,
            "serverSide": true,
            "sorting": false,
            "ordering": false,
            "filter": true,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
            "language": { searchPlaceholder: "PROCESS TYPE" },
            "pageLength": 10,
            "ajax": {
                "url": "/v2/Admin/Scheduler/GetSchedulerDetails",
                "type": "POST",
                "datatype": "json",

                data: function (dtParms) {

                    var info = $('#gvMaster').DataTable().page.info();

                    var JsonData = JSON.stringify(
                        {
                            draw: info.draw, PageNo: info.page, PageSize: info.length, SearchColumn: "PROCESS_TYPE",
                            SearchValue: $("input[type='search']").val(),
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
                { targets: [0], visible: false }
            ],

            "columns": [
                { "data": "SYS_CODE", "name": "SYS_CODE", "autoWidth": false, "searchable": false },
                { "data": "PROJECT_NAME", "name": "PROJECT_NAME", "autoWidth": false, "searchable": false },
                { "data": "PROCESS_TYPE", "name": "PROCESS_TYPE", "autoWidth": false, "searchable": false },
                { "data": "EXECUTION_DATE", "name": "EXECUTION_DATE", "autoWidth": false, "searchable": false },
                { "data": "EXECUTION_TIME", "name": "EXECUTION_TIME", "autoWidth": false, "searchable": false },
                { "data": "PROCESS_STATUS", "name": "PROCESS_STATUS", "autoWidth": false, "searchable": false },

                {
                    "data": "SYS_CODE",
                    render: function (data, type, row) {
                        return '<a class="btn btn-outline-primary btn-sm mb-1" style="text-decoration:none" href="/v2/Admin/Scheduler/Edit?Code=' + data + '"  style="cursor: pointer;"><i class="fe fe-edit me-2"></i>Edit</a> '

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

})
