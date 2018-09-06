using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using _360LawGroup.CostOfSalesBilling.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExt
    {
        public static MvcHtmlString RequiredLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string labelText, object htmlAttributes = null)
        {
            var tagBuilder = new TagBuilder("label");
            if (htmlHelper == null || expression == null)
                return new MvcHtmlString(string.Empty);
            if (string.IsNullOrEmpty(labelText))
                tagBuilder.InnerHtml += htmlHelper.DisplayNameFor(expression).ToHtmlString();
            else
                tagBuilder.InnerHtml += labelText;
            if (expression.IsRequired(htmlHelper))
            {
                tagBuilder.InnerHtml += " <span class=\"text-danger\">*</span>";
            }
            if (htmlAttributes != null)
            {
                Collections.Generic.IDictionary<string, object> attrs = new System.Web.Routing.RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(attrs);
            }
            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString RequiredLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return htmlHelper.RequiredLabelFor(expression, null, htmlAttributes);
        }

        public static T GetAttribute<T>(this ICustomAttributeProvider provider)
            where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(T), true);
            return attributes.Length > 0 ? attributes[0] as T : null;
        }

        private static bool IsRequired<T, V>(this Expression<Func<T, V>> expression, HtmlHelper<T> htmlHelper)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            var fieldMetadata = htmlHelper.ApplyFieldValidationMetadata(modelMetadata, modelName);
            var tp = expression?.Body.Type ?? typeof(object);
            foreach (var item in fieldMetadata.ValidationRules)
            {
                if (item.ValidationType == "required" && tp != typeof(bool) && tp != typeof(bool?))
                    return true;
            }
            return false;
        }

        private static FieldValidationMetadata ApplyFieldValidationMetadata<T>(this HtmlHelper<T> htmlHelper, ModelMetadata modelMetadata, string modelName)
        {
            var formContext = htmlHelper.ViewContext.FormContext;
            var fieldMetadata = formContext.GetValidationMetadataForField(modelName, true /* createIfNotFound */);
            // write rules to context object
            var validators = ModelValidatorProviders.Providers.GetValidators(modelMetadata, htmlHelper.ViewContext);
            foreach (var rule in validators.SelectMany(v => v.GetClientValidationRules()))
            {
                fieldMetadata.ValidationRules.Add(rule);
            }
            return fieldMetadata;
        }

        public static List<SelectListItem> ActiveDeactiveListForUserStaus(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Active", Value = "true" },
                    new SelectListItem { Text = "DeActive", Value = "false" },
                    new SelectListItem{Text="Pending Approval",Value="Pending Approval"}
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> isSentEmailIntroList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "No", Value = "false"},
                    new SelectListItem { Text = "Yes", Value = "true" },
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }

        public static List<SelectListItem> PaymentMethodList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "cash", Value = "Cash" },
                    new SelectListItem { Text = "Card", Value = "Card" },
                    new SelectListItem { Text = "Cheque", Value = "Cheque" },

                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> RelationList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Father", Value = "father" },
                    new SelectListItem { Text = "Mother", Value = "Mother" },
                    new SelectListItem { Text = "Uncle", Value = "Uncle" },
                    new SelectListItem { Text = "Aunty", Value = "Aunty" },
                    new SelectListItem { Text = "Brother", Value = "Brother" },
                    new SelectListItem { Text = "Sister", Value = "Sister" },
                    new SelectListItem { Text = "Wife", Value = "Wife" },
                    new SelectListItem { Text = "Son", Value = "Son" },
                    new SelectListItem { Text = "Daughter", Value = "Daughter" },
                    new SelectListItem { Text = "Grand-Father", Value = "Grand-Father" },
                    new SelectListItem { Text = "Grand-Mother", Value = "Grand-Mother" },

                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }

        public static List<SelectListItem> GoodiesList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Member", Value = "Member" },
                    new SelectListItem { Text = "Non-Member", Value = "Non-Member" },
                    new SelectListItem { Text = "Both", Value = "Both" }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }

        public static List<SelectListItem> PersonalNutritionPlansList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Member", Value = "Member" },
                    new SelectListItem { Text = "Non-Member", Value = "Non-Member" },
                    new SelectListItem { Text = "Both", Value = "Both" }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        /*
        public static List<SelectListItem> FollowUpStatusList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = FollowUpStatus.Green, Value = FollowUpStatus.Green },
                    new SelectListItem { Text = FollowUpStatus.Amber, Value = FollowUpStatus.Amber },
                    new SelectListItem { Text = FollowUpStatus.Red, Value = FollowUpStatus.Red },
                    //new SelectListItem { Text = FollowUpStatus.Closed, Value = FollowUpStatus.Closed }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        */
        public static List<SelectListItem> PTTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Regular", Value = "Regular" },
                    new SelectListItem { Text = "Alternate", Value = "Alternate" }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }

        public static List<SelectListItem> PackageTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem{Text="Please select",Value=""},
                    new SelectListItem { Text = PTPackageTypes.Regular, Value = PTPackageTypes.Regular },
                    new SelectListItem { Text = PTPackageTypes.Silver, Value = PTPackageTypes.Silver },
                    new SelectListItem { Text = PTPackageTypes.Gold, Value = PTPackageTypes.Gold },
                    new SelectListItem { Text = PTPackageTypes.Platinum, Value =PTPackageTypes.Platinum  }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }

        public static List<SelectListItem> TaxTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = TaxTypes.Fixed, Value = TaxTypes.Fixed },
                    new SelectListItem { Text = TaxTypes.Percentage, Value = TaxTypes.Percentage }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        /*
        public static List<SelectListItem> FitnessTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = FitnessType.Membership, Value = FitnessType.Membership },
                    new SelectListItem { Text = FitnessType.PersonalTraining, Value = FitnessType.PersonalTraining },
                     new SelectListItem { Text = FitnessType.Both, Value = FitnessType.Both }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }

        public static List<SelectListItem> CounsellingTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = FitnessType.Membership, Value = FitnessType.Membership },
                    new SelectListItem { Text = FitnessType.PersonalTraining, Value = FitnessType.PersonalTraining },
                     new SelectListItem { Text = FitnessType.Both, Value = FitnessType.Both }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> FitnessTypeIsintList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = IsIntType.Yes, Value = "true" },
                    new SelectListItem { Text = IsIntType.No, Value = "false" },
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> TrialStatusList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Availed", Value = "Availed" },
                    new SelectListItem { Text = "Not Availed", Value = "Not Availed" }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> RedeemVoucherList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "None", Value = "0" ,Selected = true},
                    new SelectListItem { Text = "1", Value = "1" },
                    new SelectListItem { Text = "2", Value = "2" },
                    new SelectListItem { Text = "3", Value = "3" }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> PurposeTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = RoleExtension.TrainerHead, Value = RoleExtension.TrainerHead },
                    new SelectListItem { Text = RoleExtension.Dietitian, Value =RoleExtension.Dietitian }
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> ProofTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Please select", Value = "" },
                    new SelectListItem { Text = "Pan Card", Value = "Pan Card" },
                    new SelectListItem { Text = "Election Card", Value = "Election Card" },
                    new SelectListItem { Text = "Aadhaar Card", Value = "Aadhaar Card" },
                    new SelectListItem { Text = "Passport", Value = "Passport" },
                    new SelectListItem { Text = "Driving License", Value = "Driving License" },

                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
            return list;
        }
        public static List<SelectListItem> FUTypeList(this HtmlHelper htmlHelper, bool all = false)
        {
            var list = new List<SelectListItem>
                {
                    new SelectListItem { Text = FollowUpType.AppointmentPayment, Value = FollowUpType.AppointmentPayment },
                    new SelectListItem { Text = FollowUpType.Enquiry, Value = FollowUpType.Enquiry },
                    new SelectListItem { Text = FollowUpType.MembershipRequired, Value = FollowUpType.MembershipRequired },
                    new SelectListItem { Text = FollowUpType.ProductPlanPayment, Value = FollowUpType.ProductPlanPayment },
                    new SelectListItem { Text = FollowUpType.Trail, Value = FollowUpType.Trail },
                    new SelectListItem { Text = FollowUpType.ActivationDate, Value = FollowUpType.ActivationDate },
                    new SelectListItem { Text = FollowUpType.Anniversary, Value = FollowUpType.Anniversary },
                    new SelectListItem { Text = FollowUpType.Balance, Value = FollowUpType.Balance },
                    new SelectListItem { Text = FollowUpType.BirthdayWishes, Value = FollowUpType.BirthdayWishes },
                    new SelectListItem { Text = FollowUpType.DietCounseling, Value = FollowUpType.DietCounseling },
                    new SelectListItem { Text = FollowUpType.FitnessTestBMIMeasurements, Value = FollowUpType.FitnessTestBMIMeasurements },
                    new SelectListItem { Text = FollowUpType.IrregularMember, Value = FollowUpType.IrregularMember },
                    new SelectListItem { Text = FollowUpType.PTEnquiry, Value = FollowUpType.PTEnquiry },
                    new SelectListItem { Text = FollowUpType.PTRenewal, Value = FollowUpType.PTRenewal },
                    new SelectListItem { Text = FollowUpType.Photo, Value = FollowUpType.Photo },
                    new SelectListItem { Text = FollowUpType.Referral, Value = FollowUpType.Referral },
                    new SelectListItem { Text = FollowUpType.TargetResult, Value = FollowUpType.TargetResult },
                    new SelectListItem { Text = FollowUpType.Upgrade, Value = FollowUpType.Upgrade },
                };
            if (all)
                list.Insert(0, new SelectListItem { Text = "All Followup", Value = "", Selected = true });
            return list;
        }
        */

        public static bool UserIsInRole(this HtmlHelper htmlHelper, string role)
        {
            var valid = false;
            if (!string.IsNullOrEmpty(role))
            {
                var loginCookies = WebCookie.Get("WebLogin");
                if (loginCookies != null)
                {
                    var currentUser = JsonConvert.DeserializeObject<Token>(loginCookies);
                    var userProfile = JsonConvert.DeserializeObject<UserViewModel>(currentUser.profile); ;
                    valid = role == userProfile.RoleId;
                }
            }
            return valid;
        }

        public static bool UserIsInRole(string role)
        {
            return UserIsInRole(null, role);
        }
    }
}