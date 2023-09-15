var IsFrom_Hdn = parseInt($('#IsFrom_hdn').val());

$(function () {
    fnBindActivityDropdown();

});

//============set default date Start==================
$(window).on('load', function () {
    $('#ActivityDateTime').val(formatDate(new Date()));
});

function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

function formatDate(date) {
    // debugger;
    return (
        [
            padTo2Digits(date.getDate()),
            padTo2Digits(date.getMonth() + 1),
            date.getFullYear(),
        ].join('/') +
        ' - ' +
        //[
        //    padTo2Digits(date.getHours()),
        //    padTo2Digits(date.getMinutes())
        //].join(':')
        formatAMPM(date)
    );
}
function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = padTo2Digits(hours) + ':' + padTo2Digits(minutes) + ' ' + ampm;
    return strTime;
}
//============set default date END==================

// Open Activity Pop up
function fnOpenActivity(Id, LeadStatusId, LeadId, Tab) {
    if (Id) {
        $('#ModuleId_hdn').val(Id);
        $('#LeadId_hdn').val(LeadId);
        $('#LeadStatusId_hdn').val(LeadStatusId);

        $('#ActivityById').append(new Option($("#DefaultAssignToName_hdn").val(), $("#DefaultAssignTo_hdn").val(), true, true)).trigger('change');

        if (Tab == 4) {
            $("#tab-1").removeClass('active');
            $("#tab-2").removeClass('active');
            $("#tab-3").removeClass('active');
            $("#tab-5").removeClass('active');
            $("#tab-4").addClass('active');

            $("#tab1").removeClass('active show');
            $("#tab2").removeClass('active show');
            $("#tab3").removeClass('active show');
            $("#tab5").removeClass('active show');
            $("#tab4").addClass('active show');

            if ($("#AssignTo_hdn").val() != "" && $("#AssignTo_hdn").val() != "{}") {
                var Items = JSON.parse($("#AssignTo_hdn").val());
                $("#SiteVisitAssignTo").val(Object.keys(Items)[0]).trigger('change');
            }

            fnLoadActivity(4);
            fnClearActivityData();
            $('#ActivityModal').modal('show');
        }
        else {
            $("#tab-1").addClass('active');
            $("#tab-2").removeClass('active');
            $("#tab-3").removeClass('active');
            $("#tab-4").removeClass('active');
            $("#tab-5").removeClass('active');

            $("#tab1").addClass('active show');
            $("#tab2").removeClass('active show');
            $("#tab3").removeClass('active show');
            $("#tab4").removeClass('active show');
            $("#tab5").removeClass('active show');

            fnLoadActivity(1);
            fnClearActivityData();
            $('#ActivityModal').modal('show');
        }

    }
}

// List Get list of All Activity
function fnLoadActivity(TypeId) {
    TypeId = !isNaN(parseInt(TypeId)) ? parseInt(TypeId) : 0;
    var ModuleIdId = $('#ModuleId_hdn').val();
    if (ModuleIdId && TypeId) {
        $.ajax({
            type: "POST",
            url: "/Transactions/Lead/GetActivityList",
            data: {
                RefTableId: ModuleIdId,
                IsFrom: parseInt($('#IsFrom_hdn').val()),
                TypeId: TypeId
            },
            async: !0,
            cache: !1,
            success: function (data) {
                if (TypeId == 1) {
                    $('#divFollowUpListPartial').html(data);
                } else if (TypeId == 2) {
                    $('#divLogACallListPartial').html(data);
                } else if (TypeId == 3) {
                    $('#divTaskListPartial').html(data);
                } else if (TypeId == 4) {
                    $('#divSiteVisitListPartial').html(data);
                } else if (TypeId == 5) {
                    $('#divLeadActivityListPartial').html(data);
                }
            },
            error: function (response) {
                console.log(response);
            },
            complete: function () {
                fnLoadActivityCount();
            },
        });
    }
}

///Get Count of All Activity
function fnLoadActivityCount() {
    var LeadId = $('#ModuleId_hdn').val();
    if (LeadId) {
        $.ajax({
            type: "POST",
            url: "/Transactions/Lead/GetActivityCount",
            data: {
                LeadId: LeadId,
                IsFrom: parseInt($('#IsFrom_hdn').val())
            },
            async: !1,
            cache: !1,
            success: function (data) {
                $('.lblFollowUpCount').html(data.FollowUpCount);
                $('.lblLogACallCount').html(data.LogACallCount);
                $('.lblTaskCount').html(data.TaskCount);
                $('.lblSiteVisitCount').html(data.SiteVisitCount);
                $('.lblLeadActivityCount').html(data.LeadActivityCount);
                $('#lblFollowUpCount_' + LeadId).html(data.FollowUpCount);
            },
            error: function (response) {
                console.log(response);
            },
        });
    }
}

// Submit Activity Details
function fnSubmitActivity(TypeId) {
    var RefTableId = $('#ModuleId_hdn').val();
    var Id = !isNaN(parseInt($('#ActivtiyId_hdn').val())) ? parseInt($('#ActivtiyId_hdn').val()) : 0;
    var IsDelete = 0;
    //Lead Followup details
    var SubjectId = 0;
    var SubjectName = '';
    var AssignedId = 0;
    var Description = '';
    var FollowupObjectiveId = 0;
    var FollowupObjectiveName = '';
    var NextFollowUpDate = '';
    //Lead LogACall details
    var Calllog_SubjectId = 0;
    var CalllogSubjectName = '';
    var CommentId = 0;
    var CommentName = '';
    var Calllog_Description = '';
    //Lead Task details
    var TaskId = 0;
    var TaskName = '';
    //var SubtaskId = 0
    //var SubtaskName = '';
    var StartDate = '';
    var EndDate = '';
    var ActionStep = '';
    var PlannedHours = 0;
    var ActualHours = 0;
    var TaskStatus = 0;
    var TaskStatusName = '';
    //Site Visit details
    var Sitevisit_PropertyId = 0;
    var PropertyName = '';
    var SiteVisitDescription = '';
    var SiteVisitInterestedProject = '';
    var SiteVisitMaxBudget = 0;
    var NextSiteVisitPlan = '';
    var SiteVisitStatus = 0;
    var SiteVisitStatusName = '';
    var Remarks = '';
    //Lead Activity details
    var ActivityDateTime = '';
    var ActivityById = 0;
    var ActivityByName = '';
    var ActivityStageId = 0;
    var ActivityStageName = '';
    var ActivityRemarks = '';

    if (RefTableId) {
        // Follow Up
        if (TypeId == 1) {
            SubjectId = !isNaN(parseInt($('#FollowUpSubject').val())) ? parseInt($('#FollowUpSubject').val()) : 0;
            SubjectName = $('#FollowUpSubject option:selected').text();
            if (!SubjectId) {
                swal("Please Select Subject!", {
                    icon: "error",
                });
                $('#FollowUpSubject').focus();
                return false;
            }

            AssignedId = !isNaN(parseInt($('#FollowUpAssignTo').val())) ? parseInt($('#FollowUpAssignTo').val()) : 0;
            if (!AssignedId) {
                swal("Please Select Assigned To!", {
                    icon: "error",
                });
                $('#FollowUpAssignTo').focus();
                return false;
            }

            Description = $('#FollowUpDescription').val();
            FollowupObjectiveId = !isNaN(parseInt($('#FollowUpObjective').val())) ? parseInt($('#FollowUpObjective').val()) : 0;
            FollowupObjectiveName = $('#FollowUpObjective option:selected').text();
            NextFollowUpDate = $('#NextFollowUpDate').val();
            if (!NextFollowUpDate) {
                swal("Please Select Next Followup Date/Time!", {
                    icon: "error",
                });
                $('#NextFollowUpDate').focus();
                return false;
            }
        }
        // Log A Call
        else if (TypeId == 2) {

            Calllog_SubjectId = !isNaN(parseInt($('#LogACallSubject').val())) ? parseInt($('#LogACallSubject').val()) : 0;
            CalllogSubjectName = $('#LogACallSubject option:selected').text();
            if (!Calllog_SubjectId) {
                swal("Please Select Subject!", {
                    icon: "error",
                });
                $('#LogACallSubject').focus();
                return false;
            }

            CommentId = !isNaN(parseInt($('#LogACallComment').val())) ? parseInt($('#LogACallComment').val()) : 0;
            CommentName = $('#LogACallComment option:selected').text();
            Calllog_Description = $('#LogACallDescription').val();

        }
        //Task
        else if (TypeId == 3) {

            TaskId = !isNaN(parseInt($('#Task').val())) ? parseInt($('#Task').val()) : 0;
            TaskName = $('#Task option:selected').text();
            if (!TaskId) {
                swal("Please Select Task!", {
                    icon: "error",
                });
                $('#Task').focus();
                return false;
            }

            //SubtaskId = !isNaN(parseInt($('#SubTask').val())) ? parseInt($('#SubTask').val()) : 0;
            //SubtaskName = $('#SubTask option:selected').text();
            //if (!SubtaskId) {
            //    swal("Please Select SubTask!", {
            //        icon: "error",
            //    });
            //    $('#SubTask').focus();
            //    return false;
            //}

            StartDate = $('#StartDate').val();
            EndDate = $('#EndDate').val();
            if (StartDate != "" && EndDate == "") {
                swal("Please Select EndDate", {
                    icon: "error",
                });
                $('#EndDate').focus();
                return false;
            }
            if (StartDate == "" && EndDate != "") {
                swal("Please Select StartDate", {
                    icon: "error",
                });
                $('#StartDate').focus();
                return false;
            }

            ActionStep = $('#TaskDetails').val();
            if (!ActionStep) {
                swal("Please Enter Task Details!", {
                    icon: "error",
                });
                $('#TaskDetails').focus();
                return false;
            }

            AssignedId = !isNaN(parseInt($('#TaskTo').val())) ? parseInt($('#TaskTo').val()) : 0;
            if (!AssignedId) {
                swal("Please Select Assigned To!", {
                    icon: "error",
                });
                $('#TaskTo').focus();
                return false;
            }

            PlannedHours = !isNaN(parseInt($('#PlannedHours').val())) ? parseInt($('#PlannedHours').val()) : 0;
            ActualHours = !isNaN(parseInt($('#ActualHours').val())) ? parseInt($('#ActualHours').val()) : 0;

            TaskStatus = !isNaN(parseInt($('#TaskActivityStatus').val())) ? parseInt($('#TaskActivityStatus').val()) : 0;
            TaskStatusName = $('#TaskActivityStatus option:selected').text();
        }
        //Site Visit
        else if (TypeId == 4) {
            Sitevisit_PropertyId = !isNaN(parseInt($('#SitevisitPropertyId').val())) ? parseInt($('#SitevisitPropertyId').val()) : 0;
            PropertyName = $('#SitevisitPropertyId option:selected').text();
            if (!Sitevisit_PropertyId) {
                swal("Please Select Property!", {
                    icon: "error",
                });
                $('#SitevisitPropertyId').focus();
                return false;
            }

            AssignedId = !isNaN(parseInt($('#SiteVisitAssignTo').val())) ? parseInt($('#SiteVisitAssignTo').val()) : 0;
            if (!AssignedId) {
                swal("Please Select Employee!", {
                    icon: "error",
                });
                $('#SiteVisitAssignTo').focus();
                return false;
            }

            SiteVisitDescription = $('#SiteVisitDescription').val();
            SiteVisitStatus = !isNaN(parseInt($('#SiteVisitStatus').val())) ? parseInt($('#SiteVisitStatus').val()) : 0;
            SiteVisitStatusName = $('#SiteVisitStatus option:selected').text();
            if (!SiteVisitStatus) {
                swal("Please Select Status!", {
                    icon: "error",
                });
                $('#SiteVisitStatus').focus();
                return false;
            }
            NextSiteVisitPlan = $('#NextSiteVisitPlan').val();
            if (!NextSiteVisitPlan) {
                swal("Please Select Next SiteVisit Plan Date/Time!", {
                    icon: "error",
                });
                $('#NextSiteVisitPlan').focus();
                return false;
            }
            SiteVisitInterestedProject = $('#SiteVisitInterestedProject').val();
            SiteVisitMaxBudget = $('#SiteVisitMaxBudget').val();
            Remarks = $('#SiteVisitRemarks').val();
        }
        //Lead Activity 
        else if (TypeId == 5) {
            ActivityById = !isNaN(parseInt($('#ActivityById').val())) ? parseInt($('#ActivityById').val()) : 0;
            ActivityByName = $('#ActivityById option:selected').text();
            //if (!ActivityById) {
            //    swal("Please Select ActivityBy!", {
            //        icon: "error",
            //    });
            //    $('#ActivityById').focus();
            //    return false;
            //}

            ActivityStageId = !isNaN(parseInt($('#ActivityStageId').val())) ? parseInt($('#ActivityStageId').val()) : 0;
            if (!ActivityStageId) {
                swal("Please Select Stage!", {
                    icon: "error",
                });
                $('#ActivityStageId').focus();
                return false;
            }
            ActivityDateTime = $('#ActivityDateTime').val();
            ActivityRemarks = $('#ActivityRemarks').val();
        }

        $.ajax({
            type: "POST",
            url: "/Transactions/Lead/ActivityOperation",
            data: {
                RefTableId: RefTableId,
                Id: Id,
                IsDelete: IsDelete,
                IsFrom: parseInt($('#IsFrom_hdn').val()),
                AssignedId: AssignedId,
                SubjectId: SubjectId,
                SubjectName: SubjectName,
                FollowupObjectiveId: FollowupObjectiveId,
                FollowupObjectiveName: FollowupObjectiveName,
                Description: Description,
                NextFollowUpDate: NextFollowUpDate,

                Calllog_SubjectId: Calllog_SubjectId,
                CalllogSubjectName: CalllogSubjectName,
                CommentId: CommentId,
                CommentName: CommentName,
                Calllog_Description: Calllog_Description,

                TaskId: TaskId,
                TaskName: TaskName,
                //SubtaskId: SubtaskId,
                //SubtaskName: SubtaskName,
                StartDate: StartDate,
                EndDate: EndDate,
                ActionStep: ActionStep,
                PlannedHours: PlannedHours,
                ActualHours: ActualHours,
                ActivityTypeId: TypeId,
                TaskStatus: TaskStatus,
                TaskStatusName: TaskStatus,

                Sitevisit_PropertyId: Sitevisit_PropertyId,
                InternalDescription: SiteVisitDescription,
                ExternlDescription: SiteVisitInterestedProject,
                MaxBudget: SiteVisitMaxBudget,
                NextSiteVisitPlan: NextSiteVisitPlan,
                SiteVisitStatus: SiteVisitStatus,
                Remarks: Remarks,

                ActivityById: ActivityById,
                ActivityByName: ActivityByName,
                ActivityStageId: ActivityStageId,
                ActivityStageName: ActivityStageName,
                ActivityDateTime: ActivityDateTime,
                ActivityRemarks: ActivityRemarks,

            },
            async: !1,
            cache: !1,
            success: function (data) {
                swal(data.Message, {
                    icon: data.MessageType,
                });
                fnLoadActivity(TypeId);
                //fnBindList();
            },
            error: function (response) {
                console.log(response);
            },
            complete: function () {
                fnClearActivityData();
            },
        });
    }
}
// Load Activity Data for Edit
function fnEditActivity(ActivityId, TypeId) {
    ActivityId = !isNaN(parseInt(ActivityId)) ? parseInt(ActivityId) : 0;
    TypeId = !isNaN(parseInt(TypeId)) ? parseInt(TypeId) : 0;

    //Lead Followup details
    var SubjectId = 0;
    var SubjectName = '';
    var AssignedId = 0;
    var AssignedName = '';
    var Description = '';
    var FollowupObjectiveId = 0;
    var FollowupObjectiveName = '';
    var NextFollowUpDate = '';
    //Lead LogACall details
    var Calllog_SubjectId = 0;
    var CalllogSubjectName = '';
    var CommentId = 0;
    var CommentName = '';
    var Calllog_Description = '';
    //Lead Task details
    var TaskId = 0;
    var TaskName = '';
    //var SubtaskId = 0;
    //var SubtaskName = '';
    var StartDate = '';
    var EndDate = '';
    var ActionStep = '';
    var PlannedHours = 0;
    var ActualHours = 0;
    var TaskStatus = 0;
    var TaskStatusName = '';
    //Site Visit details
    var Sitevisit_PropertyId = 0;
    var PropertyName = '';
    var SiteVisitDescription = '';
    var SiteVisitInterestedProject = '';
    var MaxBudget = 0;
    var NextSiteVisitPlan = '';
    var SiteVisitStatus = 0;
    var SiteVisitStatusName = '';
    var Remarks = '';
    //Lead Activity details
    var ActivityDateTime = '';
    var ActivityById = 0;
    var ActivityByName = '';
    var ActivityStageId = 0;
    var ActivityStageName = '';
    var ActivityRemarks = '';

    if (ActivityId && TypeId) {
        fnClearActivityData();
        $('#ActivtiyId_hdn').val(ActivityId);

        $.ajax({
            type: "POST",
            url: "/Transactions/Lead/GetActivityData",
            data: {
                Id: ActivityId
            },
            async: !1,
            cache: !1,
            success: function (data) {
                if (parseInt(data.StatusCode) == 1) {
                    var obj = data.Data;
                    AssignedId = parseInt(obj.AssignedId);
                    AssignedName = obj.AssignedName;
                    SubjectId = parseInt(obj.SubjectId);
                    SubjectName = obj.SubjectName;
                    Description = obj.Description;
                    FollowupObjectiveId = parseInt(obj.FollowupObjectiveId);
                    FollowupObjectiveName = obj.FollowupObjectiveName;
                    NextFollowUpDate = obj.NextFollowUpDate;

                    Calllog_SubjectId = parseInt(obj.Calllog_SubjectId);
                    CalllogSubjectName = obj.CalllogSubjectName;
                    CommentId = parseInt(obj.CommentId);
                    CommentName = obj.CommentName;
                    Calllog_Description = obj.Calllog_Description;

                    TaskId = parseInt(obj.TaskId);
                    TaskName = obj.TaskName;
                    //SubtaskId = parseInt(obj.SubtaskId);
                    //SubtaskName = obj.SubtaskName;
                    StartDate = obj.StartDate;
                    EndDate = obj.EndDate;
                    ActionStep = obj.ActionStep;
                    PlannedHours = parseFloat(obj.PlannedHours);
                    ActualHours = parseFloat(obj.ActualHours);
                    TaskStatus = parseInt(obj.TaskStatus);
                    TaskStatusName = obj.TaskStatusName;

                    Sitevisit_PropertyId = parseInt(obj.Sitevisit_PropertyId);
                    PropertyName = obj.PropertyName;
                    SiteVisitDescription = obj.InternalDescription;
                    SiteVisitInterestedProject = obj.ExternlDescription;
                    MaxBudget = obj.MaxBudget;
                    NextSiteVisitPlan = obj.NextSiteVisitPlan;
                    SiteVisitStatus = parseInt(obj.SiteVisitStatus);
                    SiteVisitStatusName = obj.SiteVisitStatusName;
                    BHKId = obj.BHKId;
                    Remarks = obj.Remarks;

                    ActivityById = parseInt(obj.ActivityById);
                    ActivityByName = obj.ActivityByName;
                    ActivityStageId = parseInt(obj.ActivityStageId);
                    ActivityStageName = obj.ActivityStageName;
                    ActivityDateTime = obj.ActivityDateTime;
                    ActivityRemarks = obj.ActivityRemarks;
                }
                if (data.MessageType == 'error') {
                    swal(data.Message, {
                        icon: data.MessageType,
                    });
                }
            },
            error: function (response) {
                console.log(response);
            },
        });

        if (TypeId == 1) {
            $('#FollowUpSubject').append(new Option(SubjectName, SubjectId, true, true)).trigger('change');
            $('#FollowUpAssignTo').append(new Option(AssignedName, AssignedId, true, true)).trigger('change');
            $('#FollowUpDescription').val(Description);
            $('#FollowUpObjective').append(new Option(FollowupObjectiveName, FollowupObjectiveId, true, true)).trigger('change');
            $('#NextFollowUpDate').val(NextFollowUpDate);

        } else if (TypeId == 2) {
            $('#LogACallSubject').append(new Option(CalllogSubjectName, Calllog_SubjectId, true, true)).trigger('change');
            $('#LogACallComment').append(new Option(CommentName, CommentId, true, true)).trigger('change');
            $('#LogACallDescription').val(Calllog_Description);
        } else if (TypeId == 3) {
            $('#Task').append(new Option(TaskName, TaskId, true, true)).trigger('change');
            //$('#SubTask').append(new Option(SubtaskName, SubtaskId, true, true)).trigger('change');
            $('#StartDate').val(StartDate);
            $('#EndDate').val(EndDate);
            $('#TaskDetails').val(ActionStep);
            $('#TaskTo').append(new Option(AssignedName, AssignedId, true, true)).trigger('change');
            $('#PlannedHours').val(parseFloat(PlannedHours).toFixed($('#D_L').val()));
            $('#ActualHours').val(parseFloat(ActualHours).toFixed($('#D_L').val()));
            $('#TaskActivityStatus').append(new Option(TaskStatusName, TaskStatus, true, true)).trigger('change');
        } else if (TypeId == 4) {
            $('#SitevisitPropertyId').attr("disabled", true);
            $('#NextSiteVisitPlan').attr("readonly", true);
            $('#SitevisitPropertyId').append(new Option(PropertyName, Sitevisit_PropertyId, true, true)).trigger('change');
            $('#SiteVisitAssignTo').append(new Option(AssignedName, AssignedId, true, true)).trigger('change');
            $('#SiteVisitDescription').val(SiteVisitDescription);
            //$('#SiteVisitStatus').append(new Option(SiteVisitStatusName, SiteVisitStatus, true, true)).trigger('change');
            $('#SiteVisitStatus').val(SiteVisitStatus);
            $('#NextSiteVisitPlan').val(NextSiteVisitPlan);
            $('#SiteVisitInterestedProject').val(SiteVisitInterestedProject);
            $('#SiteVisitMaxBudget').val(MaxBudget);
            $('#SiteVisitRemarks').val(Remarks);
        } else if (TypeId == 5) {
            $('#ActivityById').append(new Option(ActivityByName, ActivityById, true, true)).trigger('change');
            $('#ActivityStageId').append(new Option(ActivityStageName, ActivityStageId, true, true)).trigger('change');
            $('#ActivityDateTime').val(ActivityDateTime);
            $('#ActivityRemarks').val(ActivityRemarks);
        }
    }
}

// Clear Activity Form
function fnClearActivityData() {
    $('#ActivtiyId_hdn').val(0);

    $('#FollowUpSubject').val('').trigger('change');
    $('#FollowUpAssignTo').val('').trigger('change');
    $('#FollowUpDescription').val('');
    $('#NextFollowUpDate').val('');
    $('#FollowUpObjective').val('').trigger('change');

    $('#LogACallSubject').val('').trigger('change');
    $('#LogACallComment').val('').trigger('change');
    $('#LogACallDescription').val('');

    $('#Task').val('').trigger('change');
    //$('#SubTask').val('').trigger('change');
    $('#StartDate').val('');
    $('#EndDate').val('');
    $('#TaskDetails').val('');
    $('#TaskTo').val('').trigger('change');
    $('#PlannedHours').val(parseFloat(0).toFixed($('#D_L').val()));
    $('#ActualHours').val(parseFloat(0).toFixed($('#D_L').val()));
    $('#TaskActivityStatus').val(1).trigger('change');

    $('#SitevisitPropertyId').attr("disabled", false);
    $('#NextSiteVisitPlan').attr("readonly", false);
    $('#SitevisitPropertyId').val('').trigger('change');
    $('#SiteVisitAssignTo').val('').trigger('change');
    $('#SiteVisitDescription').val('');
    $('#SiteVisitStatus').val(0);
    $('#NextSiteVisitPlan').val('');
    $('#SiteVisitInterestedProject').val('');
    $('#SiteVisitMaxBudget').val(parseFloat(0).toFixed($('#D_L').val()));
    $('#SiteVisitRemarks').val('');

    //$('#ActivityById').val('').trigger('change');
    $('#ActivityStageId').val('').trigger('change');
    $('#ActivityDateTime').val(formatDate(new Date()));
    $('#ActivityRemarks').val('');
}

// Delete Activity Data
function fnDeleteActivity(ActivityId, TypeId) {
    ActivityId = !isNaN(parseInt(ActivityId)) ? parseInt(ActivityId) : 0;
    TypeId = !isNaN(parseInt(TypeId)) ? parseInt(TypeId) : 0;

    var Id = ActivityId;
    var IsDelete = 1;

    $.ajax({
        type: "POST",
        url: "/Transactions/Lead/ActivityOperation",
        data: {
            Id: Id,
            IsDelete: IsDelete,
        },
        async: !1,
        cache: !1,
        success: function (data) {
            swal(data.Message, {
                icon: data.MessageType,
            });
            fnLoadActivity(TypeId);
        },
        error: function (response) {
            console.log(response);
        },
    });
}

// Update Activity Status as Completed
function fnCompleteActivity(ActivityId, TypeId) {
    ActivityId = !isNaN(parseInt(ActivityId)) ? parseInt(ActivityId) : 0;
    TypeId = !isNaN(parseInt(TypeId)) ? parseInt(TypeId) : 0;

    var Id = ActivityId;

    $.ajax({
        type: "POST",
        url: "/Transactions/Lead/UpdateActivityStatus",
        data: {
            Id: Id,
            FollowupStatus: 1,
        },
        async: !1,
        cache: !1,
        success: function (data) {
            swal(data.Message, {
                icon: data.MessageType,
            });
            fnLoadActivity(TypeId);
        },
        error: function (response) {
            console.log(response);
        },
    });
}

// Bind Select2 Dropdown in Activity Pop up
function fnBindActivityDropdown() {

    $('#FollowUpSubject').select2({
        ajax: {
            url: '/Common/GetActivitySubjectSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Subject',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });
    //$('#FollowUpAssignTo, #TaskTo, #ActivityById').select2({
    //    ajax: {
    //        url: '/Common/GetEmployeeSearchList',
    //        type: 'POST',
    //        dataType: 'json',
    //        cache: false,
    //        delay: 250,
    //        data: function (params) {
    //            return {
    //                keyWord: params.term, // search term
    //                pageIndex: params.page || 1,
    //                pageSize: pageSize
    //            };
    //        },
    //        processResults: function (data, params) {
    //            params.page = params.page || 1;
    //            return {
    //                results: data.results,
    //                pagination: {
    //                    more: params.page * pageSize < data.RecordCount
    //                }
    //            };
    //        }
    //    },
    //    placeholder: 'Select Assigned To',
    //    minimumInputLength: minimumInputLength,
    //    width: '100%',
    //    allowClear: false,
    //});

    if ($("#IsFrom_hdn").val() == 1) {
        var DepartmentName = 'Presales'
    } else
        if ($("#IsFrom_hdn").val() == 2) {
            var DepartmentName = 'Sales'
        }

    $('#FollowUpAssignTo, #TaskTo, #ActivityById').select2({
        ajax: {
            url: '/Common/GetAssignToDepartmentWiseSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    Department: DepartmentName
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Assigned To',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });
    $('#FollowUpObjective').select2({
        ajax: {
            url: '/Common/GetActivityFollowupObjectiveSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Follow-up Objective',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    $('#LogACallSubject').select2({
        ajax: {
            url: '/Common/GetActivitySubjectSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Subject',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });
    $('#LogACallComment').select2({
        ajax: {
            url: '/Common/GetActivityLogsCallCommentSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Comment',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });
    $('#LogACallComment').select2({
        ajax: {
            url: '/Common/GetActivityLogsCallCommentSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Comment',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });
    $('#Task').select2({
        ajax: {
            url: '/Common/GetActivityTaskSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Task',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });
    //$('#SubTask').select2({
    //    ajax: {
    //        url: '/Common/GetActivitySubtaskSearchList',
    //        type: 'POST',
    //        dataType: 'json',
    //        cache: false,
    //        delay: 250,
    //        data: function (params) {
    //            return {
    //                keyWord: params.term, // search term
    //                pageIndex: params.page || 1,
    //                pageSize: pageSize,
    //                TaskId: $("#Task").val(),
    //            };
    //        },
    //        processResults: function (data, params) {
    //            params.page = params.page || 1;
    //            return {
    //                results: data.results,
    //                pagination: {
    //                    more: params.page * pageSize < data.RecordCount
    //                }
    //            };
    //        }
    //    },
    //    placeholder: 'Select Subtask',
    //    minimumInputLength: minimumInputLength,
    //    width: '100%',
    //    allowClear: false,
    //});
    $('#TaskActivityStatus').select2({
        ajax: {
            url: '/Common/GetActivityTaskStatusSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Task Status',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    //$('#SiteVisitAssignTo').select2({
    //    ajax: {
    //        url: '/Common/GetEmployeeSearchList',
    //        type: 'POST',
    //        dataType: 'json',
    //        cache: false,
    //        delay: 250,
    //        data: function (params) {
    //            return {
    //                keyWord: params.term, // search term
    //                pageIndex: params.page || 1,
    //                pageSize: pageSize
    //            };
    //        },
    //        processResults: function (data, params) {
    //            params.page = params.page || 1;
    //            return {
    //                results: data.results,
    //                pagination: {
    //                    more: params.page * pageSize < data.RecordCount
    //                }
    //            };
    //        }
    //    },
    //    placeholder: 'Select Employee',
    //    minimumInputLength: minimumInputLength,
    //    width: '100%',
    //    allowClear: false,
    //});
    $('#SiteVisitAssignTo').select2({
        ajax: {
            url: '/Common/GetAssignToDepartmentWiseSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    Department: 'Sales'
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Employee',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    $('#SitevisitPropertyId').select2({
        ajax: {
            url: '/Common/GetLeadMappedPropertySearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    LeadId: $("#LeadId_hdn").val(),
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: params.page * pageSize < data.RecordCount
                    }
                };
            }
        },
        placeholder: 'Select Property',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    if (IsFrom_Hdn == 1) {
        $('#ActivityStageId').select2({
            ajax: {
                url: '/Common/GetLeadStageSearchList',
                type: 'POST',
                dataType: 'json',
                cache: false,
                delay: 250,
                data: function (params) {
                    return {
                        keyWord: params.term, // search term
                        pageIndex: params.page || 1,
                        pageSize: pageSize,
                        LeadStatusId: isNaN(parseInt($("#LeadStatusId_hdn").val())) ? 0 : $("#LeadStatusId_hdn").val(),//$("#LeadStatusId_hdn").val(),

                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.results,
                        pagination: {
                            more: params.page * pageSize < data.RecordCount
                        }
                    };
                }
            },
            placeholder: 'Select Stage',
            minimumInputLength: minimumInputLength,
            width: '100%',
            allowClear: false,
        });
    }
    else if (IsFrom_Hdn == 2) {
        $('#ActivityStageId').select2({
            ajax: {
                url: '/Common/GetOpportunityStageSearchList',
                type: 'POST',
                dataType: 'json',
                cache: false,
                delay: 250,
                data: function (params) {
                    return {
                        keyWord: params.term, // search term
                        pageIndex: params.page || 1,
                        pageSize: pageSize,
                        StatusId: isNaN(parseInt($("#LeadStatusId_hdn").val())) ? 0 : $("#LeadStatusId_hdn").val(),//$("#LeadStatusId_hdn").val(),

                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.results,
                        pagination: {
                            more: params.page * pageSize < data.RecordCount
                        }
                    };
                }
            },
            placeholder: 'Select Stage',
            minimumInputLength: minimumInputLength,
            width: '100%',
            allowClear: false,
        });
    }
}

function fnCloseActivityModal() {
    fnBindList();
}
function fnCancelActivity(TypeId) {
    fnClearActivityData();
    fnLoadActivity(TypeId);
}

function FromDateToDate() {
    var StartDate = $("#StartDate").val().substring(0, 10);
    var EndDate = $("#EndDate").val().substring(0, 10);

    var StartTime = $("#StartDate").val().slice(-8);
    var EndTime = $("#EndDate").val().slice(-8);

    var CurentDateTime = new Date();
    var TodayTime = CurentDateTime.getHours().toString().padStart(2, '0') + ":" + CurentDateTime.getMinutes().toString().padStart(2, '0');

    const yyyy = CurentDateTime.getFullYear().toString().padStart(2, '0');
    let mm = CurentDateTime.getMonth() + 1; // Months start at 0!
    let dd = CurentDateTime.getDate();

    if (dd < 10) dd = '0' + dd;
    if (mm < 10) mm = '0' + mm;

    var TodayDate = dd + "/" + mm + "/" + yyyy;

    var Today = TodayDate;
    Today = Today.split("/");
    Today = Today[1] + "/" + Today[0] + "/" + Today[2];

    //if (StartDate != "") {

    //    var start = StartDate;
    //    start = start.split("/");
    //    start = start[1] + "/" + start[0] + "/" + start[2];

    //    Today = new Date(Today);
    //    start = new Date(start);
    //    var diff = new Date(start - Today);
    //    var days = diff / 1000 / 60 / 60 / 24;

    //    if (days < 0) {
    //        swal("Start Date Can not be less than Today Date", {
    //            icon: "info"
    //        })
    //        $("#StartDate").val('');
    //        $("#EndDate").val('');
    //    }

    //    if (days == 0) {
    //        var startT = StartTime;
    //        startT = startT.split(":");

    //        var TTime = TodayTime;
    //        TTime = TTime.split(":");

    //        var diffTS = startT[0] * 60;
    //        diffTS = diffTS + startT[1];

    //        var diffTE = TTime[0] * 60;
    //        diffTE = diffTE + TTime[1];

    //        if (diffTE > diffTS) {
    //            swal("Start Date Time Can not be less than Today Date Time", {
    //                icon: "info"
    //            })
    //            $("#StartDate").val('');
    //            $("#EndDate").val('');
    //        }

    //    }

    //}

    //if (EndDate != "") {
    //    var end = EndDate;
    //    end = end.split("/");
    //    end = end[1] + "/" + end[0] + "/" + end[2];

    //    Today = new Date(Today);
    //    end = new Date(end);
    //    var diff = new Date(end - Today);
    //    var days = diff / 1000 / 60 / 60 / 24;

    //    if (days < 0) {
    //        swal("End Date Can not be less than Today Date", {
    //            icon: "info"
    //        })
    //        $("#StartDate").val('');
    //        $("#EndDate").val('');
    //    }

    //    if (days == 0) {

    //        var endT = EndTime;
    //        endT = endT.split(":");

    //        var TTime = TodayTime;
    //        TTime = TTime.split(":");

    //        var diffTS = endT[0] * 60;
    //        diffTS = diffTS + endT[1];

    //        var diffTE = TTime[0] * 60;
    //        diffTE = diffTE + TTime[1];

    //        if (diffTE > diffTS) {
    //            swal("End Date Time Can not be less than Today Date Time", {
    //                icon: "info"
    //            })
    //            $("#StartDate").val('');
    //            $("#EndDate").val('');
    //        }

    //    }

    //}

    if (StartDate != "" && EndDate != "") {
        var start = StartDate;
        start = start.split("/");
        start = start[1] + "/" + start[0] + "/" + start[2];

        var end = EndDate;
        end = end.split("/");
        end = end[1] + "/" + end[0] + "/" + end[2];

        end = new Date(end);
        start = new Date(start);
        var diff = new Date(end - start);
        var days = diff / 1000 / 60 / 60 / 24;

        if (days < 0) {
            swal("End Date should be greater than or equal to Start Date", {
                icon: "info"
            })
            //$("#StartDate").val('');
            $("#EndDate").val('');
            $("#EndDate").trigger('click');
        }

        if (days == 0) {

            var startT = StartTime.split(" ")[0];
            startT = startT.split(":");
            var hours = startT[0];
            hours = hours % 12;

            var endT = EndTime.split(" ")[0];
            endT = endT.split(":");
            var Endhours = endT[0];
            Endhours = Endhours % 12;

            var diffTS = hours * 60;
            diffTS = diffTS + startT[1];

            var diffTE = Endhours * 60;
            diffTE = diffTE + endT[1];

            if (diffTE < diffTS) {
                swal("End Date Time should be greater than or equal to Start Date Time", {
                    icon: "info"
                })
                //$("#StartDate").val('');
                $("#EndDate").val('');
                $("#EndDate").trigger('click');
            }

        }
    }

}

function NextFollowUpDate() {
    debugger;
    var StartDate = $("#NextFollowUpDate").val().substring(0, 10);

    var StartTime = $("#NextFollowUpDate").val().slice(-8);

    var CurentDateTime = new Date();
    var TodayTime = CurentDateTime.getHours().toString().padStart(2, '0') + ":" + CurentDateTime.getMinutes().toString().padStart(2, '0');

    const yyyy = CurentDateTime.getFullYear().toString().padStart(2, '0');
    let mm = CurentDateTime.getMonth() + 1; // Months start at 0!
    let dd = CurentDateTime.getDate();

    if (dd < 10) dd = '0' + dd;
    if (mm < 10) mm = '0' + mm;

    var TodayDate = dd + "/" + mm + "/" + yyyy;

    var Today = TodayDate;
    Today = Today.split("/");
    Today = Today[1] + "/" + Today[0] + "/" + Today[2];

    if (StartDate != "") {

        var start = StartDate;
        start = start.split("/");
        start = start[1] + "/" + start[0] + "/" + start[2];

        Today = new Date(Today);
        start = new Date(start);
        var diff = new Date(start - Today);
        var days = diff / 1000 / 60 / 60 / 24;

        if (days < 0) {
            swal("Next Follow-Up Date should be greater than or equal to Today Date", {
                icon: "info"
            });
            $("#NextFollowUpDate").val('');
        }

        if (days == 0) {
            var startT = StartTime.split(" ")[0];
            startT = startT.split(":");

            var TTime = TodayTime;
            TTime = TTime.split(":");
            var hours = TTime[0];
            hours = hours % 12;

            var diffTS = startT[0] * 60;
            diffTS = diffTS + startT[1];

            var diffTE = hours * 60;
            diffTE = diffTE + TTime[1];

            if (diffTE > diffTS) {
                swal("Next Follow-Up Date Time should be greater than or equal to Today Date Time", {
                    icon: "info"
                });
                $("#NextFollowUpDate").val('');
                $("#NextFollowUpDate").trigger('click');
            }

        }

    }


}

function NextSiteVisitPlanDate() {
    var StartDate = $("#NextSiteVisitPlan").val().substring(0, 10);

    var StartTime = $("#NextSiteVisitPlan").val().slice(-8);

    var CurentDateTime = new Date();
    var TodayTime = CurentDateTime.getHours().toString().padStart(2, '0') + ":" + CurentDateTime.getMinutes().toString().padStart(2, '0');

    const yyyy = CurentDateTime.getFullYear().toString().padStart(2, '0');
    let mm = CurentDateTime.getMonth() + 1; // Months start at 0!
    let dd = CurentDateTime.getDate();

    if (dd < 10) dd = '0' + dd;
    if (mm < 10) mm = '0' + mm;

    var TodayDate = dd + "/" + mm + "/" + yyyy;

    var Today = TodayDate;
    Today = Today.split("/");
    Today = Today[1] + "/" + Today[0] + "/" + Today[2];

    if (StartDate != "") {

        var start = StartDate;
        start = start.split("/");
        start = start[1] + "/" + start[0] + "/" + start[2];

        Today = new Date(Today);
        start = new Date(start);
        var diff = new Date(start - Today);
        var days = diff / 1000 / 60 / 60 / 24;

        if (days < 0) {
            swal("Next Site Visit Plan Date should be greater than or equal to Today Date", {
                icon: "info"
            });
            $("#NextSiteVisitPlan").val('');
        }

        if (days == 0) {
            var startT = StartTime.split(" ")[0];
            startT = startT.split(":");           

            var TTime = TodayTime;
            TTime = TTime.split(":");
            var hours = TTime[0];
            hours = hours % 12;

            var diffTS = startT[0] * 60;
            diffTS = diffTS + startT[1];

            var diffTE = hours * 60;
            diffTE = diffTE + TTime[1];

            if (diffTE > diffTS) {
                swal("Next Site Visit Plan Date Time should be greater than or equal to Today Date Time", {
                    icon: "info"
                });
                $("#NextSiteVisitPlan").val('');
                $("#NextSiteVisitPlan").trigger('click');
            }

        }

    }


}