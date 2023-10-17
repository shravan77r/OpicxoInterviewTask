$(function () {
    $('#TargetSourceTypeId').on('change', function () {
        fnDisplayTargetDatabaseFields($(this).val());
        fnResetTargetConnection();
    });

    fnDisplayTargetDatabaseFields($('#TargetSourceTypeId').val());
    if (!$('#TargetSourcePort').val()) {
        fnChangeDefaultTargetSourcePort($('#TargetSourceTypeId').val());
    }

    $('#TargetSourceServer,#TargetSourcePort,#TargetSourceUsername,#TargetSourcePassword').on('change', function () {
        fnResetTargetConnection();
    });

    $('#TargetSourceDatabase').on('change', function () {
        fnSetTargetSourceTable();
    });

});

function fnDisplayTargetDatabaseFields(SourceId) {
    SourceId = parseInt(SourceId);
    if (SourceId == 1) {
        //$('.excelFields').toggleClass('d-none', false);
        $('.targetDatabaseFields').toggleClass('d-none', true);
        $('#lblTargetSource').val("Excel");
    } else if (SourceId == 2) {
        //$('.excelFields').toggleClass('d-none', true);
        $('.targetDatabaseFields').toggleClass('d-none', false);
        $('#lblTargetSource').val("MsSQL");
    }
    else if (SourceId == 3) {
        //$('.excelFields').toggleClass('d-none', true);
        $('.targetDatabaseFields').toggleClass('d-none', false);
        $('#lblTargetSource').val("MySQL");
    }
    fnChangeDefaultTargetSourcePort(SourceId);
}

function fnChangeDefaultTargetSourcePort(SourceId) {
    SourceId = parseInt(SourceId);
    if (SourceId == 2) {
        $('#TargetSourcePort').val($('#MsSQL_DefaultPort').val());
    }
    else if (SourceId == 3) {
        $('#TargetSourcePort').val($('#MySQL_DefaultPort').val());
    }
}

function fnTestTargetConnection(e) {
    var IsValidServer = $('#TargetSourceServer').valid();
    var IsValidPort = $('#TargetSourcePort').valid();
    var IsValidUsername = $('#TargetSourceUsername').valid();
    var IsValidPassword = $('#TargetSourcePassword').valid();
    if (IsValidServer
        && IsValidPort
        && IsValidUsername
        && IsValidPassword) {
        $('.customloader').show();
        fnResetTargetConnection();
        $.ajax({
            type: "POST",
            url: "/Template/ValidateConnection",
            data: {
                DataSourceId: parseInt($('#TargetSourceTypeId').val()),
                Server: $('#TargetSourceServer').val(),
                Port: $('#TargetSourcePort').val(),
                Username: $('#TargetSourceUsername').val(),
                Password: $('#TargetSourcePassword').val(),
            },
            async: !0,
            cache: !1,
            timeout: 25000, // sets timeout to 3 seconds
            success: function (data) {
                $('#IsValidTargetConnection').val(data.isValidConnection);
                $('#TargetConnectionString').val(data.connectionString);
                if (parseInt(data.isValidConnection) && data.databases != '') {
                    var databases = $.parseJSON(data.databases);
                    for (var i = 0; i < databases.length; i++) {
                        $('#TargetSourceDatabase')
                            .append('<option value="' + databases[i].Name + '">' + databases[i].Name + '</option>');
                    }
                }
                fnShowMessage(data.message, data.messageType);
                fnChangeTargetTestConnectionButton();
                $('.customloader').hide();
            },
            error(response) {
                fnShowMessage("Something Went Wrong", "danger");
                $('.customloader').hide();
            },
        });
       
    }
}

function fnChangeTargetTestConnectionButton() {
    var IsValidConnection = !isNaN(parseInt($('#IsValidTargetConnection').val())) ? parseInt($('#IsValidTargetConnection').val()) : 0;

    var btnClass = 'btn-warning';
    var iconClass = 'fa-circle';
    if (IsValidConnection == 1) {
        btnClass = 'btn-primary';
        iconClass = 'fa-check';
    }
    else if (IsValidConnection == 2) {
        btnClass = 'btn-danger';
        iconClass = 'fa-times';
    }

    var btn = '';
    btn += '<button class="btn ' + btnClass + ' btn-lg mr-3 btn-sm" type="button" onclick="fnTestTargetConnection(event);">';
    btn += '<i class="fa ' + iconClass + '"></i>';
    btn += 'Test Connection';
    btn += '</button>';
    $('#btnTestTargetConnection').html(btn);
}

function fnResetTargetConnection() {
    $('#IsValidTargetConnection').val(0);
    fnChangeTargetTestConnectionButton();
    $('#TargetSourceDatabase')
        .empty()
        .append('<option selected value="">Select Database</option>')
        ;

    $('#TargetSourceTable')
        .empty()
        .append('<option selected value="">Select Table</option>')
        ;
}

function fnSetTargetSourceTable() {
    $('#TargetSourceTable')
        .empty()
        .append('<option selected value="">Select Table</option>')
        ;

    var Database = $('#TargetSourceDatabase').val();
    if (Database != '') {
        $('.customloader').show();
        $.ajax({
            type: "POST",
            url: "/Template/GetTableNames",
            data: {
                DataSourceId: parseInt($('#TargetSourceTypeId').val()),
                Server: $('#TargetSourceServer').val(),
                Port: $('#TargetSourcePort').val(),
                Username: $('#TargetSourceUsername').val(),
                Password: $('#TargetSourcePassword').val(),
                Database: $('#TargetSourceDatabase').val(),
            },
            async: !0,
            cache: !1,
            timeout: 25000,
            success: function (data) {
                if (parseInt(data.isValidConnection) && data.tables != '') {
                    var tables = $.parseJSON(data.tables);
                    for (var i = 0; i < tables.length; i++) {
                        $('#TargetSourceTable')
                            .append('<option value="' + tables[i].Name + '">' + tables[i].Name + '</option>');
                    }
                }
                if (data.messageType == 'danger') {
                    fnShowMessage("Database is not accessible using given credentials!", data.messageType);
                    $('#TargetSourceDatabase').val('');
                    $('#TargetSourceTable')
                        .empty()
                        .append('<option selected value="">Select Table</option>')
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
    }
}
