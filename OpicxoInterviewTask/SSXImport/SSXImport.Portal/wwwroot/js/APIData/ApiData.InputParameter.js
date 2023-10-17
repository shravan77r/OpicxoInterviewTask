$(function () {
    var APIGUID = $('#APIGUID').val();
    if (APIGUID != '') {
        fnBindInputParameterList();
    }
    fnOnChangeInputParameterType();
});

function fnBindInputParameterList() {
    var table = $('#tblInputParameter').DataTable({
        "paging": false,
        "bLengthChange": false,
        "autoWidth": false,
        "processing": true,
        "destroy": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ajax": {
            "url": "/APIData/GetInputParameterList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.APIGUID = $("#APIGUID").val();
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "Key", "autoWidth": true, "className": 'reorder SourceTable text-left' },
            { "data": "Value", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    return '<a href="#" class="btn-transparent text-danger" onclick="fnDeleteInputParameterTable(\'' + full.InputParameterGUID + '\')" title="Delete"><i class="fa fa-trash"></i></a>';
                }
            },
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, targets: [0, 1, 2, 3] }
        ],
    });
}

function fnAddInputParameterDetail() {

    var form = $('#form').valid();

    if (!form) {
        return false;
    }

    var APIInputParameterDetail = {};

    APIInputParameterDetail.APIGUID = $('#APIGUID').val();
    APIInputParameterDetail.InputParameterGUID = null;
    APIInputParameterDetail.APIId = 0;
    APIInputParameterDetail.InputParameterTypeId = $('#InputParameterType').val();
    APIInputParameterDetail.BodyType = $("#BodyParameterType").val();
    APIInputParameterDetail.KeyColumn = $('#objInputParameter_KeyColumn').val();
    APIInputParameterDetail.ValueColumn = $('#objInputParameter_ValueColumn').val();

    $('.customloader').show();

    $.ajax({
        type: "POST",
        url: "/APIData/ManageInputParameterDetail",
        data: APIInputParameterDetail,
        async: !0,
        cache: !1,
        success: function (data) {
            fnShowMessage(data.message, data.messageType);
            fnBindInputParameterList();
            $('#objInputParameter_KeyColumn').val("");
            $('#objInputParameter_ValueColumn').val("");
            $('.customloader').hide();
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
}

function fnDeleteInputParameterTable(key) {

    if (confirm("Are you sure?")) {
        var APIInputParameterDetail = {};

        APIInputParameterDetail.InputParameterGUID = key;
        APIInputParameterDetail.IsDelete = true;

        $('.customloader').show();

        $.ajax({
            type: "POST",
            url: '/APIData/ManageInputParameterDetail',
            async: true,
            cache: false,
            data: APIInputParameterDetail,
            success: function (data) {
                fnBindInputParameterList();
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

function fnOnChangeInputParameterType() {

    var inputParameterType = !isNaN($('#InputParameterType').val()) ? parseInt($('#InputParameterType').val()) : 0;

    switch (inputParameterType) {
        case 1:
            $("#parameterType").html("Query String Parameter");
            $("#divBodyTypeDD").css("display", "none");
            break;
        case 2:
            $("#parameterType").html("Header Parameter");
            $("#divBodyTypeDD").css("display", "none");
            break;
        case 3:
            $("#parameterType").html("Body Parameter");
            $("#divBodyTypeDD").css("display", "block");
            break;
    }
}
