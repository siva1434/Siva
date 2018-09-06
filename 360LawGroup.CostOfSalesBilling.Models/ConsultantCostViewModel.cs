using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class ConsultantCostViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Matter[Consultant]-WorkPeriod")]
        public Guid Matter_Consultant_WorkPeriod { get; set; }

        [Display(Name = "Matter")]
        public Guid MatterId { get; set; }

        [Display(Name = "Client")]
        public Guid ClientId { get; set; }

        [Display(Name = "WorkRate")]
        public Guid WorkRateId { get; set; }

        [Display(Name = "Work Date")]
        public DateTime? WorkDate { get; set; }

        [Display(Name = "Month")]
        public DateTime? Month { get; set; }

        [Display(Name = "Mon Num")]
        public decimal? MonNum { get; set; }

        [Display(Name = "Year")]
        public DateTime? Year { get; set; }

        [Display(Name = "Client Work Period")]
        public string ClientWorkPeriod { get; set; }

        [Display(Name = "Hours")]
        public decimal? Hours { get; set; }

        [Display(Name = "Subscription Cost")]
        public decimal? SubscriptionCost { get; set; }

        [Display(Name = "Member Cost")]
        public decimal? MemberCost { get; set; }

        [Display(Name = "Private Client Cost")]
        public decimal? PrivateClientCost { get; set; }

        [Display(Name = "Litigation Cost")]
        public decimal? LitigationCost { get; set; }

        [Display(Name = "Regulated Cost")]
        public decimal? RegulatedCost { get; set; }

        [Display(Name = "Overseas Cost")]
        public decimal? OverseasCost { get; set; }

        [Display(Name = "Cost(incl Sub)")]
        public decimal? Cost_incl_Sub { get; set; }

        [Display(Name = "Cost(excl Sub)")]
        public decimal? Cost_excl_Sub { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Disbursement Description")]
        public string DisbursementDescription { get; set; }

        [Display(Name = "Disbursement Amount")]
        public decimal? DisbursementAmount { get; set; }

        [Display(Name = "Rate Used")]
        public decimal? RateUsed { get; set; }

        //public virtual Client Client { get; set; }
        //public virtual Matter Matter { get; set; }
        //public virtual WorkRate WorkRate { get; set; }
    }
}
