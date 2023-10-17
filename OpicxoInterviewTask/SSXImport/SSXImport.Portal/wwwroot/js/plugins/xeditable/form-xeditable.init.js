$(function () {
	//$.fn.editableform.buttons = '<button type="submit" class="btn btn-primary editable-submit btn-xs waves-effect waves-light"><i class="fa fa-check"></i></button><button type="button" class="btn btn-danger editable-cancel btn-xs waves-effect"><i class="fa fa-close"></i></button>', 
	$.fn.editableform.buttons = "",
	$(".text-box").editable({
		type: "text",
		pk: 1,
		mode: "inline",
		onblur: "submit",
		inputclass: "form-control"
	}), 
	
	$("#inline-firstname").editable({
		validate: function (e) {
			if ("" == $.trim(e)) return "This field is required"
		},
		mode: "inline",
		inputclass: "form-control"
	}), 
	
	$("#Prefix").editable({
		prepend: "--",
		mode: "inline",
		onblur: "submit",
		inputclass: "form-control",
		source: [{
			value: 1,
			text: "Mr"
		}, {
			value: 2,
			text: "Ms"
		}]});
	$("#inline-jr").editable({
		prepend: "--",
		mode: "inline",
		onblur: "submit",
		inputclass: "form-control",
		source: [{
			value: 1,
			text: "Jr"
		}, {
			value: 2,
			text: "Sr"
		}],
		display: function (t, e) {
			var n = $.grep(e, function (e) {
				return e.value == t
			});
			n.length ? $(this).text(n[0].text).css("color", {
				"": "gray",
				1: "green",
				2: "blue"
			}[t]) : $(this).empty()
		}
	}), $("#inline-group").editable({
		showbuttons: !1,
		mode: "inline",
		inputclass: "form-control"
	}), $("#inline-status").editable({
		mode: "inline",
		inputclass: "form-control"
	}), $("#inline-dob").editable({
		mode: "inline",
		inputclass: "form-control"
	}), $("#inline-event").editable({
		placement: "right",
		mode: "inline",
		combodate: {
			firstItem: "name"
		},
		inputclass: "form-control"
	}), $("#inline-comments").editable({
		showbuttons: "bottom",
		mode: "inline",
		inputclass: "form-control"
	}), $("#inline-fruits").editable({
		pk: 1,
		limit: 3,
		mode: "inline",
		inputclass: "form-control",
		source: [{
			value: 1,
			text: "Banana"
		}, {
			value: 2,
			text: "Peach"
		}, {
			value: 3,
			text: "Apple"
		}, {
			value: 4,
			text: "Watermelon"
		}, {
			value: 5,
			text: "Orange"
		}]
	})
});