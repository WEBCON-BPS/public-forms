var listCount = 5; //list of BPS objects loaded from portal
var loadedListCount = 1; //used to determine are all lists loaded and no initial value should be set after list values reloading - just set initial value only during first loading

function setError(error) {
    console.log(error)
    $(errorId).html("<p>" + error + "</p>")
}
///Remove items in dropdown
function emptyList(listId) {
     if (loadedListCount > listCount) {
        var items = '<option>' + defaultListValue + '</option>';
        $(listId).html(items);
    }
}
function isSelectionEmpty(value) {
    return (value === defaultListValue || value === "00000000-0000-0000-0000-000000000000" || value === "")
}
function copyToClipboard(element) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).val()).select();
    document.execCommand("copy");
    $temp.remove();
}

///Add items to dropdown
function populateList(url, data, listId, initialValue) {
    try {
        $.post(url, data, function (data) {
            if (data == null) return;
            var items = '<option>' + defaultListValue + '</option>';
            if (typeof (data) === 'string') {
                setError(data);
                loadedListCount = listCount+1;
                return;
            }
            var count = data.length;
            $.each(data, function (i, item) {
                items += "<option  value='" + item.key + "'>" + item.value + "</option>";
            });
            $(listId).html(items);

            //Set selected value when edit is selected in edit mode
            if (initialValue != null && loadedListCount <= listCount) {
                $(listId).val(initialValue);
                $(listId).change();
            }
            else if (count === 1) {
                $(listId).val(data[0].key);
                $(listId).change();
            }
            else {
                $(listId).change(); //trigger changing list to load next list
            }
            loadedListCount++;
        });
    } catch (e) {
        console.log(e);
    }
}
function loadBpsObjectsBasedOnParentSelection(parentArgs, bpsObjectId, populateListActionLink, selectInitialValue) {
    try {
        var currentParentValue = $(parentArgs).val();
        if (isSelectionEmpty(currentParentValue)) {
            emptyList(bpsObjectId);
            if ($(bpsObjectId).val())
                $(bpsObjectId).val($(bpsObjectId + " option:first").val()).trigger("change");
            return;
        }
        var initialValue = selectInitialValue ? $(bpsObjectId).val() : null;
        populateList(populateListActionLink, { parentGuid: currentParentValue }, bpsObjectId, initialValue);
    } catch (e) {
        console.log(e);
    }
}
function loadStartStep(args) {
    var currentWorkflow = $(args).val();
    if (isSelectionEmpty(currentWorkflow)) {     
        if ($(startStepNameId).val()) {
            $(startStepNameId).val("");
            $(startStepId).val(null).trigger("change")
        }
        return;
    }
    try {
        $.post("/BpsData/GetStartStep/", { parentGuid: currentWorkflow }, function (data) {
            if (typeof (data) === 'string') {
                setError(data);
                return;
            }
            $(startStepNameId).val(data.value);
            $(startStepId).val(data.key).trigger("change");
        });
    } catch (e) {
        console.log(e);
    }
}
/*Get form body on form selection*/
function loadFormBody(args, formTypeName) {
    //Do not load if form type is not selected
    var currentFormTypeGuid = $(args).val();
    if (isSelectionEmpty(currentFormTypeGuid)) {
        emptyFormContent();
        return;
    }
    $(contentTitleId).val(formTypeName);

    var currentWorkflow = $(workflowListId).val();
    var currentStep = $(startStepId).val();
    contentFieldsAnimation(true);
    $.post("/FormContent/Create/", { formTypeGuid: currentFormTypeGuid, contentTitle: formTypeName, workflowGuid: currentWorkflow, stepGuid: currentStep }, function (data) {
        try {
            if (typeof (data) === 'string') {
                setError(data);
                emptyFormContent();
                return;
            }
            createMetadata(data.formContentWithMetadata, data.formContentTransformed, data.contentFields)
        }
        catch (e) {
            console.log(e);
        }
        finally {
            contentFieldsAnimation(false);
        }
    });   
}
function emptyFormContent() {
    $(contentTextAreaId).val("");
    $("#form-html textarea").text("");
    $(contentFieldsTBody).empty();
    $(contentTitleId).val("");
}
$(document).ready(function () {

    if ($("#custom-connection-check").is(':checked')) {
        $("#custom-connection").toggleClass("collapse");
    }
    $("#custom-connection-check").change(function () {
        $("#custom-connection").toggleClass("collapse");
    })
})
$(document).ready(function () {
    //When value on list selected then remove validation errors
    $('select').change(function () {
        if ($(this).val() != defaultListValue) {
            $(this).valid();
        }
        $(errorId).empty();
    });

    var isLoaded = false;
    var editBpsSelection = $("#edit-bps-selection");
    if (editBpsSelection.length) //edit mode
    {
        editBpsSelection.click(function () {
            $("#bps-selection-group").toggleClass("display-disabled");
            if (!$("#bps-selection-group").hasClass("display-disabled") && !isLoaded) {
                loadBpsObjects(true)
                isLoaded = true;
            }
        });
    }
    else { loadBpsObjects(!isSelectionEmpty($(applicationListId).val())); }
});
function loadBpsObjects(selectInitialValue) {
    var token = "";
    function getTokenAndloadApplications() {
        $.post("/BpsData/GetToken/", {}, function (data) {
            token = data.token;
        }).done(function () {
            var initialApplication = selectInitialValue ? $(applicationListId).val() : null;
            populateList("/BpsData/GetApplications/", {}, applicationListId, initialApplication)

            var initialBusinessEntity = selectInitialValue ? $(businessEntityId).val() : null;
            populateList("/BpsData/GetBusinessEntities", {}, businessEntityId, initialBusinessEntity);
        })
    }
    /*Change events*/
    $(applicationListId).change(function () {
        $(applicationNameId).val($(applicationListId + " :selected").text());
        loadBpsObjectsBasedOnParentSelection(this, processListId, "/BpsData/GetProcesses/", selectInitialValue);
    });
    $(businessEntityId).change(function () {
        $(businessEntityNameId).val($(businessEntityId + " :selected").text());
    })
    $(processListId).change(function () {
        $(processNameId).val($(processListId + " :selected").text());
        loadBpsObjectsBasedOnParentSelection(this, workflowListId, "/BpsData/GetWorkflows/", selectInitialValue);
    });
    $(workflowListId).change(function () {
        $(workflowNameId).val($(workflowListId + " :selected").text());
        loadStartStep(this)
    });
    $(formTypeListId).change(function (e) {
        var currentFormTypeName = $(formTypeListId + " :selected").text()
        $(formTypeNameId).val(currentFormTypeName);
        if (!selectInitialValue || loadedListCount > listCount || e.originalEvent) {
            loadFormBody(this, currentFormTypeName)
        }
    });
    $(startStepId).change(function () {
        loadBpsObjectsBasedOnParentSelection($(workflowListId), formTypeListId, "/BpsData/GetForms/",  selectInitialValue);
        loadBpsObjectsBasedOnParentSelection(this, pathListId, "/BpsData/GetStepPaths/", selectInitialValue);
        $(this).parent().children("span").empty();
    });
    $(pathListId).change(function () {
        $(pathNameId).val($(pathListId + " :selected").text())
    });
    getTokenAndloadApplications();
}