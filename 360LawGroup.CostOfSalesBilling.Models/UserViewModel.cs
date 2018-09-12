using System;
using System.ComponentModel.DataAnnotations;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class UserViewModel : BaseViewModel<string>
    {
        public string UserName { get; set; }

        // [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Phone Number")]
        [StringLength(15, ErrorMessage = "Number must be between 10 to 15 only.", MinimumLength = 10)]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone Number Confirmed?")]
        public bool PhoneNumberConfirmed { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "User Roles")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        [Display(Name = "Name")]
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

        //  [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Gender")]
        [StringLength(50)]
        public string Gender { get; set; }

        [Display(Name = "Send an email intro")]
        public bool? IsSendAnEmailIntro { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [StringLength(256)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        //[Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Address")]
        [StringLength(1000)]
        public string Address { get; set; }

        [Display(Name = "Address 2")]
        [StringLength(1000)]
        public string AddresslLine1 { get; set; }

        [Display(Name = "City")]
        [StringLength(1000)]
        public string City { get; set; }

        [Display(Name = "Region")]
        [StringLength(1000)]
        public string Region { get; set; }

        [Display(Name = "Country")]
        [StringLength(1000)]
        public string Country { get; set; }

        [Display(Name = "Pin Code")]
        [Range(111111, 999999, ErrorMessage = "Please Enter 6 digits {0}.")]
        public int? PinCode { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name = "Consultant Company")]
        public string ConsultantCompany { get; set; }

        [Display(Name = "Email Address - 360 Business Law")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [StringLength(256)]
        public string EmailAddress_360BusinessLaw { get; set; }

        [Display(Name = "E-mail Address - Private Client Law")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [StringLength(256)]
        public string EmailAddress_PrivateClientLaw { get; set; }

        [Display(Name = "Email Address - 360 Law Services")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [StringLength(256)]
        public string EmailAddress_360LawServices { get; set; }

        [Display(Name = "Private Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [StringLength(256)]
        public string PrivateEmailAddress { get; set; }

        [Display(Name = "Area of Law")]
        [StringLength(255)]
        public string AreaofLaw { get; set; }

        [Display(Name = "Business Phone")]
        [StringLength(15, ErrorMessage = "Number must be between 10 to 15 only.", MinimumLength = 10)]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        public string BusinessPhone { get; set; }

        [Display(Name = "Home Phone")]
        [StringLength(15, ErrorMessage = "Number must be between 10 to 15 only.", MinimumLength = 10)]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        public string HomePhone { get; set; }

        [Display(Name = "Address1")]
        public string Address1 { get; set; }

        [Display(Name = "Jurisdictions Covered")]
        [StringLength(255)]
        public string JurisdictionsCovered { get; set; }

        [Display(Name = "Notes")]
        [StringLength(int.MaxValue)]
        public string Notes { get; set; }

        [Display(Name = "Attachments")]
        public string Attachments { get; set; }

        [Display(Name = "Subscription Hourly Rate")]
        public decimal? SubscriptionHourlyRate { get; set; }

        [Display(Name = "Member Hourly Rate")]
        public decimal? MemberHourlyRate { get; set; }

        [Display(Name = "Private Client Hourly Rate")]
        public decimal? PrivateClientHourlyRate { get; set; }

        [Display(Name = "Litigation Hourly Rate")]
        public decimal? LitigationHourlyRate { get; set; }

        [Display(Name = "Regulated Hourly Rate")]
        public decimal? RegulatedHourlyRate { get; set; }

        [Display(Name = "OverseasHourlyRate")]
        public decimal? OverseasHourlyRate { get; set; }

        [Display(Name = "LastUpdated")]
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Consultant")]
        public string ConsultantName { get; set; }

        [Display(Name = "Consultant")]
        public string ConsultantUserId { get; set; }

        [Display(Name = "Client")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "User Staus")]
        [StringLength(50)]
        public string UserStatus { get; set; }

        public virtual string ClientFullName { get; set; }
        public virtual string AspNetUser2FullName { get; set; }

        public virtual string tempfileId { get; set; }
        //public virtual string AttachmentsFileName { get; set; }

    }

    public class SetPasswordViewModel
    {
        [Required(ErrorMessage = Common.RequiredMsg)]
        public string Id { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}