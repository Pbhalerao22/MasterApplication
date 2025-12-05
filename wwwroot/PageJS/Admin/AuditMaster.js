$(document).ready(function () {

    let isDateRangeRequired = false;
    let isUserRequired = false;

    $("#ddlType").change(function () {

        if ($.fn.DataTable.isDataTable('#auditTable')) {
            $('#auditTable').DataTable().destroy();
        }
        $("#auditTableContainer").html("");
        $("#fromDate").val("");
        $("#toDate").val("");
        $("#ddlUser").val("");
        const selected = $("#ddlType option:selected");
        const daterange = selected.data("daterange");
        const userdd = selected.data("userdd");

        // Toggle Date Range
        if (daterange == 1) {
            $("#dateRangeContainer").show();
            isDateRangeRequired = true;
        } else {
            $("#dateRangeContainer").hide();
            $("#fromDate").val('');
            $("#toDate").val('');
            isDateRangeRequired = false;
        }

        // Toggle User Dropdown
        if (userdd == 1) {
            $("#userContainer").show();
            isUserRequired = true;
        } else {
            $("#userContainer").hide();
            $("#ddlUser").val('');
            isUserRequired = false;
        }
    });

    // Default hidden states
    $("#dateRangeContainer").hide();
    $("#userContainer").show(); // usually shown by default

    $("#btnSearch").click(function () {
        const user = $("#ddlUser").val();
        const type = $("#ddlType").val();
        const fromDate = $("#fromDate").val();
        const toDate = $("#toDate").val();

        if (!type) {
            toastr.warning("Please select Type");
            return;
        }

        if (isUserRequired && !user) {
            toastr.warning("Please select User");
            return;
        }

        if (isDateRangeRequired) {
            if (!fromDate || !toDate) {
                toastr.warning("Please select From and To Date");
                return;
            }

            if (!isValidDateRange(fromDate, toDate)) {
                toastr.warning("Date range cannot exceed 1 month");
                return;
            }
        }

        $.ajax({
            url: '/V2/Admin/AuditMaster/GetAuditDetails',
            data: {
                Type: type,
                usercode: user,
                FromDate: fromDate,
                ToDate: toDate,
                IsDownload: "0"
            },
            type: 'GET',
            success: function (res) {
                if (res.success) {
                    renderTable(res.data);
                } else {
                    $("#auditTableContainer").html('<div class="alert alert-info text-center">No data found</div>');
                    toastr.info(res.message || "No data found");
                }
            },
            error: function () {
                toastr.error("Failed to fetch data");
            }
        });
    });

    $("#btnDownload").click(function () {
        const user = $("#ddlUser").val();
        const type = $("#ddlType").val();
        const fromDate = $("#fromDate").val();
        const toDate = $("#toDate").val();

        if (!type) {
            toastr.warning("Please select Type");
            return;
        }

        if (isUserRequired && !user) {
            toastr.warning("Please select User");
            return;
        }

        if (isDateRangeRequired) {
            if (!fromDate || !toDate) {
                toastr.warning("Please select From and To Date");
                return;
            }

            if (!isValidDateRange(fromDate, toDate)) {
                toastr.warning("Date range cannot exceed 1 month");
                return;
            }
        }

        window.location = `/V2/Admin/AuditMaster/GetAuditDetails?Type=${type}&usercode=${user}&FromDate=${fromDate}&ToDate=${toDate}&IsDownload=1`;
    });

    function isValidDateRange(fromDateStr, toDateStr) {
        const from = new Date(fromDateStr);
        const to = new Date(toDateStr);
        if (to < from) return false;

        const diffInMs = to - from;
        const diffInDays = diffInMs / (1000 * 60 * 60 * 24);
        return diffInDays <= 31;
    }

    function renderTable(data) {
        if (!data || data.length === 0) {
            $("#auditTableContainer").html('<div class="alert alert-warning text-center">No data found</div>');
            return;
        }

        let html = "<table id='auditTable' class='table table-bordered table-responsive table-sm text-nowrap compact'><thead><tr>";
        let cols = Object.keys(data[0]);
        cols.forEach(c => html += `<th>${c}</th>`);
        html += "</tr></thead><tbody>";

        data.forEach(row => {
            html += "<tr>";
            cols.forEach(c => html += `<td>${row[c] ?? ''}</td>`);
            html += "</tr>";
        });

        html += "</tbody></table>";
        $("#auditTableContainer").html(html);

        // Optional: Initialize DataTable
        if ($.fn.DataTable.isDataTable('#auditTable')) {
            $('#auditTable').DataTable().destroy();
        }
        $('#auditTable').DataTable({
            pageLength: 10,
            ordering: true,
            searching: true
        });
    }
});
