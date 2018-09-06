var forceconfirmignore = false;
$(document).ready(function () {   
    $('#DateOfInvoice').datePicker({
        //format: 'yyyy-MM-dd',
        //minDate: GetTodayDate()  
        maxDate: $('#DateOfInvoice').val()       
    });
    $('#DateOfInvoice').attr('readonly', true);   
    $("#TotalDiscountAmt").on("change", function () {
        calculateTotal();
    });
    calculateTotal();
});
function ConfirmPay() {
    if (!forceconfirmignore && $("#paymentlist .pay-entry").length > 0) {
        return "Are you sure to make Payment of " + $("#TotalPaid").val() + "?";
    }
    else {
        return null;
    }
}
function InvoiceDataSuccess(data) {
    if (data.StatusCode === 417) {
        $(".modal-backdrop").remove();
        ShowModal("#FollowUpModal", function () {
            $('#FollowUpDate').datePicker({
                minDate: $('#DateOfInvoice').val()
            });
            $('#FollowUpDate').attr('readonly', true);   
        });
        if (data.Messages.length > 0 && data.Messages[0] != "") {
            AlertNotify("Error!", data.Messages, "bg-warning");
        }
    }
    else if (data.StatusCode===200) {
        forceconfirmignore = false;
        $("#btnGoBack").click();
    } else {
        bootbox.alert({
            title: "Validation", message: data.Messages, callback: function () {                
            }
        });
    }
}
function FollowupSuccess(data) {
    if (data.StatusCode === 200) {
        $("#AppFollowupDate").val(moment(eval("new " + data.Data.FollowUpDate.replace(/\//g, ""))).format('D, MMMM YYYY'));
        $("#AppFollowUpStatus").val(data.Data.FollowUpStatus);
        $("#AppFollowUpComment").val(data.Data.FollowUpComment);
        $("#FollowUpModal").modal("hide");
        setTimeout(function () {
            forceconfirmignore = true;
            $("#btnsubmitinvoice").click();
        }, 100)
    } else {
        AlertNotify("Error!", data.Messages, "bg-warning");
    }
}
function AddPaymentSuccess(html) {
    var idx = $("#paymentlist .pay-entry").length;
    if (idx == 0) { $("#paymentlist .no-records").hide(); }
    $(html).appendTo("#paymentlist");
    $("#paymentlist .pay-entry").each(function (i) {
        $(this).find("input[type='hidden']").each(function () {
            $(this).attr("name", "InvoicePaymentsList[" + i + "]." + $(this).attr("data-name"));
        });
        $(this).find(".rec-no");
    });
    calculateTotal();
    $("#AddEditPayment").modal("hide");
}
function RemovePayRow(t) {    
    $('#pay_row_' + t).remove();
    calculateTotal();
    var idx = $("#paymentlist .pay-entry").length;
    if (idx == 0) {
        $("#paymentlist .no-records").show();
       
    }
}
function calculateTotal() {    
    var amount = 0;
    var tax = 0;
    var enterAmount = 0;
    $("#paymentlist .pay-entry").each(function (i) {
        $(this).find("input[name^='InvoicePaymentsList']").each(function () {
            var name = $(this).attr("name").split(".")[1];
            $(this).attr("name", "InvoicePaymentsList[" + i + "]." + name);
        });
    });
    $("#paymentlist .pay-entry").each(function (i) {
        enterAmount += Number($(this).find("[data-name='EnterAmount']").val());
        amount += Number($(this).find("[data-name='Total']").val());
    });
    var discper = 0;//Number($("#TotalDiscountAmt").val());
    $("#basepriceamt").text(amount);
    $("#BasePrice").val(enterAmount);
    $("#payableamount").text(Number($("#payableamount").attr("data-amount")) - amount);
    var totaldisc = 0;
    if(discper) {
        totaldisc = amount * discper / 100;
        $("#totaldiscamt").text(totaldisc);
        $("#TotalDiscount").val(totaldisc);
        amount = amount - totaldisc;
    }
    $("#totalamount").text(amount);
    $("#TotalAmount").val(amount);
    var totalTax = 0;
    $(".tax-value").each(function () {
        var taxType = $(this).attr("data-type");
        var taxValue = $(this).attr("data-value");
        if (taxValue && amount) {
            if (taxType.toLowerCase() != "fixed") {
                taxValue = Math.round(amount * taxValue / 100);
                totalTax += taxValue;
                $(this).text(taxValue);
            } else {
                taxValue = Number(taxValue);
                if (isNaN(taxValue)) taxValue = 0;
                totalTax += taxValue;
                $(this).text(taxValue);
            }
        } else {
            $(this).text(0);
        }
    });
    $("#totaltaxamt").text(totalTax);
    $("#TotalTax").val(totalTax);
    amount += totalTax;
    $("#totalpaid").text(amount);
    $("#TotalPaid").val(amount);    
}
$("#TaxId").on("change", function () {
    $("#taxlist").html("");
    if ($(this).val() != "") {
        apiCall("superadmin/taxes/getbyid?id=" + $(this).val(), "get", {}, function (data) {
            if (data.StatusCode === 200) {
                $("#taxname").text(data.Data.Name);
                var type1 = '<div class="row"><div class="col-md-10 text-right">' +
                    data.Data.Title1 + "(" + data.Data.Value1 + (data.Data.Type1 != "Fixed" ? "%" : "") + '): </div><div class="col-md-1 text-right">' +
                    '<i class="fa fa-inr"></i><span class="tax-value" data-type="' +
                    data.Data.Type1 + '" data-value="' + data.Data.Value1 + '"></span></div><div class="col-md-1 text-center"></div></div>';
                $(type1).appendTo("#taxlist");

                if (data.Data.Type2 != null && data.Data.Type2 != "") {
                    var type2 = '<div class="row"><div class="col-md-10 text-right">' +
                        data.Data.Title2 + "(" + data.Data.Value2 + (data.Data.Type2 != "Fixed" ? "%" : "") + '): </div><div class="col-md-1 text-right">' +
                        '<i class="fa fa-inr"></i><span class="tax-value" data-type="' +
                        data.Data.Type2 + '" data-value="' + data.Data.Value2 + '"></span></div><div class="col-md-1 text-center"></div></div>';
                    $(type2).appendTo("#taxlist");
                }

                if (data.Data.Type3 != null && data.Data.Type3 != "") {
                    var type3 = '<div class="row"><div class="col-md-10 text-right">' +
                        data.Data.Title3 + "(" + data.Data.Value3 + (data.Data.Type3 != "Fixed" ? "%" : "") + '): </div><div class="col-md-1 text-right">' +
                        '<i class="fa fa-inr"></i><span class="tax-value" data-type="' +
                        data.Data.Type3 + '" data-value="' + data.Data.Value3 + '"></span></div><div class="col-md-1 text-center"></div></div>';
                    $(type3).appendTo("#taxlist");
                }

                if (data.Data.Type4 != null && data.Data.Type4 != "") {
                    var type4 = '<div class="row"><div class="col-md-10 text-right">' +
                        data.Data.Title4 + "(" + data.Data.Value4 + (data.Data.Type4 != "Fixed" ? "%" : "") + '): </div><div class="col-md-1 text-right">' +
                        '<i class="fa fa-inr"></i><span class="tax-value" data-type="' +
                        data.Data.Type4 + '" data-value="' + data.Data.Value4 + '"></span></div><div class="col-md-1 text-center"></div></div>';
                    $(type4).appendTo("#taxlist");
                }

                if (data.Data.Type5 != null && data.Data.Type5 != "") {
                    var type5 = '<div class="row"><div class="col-md-10 text-right">' +
                        data.Data.Title5 + "(" + data.Data.Value5 + (data.Data.Type5 != "Fixed" ? "%" : "") + '): </div><div class="col-md-1 text-right">' +
                        '<i class="fa fa-inr"></i><span class="tax-value" data-type="' +
                        data.Data.Type5 + '" data-value="' + data.Data.Value5 + '"></span></div><div class="col-md-1 text-center"></div></div>';
                    $(type5).appendTo("#taxlist");
                }
               
            }
            calculateTotal();
        });
    } else {
        calculateTotal();
    }
}).change();