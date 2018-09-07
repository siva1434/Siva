using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class MatterViewModel : BaseViewModel<Guid>
    {
        [Required(ErrorMessage = (Common.RequiredMsg))]
        [Display(Name = "Matter")]
        public string MatterName { get; set; }

        [Required(ErrorMessage = (Common.RequiredMsg))]
        [Display(Name = "Client")]
        public Guid ClientId { get; set; }


        [Display(Name = "Consultant")]
        public string ConsultantId { get; set; }

        [Required(ErrorMessage = (Common.RequiredMsg))]
        [Display(Name = "Consultants")]
        public virtual List<string> ConsultantIds { get; set; }

        [Required(ErrorMessage = (Common.RequiredMsg))]
        [Display(Name = "Work Rate")]
        public Guid WorkRateId { get; set; }

        [Display(Name = "Work Rate Choice")]
        public string WorkRateChoice { get; set; }

        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        [Display(Name = "Current Month")]
        public bool CurrentMonth { get; set; }

        [Display(Name = "Area of Law")]
        public string AreaofLaw { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required(ErrorMessage = (Common.RequiredMsg))]
        [Display(Name = "Box")]
        public string Box { get; set; }

        [Display(Name = "Overseas")]
        public bool? Overseas { get; set; }

        [Display(Name = "Overseas Country")]
        public string OverseasCountry { get; set; }

        [Display(Name = "Overseas Charge Rate")]
        public decimal? OverseasChargeRate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Billable")]
        public bool? Billable { get; set; }

        [Display(Name = "Number")]
        public long Number { get; set; }

        [Display(Name = "Client-Matter Contact")]
        public string ClientMatterContact { get; set; }

        [Display(Name = "Client-Matter Contact Email")]
        public string ClientMatter_ContactEmail { get; set; }

        [Display(Name = "Mon Num")]
        public int? MonNum { get; set; }

        [Display(Name = "Month Name")]
        public string MonthName { get; set; }

        public virtual string WorkRateText { get; set; }
        public virtual string ClientText { get; set; }
        public virtual decimal? WorkHours { get; set; }
        public virtual decimal? SubscriptionCost { get; set; }
        public virtual decimal? MemberCost { get; set; }

        public virtual decimal? PrivateClientCost { get; set; }
        public virtual decimal? PrivateClientCharge { get; set; }
        public virtual decimal? LitigationCost { get; set; }
        public virtual decimal? LitigationCharge { get; set; }
        public virtual decimal? RegulatedCost { get; set; }
        public virtual decimal? RegulatedCharge { get; set; }
        public virtual decimal? OverseasCost { get; set; }
        public virtual decimal? OverseasCharge { get; set; }
        public virtual decimal? CheckCost_inclSub { get; set; }
        public virtual decimal? CheckCost_exclSub { get; set; }
        public virtual decimal? DisbursementAmount { get; set; }

        public virtual decimal? MemberCharge { get; set; }
        public virtual string ClientFullName { get; set; }
        public virtual string WorkRateRateType { get; set; }
        //public virtual string AspNetUserConsultantName { get; set; }

        public virtual IList<UserViewModel> AspNetUsers { get; set; }
    }
}
