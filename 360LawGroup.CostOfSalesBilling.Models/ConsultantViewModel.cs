using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class ConsultantViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "User Status")]
        public string UserStatus { get; set; }

        [Display(Name = "Consultant Company")]
        public string ConsultantCompany { get; set; }

        [Display(Name = "Email Address-360 Business Law")]
        public string EmailAddress_360BusinessLaw { get; set; }

        [Display(Name = "Email Address-Private Client Law")]
        public string EmailAddress_PrivateClientLaw { get; set; }

        [Display(Name = "Email Address-360 Law Services")]
        public string EmailAddress_360LawServices { get; set; }

        [Display(Name = "Private Email Address")]
        public string PrivateEmailAddress { get; set; }

        [Display(Name = "Area of Law")]
        public string AreaofLaw { get; set; }

        [Display(Name = "Business Phone")]
        public string BusinessPhone { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }


        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Jurisdictions Covered")]
        public string JurisdictionsCovered { get; set; }

        [Display(Name = "Notes")]
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

        [Display(Name = "RegulatedHourly Rate")]
        public decimal? RegulatedHourlyRate { get; set; }

        [Display(Name = "Overseas Hourly Rate")]
        public decimal? OverseasHourlyRate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Consultant")]
        public string ConsultantName { get; set; }
    }
}
