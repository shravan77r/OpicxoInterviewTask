var dataRowEmployee = [];
var objAssignedType;
var EmployeeselectedIds = [];
var IsBookMarked;
var IsQueryStringExist = 0;
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

});
function fnChangeView(Type) {
    $(".preloader").fadeIn("slow");
    if (Type == 2) {
        $("#view-list").addClass("Opened");
        $("#view-grid").removeClass("Opened");
        $("#view-grid").removeClass('active show');
        $("#view-list").addClass('active show');
    }
    else {
        $("#view-grid").addClass("Opened");
        $("#view-list").removeClass("Opened");
        $("#view-list").removeClass('active show');
        $("#view-grid").addClass('active show');
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
        url: "/Transactions/Lead/GetList",
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
            //AssignedIds: $("#AssignedIds").val().join(","),
            //AssigneeIds: $("#AssigneeIds").val().join(","),
            IsBookMarked: $("#IsBookMarked").val(),
            //RelevanceIds: $("#RelevanceIds").val().join(","),
            StatusIds: $("#LeadStageIdUI").val().join(","),
            LeadForIds: $("#PropertyForIdUI").val().join(","),
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
                        if (EditId == vals.EncryptedId) {
                            IsScroll = 1;
                        }
                        if (EditId != vals.EncryptedId && IsScroll == 0 && EditId != "") {
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
                            Html += '       <div class="list-action" >';
                            Html += '                <div class="custom-control custom-checkbox">';
                            Html += '                    <input type="hidden" value="' + vals.QualifyDataStatus + '" id="QualifyDataStatus_' + vals.Id + '">';
                            Html += '                    <input type="checkbox" class="custom-control-input singleCheckBoxAssign" onclick="return initializeSingleCheckBoxAssign(this);" value="' + vals.Id + '" id="ListCheck_' + vals.Id + '">';
                            Html += '                    <label class="custom-control-label" for="ListCheck_' + vals.Id + '"></label>';
                            Html += '                </div>';
                            if (vals.IsBookmarked == 1) {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkLead(' + vals.Id + ',0)" style="cursor:pointer"><i class="fas fa-star zoom" aria-hidden="true"></i></a></div>';
                            }
                            else {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkLead(' + vals.Id + ',1)"><i class="far fa-star zoom"></i></a></div>';
                            }
                            if (vals.StageName == "Qualify") {
                                Html += '                <div class="list-li diamond"><i class="far fa-trophy"></i></div>';
                            }
                            Html += '            </div>';
                            Html += '            <div class="list-info">';
                            Html += '                <div class="list-id">Lead ID: ' + vals.LeadNo + '</div>';
                            Html += '                <div class="list-postdate mb-2">Lead Date: ' + vals.LeadDate + ' ' + vals.Days + ' Days Ago</div>';
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
                            Html += '                    </div>';
                            Html += '                    <div class="col-xl-4 col-lg-4 col-md-4">';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Lead Score:</label>';
                            Html += '                            <label class="list-features-text">';
                            Html += '                                <span class="d-flex">';
                            if (vals.LeadScoreName == "") {
                                Html += '<i>Not Defined</i>';
                            } else {
                                for (var iPScore = 0; iPScore < vals.LeadScoreName; iPScore++) {//to verify
                                    Html += '<span class="fas fa-star fill-yellow"></span>';
                                }
                            }
                            //Html += '                                    <span class="fas fa-star fill-yellow"></span>';
                            //Html += '                                    <span class="fas fa-star fill-yellow"></span>';
                            //Html += '                                    <span class="fas fa-star fill-yellow"></span>';
                            //Html += '                                    <span class="fas fa-star fill-yellow"></span>';
                            //Html += '                                    <span class="far fa-star fill-yellow"></span>';
                            Html += '                                </span>';
                            Html += '                            </label>';
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Housing Segment:</label>';
                            if (vals.LeadZone == "") {
                                Html += '                            <label class="list-features-text"><i>Not Defined</i></label>';
                            } else {
                                Html += '                            <label class="list-features-text">' + vals.LeadZone + '</label>';
                            }
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Lead Interest:</label>';
                            Html += '                            <label class="list-features-text">';
                            if (vals.PriorityName == "Hot") {
                                Html += '<span data-toggle="tooltip" data-placement="top" title="Hot">';
                                Html += '    <img class="list-icon" src="/adminpanel/images/hot.png" />';
                                Html += '</span>';
                            }
                            else if (vals.PriorityName == "Warm") {
                                Html += '<span data-toggle="tooltip" data-placement="top" title="Warm">';
                                Html += '    <img class="list-icon" src="/adminpanel/images/warm.png" />';
                                Html += '</span>';
                            }
                            else {
                                Html += '<span data-toggle="tooltip" data-placement="top" title="cold">';
                                Html += '    <img class="list-icon" src="/adminpanel/images/cold.png" />';
                                Html += '</span>';
                            }
                            Html += '                            </label>';
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-label">Status:</label>';
                            Html += '                            <label class="list-features-text" style="width: 150px;">';
                            if (vals.StageName == "Qualify") {
                                var ddLeadStatus = '';
                                if (vals.OpportunityNo == '') {
                                    ddLeadStatus += '<select class="form-control LeadStatusId Qualify_Status" id="LeadStatusId_' + vals.Id + '" selectedid="' + vals.LeadStageId + '">';
                                    ddLeadStatus += '<option value="' + vals.LeadStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                } else {
                                    ddLeadStatus += '<select class="form-control LeadStatusId Qualify_Status" id="LeadStatusId_' + vals.Id + '" disabled>';
                                    ddLeadStatus += '<option value="' + vals.LeadStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                Html += ddLeadStatus;
                            } else if (vals.StageName == "Disqualify") {
                                var ddLeadStatus = '';
                                if (vals.OpportunityNo == '') {
                                    ddLeadStatus += '<select class="form-control LeadStatusId Disqualify_Status" id="LeadStatusId_' + vals.Id + '" selectedid="' + vals.LeadStageId + '">';
                                    ddLeadStatus += '<option value="' + vals.LeadStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                } else {
                                    ddLeadStatus += '<select class="form-control LeadStatusId Disqualify_Status" id="LeadStatusId_' + vals.Id + '" disabled>';
                                    ddLeadStatus += '<option value="' + vals.LeadStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                Html += ddLeadStatus;
                            } else {
                                var ddLeadStatus = '';
                                if (vals.OpportunityNo == '') {
                                    ddLeadStatus += '<select class="form-control LeadStatusId" id="LeadStatusId_' + vals.Id + '" selectedid="' + vals.LeadStageId + '">';
                                    ddLeadStatus += '<option value="' + vals.LeadStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                } else {
                                    ddLeadStatus += '<select class="form-control LeadStatusId" id="LeadStatusId_' + vals.Id + '" disabled>';
                                    ddLeadStatus += '<option value="' + vals.LeadStageId + '">';
                                    ddLeadStatus += vals.StageName;
                                    ddLeadStatus += '</option>';
                                    ddLeadStatus += '</select>';
                                }
                                Html += ddLeadStatus;
                            }

                            //Html += '                                <select class="form-control">';
                            //Html += '                                    <option value="">In Process</option>';
                            //Html += '                                    <option value="">Quality</option>';
                            //Html += '                                    <option value="">Disquality</option>';
                            //Html += '                                </select>';
                            Html += '                            </label>';
                            Html += '                        </div>';
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
                            /*Html += '                            <label class="list-features-label">Followup Taken:</label>';*/
                            Html += '                            <label class="list-features-text">';
                            Html += '                                <a href="javascript:;" onclick="fnOpenActivity(\'' + vals.EncryptedId + '\',' + vals.LeadStageId + ')" class="list-link">Follow-up Taken:' + vals.FollowupTaken + '</a>';
                            Html += '                            </label>';
                            Html += '                            <span class="hint-text d-block">' + vals.FollowupDescription + '</span>';
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
                            Html += '                                <a href="javascript:;" onclick="fnShowPropertyDetailPopup(' + vals.Id + ',3,' + vals.MappedProperty + ',' + vals.AutoMatchProperty + ',' + vals.PropertyDetail + ' )" class="list-link">Property Detail (' + vals.PropertyDetail + ')</a>';
                            Html += '                            </label>';
                            Html += '                        </div>';
                            Html += '                        <div class="features-list">';
                            Html += '                            <label class="list-features-text">';
                            Html += '                                <a href="javascript:;" onclick="fnShowPropertyDetailPopup(' + vals.Id + ',2,' + vals.MappedProperty + ',' + vals.AutoMatchProperty + ',' + vals.PropertyDetail + ',\'' + vals.FullName + '\',\'' + vals.MobileNo + '\',\'' + vals.Email + '\'' + ')" class="list-link">Matching Property (' + vals.AutoMatchProperty + ')</a>';
                            Html += '                            </label>';
                            Html += '                            <label class="list-features-text pl-2">';
                            Html += '                                <span class="listicon-btn" data-toggle="tooltip" data-placement="top" title="Share All Properties">';
                            Html += '                                   <a href="javascript:;" onclick="fnBindShareMatchingPropertyList(' + vals.Id + ',1,\'' + vals.FullName + '\',\'' + vals.MobileNo + '\',\'' + vals.Email + '\'' + ')" class="list-link" data-toggle="modal" data-target="#SharePropertyModal"><img class="img-share" src="/adminpanel/images/building-share-icon.png"></a>';
                            Html += '                                </span>';
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
                            //Html += '                            <label class="list-features-text">' + vals.AssignToName + '';
                            //Html += '                                <select class="form-control">';
                            //Html += '                                    <option value="">Rohit Patel</option>';
                            //Html += '                                    <option value="">Manish Patel</option>';
                            //Html += '                                    <option value="">Akash Shah</option>';
                            //Html += '                                </select>';
                            Html += '                            </label>';
                            Html += '                        </div>';
                            Html += '                    </div>';
                            Html += '                </div>';
                            Html += '                <div class="row">';
                            Html += '                    <div class="col-xl-4">';
                            Html += '                    </div>';
                            Html += '                    <div class="col-xl-4">';
                            Html += '                        <div class="form-group mb-0">';
                            Html += '                    <input type="text" class="form-control" placeholder="Enter Special Notes" value="' + vals.SpecialNote + '" onfocusout="fnChangeSpecialNote(' + vals.Id + ', this)">';
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
                                Html += '                                    <a href="/Transactions/Lead/View_New?EncryptedId=' + vals.EncryptedId + '&PI=' + PI + '&PS=' + PS + '&ViewType=1" title="View Lead" class="list-social-link"><i class="far fa-eye"></i></a>';
                                Html += '                                   </li>';
                            }
                            if ($("#RightsEdit").val() == "true" || $("#RightsEdit").val() == true) {
                                Html += '                                   <li>';
                                Html += '                                    <a href="/Transactions/Lead/Form?mode=edit&EncryptedId=' + vals.EncryptedId + '&PI=' + PI + '&PS=' + PS + '&ViewType=1" title="Edit Lead" class="list-social-link"><i class="far fa-edit"></i></a>';
                                Html += '                                   </li>';
                            }
                            if ($("#RightsDelete").val() == "true" || $("#RightsDelete").val() == true) {
                                Html += '                                   <li>';
                                Html += '                                    <a href="#" onclick="deleteData(\'' + vals.Id + '\')" title="Delete Lead" class="list-social-link"><i class="fas fa-trash-alt"></i></a>';
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

                            $("#view-grid").append(Html);
                            Html = '';
                        }
                        else {

                            Html += '<tr>';
                            Html += '<td class="text-center">';
                            Html += '     <div class="custom-control custom-checkbox">';
                            Html += '         <input type="checkbox" class="custom-control-input singleCheckBoxAssign" onclick="return initializeSingleCheckBoxAssign(this);" value="' + vals.Id + '" id="ListCheck_' + vals.Id + '">';
                            Html += '         <label class="custom-control-label" for="ListCheck_' + vals.Id + '"></label>';
                            Html += '     </div>';
                            Html += ' </td>';
                            Html += ' <td class="text-center">';
                            if (vals.IsBookmarked == 1) {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkLead(' + vals.Id + ',0)" style="cursor:pointer"><i class="fas fa-star zoom" aria-hidden="true"></i></a></div>';
                            }
                            else {
                                Html += '                <div class="list-li wishlist"><a title="Bookmark" onclick="fnUpdateBookmarkLead(' + vals.Id + ',1)"><i class="far fa-star zoom"></i></a></div>';
                            }
                            Html += ' </td>';
                            Html += ' <td class="text-center">' + vals.RowNo + '</td>';
                            Html += ' <td>' + vals.LeadNo + '</td>';
                            Html += ' <td class="whitespace-rap">' + vals.LeadDate + ' ' + vals.Days + ' Days Ago</td>';
                            Html += ' <td class="whitespace-rap">' + vals.AssignToName + '</td>';
                            Html += ' <td>';
                            Html += '     <div class="tb-width-300">' + vals.SpecialNote + '</div>';
                            Html += ' </td>';
                            Html += ' <td class="whitespace-rap">' + vals.FullName + '</td>';
                            Html += ' <td>' + vals.MobileNo + '</td>';
                            Html += ' <td class="whitespace-rap">Residential Structures</td>';//to verify
                            Html += '<td>₹' + BudgetRangeval + '</td>';
                            Html += '<td>' + ProjectRequirementval + ' ' + vals.SuperBuildupAreaUnitName + '</td>';
                            Html += '<td>' + vals.NextFollowupDate + '</td>';
                            Html += '<td>' + vals.FollowupDate + '</td>';
                            Html += '<td class="text-center">';
                            Html += '    <a href="javascript:;" title="Property Detail" onclick="fnShowPropertyDetailPopup(' + vals.Id + ',3,' + vals.MappedProperty + ',' + vals.AutoMatchProperty + ',' + vals.PropertyDetail + ')" class="list-link">Properties</a>';
                            Html += '</td>';
                            Html += '<td>' + vals.LeadDetail + '</td>';
                            Html += ' <td class="text-center whitespace-rap">';
                            //Html += '<button type="button" class="btn btn-primary btn-sm" data-toggle="tooltip" style="margin-right: 10px" data-placement="top" title="" data-original-title="Convert"><i class="fas fa-sync"></i></button>';//to verify
                            if ($("#RightsView").val() == "true" || $("#RightsView").val() == true) {
                                Html += '<a href="/Transactions/Lead/View_New?EncryptedId=' + vals.EncryptedId + '&PI=' + PI + '&PS=' + PS + '&ViewType=2" class="btn btn-success btn-sm mr-1" data-toggle="tooltip" data-placement="top" title="View Lead" data-original-title="View"><i class="fas fa-eye"></i></a>';
                            }
                            if ($("#RightsEdit").val() == "true" || $("#RightsEdit").val() == true) {
                                Html += '<a href="/Transactions/Lead/Form?mode=edit&EncryptedId=' + vals.EncryptedId + '&PI=' + PI + '&PS=' + PS + '&ViewType=2"  class="btn btn-primary btn-sm mr-1" data-toggle="tooltip" data-placement="top" title="Edit Lead" data-original-title="Edit"><i class="fas fa-edit"></i></a>';
                            }
                            if ($("#RightsDelete").val() == "true" || $("#RightsDelete").val() == true) {
                                Html += '<button type="button" onclick="deleteData(\'' + vals.Id + '\')" class="btn btn-danger btn-sm mr-1" data-toggle="tooltip" data-placement="top" title="Delete Lead" data-original-title="Delete"><i class="fas fa-trash-alt"></i></button>';
                            }
                            Html += ' </td>';
                            Html += ' </tr>';

                            $("#view-listtBody").append(Html);
                            Html = '';
                        }
                    }
                    var Position = parseInt($("#position_hdn").val());
                    if (isNaN(Position)) {
                        Position = 0;
                    }
                    if (Position > 500) {
                        $("html, body").animate({ scrollTop: Position });
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

    setTimeout(function () {
        $(".preloader").hide();
    }, 5000);
}
function fnSetDropdown() {
    $('.LeadStatusId').select2({
        ajax: {
            url: '/Common/GetLeadStatusSearchList',
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
        placeholder: 'Select Lead Status',
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
                    Department: 'Presales,Admin'
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
    $("#pageIndex").val(1);
    fnBindList();
});

$("#RelevanceIds,#LeadStageIdUI,#PropertyForIdUI,#PropertyCategoryIdsUI" +
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
    $('#PropertyForIdUI').empty().trigger('change');
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
            url: '/Common/GetLeadStatusSearchList',
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
        placeholder: 'Select Lead Status',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
        multiple: true
    });

    $('#PropertyForIdUI').select2({
        ajax: {
            url: '/Common/GetPropertyForSearchList',
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
        placeholder: 'Select Lead Business Type',
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
                    Department: 'Presales'
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
        allowClear: false
    });
    $('#LeadAssignTo').select2({
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
                    Department: 'Presales'
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
        multiple: false
    });
}
//=========================================== Filters END =========================================//

//=========================================== For Update Fields Start =========================================//
//For Update Assign To
$(document).on('select2:select', '.AssignedToId', function () {
    var AssignedToId = !isNaN(parseInt($(this).val())) ? parseInt($(this).val()) : 0;
    var LeadId = $(this).attr("id").split("_")[1];
    var StatusId = !isNaN(parseInt($("#LeadStatusId_" + LeadId).val())) ? parseInt($("#LeadStatusId_" + LeadId).val()) : 0;
    fnUpdateAssignedToId(AssignedToId, LeadId, StatusId);
});
function fnUpdateAssignedToId(AssignedToId, LeadId, StatusId) {
    $.ajax({
        type: "POST",
        url: "/Transactions/Lead/UpdateAssignedTo",
        data: {
            LeadId: LeadId,
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

//For Update Lead Status
$(document).on('select2:select', '.LeadStatusId', function () {
    var LeadStatusId = !isNaN(parseInt($(this).val())) ? parseInt($(this).val()) : 0;
    var LeadId = $(this).attr("id").split("_")[1];
    var LeadStatusName = $("#LeadStatusId_" + LeadId + " option:selected").text();
    var SelectedLeadStatusId = $(this).attr("selectedid");
    var AssignedToId = $("#AssignedToId_" + LeadId).val();
    var QualifyDataStatus = $("#QualifyDataStatus_" + LeadId).val();
    if (LeadStatusName == "Qualify") {
        $("#QualifyLeadId").val(LeadId);
        $("#QualifyLeadStatusId").val(LeadStatusId);
        $("#QualifySelectedLeadStatusId").val(SelectedLeadStatusId);
        if (QualifyDataStatus == '1') {
            $("#QualifyConfirmationModal").modal('show');
        } else {
            swal("To Qualify this Lead, Please set the required parameters.", {
                icon: "error",
            });
            fnBindList();
        }

        //swal({
        //    title: 'Are you sure?',
        //    text: 'Once Qualified, it will convert into Opportunity!',
        //    icon: 'warning',
        //    buttons: true,
        //    dangerMode: true,
        //}).then((willConti) => {
        //    if (willConti) {
        //        fnUpdateLeadStatusId(LeadStatusId, LeadId, LeadStatusName);
        //    } else {
        //        $("#LeadStatusId_" + LeadId + "").val(selectedid).trigger('change');
        //    }
        //});
    }
    else {
        fnUpdateLeadStatusId(LeadStatusId, LeadId, LeadStatusName, 0, AssignedToId);
    }
});
function fnAcceptQualifyConfirmationModal() {
    var LeadStatusId = $("#QualifyLeadStatusId").val();
    var LeadId = $("#QualifyLeadId").val();
    var LeadStatusName = $("#LeadStatusId_" + LeadId + " option:selected").text();
    var AssignTo = $("#OppAssignedToId").val();
    var AssignedToId = $("#AssignedToId_" + LeadId).val();
    if (!AssignTo) {
        swal("Please Select Opportunity Assigned To", {
            icon: "error",
        });
        return false;
    }
    fnUpdateLeadStatusId(LeadStatusId, LeadId, LeadStatusName, AssignTo, AssignedToId);
    $('#OppAssignedToId').empty().trigger('change');
    $("#QualifyConfirmationModal").modal('hide');
}
function fnCancelQualifyConfirmationModal() {
    $("#LeadStatusId_" + $("#QualifyLeadId").val() + "").val($("#QualifySelectedLeadStatusId").val()).trigger('change');
    $("#QualifyLeadId").val(0);
    $("#QualifyLeadStatusId").val(0);
    $("#QualifySelectedLeadStatusId").val(0);
    $('#OppAssignedToId').empty().trigger('change');
    $("#QualifyConfirmationModal").modal('hide');
}
function fnUpdateLeadStatusId(LeadStatusId, LeadId, LeadStatusName, AssignTo, AssignedToId) {
    $.ajax({
        type: "POST",
        url: "/Transactions/Lead/UpdateLeadStatus",
        data: {
            LeadId: LeadId,
            LeadStatusId: LeadStatusId,
            LeadStatusName: LeadStatusName,
            AssignTo: AssignTo,
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
            url: "/Transactions/Lead/UpdateSpecialNote",
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
function fnUpdateBookmarkLead(Id, Flag) {
    $.ajax({
        type: "GET",
        url: "/Transactions/Lead/UpdateBookmark",
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
function fnShowPropertyDetailPopup(Id, Tab, MappedCount, MatchingCount, AllCount, FullName, MobileNo, Email) {
    $("#PropertyDetailPopupLeadId").val(Id);
    $("#MappedCount").html(MappedCount);
    $("#MatchingCount").html(MatchingCount);
    $("#AllCount").html(AllCount);

    if (MappedCount > 0) {
        $('#SharePropertyPopup').css("pointer-events", "auto");
    } else {
        $('#SharePropertyPopup').css("pointer-events", "none");
    }

    $('#SharePropertyPopup').hide();
    $("#SharePropertyPopup").attr("onclick", "fnBindShareMatchingPropertyList(" + Id + ',1,\'' + FullName + '\',\'' + MobileNo + '\',\'' + Email + '\'' + ")");

    $("#IsMapCategory").prop('checked', $("#IsMapCategoryhidden").val());
    $("#IsMapSubCategory").prop('checked', $("#IsMapSubCategoryhidden").val());
    $("#IsMapArea").prop('checked', $("#IsMapAreahidden").val());
    $("#IsMapProject").prop('checked', $("#IsMapProjecthidden").val());
    $("#IsMapBudget").prop('checked', $("#IsMapBudgethidden").val());

    fnChangePropertyDetailTab(Tab);

    $("#PropertyDetailModal").modal('show');
}
function fnChangePropertyDetailTab(Tab) {
    var Id = $("#PropertyDetailPopupLeadId").val();
    if (Tab == 1) {
        $("#Mappedtab-1").addClass('active');
        $("#Matchingtab-2").removeClass('active');
        $("#Alltab-3").removeClass('active');

        $("#PropertyDetailtab1").addClass('active show');
        $("#PropertyDetailtab2").removeClass('active show');
        $("#PropertyDetailtab3").removeClass('active show');

        fnBindMappedPropertyList(Id, Tab);
    }
    else if (Tab == 2) {
        $("#Mappedtab-1").removeClass('active');
        $("#Matchingtab-2").addClass('active');
        $("#Alltab-3").removeClass('active');

        $("#PropertyDetailtab1").removeClass('active show');
        $("#PropertyDetailtab2").addClass('active show');
        $("#PropertyDetailtab3").removeClass('active show');

        //$("#IsMapCategory").prop('checked', false);
        //$("#IsMapSubCategory").prop('checked', false);
        //$("#IsMapBudget").prop('checked', false);
        //$("#IsMapArea").prop('checked', false);
        //$("#IsMapProject").prop('checked', false);

        fnBindMatchingPropertyList(Id, Tab);
    }
    else {

        $("#Mappedtab-1").removeClass('active');
        $("#Matchingtab-2").removeClass('active');
        $("#Alltab-3").addClass('active');

        $("#PropertyDetailtab1").removeClass('active show');
        $("#PropertyDetailtab2").removeClass('active show');
        $("#PropertyDetailtab3").addClass('active show');

        fnBindAllPropertyList(Id, Tab);
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
        "filter": true,
        "orderMulti": false,
        "ordering": false,
        "order": [0, "desc"],
        "ajax": {
            "url": "/Transactions/Lead/GetPropertyDetailList",
            "type": "POST",
            "datatype": "json",
            "data": function (o) {
                //o.search = $("#search").val();
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
            , {
                mRender: function (data, type, full) {
                    return '<button class="btn btn-danger btn-sm sweet-1" onclick="deleteMappedPropertyDetail(\'' + full.PropertyDetailId + '\')"><i class="far fa-trash-alt"></i></button>';
                }
            }
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, "targets": [0] }
        ]
    });
}
function fnBindMatchingPropertyList(LeadId, TypeId) {
    var table = $('#TBLMatchingProperty').DataTable({
        "bDestroy": true,
        "pageLength": $('#PageSize').val(),
        "lengthChange": true,
        "paging": true,
        "autoWidth": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "ordering": false,
        "order": [0, "desc"],
        "ajax": {
            "url": "/Transactions/Lead/GetPropertyDetailList",
            "type": "POST",
            "datatype": "json",
            "data": function (o) {
                //o.search = $("#search").val();
                o.LeadId = LeadId;
                o.TypeId = TypeId;
                o.IsMapCategory = $("#IsMapCategory").is(':checked');
                o.IsMapSubCategory = $("#IsMapSubCategory").is(':checked');
                o.IsMapBudget = $("#IsMapBudget").is(':checked');
                o.IsMapArea = $("#IsMapArea").is(':checked');
                o.IsMapProject = $("#IsMapProject").is(':checked');
            }
        },
        "columns": [
            {
                mRender: function (data, type, full) {
                    if (full.PropertyDetailId > 0) {
                        return '<button type="button" class="btn btn-primary btn-sm" onclick="fnMapUnmapProperty(' + full.PropertyDetailId + ',' + LeadId + ',' + full.Id + ',2,' + TypeId + ')" data-toggle="tooltip" data-placement="top" title="" data-original-title="Convert">Unmap</button>';
                    }
                    else {
                        return '<button type="button" class="btn btn-primary btn-sm" onclick="fnMapUnmapProperty(' + full.PropertyDetailId + ',' + LeadId + ',' + full.Id + ',1,' + TypeId + ')" data-toggle="tooltip" data-placement="top" title="" data-original-title="Convert">Map</button>';
                    }
                }
            },
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
        ],
        "drawCallback": function () {
            $("#MatchingCount").html($('#TBLMatchingProperty').DataTable().page.info().recordsTotal);
        }
    });
}
$(".clsMatchingPropertyCheckBox").click(function (event) {
    var LeadId = $("#PropertyDetailPopupLeadId").val();
    fnBindMatchingPropertyList(LeadId, 2);
    //setTimeout(function () {
    //    $("#MatchingCount").html($('#TBLMatchingProperty').DataTable().page.info().recordsTotal);
    //}, 1200);
});

function fnBindAllPropertyList(LeadId, TypeId) {
    var table = $('#TBLAllProperty').DataTable({
        "bDestroy": true,
        "pageLength": $('#PageSize').val(),
        "lengthChange": true,
        "paging": true,
        "autoWidth": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "ordering": false,
        "order": [0, "desc"],
        "ajax": {
            "url": "/Transactions/Lead/GetPropertyDetailList",
            "type": "POST",
            "datatype": "json",
            "data": function (o) {
                //o.search = $("#search").val();
                o.LeadId = LeadId;
                o.TypeId = TypeId;
            }
        },
        "columns": [
            {
                mRender: function (data, type, full) {
                    if (full.PropertyDetailId > 0) {
                        return '<button type="button" class="btn btn-primary btn-sm" onclick="fnMapUnmapProperty(' + full.PropertyDetailId + ',' + LeadId + ',' + full.Id + ',2,' + TypeId + ')" data-toggle="tooltip" data-placement="top" title="" data-original-title="Convert">Unmap</button>';
                    }
                    else {
                        return '<button type="button" class="btn btn-primary btn-sm" onclick="fnMapUnmapProperty(' + full.PropertyDetailId + ',' + LeadId + ',' + full.Id + ',1,' + TypeId + ')" data-toggle="tooltip" data-placement="top" title="" data-original-title="Convert">Map</button>';
                    }
                }
            },
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
function deleteMappedPropertyDetail(id) {
    swal({
        title: 'Are you sure?',
        text: 'Once deleted, you will not be able to recover this Record!',
        icon: 'warning',
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                fnDeleteMappedPropertyDetailFromDB(id, 1);
            } else {
            }
        });
}
function fnDeleteMappedPropertyDetailFromDB(id, DisplayMessage) {
    $.ajax({
        type: "GET",
        url: '/Transactions/Lead/DeleteMappedPropertyDetail',
        async: true,
        cache: false,
        data: {
            id: id,
        },
        success: function (data) {
            if (DisplayMessage == 1) {
                swal(data.message, {
                    icon: data.messageType,
                });
                var MappedCount = parseInt($("#MappedCount").html());
                $("#MappedCount").html(MappedCount - 1);
                if (parseInt($("#MappedCount").html()) > 0) {
                    $('#SharePropertyPopup').css("pointer-events", "auto");
                } else {
                    $('#SharePropertyPopup').css("pointer-events", "none");
                }
                $('#TBLMappedProperty').DataTable().draw();
                fnBindList();
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}
function fnMapUnmapProperty(id, LeadId, PropertyId, Type, Tab) {
    $.ajax({
        type: "GET",
        url: '/Transactions/Lead/MapUnmapLeadProperty',
        async: true,
        cache: false,
        data: {
            id: id,
            LeadId: LeadId,
            PropertyId: PropertyId,
            Type: Type
        },
        success: function (data) {
            swal(data.message, {
                icon: data.messageType,
            });
            var MappedCount = parseInt($("#MappedCount").html());
            if (Type == 1) {
                $("#MappedCount").html(MappedCount + 1);
            }
            else {
                $("#MappedCount").html(MappedCount - 1);
            }

            if (parseInt($("#MappedCount").html()) > 0) {
                $('#SharePropertyPopup').css("pointer-events", "auto");
            } else {
                $('#SharePropertyPopup').css("pointer-events", "none");
            }

            if (Tab == 2) {
                $('#TBLMatchingProperty').DataTable().draw();
            } else {
                $('#TBLAllProperty').DataTable().draw();
            }
            fnBindList();
        },
        error: function (response) {
            console.log(response);
        }
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


function fnCheckIsDeleteLeadDetailFromDB(id) {
    swal({
        title: 'Are you sure?',
        text: 'Once deleted, you will not be able to recover this Record!',
        icon: 'warning',
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                fnDeleteLeadDetailFromDB(id, 1);
            } else {
            }
        });
}
function fnDeleteLeadDetailFromDB(id, DisplayMessage) {
    $.ajax({
        type: "GET",
        url: '/Transactions/Lead/Delete',
        async: true,
        cache: false,
        data: {
            id: id,
        },
        success: function (data) {
            if (DisplayMessage == 1) {
                swal(data.message, {
                    icon: data.messageType,
                });
                fnBindList();
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function deleteData(id) {
    $.ajax({
        type: "GET",
        url: '/Transactions/Lead/IsDelete',
        async: true,
        cache: false,
        data: {
            id: id,
        },
        success: function (data) {
            returnval = data.messageType;
            if (data.messageType == "success") {
                fnCheckIsDeleteLeadDetailFromDB(id);
            } else {
                swal(data.message, {
                    icon: data.messageType,
                });
            }
        },
        error: function (response) {
            console.log(response);
        }
    });

}
//=========================================== Share Matching Property Start ===========================================//

function fnBindShareMatchingPropertyList(LeadId, TypeId, FullName, MobileNo, Email) {

    window.open("/Transactions/Property/ShareProperty?Type=Lead&Id=" + LeadId, "_blank");
    //window.location.href = "/Transactions/Property/ShareProperty?Type=Lead&Id=" + LeadId

    //$("#TBLShareMatchingProperty").addClass('active show');

    //$("#CustomerName").html(FullName);
    //$("#UserMobileNo").val(MobileNo);
    //$("#UserEmailAddress").val(Email);
    //$("#PropertyPopupLeadId").val(LeadId);

    //var table = $('#TBLShareMatchingProperty').DataTable({
    //    "bDestroy": true,
    //    "pageLength": $('#PageSize').val(),
    //    "lengthChange": true,
    //    "paging": true,
    //    "autoWidth": true,
    //    "processing": true,
    //    "serverSide": true,
    //    "filter": false,
    //    "orderMulti": false,
    //    "ordering": false,
    //    "order": [0, "desc"],
    //    "select": {
    //        style: 'os',
    //        selector: 'td:first-child'
    //    },
    //    "ajax": {
    //        "url": "/Transactions/Lead/GetPropertyDetailList",
    //        "type": "POST",
    //        "datatype": "json",
    //        "data": function (o) {
    //            o.search = $("#search").val();
    //            o.LeadId = LeadId;
    //            o.TypeId = TypeId;
    //        }
    //    },
    //    "columns": [
    //        {
    //            mRender: function (data, type, full) {
    //                return '<input type="checkbox" class="singleCheckBoxShare" data-value="' + full.Brochure + '" PropertyID="' + full.PropertyID + '" CategoryName="' + full.CategoryName + '" SubCategoryName="' + full.SubCategoryName + '" ProjectName="' + full.ProjectName + '" AreaName="' + full.AreaName + '" SuperBuildupArea="' + full.SuperBuildupArea + ' ' + full.SuperBuildupAreaUnitName + '" OfferPrice="' + full.OfferPrice + '" onclick="return initializeSingleCheckBoxShare(this);" data-id = "' + full.Id + '" id="' + full.Id + '" name="HiddenId" value="id_' + full.Id + '" />';
    //            }
    //        },
    //        { "data": "RowNo", "autoWidth": true },
    //        { "data": "PropertyID", "autoWidth": true },
    //        { "data": "AvailabilityName", "autoWidth": true },
    //        { "data": "CategoryName", "autoWidth": true },
    //        { "data": "SubCategoryName", "autoWidth": true },
    //        { "data": "PropertyFor", "autoWidth": true },
    //        { "data": "PropertyBHKName", "autoWidth": true },
    //        { "data": "AreaName", "autoWidth": true },
    //        {
    //            mRender: function (data, type, full) {
    //                return full.SuperBuildupArea + ' ' + full.SuperBuildupAreaUnitName;
    //            }
    //        },
    //        {
    //            mRender: function (data, type, full) {
    //                return full.CarpetArea + ' ' + full.CarpetAreaUnitName;
    //            }
    //        },
    //        { "data": "ProjectName", "autoWidth": true },
    //        { "data": "Date", "autoWidth": true },
    //        { "data": "PropertyStatusName", "autoWidth": true },
    //        { "data": "PropertyFurnishStatusName", "autoWidth": true },
    //        { "data": "PropertyAvailableFor", "autoWidth": true },
    //        {
    //            mRender: function (data, type, full) {
    //                return '<a target="_blank" href="' + full.Brochure + '" title="View" class="btn btn-success btn-sm"><i class="fas fa-eye"></i></a>';
    //            }
    //        }
    //    ],
    //    "responsive": true,
    //    "columnDefs": [
    //        { orderable: false, "targets": [0] }
    //    ],
    //    "drawCallback": function () {
    //        var RowCount = $('#TBLShareMatchingProperty').DataTable().page.info().recordsTotal;
    //        if (RowCount > 0) {
    //            $("#allCheckBoxShare").removeAttr("hidden");
    //            $("#allCheckBoxShare").prop('checked', true);
    //            initializeAllCheckBoxShare();
    //        } else {
    //            $("#allCheckBoxShare").prop('checked', false);
    //            $("#allCheckBoxShare").prop("hidden", true);
    //            $(".custom-radio").css("pointer-events", "none");
    //            $("#EmailCheck").prop('checked', false);
    //            $("#WhatsAppCheck").prop('checked', false);
    //        }
    //    }
    //});

    //$("#ShareMatchingPropertyDetailModal").modal('show');
}

//function initializeAllCheckBoxShare() {
//    var n = $("#allCheckBoxShare").is(":checked");
//    $("#TBLShareMatchingProperty .singleCheckBoxShare").prop("checked", n ? !0 : !1);
//    $("#TBLShareMatchingProperty .singleCheckBoxShare").closest("tr").addClass(n ? "selected-row" : "not-selected-row");
//    $("#TBLShareMatchingProperty .singleCheckBoxShare").closest("tr").removeClass(n ? "not-selected-row" : "selected-row");
//    if (n == true) {
//        $(".custom-radio").css("pointer-events", "visible");
//    } else {
//        $(".custom-radio").css("pointer-events", "none");
//    }

//}

//function initializeSingleCheckBoxShare(n) {
//    var t = $(n).is(":checked");
//    $(n).closest("tr").addClass(t ? "selected-row" : "not-selected-row");
//    $(n).closest("tr").removeClass(t ? "not-selected-row" : "selected-row");
//    t && $("#TBLShareMatchingProperty .singleCheckBoxShare").length == $("#TBLShareMatchingProperty .selected-row").length ? $("#allCheckBoxShare").prop("checked", !0) : $("#allCheckBoxShare").prop("checked", !1);

//    var CheckCount = 0;
//    $(".singleCheckBoxShare").each(function () {
//        if ($(this).is(":checked")) {
//            CheckCount = CheckCount + 1;
//        }
//    });

//    if (CheckCount > 0) {
//        $(".custom-radio").css("pointer-events", "visible");
//    } else {
//        $(".custom-radio").css("pointer-events", "none");
//    }
//}

//function fnWhatsAppShare() {
//    $("#EmailCheck").prop('checked', false);
//    var FinalURL = "";
//    $(".singleCheckBoxShare").each(function () {
//        if ($(this).is(":checked")) {
//            var DataURL = $("#URLDomainPath").val() + $(this).data("value");
//            var PropertyID = $(this).attr("PropertyID");
//            var CategoryName = $(this).attr("CategoryName");
//            var SubCategoryName = $(this).attr("SubCategoryName");
//            var ProjectName = $(this).attr("ProjectName");
//            var AreaName = $(this).attr("AreaName");
//            var SuperBuildupArea = $(this).attr("SuperBuildupArea");
//            var OfferPrice = $(this).attr("OfferPrice");

//            var URL = "https://tinyurl.com/api-create.php?url=" + DataURL;

//            var xmlHttp = new XMLHttpRequest();
//            xmlHttp.open("GET", URL, false);
//            xmlHttp.send(null);
//            URL = xmlHttp.responseText;

//            if (FinalURL == "") {
//                var message = "Property Id : " + PropertyID + " %0D%0A";
//                message += "Property Category : " + CategoryName + " %0D%0A";
//                message += "Property Subcategory: " + SubCategoryName + " %0D%0A";
//                message += "Project Name: " + ProjectName + " %0D%0A";
//                message += "Area: " + AreaName + " %0D%0A";
//                message += "Super Buildup Area: " + SuperBuildupArea + " %0D%0A";
//                message += "Price : " + OfferPrice + " %0D%0A";
//                message += "Brochure: " + URL;
//                //FinalURL += URL;
//                FinalURL += message;
//            }
//            else {
//                var message = "Property Id : " + PropertyID + " %0D%0A";
//                message += "Property Category : " + CategoryName + " %0D%0A";
//                message += "Property Subcategory: " + SubCategoryName + " %0D%0A";
//                message += "Project Name: " + ProjectName + " %0D%0A";
//                message += "Area: " + AreaName + " %0D%0A";
//                message += "Super Buildup Area: " + SuperBuildupArea + " %0D%0A";
//                message += "Price : " + OfferPrice + " %0D%0A";
//                message += "Brochure: " + URL;
//                //FinalURL += ' %0a ' + URL;//Uttam Work
//                //FinalURL += ' %0D%0A' + URL;
//                FinalURL += ' %0D%0A %0D%0A' + message;
//            }
//        }
//    });
//    //var href = "https://www.addtoany.com/add_to/whatsapp?linkurl=" + FinalURL + "&linknote=";//Uttam Work
//    //var href = "https://wa.me/" + $("#UserMobileNo").val() +"?text=" + FinalURL;
//    //window.open(href, "myWindowName", "width=800, height=600");//Uttam Work

//    var FinalMessage = "https://wa.me/+91" + $("#UserMobileNo").val() + "?text=" + FinalURL +
//        "%0D%0A %0D%0A" + $("#SharePropertyCompanyName").val() + "%0D%0A" + $("#CompanyHostingURL").val()
//        + "%0D%0A*Note:- Please add this contact in your device for easy access";
//    window.open(FinalMessage, "myWindowName", "width=800, height=600");

//    return false;
//}

//function fnEmailShare() {
//    $("#WhatsAppCheck").prop('checked', false);
//    var Ids = "";
//    $(".singleCheckBoxShare").each(function () {

//        var Id = $(this).data("id");
//        if ($(this).is(":checked")) {
//            if (Ids == "") {
//                Ids += Id;
//            }
//            else {
//                Ids += ',' + Id;
//            }
//        }
//    });

//    var UserEmail = $("#UserEmailAddress").val();
//    var LeadId = $("#PropertyPopupLeadId").val();

//    if (UserEmail != "") {
//        $(".preloader").fadeIn("slow");
//        $.ajax({
//            type: "POST",
//            url: '/Transactions/Lead/LeadShareEmailSend',
//            async: !0,
//            cache: !1,
//            data: {
//                LeadId: LeadId,
//                Ids: Ids,
//                UserEmail: UserEmail
//            },
//            success: function (data) {
//                if (data.IsEmailValidate == false) {
//                    $(".preloader").hide();
//                    if (data.message == "No details found in email setting") {
//                        const el = document.createElement('div')
//                        el.innerHTML = "Click <a href='/Masters/SettingEmail/List' target='_blank'>here</a> to set SMTP Setting"

//                        swal(data.message, {
//                            icon: data.messageType,
//                            content: el,
//                        });
//                    } else {
//                        swal(data.message, {
//                            icon: data.messageType
//                        });
//                    }

//                    $('#ShareMatchingPropertyDetailModal').modal('hide');
//                    return false;
//                } else {
//                    $(".preloader").hide();
//                    swal(data.message, {
//                        icon: data.messageType
//                    });
//                    $('#ShareMatchingPropertyDetailModal').modal('hide');
//                }
//            },
//            error: function (response) {
//                $(".preloader").hide();
//                console.log(response);
//            }

//        });
//    } else {
//        swal("Email Address Not Set!", {
//            icon: "error",
//        });
//    }
//}
//=========================================== Share Matching Property End ===========================================//


//=========================================== Lead Assign End ===========================================//
function initializeAllCheckBoxAssign() {
    var n = $("#allCheckBoxAssign").is(":checked");
    $("#TBL .singleCheckBoxAssign").prop("checked", n ? !0 : !1);
    $("#TBL .singleCheckBoxAssign").closest("tr").addClass(n ? "selected-row" : "not-selected-row");
    $("#TBL .singleCheckBoxAssign").closest("tr").removeClass(n ? "not-selected-row" : "selected-row");
    if (n == true) {
        $(".custom-radio").css("pointer-events", "visible");
    } else {
        $(".custom-radio").css("pointer-events", "none");
    }

}


function initializeSingleCheckBoxAssign(n) {
    var t = $(n).is(":checked");
    $(n).closest("tr").addClass(t ? "selected-row" : "not-selected-row");
    $(n).closest("tr").removeClass(t ? "not-selected-row" : "selected-row");
    t && $("#TBL .singleCheckBoxAssign").length == $("#TBL .selected-row").length ? $("#allCheckBoxAssign").prop("checked", !0) : $("#allCheckBoxAssign").prop("checked", !1);

    var CheckCount = 0;
    $(".singleCheckBoxAssign").each(function () {
        if ($(this).is(":checked")) {
            CheckCount = CheckCount + 1;
        }
    });

    if (CheckCount > 0) {
        $(".custom-radio").css("pointer-events", "visible");
    } else {
        $(".custom-radio").css("pointer-events", "none");
    }
}

function fnLeadAssignTo() {
    $("#LeadIds").val("");
    var FinalString = "";
    $(".singleCheckBoxAssign").each(function () {
        if ($(this).is(":checked")) {
            if (FinalString == "") {
                FinalString = $(this).val();
            } else {
                FinalString = FinalString + "," + $(this).val();
            }
        }
    });

    if (FinalString != "") {
        $('#LeadAssignTo').empty().trigger('change');
        $('#LeadAssignToModal').modal('show');
        $("#LeadIds").val(FinalString);
    } else {
        swal("Please Select Atleast One Record", {
            icon: "info",
        });
    }
}
function AssignToSelectedRows() {

    LeadIds = $("#LeadIds").val();
    LeadAssignTo = $("#LeadAssignTo").val();
    if (!LeadAssignTo) {
        swal("Please Select Assign To.", {
            icon: "error",
        });
        return false;
    }

    swal({
        title: 'Are you sure?',
        text: 'Are you sure want to assign these leads?',
        icon: 'info',
        buttons: true,
        dangerMode: true,
    }).then((willAssign) => {
        if (willAssign) {
            $.ajax({
                type: "POST",
                url: '/Transactions/Lead/AssignMultiple',
                async: false,
                cache: false,
                data: {
                    LeadIds: LeadIds,
                    AssignToId: LeadAssignTo,
                },
                success: function (data) {
                    swal(data.message, {
                        icon: data.messageType,
                    });
                    $('#LeadAssignToModal').modal('hide');
                    fnBindList();
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
    });
}

//=========================================== Lead Assign End ===========================================//