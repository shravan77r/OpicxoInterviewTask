$(function () {
    let enableAllSteps = false;

    $(".authorization").css("display", "none");

    if ($('#APIGUID').val() != '') {
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
                return true;
            }

            var form = $(this);

            // Clean up if user went backward before
            if (currentIndex < newIndex) {
                // To remove error styles
                $(".body:eq(" + newIndex + ") label.error", form).remove();
                $(".body:eq(" + newIndex + ") .error", form).removeClass("error");
            }

            // Disable validation on fields that are disabled or hidden.
            form.validate().settings.ignore = ":disabled,:hidden,.excluded";
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

    fnAuthorizationOnchange();
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

    var ApiData = {};

    ApiData.APIGUID = $('#APIGUID').val() != '' ? $('#APIGUID').val() : null;
    ApiData.Name = $('#Name').val();
    ApiData.Description = $('#Description').val();
    ApiData.APIEndPoint = $('#APIEndPoint').val();
    ApiData.AuthorizationType = $('#AuthorizationType').val();
    ApiData.AuthorizationUsername = $('#AuthorizationUsername').val();
    ApiData.AuthorizationPassword = $('#AuthorizationPassword').val();
    ApiData.InputParameterType = $('#InputParameterType').val();
    ApiData.BodyParameterType = $('#BodyParameterType').val();
    ApiData.AuthorizationOathAPIId = $('#AuthorizationOathAPIId').val();
    ApiData.AuthorizationTokenName = $('#AuthorizationTokenName').val();
 
    $('.customloader').show();

    $.ajax({
        type: "POST",
        url: "/APIData/ManageAPIData",
        data: ApiData,
        async: !0,
        cache: !1,
        success: function (data) {
            $('#APIGUID').val(data.apiGUID);
            fnShowMessage(data.message, data.messageType);
            $('.customloader').hide();
            IsSuccess = parseInt(data.status) == 1;
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });

    return IsSuccess;
}

function fnAuthorizationOnchange() {

    var authorization = !isNaN($('#AuthorizationType').val()) ? parseInt($('#AuthorizationType').val()) : 0;

    switch (authorization) {
        case 2:
            $(".divBasicAuth").removeClass("d-none");
            $(".divoAuth").addClass("d-none");
            break;
        case 3:
            $(".divBasicAuth").addClass("d-none");
            $(".divoAuth").removeClass("d-none");
            break;
    }
}
