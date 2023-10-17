function GetTables() {
    $.ajax({
        type: "POST",
        url: "/GenericTemplate/GetTables",
        data: '{}',
        async: !0,
        cache: !1,
        success: function (r) {
            var ddlObjects = $("[id=Object]");
            ddlObjects.empty().append('<option selected="selected" value="0">Please select</option>');

            $.each(r.data, function (i, val) {
                ddlObjects.append($("<option></option>").val(val).html(val));
            });

           
        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
}


$(function () {
    $("#ObjectType").change(function () {
        if ($('option:selected', this).val() == 1) {
            GetTables();
        }
        else if ($('option:selected', this).val() == 2) {
            GetViews();
        }
        else {
            var ddlObjects = $("[id=Object]");
            ddlObjects.empty().append('<option selected="selected" value="0">Please select</option>');
        }
    });


    $("#Object").change(function () {
        GetFieldsByTableName();
    });
});

function GetViews() {
    $.ajax({
        type: "POST",
        url: "/GenericTemplate/GetViews",
        data: '{}',
        async: !0,
        cache: !1,
        success: function (r) {
            var ddlObjects = $("[id=Object]");
            ddlObjects.empty().append('<option selected="selected" value="0">Please select</option>');
            $.each(r.data, function (i, val) {
                ddlObjects.append($("<option></option>").val(val).html(val));
            });

        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
}


function GetFieldsByTableName() {

    var key = $("[id=Object]").val();
    $.ajax({
        type: "POST",
        url: "/GenericTemplate/GetFieldsByTableName",
        data: { key : key },
        async: !0,
        cache: !1,
        success: function (r) {
            var listObjects = $("[id=listGroup]");
            listObjects.empty();
            $.each(r.fieldarray, function (i, val) {
                if (i == 0) {
                    var html = '<a href="#" class="list-group-item list-group-item-action active">' + val + '</a>';
                    $('.list-group').append(html);
                }
                else {
                    var html = "<a href='#' class='list-group-item list-group-item-action'>" + val + "</a>";
                    $('.list-group').append(html);
                }
                
            });
           

        },
        error(response) {
            console.log(response);
            fnShowMessage("Something Went Wrong", "danger");
            $('.customloader').hide();
        },
    });
}

$('.list-group').on('dblclick', '.list-group-item', function (event) {
    event.preventDefault();

    $(this).addClass('active').siblings().removeClass('active');


    var $this = $(this);
    var $alias = $this.data('alias');

  
    myfunction($this, $alias);
});



function myfunction($this, $alias) {

    var editorselection = $("#html_editor").dxHtmlEditor("instance").getSelection();
    if (editorselection != null) {
        $("#html_editor").dxHtmlEditor("instance").insertText(editorselection.index, "#" + $this.text(), {
            bold: true,
            color: "green"
        });
    }
    console.log($this.text());  // Will log Paris | France | etc...

    console.log($alias);  // Will output whatever is in data-alias=""
}


