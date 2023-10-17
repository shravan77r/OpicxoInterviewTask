$(function () {
    console.log('Session value from ViewData:', valueFromViewData);
    console.log('Session value from ViewBag:', valueFromViewBag);

    //BindList();
});

function BindList() {
    console.log('I am in BindList');
    var table = $('#tblTemplate').DataTable({
        "paging": true,
        "pageSize": 10,
        "bLengthChange": false,
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ajax": {
            "url": "/Template/GetTemplateList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.keyword = $("#search").val();
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "TemplateName", "autoWidth": true },
            { "data": "Source", "autoWidth": true },
            { "data": "Target", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    
                    return fnCreateActions(full.TemplateGUID);
                }
            },
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [2, 3, 4] }
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
    returnString += '<a href="TemplateForm?key=' + key + '" class="btn-transparent" title="Edit"><i class="fa fa-pencil"></i></a>';
    returnString += '<a href="#" class="btn-transparent" title="Use Template" onclick="fnUseTemplate(\'' + key + '\')"><i class="fa fa-window-maximize"></i></a>';
    returnString += '<a href="#" class="btn-transparent text-danger" onclick="fnDeleteData(\'' + key + '\')" title="Delete"><i class="fa fa-trash"></i></a>';
    return returnString;
}

function fnDeleteData(key) {
    if (confirm("Are you sure?")) {
        $.ajax({
            type: "GET",
            url: '/Template/DeleteTemplate',
            async: true,
            cache: false,
            data: {
                key: key,
            },
            success: function (data) {
                $('#tblTemplate').DataTable().draw();
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
}

function fnUseTemplate(key) {
    if (confirm("Do you want to transfer data?")) {
        $.ajax({
            type: "POST",
            url: '/DataTransfer/ExecuteTransfer',
            async: true,
            cache: false,
            data: {
                key: key,
            },
            success: function (data) {
                if (data.messageType == "danger") {
                    fnShowMessage(data.message, data.messageType);
                }
                else {
                    window.location.href = '/DataTransfer/DataTransfer';
                }
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
}

function fnShowMessage(message, messageType) {
    if (message && messageType) {
        $('#divAlertMessage').removeClass();
        $('#divAlertMessage').addClass('alert alert-' + messageType, true);
        $('#divAlertMessage').html(message);
        $('#divAlertMessage').toggleClass('d-none', false);
    }
}