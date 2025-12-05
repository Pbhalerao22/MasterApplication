$(document).ready(function () {
    LoadMasterOnLoad($('#drpRoleCode option:selected').val(),'Page Load');
    function LoadMasterOnLoad(vRoleCode, vCaller){
        $('#gvMaster').DataTable({
            "destroy": true,
            "processing": true,
            "serverSide": true,
            "filter": true,
            "lengthMenu": [[10, 25, 50, 100], [10, 25, 50, 100]],
            "pageLength": 10,
            "ajax": {
                "url": "/v2/Admin/RoleMenuMapping/GetRoles",
                "type": "POST",
                "datatype": "json",
                data: function (dtParms) {
                  var info = $('#gvMaster').DataTable().page.info();
                      var JsonData = JSON.stringify(
                        {
                            draw: info.draw, PageNo: info.page, PageSize: info.length, SearchColumn: "MENUNAME", SearchValue: $("input[type='search']").val(),
                            SortColumn: "MENUNAME", SortType: "ASC", RoleCode: vRoleCode

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
            ],

            "columns": [
                { "data": "MENUNAME", "name": "MENUNAME", "autoWidth": true, "searchable": false },
                { "data": "CODE", "name": "CODE", "autoWidth": true, "searchable": true },
                {

                    "data": "ISASSIGNED",

                    render: function (data, type, row) {
                        if (type === 'display') {
                            if (data == "True") {
                                return '<input type="checkbox" class="chkAssign" id="chkIsAssigned" checked="true">';
                            } else {
                                return '<input type="checkbox" class="chkAssign" id="chkIsAssigned" >';
                            }
                        }
                        return data;
                    },

                },


            ],
            'createdRow': function (row, data, dataIndex) {
                $(row).attr('MenuCode', data.MENUNAME);
            }
            , "fnInitComplete": function (response) {
                console.log(response);
                $('#gvMaster').css('width', '100%')
            }, "error": function (response) {
                console.log("error");
                console.log(response);
                console.log("----------------");
            }

        });
    }

    $("#drpRoleCode").change(function () {
        var vRoleCode = $('#drpRoleCode option:selected').val();
        LoadMasterOnLoad(vRoleCode,'Role Change');

    });

    $(document).on('click', '#chkAllMenu', function () {
        var ObjClass = {}
        var ApprovArray = []

        var chkIsAssigned = $(this).prop('checked');
        console.log('Printing My Data')
        console.log(chkIsAssigned )
        
        var RoleName = $("#drpRoleCode").val();
         $('#gvMaster tbody tr').each(function (i, e) {
        var curtr = $(this);
        var MenuCode = $(curtr).closest('tr').attr('MenuCode');

             var CurrCheckboxState = chkIsAssigned.toString();
        ApprovArray.push({ RoleCode: RoleName, MenuName: MenuCode, IsAssgined: CurrCheckboxState })

         });

        if (ApprovArray.length > 0) {
            ObjClass = ApprovArray[0];

            $.ajax({
                type: "POST",
                url: "/v2/Admin/RoleMenuMapping/UpdateRoleMenu",
                data: JSON.stringify(ApprovArray),
                contentType: "application/json",
                dataType: "json",
                success: function (response) {
                    if (response.status.toLowerCase() == "saved") {
                        toastr.success('Data Updated Successfully!');
                    }
                    else {
                        toastr.warning('Could not save the informatiSecurityProtocolTypeon, Contact Administration!!');
                    }
                    return false;

                },
                error: function (res) {
                    toastr.error('Error Occurred while updating data, Please contact Administrator');
                    return true;
                }
            });

        }
    });
    
    $(document).on('click', '#gvMaster tbody tr input[type=checkbox]', function () {

        var RoleName = $("#drpRoleCode").val();
        if (RoleName == "0") {
            toastr.warning('Please Select Valid Role');
            return false;
        }

        var ObjClass = {}
        var ApprovArray = []

        tbl = $('#gvMaster').DataTable();
        var data = tbl.rows().data();

        
        
        var curtr = $(this).closest('tr');
            var MenuCode = $(curtr).attr('MenuCode');

        var CurrCheckboxState = $(curtr).find('td:eq(1)').find('input[type=checkbox]').prop('checked').toString();
        console.log('printing my data')
        console.log(CurrCheckboxState)
            ApprovArray.push({ RoleCode: RoleName, MenuName: MenuCode, IsAssgined: CurrCheckboxState })

     

        if (ApprovArray.length > 0) {
            ObjClass = ApprovArray[0];

            $.ajax({
                type: "POST",
                url: "/v2/Admin/RoleMenuMapping/UpdateRoleMenu",
                data: JSON.stringify(ApprovArray ),
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

        }
    });

   

  
   
});