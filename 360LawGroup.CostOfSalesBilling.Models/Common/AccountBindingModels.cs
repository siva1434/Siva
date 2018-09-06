using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _360LawGroup.CostOfSalesBilling.Utilities;

namespace _360LawGroup.CostOfSalesBilling.Models {
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class LoginViewModel {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Username/Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DisplayName("Remember")]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordBindingModel {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-enter new password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "The new password and re-enter new password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UpdateProfileModel {

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Email")]
        [StringLength(256)]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string Email { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Phone Number")]
        [StringLength(15, ErrorMessage = "Phone Number must be between 10 to 15 Numbers only.", MinimumLength = 10)]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }

    public class RegisterAdminBindingModel {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Email")]
        [StringLength(256)]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string Email { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Phone Number")]
        [StringLength(15, ErrorMessage = "Phone Number must be between 10 to 15 Numbers only.", MinimumLength = 10)]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-enter password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "The password and re-enter password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [Display(Name = "First Name")]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [RegularExpression(Common.RegexName, ErrorMessage = "Only Alphabates, spaces and special characters(&,-.') allowed.")]
        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(2000)]
        [Display(Name = "Institution Address")]
        public string InstitutionAddress { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Ip Address")]
        public string IpAddress { get; set; }

        public bool AcceptTerms { get; set; }
    }

    public class RegisterPublisherBindingModel {

        public string InviteCode { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Email")]
        [StringLength(256)]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string Email { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Phone Number")]
        [StringLength(15, ErrorMessage = "Phone Number must be between 10 to 15 Numbers only.", MinimumLength = 10)]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-enter password")]
        [Compare("Password", ErrorMessage = "The password and re-enter password do not match.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Ip Address")]
        public string IpAddress { get; set; }

        public bool AcceptTerms { get; set; }
    }
    
    public class SetPasswordBindingModel {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [DataType(DataType.Password)]
        [Display(Name = "Re-enter new password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "The new password and re-enter new password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

        public string UserId { get; set; }
    }

    public class ForgotPasswordViewModel {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [Display(Name = "Email")]
        [StringLength(256)]
        public string Email { get; set; }
    }
}
