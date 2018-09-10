var SHOW_NP_PROGRESS = true;
var DISABLE_FOCUS = false;
var focusedEle = null;
var currentScroll = 0;
// NProgress
if (typeof NProgress !== "undefined") {
    $(document).ajaxStart(function () {
        currentScroll = $(window).scrollTop();
        NPStart();
    }).ajaxStop(function () {
        NPStop();
        if (currentScroll > 0)
            $(window).scrollTop(currentScroll);
    });
}
function DisableNewTab() {
    $('a[data-ajax="true"]').off("contextmenu").on("contextmenu", function (e) {
        e.preventDefault(); return false;
    }).off("mousedown").on('mousedown', function (e) {
        if (e.which == 2) {
            $(this).click();
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
    });
}
var NPStart = function () {
    if (SHOW_NP_PROGRESS && !NProgress.isStarted()) {
        if (!DISABLE_FOCUS) {
            focusedEle = $(":focus");
            NProgress.start();
            $("a,span.btn,button,input,textarea,select").each(function () {
                if ($(this).hasClass("select2-search__field")) return;
                if ($(this).prop("disabled")) {
                    $(this).attr("data-disabled", "true");
                }
                $(this).prop("disabled", true);
                $(this).addClass("disabled");
            });
        }
    }
}
var NPStop = function () {
    DisableNewTab()
    $("form[data-ajax='true']").attr("data-ajax-begin", "setHeader");
    if (SHOW_NP_PROGRESS && NProgress.isStarted()) {
        NProgress.done();
        if (!DISABLE_FOCUS) {
            $("a,span.btn,button,input,textarea,select").each(function () {
                if ($(this).attr("data-disabled") !== "true") {
                    $(this).prop("disabled", false);
                    $(this).removeClass("disabled");
                }
            });
            BindSelect2();
            DefaultCollapsedAll();
            $("form").each(function () {
                var $form = $(this);
                $form.unbind();
                $form.data("validator", null);
                $.validator.unobtrusive.parse(document);
                if ($form.data("unobtrusiveValidation")) {
                    $form.validate($form.data("unobtrusiveValidation").options);
                }
                $form.attr("data-ajax-begin", "setHeader");
            });
            $(".styled").uniform({
                radioClass: "choice"
            });
            $('[data-popup="lightbox"]').fancybox({
                padding: 3
            });
            if (focusedEle !== null) {
                setTimeout(function () {
                    if ($("#popupmodals .modal.in").length === 0)
                        focusedEle.focus();
                    else
                        $("#popupmodals .modal.in .form-control:not([readonly]):first").focus();
                }, 500);
            } else {
                FocusOnFirst();
            }
        }
    }
    setTimeout(function () {
        window.prettyPrint && prettyPrint();
        $('.wrapper').scrollbar();
        $('.scrollbar-outer').scrollbar();
    }, 1000);
}
$.fn.select2placeholder = function () {
    var options = {};
    var plholder = $(this).attr("placeholder");
    if (typeof plholder !== "undefined") {
        options["placeholder"] = plholder;
    }
    if ($(this).attr("data-keyboard") === "false") {
        options["minimumResultsForSearch"] = -1;
    }
    var $ele = $(this);
    if (typeof $(this).attr("data-dynamicurl") !== "undefined") {
        $ele.on("select2:opening", function () {
            focusedEle = $ele;
            SHOW_NP_PROGRESS = false;
        });
        $ele.on("select2:close", function () {
            SHOW_NP_PROGRESS = true;
        });
        $ele.on("select2:select", function () {
            setTimeout(function () {
                focusedEle.focus();
            }, 100);
        });
        options["minimumInputLength"] = 1;
        options["initSelection"] = function (element, callback) {
            var selectedValues = [];
            var dataValue = $(element).attr("data-value");
            if (typeof dataValue !== "undefined" && $.trim(dataValue) !== "" &&
                $.trim(dataValue) !== "0") {
                if (typeof dataValue === "string") {
                    if (dataValue.indexOf("[") === 0 &&
                        dataValue.lastIndexOf("]") === dataValue.length - 1) {
                        selectedValues = JSON.parse(dataValue);
                    } else {
                        $(dataValue.split(",")).each(function (i, v) {
                            selectedValues.push(v);
                        });
                    }
                } else {
                    selectedValues = dataValue;
                }
                $(selectedValues).each(function (i, v) {
                    var opt = $("<option/>");
                    opt.text(v);
                    opt.attr("value", v);
                    opt.appendTo($(element));
                });
                $(element).val(selectedValues);
            }
            callback($.map(selectedValues, function (id) {
                return { id: id, text: id };
            }));
        };
        options["ajax"] = {
            url: $(this).attr("data-dynamicurl"),
            dataType: "json",
            delay: 250,
            beforeSend: function (xhr) {
                setHeader(xhr);
            },
            data: function (params) {
                return {
                    q: params.term
                };
            },
            processResults: function (data, params) {
                if (data.StatusCode === 200) {
                    return {
                        results: $.map(data.Data, function (item) {
                            return {
                                text: item.Value,
                                slug: item.Value,
                                id: item.Key
                            }
                        })
                    };
                } else {
                    return {
                        results: []
                    };
                }
            },
            cache: true
        };
    } else {
        $ele.on("select2:opening", function () {
            focusedEle = $ele;
        });
    }
    return options;
}
function GetTodayDate() {
    if (IsSuperAdmin == "true") {
        return "1990-01-01";
    }
    else {
        return GetTodayDateNoSA();
    }
}

function GetTodayDateNoSA() {
    return moment(new Date()).format("MM/DD/YYYY");
}

function DefaultCollapsedAll() {
    $('.panel-collapsed').children('.panel-heading').nextAll().hide();
    $('.panel-collapsed').find('[data-action=collapse]').addClass('rotate-180');
}
$(document).ready(function () {
    NPStop();
    FocusOnFirst();
    BindSelect2();
    $(".styled").uniform({
        radioClass: "choice"
    });
    $('[data-popup="lightbox"]').fancybox({
        padding: 3
    });
    $(document).on("click", ".panel-filter", function () {
        $(this).toggleClass("bg-blue-800");
        $(this).toggleClass("bg-blue");
        $(this).parent().find("[data-action='collapse']").click();
    });
    $(document).on("keypress", ".search-filter input", function (e) {
        if (e.keyCode == 13) {
            var btnsearch = $(this).parents(".search-filter").find("#btnsearch2");
            if (btnsearch.length == 0)
                btnsearch = $(this).parents(".search-filter").find("#btnsearch");
            if (btnsearch.length > 0)
                btnsearch.click();
        }
    });
});
function FocusOnFirst() {
    setTimeout(function () {
        focusedEle = $(".form-control:not([readonly]):first");
        if (focusedEle) focusedEle.focus();
    }, 200);
}
$.fn.BindSelectClose = function () {
    $(this).on("select2:close", function (e) {
        var select = $(this);
        setTimeout(function () {
            select.next().find(".select2-selection").focus();
        }, 100);
    });
}
function setSelectInitFirst($this) {
    if (typeof $this.attr("data-value") === "undefined" || $this.attr("data-value") === null || $this.attr("data-value") === "") {
        var dataValue = $this.attr("data-select-first");
        if (typeof dataValue !== "undefined" && $.trim(dataValue) !== "" &&
            $.trim(dataValue) === "true") {
            var firstOption = $this.find("option[value!='']:first");
            if (firstOption.length > 0) {
                $this.val(firstOption.attr("value")).change();
            }
        }
    }
}
function setSelect2Value($this, isChange) {
    var dataValue = $this.attr("data-value");
    if (typeof dataValue !== "undefined" && $.trim(dataValue) !== "" &&
        $.trim(dataValue) !== "0") {
        var selectedValues = [];
        if (typeof dataValue === "string") {
            if (dataValue.indexOf("[") === 0 &&
                dataValue.lastIndexOf("]") === dataValue.length - 1) {
                selectedValues = JSON.parse(dataValue);
            } else {
                $(dataValue.split(",")).each(function (i, v) {
                    selectedValues.push(v);
                });
            }
        } else {
            selectedValues = dataValue;
        }
        $this.val(selectedValues);
    }
    if (isChange) {
        $this.change();
        var validation = $this.parent().find(".field-validation-error");
        if (validation.length > 0) {
            validation.removeClass("field-validation-error");
            validation.addClass("field-validation-valid")
            validation.html("");
        }
    }
}
function BindSelect2(selector, rebind) {
    if (typeof selector === "undefined") {
        selector = "body";
    }
    if (typeof rebind === "undefined") {
        rebind = false;
    }
    $(selector + " select").each(function () {
        var options = $(this).select2placeholder();
        var $this = $(this);
        if (!rebind && $this.attr("data-init") === "false") {
            return;
        }
        $this.attr("data-init", "false");
        var url = $this.attr("data-url");
        if (typeof (url) === "undefined") {
            setSelect2Value($this, false);
            setSelectInitFirst($this);
            $this.select2(options).BindSelectClose();
        } else {
            var cascadeSelector = $this.attr("data-cascade-selector");
            if (typeof (cascadeSelector) === "undefined") {
                BindSelect2Cascase($this, url, cascadeSelector, options);
            } else {
                $(cascadeSelector).on("change", function () {
                    BindSelect2Cascase($this, url, cascadeSelector, options);
                }).change();
            }
        }
        $this.on("change", function () {
            if ($(this).val() != "") {
                try {
                    $(this).valid();
                }
                catch (e) { }
            }
        });
    });
}
function BindSelect2Cascase(ele, url, cascadeSelector, options) {
    var id = $(cascadeSelector).val();
    var dataBlank = $(ele).attr("data-blank");
    if (typeof dataBlank === "undefined") dataBlank = false;
    else dataBlank = true;
    var def = $(ele).attr("data-default");
    if (id !== "" || dataBlank) {
        if (typeof id !== "undefined")            
            url = url + (url.indexOf('?') == -1 ? "?" : "&") + "id=" + id;
        $.ajax
            ({
                type: "get",
                url: url,
                dataType: "json",
                beforeSend: function (xhr) {
                    setHeader(xhr);
                },
                success: function (result) {
                    if (result.StatusCode === 200) {
                        var data = result.Data;
                        if (typeof def !== "undefined" && def !== "")
                            $(ele).html('<option value="">' + def + "</option>");
                        else
                            $(ele).html('<option value="">Select Any</option>');
                        $(data).each(function (i, item) {
                            var opt = $("<option/>");
                            opt.text(item.Value);
                            opt.attr("value", item.Key);
                            if (typeof item.ExtraData !== "undefined") {
                                opt.attr("data-extra", item.ExtraData);
                            }
                            opt.appendTo(ele);
                        });
                        setSelect2Value($(ele), true);
                        setSelectInitFirst($(ele));
                        $(ele).select2(options).BindSelectClose();
                    }
                },
                error: function (result) {
                    //Failed(result);
                }
            });
    } else {
        if (typeof def !== "undefined" && def !== "")
            $(ele).html('<option value="">' + def + "</option>");
        else
            $(ele).html('<option value="">Select Any</option>');
        setSelect2Value($(ele), false);
        setSelectInitFirst($(ele));
        $(ele).select2(options).BindSelectClose();
    }
}
function ShowModal(selector, callback, hiddencallback) {
    $(".modal-backdrop.in").remove();
    $(selector).data('bs.modal', null);
    $(selector).modal().on("shown.bs.modal", function (event) {
        var $form = $(selector + " form:first");
        $form.unbind();
        $form.data("validator", null);
        $.validator.unobtrusive.parse(document);
        if ($form.data("unobtrusiveValidation")) {
            $form.validate($form.data("unobtrusiveValidation").options);
        }
        $form.attr("data-ajax-begin", "setHeader");
        BindSelect2();
        $(selector).find(".styled").uniform({
            radioClass: "choice"
        });
        $(selector).find('[data-popup="lightbox"]').fancybox({
            padding: 3
        });
        BindBtTable(selector);
        BindBtDiv(selector);
        BindSearchReset(selector);
        $(selector).scroll(function () {
            $(".date-picker:visible").each(function () {
                $(this).click();
            });
        });
        $("body").addClass("modal-open")
        if (typeof callback === "function") {
            callback();
        }
    }).on('hidden.bs.modal', function () {
        $(this).data('bs.modal', null);
        if (typeof hiddencallback === "function") {
            hiddencallback();
        }
    });
}
function setHeader(xhr) {
    if (typeof AuthHeader !== "undefined" && AuthHeader !== null && AuthHeader !== "") {
        xhr.setRequestHeader("Authorization", AuthHeader);
    }
}

function apiCall(url, methodType, data, successCallback, errorCallback, async) {
    if (typeof async === "undefined")
        async = true;
    var options = {
        type: methodType,
        url: ApiUrl + url,
        dataType: "json",
        async: async,
        beforeSend: function (xhr) {
            setHeader(xhr);
        },
        success: function (result) {
            if (typeof successCallback !== "undefined" && successCallback !== null)
                successCallback(result);
        },
        error: function (result) {
            if (result.status === 200) {
                if (typeof successCallback !== "undefined" && successCallback !== null)
                    successCallback(result.responseText);
            } else {
                if (typeof errorCallback !== "undefined" && errorCallback !== null)
                    errorCallback(result);
            }
        }
    }
    if (methodType.toLowerCase() === "get") {
        options["data"] = data;
    } else {
        options["contentType"] = "application/json";
        options["data"] = JSON.stringify(data);
    }
    $.ajax(options);
}

function LogOutUser(url) {
    apiCall("Account/Logout", "POST", {}, function (data) {
        $.post(url, { user: null, token: null }).done(function () {
            window.location.reload();
        });
    });
}
function yesnoformat(value) {
    return value === true ? "Yes" : value === false ? "No" : "";
}
function dateformatter(value) {
    if (value != null) {
        return moment(value).format("MM/DD/YYYY");
    }
    else {
        return "";
    }
}
function datetimeformatter(value) {
    return datetimeformatterinline(value);
}
function datetimeformatterinline(value) {
    if (value != null) {
        return moment(value).format("MM/DD/YYYY h:mm A").toUpperCase();
    }
    else {
        return "";
    }
}
function timespanformatter(value) {
    if (value != null) {
        var splitedValue = value.split(":");
        var dt = new Date(1, 1, 1, Number(splitedValue[0]), Number(splitedValue[1]), Number(splitedValue[2]), 0);
        return moment(dt).format("h:mm a").toUpperCase();
    }
    else {
        return "";
    }
}
function timeformatter(value) {
    if (value != null) {
        return moment(value).format("h:mm a").toUpperCase();
    }
    else {
        return "";
    }
}
function urlformatter(value) {
    return '<a href="' + BASEPATH + "/" + value + '" target="_blank">' + BASEPATH + "/" + value + "</a>";
}

function IsModalState(data) {
    var error = "";
    if (typeof data.ModelState !== "undefined") {
        error = "<ul>";
        $(data.ModelState.Error).each(function (i, item) {
            error += "<li>" + item + "</li>";
        });
        error += "</ul>";
    }
    return error;
}
function Failed(d1) {
    var data = d1.error().responseJSON;
    var modelStateErr = IsModalState(data);
    if (modelStateErr !== "") {
        bootbox.alert({ title: "Validation", message: modelStateErr });
    } else if (typeof data.error_description !== "undefined") {
        bootbox.alert({ title: "Validation", message: data.error_description });
    } else if (typeof data.error !== "undefined") {
        bootbox.alert({ title: "Validation", message: data.error });
    } else {
        bootbox.alert({ title: "Error", message: "Oops! Something went wronge." });
    }
}

function isSuccess(data, isrefresh) {
    if (typeof isrefresh == "undefined")
        isrefresh = true;
    $("#popupmodals .modal.in").modal("hide");
    if (data.StatusCode === 200) {
        if (isrefresh) {
            setTimeout(function () {
                var isAnyPopupOpen = $("#popupmodals .modal:visible").length > 0;
                if (isAnyPopupOpen) {
                    $("#popupmodals [data-toggle='table']").bootstrapTable('refresh', { pageNumber: 1 });
                    $("#popupmodals [data-toggle='btdiv']").bootstrapDivRefresh();
                } else {
                    $("[data-toggle='table']").bootstrapTable('refresh', { pageNumber: 1 });
                    $("[data-toggle='btdiv']").bootstrapDivRefresh();
                }
            }, 1000);
        }
        return true;
    }
    else if (data.StatusCode === 403) {
        bootbox.alert({
            title: "Forbidden Access", message: data.Messages, callback: function () {
                window.location = BASEPATH;
            }
        });
    } else if (data.StatusCode === 401) {
        bootbox.alert({
            title: "Unauthorized Access", message: data.Messages, callback: function () {
                window.location = BASEPATH;
            }
        });
    } else if (data.StatusCode === 404) {
        bootbox.alert({
            title: "Not Found", message: data.Messages, callback: function () {
                window.location = BASEPATH;
            }
        });
    } else if (data.StatusCode === 406) {
        bootbox.alert({
            title: "Validations", message: data.Messages, callback: function () {
                $("#popupmodals .modal:first").modal("show");
            }
        });
    }
    else if (data.StatusCode === 203) {
        bootbox.alert({
            title: "Access Denied!", message: data.Messages, callback: function () {
                $('#frmlogout').click();
            }
        });
    } else {
        bootbox.alert({
            title: "Error", message: data.Messages, callback: function () {
                $("#popupmodals .modal:first").modal("show");
            }
        });
    }
    return false;
}

window["loadFirstTime"] = true;
function loadswagger() {
    $("#frameSwagger").attr("src", ApiUrl + "swagger/ui/index?token=" + encodeURIComponent(AuthHeader));
}

function GoToUrl(url) {
    window.location.href = BASEPATH + url;
}
$(document).ready(function () {
    BindSearchReset("body");
});
function BindSearchReset(selector) {
    if (typeof selector === "undefined") {
        selector = "body";
    }
    $(selector + " #btnsearch").off("click").on("click",
        function () {
            var filterDiv = $(this).parents(".search-filter").attr("id");
            if (filterDiv === "search_filter") {
                $(selector + " [data-toggle='table']").bootstrapTable('refresh', { pageNumber: 1 });
                $(selector + " [data-toggle='btdiv']").bootstrapDivRefresh();
            } else {
                $(selector + " [data-toggle='table'][data-searchselector='#" + filterDiv + "']").bootstrapTable('refresh', { pageNumber: 1 });
                $(selector + " [data-toggle='btdiv'][data-searchselector='#" + filterDiv + "']").bootstrapDivRefresh();
            }
        });
    $(selector + " #btnreset").off("click").on("click",
        function () {
            $(this).parents(".search-filter").find("input.form-control,input[type='checkbox'],input[type='radio'],select").each(function () {
                if ($(this).is("select") && $(this).find("option[value='']").length > 0) {
                    if (typeof $(this).attr("data-value") !== "undefined")
                        $(this).val($(this).attr("data-value")).trigger("change");
                    else
                        $(this).val("").trigger("change");
                } else {
                    $(this).val("").trigger("change");
                }
            });
            var filterDiv = $(this).parents(".search-filter").attr("id");
            if (filterDiv === "search_filter") {
                $(selector + " [data-toggle='table']").bootstrapTable('refresh', { pageNumber: 1 });
                $(selector + " [data-toggle='btdiv']").bootstrapDivRefresh();
            } else {
                $(selector + " [data-toggle='table'][data-searchselector='#" + filterDiv + "']").bootstrapTable('refresh', { pageNumber: 1 });
                $(selector + " [data-toggle='btdiv'][data-searchselector='#" + filterDiv + "']").bootstrapDivRefresh();
            }
        });
}
function ResetSearchFilter() {

}
var BT_TABLE_INIT = true;
function getSearchParam(btdivtable, params) {
    var $btdivtable = $(btdivtable);
    var filterDiv = $btdivtable.attr("data-searchselector");
    if (typeof filterDiv === "undefined") {
        filterDiv = "#search_filter";
    }
    params.search = {};
    $(filterDiv).find("input[type='hidden'],input.form-control,input[type='checkbox'],input[type='radio'],select").each(function () {
        if (BT_TABLE_INIT) {
            if ($(this).is("select") && $(this).find("option[value='']").length > 0) {
                if (typeof $(this).attr("data-value") !== "undefined")
                    $(this).val($(this).attr("data-value"));
                else
                    $(this).val("");
            }
        }
        params.search[$(this).attr("name")] = $.trim($(this).val());
    });
    if (BT_TABLE_INIT) {
        BT_TABLE_INIT = false;
    }
    return params;
}

var checktimer = null;
function waitUntilAjaxFinish(callback) {
    checktimer = setInterval(function () {
        checkAjaxFinish(callback);
    }, 1000);
}
function checkAjaxFinish(callback) {
    if ($("#nprogress").length === 0) {
        clearInterval(checktimer);
        callback();
    }
}
function AlertNotify(title, message, cls) {
    PNotify.removeAll();
    var html = "";
    if (typeof message === "string" || message.length <= 1)
        html = message;
    else {
        html = "<ul>";
        $(message).each(function (i, m) {
            html += "<li>" + m + "</li>";
        });
        html += "</ul>";
    }
    new PNotify({
        title: title,
        text: message,
        addclass: cls,
        delay: 5000
    });
}
function ConfirmMsg(title, messages, callback, cancelCallback) {
    bootbox.confirm({
        title: title,
        message: messages,
        buttons: { "cancel": { label: "No" }, "confirm": { label: "Yes" } },
        callback: function (r) {
            if (r) {
                callback();
            } else if (typeof cancelCallback !== "undefined") {
                setTimeout(function () {
                    cancelCallback();
                }, 500);
            }
        }
    });
}
var uploadFormDatafn = null;
function BindFileUpload(url, selector, formData, callback, startCallback) {
    var fileTypes = $(selector).attr("data-filetypes");
    if (typeof (fileTypes) === "undefined") {
        fileTypes = "^image\/(gif|jpe?g|png|bmp)$|^application\/(pdf|msword|msexcel|spreadsheetml)$|^text\/plain$";
    }
    if (typeof formData !== "function")
        formData = function () { return {}; }
    $(selector).parent().on("click", function () {
        $(selector).attr("data-file-count", 0);
        $(selector).attr("data-current-file", 0);
    });
    $(selector).fileupload({
        add: function (e, data) {
            $(selector).attr("data-file-count", Number($(selector).attr("data-file-count")) + 1);
            if (fileTypes != "") {
                var uploadErrors = [];
                var acceptFileTypes = new RegExp(fileTypes, "i");
                if (data.originalFiles[0]["type"].length && !acceptFileTypes.test(data.originalFiles[0]["type"])) {
                    if (fileTypes.indexOf("image") >= 0) {
                        uploadErrors.push("Not an accepted file type (only gif,jpg,jpeg,png,bmp file types accepted)");
                    } else if (fileTypes.indexOf("csv") >= 0) {
                        uploadErrors.push("Not an accepted file type (only csv files accepted)");
                    }
                    else if (fileTypes.indexOf("msexcel") >= 0 || fileTypes.indexOf("spreadsheetml") >= 0) {
                        uploadErrors.push("Not an accepted file type (only excel files accepted)");
                    } else {
                        uploadErrors.push('Not an accepted file type (only "' + fileTypes + '" file types accepted)');
                    }
                }
                if (data.originalFiles[0]["size"].length && data.originalFiles[0]["size"] > 5000000) {
                    uploadErrors.push("Filesize is too big(Maximum file size 5Mb)");
                }
                if (uploadErrors.length > 0) {
                    bootbox.alert({ title: "Error", message: uploadErrors });
                } else {
                    uploadFormDatafn = formData;
                    data.submit();
                }
            }
        },
        sequentialUploads: true,
        url: url,
        dataType: "json",
        beforeSend: function (xhr, settings) {
            var fdata = uploadFormDatafn();
            for (var key in fdata)
                settings.data.append(key, fdata[key]);
            setHeader(xhr);
        },
        start: function (e, data) {
            if (typeof (startCallback) !== "undefined" && startCallback != null) {
                startCallback(data);
            }
        },
        done: function (e, data) {
            $("#loader").hide();
            var currentFile = Number($(selector).attr("data-current-file")) + 1;
            var fileCount = Number($(selector).attr("data-file-count"));
            $(selector).attr("data-current-file", currentFile);
            if (currentFile === fileCount) {
                $(selector).attr("data-file-count", 0);
                $(selector).attr("data-current-file", 0);
                if (typeof (callback) !== "undefined" && callback != null) {
                    callback(data, true);
                }
            } else if (typeof (callback) !== "undefined" && callback != null) {
                callback(data, false);
            }
        },
        progressall: function (e, data) {
            $("#loader").show();
        }
    }).prop("disabled", !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : "disabled");
}