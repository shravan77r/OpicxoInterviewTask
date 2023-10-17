$(function () {
    BindList();
    $('#ifLogFile').html('');
});

function BindList() {
    var table = $('#tblDataTransfer').DataTable({
        "paging": true,
        "pageSize": 10,
        "bLengthChange": false,
        "autoWidth": false,
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ajax": {
            "url": "/DataTransfer/GetDataTransferList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.keyword = $("#search").val();
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "TemplateName", "autoWidth": true },
            { "data": "TransferDate", "autoWidth": true },
            { "data": "Source", "autoWidth": true },
            { "data": "Target", "autoWidth": true },
            { "data": "TransferStatus", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    if (full.TransferStatus == 'Failed')
                        return '<a href="#" class="btn-transparent" onclick="fnShowLogFile(\'' + full.DataTransferGUID + '\')" title="View Log"><i class="fa fa-eye"></i></a>';
                    else
                        return '<a href="#" class="btn-transparent" onclick="fnShowLogFile(\'' + full.DataTransferGUID + '\')" title="View Log"><i class="fa fa-eye"></i></a>';
                        //return '-';
                }
            },
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [3, 4, 5, 6] }
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

function fnShowLogFile(key) {
    $('.customloader').show();
    $.ajax({
        type: "POST",
        url: '/DataTransfer/GetLogFileContent',
        async: !0,
        cache: !1,
        data: {
            key: key,
        },
        success: function (data) {
            if (data.status == 1) {
                //document.getElementById('ifLogFile').src = "data:text/html;charset=utf-8," + data.logFileContent;
                //$('#iFrameLogContent').attr('src', data.logFileContent)
                //$('#iFrameLogContent').contents().find('html').html(data.logFileContent);
                $('#preLogFile').html(data.logFileContent);
                $('#divLogFile').html(data.logFileContent);
                $('#ModalLogFile').modal('show');
            } else {
                alert("Log File not found!");
                console.log(data);
            }
            $('.customloader').hide();
        },
        error: function (response) {
            console.log(response);
            $('.customloader').hide();
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