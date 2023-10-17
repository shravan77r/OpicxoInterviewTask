$(function () {
    BindList();
});

function BindList() {
    var table = $('#tblAPITemplate').DataTable({
        "paging": true,
        "pageSize": 10,
        "bLengthChange": false,
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ajax": {
            "url": "/APIData/GetAPIDataList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.keyword = $("#search").val();
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "APIEndPoint", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    return fnCreateActions(full.APIGUID);
                }
            },
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [2, 3] },
        ],
        "order": [[0, "desc"]],
    });

    $("#search").on('keyup', function (event) {
        event.preventDefault();
        table.search(this.value).draw();
    });

    $("#btnSearch").on('click', function (event) {
        event.preventDefault();
        table.search(this.value).draw();
    });
}

function fnCreateActions(key) {
    var returnString = '';
    returnString += '<a href="/APIData/APIDataForm?key=' + key + '" class="btn-transparent" title="Edit"><i class="fa fa-pencil"></i></a>';
    returnString += '<a href="#" class="btn-transparent text-danger" onclick="fnDeleteData(\'' + key + '\')" title="Delete"><i class="fa fa-trash"></i></a>';
    returnString += '<a href="/APIData/RunAPI?key=' + key + '" class="btn-transparent" title="RunAPI"><i class="fa fa-play"></i></a>';
    return returnString;
}

function fnDeleteData(key) {
    if (confirm("Are you sure?")) {
        $.ajax({
            type: "GET",
            url: '/APIData/DeleteAPIData',
            async: true,
            cache: false,
            data: {
                key: key,
            },
            success: function (data) {
                $('#tblAPITemplate').DataTable().draw();
                fnShowMessage(data.message, data.messageType);
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
}

function fnRunAPI(apiGUID) {

    $.ajax({
        type: "GET",
        url: '/APIData/RunAPI',
        async: true,
        cache: false,
        data: {
            key: apiGUID,
        },
        success: function (data) {
            $('#tblAPITemplate').DataTable().draw();
            fnShowMessage(data.message, data.messageType);
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function fnShowMessage(message, messageType) {
    if (message && messageType) {
        $('#divAlertMessage').removeClass();
        $('#divAlertMessage').addClass('alert alert-' + messageType, true);
        $('#divAlertMessage').html(message);
        $('#divAlertMessage').toggleClass('d-none', false);
    }
}
