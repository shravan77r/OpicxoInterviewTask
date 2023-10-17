$(function () {
    console.log('test');
    var templateType = !isNaN($("#TemplateType").val()) ? parseInt($("#TemplateType").val()) : 1;

    fnDisplayUniqueColumns(templateType);

    let enableAllSteps = false;
    if ($('#TemplateGUID').val() != '') {
        enableAllSteps = true;
    }
    $("#wizard").steps();
    $("#form").steps({
        bodyTag: "fieldset",
        enableFinishButton: false,
        enableAllSteps: enableAllSteps,
        onStepChanging: function (event, currentIndex, newIndex) {
            // Always allow going backward even if the current step contains invalid fields!
            if (currentIndex > newIndex) {
                if (newIndex === 0) { $("#TemplateType").attr("disabled", false) };
                return true;
            }

            // Forbid suppressing "Warning" step if the user is to young
            var TemplateForm = $('#TemplateForm').valid();
            if (!TemplateForm) {
                return false;
            }

            var form = $(this);

            // Clean up if user went backward before
            if (currentIndex < newIndex) {
                // To remove error styles
                $(".body:eq(" + newIndex + ") label.error", form).remove();
                $(".body:eq(" + newIndex + ") .error", form).removeClass("error");
                $("#TemplateType").attr("disabled", true);
            }

            // Disable validation on fields that are disabled or hidden.
            form.validate().settings.ignore = ":disabled,:hidden";
            var IsValidForm = form.valid();
            if (IsValidForm) {
                fnSaveData(currentIndex == 0);
                return true;
            } else {
                return false;
            }
            // Start validation; Prevent going forward if false
            //return form.valid();
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            //// Suppress (skip) "Warning" step if the user is old enough.
            //if (currentIndex === 2 && Number($("#age").val()) >= 18) {
            //    $(this).steps("next");
            //}

            //// Suppress (skip) "Warning" step if the user is old enough and wants to the previous step.
            //if (currentIndex === 2 && priorIndex === 3) {
            //    $(this).steps("previous");
            //}
        },
        onFinishing: function (event, currentIndex) {
            var form = $(this);

            // Disable validation on fields that are disabled.
            // At this point it's recommended to do an overall check (mean ignoring only disabled fields)
            form.validate().settings.ignore = ":disabled";

            // Start validation; Prevent form submission if false
            return form.valid();
        },
        onFinished: function (event, currentIndex) {
            var form = $(this);

            // Submit form input
            form.submit();
        }
    }).validate({
        errorPlacement: function (error, element) {
            element.before(error);
        },
        rules: {
            OriginSourceTypeId: {
                required: true
            }
        }
    });

    $('#linkTransferSchedule').on('click', function () {
        $('#ManageTransferSchedule').modal('show');
    });
});

function fnShowMessage(message, messageType) {
    if (message && messageType) {
        $('#divAlertMessage').removeClass();
        $('#divAlertMessage').addClass('alert alert-' + messageType, true);
        $('#divAlertMessage').html(message);
        $('#divAlertMessage').toggleClass('d-none', false);

        //setTimeout(function () {
        //    $('#divAlertMessage').toggleClass('d-none', true);
        //    $('#divAlertMessage').html('');
        //}, 3000);
    }
}

function fnSaveData(IsSaveFile) {
    var IsSuccess = false;
    var template = {};
    template.TemplateGUID = $('#TemplateGUID').val() != '' ? $('#TemplateGUID').val() : null;
    template.TemplateName = $('#TemplateName').val();
    template.TemplateType = $('#TemplateType').val();
    template.OriginSourceTypeId = $('#OriginSourceTypeId').val();
    template.OriginSourceFileTypeId = $('#OriginSourceFileTypeId').val();
    template.OriginSourceServer = $('#OriginSourceServer').val();
    template.OriginSourcePort = $('#OriginSourcePort').val();
    template.OriginSourceUsername = $('#OriginSourceUsername').val();
    template.OriginSourcePassword = $('#OriginSourcePassword').val();
    template.OriginSourceDatabase = $('#OriginSourceDatabase').val();
    template.OriginSourceFilePath = $('#OriginSourceFilePath').val();
    template.OriginSourceFileName = $('#OriginSourceFileName').val();
    template.OriginSourceAPITemplateId = $('#OriginSourceAPITemplateId').val();
    template.IsFirstColumnContainHeader = $('#IsFirstColumnContainHeader').is(':checked');
    template.TargetSourceTypeId = $('#TargetSourceTypeId').val();
    template.TargetSourceServer = $('#TargetSourceServer').val();
    template.TargetSourcePort = $('#TargetSourcePort').val();
    template.TargetSourceUsername = $('#TargetSourceUsername').val();
    template.TargetSourcePassword = $('#TargetSourcePassword').val();
    template.TargetSourceDatabase = $('#TargetSourceDatabase').val();
    template.IsScheduleEnabled = $('#IsScheduleEnabled').is(':checked');
    template.ScheduleType = $('#ScheduleType').val();
    template.FrequencyType = $('#FrequencyType').val();
    template.FrequencyRecurrsDailyEveryDay = $('#FrequencyRecurrsDailyEveryDay').val();
    template.FrequencyRecurrsWeeklyEveryWeek = $('#FrequencyRecurrsWeeklyEveryWeek').val();
    template.IsFrequencyRecurrsWeeklyOnMonday = $('#IsFrequencyRecurrsWeeklyOnMonday').is(':checked');
    template.IsFrequencyRecurrsWeeklyOnTuesday = $('#IsFrequencyRecurrsWeeklyOnTuesday').is(':checked');
    template.IsFrequencyRecurrsWeeklyOnWednesday = $('#IsFrequencyRecurrsWeeklyOnWednesday').is(':checked');
    template.IsFrequencyRecurrsWeeklyOnThursday = $('#IsFrequencyRecurrsWeeklyOnThursday').is(':checked');
    template.IsFrequencyRecurrsWeeklyOnFriday = $('#IsFrequencyRecurrsWeeklyOnFriday').is(':checked');
    template.IsFrequencyRecurrsWeeklyOnSaturday = $('#IsFrequencyRecurrsWeeklyOnSaturday').is(':checked');
    template.IsFrequencyRecurrsWeeklyOnSunday = $('#IsFrequencyRecurrsWeeklyOnSunday').is(':checked');
    template.FrequencyRecurrsMonthlyType = $('#FrequencyRecurrsMonthlyType').val();
    template.FrequencyRecurrsMonthtlyEveryMonth = $('#FrequencyRecurrsMonthtlyEveryMonth').val();
    template.FrequencyRecurrsMonthtlyDayOfMonth = $('#FrequencyRecurrsMonthtlyDayOfMonth').val();
    template.FrequencyRecurrsMonthtlyDayOfWeekOccurance = $('#FrequencyRecurrsMonthtlyDayOfWeekOccurance').val();
    template.FrequencyRecurrsMonthtlyDayOfWeek = $('#FrequencyRecurrsMonthtlyDayOfWeek').val();
    template.DailyFrequencyType = $('#DailyFrequencyType').val();
    template.DailyFrequencyTime = $('#DailyFrequencyTime').val();
    template.DailyFrequencyOccuranceType = $('#DailyFrequencyOccuranceType').val();
    template.DailyFrequencyOccuranceEvery = $('#DailyFrequencyOccuranceEvery').val();
    template.DailyFrequencyOccuranceStartTime = $('#DailyFrequencyOccuranceStartTime').val();
    template.DailyFrequencyOccuranceEndTime = $('#DailyFrequencyOccuranceEndTime').val();
    template.DurationStartDate = $('#DurationStartDate').val();
    template.IsDurationEndDateSpecified = $('#IsDurationEndDateSpecified').is(':checked');
    template.DurationEndDate = $('#DurationEndDate').val();
    template.OriginSourceAPITemplateId = $('#OriginSourceAPITemplateId').val();
    template.TargetSourceAPITemplateId = $('#TargetSourceAPITemplateId').val();
    $('.customloader').show();

    $.ajax({
        type: "POST",
        url: "/Template/ManageTemplate",
        data: template,
        async: !0,
        cache: !1,
        success: function (data) {
            $('#TemplateGUID').val(data.templateGUID);
            fnShowMessage(data.message, data.messageType);
           // fnChangeOriginTestConnectionButton();
            $('.customloader').hide();
            IsSuccess = parseInt(data.status) == 1;
            if (parseInt($('#OriginSourceTypeId').val()) == 1) {
                fnBindOriginSheetDropdown();
                if (IsSaveFile && parseInt($('#OriginSourceFileTypeId').val()) == 1) {
                    fnSaveFile();
                }
            }
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });

    return IsSuccess;
}

function fnSaveFile() {
    var TemplateGUID = $('#TemplateGUID').val();
    var file = $('#OriginFile')[0].files[0];
    if (file) {
        var extension = file.name.split('.').pop();
        if (file.size > 0 && (extension == 'csv' || extension == 'xlsx' || extension == 'xls')) {
            UploadFile(file);
        }
        function UploadFile(TargetFile) {
            $('.customloader').show();
            // create array to store the buffer chunks
            var FileChunk = [];
            // the file object itself that we will work with
            var file = TargetFile;
            // set up other initial vars
            var MaxFileSizeMB = 3;
            var BufferChunkSize = MaxFileSizeMB * (1024 * 1024);
            var FileStreamPos = 0;
            // set the initial chunk length
            var EndPos = BufferChunkSize;
            var Size = file.size;

            // add to the FileChunk array until we get to the end of the file
            while (FileStreamPos < Size) {
                // "slice" the file from the starting position/offset, to  the required length
                FileChunk.push(file.slice(FileStreamPos, EndPos));
                FileStreamPos = EndPos; // jump by the amount read
                EndPos = FileStreamPos + BufferChunkSize; // set next chunk length
            }
            // get total number of "files" we will be sending
            var TotalParts = FileChunk.length;
            var PartCount = 0;
            // loop through, pulling the first item from the array each time and sending it
            while (chunk = FileChunk.shift()) {
                PartCount++;
                // file name convention
                var FilePartName = file.name + ".part_" + PartCount + "." + TotalParts;
                // send the file
                UploadFileChunk(chunk, FilePartName);
            }
        }

        function UploadFileChunk(Chunk, FileName) {
            var formData = new FormData();
            formData.append('file', Chunk, FileName);
            formData.append('TemplateGUID', TemplateGUID);
            $.ajax({
                type: "POST",
                url: '/Template/UploadFile/',
                contentType: false,
                processData: false,
                data: formData,
                success: function (data) {
                    if (parseInt(data.IsSuccess) == 1) {
                        $('.customloader').hide();
                        fnBindOriginSheetDropdown();
                    }
                }, error: function (response) {
                    console.log(response);
                }
            });
        }
    }
}