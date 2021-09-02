const elementSignature = "{element_signature}";
const elementId = "{element_id}";
const submitSuccessId = "#SubmitSuccess";
const submitErrorId = "#SubmitErrors";
const submitProcessingId = "#SubmitProcessing";


//returns if any checkbox in element is selected
function noItemSelected(element) {
    var checkboxes = $(element).children("div").children("input:checkbox:checked");
    if (checkboxes != null)
        return checkboxes.length == 0;
    else
    return $(element).children("input:checkbox:checked").length == 0;
}
function setSingleValueFieldset() {
    try {
        $("fieldset").each(function () {
            var isMultivalue = $(this).data("bpsmultivalue");
            if (isMultivalue.toString().toLowerCase() === "true") return;

            $(this).find('div input[type="checkbox"]').on('change', function () {
                $(this).parent().parent().find('div input[type="checkbox"]').not(this).prop('checked', false);
            });
        })
    } catch (e) {
        console.log(e);
    }
}
function getCurrentLanguage() {
    var language = "en"
    try {
        language = window.navigator.userLanguage || window.navigator.language;
        if (language.split("-").length > 0) {
            language = language.split("-")[0];
        }
    } catch (e) {
        console.log(e);
    }
    return language;
}
function disableForm(isDisabled) {
    if (isDisabled) {
        $("form input[type=submit]").attr("disabled", true);
        $("input, textarea, select, option").addClass("disabled");
        $(submitProcessingId).attr("hidden", false);
    }
    else {
        $("form input[type=submit]").attr("disabled", false);
        $("input, textarea, fieldset, select, option").removeClass("disabled");
        $(submitProcessingId).attr("hidden", true);
    }
}
function setErrorsList(data) {
    $(submitErrorId).empty();
    $(submitSuccessId).text("");
    $(submitErrorId).append("<ul>");
    var errorArray = data.split("\r\n");
    if (errorArray != null) {
        errorArray.forEach(function (e) {
            if (e === "") return;
            $(submitErrorId).append("<li>" + e + "</li>");
        });
    }
    $(submitErrorId).append("</ul>");
}
function setSuccessMessage(data) {
    $(submitErrorId).text("");
    var successValue = $("#ContentSubmitSuccessMessage").val().replace(elementSignature, data.elementNumber).replace(elementId, data.elementId);
    $(submitSuccessId).text(successValue);
}
function removeLeadingZeroFromNumber(element) {
    var value = $(element).val();
    if (value === null || value === "0" || value.startsWith("0.") || value.startsWith("0,")) return;
    if (value.startsWith("0"))
        $(element).val(value.replace(/^0+/, ''));
}
function initializeDatetimePickers() {
    try {
        var currentFormat = getCurrentDateFormat();
        var currentTimeFormat = getCurrentTimeFormat();
        jQuery.datetimepicker.setLocale(getCurrentLanguage());
        $('input[datetime-control=true]').datetimepicker({ format: currentFormat + ' ' + currentTimeFormat, formatDate: currentFormat, onShow: onShowDateTimePicker, formatTime: currentTimeFormat });
        $('input[date-control=true]').each(function () {
            $(this).datetimepicker({ timepicker: false, format: currentFormat, formatDate: currentFormat, onShow: onShowDateTimePicker })
        });
        $.datetimepicker.setDateFormatter('moment');
    } catch (e) {
        console.log(e);
    }
}
function getCurrentTimeFormat() {
    const locale = navigator.language
    var hours =  Intl.DateTimeFormat(locale, { hour: 'numeric' }).resolvedOptions().hourCycle;
    if (hours===("h12")) 
        //12 hour clock
        return "hh:mm A"
    
        //24 hour clock
        return "HH:mm"   
}
function getCurrentDateFormat() {
    var checkDate = new Date(2013, 11, 31, 15, 10);
    var date = checkDate.toLocaleDateString();
    date = date.replace("31", "DD");
    date = date.replace("12", "MM");
    date = date.replace("2013", "YYYY");
    return date;
}
function getCurrentDatetimePickerFormat(format) {
    if (format == null) return;
    format = format.toUpperCase();
    return format;
}
function onShowDateTimePicker(time, input) {
    var width = $(input).parent().width();
    if (width > 0) {
        $(this).css("width", width + "px");
        var timepickerwidth = $(this).find(".xdsoft_timepicker.active").outerWidth(true);
        if (timepickerwidth === undefined)
            timepickerwidth = 0;
        $(this).children(".xdsoft_datepicker").css("width", "calc(100% - " + (timepickerwidth + 10) + "px)");
    }
}
function setRequiredSettings() {
    try {
        $("input,select,fieldset,textarea").filter("[required]").each(function () {
            $(this).parent().append('<span class="text-danger field-validation-valid" data-valmsg-for="' + this.id + '" data-valmsg-replace="true"></span>');
        })
        $(":input[required], fieldset[required]").parent().children("label").addClass("required");
        $(":input[required]").parent().parent().children("label").addClass("required");
        $("input:radio[required]").parent().children("label").removeClass("required")
        $("input:radio[required]").parent().parent().parent().children("label").addClass("required");
    } catch (e) {
        console.log(e);
    }
}
$(document).ready(function () {

    setRequiredSettings();
    setSingleValueFieldset();
    initializeDatetimePickers();

    $("input[type=number]").each(function () {
        $(this).change(function () { removeLeadingZeroFromNumber(this); })
    })
    if (getCurrentLanguage() == "pl")
        import('../lib/jquery-validation/localization/messages_pl.js');

    /*additional validation for checkboxes and fieldset, custom messages, custom fieldset validation*/
    var form = $("form");
    form.validate();
    form.find("input[type=checkbox][required]").each(function () {
        $(this).rules("add",
            {
                required: true,
                messages: {
                    required: $(this).attr("data-val-required")
                }
            });
    })
    form.find("fieldset[required]").each(function () {
        $(this).rules("remove")
        $(this).rules("add",
            {
                required: {
                    param: true,
                    depends: function () {
                        var result = noItemSelected(this); if (!result) { $(this).next("span").empty(); } return result; },
                    messages: $(this).attr("data-val-required")
                },
            });

    })
    form.find("input, select, textarea, option").change(function () {
            $(submitSuccessId).empty()
    })
});
//submit action
$(document).ready(function () {
    try {
        $("form").submit(function (e) {
            if ($("#IsCaptchaRequired").val() == "True") {
                grecaptcha.execute();
            }
            if (!$("form").valid()) return;
            e.preventDefault();
            disableForm(true);

            var form = $(this);
            $.ajax({
                type: "POST",
                url: "/BpsElement/Start",
                data: form.serialize() + '&' + "culture="+(window.navigator.userLanguage || window.navigator.language),
                success: function (data) {
                    try {
                        if (typeof data == "string") {
                            setErrorsList(data);
                        }
                        else {
                            setSuccessMessage(data);
                            $('form').trigger("reset");
                        }
                    } catch (e) {
                        $(submitErrorId).text(err);
                    }
                    finally {
                        disableForm(false);
                    }
                }
            });
        })
    } catch (e) {
        console.log(e);
    }
});
