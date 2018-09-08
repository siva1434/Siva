using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class ClientViewModel : BaseViewModel<Guid>
    {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Company")]
        [StringLength(100)]
        public string Company { get; set; }

        [Display(Name = "First Name")]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100)]
        [RegularExpression(Common.RegexAlphaSpace, ErrorMessage = "Only Alphabates & spaces allowed.")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        [StringLength(256)]
        [Display(Name = "E-mail Address")]
        public string Email { get; set; }

        [Display(Name = "Job Title")]
        [StringLength(50)]
        public string JobTitle { get; set; }

        [Display(Name = "Business Phone")]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        [StringLength(15, ErrorMessage = "Number must be between 10 to 15 only.", MinimumLength = 10)]
        public string BusinessPhone { get; set; }

        [Display(Name = "Fax Number")]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        [StringLength(15, ErrorMessage = "Number must be between 10 to 15 only.", MinimumLength = 10)]
        public string FaxNumber { get; set; }

        [Display(Name = "Mobile Number")]
        [StringLength(15, ErrorMessage = "Number must be between 10 to 15 only.", MinimumLength = 10)]
        [RegularExpression(Common.RegexNum, ErrorMessage = "Only numbers allowed.")]
        public string MobileNumber { get; set; }

        [Display(Name = "Address")]
        [StringLength(int.MaxValue)]
        public string Address { get; set; }

        [Display(Name = "Address 1")]
        [StringLength(int.MaxValue)]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address 2")]
        [StringLength(int.MaxValue)]
        public string AddressLIne2 { get; set; }

        [Display(Name = "City")]
        [StringLength(100)]
        public string City { get; set; }

        [Display(Name = "Country")]
        [StringLength(100)]
        public string Country { get; set; }

        [Display(Name = "Province/Region")]
        [StringLength(100)]
        public string Region { get; set; }

        [Display(Name = "Postal Code")]
        [Range(111111, 999999, ErrorMessage = "Please Enter 6 digits {0}.")]
        public int? PinCode { get; set; }

        [Display(Name = "Contracted Countries")]
        public string ContractedCountries { get; set; }

        [Display(Name = "Contract Renewal Date")]
        public DateTime? ContractRenewalDate { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Attechments")]
        public string Attechments { get; set; }

        [Display(Name = "Subscription")]
        public bool IsSubscription { get; set; }

        [Display(Name = "Subscription Basis")]
        public string SubscriptionBasis { get; set; }

        [Display(Name = "Monthly Subscription")]
        public decimal? MonthlySubscription { get; set; }

        [Display(Name = "Subscription Date")]
        public DateTime? SubscriptionDate { get; set; }

        [Display(Name = "Subscription Type")]
        [StringLength(50)]
        public string SubscriptionType { get; set; }

        [Display(Name = "Payment Months")]
        [StringLength(50)]
        public string PaymentMonths { get; set; }

        [Display(Name = "Member Charge Rate")]
        public decimal? MemberChargeRate { get; set; }

        [Display(Name = "Private Client Charge Rate")]
        public decimal? PrivateClientChargeRate { get; set; }

        [Display(Name = "Litigation Charge Rate")]
        public decimal? LitigationChargeRate { get; set; }

        [Display(Name = "Regulated Charge Rate")]
        public decimal? RegulatedChargeRate { get; set; }

        [Display(Name = "SPARE Charge Rate")]
        public decimal? SPAREChargeRate { get; set; }

        [Display(Name = "Work this month?")]
        public bool InWorkThisMonth { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Subscription Charge")]
        public decimal? SubscriptionCharge { get; set; }

        [Display(Name = "Next Subscription Date")]
        public DateTime? NextSubscriptionDate { get; set; }

        [Display(Name = "Next Subscription Month")]
        public int NextSubMonth { get; set; }


        public virtual decimal? MatterWorkHours { get; set; }
        public virtual decimal? SubscriptionCost { get; set; }
        public virtual decimal? MemberHours { get; set; }
        public virtual decimal? MemberCosts { get; set; }
        public virtual decimal? PrivateClientHours { get; set; }
        public virtual decimal? PrivateClientCost { get; set; }
        public virtual decimal? LitigationHours { get; set; }
        public virtual decimal? LitigationCosts { get; set; }
        public virtual decimal? RegulatedHours { get; set; }
        public virtual decimal? RegulatedCost { get; set; }
        public virtual decimal? OverseasHours { get; set; }
        public virtual decimal? OverseasCost { get; set; }
        public virtual decimal? OverseasCharge { get; set; }
        public virtual decimal? Matters_CurrentMonth { get; set; }
        public virtual decimal? Matters_Total { get; set; }
        public virtual decimal? SubscriptionFeeMinusCosts { get; set; }
        public virtual decimal? MemberCharge { get; set; }
        public virtual decimal? PrivateClientCharge { get; set; }
        public virtual decimal? LitigationCharge { get; set; }
        public virtual decimal? RegulatedCharge { get; set; }

        public virtual decimal? TotalMonthlySubscrptionForAll { get; set; }
    }
}
