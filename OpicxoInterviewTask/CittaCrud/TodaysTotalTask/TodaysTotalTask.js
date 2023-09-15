$(function () {
    fnAwaitBindSelectDropdowns();

    if ($("#RightsGroupName").val().toLowerCase() == "admin" || $("#RightsGroupName").val().toLowerCase() == "super admin") {
        $('#AssignToUI').append(new Option("All", "0", true, true));
    } else {
        $('#AssignToUI').append(new Option($("#UserName").val(), $("#UserId").val(), true, true));
        $("#AssignToUI").prop("disabled", true);
    }
    $("#AssignToIds_hdn").val($('#AssignToUI').val());

    if ($("#tab_1").hasClass('active')) {
        $('.FollowUpsStatus').show();
        $('.SitesVisitsStatus').hide();
    }
    if ($("#tab_2").hasClass('active')) {
        $('.FollowUpsStatus').hide();
        $('.SitesVisitsStatus').show();
    }

    //$('#FollowUpsStatusIdUI').append(new Option("In Process", "0", false, false));
    //$('#FollowUpsStatusIdUI').append(new Option("Completed", "1", false, false));
    $("#FollowUpsStatusIds_hdn").val($('#FollowUpsStatusIdUI').val());

    //$('#SitesVisitsStatusIdUI').append(new Option("Pending", "1", false, false));
    //$('#SitesVisitsStatusIdUI').append(new Option("Complete", "2", false, false));
    //$('#SitesVisitsStatusIdUI').append(new Option("Cancelled", "3", false, false));
    $("#SitesVisitsStatusIds_hdn").val($('#SitesVisitsStatusIdUI').val());

});

$("#FollowUpsStatusIdUI").on('change', function () {
    $("#FollowUpsStatusIds_hdn").val($('#FollowUpsStatusIdUI').val());
    fnFromDateToDate();
});
$("#SitesVisitsStatusIdUI").on('change', function () {
    $("#SitesVisitsStatusIds_hdn").val($('#SitesVisitsStatusIdUI').val());
    fnFromDateToDate();
});

$("#AssignToUI").on('change', function () {
    if ($("#AssignToIds_hdn").val() == '0') {
        $("#AssignToUI option[value='0']").remove();
    }
    $("#AssignToIds_hdn").val($('#AssignToUI').val());
    fnFromDateToDate();
});

async function fnAwaitBindSelectDropdowns() {
    await fnBindSelectDropdowns();
}

async function fnBindSelectDropdowns() {
    $('#AssignToUI').select2({
        ajax: {
            url: '/Common/GetAssignToSearchList',
            type: 'POST',
            async: false,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Employee',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: true,
        multiple: true
    });

    //$('#FollowUpsStatusIdUI').select2({
    //    placeholder: 'Select Status',
    //    minimumInputLength: minimumInputLength,
    //    width: '100%',
    //    allowClear: false
    //});
    //$('#SitesVisitsStatusIdUI').select2({
    //    placeholder: 'Select Status',
    //    minimumInputLength: minimumInputLength,
    //    width: '100%',
    //    allowClear: false
    //});
}

$(document).ready(function () {

    $('#FromDate').val(formatDateFT(new Date()));
    $('#ToDate').val(formatDateFT(new Date()));
    BindListfollowups();
    BindListSitesVisits();
    fnIsFromAddActivityPartial();
});

function padTo2DigitsFT(num) {
    return num.toString().padStart(2, '0');
}

function formatDateFT(date) {
    return (
        [
            padTo2DigitsFT(date.getDate()),
            padTo2DigitsFT(date.getMonth() + 1),
            date.getFullYear(),
        ].join('/')
    );
}

function BindListfollowups() {
    if ($.fn.DataTable.isDataTable('#data-table-follow-ups')) {
        $('#data-table-follow-ups').DataTable().destroy();
        fnIsFromAddActivityPartial();
    }
    var table1 = $('#data-table-follow-ups').DataTable({
        "pageLength": $('#PageSize').val(),
        "lengthChange": true,
        "paging": true,
        "autoWidth": true,
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "order": [0, "desc"],
        "ajax": {
            "url": "/Transactions/TodaysTotalTask/GetListTodaysTotalTask",
            "type": "POST",
            "datatype": "json",
            "data": function (o) {
                o.FromDate = $('#FromDate').val();
                o.ToDate = $('#ToDate').val();
                o.AssignTo = $('#AssignToIds_hdn').val();
                o.StatusIds = $('#FollowUpsStatusIds_hdn').val();
                o.ActivityTypeId = '1';
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "LONo", "autoWidth": true },
            { "data": "Date", "autoWidth": true },
            { "data": "ModuleType", "autoWidth": true },
            { "data": "SubjectName", "autoWidth": true },
            { "data": "AssignedName", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "FollowupObjectiveName", "autoWidth": true },
            { "data": "NextFollowUpDate", "autoWidth": true },
            { "data": "CustomerName", "autoWidth": true },
            { "data": "CustomerContectNo", "autoWidth": true },
            { "data": "Requirement", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    if (full.FollowUpsStatusName.toLowerCase() == 'completed') {
                        return '<span class="badge badge-success">' + full.FollowUpsStatusName + '</span>';
                    } else {
                        return '<span class="badge badge-warning">' + full.FollowUpsStatusName + '</span>';
                    }
                }
            },
            {
                mRender: function (data, type, full) {
                    return '<a target="_blank" href="/Transactions/' + full.ModuleType + '/View_New?EncryptedId=' + full.EncryptedId + '" title="View" class="btn btn-success btn-sm"><i class="fas fa-eye"></i></a>';
                }
            },
            {
                mRender: function (data, type, full) {
                    if (full.ModuleType == "Lead") {
                        return '<a href="javascript:;" onclick="fnOpenActivity(\'' + full.LeadEncryptedId + '\',' + full.LeadStageId + ')" title="Add" class="btn btn-success btn-sm onhoverLead"><i class="fas fa-plus"></i></a>';
                    } else {
                        return '<a href="javascript:;" onclick="fnOpenActivity(\'' + full.LeadEncryptedId + '\',' + full.LeadStageId + ',' + full.LeadId + ',1)" title="Add" class="btn btn-success btn-sm onhoverOpportunity"><i class="fas fa-plus"></i></a>';
                    }
                }
            }
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13] }
        ],
        "drawCallback": function () {
            $("#FollowUpsCount").html($('#data-table-follow-ups').DataTable().page.info().recordsTotal);
            $("#SitesVisitsCount").html($('#data-table-Sites-Visits').DataTable().page.info().recordsTotal);
            fnIsFromAddActivityPartial();
            $('.onhoverLead').hover(function () {
                $("#IsFrom_hdn").val('1');
            });
            $('.onhoverOpportunity').hover(function () {
                $("#IsFrom_hdn").val('2');
            });
        }
    });
}

function BindListSitesVisits() {
    if ($.fn.DataTable.isDataTable('#data-table-Sites-Visits')) {
        $('#data-table-Sites-Visits').DataTable().destroy();
        fnIsFromAddActivityPartial();
    }
    var table2 = $('#data-table-Sites-Visits').DataTable({
        "pageLength": $('#PageSize').val(),
        "lengthChange": true,
        "paging": true,
        "autoWidth": true,
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "order": [0, "desc"],
        "ajax": {
            "url": "/Transactions/TodaysTotalTask/GetListTodaysTotalTask",
            "type": "POST",
            "datatype": "json",
            "data": function (o) {
                o.FromDate = $('#FromDate').val();
                o.ToDate = $('#ToDate').val();
                o.AssignTo = $('#AssignToIds_hdn').val();
                o.StatusIds = $('#SitesVisitsStatusIds_hdn').val();
                o.ActivityTypeId = '4';
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "LONo", "autoWidth": true },
            { "data": "Date", "autoWidth": true },
            { "data": "PropertyName", "autoWidth": true },
            { "data": "AssignedName", "autoWidth": true },
            { "data": "InterestedProject", "autoWidth": true },
            { "data": "MaxBudget", "autoWidth": true },
            { "data": "SitevisitDescription", "autoWidth": true },
            { "data": "Remarks", "autoWidth": true },
            { "data": "NextSiteVisitPlan", "autoWidth": true },
            { "data": "CustomerName", "autoWidth": true },
            { "data": "CustomerContectNo", "autoWidth": true },
            { "data": "Requirement", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    if (full.SiteVisitStatusName.toLowerCase() == 'pending') {
                        return '<span class="badge badge-warning">' + full.SiteVisitStatusName + '</span>';
                    } else if (full.SiteVisitStatusName.toLowerCase() == 'complete') {
                        return '<span class="badge badge-success">' + full.SiteVisitStatusName + '</span>';
                    } else {
                        return '<span class="badge badge-danger">' + full.SiteVisitStatusName + '</span>';
                    }
                }
            },
            {
                mRender: function (data, type, full) {
                    return '<a target="_blank" href="/Transactions/Opportunity/View_New?EncryptedId=' + full.EncryptedId + '" title="View" class="btn btn-success btn-sm"><i class="fas fa-eye"></i></a>';
                }
            },
            {
                mRender: function (data, type, full) {
                    return '<a href="javascript:;" onclick="fnOpenActivity(\'' + full.LeadEncryptedId + '\',' + full.LeadStageId + ',' + full.LeadId + ',4)" title="Add" class="btn btn-success btn-sm onhoverOpportunity"><i class="fas fa-plus"></i></a>';
                }
            }
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] }
        ],
        "drawCallback": function () {
            $("#SitesVisitsCount").html($('#data-table-Sites-Visits').DataTable().page.info().recordsTotal);
            $("#FollowUpsCount").html($('#data-table-follow-ups').DataTable().page.info().recordsTotal);
            fnIsFromAddActivityPartial();
            $('.onhoverOpportunity').hover(function () {
                $("#IsFrom_hdn").val('2');
            });
        }
    });

}

function fnFromDateToDate() {
    var FromDate = $("#FromDate").val().substring(0, 10);
    var ToDate = $("#ToDate").val().substring(0, 10);

    if (FromDate != "" && ToDate != "") {
        var start = FromDate;
        start = start.split("/");
        start = start[1] + "/" + start[0] + "/" + start[2];

        var end = ToDate;
        end = end.split("/");
        end = end[1] + "/" + end[0] + "/" + end[2];

        end = new Date(end);
        start = new Date(start);
        var diff = new Date(end - start);
        var days = diff / 1000 / 60 / 60 / 24;

        if (days < 0) {
            swal("End Date should be greater than or equal to Start Date", {
                icon: "info"
            });
            $('#FromDate').val(formatDateFT(new Date()));
            $('#ToDate').val(formatDateFT(new Date()));
        }
        BindListfollowups();
        BindListSitesVisits();
        fnIsFromAddActivityPartial();

    } else {
        swal("Start Date and End Date can't be Null", {
            icon: "info"
        });
        $('#FromDate').val(formatDateFT(new Date()));
        $('#ToDate').val(formatDateFT(new Date()));
        BindListfollowups();
        BindListSitesVisits();
        fnIsFromAddActivityPartial();
    }

}

function fnIsFromAddActivityPartial() {

    if ($("#tab_1").hasClass('active')) {
        $("#IsFrom_hdn").val("1");
        $.ajax({
            type: "GET",
            url: "/Transactions/TodaysTotalTask/ActivityPartial",
            data: {
                IsFrom_hdn: "1"
            },
            async: !0,
            cache: !1,
            success: function (data) {
                $("#listActivity").html(data);
                $(".datetimepickeralt").datetimepicker({
                    format: 'DD/MM/YYYY - hh:mm A'
                });
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
    if ($("#tab_2").hasClass('active')) {
        $("#IsFrom_hdn").val("2");
        $.ajax({
            type: "GET",
            url: "/Transactions/TodaysTotalTask/ActivityPartial",
            data: {
                IsFrom_hdn: "2"
            },
            async: !0,
            cache: !1,
            success: function (data) {
                $("#listActivity").html(data);
                $(".datetimepickeralt").datetimepicker({
                    format: 'DD/MM/YYYY - hh:mm A'
                });
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
    setTimeout(function () {
        $('.select2-selection__arrow').css('display', 'block');
    }, 500);
}

$("#tab_1").on('click', function () {
    $('.FollowUpsStatus').show();
    $('.SitesVisitsStatus').hide();
    fnFromDateToDate();
});
$("#tab_2").on('click', function () {
    $('.FollowUpsStatus').hide();
    $('.SitesVisitsStatus').show();
    fnFromDateToDate();
});
