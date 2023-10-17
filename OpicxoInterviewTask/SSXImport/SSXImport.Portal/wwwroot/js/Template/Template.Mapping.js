$(function () {
    var TemplateGUID = $('#TemplateGUID').val();
    if (TemplateGUID != '') {
        fnBindTemplateTableList();
    }
});

function fnBindTemplateTableList() {
    var table = $('#tblMappingTable').DataTable({
        "paging": false,
        "bLengthChange": false,
        "autoWidth": false,
        "processing": true,
        "destroy": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        //"rowReorder": true,
        "ajax": {
            "url": "/Template/GetTemplateTableList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.TemplateGUID = $("#TemplateGUID").val();
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "SourceTable", "autoWidth": true, "className": 'reorder SourceTable text-left' },
            { "data": "TargetTable", "autoWidth": true },
            { "data": "IsDeduplicateData", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    return '<a href="#" title="Manage Filter" onClick = "fnManageFilter(\'' + full.TemplateTableDetailGUID + '\')"><i class="fa fa-filter"></i> Manage Filter</a>';
                }
            },
            {
                mRender: function (data, type, full) {
                    var columnMappingStatus = full.IsColumnMapped == "1" ? '<i  class="fa fa-check-circle-o" style="color:#0ea00e" aria-hidden="true"></i>' : '<i class="fa fa-clock-o" aria-hidden="true" style="color:#ffc107"></i>';
                    return '<a href="#" title="Manage Mapping" onClick = "fnManageTemplateColumn(\'' + full.TemplateTableDetailGUID + '\')"><i class="fa fa fa-map-signs"></i> Manage Mapping ' + columnMappingStatus + '</a>';
                }
            },
            {
                mRender: function (data, type, full) {
                    return '<a href="#" class="btn-transparent text-danger" onclick="fnDeleteMappingTable(\'' + full.TemplateTableDetailGUID + '\')" title="Delete"><i class="fa fa-trash"></i></a>';
                }
            },
        ],
        "rowReorder": {
            "dataSrc": 'SourceTable',
            "selector": '.SourceTable',
            "update": false
        },
        "responsive": true,
        "columnDefs": [
            { orderable: false, targets: [0, 2, 3, 4, 5, 6] }
        ],
        //"order": [[1, "desc"]],
    });

    // In Case of API Hide Filter Option
    if (parseInt($('#OriginSourceTypeId').val()) == 4) {
        table.column(4).visible(false);
    } else {
        table.column(4).visible(true);
    }

    // In Case of Template Type Normal then only display Duplicate Column
    var templateType = !isNaN($("#TemplateType").val()) ? parseInt($("#TemplateType").val()) : 1;
    if (templateType == 1) {
        table.column(3).visible(true);
    } else {
        table.column(3).visible(false);
    }

    table.on('row-reorder', function (e, diff, edit) {

        if (confirm("All Table Mapping will be clear if you change Table Order, Are you sure?")) {

            var updatedRows = [];
            for (var i = 0, ien = diff.length; i < ien; i++) {
                var updatedRow = {};
                updatedRow.TemplateTableDetailGUID = table.row(diff[i].node).data()["TemplateTableDetailGUID"];
                updatedRow.OldPosition = diff[i].oldPosition + 1;
                updatedRow.NewPosition = diff[i].newPosition + 1;
                updatedRows.push(updatedRow);
            }
            if (updatedRows.length > 0) {
                $('.customloader').show();
                $.ajax({
                    type: "POST",
                    url: "/Template/UpdateTemplateTableOrder",
                    data: {
                        UpdatedTemplateTableRowsDetails: JSON.stringify(updatedRows),
                    },
                    async: !0,
                    cache: !1,
                    success: function (data) {
                        fnShowMessage(data.message, data.messageType);
                        $('.customloader').hide();
                        $('#tblMappingTable').DataTable().draw(true);
                    },
                    error(response) {
                        console.log(response);
                        fnShowMessage("Something Went Wrong", "danger");
                        $('.customloader').hide();
                    },
                });

                fnBindTemplateTableList();
            }

        }
        else {
            fnBindTemplateTableList();
        }

    });
}

function fnAddMappingTableDetail() {

    var form = $('#form').valid();

    if (!form) {
        return false;
    }

    var templateTableDetail = {};

    templateTableDetail.TemplateGUID = $('#TemplateGUID').val();

    templateTableDetail.SourceTable = $('#OriginSourceTable').val();
    templateTableDetail.IsDeduplicateData = $('#IsDeduplicateData').is(":checked");
    templateTableDetail.TargetTable = $('#TargetSourceTable').val();

    $('.customloader').show();

    $.ajax({
        type: "POST",
        url: "/Template/ManageTemplateTableDetail",
        data: templateTableDetail,
        async: !0,
        cache: !1,
        success: function (data) {
            fnShowMessage(data.message, data.messageType);
            fnBindTemplateTableList();
            $('.customloader').hide();
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });

    $('#OriginSourceTable').val('');
    $('#TargetSourceTable').val('');
}

function fnDeleteMappingTable(key) {
    //$.confirm({
    //    title: 'Confirm Delete !!',
    //    content: 'Delete This Record?',
    //    type: 'red',
    //    buttons: {
    //        ok: {
    //            text: "Delete!",
    //            btnClass: 'btn-primary',
    //            keys: ['enter'],
    //            action: function () {

    //                var templateTableDetail = {};

    //                templateTableDetail.TemplateTableDetailGUID = key;
    //                templateTableDetail.IsDelete = 1;

    //                $('.customloader').show();

    //                $.ajax({
    //                    type: "GET",
    //                    url: '/Template/ManageTemplateTableDetail',
    //                    async: true,
    //                    cache: false,
    //                    data: templateTableDetail,
    //                    success: function (data) {
    //                        fnBindTemplateTableList();
    //                        toastr[data.messageType](data.message);
    //                        $('.customloader').hide();
    //                    },
    //                    error: function (response) {
    //                        console.log(response);
    //                        toastr.error("Something Went Wrong!");
    //                        $('.customloader').hide();
    //                    }
    //                });
    //            }
    //        },
    //        cancel: function () {
    //        }
    //    }
    //});

    if (confirm("Are you sure?")) {
        var templateTableDetail = {};

        templateTableDetail.TemplateTableDetailGUID = key;
        templateTableDetail.IsDelete = true;

        $('.customloader').show();

        $.ajax({
            type: "POST",
            url: '/Template/ManageTemplateTableDetail',
            async: true,
            cache: false,
            data: templateTableDetail,
            success: function (data) {
                fnBindTemplateTableList();
                fnShowMessage(data.message, data.messageType)
                $('.customloader').hide();
            },
            error: function (response) {
                console.log(response);
                fnShowMessage("Something Went Wrong!", "danger")
                $('.customloader').hide();
            }
        });
    }
}

function fnManageTemplateColumn(key) {

    var templateType = !isNaN($("#TemplateType").val()) ? parseInt($("#TemplateType").val()) : 0;

    $('#hdn_TemplateTableDetailGUID').val(key);
    $('.customloader').show();
    $('#tblMappingColumn > tbody').html('');
    $.ajax({
        type: "POST",
        url: "/Template/GetTemplateColumnList",
        data: {
            TemplateGUID: $('#TemplateGUID').val(),
            TemplateTableDetailGUID: key,
        },
        async: !0,
        cache: !1,
        success: function (data) {
            if (data.targetSourceColumnDetails && data.originSourceColumnDetails && data.originSourceDependentColumnDetails) {
                var originSourceColumnDetails = data.originSourceColumnDetails;
                var targetSourceColumnDetails = data.targetSourceColumnDetails;
                var originSourceDependentColumnDetails = data.originSourceDependentColumnDetails;

                for (var i = 0; i < targetSourceColumnDetails.length; i++) {
                    var column = targetSourceColumnDetails[i];
                    var appendString = '<tr>';
                    appendString += '<td>';
                    appendString += i + 1;
                    appendString += '</td>';
                    appendString += '<td>';

                    appendString += '<select class="form-control SourceColumn">';
                    appendString += '<option value="">Select Origin Column</option>';
                    var OriginSourceColumnList = originSourceColumnDetails;
                    for (var j = 0; j < OriginSourceColumnList.length; j++) {

                        var SourceColumnValue = OriginSourceColumnList[j].SourceColumn.indexOf(".") != -1 ? "#2" + OriginSourceColumnList[j].SourceColumn : OriginSourceColumnList[j].SourceColumn;
                        var SourceColumnName = OriginSourceColumnList[j].SourceColumn;

                        var optionSelected = (SourceColumnValue == column.SourceColumn ? "selected" : "");
                        appendString += '<option value="' + SourceColumnValue + '" ' + optionSelected + '>' + SourceColumnName + '</option>';
                    }
                    appendString += '</select>';
                    appendString += '<input type="hidden" class="TemplateColumnDetailGUID" value="' + column.TemplateColumnDetailGUID + '"/>';
                    appendString += '</td>';

                    // Source Dependent Column DropDwon - Start
                    if (templateType == 3) {
                        appendString += '<td>';
                        appendString += '<select class="form-control SourceDependentColumn">';
                        appendString += '<option value="">Select Origin Dependent Column </option>';
                        var OriginSourceDependentColumnDetailsList = originSourceDependentColumnDetails;

                        if (OriginSourceDependentColumnDetailsList.length > 0) {
                            for (var j = 0; j < OriginSourceDependentColumnDetailsList.length; j++) {

                                var SourceColumnValue = OriginSourceDependentColumnDetailsList[j].SourceDependentColumn.indexOf(".") != -1 ? "#2" + OriginSourceDependentColumnDetailsList[j].SourceDependentColumn : OriginSourceDependentColumnDetailsList[j].SourceDependentColumn;
                                var SourceColumnName = OriginSourceDependentColumnDetailsList[j].SourceDependentColumn;

                                var optionSelected = SourceColumnValue == column.SourceDependentColumn ? "selected" : "";
                                appendString += '<option value="' + SourceColumnValue + '" ' + optionSelected + '>' + SourceColumnName + '</option>';

                            }
                        }
                        appendString += '</select>';
                        appendString += '</td>';
                    }

                    // Source Dependent Column DropDwon - End

                    appendString += '<td>';
                    appendString += '<span class="TargetColumn">';
                    appendString += column.TargetColumn;
                    appendString += '</span>';
                    appendString += '</td>';

                    // is Unique Column - Start
                    if (templateType == 1) {
                        appendString += '<td>';
                        appendString += '<div class="icheck-primary d-inline">';
                        appendString += '<input type="checkbox" class="IsUniqueColumn" id= "IsUniqueColumn_' + i + '" ' + (column.IsUniqueColumn == "true" ? "checked" : "") + ' />';
                        appendString += '</div>';
                        appendString += '</td>';
                    }
                    // is Unique Column - End

                    appendString += '</tr>';

                    $('#tblMappingColumn > tbody').append(appendString);
                }
            }
            $('#ManageMapping').modal('show');
            $('.customloader').hide();
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
}

function fnSaveTemplateColumn() {
    var detailsArray = [];
    $("#tblMappingColumn tr:gt(0)").each(function () {
        var this_row = $(this);
        var SourceColumn = $.trim(this_row.find('.SourceColumn').val());
        var SourceDependentColumn = $.trim(this_row.find('.SourceDependentColumn').val());
        var TargetColumn = $.trim(this_row.find('.TargetColumn').html());
        var IsUniqueColumn = this_row.find('.IsUniqueColumn').is(":checked");
        var TemplateColumnDetailGUID = ($.trim(this_row.find('.TemplateColumnDetailGUID').val()) != null && $.trim(this_row.find('.TemplateColumnDetailGUID').val()) != "null")
            ? $.trim(this_row.find('.TemplateColumnDetailGUID').val()) : null;
        if (SourceColumn != "" || TemplateColumnDetailGUID != null) {
            detailsArray.push({
                SourceColumn: SourceColumn,
                SourceDependentColumn: SourceDependentColumn,
                TargetColumn: TargetColumn,
                TemplateColumnDetailGUID: TemplateColumnDetailGUID,
                IsUniqueColumn: IsUniqueColumn,
            });
        }
    });

    $('.customloader').show();

    $.ajax({
        type: "POST",
        url: "/Template/ManageTemplateColumnDetail",
        data: {
            Details_Insert: JSON.stringify(detailsArray),
            TemplateGUID: $('#TemplateGUID').val(),
            TemplateTableDetailGUID: $('#hdn_TemplateTableDetailGUID').val(),
        },
        async: !0,
        cache: !1,
        success: function (data) {
            fnShowMessage(data.message, data.messageType);
            $('#ManageMapping').modal('hide');
            $('.customloader').hide();
            $('#tblMappingColumn > tbody').html('');
            fnBindTemplateTableList();
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
            $('#tblMappingColumn > tbody').html('');
            fnBindTemplateTableList();
        },
    });
}

function fnManageFilter(key) {

    $('#hdn_TemplateTableDetailGUID').val(key);
    $('.customloader').show();
    $('#tblMappingFilter > tbody').html('');
    $.ajax({
        type: "POST",
        url: "/Template/GetTemplateFilterList",
        data: {
            TemplateGUID: $('#TemplateGUID').val(),
            TemplateTableDetailGUID: key,
        },
        async: !0,
        cache: !1,
        success: function (data) {
            if (data.originSourceColumnDetails) {
                var originSourceFilterDetails = data.originSourceColumnDetails;
                var operators = data.operators;

                for (var i = 0; i < originSourceFilterDetails.length; i++) {
                    var column = originSourceFilterDetails[i];
                    var appendString = '<tr>';
                    appendString += '<td>';
                    appendString += i + 1;
                    appendString += '</td>';
                    appendString += '<td>';

                    appendString += '<input type="text" class="form-control FilterColumn" value="' + column.FilterColumn + '" disabled />';
                    appendString += '<input type="hidden" class="TemplateFilterDetailGUID" value="' + column.TemplateFilterDetailGUID + '"/>';

                    appendString += '</td>';

                    appendString += '<td>';
                    appendString += '<select class="form-control FilterOperator" onchange ="fnChangeFilterValueState(this);">';
                    appendString += '<option value="0">Operator</option>';
                    var operatorsList = operators;
                    for (var j = 0; j < operatorsList.length; j++) {
                        if (operatorsList[j].Id == column.FilterOperator)
                            appendString += '<option value="' + operatorsList[j].Id + '" selected>' + operatorsList[j].Name + '</option>';
                        else
                            appendString += '<option value="' + operatorsList[j].Id + '">' + operatorsList[j].Name + '</option>';
                    }
                    appendString += '</select>';
                    appendString += '</td>';

                    appendString += '<td>';
                    var filterValueState = (column.FilterOperator != null && (column.FilterOperator == 9 || column.FilterOperator == 10)) ? "disabled" : "";
                    appendString += '<input type="text" class="form-control FilterValue" value="' + column.FilterValue + '" placeholder = "Enter Filter Value" maxlength="20" ' + filterValueState + '/>';
                    appendString += '</td>';
                    appendString += '</tr>';

                    $('#tblMappingFilter > tbody').append(appendString);
                }
            }
            $('#ManageFilter').modal('show');
            $('.customloader').hide();
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
}

function fnSaveTemplateFilter() {
    var detailsArray = [];
    $("#tblMappingFilter tr:gt(0)").each(function () {
        var this_row = $(this);
        var FilterColumn = $.trim(this_row.find('.FilterColumn').val());
        var FilterOperator = $.trim(this_row.find('.FilterOperator').val());
        var FilterValue = $.trim(this_row.find('.FilterValue').val());
        var TemplateFilterDetailGUID = ($.trim(this_row.find('.TemplateFilterDetailGUID').val()) != null && $.trim(this_row.find('.TemplateFilterDetailGUID').val()) != "null")
            ? $.trim(this_row.find('.TemplateFilterDetailGUID').val()) : null;
        if (TemplateFilterDetailGUID != null || parseInt(FilterOperator) > 0)
            detailsArray.push({
                FilterColumn: FilterColumn,
                FilterOperator: FilterOperator,
                FilterValue: FilterValue,
                TemplateFilterDetailGUID: TemplateFilterDetailGUID,
            });
    });
    $('.customloader').show();

    $.ajax({
        type: "POST",
        url: "/Template/ManageTemplateFilterDetail",
        data: {
            Details_Insert: JSON.stringify(detailsArray),
            TemplateGUID: $('#TemplateGUID').val(),
            TemplateTableDetailGUID: $('#hdn_TemplateTableDetailGUID').val(),
        },
        async: !0,
        cache: !1,
        success: function (data) {
            fnShowMessage(data.message, data.messageType);
            $('#ManageFilter').modal('hide');
            $('.customloader').hide();
            $('#tblMappingFilter > tbody').html('');
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('#ManageFilter').modal('hide');
            $('.customloader').hide();
            $('#tblMappingFilter > tbody').html('');
        },
    });

}

function fnChangeFilterValueState(filterOperator) {
    var filterOperatorValue = parseInt($(filterOperator).val());
    var filterValue = $(filterOperator).closest("tr").find('.FilterValue')[0];
    if (filterOperatorValue == 9 || filterOperatorValue == 10) {
        $(filterValue).val('');
        $(filterValue).prop("disabled", true);
    } else {
        $(filterValue).prop("disabled", false);
    }
}

function fnBindOriginSheetDropdown() {
    $('#lblOriginSourceTable').html("Origin Source Sheet");
    $('#OriginSourceTable')
        .empty()
        .append('<option selected value="">Select Sheet</option>')
        ;

    //var OriginSourceFileName = $('#OriginSourceFileName').val();
    //if (OriginSourceFileName != '') {
    $('.customloader').show();
    $.ajax({
        type: "POST",
        url: "/Template/GetSheetNames",
        data: {
            TemplateGUID: $('#TemplateGUID').val(),
        },
        async: !0,
        cache: !1,
        success: function (data) {
            if (data.sheets != '') {
                var sheets = $.parseJSON(data.sheets);
                for (var i = 0; i < sheets.length; i++) {
                    $('#OriginSourceTable')
                        .append('<option value="' + sheets[i].Name + '">' + sheets[i].Name + '</option>');
                }
            }
            if (data.messageType == 'danger') {
                fnShowMessage("File not found!!", data.messageType);
                $('#OriginSourceTable')
                    .empty()
                    .append('<option selected value="">Select Sheet</option>')
                    ;
            }
            $('.customloader').hide();
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
    //}
}

function fnTemplateTypeOnChange(objThis) {

    templateType = !isNaN($(objThis).val()) ? parseInt($(objThis).val()) : 1;

    fnDisplayUniqueColumns(templateType);

    var templateGUID = $("#TemplateGUID").val();

    if (templateGUID != "") {

        if (confirm("All Table Mapping will be clear if you change Template Type, Are you sure?")) {

            $('.customloader').show();

            $.ajax({
                type: "POST",
                url: "/Template/RemoveColumnMapping",
                data: {
                    TemplateGUID: templateGUID,
                },
                async: !0,
                cache: !1,
                success: function (data) {
                    fnShowMessage(data.message, data.messageType);
                    $('.customloader').hide();
                    $('#tblMappingTable').DataTable().draw(true);
                },
                error(response) {
                    console.log(response);
                    fnShowMessage("Something Went Wrong", "danger");
                    $('.customloader').hide();
                },
            });

        }
    }
}

function fnDisplayUniqueColumns(templateType) {
    if (templateType == 1) {
        $("#thSourceDependentColumn").css("display", "none");
        $(".clsIsDeduplicateData").css("display", "block");
    }
    else if (templateType == 2) {
        $("#thSourceDependentColumn").css("display", "none");
        $(".clsIsDeduplicateData").css("display", "none");
    }
    else if (templateType == 3) {
        $("#thSourceDependentColumn").css("display", "block");
        $(".clsIsDeduplicateData").css("display", "none");
    }

}
