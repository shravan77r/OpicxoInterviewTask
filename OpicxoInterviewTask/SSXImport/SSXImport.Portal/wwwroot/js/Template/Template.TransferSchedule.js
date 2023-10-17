$(function () {

    fnDisplayScheduleFields();
    fnDisplayFrequencyTypeFeilds();
    fnDisplayDailyFrequency();
    fnChangeDisplayDurationEndDate();

    $('#ScheduleType').on('change', function () {
        fnDisplayScheduleFields();
        fnDisplayFrequencyTypeFeilds();
        fnDisplayDailyFrequency();
        fnChangeDisplayDurationEndDate();
    });

    $('#FrequencyType').on('change', function () {
        fnDisplayFrequencyTypeFeilds();
    });

    $('#FrequencyRecurrsMonthlyType').on('change', function () {
        fnDisplayFrequencyRecurrsMonthlyType();
    });

    $('#DailyFrequencyType').on('change', function () {
        fnDisplayDailyFrequency();
    });
});

function fnDisplayScheduleFields() {
    var ScheduleType = parseInt($('#ScheduleType').val());
    if (ScheduleType == 1) {
        $('.schedulerFields').toggleClass("d-none", true);
    } else {
        $('.schedulerFields').toggleClass("d-none", false);
    }
}

function fnDisplayFrequencyTypeFeilds() {
    var ScheduleType = parseInt($('#ScheduleType').val());
    var FrequencyType = parseInt($('#FrequencyType').val());
    $('.schedulerFieldsDaily').toggleClass("d-none", true);
    $('.schedulerFieldsWeekly').toggleClass("d-none", true);
    $('.schedulerFieldsMonthly').toggleClass("d-none", true);
    if (ScheduleType == 2)
        if (FrequencyType == 1) {
            $('.schedulerFieldsDaily').toggleClass("d-none", false);
        } else if (FrequencyType == 2) {
            $('.schedulerFieldsWeekly').toggleClass("d-none", false);
        } else if (FrequencyType == 3) {
            $('.schedulerFieldsMonthly').toggleClass("d-none", false);
            fnDisplayFrequencyRecurrsMonthlyType();
        }
}

function fnDisplayFrequencyRecurrsMonthlyType() {
    $('.schedulerFieldsMonthlyDay').toggleClass("d-none", true);
    $('.schedulerFieldsMonthlyThe').toggleClass("d-none", true);
    var FrequencyRecurrsMonthlyType = parseInt($('#FrequencyRecurrsMonthlyType').val());
    if (FrequencyRecurrsMonthlyType == 1) {
        $('.schedulerFieldsMonthlyDay').toggleClass("d-none", false);
    } else if (FrequencyRecurrsMonthlyType == 2) {
        $('.schedulerFieldsMonthlyThe').toggleClass("d-none", false);
    }
}

function fnDisplayDailyFrequency() {
    $('.schedulerFieldsDailyFrequencyOnce').toggleClass("d-none", true);
    $('.schedulerFieldsDailyFrequencyEvery').toggleClass("d-none", true);
    var DailyFrequencyType = parseInt($('#DailyFrequencyType').val());
    if (DailyFrequencyType == 1) {
        $('.schedulerFieldsDailyFrequencyOnce').toggleClass("d-none", false);
    } else if (DailyFrequencyType == 2) {
        $('.schedulerFieldsDailyFrequencyEvery').toggleClass("d-none", false);
    }
}

function fnChangeInTransferSchedule() {
    if (!$('#frmTransferSchedule').valid()) {
        return false;
    }
    $('#ManageTransferSchedule').modal('hide');
}

$('#ManageTransferSchedule').on('hide.bs.modal', function (e) {
    console.log(e);
    if (e.target.className && e.target.className != 'input-group date') {
        var form = $('#frmTransferSchedule').valid();
        var ScheduleType = parseInt($('#ScheduleType').val());
        if (ScheduleType == 1) {
            $('#linkTransferSchedule').html('Not Scheduled');
        } else {
            if (!form) {
                $('#ScheduleType').val(1);
                $('#linkTransferSchedule').html('Not Scheduled');
                fnDisplayScheduleFields();
            } else {
                $('#linkTransferSchedule').html('Scheduled');
            }
        }
    }
});

function fnChangeDisplayDurationEndDate() {
    var IsDurationEndDateSpecified = $('#IsDurationEndDateSpecified').is(":checked");
    if (IsDurationEndDateSpecified) {
        $('.durationEndDate').toggleClass('d-none', false);
    } else {
        $('.durationEndDate').toggleClass('d-none', true);
    }
}