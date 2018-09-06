using System;
using System.Configuration;
using System.Web.Optimization;

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/js/core/libraries/jquery.min.js",
                        "~/assets/js/core/libraries/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/validations").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/assets/js/plugins/forms/styling/uniform.min.js",
                        "~/assets/js/core/libraries/jquery_ui/interactions.min.js",
                        "~/assets/js/core/libraries/jquery_ui/touch.min.js",
                        "~/assets/js/core/app.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                "~/assets/js/plugins/forms/selects/select2.min.js",
                "~/assets/js/plugins/tables/datatables/datatables.min.js",
                "~/assets/js/pages/datatables_basic.js",
                "~/Scripts/bootstrap-table.js",
                "~/Scripts/bootstrap-div.js",
                "~/Scripts/nprogress.js",
                "~/Scripts/bootbox.js",
                "~/assets/js/plugins/ui/moment/moment.min.js",
                "~/assets/js/plugins/pickers/daterangepicker.js",
                "~/assets/js/plugins/pickers/anytime.min.js",
                "~/assets/js/plugins/pickers/pickadate/picker.js",
                "~/assets/js/plugins/pickers/pickadate/picker.date.js",
                "~/assets/js/plugins/pickers/pickadate/picker.time.js",
                "~/assets/js/plugins/pickers/pickadate/legacy.js",
                "~/assets/js/pages/picker_date.js",
                "~/assets/js/pages/appearance_draggable_panels.js",
                "~/Scripts/fileupload/jquery.fileupload.js",
                "~/assets/js/plugins/notifications/pnotify.min.js",
                "~/assets/js/plugins/media/fancybox.min.js",
                "~/assets/js/plugins/tinypicker/calendar.js",
                "~/assets/js/plugins/tinypicker/datePicker.js",
                "~/assets/js/plugins/tinypicker/jqDatePicker.js",
                "~/assets/js/plugins/forms/wizards/steps.min.js",
                "~/assets/js/plugins/stepper/js/jquery.easing.min.js",
                "~/assets/js/plugins/stepper/stepper-2.js",
                "~/assets/js/plugins/ScrollBar/includes/prettify/prettify.js",
                "~/assets/js/plugins/ScrollBar/jquery.scrollbar.js",                
                "~/Scripts/common.js",
                "~/Scripts/custom.js"
            ));

            bundles.Add(new ScriptBundle("~/assets/js/plugins/tinymce/bundle").Include("~/assets/js/plugins/tinymce/tinymce.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/questionbank").Include(
                "~/Scripts/questionbank.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/paper").Include(
               "~/Scripts/paper.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/paymenteditor").Include(
               "~/Scripts/pages/paymenteditor.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/css/paper").Include(
               "~/assets/css/custom-paper.css",
               "~/assets/css/paper.css"
            ));

            // will add later on
            bundles.Add(new ScriptBundle("~/bundles/notification").Include(
                "~/Scripts/signalr/notify.js",
                "~/Scripts/signalr/notification.js"));

            // Client renewal Calender 
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                "~/assets/js/plugins/ui/fullcalendar/fullcalendar.min.js"));

            //styling
            bundles.Add(new StyleBundle("~/bundles/css/global").Include(
                            "~/assets/css/icons/icomoon/styles.css",
                            "~/assets/css/icons/fontawesome/styles.min.css",
                            "~/assets/css/bootstrap.css",
                            "~/assets/css/core.css",
                            "~/assets/css/custom.css",
                            "~/assets/css/components.css",
                            "~/assets/js/plugins/stepper/stepper-2.css",
                            "~/assets/js/plugins/ScrollBar/includes/prettify/prettify.css",
                            "~/assets/js/plugins/ScrollBar/jquery.scrollbar.css",
                            "~/assets/css/colors.css",
                            "~/assets/js/plugins/tinypicker/datePicker.css",
                            "~/assets/css/nprogress.css",
                            "~/Scripts/fileupload/jquery.fileupload.css",
                            "~/assets/css/bootstrap-table.css"
                            ));
            var enableOptimizations = ConfigurationManager.AppSettings["EnableOptimizations"] ?? string.Empty;
            BundleTable.EnableOptimizations = enableOptimizations.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
}