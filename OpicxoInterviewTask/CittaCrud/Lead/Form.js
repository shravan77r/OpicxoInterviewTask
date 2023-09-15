$(function () {
    fnAwaitBindSelectDropdowns();

    if ($(".PropertyForId_hdn").val() != "" && $(".PropertyForId_hdn").val() != "{}") {
        var Items = JSON.parse($(".PropertyForId_hdn").val());
        $("#PropertyForIdUI").val(Object.keys(Items)[0]).trigger('change');
        $("#QuickPropertyForIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($(".PropertyCategoryIds_hdn").val() != "" && $(".PropertyCategoryIds_hdn").val() != "{}") {
        var Items = JSON.parse($(".PropertyCategoryIds_hdn").val());
        $("#PropertyCategoryIdsUI").val(Object.keys(Items)[0]).trigger('change');
        $("#QuickPropertyCategoryIdsUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#PropertySubcategoryIds_hdn").val() != "" && $("#PropertySubcategoryIds_hdn").val() != "{}") {
        var PropertySubcategoryIds_hdn = $("#PropertySubcategoryIds_hdn").val();
        if (PropertySubcategoryIds_hdn != null && PropertySubcategoryIds_hdn != "") {
            $("#PropertySubcategoryIdsUI").val(PropertySubcategoryIds_hdn.split(',')).trigger("change");
        }
    }

    if ($("#PreferredProjectIds_hdn").val() != "" && $("#PreferredProjectIds_hdn").val() != "{}") {
        var PreferredProjectIds_hdn = $("#PreferredProjectIds_hdn").val();
        if (PreferredProjectIds_hdn != null && PreferredProjectIds_hdn != "") {
            $("#PreferredProjectIdsUI").val(PreferredProjectIds_hdn.split(',')).trigger("change");
        }
    }

    if ($("#PropertyZoneIds_hdn").val() != "" && $("#PropertyZoneIds_hdn").val() != "{}") {
        var Items = JSON.parse($("#PropertyZoneIds_hdn").val());
        $("#PropertyZoneIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#Salutation_hdn").val() != "" && $("#Salutation_hdn").val() != "{}") {
        var Items = JSON.parse($("#Salutation_hdn").val());
        $("#SalutationUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($(".CountryId_hdn").val() != "" && $(".CountryId_hdn").val() != "{}") {
        var Items = JSON.parse($(".CountryId_hdn").val());
        $("#CountryIdUI").val(Object.keys(Items)[0]).trigger('change');
        $("#QuickCountryIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($(".StateId_hdn").val() != "" && $(".StateId_hdn").val() != "{}") {
        var Items = JSON.parse($(".StateId_hdn").val());
        $("#StateIdUI").val(Object.keys(Items)[0]).trigger('change');
        $("#QuickStateIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($(".CityId_hdn").val() != "" && $(".CityId_hdn").val() != "{}") {
        var Items = JSON.parse($(".CityId_hdn").val());
        $("#CityIdUI").val(Object.keys(Items)[0]).trigger('change');
        $("#QuickCityIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#BHKIds_hdn").val() != "" && $("#BHKIds_hdn").val() != "{}") {
        var BHKIds_hdn = $("#BHKIds_hdn").val();
        if (BHKIds_hdn != null && BHKIds_hdn != "") {
            $("#BHKIdsUI").val(BHKIds_hdn.split(',')).trigger("change");
            $("#QuickBHKIdsUI").val(BHKIds_hdn.split(',')).trigger("change");
        }
    }

    if ($("#FloorIds_hdn").val() != "" && $("#FloorIds_hdn").val() != "{}") {
        var FloorIds_hdn = $("#FloorIds_hdn").val();
        if (FloorIds_hdn != null && FloorIds_hdn != "") {
            $("#FloorIdsUI").val(FloorIds_hdn.split(',')).trigger("change");
        }
    }

    if ($("#FurnishStatusIds_hdn").val() != "" && $("#FurnishStatusIds_hdn").val() != "{}") {
        var FurnishStatusIds_hdn = $("#FurnishStatusIds_hdn").val();
        if (FurnishStatusIds_hdn != null && FurnishStatusIds_hdn != "") {
            $("#FurnishStatusIdsUI").val(FurnishStatusIds_hdn.split(',')).trigger("change");
            $("#QuickFurnishStatusIdsUI").val(FurnishStatusIds_hdn.split(',')).trigger("change");
        }
    }

    if ($("#DoorFacingDirectionId_hdn").val() != "" && $("#DoorFacingDirectionId_hdn").val() != "{}") {
        var DoorFacingDirectionId_hdn = $("#DoorFacingDirectionId_hdn").val();
        if (DoorFacingDirectionId_hdn != null && DoorFacingDirectionId_hdn != "") {
            $("#DoorFacingDirectionIdsUI").val(DoorFacingDirectionId_hdn.split(',')).trigger("change");
        }
    }

    if ($(".AvailableForIds_hdn").val() != "" && $(".AvailableForIds_hdn").val() != "{}") {
        var Items = JSON.parse($("#AvailableForIds_hdn").val());
        $("#AvailableForIdUI").val(Object.keys(Items)[0]).trigger('change');
        $("#QuickAvailableForIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#ProjectTypeIds_hdn").val() != "" && $("#ProjectTypeIds_hdn").val() != "{}") {
        var Items = JSON.parse($("#ProjectTypeIds_hdn").val());
        $("#ProjectTypeIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#SuperBuildupAreaUnitId_hdn").val() != "" && $("#SuperBuildupAreaUnitId_hdn").val() != "{}") {
        var Items = JSON.parse($("#SuperBuildupAreaUnitId_hdn").val());
        $("#SuperBuildupAreaUnitIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#CarpetAreaUnitId_hdn").val() != "" && $("#CarpetAreaUnitId_hdn").val() != "{}") {
        var Items = JSON.parse($("#CarpetAreaUnitId_hdn").val());
        $("#CarpetAreaUnitIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#AssignTo_hdn").val() != "" && $("#AssignTo_hdn").val() != "{}") {
        var Items = JSON.parse($("#AssignTo_hdn").val());
        $("#AssignToUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($(".LeadSourceId_hdn").val() != "" && $(".LeadSourceId_hdn").val() != "{}") {
        var Items = JSON.parse($(".LeadSourceId_hdn").val());
        $("#LeadSourceIdUI").val(Object.keys(Items)[0]).trigger('change');
        $("#QuickLeadSourceIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#LeadSubsourceId_hdn").val() != "" && $("#LeadSubsourceId_hdn").val() != "{}") {
        var Items = JSON.parse($("#LeadSubsourceId_hdn").val());
        $("#LeadSubsourceIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#LeadStageId_hdn").val() != "" && $("#LeadStageId_hdn").val() != "{}") {
        var Items = JSON.parse($("#LeadStageId_hdn").val());
        $("#LeadStageIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#LeadPriorityId_hdn").val() != "" && $("#LeadPriorityId_hdn").val() != "{}") {
        var Items = JSON.parse($("#LeadPriorityId_hdn").val());
        $("#LeadPriorityIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#LeadLostReasonId_hdn").val() != "" && $("#LeadLostReasonId_hdn").val() != "{}") {
        var Items = JSON.parse($("#LeadLostReasonId_hdn").val());
        $("#LeadLostReasonIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($("#LeadScoreId_hdn").val() != "" && $("#LeadScoreId_hdn").val() != "{}") {
        var Items = JSON.parse($("#LeadScoreId_hdn").val());
        $("#LeadScoreIdUI").val(Object.keys(Items)[0]).trigger('change');
    }

    if ($(".AreaIds_hdn").val() != "" && $(".AreaIds_hdn").val() != "{}") {
        var AreaIds_hdn = $(".AreaIds_hdn").val();
        if (AreaIds_hdn != null && AreaIds_hdn != "") {
            $("#AreaIdUI").val(AreaIds_hdn.split(',')).trigger("change");
            $("#QuickAreaIdUI").val(AreaIds_hdn.split(',')).trigger("change");
        }
    }

});
$(document).ready(function () {
    $('.PropertyCategoryIdsUI').on('select2:unselect select2:select', function (e) {
        $('#PropertySubcategoryIdsUI').empty().trigger('change');
    });

    $('#LeadSourceIdUI').on('select2:unselect select2:select', function (e) {
        $('#LeadSubsourceIdUI').empty().trigger('change');
        $('#QuickLeadSourceIdUI').append(new Option($("#LeadSourceIdUI option:selected").text(), $("#LeadSourceIdUI option:selected").val(), true, true));
    });

    $('#QuickLeadSourceIdUI').on('select2:unselect select2:select', function (e) {
        $('#LeadSubsourceIdUI').empty().trigger('change');
        $('#LeadSourceIdUI').append(new Option($("#QuickLeadSourceIdUI option:selected").text(), $("#QuickLeadSourceIdUI option:selected").val(), true, true));
    });

    $('.CountryIdUI').on('select2:unselect select2:select', function (e) {
        $('.StateIdUI').empty().trigger('change');
        $('.CityIdUI').empty().trigger('change');
        $('.AreaIdUI').empty().trigger('change');
    });
    $('.StateIdUI').on('select2:unselect select2:select', function (e) {
        $('.CityIdUI').empty().trigger('change');
        $('.AreaIdUI').empty().trigger('change');
    });
    $('.CityIdUI').on('select2:unselect select2:select', function (e) {
        $('.AreaIdUI').empty().trigger('change');
    });

    $("#PropertySubcategoryIds_hdn").val($("#PropertySubcategoryIdsUI").val());
    $("#PreferredProjectIds_hdn").val($("#PreferredProjectIdsUI").val());
    $("#AreaIds_hdn").val($("#AreaIdUI").val());
    $("#BHKIds_hdn").val($("#BHKIdsUI").val());
    $("#FloorIds_hdn").val($("#FloorIdsUI").val());
    $("#FurnishStatusIds_hdn").val($("#FurnishStatusIdsUI").val());
    $("#DoorFacingDirectionId_hdn").val($("#DoorFacingDirectionIdsUI").val());

    if ($('#Id').val() > 0 && $('#LeadStageIdUI option:selected').text().toLowerCase() == "qualify") {
        $('#QuickLeadDate').attr("readonly", true);
        $('#LeadDate').attr("readonly", true);
        $('#QuickPropertyForIdUI').attr("disabled", true);
        $('#PropertyForIdUI').attr("disabled", true);
        $('#QuickPropertyCategoryIdsUI').attr("disabled", true);
        $('#PropertyCategoryIdsUI').attr("disabled", true);
        $('#AssignToUI').attr("disabled", true);
        $('#LeadStageIdUI').attr("disabled", true);
    }

});

function LeadDateIndexChanged() {
    $("#QuickLeadDate").val($("#LeadDate").val());
}
function QuickLeadDateIndexChanged() {
    $("#LeadDate").val($("#QuickLeadDate").val());
}

function SubcategorySelectedIndexChanged() {
    $("#PropertySubcategoryIds_hdn").val($("#PropertySubcategoryIdsUI").val());
}
function PreferredProjectSelectedIndexChanged() {
    $("#PreferredProjectIds_hdn").val($("#PreferredProjectIdsUI").val());
}
function AreaSelectedIndexChanged() {
    $("#AreaIds_hdn").val($("#AreaIdUI").val());

    var AreaIds = $("#AreaIdUI").val();
    $('#QuickAreaIdUI').empty();

    $("#AreaIdUI option").each(function () {
        if (AreaIds.includes($(this).val())) {
            $('#QuickAreaIdUI').append(new Option($(this).text(), $(this).val(), true, true));
        }
    });
}
function QuickAreaSelectedIndexChanged() {
    debugger
    $("#AreaIds_hdn").val($("#QuickAreaIdUI").val());

    var AreaIds = $("#QuickAreaIdUI").val();
    $('#AreaIdUI').empty();

    $("#QuickAreaIdUI option").each(function () {
        if (AreaIds.includes($(this).val())) {
            $('#AreaIdUI').append(new Option($(this).text(), $(this).val(), true, true));
        }
    });
}
function BHKSelectedIndexChanged() {
    $("#BHKIds_hdn").val($("#BHKIdsUI").val());

    var BHKIds = $("#BHKIdsUI").val();
    $('#QuickBHKIdsUI').empty();

    $("#BHKIdsUI option").each(function () {
        if (BHKIds.includes($(this).val())) {
            $('#QuickBHKIdsUI').append(new Option($(this).text(), $(this).val(), true, true));
        }
    });
}
function QuickBHKSelectedIndexChanged() {
    $("#BHKIds_hdn").val($("#QuickBHKIdsUI").val());

    var BHKIds = $("#QuickBHKIdsUI").val();
    $('#BHKIdsUI').empty();

    $("#QuickBHKIdsUI option").each(function () {
        if (BHKIds.includes($(this).val())) {
            $('#BHKIdsUI').append(new Option($(this).text(), $(this).val(), true, true));
        }
    });
}
function FloorSelectedIndexChanged() {
    $("#FloorIds_hdn").val($("#FloorIdsUI").val());
}
function FurnishStatusSelectedIndexChanged() {
    $("#FurnishStatusIds_hdn").val($("#FurnishStatusIdsUI").val());

    var FurnishStatusIds = $("#FurnishStatusIdsUI").val();
    $('#QuickFurnishStatusIdsUI').empty();

    $("#FurnishStatusIdsUI option").each(function () {
        if (FurnishStatusIds.includes($(this).val())) {
            $('#QuickFurnishStatusIdsUI').append(new Option($(this).text(), $(this).val(), true, true));
        }
    });
}

function QuickFurnishStatusSelectedIndexChanged() {
    $("#FurnishStatusIds_hdn").val($("#QuickFurnishStatusIdsUI").val());

    var FurnishStatusIds = $("#QuickFurnishStatusIdsUI").val();
    $('#FurnishStatusIdsUI').empty();

    $("#QuickFurnishStatusIdsUI option").each(function () {
        if (FurnishStatusIds.includes($(this).val())) {
            $('#FurnishStatusIdsUI').append(new Option($(this).text(), $(this).val(), true, true));
        }
    });
}

function DoorFacingDirectionSelectedIndexChanged() {
    $("#DoorFacingDirectionId_hdn").val($("#DoorFacingDirectionIdsUI").val());
}
async function fnAwaitBindSelectDropdowns() {
    await fnBindSelectDropdowns();
}

async function fnBindSelectDropdowns() {

    $('.PropertyForIdUI').select2({
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
    });

    $('.PropertyCategoryIdsUI').select2({
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
        placeholder: 'Select Property Category',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
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
                    CategoryIds: !isNaN(parseInt($('#PropertyCategoryIdsUI').val())) ? parseInt($('#PropertyCategoryIdsUI').val()) : 0,
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
        placeholder: 'Select Property Subcategory',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: true,
        multiple: true
    });

    $('#PreferredProjectIdsUI').select2({
        ajax: {
            url: '/Common/GetProjectSearchList',
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
        placeholder: 'Select Preferred Project',
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
    });

    $('#SalutationUI').select2({
        ajax: {
            url: '/Common/GetSalutationSearchList',
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
        placeholder: 'Select Salutation',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    $('.CountryIdUI').select2({
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

    $('.StateIdUI').select2({
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
                    CountryId: isNaN(parseInt($('#CountryIdUI').val())) ? 0 : $('#CountryIdUI').val()
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

    $('.CityIdUI').select2({
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
                    CountryId: isNaN(parseInt($('#CountryIdUI').val())) ? 0 : $('#CountryIdUI').val(),
                    StateId: isNaN(parseInt($('#StateIdUI').val())) ? 0 : $('#StateIdUI').val()
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

    $('.AreaIdUI').select2({
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
                    CountryId: isNaN(parseInt($('#CountryIdUI').val())) ? 0 : $('#CountryIdUI').val(),
                    StateId: isNaN(parseInt($('#StateIdUI').val())) ? 0 : $('#StateIdUI').val(),
                    CityId: isNaN(parseInt($('#CityIdUI').val())) ? 0 : $('#CityIdUI').val()
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
        allowClear: true,
        multiple: true
    });

    $('.BHKIdsUI').select2({
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
        allowClear: true,
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
        placeholder: 'Select Floor Preference',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: true,
        multiple: true
    });

    $('.FurnishStatusIdsUI').select2({
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
        allowClear: true,
        multiple: true
    });

    $('#DoorFacingDirectionIdsUI').select2({
        ajax: {
            url: '/Common/GetDoorFacingDirectionSearchList',
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
        placeholder: 'Select Door Facing Direction',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: true,
        multiple: true
    });

    $('.AvailableForIdUI').select2({
        ajax: {
            url: '/Common/GetPropertyAvailableForSearchList',
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
        placeholder: 'Select Tenant/Buyer Preference',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    $('#ProjectTypeIdUI').select2({
        ajax: {
            url: '/Common/GetProjectTypeSearchList',
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
        placeholder: 'Select Project Category',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#PropertyScoreIdUI').select2({
        ajax: {
            url: '/Common/GetPropertyScoreSearchList',
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
        placeholder: 'Select Property Score',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    $('#SuperBuildupAreaUnitIdUI,#CarpetAreaUnitIdUI').select2({
        ajax: {
            url: '/Common/GetUnitSearchList',
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
        placeholder: 'Select Unit',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false,
    });

    //$('#AssignToUI').select2({
    //    ajax: {
    //        url: '/Common/GetAssignToSearchList',
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
    //    placeholder: 'Select Assign To',
    //    minimumInputLength: minimumInputLength,
    //    width: '100%',
    //    allowClear: false
    //});

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
        allowClear: false
    });

    $('.LeadSourceIdUI').select2({
        ajax: {
            url: '/Common/GetLeadSourceSearchList',
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
        placeholder: 'Select Lead Source',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#LeadSubsourceIdUI').select2({
        ajax: {
            url: '/Common/GetLeadSubsourceSearchList',
            type: 'POST',
            dataType: 'json',
            cache: false,
            delay: 250,
            data: function (params) {
                return {
                    keyWord: params.term, // search term
                    pageIndex: params.page || 1,
                    pageSize: pageSize,
                    LeadSourceId: isNaN(parseInt($('#LeadSourceIdUI').val())) ? 0 : $('#LeadSourceIdUI').val(),
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
        placeholder: 'Select Lead Subsource',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

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
        allowClear: false
    });

    $('#LeadPriorityIdUI').select2({
        ajax: {
            url: '/Common/GetLeadPrioritySearchList',
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
        placeholder: 'Select Lead Interest',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });

    $('#LeadLostReasonIdUI').select2({
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

    $('#LeadScoreIdUI').select2({
        ajax: {
            url: '/Common/GetLeadScoreSearchList',
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
        placeholder: 'Select Lead Score',
        minimumInputLength: minimumInputLength,
        width: '100%',
        allowClear: false
    });
}

function BudgetFromTOValid() {
    var BudgetFrom = parseFloat($("#BudgetFrom").val());
    var BudgetTo = parseFloat($("#BudgetTo").val());

    if (isNaN(BudgetFrom)) {
        BudgetFrom = 0;
    }

    if (isNaN(BudgetTo)) {
        BudgetTo = 0;
    }
    if (BudgetFrom > 0) {
        if (BudgetTo == 0) {
            swal("Please Enter Valid Budget To.");
            $("#BudgetTo").focus();
            return false;
        } else if (BudgetTo > 0) {
            if (BudgetFrom > BudgetTo) {
                swal("Please Enter Valid Budget To.");
                $("#BudgetTo").val(0);
                $("#BudgetTo").focus();
                return false;
            }
        }
    } else {
        if (BudgetTo > 0) {
            swal("Please Enter Valid Budget From.");
            $("#BudgetFrom").focus();
            return false;
        }
    }


    var SuperBuildupAreaFrom = parseFloat($("#SuperBuildupAreaFrom").val());
    var SuperBuildupAreaTo = parseFloat($("#SuperBuildupAreaTo").val());

    if (isNaN(SuperBuildupAreaFrom)) {
        SuperBuildupAreaFrom = 0;
    }

    if (isNaN(SuperBuildupAreaTo)) {
        SuperBuildupAreaTo = 0;
    }
    if (SuperBuildupAreaFrom > 0) {
        if (SuperBuildupAreaTo == 0) {
            swal("Please Enter Valid Super Buildup Area To.");
            $("#SuperBuildupAreaTo").focus();
            return false;
        } else if (SuperBuildupAreaTo > 0) {
            if (SuperBuildupAreaFrom > SuperBuildupAreaTo) {
                swal("Please Enter Valid Super Buildup Area To.");
                $("#SuperBuildupAreaTo").val(0);
                $("#SuperBuildupAreaTo").focus();
                return false;
            }
        }
    } else {
        if (SuperBuildupAreaTo > 0) {
            swal("Please Enter Valid Super Buildup Area From.");
            $("#SuperBuildupAreaFrom").focus();
            return false;
        }
    }

    var CarpetAreaFrom = parseFloat($("#CarpetAreaFrom").val());
    var CarpetAreaTo = parseFloat($("#CarpetAreaTo").val());

    if (isNaN(CarpetAreaFrom)) {
        CarpetAreaFrom = 0;
    }

    if (isNaN(CarpetAreaTo)) {
        CarpetAreaTo = 0;
    }

    if (CarpetAreaFrom > 0) {
        if (CarpetAreaTo == 0) {
            swal("Please Enter Valid Carpet Area To.");
            $("#CarpetAreaTo").focus();
            return false;
        } else if (CarpetAreaTo > 0) {
            if (CarpetAreaFrom > CarpetAreaTo) {
                swal("Please Enter Valid Carpet Area To.");
                $("#CarpetAreaTo").val(0);
                $("#CarpetAreaTo").focus();
                return false;
            }
        }
    } else {
        if (CarpetAreaTo > 0) {
            swal("Please Enter Valid Carpet Area From.");
            $("#CarpetAreaFrom").focus();
            return false;
        }
    }

    if ($('#myform').valid()) {
        $('#QuickPropertyForIdUI').attr("disabled", false);
        $('#PropertyForIdUI').attr("disabled", false);
        $('#QuickPropertyCategoryIdsUI').attr("disabled", false);
        $('#PropertyCategoryIdsUI').attr("disabled", false);
        $('#AssignToUI').attr("disabled", false);
        $('#LeadStageIdUI').attr("disabled", false);
    }
}

$("#BudgetFrom").on('change', function () {

    var BudgetFrom = parseFloat($("#BudgetFrom").val());
    var BudgetTo = parseFloat($("#BudgetTo").val());

    if (isNaN(BudgetFrom)) {
        BudgetFrom = 0;
    }

    if (isNaN(BudgetTo)) {
        BudgetTo = 0;
    }
    if (BudgetFrom > 0) {
        if (BudgetTo == 0) {

        } else if (BudgetTo > 0) {
            if (BudgetFrom > BudgetTo) {
                swal("Please Enter Valid Budget To.");
                $("#BudgetTo").val(0);
                $("#BudgetTo").focus();
                return false;
            }
        }
    } else {
        if (BudgetTo > 0) {
            swal("Please Enter Valid Budget From.");
            $("#BudgetFrom").focus();
            return false;
        }
    }
    //if (BudgetTo > 0) {
    //    if (BudgetFrom > 0) {
    //        if (BudgetFrom > 0 && BudgetTo > 0) {
    //            if (BudgetFrom > BudgetTo) {
    //                swal("Please Enter Valid Budget To.");
    //                $("#BudgetTo").val(0);
    //                $("#BudgetTo").focus();
    //            }
    //        }
    //    } else {
    //        swal("Please Enter Budget From.");
    //        $("#BudgetTo").val(0);
    //    }
    //}
});

$("#BudgetTo").on('change', function () {
    var BudgetFrom = parseFloat($("#BudgetFrom").val());
    var BudgetTo = parseFloat($("#BudgetTo").val());

    if (isNaN(BudgetFrom)) {
        BudgetFrom = 0;
    }

    if (isNaN(BudgetTo)) {
        BudgetTo = 0;
    }
    if (BudgetFrom > 0) {
        if (BudgetTo == 0) {
            swal("Please Enter Valid Budget To.");
            $("#BudgetTo").focus();
            return false;
        } else if (BudgetTo > 0) {
            if (BudgetFrom > BudgetTo) {
                swal("Please Enter Valid Budget To.");
                $("#BudgetTo").val(0);
                $("#BudgetTo").focus();
                return false;
            }
        }
    } else {
        if (BudgetTo > 0) {
            swal("Please Enter Valid Budget From.");
            $("#BudgetFrom").focus();
            return false;
        }
    }
    //var BudgetFrom = parseFloat($("#BudgetFrom").val());
    //var BudgetTo = parseFloat($("#BudgetTo").val());

    //if (isNaN(BudgetFrom)) {
    //    BudgetFrom = 0;
    //}

    //if (isNaN(BudgetTo)) {
    //    BudgetTo = 0;
    //}

    //if (BudgetTo > 0) {
    //    if (BudgetFrom > 0) {
    //        if (BudgetFrom > 0 && BudgetTo > 0) {
    //            if (BudgetFrom > BudgetTo) {
    //                swal("Please Enter Valid Budget To.");
    //                $("#BudgetTo").val(0);
    //                $("#BudgetTo").focus();
    //            }
    //        }
    //    } else {
    //        swal("Please Enter Budget From.");
    //        $("#BudgetTo").val(0);
    //    }
    //}
});

$("#SuperBuildupAreaFrom").on('change', function () {

    var SuperBuildupAreaFrom = parseFloat($("#SuperBuildupAreaFrom").val());
    var SuperBuildupAreaTo = parseFloat($("#SuperBuildupAreaTo").val());

    if (isNaN(SuperBuildupAreaFrom)) {
        SuperBuildupAreaFrom = 0;
    }

    if (isNaN(SuperBuildupAreaTo)) {
        SuperBuildupAreaTo = 0;
    }
    if (SuperBuildupAreaFrom > 0) {
        if (SuperBuildupAreaTo == 0) {

        } else if (SuperBuildupAreaTo > 0) {
            if (SuperBuildupAreaFrom > SuperBuildupAreaTo) {
                swal("Please Enter Valid Super Buildup Area To.");
                $("#SuperBuildupAreaTo").val(0);
                $("#SuperBuildupAreaTo").focus();
                return false;
            }
        }
    } else {
        if (SuperBuildupAreaTo > 0) {
            swal("Please Enter Valid Super Buildup Area From.");
            $("#SuperBuildupAreaFrom").focus();
            return false;
        }
    }
    //if (SuperBuildupAreaTo > 0) {
    //    if (SuperBuildupAreaFrom > 0) {
    //        if (SuperBuildupAreaFrom > 0 && SuperBuildupAreaTo > 0) {
    //            if (SuperBuildupAreaFrom > SuperBuildupAreaTo) {
    //                swal("Please Enter Valid Super Buildup Area To.");
    //                $("#SuperBuildupAreaTo").val(0);
    //                $("#SuperBuildupAreaTo").focus();
    //            }
    //        }
    //    } else {
    //        swal("Please Enter Super Buildup Area From.");
    //        $("#SuperBuildupAreaTo").val(0);
    //    }
    //}
});

$("#SuperBuildupAreaTo").on('change', function () {

    var SuperBuildupAreaFrom = parseFloat($("#SuperBuildupAreaFrom").val());
    var SuperBuildupAreaTo = parseFloat($("#SuperBuildupAreaTo").val());

    if (isNaN(SuperBuildupAreaFrom)) {
        SuperBuildupAreaFrom = 0;
    }

    if (isNaN(SuperBuildupAreaTo)) {
        SuperBuildupAreaTo = 0;
    }
    if (SuperBuildupAreaFrom > 0) {
        if (SuperBuildupAreaTo == 0) {
            swal("Please Enter Valid Super Buildup Area To.");
            $("#SuperBuildupAreaTo").focus();
            return false;
        } else if (SuperBuildupAreaTo > 0) {
            if (SuperBuildupAreaFrom > SuperBuildupAreaTo) {
                swal("Please Enter Valid Super Buildup Area To.");
                $("#SuperBuildupAreaTo").val(0);
                $("#SuperBuildupAreaTo").focus();
                return false;
            }
        }
    } else {
        if (SuperBuildupAreaTo > 0) {
            swal("Please Enter Valid Super Buildup Area From.");
            $("#SuperBuildupAreaFrom").focus();
            return false;
        }
    }
    //if (SuperBuildupAreaTo > 0) {
    //    if (SuperBuildupAreaFrom > 0) {
    //        if (SuperBuildupAreaFrom > 0 && SuperBuildupAreaTo > 0) {
    //            if (SuperBuildupAreaFrom > SuperBuildupAreaTo) {
    //                swal("Please Enter Valid Super Buildup Area To.");
    //                $("#SuperBuildupAreaTo").val(0);
    //                $("#SuperBuildupAreaTo").focus();
    //            }
    //        }
    //    } else {
    //        swal("Please Enter Super Buildup Area From.");
    //        $("#SuperBuildupAreaTo").val(0);
    //    }
    //}
});

$("#CarpetAreaFrom").on('change', function () {

    var CarpetAreaFrom = parseFloat($("#CarpetAreaFrom").val());
    var CarpetAreaTo = parseFloat($("#CarpetAreaTo").val());

    if (isNaN(CarpetAreaFrom)) {
        CarpetAreaFrom = 0;
    }

    if (isNaN(CarpetAreaTo)) {
        CarpetAreaTo = 0;
    }
    if (CarpetAreaFrom > 0) {
        if (CarpetAreaTo == 0) {

        } else if (CarpetAreaTo > 0) {
            if (CarpetAreaFrom > CarpetAreaTo) {
                swal("Please Enter Valid Carpet Area To.");
                $("#CarpetAreaTo").val(0);
                $("#CarpetAreaTo").focus();
                return false;
            }
        }
    } else {
        if (CarpetAreaTo > 0) {
            swal("Please Enter Valid Carpet Area From.");
            $("#CarpetAreaFrom").focus();
            return false;
        }
    }
    //if (CarpetAreaTo > 0) {
    //    if (CarpetAreaFrom > 0) {
    //        if (CarpetAreaFrom > 0 && CarpetAreaTo > 0) {
    //            if (CarpetAreaFrom > CarpetAreaTo) {
    //                swal("Please Enter Valid Carpet Area To.");
    //                $("#CarpetAreaTo").val(0);
    //                $("#CarpetAreaTo").focus();
    //            }
    //        }
    //    } else {
    //        swal("Please Enter Carpet Area From.");
    //        $("#CarpetAreaTo").val(0);
    //    }
    //}
});

$("#CarpetAreaTo").on('change', function () {

    var CarpetAreaFrom = parseFloat($("#CarpetAreaFrom").val());
    var CarpetAreaTo = parseFloat($("#CarpetAreaTo").val());

    if (isNaN(CarpetAreaFrom)) {
        CarpetAreaFrom = 0;
    }

    if (isNaN(CarpetAreaTo)) {
        CarpetAreaTo = 0;
    }

    if (CarpetAreaFrom > 0) {
        if (CarpetAreaTo == 0) {
            swal("Please Enter Valid Carpet Area To.");
            $("#CarpetAreaTo").focus();
            return false;
        } else if (CarpetAreaTo > 0) {
            if (CarpetAreaFrom > CarpetAreaTo) {
                swal("Please Enter Valid Carpet Area To.");
                $("#CarpetAreaTo").val(0);
                $("#CarpetAreaTo").focus();
                return false;
            }
        }
    } else {
        if (CarpetAreaTo > 0) {
            swal("Please Enter Valid Carpet Area From.");
            $("#CarpetAreaFrom").focus();
            return false;
        }
    }
    //if (CarpetAreaTo > 0) {
    //    if (CarpetAreaFrom > 0) {
    //        if (CarpetAreaFrom > 0 && CarpetAreaTo > 0) {
    //            if (CarpetAreaFrom > CarpetAreaTo) {
    //                swal("Please Enter Valid Carpet Area To.");
    //                $("#CarpetAreaTo").val(0);
    //                $("#CarpetAreaTo").focus();
    //            }
    //        }
    //    } else {
    //        swal("Please Enter Carpet Area From.");
    //        $("#CarpetAreaTo").val(0);
    //    }
    //}
});

$(".FirstName").on('keyup', function () {
    $('.FirstName').val($(this).val());
});
$(".LastName").on('keyup', function () {
    $('.LastName').val($(this).val());
});
$(".MobileNo").on('keyup', function () {
    $('.MobileNo').val($(this).val());
});
$(".Email").on('keyup', function () {
    $('.Email').val($(this).val());
});
$(".Requirement").on('keyup', function () {
    $('.Requirement').val($(this).val());
});
$(".BudgetFrom").on('keyup', function () {
    $('.BudgetFrom').val($(this).val());
});
$(".BudgetTo").on('keyup', function () {
    $('.BudgetTo').val($(this).val());
});

$("#CityIdUI").on('change', function () {
    $('#QuickCityIdUI').append(new Option($("#CityIdUI option:selected").text(), $("#CityIdUI option:selected").val(), true, true));
});
$("#QuickCityIdUI").on('change', function () {
    $('#CityIdUI').append(new Option($("#QuickCityIdUI option:selected").text(), $("#QuickCityIdUI option:selected").val(), true, true));
});

$("#CountryIdUI").on('change', function () {
    $('#QuickCountryIdUI').append(new Option($("#CountryIdUI option:selected").text(), $("#CountryIdUI option:selected").val(), true, true));
});
$("#QuickCountryIdUI").on('change', function () {
    $('#CountryIdUI').append(new Option($("#QuickCountryIdUI option:selected").text(), $("#QuickCountryIdUI option:selected").val(), true, true));
});


$("#StateIdUI").on('change', function () {
    $('#QuickStateIdUI').append(new Option($("#StateIdUI option:selected").text(), $("#StateIdUI option:selected").val(), true, true));
});
$("#QuickStateIdUI").on('change', function () {
    $('#StateIdUI').append(new Option($("#QuickStateIdUI option:selected").text(), $("#QuickStateIdUI option:selected").val(), true, true));
});

$("#AvailableForIdUI").on('change', function () {
    $('#QuickAvailableForIdUI').append(new Option($("#AvailableForIdUI option:selected").text(), $("#AvailableForIdUI option:selected").val(), true, true));
});
$("#QuickAvailableForIdUI").on('change', function () {
    $('#AvailableForIdUI').append(new Option($("#QuickAvailableForIdUI option:selected").text(), $("#QuickAvailableForIdUI option:selected").val(), true, true));
});

$("#PropertyForIdUI").on('change', function () {

    $('#QuickPropertyForIdUI').append(new Option($("#PropertyForIdUI option:selected").text(), $("#PropertyForIdUI option:selected").val(), true, true));

    var PropertyFor = $("#PropertyForIdUI option:selected").text().toLowerCase();

    if ($("#PropertyForIdUI").val() != '') {
        if (PropertyFor != 'sale') {
            $('.IsLoanRequired').hide();
        } else {
            $('.IsLoanRequired').show();
        }
    }
});
$("#QuickPropertyForIdUI").on('change', function () {

    $('#PropertyForIdUI').append(new Option($("#QuickPropertyForIdUI option:selected").text(), $("#QuickPropertyForIdUI option:selected").val(), true, true));

    var PropertyFor = $("#PropertyForIdUI option:selected").text().toLowerCase();

    if ($("#PropertyForIdUI").val() != '') {
        if (PropertyFor != 'sale') {
            $('.IsLoanRequired').hide();
        } else {
            $('.IsLoanRequired').show();
        }
    }
});

$("#PropertyCategoryIdsUI").on('change', function () {

    $('#QuickPropertyCategoryIdsUI').append(new Option($("#PropertyCategoryIdsUI option:selected").text(), $("#PropertyCategoryIdsUI option:selected").val(), true, true));

    var PropertyCategory = $("#PropertyCategoryIdsUI option:selected").text().toLowerCase();

    $("#PropertyCategoryNameHidden").val(PropertyCategory);

    if ($("#PropertyCategoryIdsUI").val() != '') {
        if (PropertyCategory != 'residential') {
            $('.PropertyCategoryHideShow').hide();
        } else {
            $('.PropertyCategoryHideShow').show();
        }
    }

});

$("#QuickPropertyCategoryIdsUI").on('change', function () {

    $('#PropertyCategoryIdsUI').append(new Option($("#QuickPropertyCategoryIdsUI option:selected").text(), $("#QuickPropertyCategoryIdsUI option:selected").val(), true, true));

    var PropertyCategory = $("#PropertyCategoryIdsUI option:selected").text().toLowerCase();

    $("#PropertyCategoryNameHidden").val(PropertyCategory);

    if ($("#PropertyCategoryIdsUI").val() != '') {
        if (PropertyCategory != 'residential') {
            $('.PropertyCategoryHideShow').hide();
        } else {
            $('.PropertyCategoryHideShow').show();
        }
    }

});

function AnniversaryDateChanged() {
    var AnniversaryDate = $("#AnniversaryDate").val().substring(0, 10);
    var BirthDate = $("#BirthDate").val().substring(0, 10);

    if (AnniversaryDate != "" && BirthDate != "") {

        var Anniversary = AnniversaryDate;
        Anniversary = Anniversary.split("/");
        Anniversary = Anniversary[1] + "/" + Anniversary[0] + "/" + Anniversary[2];

        var Birth = BirthDate;
        Birth = Birth.split("/");
        Birth = Birth[1] + "/" + Birth[0] + "/" + Birth[2];

        Birth = new Date(Birth);
        Anniversary = new Date(Anniversary);
        var diff = new Date(Anniversary - Birth);
        var days = diff / 1000 / 60 / 60 / 24;

        if (days < 0) {
            swal("Anniversary Date should be greater than Birth Date", {
                icon: "info"
            });
            $("#AnniversaryDate").val('');
        }
    }
}