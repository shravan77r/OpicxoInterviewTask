var jsonKeyNameArray = [];

$(function () {
    var APIGUID = $('#APIGUID').val();
    if (APIGUID != '') {
        fnBindOutputParameterList();
    }
});

function fnBindOutputParameterList() {
    var table = $('#tblOutputParameter').DataTable({
        "paging": false,
        "bLengthChange": false,
        "autoWidth": false,
        "processing": true,
        "destroy": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "ajax": {
            "url": "/APIData/GetOutputParameterList",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.APIGUID = $("#APIGUID").val();
            }
        },
        "columns": [
            { "data": "RowNo", "autoWidth": true },
            { "data": "Key", "autoWidth": true },
            { "data": "Type", "autowidth": true },
            {
                mRender: function (data, type, full) {
                    return '<a href="#" class="btn-transparent text-danger" onclick="fnDeleteOutputParameterTable(\'' + full.OutputParameterGUID + '\')" title="Delete"><i class="fa fa-trash"></i></a>';
                }
            },
        ],
        "responsive": true,
        "columnDefs": [
            { orderable: false, targets: [0, 1] }
        ],
    });
}

function fnAddOutputParameterDetail() {

    var form = $('#form').valid();

    if (!form) {
        return false;
    }

    var APIOutputParameterDetail = {};

    APIOutputParameterDetail.APIGUID = $('#APIGUID').val();
    APIOutputParameterDetail.OutputParameterGUID = null;
    APIOutputParameterDetail.APIId = 0;
    APIOutputParameterDetail.Type = $('#objOutputParametere_Type').val();
    APIOutputParameterDetail.KeyColumn = $('#objOutputParametere_KeyColumn').val();

    $('.customloader').show();

    $.ajax({
        type: "POST",
        url: "/APIData/ManageOutputParameterDetail",
        data: APIOutputParameterDetail,
        async: !0,
        cache: !1,
        success: function (data) {
            fnShowMessage(data.message, data.messageType);
            fnBindOutputParameterList();
            $('.customloader').hide();
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
}

function fnDeleteOutputParameterTable(key) {

    if (confirm("Are you sure?")) {
        var APIOutputParameterDetail = {};

        APIOutputParameterDetail.OutputParameterGUID = key;
        APIOutputParameterDetail.IsDelete = true;

        $('.customloader').show();

        $.ajax({
            type: "POST",
            url: '/APIData/ManageOutputParameterDetail',
            async: true,
            cache: false,
            data: APIOutputParameterDetail,
            success: function (data) {
                fnBindOutputParameterList();
                fnShowMessage(data.message, data.messageType)
                $('.customloader').hide();
            },
            error: function (response) {
                console.log(response);
                fnShowMessage("Something Went Wrong!", "danger")
                $('.customloader').hide();
            }
        });
    }
}

function fnConvertJsonOutputParameter() {

    var obj = {};

    try {
        obj = JSON.parse($("#sampledata").val());

    } catch (error) {
        console.error(error);
        fnShowMessage("Not valid JSON format", "danger");
    }

    fnGetFieldNameByJsonObject(obj);

}


function fnGetFieldNameByJsonObject(obj) {

    var inputJsonObj = obj;

    var strJson = "";

    for (var key in inputJsonObj) {
        if (typeof (inputJsonObj[key]) == 'object') {
            strJson = key + " : <br>";
            jsonKeyNameArray.push({
                OutputParameterKey: key,
            });
            fnGetFieldNameByJsonObject(inputJsonObj[key]);
        } else {
            strJson = " Key: " + key + " Value: " + inputJsonObj[key] + '<br>';
            //jsonKeyNameArray.push(key);
            jsonKeyNameArray.push({
                OutputParameterKey: key,
            });
        }
    }


    for (var i = 0; i < jsonKeyNameArray.length; i++) {
        fnAddRow(jsonKeyNameArray[i]);
    }

}

function fnAddRow(obj) {

    var UpdateId = 0;

    var lastId = 0;
    try {
        lastId = parseInt($("#tblOutputParameter tr:last td.DetailId:last").attr('id').split("_").pop());
    } catch (e) {
        lastId = 0;
    }
    lastId = lastId === 0 ? 1 : lastId + 1;

    var DeleteRow = "<a title='Delete' onclick = 'fnDeleteOutputParameterTable(" + lastId + ");' class='text-danger mr-2'><i class='nav-icon i-Close-Window font-weight-bold deleteRow'></i></a>";
    var TypeDropDown = '<td><select> <option value=1> string </option> <option value=1> Number </option> <option value=1> Boolean </option> <option value=1> Datetime </option> </select></td>';

    var htmlString = ''
        + '<td class = "Key" id="Key_' + lastId + '">' + obj.OutputParameterKey + '</td>'
        + '<td>' + TypeDropDown + '</td>';
    + '<td>' + DeleteRow + '</td>';


    if (UpdateId > 0) {
        $('#tr_' + lastId).html(htmlString);
    } else {
        htmlString = '<tr id="tr_' + lastId + '">'
            + htmlString
            + '</tr>';
        $('#tblOutputParameter').append(htmlString);

    }

}

