var dataRowEmployee = [];
var objAssignedType;
var EmployeeselectedIds = [];
var IsBookMarked;
var IsQueryStringExist = 0;
var SellerMobileNoIsEdit = 0;
var BuyerMobileNoIsEdit = 0;
var ValidationChecked = 0;
$(document).ready(function () {
    $(".preloader").fadeIn("slow");
    var temPI = parseInt($("#PI_hdn").val());
    if (isNaN(temPI)) {
        temPI = 0;
    }
    var temPS = parseInt($("#PS_hdn").val());
    if (isNaN(temPS)) {
        temPS = 10;
    }
    var temViewType = parseInt($("#ViewType_hdn").val());
    if (isNaN(temViewType)) {
        temViewType = 1;
    }

    $("#pageIndex").val(temPI);
    $("#NewpageSize").val(temPS);
    $(".preloader").fadeIn("slow");
    if (temViewType == 2) {
        $("#view-grid").removeClass("Opened");
        $("#view-list").addClass("Opened");
        $("#view-grid").removeClass('active show');
        $("#view-list").addClass('active show');
    }
    else {
        $("#view-list").removeClass("Opened");
        $("#view-grid").addClass("Opened");
        $("#view-list").removeClass('active show');
        $("#view-grid").addClass('active show');
    }
    fnBindSelectDropdowns();
    fnBindList();
    //fnSetEmployeeListData();
});
function fnChangeView(Type) {
    $(".preloader").fadeIn("slow");
    if (Type == 2) {
        $("#view-list").addClass("Opened");
        $("#view-grid").removeClass("Opened");
    }
    else {
        $("#view-grid").addClass("Opened");
        $("#view-list").removeClass("Opened");
    }
    fnBindList();
}
function fnBindList() {
    var Html = "";
    var PagingHtml = '';
    var IsGrid = 1;
    if ($("#view-list").hasClass("Opened")) {
        IsGrid = 0;
    }

    var FromToDate = $("#FromToDate").val().split('-');
    var FromDate = FromToDate[0];
    var ToDate = FromToDate[1];
    var PageRec = 0;
    if ($("#pageIndex").val() > 0) {
        PageRec = $("#pageIndex").val() - 1;
    }

    $.ajax({
        type: "POST",
        url: "/Transactions/Opportunity/GetList",
        async: true,
        cache: false,
        data: {
            order: $("#order").val(),
            orderDir: $("#orderDir").val(),
            startRec: PageRec,
            pageSize: $("#NewpageSize").val(),
            Search: $("#WildSearch").val(),
            FromDate: FromDate,
            ToDate: ToDate,
            AssignTypeId: $("#AssignTypeId").val(),
            MainAssignToIds: $("#MainAssignToIds").val(),
            IsBookMarked: $("#IsBookMarked").val(),
            //RelevanceIds: $("#RelevanceIds").val().join(","),
            StatusIds: $("#LeadStageIdUI").val().join(","),
            CategoryIds: $("#PropertyCategoryIdsUI").val().join(","),
            SubcategoryIds: $("#PropertySubcategoryIdsUI").val().join(","),
            BHKIds: $("#BHKIdsUI").val().join(","),
            FloorIds: $("#FloorIdsUI").val().join(","),
            LeadZoneIds: $("#PropertyZoneIdUI").val().join(","),
            FurnishStatusIds: $("#FurnishStatusIdsUI").val().join(","),
            CityIds: $("#CityIdUI").val().join(","),
            AreaIds: $("#AreaIdUI").val().join(","),
            PriceFrom: $("#PriceFrom").val(),
            PriceTo: $("#PriceTo").val(),
            TotalAreaFrom: $("#TotalAreaFrom").val(),
            TotalAreaTo: $("#TotalAreaTo").val(),
            AssignToIds: $("#AssignToUI").val().join(","),
        },
        success: function (data) {
            $("#FiltersTotalCount").html(data.recordsTotal);
            $("#view-grid").html('');
            $("#view-listtBody").html('');
            $("#position_hdn").val(500);
            if (data.data != undefined) {

                if (data.data.length > 0) {
                    var IsScroll = 0;
                    for (var i = 0; i < data.data.length; i++) {
                        var vals = data.data[i];
                        var BudgetRangeval = vals.BudgetRange;
                        var ProjectRequirementval = vals.ProjectRequirement;
                        var CarpetAreaval = vals.CarpetArea;
                        var BudgetRangearr = vals.BudgetRange.split("-");
                        BudgetRangearr[0] = fnAmtConvertInShort(BudgetRangearr[0]);
                        BudgetRangearr[1] = fnAmtConvertInShort(BudgetRangearr[1]);
                        BudgetRangeval = BudgetRangearr.join(" - ");

                        var ProjectRequirementarr = vals.ProjectRequirement.split("-");
                        ProjectRequirementarr[0] = AmountWithComma(ProjectRequirementarr[0]);
                        ProjectRequirementarr[1] = AmountWithComma(ProjectRequirementarr[1]);
                        ProjectRequirementval = ProjectRequirementarr.join(" - ");

                        var CarpetAreaarr = vals.CarpetArea.split("-");
                        CarpetAreaarr[0] = AmountWithComma(CarpetAreaarr[0]);
                        CarpetAreaarr[1] = AmountWithComma(CarpetAreaarr[1]);
                        CarpetAreaval = CarpetAreaarr.join(" - ");
                        var PI = $("#pageIndex").val();
                        var PS = $("#NewpageSize").val();
                        var PoVal = parseInt($("#position_hdn").val());
                        if (isNaN(PoVal)) {
                            PoVal = 0;
                        }
                        var EditId = $("#EditId_hdn").val();
                        if (EditId == vals.EncryptedLeadId) {
                            IsScroll = 1;
                        }
                        debugger;
                        if (EditId != vals.EncryptedLeadId && IsScroll == 0 && EditId != "") {
                            if (i > 1) {
                                PoVal = PoVal + 420;
                            }
                        }
                        if (IsGrid) {
                            $("#position_hdn").val(PoVal);
                        } else {
                            $("#position_hdn").val(0);
                        }
                        if (IsGrid) {
                            Html += '<div class="card">';
                            Html += '<div class="card-body">';
                            Html += '    <div class="list-row">';
                            Html += '       <div class="listcol-full">';
                            Html += '           <div class="list-action">';
                            Html += '                <div class="custom-control custom-checkbox">';
                            Html += '                    <input type="checkbox" class="custom-control-input" id="ListCheck_' + vals.Id + '">';
                            Html += '                    <label class="custom-control-label" for="ListCheck_' + vals.Id + '"></label>';
                            Html += '                </div>';
                            if (vals.IsBookmarked == 1) {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkOpportunity(' + vals.Id + ',0)" style="cursor:pointer"><i class="fas fa-star zoom" aria-hidden="true"></i></a></div>';
                            }
                            else {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkOpportunity(' + vals.Id + ',1)"><i class="far fa-star zoom"></i></a></div>';
                            }
                            if (vals.StageName == "Close Won") {
                                Html += '                <div class="list-li diamond"><i class="far fa-trophy"></i></div>';
                            }
                            Html += '            </div>';
                            Html += '            <div class="list-info">';
                            Html += '                <div class="list-id">Opportunity ID: ' + vals.OpportunityNo + '</div>';
                            Html += '                <div class="list-postdate mb-2"><span>Lead Id: ' + vals.LeadNo + '</span><span class="pl-3">Lead Date: ' + vals.LeadDate + '</span><span class="pl-3">Qualify Date: ' + vals.OpportunityDate + ' </span></div>';
                            Html += '                <h1 class="list-title mb-3">' + vals.LeadDetail + ' </h1>';
                            Html += '                <div class="row">';
                            Html += '                    <div class="col-xl-4 col-lg-4 col-md-4">';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Preferred Projects:</label>';
                            if (vals.PreferredProjects == "") {
                                Html += '                            <label class="list-features-text assignto-label"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text assignto-label">' + vals.PreferredProjects + '</label>';
                            }
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Preferred Area:</label>';
                            if (vals.PreferredArea == "") {
                                Html += '                            <label class="list-features-text"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text">' + vals.PreferredArea + '</label>';
                            }
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Budget Range:</label>';
                            if (vals.BudgetRange == "0.00-0.00") {
                                Html += '                            <label class="list-features-text price-label"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text price-label">₹' + BudgetRangeval + '</label>';
                            }
                            Html += '                        </div>';
                            Html += '                    </div>';
                            Html += '                    <div class="col-xl-4 col-lg-4 col-md-4">';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Super Buildup Area:</label>';
                            if (vals.ProjectRequirement == "0.00-0.00") {
                                Html += '                            <label class="list-features-text"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text">' + ProjectRequirementval + ' ' + vals.SuperBuildupAreaUnitName + '</label>';
                            }
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Carpet Area:</label>';
                            if (vals.CarpetArea == "0.00-0.00") {
                                Html += '                            <label class="list-features-text"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text">' + CarpetAreaval + ' ' + vals.CarpetAreaUnitName + '</label>';
                            }
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Status:</label>';
                            Html += '                            <label class="list-features-text" style="width: 150px;">';

                            if (vals.StageName == "Close Won") {
                                var ddLeadStatus = '';
                                if (vals.StageName == "Close Won" || vals.StageName == "Close Lost") {
                                    ddLeadStatus += '<select class="form-control OpportunityStageId Close_Won" id="OpportunityStageId_' + vals.Id + '" selectedid="' + vals.OpportunityStageId + '" selectedLeadId="' + vals.LeadId + '" disabled>';
                                    ddLeadStatus += '<option value="' + vals.OpportunityStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                else {
                                    ddLeadStatus += '<select class="form-control OpportunityStageId Close_Won" id="OpportunityStageId_' + vals.Id + '" selectedid="' + vals.OpportunityStageId + '" selectedLeadId="' + vals.LeadId + '" FullName="' + vals.FullName + '" MobileNo="' + vals.MobileNo + '" Email="' + vals.Email + '">';
                                    ddLeadStatus += '<option value="' + vals.OpportunityStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                Html += ddLeadStatus;
                            } else if (vals.StageName == "Close Lost") {
                                var ddLeadStatus = '';
                                if (vals.StageName == "Close Won" || vals.StageName == "Close Lost") {
                                    ddLeadStatus += '<select class="form-control OpportunityStageId Close_Lost" id="OpportunityStageId_' + vals.Id + '" selectedid="' + vals.OpportunityStageId + '" selectedLeadId="' + vals.LeadId + '" disabled>';
                                    ddLeadStatus += '<option value="' + vals.OpportunityStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                else {
                                    ddLeadStatus += '<select class="form-control OpportunityStageId Close_Lost" id="OpportunityStageId_' + vals.Id + '" selectedid="' + vals.OpportunityStageId + '" selectedLeadId="' + vals.LeadId + '" FullName="' + vals.FullName + '" MobileNo="' + vals.MobileNo + '" Email="' + vals.Email + '">';
                                    ddLeadStatus += '<option value="' + vals.OpportunityStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                Html += ddLeadStatus;
                            } else {
                                var ddLeadStatus = '';
                                if (vals.StageName == "Close Won" || vals.StageName == "Close Lost") {
                                    ddLeadStatus += '<select class="form-control OpportunityStageId" id="OpportunityStageId_' + vals.Id + '" selectedid="' + vals.OpportunityStageId + '" selectedLeadId="' + vals.LeadId + '" disabled>';
                                    ddLeadStatus += '<option value="' + vals.OpportunityStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                else {
                                    ddLeadStatus += '<select class="form-control OpportunityStageId" id="OpportunityStageId_' + vals.Id + '" selectedid="' + vals.OpportunityStageId + '" selectedLeadId="' + vals.LeadId + '" FullName="' + vals.FullName + '" MobileNo="' + vals.MobileNo + '" Email="' + vals.Email + '">';
                                    ddLeadStatus += '<option value="' + vals.OpportunityStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                Html += ddLeadStatus;
                            }

                            Html += '                            </label>';
                            Html += '                        </div>';
                            if (vals.StageName == "Close Lost") {
                                Html += '                        <div class="features-list">';
                                Html += '                            <label class="list-features-label">Lost reason:</label>';
                                if (vals.LostReasonName == "") {
                                    Html += '                            <label class="list-features-text"><i>Not Defined</i></label>';
                                } else {
                                    Html += '                            <label class="list-features-text">' + vals.LostReasonName + '</label>';
                                }
                                Html += '                        </div>';
                            }
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Lead Source:</label>';
                            if (vals.LeadSourceName == "") {
                                Html += '                            <label class="list-features-text"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text">' + vals.LeadSourceName + '</label>';
                            }
                            Html += '                        </div>';
                            Html += '                    </div>';
                            Html += '                    <div class="col-xl-4 col-lg-4 col-md-4">';
                            Html += '                        <div class="features-list">';
                            /*         Html += '                            <label class="list-features-label">Followup Taken:</label>';*/
                            Html += '                            <label class="list-features-text">';
                            Html += '                                <a href="javascript:;" onclick="fnOpenActivity(\'' + vals.EncryptedId + '\',' + vals.OpportunityStageId + ',' + vals.LeadId + ',1)" class="list-link">Follow-up Taken:' + vals.FollowupTaken + '</a>';
                            Html += '                            </label>';
                            Html += '                            <span class="hint-text d-block"> ' + vals.FollowupDescription + '</span>';
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Next Follow-up Date:</label>';
                            if (vals.NextFollowupDate == "") {
                                Html += '                            <label class="list-features-text"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text">' + vals.NextFollowupDate + '</label>';
                            }

                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-text">';
                            Html += '                                <a href="javascript:;" onclick="fnOpenActivity(\'' + vals.EncryptedId + '\',' + vals.OpportunityStageId + ',' + vals.LeadId + ',4)" class="list-link">Site Visit Done (' + vals.SiteVisitDone + ')</a>';
                            Html += '                            </label>';
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-text">';
                            Html += '                                <a href="javascript:;" onclick="fnShowPropertyDetailPopup(' + vals.LeadId + ',' + vals.MappedProperty + ')" class="list-link">Property Details (' + vals.MappedProperty + ')</a>';
                            Html += '                            </label>';
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Assign To:</label>';
                            Html += '                            <label class="list-features-text" style="width: 150px;">';
                            var ddEmployee = '';
                            ddEmployee += '<select class="form-control AssignedToId" id="AssignedToId_' + vals.Id + '">';
                            ddEmployee += '<option value="' + vals.AssignTo + '">';
                            ddEmployee += vals.AssignToName;
                            ddEmployee += '</option>';
                            ddEmployee += '</select>';
                            Html += ddEmployee;
                            Html += '                            </label>';
                            Html += '                        </div>';
                            Html += '                    </div>';
                            Html += '                </div>';
                            Html += '                <div class="row">';
                            //Html += '                    <div class="col-xl-4">';
                            //Html += '                        <div class="box-hint-desc">';
                            //Html += '                           <p>Internal Desc. : ' + vals.InternalDesc + '</p>';
                            //Html += '                           <p>External Desc. : ' + vals.ExternalDesc + '</p>';
                            //Html += '                        </div>';
                            //Html += '                    </div>';
                            Html += '                    <div class="col-xl-4">';
                            Html += '                    </div>';
                            Html += '                    <div class="col-xl-4">';
                            Html += '                        <div class="form-group mb-0">';
                            Html += '                           <input type="text" class="form-control" placeholder="Enter Special Notes" value="' + vals.SpecialNote + '" onfocusout="fnChangeSpecialNote(' + vals.Id + ', this)">';
                            Html += '                        </div>';
                            Html += '                    </div>';
                            Html += '                    <div class="col-xl-4">';
                            Html += '                        <div class="owner-box">';
                            Html += '                            <div class="features-list mb-0">';
                            Html += '                                <label class="list-features-text">' + vals.FullName + '</label>';
                            Html += '                                <div class="list-features-text ml-2">';
                            Html += '                                    <ul class="list-social">';

                            if ($("#RightsView").val() == "true" || $("#RightsView").val() == true) {
                                Html += '                                   <li>';
                                Html += '                                    <a title="View Opportunity" href="/Transactions/Opportunity/View_New?EncryptedId=' + vals.EncryptedLeadId + '&PI=' + PI + '&PS=' + PS + '&ViewType=1" class="list-social-link"><i class="far fa-eye"></i></a>';
                                Html += '                                   </li>';
                            }
                            if ($("#RightsEdit").val() == "true" || $("#RightsEdit").val() == true) {
                                Html += '                                   <li>';
                                Html += '                                    <a target="_blank" title="Edit Opportunity" href="/Transactions/Lead/Form?mode=edit&EncryptedId=' + vals.EncryptedLeadId + '&PI=' + PI + '&PS=' + PS + '&ViewType=1&IsOpprtunity=1" class="list-social-link"><i class="far fa-edit"></i></a>';
                                Html += '                                   </li>';
                            }
                            Html += '                                    </ul>';
                            Html += '                                </div>';
                            Html += '                            </div>';
                            Html += '                            <div class="ownercontact">' + vals.MobileNo + ' | ' + vals.Email + '</div>';
                            Html += '                        </div>';
                            Html += '                    </div>';
                            Html += '                </div>';
                            Html += '            </div>';
                            Html += '        </div>';
                            Html += '    </div>';
                            Html += '</div>';
                            Html += '</div>';
                            Html += '</div>';

                            $("#view-grid").append(Html);
                            Html = '';
                        }
                        else {

                            Html += '<tr>';
                            Html += '<td class="text-center">';
                            Html += '     <div class="custom-control custom-checkbox">';
                            Html += '         <input type="checkbox" class="custom-control-input" id="ListCheck_' + vals.Id + '">';
                            Html += '         <label class="custom-control-label" for="ListCheck_' + vals.Id + '"></label>';
                            Html += '     </div>';
                            Html += ' </td>';
                            Html += ' <td class="text-center">';
                            if (vals.IsBookmarked == 1) {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkOpportunity(' + vals.Id + ',0)" style="cursor:pointer"><i class="fas fa-star zoom" aria-hidden="true"></i></a></div>';
                            }
                            else {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkOpportunity(' + vals.Id + ',1)"><i class="far fa-star zoom"></i></a></div>';
                            }
                            Html += ' </td>';
                            Html += ' <td class="text-center">' + vals.RowNo + '</td>';
                            Html += ' <td>' + vals.OpportunityNo + '</td>';
                            Html += ' <td class="whitespace-rap">' + vals.OpportunityDate + ' ' + vals.Days + ' Days Ago</td>';
                            Html += '<td>' + vals.LeadNo + '</td>';
                            Html += '<td>' + vals.LeadDate + '</td>';
                            Html += ' <td class="whitespace-rap">' + vals.AssignToName + '</td>';
                            Html += ' <td>';
                            Html += '     <div class="tb-width-300">' + vals.SpecialNote + '</div>';
                            Html += ' </td>';
                            Html += ' <td class="whitespace-rap">' + vals.FullName + '</td>';
                            Html += ' <td>' + vals.MobileNo + '</td>';
                            Html += '<td>' + vals.NextFollowupDate + '</td>';
                            Html += '<td>' + vals.FollowupDate + '</td>';
                            Html += '<td>' + vals.NextSiteVisitPlan + '</td>';
                            Html += '<td class="text-center">';
                            Html += '    <a href="javascript:;" onclick="fnShowPropertyDetailPopup(' + vals.LeadId + ',' + vals.MappedProperty + ')" class="list-link">Properties (' + vals.MappedProperty + ')</a>';
                            Html += '</td>';
                            Html += ' <td class="text-center whitespace-rap">';
                            if ($("#RightsView").val() == "true" || $("#RightsView").val() == true) {
                                Html += '<a title="View Opportunity" href="/Transactions/Opportunity/View_New?EncryptedId=' + vals.EncryptedLeadId + '&PI=' + PI + '&PS=' + PS + '&ViewType=2" class="btn btn-success btn-sm mr-1" data-toggle="tooltip" data-placement="top" title="View Lead" data-original-title="View"><i class="fas fa-eye"></i></a>';
                            }
                            if ($("#RightsEdit").val() == "true" || $("#RightsEdit").val() == true) {
                                Html += '<a target="_blank" href="/Transactions/Lead/Form?mode=edit&EncryptedId=' + vals.EncryptedLeadId + '&PI=' + PI + '&PS=' + PS + '&ViewType=2&IsOpprtunity=1" class="btn btn-primary btn-sm" data-toggle="tooltip" data-placement="top" title="Edit Opportunity" data-original-title="View"><i class="fas fa-edit"></i></a>';
                            }
                            Html += ' </td>';
                            Html += ' </tr>';

                            $("#view-listtBody").append(Html);
                            Html = '';
                        }
                        var Position = parseInt($("#position_hdn").val());
                        if (isNaN(Position)) {
                            Position = 0;
                        }
                        if (Position > 500) {
                            $("html, body").animate({ scrollTop: Position });
                        }
                    }

                    //================== paging Start =========================//

                    //PagingHtml += '<ul class="pagination justify-content-center">';
                    //var recordsTotal = data.recordsTotal;
                    //let pageSize = parseInt($("#pageSize").val());
                    //let pageIndex = parseInt($("#pageIndex").val());
                    //var numPages = Math.ceil(recordsTotal / pageSize);

                    //if (pageIndex == 0) {
                    //    PagingHtml += '<li class="page-item"><a style="pointer-events: none;cursor: pointer;" class="page-link" onclick="fnSetPageIndex(' + (pageIndex - 1) + ')">Previous</a></li>';
                    //}
                    //else {
                    //    PagingHtml += '<li class="page-item"><a class="page-link" style="cursor: pointer;" onclick="fnSetPageIndex(' + (pageIndex - 1) + ')">Previous</a></li>';
                    //}

                    //for (i = 0; i < numPages; i++) {
                    //    var pageNum = i + 1;
                    //    if (i == pageIndex) {
                    //        PagingHtml += '<li class="page-item active"><a class="page-link" style="cursor: pointer;" onclick="fnSetPageIndex(' + i + ')">' + pageNum + '</a></li>';
                    //    }
                    //    else {
                    //        PagingHtml += '<li class="page-item"><a class="page-link" style="cursor: pointer;" onclick="fnSetPageIndex(' + i + ')">' + pageNum + '</a></li>';
                    //    }
                    //}
                    //if (pageIndex >= (numPages - 1)) {
                    //    PagingHtml += '<li class="page-item"><a style="pointer-events: none;cursor: pointer;" class="page-link" onclick="fnSetPageIndex(' + (pageIndex + 1) + ')">Next</a></li>';
                    //}
                    //else {
                    //    PagingHtml += '<li class="page-item"><a class="page-link" style="cursor: pointer;" onclick="fnSetPageIndex(' + (pageIndex + 1) + ')">Next</a></li>';
                    //}

                    //PagingHtml += '</ul>';

                    PagingHtml += '<div id="NewPagination" style="text-align: center;"></div>';

                    //================== paging END =========================//
                }
                else {
                    Html = '';

                    if (IsGrid) {

                        Html += '<div class="card">';
                        Html += '<div class="card-body">';
                        Html += '    <div class="no-data-box">';
                        Html += '        <img class="img-fluid" src="/adminpanel/images/no-data.png" alt="No Data" />';
                        Html += '        <h2 class="no-data-title">No Data Found</h2>';
                        Html += '        <p class="no-data-text">We are unable to find the data that you are looking for</p>';
                        Html += '    </div>';
                        Html += '</div>';
                        Html += '</div>';

                        $("#view-grid").html(Html);
                    }
                    else {
                        Html += '<tr id="noRecordTR">';
                        Html += '    <td colspan="17" style="text-align: center;">No Record Found</td>';
                        Html += '</tr>';
                        $("#view-listtBody").html(Html);
                    }
                }

                $("#listdivPaging").html(PagingHtml);
    
                var recordsTotal = data.recordsTotal;
                let pageIndex = parseInt($("#pageIndex").val());

                var myPagination = new Pagination({

                    container: $("#NewPagination"),

                    pageClickCallback: function (e) {

                        fnSetPageIndex(e)
                    },

                    callPageClickCallbackOnInit: false,

                    maxVisibleElements: 20,

                    showInput: false,

                    enhancedMode: false
                });
                var PgSize = $("#NewpageSize").val();
                myPagination.make(recordsTotal, PgSize, pageIndex);
                //myPagination.getPageCount();
                //myPagination.getCurrentPage();
            }
            else {
                Html = '';

                if (IsGrid) {

                    Html += '<div class="card">';
                    Html += '<div class="card-body">';
                    Html += '    <div class="no-data-box">';
                    Html += '        <img class="img-fluid" src="/adminpanel/images/no-data.png" alt="No Data" />';
                    Html += '        <h2 class="no-data-title">No Data Found</h2>';
                    Html += '        <p class="no-data-text">We are unable to find the data that you are looking for</p>';
                    Html += '    </div>';
                    Html += '</div>';
                    Html += '</div>';

                    $("#view-grid").html(Html);
                }
                else {
                    Html += '<tr id="noRecordTR">';
                    Html += '    <td colspan="17" style="text-align: center;">No Record Found</td>';
                    Html += '</tr>';
                    $("#view-listtBody").html(Html);
                }
                $("#listdivPaging").html('');
            }
            //setTimeout(function () {
            fnSetDropdown();
            //}, 100);
        }
    });

    function fnSetDropdown() {
        $('.OpportunityStageId').select2({
            ajax: {
                url: '/Common/GetOpportunityStatusSearchList',
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
            placeholder: 'Select Opportunity Status',
            minimumInputLength: minimumInputLength,
            width: '100%',
            allowClear: false,
        });
        $('.AssignedToId').select2({
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
                        Department: 'Sales,Admin'
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
            placeholder: 'Select Assign To',
            minimumInputLength: minimumInputLength,
            width: '100%',
            allowClear: false
        });
    };

    setTimeout(function () {
        $(".preloader").hide();
    }, 5000);
}

//=========================================== Filters Start =========================================//

var typingTimer;
var doneTypingInterval = 1000;

$("#WildSearch").on('keyup', function (event) {
    clearTimeout(typingTimer);
    typingTimer = setTimeout(function () {
        event.preventDefault();
        fnBindList();
    }, doneTypingInterval);
});
$("#WildSearch").on('keydown', function () {
    clearTimeout(typingTimer);
});

$("#search,#PriceFrom,#PriceTo,#TotalAreaFrom,#TotalAreaTo").on('keyup', function (event) {
    event.preventDefault();
    //table.search(this.value).draw();
    $("#pageIndex").val(1);
    fnBindList();
});

$("#NewpageSize").on('change', function (event) {
    event.preventDefault();
    $("#pageIndex").val(1);
    fnBindList();
});

$("#FromToDate").on('change', function (event) {
    event.preventDefault();
     fnBindList();
});

$("#RelevanceIds,#LeadStageIdUI,#PropertyCategoryIdsUI" +
    ",#PropertySubcategoryIdsUI,#BHKIdsUI, #FloorIdsUI, #PropertyZoneIdUI,#FurnishStatusIdsUI" +
    ", #CityIdUI, #AreaIdUI, #AssignToUI").on('select2:unselect select2:select', function (event) {
        event.preventDefault();
        $("#pageIndex").val(1);
        fnBindList();
    });

function fnSetPageIndex(Index) {
    $("#pageIndex").val(Index);
    fnBindList();
}

function fnChangeAssignedType(event, Type) {
    event.preventDefault();
    $('#AssignTypeId').val(Type);

    if (Type == 1) {
        $('#btnAssigned').addClass('activeButton');
        $('#btnAssignee').removeClass('activeButton');
        $('#btnTeam').removeClass('activeButton');
        $('#btnAll').removeClass('activeButton');
    } else if (Type == 2) {
        $('#btnAssigned').removeClass('activeButton');
        $('#btnAssignee').addClass('activeButton');
        $('#btnTeam').removeClass('activeButton');
        $('#btnAll').removeClass('activeButton');
    } else if (Type == 3) {
        $('#btnAssigned').removeClass('activeButton');
        $('#btnAssignee').removeClass('activeButton');
        $('#btnTeam').addClass('activeButton');
        $('#btnAll').removeClass('activeButton');
        $('#MainAssignToIds').val('0');
        $('#teamModel').modal('show');

    } else if (Type == 4) {

        $('#btnAssigned').removeClass('activeButton');
        $('#btnAssignee').removeClass('activeButton');
        $('#btnTeam').removeClass('activeButton');
        $('#btnAll').addClass('activeButton');
    }

    MainAssignToIds = Type;
    fnBindList();

    if (Type != 3) {
        $('#MainAssignToIds').val('');
        $('.clsCheckBox').prop('checked', false);
    }
}

//=========================================== Team Popup Start =========================================//
$('.main-collapse[aria-expanded="true"]').click(function () {
    $('.collapse.show').collapse('hide');
});
$('#accordion-core-02 .panel-collapse').on('shown.collapse', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var $panel = $(this).closest('.panel-collapse');
    $('html,body').animate({ scrollTop: $panel.offset().top - 30 }, 500);
});
$('.main-collapse').click(function () {
    var pane = $(this);
    var $panel1 = pane.closest('.panel-collapse-top');
    $('html,body').animate({ scrollTop: $panel1.offset().top - 30 }, 500);
});
$(".structure-boxcollapse, .structure-tree-report-manager-box").click(function () {
    $('html, body').animate({
        scrollTop: $(".structure-tree-area.accordion").offset().top - 30
    }, 500);
});

function fnexpandteam(e) {
    var id = $(e).attr("data-target");

    if ($(e).hasClass('collapsed')) {
        $(e).removeClass("collapsed");
        $(e).attr("aria-expanded", true);
        $(id).addClass("show");
    }
    else {
        $(e).addClass("collapsed");
        $(e).attr("aria-expanded", false);
        $(id).removeClass("show");
    }
}
$("#btnApplyTeamFilter").click(function (event) {
    event.preventDefault();
    var MainAssignToIds = '';
    $('.clsCheckBox:checkbox:checked').each(function () {
        if ($(this).is(":checked")) {
            MainAssignToIds += $(this).prop('id').split('_')[1] + ',';
        }
    });
    if (!MainAssignToIds) {
        MainAssignToIds = "0";
    }
    $('#MainAssignToIds').val(MainAssignToIds);
    EmployeeselectedIds = MainAssignToIds.split(',');
    fnBindList();
    $('#teamModel').modal('hide');
});
//var arr = [];
$(".clsCheckBox").click(function (event) {
    var rowid = $(this).attr("row");
    var id = $(this).prop('id').split('_')[1];

    //$('.clsCheckBox:checkbox').each(function () {
    //    var dsid = $(this).prop('id').split('_')[1];
    //    $('#employeeCheck_' + dsid).prop("checked", false);
    //});

    if (rowid === "1") {
        //arr = [];
        $('.clsCheckBox:checkbox').each(function () {
            //arr.push($(this).prop('id').split('_')[1]);
            id = $(this).prop('id').split('_')[1];
            if ($(event.target).is(":checked"))
                $('#employeeCheck_' + id).prop("checked", true);
            else
                $('#employeeCheck_' + id).prop("checked", false);
        });
    }
    else {
        if ($(event.target).is(":checked"))
            $('#employeeCheck_' + id).prop("checked", true);
        else
            $('#employeeCheck_' + id).prop("checked", false);

        $('#report_' + rowid + ' .clsCheckBox:checkbox').each(function () {
            //arr.push($(this).prop('id').split('_')[1]);
            var tt = $(this).prop('id').split('_')[1];
            $(this).trigger('click');
        });
    }
});
//=========================================== Team Popup End =========================================//


function fnToggleBookmarkOff(event) {
    event.preventDefault();
    $("#btnBookmark").attr("hidden", true);
    $("#btnBookmark1").attr("hidden", false);
    $("#IsBookMarked").val(1);
    IsBookMarked = 1;
    fnBindList();
}

function fnToggleBookmarkOn(event) {
    event.preventDefault();
    $("#btnBookmark").attr("hidden", false);
    $("#btnBookmark1").attr("hidden", true);
    $("#IsBookMarked").val(0);
    IsBookMarked = 0;
    fnBindList();
}


function fnClearAdvanceFilter() {
    $('#search').val('');
    $('#PriceFrom').val('');
    $('#PriceTo').val('');
    $('#TotalAreaFrom').val('');
    $('#TotalAreaTo').val('');
    $('#RelevanceIds').empty().trigger('change');
    $('#LeadStageIdUI').empty().trigger('change');
    $('#PropertyCategoryIdsUI').empty().trigger('change');
    $('#PropertySubcategoryIdsUI').empty().trigger('change');
    $('#BHKIdsUI').val('').trigger('change');
    $('#FloorIdsUI').val('').trigger('change');
    $('#PropertyZoneIdUI').val('').trigger('change');
    $('#FurnishStatusIdsUI').val('').trigger('change');
    $('#CityIdUI').val('').trigger('change');
    $('#AreaIdUI').val('').trigger('change');
    $('#AssignToUI').val('').trigger('change');
    $("#pageIndex").val(1);
    fnBindList();
}

function fnBindSelectDropdowns() {
    $('#LeadStageIdUI').select2({
        ajax: {
            url: '/Common/GetOpportunityStatusSearchList',
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
        placeholder: 'Select Opportunity Status',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#PropertyCategoryIdsUI').select2({
        ajax: {
            url: '/Common/GetCategorySearchList',
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
        placeholder: 'Select Category',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#PropertySubcategoryIdsUI').select2({
        ajax: {
            url: '/Common/GetPropertySubcategoryMultiSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    CategoryIds: $("#PropertyCategoryIdsUI").val().join(","),
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
        placeholder: 'Select Subcategory',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#BHKIdsUI').select2({
        ajax: {
            url: '/Common/GetPropertyBHKSearchList',
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
        placeholder: 'Select Residential Structures',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#FloorIdsUI').select2({
        ajax: {
            url: '/Common/GetFloorSearchList',
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
        placeholder: 'Select Floor',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#PropertyZoneIdUI').select2({
        ajax: {
            url: '/Common/GetPropertyZoneSearchList',
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
        placeholder: 'Select Housing Segments',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#FurnishStatusIdsUI').select2({
        ajax: {
            url: '/Common/GetPropertyFurnishStatusSearchList',
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
        placeholder: 'Select Furnished Status',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#CityIdUI').select2({
        ajax: {
            url: '/Common/GetCitySearchList',
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
        placeholder: 'Select City',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#AreaIdUI').select2({
        ajax: {
            url: '/Common/GetAreamultiSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    //CountryId: isNaN(parseInt($(CountryIdUI).val())) ? 0 : $(CountryIdUI).val(),
                    //StateId: isNaN(parseInt($(StateIdUI).val())) ? 0 : $(StateIdUI).val(),
                    CityId: $("#CityIdUI").val().join(",")
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
        placeholder: 'Select Area',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#AssignToUI').select2({
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
        placeholder: 'Select Assign To',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#OppAssignedToId').select2({
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
                    Department: 'Admin,Super Admin'
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
        placeholder: 'Select Assign To',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });
    $('#OppLostReasonId').select2({
        ajax: {
            url: '/Common/GetLeadLostReasonSearchList',
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
        placeholder: 'Select Lost Reason',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });
    $('#WonPropertyId').select2({
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
                    LeadId: $("#QualifyLeadId_hdn").val(),
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

    $('#SellerCountry').select2({
        ajax: {
            url: "/Common/GetCountrySearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
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
        placeholder: 'Select Country',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });
    $('#SellerState').select2({
        ajax: {
            url: "/Common/GetStateSearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    CountryId: isNaN(parseInt($('#SellerCountry').val())) ? 0 : $('#SellerCountry').val()
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
        placeholder: 'Select State',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#SellerCity').select2({
        ajax: {
            url: "/Common/GetCitySearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    CountryId: isNaN(parseInt($('#SellerCountry').val())) ? 0 : $('#SellerCountry').val(),
                    StateId: isNaN(parseInt($('#SellerState').val())) ? 0 : $('#SellerState').val()
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
        placeholder: 'Select City',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#SellerArea').select2({
        ajax: {
            url: "/Common/GetAreaSearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    CountryId: isNaN(parseInt($('#SellerCountry').val())) ? 0 : $('#SellerCountry').val(),
                    StateId: isNaN(parseInt($('#SellerState').val())) ? 0 : $('#SellerState').val(),
                    CityId: isNaN(parseInt($('#SellerCity').val())) ? 0 : $('#SellerCity').val()
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
        placeholder: 'Select Area',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: false
    });

    $('#SellerCountry').on('select2:unselect select2:select', function (e) {
        $('#SellerState').empty().trigger('change');
        $('#SellerCity').empty().trigger('change');
        $('#SellerArea').empty().trigger('change');
    });
    $('#SellerState').on('select2:unselect select2:select', function (e) {
        $('#SellerCity').empty().trigger('change');
        $('#SellerArea').empty().trigger('change');
    });
    $('#SellerCity').on('select2:unselect select2:select', function (e) {
        $('#SellerArea').empty().trigger('change');
    });

    $('#BuyerCountry').select2({
        ajax: {
            url: "/Common/GetCountrySearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
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
        placeholder: 'Select Country',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#BuyerState').select2({
        ajax: {
            url: "/Common/GetStateSearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    CountryId: isNaN(parseInt($('#BuyerCountry').val())) ? 0 : $('#BuyerCountry').val()
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
        placeholder: 'Select State',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#BuyerCity').select2({
        ajax: {
            url: "/Common/GetCitySearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    CountryId: isNaN(parseInt($('#BuyerCountry').val())) ? 0 : $('#BuyerCountry').val(),
                    StateId: isNaN(parseInt($('#BuyerState').val())) ? 0 : $('#BuyerState').val()
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
        placeholder: 'Select City',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#BuyerArea').select2({
        ajax: {
            url: "/Common/GetAreaSearchList",
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    CountryId: isNaN(parseInt($('#BuyerCountry').val())) ? 0 : $('#BuyerCountry').val(),
                    StateId: isNaN(parseInt($('#BuyerState').val())) ? 0 : $('#BuyerState').val(),
                    CityId: isNaN(parseInt($('#BuyerCity').val())) ? 0 : $('#BuyerCity').val()
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
        placeholder: 'Select Area',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: false
    });

    $('#BuyerCountry').on('select2:unselect select2:select', function (e) {
        $('#BuyerState').empty().trigger('change');
        $('#BuyerCity').empty().trigger('change');
        $('#BuyerArea').empty().trigger('change');
    });
    $('#BuyerState').on('select2:unselect select2:select', function (e) {
        $('#BuyerCity').empty().trigger('change');
        $('#BuyerArea').empty().trigger('change');
    });
    $('#BuyerCity').on('select2:unselect select2:select', function (e) {
        $('#BuyerArea').empty().trigger('change');
    });
}
//=========================================== Filters END =========================================//

//=========================================== For Update Fields Start =========================================//
//For Update Assign To
$(document).on('select2:select', '.AssignedToId', function () {
    var AssignedToId = !isNaN(parseInt($(this).val())) ? parseInt($(this).val()) : 0;
    var OpportunityId = $(this).attr("id").split("_")[1];
    var StatusId = $("#OpportunityStageId_" + OpportunityId).val();
    fnUpdateAssignedToId(AssignedToId, OpportunityId, StatusId);
});

function fnUpdateAssignedToId(AssignedToId, OpportunityId, StatusId) {
    $.ajax({
        type: "POST",
        url: "/Transactions/Opportunity/UpdateAssignedTo",
        data: {
            OpportunityId: OpportunityId,
            AssignedToId: AssignedToId,
            StatusId: StatusId
        },
        async: !1,
        cache: !1,
        success: function (data) {
            swal(data.Message, {
                icon: data.MessageType,
            });
            fnBindList();
        },
        error: function (response) {
            console.log(response);
        }
    });
}

//For Update Opportunity Status
$(document).on('select2:select', '.OpportunityStageId', function () {
    var OpportunityStageId = !isNaN(parseInt($(this).val())) ? parseInt($(this).val()) : 0;
    var OpportunityId = $(this).attr("id").split("_")[1];
    var OpportunityStageName = $("#OpportunityStageId_" + OpportunityId + " option:selected").text();
    var SelectedOpportunityStageId = $(this).attr("selectedid");
    var QualifyLeadId = $(this).attr("selectedLeadId");
    //var FullName = $(this).attr("FullName");
    //var MobileNo = $(this).attr("MobileNo");
    //var Email = $(this).attr("Email");
    $("#CustomerName").val('');
    $("#Email").val('');
    $("#MobileNo").val('');
    if (OpportunityStageName == "Close Won" || OpportunityStageName == "Close Lost") {
        $("#QualifyOpportunityId").val(OpportunityId);
        $("#QualifyOpportunityStageId").val(OpportunityStageId);
        $("#QualifySelectedOpportunityStageId").val(SelectedOpportunityStageId);
        $("#QualifyLeadId_hdn").val(QualifyLeadId);
        if (OpportunityStageName == "Close Won") {
            //$("#CustomerName").val(FullName);
            //$("#Email").val(Email);
            //$("#MobileNo").val(MobileNo);
            fnClearWonPropertyValue();

            $("#ShowWon").show();
            $(".ShowWon").show();
            $("#ShowClose").hide();

            $('#IsSellerBrokerInvolved').change(function () {
                if (this.checked) {
                    $(".SellerBrokerName").show();
                } else {
                    $(".SellerBrokerName").hide();
                }
            });
            $('#IsBuyerBrokerInvolved').change(function () {
                if (this.checked) {
                    $(".BuyerBrokerName").show();
                } else {
                    $(".BuyerBrokerName").hide();
                }
            });
        }
        if (OpportunityStageName == "Close Lost") {
            //$("#CustomerName").val('');
            //$("#Email").val('');
            //$("#MobileNo").val('');
            $("#ShowWon").hide();
            $(".ShowWon").hide();
            $("#ShowClose").show();
        }
        $("#QualifyConfirmationModal").modal('show');
    }
    else {
        fnUpdateOpportunityStatusId(OpportunityStageId, OpportunityId, OpportunityStageName, 0);
    }
});

var IsSellerMobileNoIsEdit = false;
var IsBuyerMobileNoIsEdit = false;

function fnConfirmationInsertEdit(MobileNo, Fld) {

    $.ajax({
        type: "GET",
        url: "/Transactions/Opportunity/IsDuplicateMobileNo",
        data: {
            MobileNo: MobileNo
        },
        async: true,
        success: function (data) {

            if (data.Value.CustomerId > 0) {
                if (Fld == "Seller") {
                    swal({
                        title: 'Seller Already Exist !',
                        text: 'This ' + data.Value.MobileNo + ' Mobile No Already Exist as a ' + data.Value.CustomerName + '. \n Still do you want to register new customer?',
                        icon: 'info',
                        buttons: true,
                        dangerMode: true,
                        buttons: ["No", "Yes"],
                    }).then((willCustomer) => {
                        if (willCustomer) {
                            SellerMobileNoIsEdit = data.Value.CustomerId;
                            fnConfirmationInsertEdit($("#BuyerMobileNo").val(), "Buyer");
                        } else {
                            SellerMobileNoIsEdit = 0;
                            fnConfirmationInsertEdit($("#BuyerMobileNo").val(), "Buyer");
                        }
                    });
                } else {
                    swal({
                        title: 'Buyer Already Exist !',
                        text: 'This ' + data.Value.MobileNo + ' Mobile No Already Exist as a ' + data.Value.CustomerName + '. \n Still do you want to register new customer?',
                        icon: 'info',
                        buttons: true,
                        dangerMode: true,
                        buttons: ["No", "Yes"],
                    }).then((willCustomer) => {
                        if (willCustomer) {
                            BuyerMobileNoIsEdit = data.Value.CustomerId;
                            ValidationChecked = 1;
                        } else {
                            BuyerMobileNoIsEdit = 0;
                            ValidationChecked = 1;
                        }
                    });
                }
            } else {
                if (Fld == "Seller") {
                    SellerMobileNoIsEdit = 0;
                    fnConfirmationInsertEdit($("#BuyerMobileNo").val(), "Buyer");
                    ValidationChecked = 1;
                } else {
                    BuyerMobileNoIsEdit = 0;
                    ValidationChecked = 1;
                }
            }
        },
        error: function (response) {
            console.log('Error:', response);
        }
    });
}

function fnOnChangeValidation() {
    $('#OppAssignedToId').change(function () {
        $("#OppAssignedToIdErrorMessage").text('');
        var AssignTo = $("#OppAssignedToId").val();
        if (!AssignTo) {
            $("#OppAssignedToIdErrorMessage").text("Please Select Assigned To.");
            return false;
        }
    });
    $('#WonPropertyId').change(function () {
        $("#WonPropertyIdErrorMessage").text('');
        var WonPropertyId = $("#WonPropertyId").val();
        if (!WonPropertyId) {
            $("#WonPropertyIdErrorMessage").text("Please Select Property.");
            return false;
        }
    });
    $('#OppRemarks').keyup(function () {
        $("#OppRemarksErrorMessage").text('');
        var OppRemarks = $("#OppRemarks").val();
        if (!OppRemarks) {
            $("#OppRemarksErrorMessage").text("Please Enter Remarks.");
            return false;
        }
    });
    $('#SellerName').on("blur keyup change", function () {
        $("#SellerNameErrorMessage").text('');
        var SellerName = $("#SellerName").val();
        if (!SellerName) {
            $("#SellerNameErrorMessage").text("Please Enter Name.");
            return false;
        }
    });
    $('#SellerMobileNo').on("blur keyup change", function () {
        $("#SellerMobileNoErrorMessage").text('');
        var SellerMobileNo = $("#SellerMobileNo").val();
        if (!SellerMobileNo) {
            $("#SellerMobileNoErrorMessage").text("Please Enter Mobile No.");
            return false;
        }
        var ObjVal = SellerMobileNo.length;
        var MobilePat = -1;
        if (ObjVal == 10) {
            MobilePat = 1;
        }
        if (MobilePat == -1) {
            $("#SellerMobileNoErrorMessage").text("Please Enter Valid Mobile No.");
            return false;
        }
    });
    $('#SellerEmail').on("blur keyup change", function () {
        $("#SellerEmailErrorMessage").text('');
        var SellerEmail = $("#SellerEmail").val();
        if (!SellerEmail) {
            $("#SellerEmailErrorMessage").text("Please Enter Email.");
            return false;
        }
        var EmailPat = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (EmailPat.test(SellerEmail) == false) {
            $("#SellerEmailErrorMessage").text("Please Enter Valid Email.");
            return false;
        }
    });
    $('#SellerState').change(function () {
        $("#SellerStateErrorMessage").text('');
        var SellerState = $("#SellerState").val();
        if (!SellerState) {
            $("#SellerStateErrorMessage").text("Please Select State.");
            return false;
        }
    });
    $('#SellerCity').change(function () {
        $("#SellerCityErrorMessage").text('');
        var SellerCity = $("#SellerCity").val();
        if (!SellerCity) {
            $("#SellerCityErrorMessage").text("Please Select City.");
            return false;
        }
    });
    $('#SellerAddress').keyup(function () {
        $("#SellerAddressErrorMessage").text('');
        var SellerAddress = $("#SellerAddress").val();
        if (!SellerAddress) {
            $("#SellerAddressErrorMessage").text("Please Enter Address.");
            return false;
        }
    });
    $('#SellerBrokerName').keyup(function () {
        $("#SellerBrokerNameErrorMessage").text('');
        var SellerBrokerName = $("#SellerBrokerName").val();
        if (!SellerBrokerName) {
            $("#SellerBrokerNameErrorMessage").text("Please Enter Broker Name.");
            return false;
        }
    });
    $('#BuyerName').on("blur keyup change", function () {
        $("#BuyerNameErrorMessage").text('');
        var BuyerName = $("#BuyerName").val();
        if (!BuyerName) {
            $("#BuyerNameErrorMessage").text("Please Enter Name.");
            return false;
        }
    });
    $('#BuyerMobileNo').on("blur keyup change", function () {
        $("#BuyerMobileNoErrorMessage").text('');
        var BuyerMobileNo = $("#BuyerMobileNo").val();
        if (!BuyerMobileNo) {
            $("#BuyerMobileNoErrorMessage").text("Please Enter Mobile No.");
            return false;
        }
        var ObjVal = BuyerMobileNo.length;
        var MobilePat = -1;
        if (ObjVal == 10) {
            MobilePat = 1;
        }
        if (MobilePat == -1) {
            $("#BuyerMobileNoErrorMessage").text("Please Enter Valid Mobile No.");
            return false;
        }
    });
    $('#BuyerEmail').on("blur keyup change", function () {
        $("#BuyerEmailErrorMessage").text('');
        var BuyerEmail = $("#BuyerEmail").val();
        if (!BuyerEmail) {
            $("#BuyerEmailErrorMessage").text("Please Enter Email.");
            return false;
        }
        var EmailPat = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (EmailPat.test(BuyerEmail) == false) {
            $("#BuyerEmailErrorMessage").text("Please Enter Valid Email.");
            return false;
        }
    });
    $('#BuyerState').change(function () {
        $("#BuyerStateErrorMessage").text('');
        var BuyerState = $("#BuyerState").val();
        if (!BuyerState) {
            $("#BuyerStateErrorMessage").text("Please Select State.");
            return false;
        }
    });
    $('#BuyerCity').change(function () {
        $("#BuyerCityErrorMessage").text('');
        var BuyerCity = $("#BuyerCity").val();
        if (!BuyerCity) {
            $("#BuyerCityErrorMessage").text("Please Select City.");
            return false;
        }
    });
    $('#BuyerAddress').keyup(function () {
        $("#BuyerAddressErrorMessage").text('');
        var BuyerAddress = $("#BuyerAddress").val();
        if (!BuyerAddress) {
            $("#BuyerAddressErrorMessage").text("Please Enter Address.");
            return false;
        }
    });
    $('#BuyerBrokerName').keyup(function () {
        $("#BuyerBrokerNameErrorMessage").text('');
        var BuyerBrokerName = $("#BuyerBrokerName").val();
        if (!BuyerBrokerName) {
            $("#BuyerBrokerNameErrorMessage").text("Please Enter Broker Name.");
            return false;
        }
    });
}

function fnAcceptQualifyConfirmationModal() {
    var Validation = true;
    var OpportunityStageId = $("#QualifyOpportunityStageId").val();
    var OpportunityId = $("#QualifyOpportunityId").val();
    var OpportunityStageName = $("#OpportunityStageId_" + OpportunityId + " option:selected").text();
    var WonPropertyId = $("#WonPropertyId").val();
    var AssignTo = 0;
    var LostReasonId = 0;

    var SellerName = $("#SellerName").val();
    var SellerMobileNo = $("#SellerMobileNo").val();
    var SellerEmail = $("#SellerEmail").val();
    var SellerCountry = $("#SellerCountry").val();
    var SellerState = $("#SellerState").val();
    var SellerCity = $("#SellerCity").val();
    var SellerArea = 0;
    var SellerAddress = $("#SellerAddress").val();
    var IsSellerBrokerInvolved = ($("#IsSellerBrokerInvolved").is(":checked"));
    var SellerBrokerName = $("#SellerBrokerName").val();

    var BuyerName = $("#BuyerName").val();
    var BuyerMobileNo = $("#BuyerMobileNo").val();
    var BuyerEmail = $("#BuyerEmail").val();
    var BuyerCountry = $("#BuyerCountry").val();
    var BuyerState = $("#BuyerState").val();
    var BuyerCity = $("#BuyerCity").val();
    var BuyerArea = 0;
    var BuyerAddress = $("#BuyerAddress").val();
    var IsBuyerBrokerInvolved = $("#IsBuyerBrokerInvolved").is(":checked");
    var BuyerBrokerName = $("#BuyerBrokerName").val();

    var TransactionValue = $("#TransactionValue").val();
    var OtherValue = $("#OtherValue").val();
    var SaleDeedValue = $("#SaleDeedValue").val();
    var TotalTransactionValue = $("#TotalTransactionValue").val();
    var TimePeriod = $("#TimePeriod").val();
    var LoanAmount = $("#LoanAmount").val();
    var Brokerage = $("#Brokerage").val();

    if (OpportunityStageName == "Close Won") {
        $("#OppAssignedToIdErrorMessage").text('');
        $("#WonPropertyIdErrorMessage").text('');
        $("#OppRemarksErrorMessage").text('');
        fnclearValidation();

        AssignTo = $("#OppAssignedToId").val();
        if (!AssignTo) {
            $("#OppAssignedToIdErrorMessage").text("Please Select Assigned To.");
            Validation = false;
        }

        if (!WonPropertyId) {
            $("#WonPropertyIdErrorMessage").text("Please Select Property.");
            Validation = false;
        }
        var OppRemarks = $("#OppRemarks").val();
        if (!OppRemarks) {
            $("#OppRemarksErrorMessage").text("Please Enter Remarks.");
            Validation = false;
        }
        if (!SellerName) {
            $("#SellerNameErrorMessage").text("Please Enter Name.");
            Validation = false;
        }
        if (!SellerMobileNo) {
            $("#SellerMobileNoErrorMessage").text("Please Enter Mobile No.");
            Validation = false;
        }
        var ObjVal = SellerMobileNo.length;
        var MobilePat = -1;
        if (ObjVal == 10) {
            MobilePat = 1;
        }
        if (MobilePat == -1) {
            $("#SellerMobileNoErrorMessage").text("Please Enter Valid Mobile No.");
            Validation = false;
        }
        if (!SellerEmail) {
            $("#SellerEmailErrorMessage").text("Please Enter Email.");
            Validation = false;
        }
        var EmailPat = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (EmailPat.test(SellerEmail) == false) {
            $("#SellerEmailErrorMessage").text("Please Enter Valid Email.");
            Validation = false;
        }
        if (!SellerState) {
            $("#SellerStateErrorMessage").text("Please Select State.");
            Validation = false;
        }
        if (!SellerCity) {
            $("#SellerCityErrorMessage").text("Please Select City.");
            Validation = false;
        }
        if (!SellerAddress) {
            $("#SellerAddressErrorMessage").text("Please Enter Address.");
            Validation = false;
        }

        if (!BuyerName) {
            $("#BuyerNameErrorMessage").text("Please Enter Name.");
            Validation = false;
        }
        if (!BuyerMobileNo) {
            $("#BuyerMobileNoErrorMessage").text("Please Enter Mobile No.");
            Validation = false;
        }
        var ObjVal = BuyerMobileNo.length;
        var MobilePat = -1;
        if (ObjVal == 10) {
            MobilePat = 1;
        }
        if (MobilePat == -1) {
            $("#BuyerMobileNoErrorMessage").text("Please Enter Valid Mobile No.");
            Validation = false;
        }
        if (!BuyerEmail) {
            $("#BuyerEmailErrorMessage").text("Please Enter Email.");
            Validation = false;
        }
        var EmailPat = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (EmailPat.test(BuyerEmail) == false) {
            $("#BuyerEmailErrorMessage").text("Please Enter Valid Email.");
            Validation = false;
        }
        if (!BuyerState) {
            $("#BuyerStateErrorMessage").text("Please Select State.");
            Validation = false;
        }
        if (!BuyerCity) {
            $("#BuyerCityErrorMessage").text("Please Select City.");
            Validation = false;
        }

        if (!BuyerAddress) {
            $("#BuyerAddressErrorMessage").text("Please Enter Address.");
            Validation = false;
        }
        if (IsSellerBrokerInvolved == true) {
            if (!SellerBrokerName) {
                $("#SellerBrokerNameErrorMessage").text("Please Enter Broker Name.");
                Validation = false;
            }
        }
        if (IsBuyerBrokerInvolved == true) {
            if (!BuyerBrokerName) {
                $("#BuyerBrokerNameErrorMessage").text("Please Enter Broker Name.");
                Validation = false;
            }
        }

        if (!Validation) {
            fnOnChangeValidation();
            return false;
        }

        var PaymentDetails = "";
        var docDetails = [];
        $("#tblPaymentTermsDetail tr").each(function () {
            var this_row = $(this);
            var DetailId = parseInt($.trim(this_row.find('.DetailId').html()));
            if (DetailId == 0) {
                docDetails.push({
                    Date: $.trim(this_row.find('.Date').html()),
                    Description: $.trim(this_row.find('.Description').html()),
                    Amount: $.trim(this_row.find('.Amount').html()),
                    DetailId: parseInt($.trim(this_row.find('.DetailId').html()))
                });
            }
        });
        if (docDetails.length > 0) {
            PaymentDetails = JSON.stringify(docDetails);
        } else {
            swal("Please Enter Provisional Payment Terms Detail", {
                icon: "error",
            });
            PaymentDetails = "";
            return true;
        }
        if (ValidationChecked == 0) {
            fnConfirmationInsertEdit(SellerMobileNo, "Seller");
        }
    }
    if (OpportunityStageName == "Close Lost") {
        LostReasonId = $("#OppLostReasonId").val();
        if (!LostReasonId) {
            swal("Please Select Lost Reason", {
                icon: "error",
            });
            return false;
        }
        var OppRemarks = $("#OppRemarks").val();
        if (!OppRemarks) {
            swal("Please Enter Remarks", {
                icon: "error",
            });
            return false;
        }
    }

    if (ValidationChecked == 1 || OpportunityStageName == "Close Lost") {
        fnUpdateOpportunityStatusId(OpportunityStageId, OpportunityId, OpportunityStageName, AssignTo, LostReasonId, OppRemarks, WonPropertyId, SellerName, SellerMobileNo, SellerEmail, SellerCountry, SellerState, SellerCity, SellerArea, SellerAddress, IsSellerBrokerInvolved, SellerBrokerName, BuyerName, BuyerMobileNo, BuyerEmail, BuyerCountry, BuyerState, BuyerCity, BuyerArea, BuyerAddress, IsBuyerBrokerInvolved, BuyerBrokerName, TransactionValue, OtherValue, SaleDeedValue, TotalTransactionValue, TimePeriod, LoanAmount, Brokerage, PaymentDetails, SellerMobileNoIsEdit, BuyerMobileNoIsEdit);
        $('#OppAssignedToId').empty().trigger('change');
        $('#OppLostReasonId').empty().trigger('change');
        $("#OppRemarks").val('');
        $("#CustomerName").val('');
        $("#Email").val('');
        $("#MobileNo").val('');
        $("#QualifyOpportunityId").val(0);
        $("#QualifyOpportunityStageId").val(0);
        $("#QualifySelectedOpportunityStageId").val(0);
        $("#QualifyLeadId_hdn").val(0);
        $("#QualifyConfirmationModal").modal('hide');
    }
}


function fnCancelQualifyConfirmationModal() {
    $("#OpportunityStageId_" + $("#QualifyOpportunityId").val() + "").val($("#QualifySelectedOpportunityStageId").val()).trigger('change');
    $('#OppAssignedToId').empty().trigger('change');
    $('#OppLostReasonId').empty().trigger('change');
    $("#OppRemarks").val('');
    $("#QualifyOpportunityId").val(0);
    $("#QualifyOpportunityStageId").val(0);
    $("#QualifySelectedOpportunityStageId").val(0);
    $("#QualifyLeadId_hdn").val(0);
    $('#WonPropertyId').empty().trigger('change');
    $("#QualifyConfirmationModal").modal('hide');
}
function fnUpdateOpportunityStatusId(OpportunityStageId, OpportunityId, OpportunityStageName, AssignTo, LostReasonId, OppRemarks, WonPropertyId, SellerName, SellerMobileNo, SellerEmail, SellerCountry, SellerState, SellerCity, SellerArea, SellerAddress, IsSellerBrokerInvolved, SellerBrokerName, BuyerName, BuyerMobileNo, BuyerEmail, BuyerCountry, BuyerState, BuyerCity, BuyerArea, BuyerAddress, IsBuyerBrokerInvolved, BuyerBrokerName, TransactionValue, OtherValue, SaleDeedValue, TotalTransactionValue, TimePeriod, LoanAmount, Brokerage, PaymentDetails, SellerMobileNoIsEdit, BuyerMobileNoIsEdit) {
    var AssignedToId = $("#AssignedToId_" + OpportunityId).val();
    $.ajax({
        type: "POST",
        url: "/Transactions/Opportunity/UpdateOpportunityStatus",
        data: {
            OpportunityId: OpportunityId,
            OpportunityStageId: OpportunityStageId,
            OpportunityStageName: OpportunityStageName,
            AssignTo: AssignTo,
            LostReasonId: LostReasonId,
            WonPropertyId: WonPropertyId,
            Remarks: OppRemarks,
            SellerName: SellerName,
            SellerMobileNo: SellerMobileNo,
            SellerEmail: SellerEmail,
            SellerCountry: SellerCountry,
            SellerState: SellerState,
            SellerCity: SellerCity,
            SellerArea: SellerArea,
            SellerAddress: SellerAddress,
            IsSellerBrokerInvolved: IsSellerBrokerInvolved,
            SellerBrokerName: SellerBrokerName,
            BuyerName: BuyerName,
            BuyerMobileNo: BuyerMobileNo,
            BuyerEmail: BuyerEmail,
            BuyerCountry: BuyerCountry,
            BuyerState: BuyerState,
            BuyerCity: BuyerCity,
            BuyerArea: BuyerArea,
            BuyerAddress: BuyerAddress,
            IsBuyerBrokerInvolved: IsBuyerBrokerInvolved,
            BuyerBrokerName: BuyerBrokerName,
            TransactionValue: TransactionValue,
            OtherValue: OtherValue,
            SaleDeedValue: SaleDeedValue,
            TotalTransactionValue: TotalTransactionValue,
            TimePeriod: TimePeriod,
            LoanAmount: LoanAmount,
            Brokerage: Brokerage,
            PaymentDetails: PaymentDetails,
            SellerMobileNoIsEdit: SellerMobileNoIsEdit,
            BuyerMobileNoIsEdit: BuyerMobileNoIsEdit,
            AssignedToId: AssignedToId

        },
        async: !1,
        cache: !1,
        success: function (data) {
            swal(data.Message, {
                icon: data.MessageType,
            });
            fnBindList();
        },
        error: function (response) {
            console.log(response);
        }
    });
}
//for update specialnote
function fnChangeSpecialNote(Id, e) {
    var SpecialNotes = $(e).val();
    if (SpecialNotes != "") {
        $.ajax({
            url: "/Transactions/Opportunity/UpdateSpecialNote",
            data: { Id: Id, SpecialNotes: SpecialNotes },
            async: true,
            cache: false,
            type: "GET",
            success: function (data) {
                fnBindList();
                swal(data.message, {
                    icon: data.messageType,
                });
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
}
//for update bookmarked
function fnUpdateBookmarkOpportunity(Id, Flag) {
    $.ajax({
        type: "GET",
        url: "/Transactions/Opportunity/UpdateBookmark",
        data: {
            Id: Id,
            Flag: Flag,
        },
        async: true,
        cache: false,
        success: function (data) {
            fnBindList();
            swal(data.message, {
                icon: data.messageType,
            });
        },
        error: function (response) {
            console.log(response);
        }
    });
}

//=========================================== For Update Fields END =========================================//

//=========================================== Property Details Start =========================================//
//for open PropertyDetail popup
function fnShowPropertyDetailPopup(Id, MappedCount) {
    $("#PropertyDetailPopupId").val(Id);
    $("#MappedCount").html(MappedCount);

    fnChangePropertyDetailTab(1);

    $("#PropertyDetailModal").modal('show');
}
function fnChangePropertyDetailTab(Tab) {
    var Id = $("#PropertyDetailPopupId").val();
    if (Tab == 1) {
        $("#Mappedtab-1").addClass('active');

        $("#PropertyDetailtab1").addClass('active show');

        fnBindMappedPropertyList(Id, Tab);
    }
}
function fnBindMappedPropertyList(LeadId, TypeId) {
    var table = $('#TBLMappedProperty').DataTable({
        "bDestroy": true,
        "pageLength": $('#PageSize').val(),
        "lengthChange": true,
        "paging": true,
        "autoWidth": true,
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ordering": false,
        "order": [0, "desc"],
        "ajax": {
            "url": "/Transactions/Lead/GetPropertyDetailList",
            "type": "POST",
            "datatype": "json",
            "data": function (o) {
                o.search = $("#search").val();
                o.LeadId = LeadId;
                o.TypeId = TypeId;
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "PropertyID", "autoWidth": true },
            { "data": "AvailabilityName", "autoWidth": true },
            { "data": "CategoryName", "autoWidth": true },
            { "data": "SubCategoryName", "autoWidth": true },
            { "data": "PropertyFor", "autoWidth": true },
            { "data": "PropertyBHKName", "autoWidth": true },
            { "data": "AreaName", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    var SuperBuildupAreaVal = AmountWithComma(full.SuperBuildupArea);
                    return SuperBuildupAreaVal + ' ' + full.SuperBuildupAreaUnitName;
                }
            },
            {
                mRender: function (data, type, full) {
                    var CarpetAreaVal = AmountWithComma(full.CarpetArea);
                    return CarpetAreaVal + ' ' + full.CarpetAreaUnitName;
                }
            },
            { "data": "ProjectName", "autoWidth": true },
            { "data": "Date", "autoWidth": true },
            { "data": "PropertyStatusName", "autoWidth": true },
            { "data": "PropertyFurnishStatusName", "autoWidth": true },
            { "data": "PropertyAvailableFor", "autoWidth": true },
            { "data": "PropertyPossessionStatus", "autoWidth": true },
            {
                mRender: function (data, type, full) {
                    return '<a target="_blank" href="/Transactions/Property/View_New?EncryptedId=' + full.EncryptedId + '" title="View" class="btn btn-success btn-sm"><i class="fas fa-eye"></i></a>';
                }
            }
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [0] }
        ]
    });
}
//=========================================== Property Details End ===========================================//

function initializeAllCheckBox() {
    var n = $("#allCheckBox").is(":checked");
    $("#TBL .singleCheckBox").prop("checked", n ? !0 : !1);
    $("#TBL .singleCheckBox").closest("tr").addClass(n ? "selected-row" : "not-selected-row");
    $("#TBL .singleCheckBox").closest("tr").removeClass(n ? "not-selected-row" : "selected-row");

}
function initializeSingleCheckBox(n) {
    var t = $(n).is(":checked");
    $(n).closest("tr").addClass(t ? "selected-row" : "not-selected-row");
    $(n).closest("tr").removeClass(t ? "not-selected-row" : "selected-row");
    t && $("#TBL .singleCheckBox").length == $("#TBL .selected-row").length ? $("#allCheckBox").prop("checked", !0) : $("#allCheckBox").prop("checked", !1);
}


//=========================================== Close Won Start ===========================================//

function fnclearValidation() {
    
    $("#SellerNameErrorMessage").text('');
    $("#SellerMobileNoErrorMessage").text('');
    $("#SellerEmailErrorMessage").text('');
    $("#SellerStateErrorMessage").text('');
    $("#SellerCityErrorMessage").text('');
    $("#SellerAddressErrorMessage").text('');
    $("#SellerBrokerNameErrorMessage").text('');
    $("#BuyerNameErrorMessage").text('');
    $("#BuyerMobileNoErrorMessage").text('');
    $("#BuyerEmailErrorMessage").text('');
    $("#BuyerStateErrorMessage").text('');
    $("#BuyerCityErrorMessage").text('');
    $("#BuyerAddressErrorMessage").text('');
    $("#BuyerBrokerNameErrorMessage").text('');
}

$("#WonPropertyId").on('change', function () {
    if ($("#WonPropertyId").val() != null && $("#WonPropertyId").val() != "" && $("#WonPropertyId").val() != 0) {
        $.ajax({
            type: "GET",
            url: "/Transactions/Opportunity/GetPropertySellerBuyerDetails",
            data: {
                WonPropertyId: $("#WonPropertyId").val(),
                LeadId: $("#QualifyLeadId_hdn").val(),
            },
            async: true,
            cache: false,
            success: function (data) {
                fnClearWonChangeValue();
                $("#ProjectDetails").attr('hidden', false);
                $("#ProjectName").html(data.property.ProjectName);
                $("#BlockNo").html(data.property.BlockNo);
                $("#BHKName").html(data.property.BHKName);
                $("#FloorName").html(data.property.FloorName);
                $("#UnitNo").html(data.property.UnitNo);
                $("#Address1").html(data.property.Address1 + ', ' + data.property.AreaName + ', ' + data.property.CityName + ', ' + data.property.StateName);
                var CarpetVal = AmountWithComma(data.property.CarpetArea);
                $("#CarpetAreaUnitName").html(data.property.CarpetAreaUnitName);
                $("#CarpetArea").html(CarpetVal);
                $("#SuperBuildupAreaUnitName").html(data.property.SuperBuildupAreaUnitName);
                var SuperBuildVal = AmountWithComma(data.property.SuperBuildupArea);
                $("#SuperBuildupArea").html(SuperBuildVal);
                var BasicPriceVal = AmountWithComma(data.property.BasicPrice);
                $("#BasicPrice").html(BasicPriceVal);

                $("#SellerName").val(data.property.FirstName + ' ' + data.property.LastName);
                $("#SellerMobileNo").val(data.property.MobileNo);
                $("#SellerEmail").val(data.property.Email);
                $('#SellerCountry').append(new Option(data.property.CountryName, data.property.CountryId, true, true));
                //$('#SellerState').append(new Option(data.property.StateName, data.property.StateId, true, true));
                //$('#SellerCity').append(new Option(data.property.CityName, data.property.CityId, true, true));
                //$('#SellerArea').append(new Option(data.property.AreaName, data.property.AreaId, true, true));
                //$("#SellerAddress").val(data.property.Address1);

                $("#BuyerName").val(data.lead.FirstName + ' ' + data.lead.LastName);
                $("#BuyerMobileNo").val(data.lead.MobileNo);
                $("#BuyerEmail").val(data.lead.Email);
                $('#BuyerCountry').append(new Option(data.lead.CountryName, data.lead.CountryId, true, true));
                //$('#BuyerState').append(new Option(data.lead.StateName, data.lead.StateId, true, true));
                //$('#BuyerCity').append(new Option(data.lead.CityName, data.lead.CityId, true, true));
                //$('#BuyerArea').append(new Option(data.lead.AreaName, data.lead.AreaId, true, true));


                $("#TransactionValue").val(data.property.BasicPrice);
                $("#Brokerage").val(data.property.Brokerage);
                fnTotalTransactionValue();
            },
            error: function (response) {
                console.log(response);
            }
        });
    }
});

function fnClearWonChangeValue() {
    $("#SellerName").val("");
    $("#SellerMobileNo").val('');
    $("#SellerEmail").val('');
    $("#SellerCountry").empty().trigger('change');
    $("#SellerState").empty().trigger('change');
    $("#SellerCity").empty().trigger('change');
    $("#SellerArea").empty().trigger('change');
    $("#SellerAddress").val('');
    $("#SellerBrokerName").val('');
    $('#IsSellerBrokerInvolved').prop('checked', false);

    $("#BuyerName").val('');
    $("#BuyerMobileNo").val('');
    $("#BuyerEmail").val('');
    $("#BuyerCountry").empty().trigger('change');
    $("#BuyerState").empty().trigger('change');
    $("#BuyerCity").empty().trigger('change');
    $("#BuyerArea").empty().trigger('change');
    $("#BuyerAddress").val('');
    $("#BuyerBrokerName").val('');
    $('#IsBuyerBrokerInvolved').prop('checked', false);

    $("#TransactionValue").val(0.00);
    $("#OtherValue").val(0.00);
    $("#SaleDeedValue").val(0.00);
    $("#TotalTransactionValue").val(0.00);
    $("#TimePeriod").val(0);
    $("#LoanAmount").val(0);
    $("#Brokerage").val(0);

    $("#Date").val('');
    $("#Description").val('');
    $("#Amount").val(0);

    $("#PaymentTermsDetailTotalAmount").text(0.00);

    $('#tblPaymentTermsDetail tr').remove();
    var htmlString = ''
        + '<tr id="tr_0">'
        + '<td colspan="6" style="text-align: center;">No Record Found</td>'
        + '</tr>'
        + '';

    $('#tblPaymentTermsDetail').append(htmlString);
    ValidationChecked = 0;
    SellerMobileNoIsEdit = 0;
    BuyerMobileNoIsEdit = 0;
    fnclearValidation();
}

function fnTotalTransactionValue() {
    var TransactionValue = parseFloat($("#TransactionValue").val());
    var OtherValue = parseFloat($("#OtherValue").val());
    var SaleDeedValue = parseFloat($("#SaleDeedValue").val());
    if (isNaN(TransactionValue)) {
        TransactionValue = 0;
    }
    if (isNaN(OtherValue)) {
        OtherValue = 0;
    }
    if (isNaN(SaleDeedValue)) {
        SaleDeedValue = 0;
    }
    var FinalTotalTransactionValue = TransactionValue + OtherValue + SaleDeedValue;
    $("#TotalTransactionValue").val(0);
    $("#TotalTransactionValue").val(parseFloat(FinalTotalTransactionValue).toFixed(2));
}

function fnClearWonPropertyValue() {
    $("#ProjectDetails").attr('hidden', true);
    $(".SellerBrokerName").hide();
    $(".BuyerBrokerName").hide();

    $("#WonPropertyId").empty().trigger('change');
    $("#OppAssignedToId").empty().trigger('change');
    $("#OppRemarks").val('');

    $("#ProjectName").val('Not Defined');
    $("#BlockNo").val('Not Defined');
    $("#BHKName").val('Not Defined');
    $("#FloorName").val('Not Defined');
    $("#UnitNo").val('Not Defined');
    $("#Address1").val('Not Defined');
    $("#CarpetAreaUnitName").val('Not Defined');
    $("#CarpetArea").val('Not Defined');
    $("#SuperBuildupAreaUnitName").val('Not Defined');
    $("#SuperBuildupArea").val('Not Defined');
    $("#BasicPrice").val('Not Defined');

    $("#SellerName").val("");
    $("#SellerMobileNo").val('');
    $("#SellerEmail").val('');
    $("#SellerCountry").empty().trigger('change');
    $("#SellerState").empty().trigger('change');
    $("#SellerCity").empty().trigger('change');
    $("#SellerArea").empty().trigger('change');
    $("#SellerAddress").val('');
    $("#SellerBrokerName").val('');
    $('#IsSellerBrokerInvolved').prop('checked', false);

    $("#BuyerName").val('');
    $("#BuyerMobileNo").val('');
    $("#BuyerEmail").val('');
    $("#BuyerCountry").empty().trigger('change');
    $("#BuyerState").empty().trigger('change');
    $("#BuyerCity").empty().trigger('change');
    $("#BuyerArea").empty().trigger('change');
    $("#BuyerAddress").val('');
    $("#BuyerBrokerName").val('');
    $('#IsBuyerBrokerInvolved').prop('checked', false);

    $("#TransactionValue").val(0.00);
    $("#OtherValue").val(0.00);
    $("#SaleDeedValue").val(0.00);
    $("#TotalTransactionValue").val(0.00);
    $("#TimePeriod").val(0);
    $("#LoanAmount").val(0);
    $("#Brokerage").val(0);

    $("#Date").val('');
    $("#Description").val('');
    $("#Amount").val(0);

    $("#PaymentTermsDetailTotalAmount").text(0.00);

    $('#tblPaymentTermsDetail tr').remove();
    var htmlString = ''
        + '<tr id="tr_0">'
        + '<td colspan="6" style="text-align: center;">No Record Found</td>'
        + '</tr>'
        + '';

    $('#tblPaymentTermsDetail').append(htmlString);
    ValidationChecked = 0;
    SellerMobileNoIsEdit = 0;
    BuyerMobileNoIsEdit = 0;
    $("#OppAssignedToIdErrorMessage").text('');
    $("#WonPropertyIdErrorMessage").text('');
    $("#OppRemarksErrorMessage").text('');
    fnclearValidation();
}

function fnClearPaymentTermsDetail() {
    $("#Date").val('');
    $("#Description").val('');
    $("#Amount").val(0.00);
    //$('#DetailId').val(DetailId);
    $('#UpdateId').val(0);
    $("#DateErrorMessage").text("");
    $("#DescriptionErrorMessage").text("");
    $("#AmountErrorMessage").text("");
}

function fnDateValidation() {
    $("#DateErrorMessage").text('');
    var Date = $("#Date").val();
    if (!Date) {
        $("#DateErrorMessage").text("Please Select Date.");
        return false;
    }
}

function fnOnChangePaymentValidation() {
    $('#Description').keyup(function () {
        $("#DescriptionErrorMessage").text('');
        var Description = $("#Description").val();
        if (!Description) {
            $("#DescriptionErrorMessage").text("Please Enter Description.");
            return false;
        }
    });
    $('#Amount').keyup(function () {
        $("#AmountErrorMessage").text('');
        var Amount = $("#Amount").val();
        if (isNaN(Amount) || Amount < 0.01) {
            $("#AmountErrorMessage").text("Please Enter Amount.");
            return false;
        }
    });
}

function fnAddPaymentDetails() {
    var ValidationPayment = true;
    var Date = $("#Date").val();
    var Description = $("#Description").val();
    var Amount = parseFloat($('#Amount').val());

    $("#DateErrorMessage").text("");
    $("#DescriptionErrorMessage").text("");
    $("#AmountErrorMessage").text("");
    if (!Date) {
        $("#DateErrorMessage").text("Please Select Date.");
        ValidationPayment = false;
    }
    if (!Description) {
        $("#DescriptionErrorMessage").text("Please Enter Description.");
        ValidationPayment = false;
    }
    if (isNaN(Amount) || Amount < 0.01) {
        $("#AmountErrorMessage").text("Please Enter Amount.");
        ValidationPayment = false;
    }

    if (!ValidationPayment) {
        fnOnChangePaymentValidation();
        return false;
    }

    var DetailId = parseInt($('#DetailId').val());
    if (isNaN(DetailId))
        DetailId = 0;

    var obj = new Object();
    obj.Date = Date;
    obj.Description = Description;
    obj.Amount = Amount;
    obj.DetailId = DetailId;

    fnAddPaymentDetailsRow(obj);
    fnClearPaymentTermsDetail();

};

function fnAddPaymentDetailsRow(obj) {

    var UpdateId = parseInt($('#UpdateId').val());
    if (isNaN(UpdateId))
        UpdateId = 0;
    var lastId = 0;
    if (UpdateId === 0) {
        try {
            lastId = parseInt($("#tblPaymentTermsDetail tr:last td.DetailId:last").attr('id').split("_").pop());
        } catch (e) {
            lastId = 0;
        }
        if (isNaN(lastId))
            lastId = 0;

        lastId = lastId === 0 ? 1 : lastId + 1;

        if (lastId === 1) {
            $('#tr_0').remove();
        }

    } else {
        lastId = UpdateId;
    }

    var EditRow = '<button type="button" onclick="fnEditDetail(' + lastId + ');" title="Edit" class="btn btn-primary btn-sm"><i class="fa fa-edit"></i></button>';
    var DeleteRow = '<button type="button" onclick="fnDeleteDetail(' + lastId + ');" title="Delete" class="btn btn-danger btn-sm"><i class="fa fa-trash"></i></button>';

    var htmlString = ''
        + '<td>' + lastId + '</td>'
        + '<td class="Date" id="Date_' + lastId + '">' + obj.Date + '</td>'
        + '<td class="Description" id="Description_' + lastId + '">' + obj.Description + '</td>'
        + '<td class="Amount" id="Amount_' + lastId + '">' + obj.Amount + '</td>'
        + '<td>' + EditRow + '</td>'
        + '<td>' + DeleteRow + '</td>'
        + '<td class="DetailId" id="DetailId_' + lastId + '" hidden>' + obj.DetailId + '</td>'
        + '';

    if (UpdateId > 0) {
        $('#tr_' + lastId).html(htmlString);
        $('#UpdateId').val(0);
    }
    else {
        htmlString = '<tr id="tr_' + lastId + '">'
            + htmlString
            + '</tr>';
        $('#tblPaymentTermsDetail').append(htmlString);
    }

    fnPaymentTermsDetailTotalAmount();
}

function fnEditDetail(RowId) {

    var Date = $('#Date_' + RowId).text();
    $('#Date').val(Date);

    var Description = $('#Description_' + RowId).text();
    $('#Description').val(Description);

    var Amount = $('#Amount_' + RowId).text();
    $('#Amount').val(Amount);

    var DetailId = parseInt($('#DetailId_' + RowId).text());
    $('#DetailId').val(DetailId);
    $('#UpdateId').val(RowId);
}

function fnDeleteDetail(RowId) {
    swal({
        title: 'Are you sure?',
        text: 'Provisional Payment Terms Detail will be Removed!',
        icon: 'warning',
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $('#tr_' + RowId).remove();
                fnPaymentTermsDetailTotalAmount();
            } else {
            }
        });
}

function fnPaymentTermsDetailTotalAmount() {
    $("#PaymentTermsDetailTotalAmount").text(0.00);
    $(".Amount").each(function () {
        var Amount = parseFloat($(this).text());
        if (isNaN(Amount)) {
            Amount = 0;
        }
        var PaymentTermsDetailTotalAmount = parseFloat($("#PaymentTermsDetailTotalAmount").text());
        if (isNaN(PaymentTermsDetailTotalAmount)) {
            PaymentTermsDetailTotalAmount = 0;
        }
        var FinalPaymentTermsDetailTotalAmount = PaymentTermsDetailTotalAmount + Amount
        $("#PaymentTermsDetailTotalAmount").text(parseFloat(FinalPaymentTermsDetailTotalAmount).toFixed(2));
    });
}

//=========================================== Close Won End ===========================================//