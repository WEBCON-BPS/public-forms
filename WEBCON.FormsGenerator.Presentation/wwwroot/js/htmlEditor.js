//Change tab in editor
function changeTab(tablink, tabContentId) {
    $(".tabcontent").each(function () {
        $(this).css("display", "none");
    });
    $(".tablinks").each(function () {
        $(this).removeClass("active");
    });
    $(tabContentId).css("display", "block");
    $(tablink.target).addClass("active");
}
$.fn.selectRange = function (start, end) {
    if (end === undefined) {
        end = start;
    }
    return this.each(function () {
        if ('selectionStart' in this) {
            this.selectionStart = start;
            this.selectionEnd = end;
        } else if (this.setSelectionRange) {
            this.setSelectionRange(start, end);
        } else if (this.createTextRange) {
            var range = this.createTextRange();
            range.collapse(true);
            range.moveEnd('character', end);
            range.moveStart('character', start);
            range.select();
        }
    });
};
function pasteStringInString(whereToPasteObject, textToPast) {
    var caretPos = whereToPasteObject[0].selectionStart;
    var textAreaTxt = whereToPasteObject.val();
    whereToPasteObject.val(textAreaTxt.substring(0, caretPos) + textToPast + textAreaTxt.substring(caretPos));

    whereToPasteObject.selectRange(caretPos + textToPast.length);
    whereToPasteObject.focus();
}
function contentFieldsAnimation(isEnabled) {
    if (isEnabled) {
        $(contentFieldsListId).addClass("display-disabled");
        $(fieldsLoadingIconContainerId).css("visibility", "visible");
        enableRotate(fieldsLoadingIconId, true);
    }
    else {
        $(contentFieldsListId).removeClass("display-disabled");
        $(fieldsLoadingIconContainerId).css("visibility", "hidden");
        enableRotate(fieldsLoadingIconId, false);
    }
}
///Fires when refresh button is clicked. Download new fields from BPS/ remove removed fields
function refreshForm(formguid, bpsformtypeguid, bpsworkflowguid, stepguid) {
    enableRotate(refreshFormButtonIcon, true);
    $(refreshFormButton).attr("disabled", true);
    contentFieldsAnimation(true);
    $.post("/FormContentRefresh/RefreshContent/", { bpsFormTypeGuid: bpsformtypeguid, bpsWorkflowGuid: bpsworkflowguid, formGuid: formguid, stepGuid: stepguid, token: "" }, function (data) {
        try {
            if (typeof (data) === 'string') {
                if (data !== "") {
                    alert(data);
                    return;
                }
            }
            createMetadata(data.formContentWithMetadata, data.formContentTransformed, data.contentFields)
        }
        catch (e) {
            console.log(e);
        }
        finally {
            enableRotate(refreshFormButtonIcon, false);
            $(refreshFormButton).attr("disabled", false);
            contentFieldsAnimation(false);
        }
    });

}
function enableRotate(element, isEnabled) {
    if (isEnabled)
        $(element).addClass("rotate");
    else
        $(element).removeClass("rotate");
}
///Fires when reload form is expected
function reloadForm() {
    var currentHtml = $(contentTextAreaId).val();
    var title = $(contentTitleId).val();
    var description = $(contentDescriptionId).val();
    var submitText = $(contentSubmitTextId).val();
    var currentContentFields = new Array();
    $(".meta-row").each(function () {     
        currentContentFields.push({ isRequired: $(this).find(".meta-isRequired").prop('checked'), name: $(this).find(".meta-name").text(), customName: $(this).find(".meta-customName").val(), guid: $(this).find(".meta-guid").val(), customRequiredWarningText: $(this).find(".meta-fieldRequiredText").val() })
    });
    $.post("/FormContent/GetWithTransformedMeta/", { contentFields: currentContentFields, contentWithMetadata: currentHtml, contentTitle: title, contentDescription: description, contentSubmitText: submitText }, function (data) {
        if (typeof (data) === 'string') {
            $(errorId).html("<p>" + data + "</p>")
            $(formDisplayDivId).empty();
            $("#form-html textarea").text("");
            return;
        }
        $(contentTransformedId).text(data.formContentTransformed);
    });
}
///Create metadatas - fill content title etc., add table with content fields
function createMetadata(htmlFormWithMetadata, htmlFormTransformed, contentFields) {
    try {
        $(contentTextAreaId).val(htmlFormWithMetadata);
        $("#form-html textarea").text(htmlFormTransformed);
        $(contentFieldsTBody).empty();
        ///Add rows to fields table
        $.each(contentFields, function (index, metadata) {
            var checked = (metadata.isRequired == true) ? 'data-val="true" value="true" checked="checked"' : 'data-val="false" value="false"';
            $(contentFieldsTBody).append(
                '<tr class="meta-row">' +
                '<td scope="col" class="meta-col">' + '<label>' + metadata.bpsName + '</label><br /><label class="meta-name label-small">' + metadata.name + '</label ></td>' +
                '<td scope="col" class="meta-col"><input class="meta-customName form-control" name="ContentFields[' + index + '].CustomName" value="' + metadata.customName + '" type="text"></td>' +
                '<td scope="col" class="meta-col"><input class="meta-isRequired" type="checkbox" id="ContentFields_' + index + '__IsRequired" name="ContentFields[' + index + '].IsRequired"' + checked + '></td>' +
                '<td scope="col" class="meta-col"><input class="meta-fieldRequiredText form-control" name="ContentFields[' + index + '].CustomRequiredWarningText" value="' + metadata.customRequiredWarningText + '" type="text"></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].BpsFormFieldGuid" value="' + metadata.bpsFormFieldGuid + '" /></td>' +
                '<td hidden><input class="meta-guid" type="hidden" name="ContentFields[' + index + '].Guid" value="' + metadata.guid + '" /></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].Name" value="' + metadata.name + '" /></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].IsNewField" value="' + metadata.isNewField + '" /></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].Type" value="' + metadata.type + '" /></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].BpsName" value="' + metadata.bpsName + '" /></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].BpsIsReadonly" value="' + metadata.bpsIsReadonly + '" /></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].BpsIsRequired" value="' + metadata.bpsIsRequired + '" /></td>' +
                '<td hidden><input type="hidden" name="ContentFields[' + index + '].AllowMultipleValues" value="'+ metadata.allowMultipleValues +'" /></td>'+
                '</tr>');
        })
    } catch (e) {
        console.log(e);
    }
}
function addClickEvents() {
    $("#button-html-meta").click(function (tablink) { changeTab(tablink, "#form-html-metadata"); })
    $("#button-css").click(function (tablink) { changeTab(tablink, "#form-style"); })
    $("#button-preview").click(function (tablink) { changeTab(tablink, "#form-html"); })

    $("#button-run").click(function () {
        var viewModel = {
            formContent: $(contentTransformedId).val(),
            useStandardBootstrapStyle: $(useStandardBootstrapStyleId).prop('checked'),
            customCssLink: $(customLinkCssId).val(),
            customCss: $(customCssId).val()
        };
        var recursiveEncoded = $.param(viewModel, true);
        $("iframe").attr("src", "/FormContentPreview/Preview?" + recursiveEncoded);
    });
    ///Add parameter to string (here add parameter to success submit message)
    $(".parameter-button").click(function () {
        pasteStringInString($("#ContentSubmitSuccessMessage"), $(this).val());
    });
    $("#refresh-form").click(function () {
        var formGuid = $("#Guid").val();
        var bpsFormTypeGuid = $(formTypeListId).val();
        var bpsWorkflowGuid = $(workflowListId).val();
        var stepGuid = $(startStepId).val();
        refreshForm(formGuid, bpsFormTypeGuid, bpsWorkflowGuid, stepGuid);
    })
    /*Frame ancestors*/
    $("#add-frame-ancestor").click(function () {
        $("#frame-ancestors").append('<div class="form-inline frame-ancestor-line"><input type="text" class="form-control" name="FrameAncestors" /><button value="Remove" class="btn btn-light ml-2 remove-frame-ancestor" type="button"><i class="fas fa-minus"></i></button></div>');
    });
    $("#frame-ancestors").on('click', '.remove-frame-ancestor', function () {
        $(this).parent().remove();
    });
}
function addTriggerEvents() {
    $("#button-html-meta").trigger("click");

}
function addChangeEvents() {
    ///reload form if any selector has changed
    $(document).on("change", ".meta-customName, .meta-isRequired, .meta-fieldRequiredText, #ContentTitle, #ContentDescription, #ContentSubmitText, #content", reloadForm);
    //content field if required change then change value
    $(document).on("change", ".meta-isRequired", function () { $(this).val(this.checked) });
    $(contentTextAreaId).change(function () { $(this).valid() });
}
$(document).ready(function () {
    addClickEvents();
    addTriggerEvents();
    addChangeEvents();
});

$(document).ready(function () {
    ///Set height of preview depending on window height
    var height = $(window).height();
    $("frame-preview").css('height', height * 0.9 | 0);
    $('[data-toggle="tooltip"]').tooltip()
});