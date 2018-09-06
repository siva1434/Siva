$(function () {
    // Initialize tinymce editor
    tinymceEditor(".basictinymce");
});

var tinymceEditor = function (e) {
    tinymce.init({
        selector:e,
        theme: 'modern',
        height: 50,
        plugins: [
          "advlist autolink autosave link image lists charmap preview hr anchor",
          "searchreplace visualblocks visualchars code fullscreen media nonbreaking",
          "table contextmenu directionality paste fullpage autoresize"
        ],
        toolbar1: "styleselect | bold alignleft aligncenter alignright alignjustify | bullist numlist | outdent indent | undo redo | preview ",
        menubar: false,
        statusbar: false,
        toolbar_items_size: 'small',
        init_instance_callback: function (editor) {
            var id = $("#Id").val();
            if (id == "00000000-0000-0000-0000-000000000000" || id == "") {
                editor.setContent("");
            }
        },
    });
}
