$(function () {
    $('#OriginSourceTypeId').on('change', function () {
        fnDisplayOriginDatabaseFields($(this).val());
        fnResetOriginConnection();
    });

    fnDisplayOriginDatabaseFields($('#OriginSourceTypeId').val());
    if ($('#OriginSourcePort').val() == '') {
        fnChangeDefaultOriginSourcePort($('#OriginSourceTypeId').val());
    }

    $('#OriginSourceServer,#OriginSourcePort,#OriginSourceUsername,#OriginSourcePassword').on('change', function () {
        fnResetOriginConnection();
    });

    $('#OriginSourceDatabase,OriginSourceAPITemplateId').on('change', function () {
        fnSetOriginSourceTable();
    });

    $('#OriginSourceFileTypeId').on('change', function () {
        fnSetDisplayFTPFields();
    });

    $('#OriginSourceFilePath').on('change', function () {
        fnSetOriginFiles();
    });

});

function fnDisplayOriginDatabaseFields(SourceId) {
    SourceId = parseInt(SourceId);
    if (SourceId == 1) {
        $('.excelFields').toggleClass('d-none', false);
        $('.originDatabaseFields').toggleClass('d-none', true);
        $('.apiFields').toggleClass('d-none', true);
        $('#lblOriginSource').val("Excel");
    } else if (SourceId == 2) {
        $('.excelFields').toggleClass('d-none', true);
        $('.ftpFields').toggleClass('d-none', true);
        $('.originDatabaseFields').toggleClass('d-none', false);
        $('.excelUploadFields').toggleClass('d-none', true);
        $('.apiFields').toggleClass('d-none', true);
        $('#lblOriginSource').val("MsSQL");
    }
    else if (SourceId == 3) {
        $('.excelUploadFields').toggleClass('d-none', true);
        $('.ftpFields').toggleClass('d-none', true);
        $('.excelFields').toggleClass('d-none', true);
        $('.originDatabaseFields').toggleClass('d-none', false);
        $('.apiFields').toggleClass('d-none', true);
        $('#lblOriginSource').val("MySQL");
    }
    else if (SourceId == 4) {
        $('.apiFields').toggleClass('d-none', false);
        $('.excelUploadFields').toggleClass('d-none', true);
        $('.ftpFields').toggleClass('d-none', true);
        $('.excelFields').toggleClass('d-none', true);
        $('.originDatabaseFields').toggleClass('d-none', true);
        $('#lblOriginSource').val("API");
    }
    fnChangeDefaultOriginSourcePort(SourceId);
    fnSetDisplayFTPFields();
}

function fnChangeDefaultOriginSourcePort(SourceId) {
    SourceId = parseInt(SourceId);
    if (SourceId == 1) {
        $('#OriginSourcePort').val($('#FTP_DefaultPort').val());
    }
    else if (SourceId == 2) {
        $('#OriginSourcePort').val($('#MsSQL_DefaultPort').val());
    }
    else if (SourceId == 3) {
        $('#OriginSourcePort').val($('#MySQL_DefaultPort').val());
    }
}

function fnTestOriginConnection() {
    var IsValidServer = $('#OriginSourceServer').valid();
    var IsValidPort = $('#OriginSourcePort').valid();
    var IsValidUsername = $('#OriginSourceUsername').valid();
    var IsValidPassword = $('#OriginSourcePassword').valid();
    var OriginSourceTypeId = parseInt($('#OriginSourceTypeId').val());
    if (IsValidServer
        && IsValidPort
        && IsValidUsername
        && IsValidPassword) {
        $('.customloader').show();
        fnResetOriginConnection();

        $.ajax({
            type: "POST",
            url: "/Template/ValidateConnection",
            data: {
                DataSourceId: OriginSourceTypeId,
                Server: $('#OriginSourceServer').val(),
                Port: $('#OriginSourcePort').val(),
                Username: $('#OriginSourceUsername').val(),
                Password: $('#OriginSourcePassword').val(),
                OriginSourceFileTypeId: !isNaN(parseInt($('#OriginSourceFileTypeId').val())) ? parseInt($('#OriginSourceFileTypeId').val()) : 0,
            },
            async: !0,
            cache: !1,
            timeout: 25000,
            success: function (data) {
                $('#IsValidOriginConnection').val(data.isValidConnection);

                if (OriginSourceTypeId == 1) {
                    if (parseInt(data.isValidConnection)) {
                        if (data.directories != '') {
                            var directories = $.parseJSON(data.directories);
                            for (var i = 0; i < directories.length; i++) {
                                $('#OriginSourceFilePath')
                                    .append('<option value="' + directories[i].Id + '">' + directories[i].Name + '</option>');
                            }
                            $('#OriginSourceFilePath').val('/');
                            if (data.files != '') {
                                var files = $.parseJSON(data.files);
                                for (var i = 0; i < files.length; i++) {
                                    $('#OriginSourceFileName')
                                        .append('<option value="' + files[i].Id + '">' + files[i].Name + '</option>');
                                }
                            }
                        }



                    }
                } else {
                    $('#OriginConnectionString').val(data.connectionString);
                    if (parseInt(data.isValidConnection) && data.databases != '') {
                        var databases = $.parseJSON(data.databases);
                        for (var i = 0; i < databases.length; i++) {
                            $('#OriginSourceDatabase')
                                .append('<option value="' + databases[i].Name + '">' + databases[i].Name + '</option>');
                        }
                    }
                }
                fnShowMessage(data.message, data.messageType);
                fnChangeOriginTestConnectionButton();
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

function fnChangeOriginTestConnectionButton() {
    var IsValidConnection = !isNaN(parseInt($('#IsValidOriginConnection').val())) ? parseInt($('#IsValidOriginConnection').val()) : 0;

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
    btn += '<button class="btn ' + btnClass + ' btn-lg mr-3 btn-sm" type="button" onclick="fnTestOriginConnection(event);">';
    btn += '<i class="fa ' + iconClass + '"></i>';
    btn += 'Test Connection';
    btn += '</button>';
    $('#btnTestOriginConnection').html(btn);
}

function fnResetOriginConnection() {
    $('#IsValidOriginConnection').val(0);
    fnChangeOriginTestConnectionButton();
    $('#OriginSourceDatabase')
        .empty()
        .append('<option selected value="">Select Database</option>')
        ;
    $('#OriginSourceTable')
        .empty()
        .append('<option selected value="">Select Table</option>')
        ;
    $('#OriginSourceFilePath')
        .empty()
        .append('<option selected value="">Select Directory</option>')
        ;
    $('#OriginSourceFileName')
        .empty()
        .append('<option selected value="">Select File</option>')
        ;
    $('#lblOriginSourceTable').html("Origin Source Table");
}

function fnSetOriginSourceTable() {
    $('#lblOriginSourceTable').html("Origin Source Table");
    $('#OriginSourceTable')
        .empty()
        .append('<option selected value="">Select Table</option>')
        ;

    var OriginSourceTypeId = parseInt($('#OriginSourceTypeId').val());

    if (OriginSourceTypeId == 4) // Origin Source = API 
    {
        $('#OriginSourceTable')
            .append('<option selected value="API Response">API Response</option>')
            ;
        return;
    }

    var Database = $('#OriginSourceDatabase').val();
    if (Database != '') {
        $('.customloader').show();
        $.ajax({
            type: "POST",
            url: "/Template/GetTableNames",
            data: {
                DataSourceId: OriginSourceTypeId,
                Server: $('#OriginSourceServer').val(),
                Port: $('#OriginSourcePort').val(),
                Username: $('#OriginSourceUsername').val(),
                Password: $('#OriginSourcePassword').val(),
                Database: $('#OriginSourceDatabase').val(),
            },
            async: !0,
            cache: !1,
            timeout: 25000,
            success: function (data) {
                if (parseInt(data.isValidConnection) && data.tables != '') {
                    var tables = $.parseJSON(data.tables);
                    for (var i = 0; i < tables.length; i++) {
                        $('#OriginSourceTable')
                            .append('<option value="' + tables[i].Name + '">' + tables[i].Name + '</option>');
                    }
                }
                if (data.messageType == 'danger') {
                    fnShowMessage("Database is not accessible using given credentials!", data.messageType);
                    $('#OriginSourceDatabase').val('');
                    $('#OriginSourceTable')
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

function fnSetOriginFiles() {
    $('#OriginSourceFileName')
        .empty()
        .append('<option selected value="">Select File</option>')
        ;

    var OriginSourceFilePath = $('#OriginSourceFilePath').val();
    var OriginSourceTypeId = parseInt($('#OriginSourceTypeId').val());
    if (OriginSourceFilePath != '') {
        $('.customloader').show();
        $.ajax({
            type: "POST",
            url: "/Template/GetFileNames",
            data: {
                DataSourceId: OriginSourceTypeId,
                Server: $('#OriginSourceServer').val(),
                Username: $('#OriginSourceUsername').val(),
                Password: $('#OriginSourcePassword').val(),
                Port: $('#OriginSourcePort').val(),
                OriginSourceFilePath: OriginSourceFilePath,
                OriginSourceFileTypeId: $('#OriginSourceFileTypeId').val(),
            },
            async: !0,
            cache: !1,
            timeout: 25000,
            success: function (data) {
                if (parseInt(data.isValidConnection) && data.files != '') {
                    var files = $.parseJSON(data.files);
                    for (var i = 0; i < files.length; i++) {
                        $('#OriginSourceFileName')
                            .append('<option value="' + files[i].Name + '">' + files[i].Name + '</option>');
                    }
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

function fnSetDisplayFTPFields() {
    var OriginSourceTypeId = parseInt($('#OriginSourceTypeId').val());
    if (OriginSourceTypeId == 1) {
        var OriginSourceFileTypeId = !isNaN(parseInt($('#OriginSourceFileTypeId').val())) ? parseInt($('#OriginSourceFileTypeId').val()) : 0;
        if (OriginSourceFileTypeId == 1) {
            $('.ftpFields').toggleClass('d-none', true);
            $('.excelUploadFields').toggleClass('d-none', false);
        } else if (OriginSourceFileTypeId == 2) {
            $('.excelUploadFields').toggleClass('d-none', true);
            $('.ftpFields').toggleClass('d-none', false);
        }
    }
}

