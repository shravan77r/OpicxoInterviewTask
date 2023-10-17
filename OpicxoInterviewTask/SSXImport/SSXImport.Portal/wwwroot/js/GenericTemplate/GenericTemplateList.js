$(function () {
    BindList();
});

function BindList() {
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
            "url": "/GenericTemplate/GetTemplateList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.keyword = $("#search").val();
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "EmailTemplateName", "autoWidth": true },
            { "data": "ObjectType", "autoWidth": true },
            { "data": "Object", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    return fnCreateActions(full.EmailTemplateGUID);
                }
            },
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [2,3,4] }
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
    returnString += '<a href="#" onclick="fnUseTemplate(\'' + key + '\')" class="btn-transparent" title="Execute"><i class="fa fa-play"></i></a>';

    returnString += '<a href="/GenericTemplate/GenericTemplateForm?key=' + key + '" class="btn-transparent" title="Edit"><i class="fa fa-pencil"></i></a>';
    returnString += '<a href="#" class="btn-transparent text-danger" onclick="fnDeleteData(\'' + key + '\')" title="Delete"><i class="fa fa-trash"></i></a>';
    return returnString;
}

function fnDeleteData(key) {
    if (confirm("Are you sure?")) {
        $.ajax({
            type: "GET",
            url: '/GenericTemplate/DeleteEmailTemplate',
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


function fnShowMessage(message, messageType) {
    if (message && messageType) {
        $('#divAlertMessage').removeClass();
        $('#divAlertMessage').addClass('alert alert-' + messageType, true);
        $('#divAlertMessage').html(message);
        $('#divAlertMessage').toggleClass('d-none', false);
    }
}


function fnUseTemplate(key) {
  
        $('#hdn_TemplateTableGUID').val(key);
        $('#ManageFilter').modal('show');
    
}


function fnExecuteTemplate() {
    var key = $('#hdn_TemplateTableGUID').val();
    location.href = "/GenericTemplate/CreateMultipleGenericTemplate?key="+key;
}