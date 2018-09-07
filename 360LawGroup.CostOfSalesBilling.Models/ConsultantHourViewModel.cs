using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class ConsultantHourViewModel : BaseViewModel<Guid>
    {

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Consultant")]
        public Guid ConsultantId { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Matter")]
        public Guid MatterId { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Client")]
        public Guid ClientId { get; set; }

        [Display(Name = "Work Date")]
        public DateTime? WorkDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Work Hours")]
        public decimal? WorkHours { get; set; }

        [Display(Name = "Current Month")]
        public bool? CurrentMonth { get; set; }

        [Display(Name = "Cost Updated")]
        public bool? CostUpdated { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Work Rate ID")]
        public decimal? WorkRateId { get; set; }

        [Display(Name = "Agreed")]
        public bool? Agreed { get; set; }

        [Display(Name = "Reset New Month")]
        public Guid? ResetNewMonthId { get; set; }

        [Display(Name = "Disbursement Description")]
        public string DisbursementDescription { get; set; }

        [Display(Name = "Disbursement Amount")]
        public decimal? DisbursementAmount { get; set; }

        [Display(Name = "Hours")]
        public decimal? Hours { get; set; }

        [Display(Name = "Minutes")]
        public decimal? Minutes { get; set; }

        [Display(Name = "Subscription Hours")]
        public decimal? SubscriptionHours { get; set; }

        [Display(Name = "Member Hours")]
        public decimal? MemberHours { get; set; }

        [Display(Name = "Private Client Hours")]
        public decimal? PrivateClientHours { get; set; }

        [Display(Name = "Litigation Hours")]
        public decimal? LitigationHours { get; set; }

        [Display(Name = "Consultant")]
        public decimal? RegulatedHours { get; set; }

        [Display(Name = "Overseas Hours")]
        public decimal? OverseasHours { get; set; }

        [Display(Name = "Last Date")]
        public DateTime? LastDate { get; set; }

        [Display(Name = "Mon Num")]
        public int? MonNum { get; set; }

        [Display(Name = "Month Name")]
        public string MonthName { get; set; }

        [Display(Name = "Year")]
        public int? Year { get; set; }

        [Display(Name = "TotalHours(excl Sub)")]
        public decimal? TotalHours_excl_Sub { get; set; }

        [Display(Name = "Month")]
        public string Month { get; set; }

        public virtual decimal? OverseasCost { get; set; }
        public virtual string MatterWorkPeriod { get; set; }
        public virtual string Matter_Consultant_WorkPeriod { get; set; }
        //public virtual string ClientWorkPeriod { get; set; }
        public virtual decimal? SubscriptionCost { get; set; }
        public virtual decimal? MemberCost { get; set; }
        public virtual decimal? PrivateClientCost { get; set; }
        public virtual decimal? LitigationCost { get; set; }
        public virtual decimal? RegulatedCost { get; set; }
        public virtual decimal? TotalCost_inclSub { get; set; }
        public virtual decimal? TotalCost_exclSub { get; set; }
        public virtual string WorkRateText { get; set; }
        public virtual string ClientWorkPeriod { get; set; }

        //consultant
        public virtual string AspNetUser2FullName { get; set; }
        //public virtual Client Client { get; set; }
        //public virtual Consultant Consultant { get; set; }
        //public virtual Matter Matter { get; set; }
    }
}
