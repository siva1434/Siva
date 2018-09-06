function DataSuccess(data) {
    if (isSuccess(data)) {
        bootbox.alert({
            title: "Success",
            message: data.Messages,
            callback: function () {
                $("#lnkreset").parents("form").find(".form-control:not([readonly])").val("");
                $("#lnkreset").trigger("click");
            }
        });
    }
}
function PopupDataSuccess(data) {
    if (isSuccess(data)) {
        AlertNotify("Success", data.Messages, "bg-success");
        
        window.location.reload();
    }
     
}
function popupsuccess(html) {
    if (html == "") {
        AlertNotify("Not Found", "Oops! Something went wrong. Please try again.", "bg-warning");
    }
}
function tbl_Status(value) {
    if (value)
        return "<span class='label label-success'>Active</span>";
    else
        return "<span class='label label-warning'>Deactive</span>";
}
function tbl_Mode(value) {
    if (value)
        return "<span class='label label-default'>ADD</span>";
    else
        return "<span class='label label-default'>EDIT</span>";
}

function editsuccess() {
    FocusOnFirst();
}

function tbl_Edit(value) {
    return tblEdit(this, value);
}
function tblEdit($this, value) {
    return '<a data-ajax="true" data-ajax-mode="replace" data-ajax-update="#addeditarea" href="' +
        BASEPATH + $this.url + "?id=" + value +
        '" class="btn btn-info btn-sm" data-ajax-success="editsuccess"><i class="glyphicon glyphicon-pencil"></i></a>';
}
function tblEditRedirect($this, value) {
    return '<a href="' + BASEPATH + $this.url + "?id=" + value + '" class="btn btn-info btn-sm"><i class="glyphicon glyphicon-pencil"></i></a>';
}
function tbl_Edit_Redirect(value) {
    return tblEditRedirect(this, value)
}
function tblEditPopup($this, value) {
    return '<a href="' + BASEPATH + $this.url + "?id=" + value +
        '" data-ajax="true" data-ajax-mode="replace" data-ajax-update="#popupmodals" data-success="popupsuccess" class="btn btn-info btn-sm"><i class="glyphicon glyphicon-pencil"></i></a>';
}
function tbl_Edit_Popup(value) {
    return tblEditPopup(this, value);
}
function tblEditPopupPlus($this, value) {
    return '<a href="' + BASEPATH + $this.url + "?id=" + value +
        '" data-ajax="true" data-ajax-mode="replace" data-ajax-update="#popupmodals" data-success="popupsuccess" class="btn btn-info btn-sm"><i class="glyphicon glyphicon-plus"></i></a>';
}
function tbl_Edit_PopupPlus(value) {
    return tblEditPopupPlus(this, value);
}
function tblEditPopupCheck($this, value) {
    return '<a href="' + BASEPATH + $this.url + "?id=" + value +
        '" data-ajax="true" data-ajax-mode="replace" data-ajax-update="#popupmodals" data-success="popupsuccess" class="btn bg-orange btn-sm"><i class="glyphicon glyphicon-check"></i></a>';
}
function tbl_Edit_PopupCheck(value) {
    return tblEditPopupCheck(this, value);
}

function tblDelete($this, value, reload) {
    if (typeof reload === "undefined") reload = false;
    return "<a class='btn btn-sm btn-danger' onclick='DataDelete(\"" + $this.url + "\",\"" + value + "\",\"" + reload+"\");' href='javascript:void(0);'><i class='glyphicon glyphicon-trash'></i></a>";
}
function tbl_Delete(value) {
    return tblDelete(this, value);
    
}
function DataDelete(url, id, reload) {
    PNotify.removeAll()
    var notice = new PNotify({
        title: "Delete Confirmation",
        text: "Are you sure you want to delete?",
        hide: false,
        type: 'warning',
        confirm: {
            confirm: true,
            buttons: [
                {
                    text: 'Yes',
                    addClass: 'btn btn-sm btn-danger'
                },
                {
                    addClass: 'btn btn-sm btn-link'
                }
            ]
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        }
    })
   
    // On confirm
    notice.get().on('pnotify.confirm', function () {
        apiCall(url + "?id=" + id,
            "get",
            {},
            function (data) {
                if (data.StatusCode === 200) {
                    AlertNotify("Deleted", data.Messages, "bg-success");
                    if (reload == "true") {
                        setTimeout(function () {
                            window.location.reload();
                        }, 500);
                    } else {
                        $("#lnkreset").click();
                        $("[data-toggle='table'],[data-toggle='btdiv']").each(function () {
                            var $this = $(this);
                            var filterDiv = $this.attr("data-searchselector");
                            if (typeof filterDiv === "undefined") {
                                filterDiv = "#search_filter";
                            }
                            $(filterDiv).find("input.form-control,input[type='checkbox'],input[type='radio'],select").each(function () {
                                if ($(this).is("select") && $(this).find("option[value='']").length > 0) {
                                    if (typeof $(this).attr("data-value") !== "undefined")
                                        $(this).val($(this).attr("data-value")).trigger("change");
                                    else
                                        $(this).val("").trigger("change");
                                } else {
                                    $(this).val("").trigger("change");
                                }
                            });
                        });
                        $("[data-toggle='table']").bootstrapTable('refresh', { pageNumber: 1 });
                        $("[data-toggle='btdiv']").bootstrapDivRefresh();
                    }
                } else {
                    AlertNotify("Attention!", data.Messages, "bg-warning");
                }
            });
    })
}

// Number input validation
$(document).on("keypress", 'input[type="number"].mark', function (evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    var $this = $(this).val();
    if ((charCode > 31 && (charCode < 48 || charCode > 57)) || ($this.length > 1))
        return false;
    else
        return true;
});

if (!Array.from) {
    Array.from = (function () {
        var toStr = Object.prototype.toString;
        var isCallable = function (fn) {
            return typeof fn === 'function' || toStr.call(fn) === '[object Function]';
        };
        var toInteger = function (value) {
            var number = Number(value);
            if (isNaN(number)) { return 0; }
            if (number === 0 || !isFinite(number)) { return number; }
            return (number > 0 ? 1 : -1) * Math.floor(Math.abs(number));
        };
        var maxSafeInteger = Math.pow(2, 53) - 1;
        var toLength = function (value) {
            var len = toInteger(value);
            return Math.min(Math.max(len, 0), maxSafeInteger);
        };

        // The length property of the from method is 1.
        return function from(arrayLike/*, mapFn, thisArg */) {
            // 1. Let C be the this value.
            var C = this;

            // 2. Let items be ToObject(arrayLike).
            var items = Object(arrayLike);

            // 3. ReturnIfAbrupt(items).
            if (arrayLike == null) {
                throw new TypeError("Array.from requires an array-like object - not null or undefined");
            }

            // 4. If mapfn is undefined, then let mapping be false.
            var mapFn = arguments.length > 1 ? arguments[1] : void undefined;
            var T;
            if (typeof mapFn !== 'undefined') {
                // 5. else
                // 5. a If IsCallable(mapfn) is false, throw a TypeError exception.
                if (!isCallable(mapFn)) {
                    throw new TypeError('Array.from: when provided, the second argument must be a function');
                }

                // 5. b. If thisArg was supplied, let T be thisArg; else let T be undefined.
                if (arguments.length > 2) {
                    T = arguments[2];
                }
            }

            // 10. Let lenValue be Get(items, "length").
            // 11. Let len be ToLength(lenValue).
            var len = toLength(items.length);

            // 13. If IsConstructor(C) is true, then
            // 13. a. Let A be the result of calling the [[Construct]] internal method of C with an argument list containing the single item len.
            // 14. a. Else, Let A be ArrayCreate(len).
            var A = isCallable(C) ? Object(new C(len)) : new Array(len);

            // 16. Let k be 0.
            var k = 0;
            // 17. Repeat, while k < len… (also steps a - h)
            var kValue;
            while (k < len) {
                kValue = items[k];
                if (mapFn) {
                    A[k] = typeof T === 'undefined' ? mapFn(kValue, k) : mapFn.call(T, kValue, k);
                } else {
                    A[k] = kValue;
                }
                k += 1;
            }
            // 18. Let putStatus be Put(A, "length", len, true).
            A.length = len;
            // 20. Return A.
            return A;
        };
    }());
}