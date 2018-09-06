$(function () {
    // Initialize tinymce editor
    tinymceEditor(".tinymce");
    var qid = $("#Id").val();
    if (qid !== "00000000-0000-0000-0000-000000000000" && qid !== "") {
        $("#TypeCode").trigger("change");
    }

    $(document).on("change", "input.answer", function () {
        var id = $(this).attr("id");
        var checked = $(this).is(":checked");
        $(this).parents("td").find("input[type='hidden']").val(checked);
    });
});

var tinymceEditor = function (e) {
    tinymce.init({
        selector: e,
        theme: "modern",
        height: 50,
        plugins: [
            "advlist autolink autosave link image lists charmap preview hr anchor",
            "searchreplace visualblocks visualchars code fullscreen media nonbreaking",
            "table contextmenu directionality paste fullpage autoresize codesample"
        ],
        codesample_languages: [
            { text: "HTML/XML", value: "markup" },
            { text: "JavaScript", value: "javascript" },
            { text: "CSS", value: "css" },
            { text: "PHP", value: "php" },
            { text: "Ruby", value: "ruby" },
            { text: "Python", value: "python" },
            { text: "Java", value: "java" },
            { text: "C", value: "c" },
            { text: "C#", value: "csharp" },
            { text: "C++", value: "cpp" }
        ],
        toolbar1: "styleselect | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | bullist numlist | outdent indent | undo redo | codesample",
        toolbar2: "| link unlink image | preview | forecolor backcolor table | hr removeformat | subscript superscript | ltr rtl | spellchecker | visualchars visualblocks template pagebreak| addfb",
        menubar: false,
        statusbar: false,
        toolbar_items_size: "small",
        file_picker_callback: function (callback, value, meta) {
            if (meta.filetype === "image") {
                var input = $("<input type='file'/>");
                input.click();
                input.on("change", function () {
                    var files = $(this)[0].files;
                    if (files.length > 0) {
                        var file = files[0];
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            callback(e.target.result,
                                {
                                    alt: file.name
                                });
                        };
                        reader.readAsDataURL(file);
                    }
                });
            }
        },
        file_picker_types: "image",
        image_title: true,
        setup: function (editor) {
            var typeCode = $("#TypeCode").val();
            if (typeCode === "" || typeCode == null) {
                editor.addButton("addfb", {
                    text: "Add Text Entry",
                    icon: false,
                    onclick: function () {
                        var thtml = FBTextBox();
                        if (thtml != "") {
                            editor.insertContent(thtml);
                        }
                    },
                    onchange: function () {
                        editor.save();
                        tinyMCE.triggerSave();
                    },
                });
                editor.on("blur", function (e) {
                    InsertFbTableTextBox();
                });
            }
        },
        init_instance_callback: function (editor) {
            //var qid = $("#Id").val();
            //if (qid == "00000000-0000-0000-0000-000000000000" || qid == "") {
            //    editor.setContent("");
            //}
        },
        content_css: []
    });
}
var checkbox = function () {
    $(".styled").uniform({
        radioClass: "choice"
    });
}
var fbCounter = 1;
function FBTextBox() {
    var typeCode = $("#TypeCode").val();
    if (typeCode === "FB") {
        var thtml = '<input type="text" class="qtb" readonly id="qtb-' + fbCounter + '"/>';
        fbCounter++;
        return thtml;
    }
    else {
        bootbox.alert({
            title: "Error",
            message: "Please Select Question Category Fill in the blank",
        });
        return "";
    }
}

function InsertFbTableTextBox() {
    var fc = 0;
    var thtml = "";
    var typeCode = $("#TypeCode").val();
    if (typeCode === "FB") {
        var _content = tinyMCE.get("Title").getContent();
        if (_content != null) {
            var tc = $(_content).find("input").length;
            if (tc > 0) {
                for (var i = 1; i <= tc; i++) {
                    thtml +=
                        '<tr class="QAttribute" id="' + i + '">' +
                        '<td class="Srno">' + i + "</td>" +
                        "<td>" +
                        '<input type="text" name="QzAnswers[' + fc + '].AnsAttributes" class="form-control qtb answer"/>' +
                        "</td>" +
                        "</tr>";
                    fc++;
                }
                $("tbody#FB tr").remove();
                $("tbody#FB").append(thtml);
            }
            else {
                $("tbody#FB tr").remove();
            }
        }
    }
}

$("#TypeCode").change(function () {
    var qid = $("#Id").val();
    var typeCode = $(this).val();
    if (typeCode === "" || typeCode == null)
        return false;
    $("#qpattern").load(BASEPATH + $(this).attr("data-typeurl") + "?id=" + typeCode + "&qid=" + qid, function (response) {
        $("#qpattern").html(response);
        tinymce.remove("." + typeCode);
        tinymceEditor("." + typeCode);
        if (typeCode === "MC" || typeCode === "YN" || typeCode === "TF") {
            $("input[type='checkbox'].answer").on("change", function () {
                var checked = $(this).is(":checked");
                $("input[type='checkbox'].answer").prop("checked", false);
                $(this).prop("checked", checked);
                $.uniform.update('.styled');
            });
        }
        $.uniform.update('.styled');
    });
    if (typeCode !== "FB") {
        $("#addFBAnswer").removeClass("show").addClass("hide");
    }
    else {
        $("#addFBAnswer").removeClass("hide").addClass("show");
    }
});

function MoreAnswerChoice(type) {
    var sGuid = "";
    for (var i = 0; i < 32; i++) {
        sGuid += Math.floor(Math.random() * 0xF).toString(0xF);
    }
    var srno = $("#" + type).children("tr").length;
    var temp = "";
    if (type == "MT") {
        temp = "<tr id=" + sGuid + ' class="QAttribute">' +
            '<td class="Srno">' + (srno + 1) + "</td>" +
            "<td>" +
            '<textarea name="QzAnswers[' + (srno) + '].QAttributes" class="form-control MT question" placeholder="Enter question choice"></textarea>' +
            "</td>" +
            "<td>" +
            '<textarea name="QzAnswers[' + (srno) + '].AnsAttributes" class="form-control MT answer" placeholder="Enter answer choice"></textarea>' +
            "</td>" +
            "<td>" +
            '<a href="javascript:void(0)" class="label label-danger label-icon" onclick="QCRemove(this);"><i class="icon-trash"></i></a>' +
            "</td></tr>";

    }
    if (type == "MC") {
        temp = "<tr id=" + sGuid + ' class="QAttribute">' +
            '<td class="Srno">' + (srno + 1) + "</td>" +
            "<td>" +
            '<textarea name="QzAnswers[' + (srno) + '].QAttributes" class="form-control MC question" placeholder="Enter question choice"></textarea>' +
            "</td>" +
            "<td>" +
            '<input type="checkbox" class="answer styled" id=' + (srno) + '>' +
            '<input type="hidden" name="QzAnswers[' + (srno) + '].AnsAttributes"  value="false" id="h_' + (srno) + ' "/>' +
            "</td>" +
            "<td>" +
            '<a href="javascript:void(0)" class="label label-danger label-icon" onclick="QCRemove(this);"><i class="icon-trash"></i></a>' +
            "</td></tr>";
    }
    if (type == "MS") {
        temp = "<tr id=" + sGuid + ' class="QAttribute">' +
            '<td class="Srno">' + (srno + 1) + "</td>" +
            "<td>" +
            '<textarea name="QzAnswers[' + (srno) + '].QAttributes" class="form-control MS question" placeholder="Enter question choice"></textarea>' +
            "</td>" +
            "<td>" +
            '<input type="checkbox" class="answer styled" id=' + (srno) + '>' +
            '<input type="hidden" name="QzAnswers[' + (srno) + '].AnsAttributes"  value="false" id="h_' + (srno) + ' "/>' +
            "</td>" +
            "<td>" +
            '<a href="javascript:void(0)" class="label label-danger label-icon" onclick="QCRemove(this);"><i class="icon-trash"></i></a>' +
            "</td></tr>";
    }
    if (type == "OR") {
        temp = "<tr id=" + sGuid + ' class="QAttribute">' +
            '<td class="Srno">' + (srno + 1) + "</td>" +
            "<td>" +
            '<textarea name="QzAnswers[' + (srno) + '].QAttributes" class="form-control OR question" placeholder="Enter question choice"></textarea>' +
            "</td>" +
            "<td>" +
            '<input type="number" name="QzAnswers[' + (srno) + '].AnsAttributes" class="form-control qtb answer" />' +
            "</td>" +
            "<td>" +
            '<a href="javascript:void(0)" class="label label-danger label-icon" onclick="QCRemove(this);"><i class="icon-trash"></i></a>' +
            "</td></tr>";
    }
    $("#" + type).append(temp);
    tinymce.remove("." + type);
    tinymceEditor("." + type);
    checkbox();
}
function QCRemove(elem) {
    var tr = $(elem).closest("tr");
    var tbody = $(elem).closest("tbody");
    var tbodyId = $(tbody).attr("id");
    $(tr).remove();
    ReOrder(tbodyId);
}
function deleteNumericEntryContent(elem) {
    var div = $(elem).closest("div.qtb-wrapper");
    var divId = $(div).attr("id");
    $(div).remove();
    ReOrder(tbodyId);
}
function ReOrder(tbodyId) {
    var id = "#" + tbodyId + " tr";
    var c = 0;
    $(id).each(function (index, el) {
        $(this).children("td").first().text(++index);
        $(this).find(".question").attr("name", "QzAnswers[" + (c) + "].QAttributes");
        $(this).find(".answer").attr("name", "QzAnswers[" + (c) + "].AnsAttributes");
        c++;
    });
}