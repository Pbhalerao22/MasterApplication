$(document).ready(function () {
    var NumChar = 100;
    LoadMasterOnLoad();
    function LoadMasterOnLoad() {
        $('#gvMaster').DataTable({
            "processing": true,
            "serverSide": true,
            "filter": true,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
            "pageLength": 10,
            "autoWidth": false,
            "ajax": {
                "url": "/v2/Admin/ExceptionLog/GetLog",
                "type": "POST",
                "datatype": "json",

                data: function (dtParms) {

                    var info = $('#gvMaster').DataTable().page.info();

                    var JsonData = JSON.stringify(
                        {
                            draw: info.draw, PageNo: info.page, PageSize: info.length, SearchColumn: "MACHINENAME", SearchValue: $("input[type='search']").val(),
                            SortColumn: "MACHINENAME", SortType: "ASC"
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
                { targets: [0], visible: false },
                { targets: [1], visible: false }
            ],

            "columns": [
                { "data": "USERCODE", "name": "USERCODE", "searchable": false },
                { "data": "CODE", "name": "CODE", "searchable": false},
                { "data": "MACHINENAME", "name": "MACHINENAME", "searchable": true  },
                { "data": "URL", "name": "URL", "searchable": false },
                
                {
                    "data": "EXCEPTIONDETAILS", "class": "tdException", "name": "EXCEPTIONDETAILS", "searchable": false, "render": function (data, type, row) {
                        var CurrData = data;
                      
                        if (data.length > NumChar) {
                          
                            CurrData = data.substring(0, NumChar) + ' <label class="morelink" >Show more</label>';
                        }
                        
                        return CurrData;
                    }

                },
                { "data": "LOGDATE", "name": "LOGDATE", "searchable": false }





            ],"createdRow": function (row, data, dataIndex) {
                // Set the data-status attribute, and add a class
                $(row).find('td:eq(2)')
                    .attr('EXCEPTIONDETAILS', data.EXCEPTIONDETAILS)
                    
            }, "fnInitComplete": function (response) {
               
                $('#gvMaster').css('width', '100%')
            }, "error": function (response) {
                console.log("error");
                console.log(response);
                console.log("----------------");
            }

        });
    }
    $(document).on('click', ".morelink", function () {
        var FullText = $(this).parent().attr('EXCEPTIONDETAILS')
        
        var NumLines = Math.ceil(FullText.length / NumChar);
        var FinalText = '';
        for (var i = 0; i < NumLines; i++) {
            var start = i * NumChar;
            var end = NumChar;
            if (i != 0) {
                start = (i * NumChar) + 1;
                end = (start + NumChar)-1;
            }
            var subText = FullText.substring(start, end);
            FinalText += subText + " </br> ";
        }
        FinalText += '<label class="ShowLess" >Show less</label>'
        $(this).parent().html(FinalText)
    })
    $(document).on('click', ".ShowLess", function () {
        var FullText = $(this).parent().attr('EXCEPTIONDETAILS')
        
        var subText = FullText.substring(0, NumChar);
        var FinalText = subText ;
        FinalText += ' </br> <label class="morelink" >Show more</label>'
        $(this).parent().html(FinalText)
    })
    
  
})