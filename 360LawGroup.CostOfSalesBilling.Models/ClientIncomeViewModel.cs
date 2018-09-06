using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class ClientIncomeViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Client")]
        public Guid ClientId { get; set; }

        [Display(Name = "Reset New Month")]
        public Guid ResetNewMonthId { get; set; }

        [Display(Name = "Mon Num")]
        public long MonNum { get; set; }

        [Display(Name = "Month")]
        public string Month { get; set; }

        [Display(Name = "Year")]
        public long Year { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Total Hours")]
        public long TotalHours { get; set; }

        [Display(Name = "Subscription Fee")]
        public decimal SubscriptionFee { get; set; }

        [Display(Name = "Subscription Cost")]
        public decimal SubscriptionCost { get; set; }

        [Display(Name = "SubscriptionFee - Cost")]
        public decimal? SubscriptionFee_Cost { get; set; }

        [Display(Name = "Member Cost")]
        public decimal? MemberCost { get; set; }

        [Display(Name = "Member Charge")]
        public decimal? MemberCharge { get; set; }

        [Display(Name = "Private Client Cost")]
        public decimal? PrivateClientCost { get; set; }

        [Display(Name = "Private Client Charge")]
        public decimal? PrivateClientCharge { get; set; }

        [Display(Name = "Litigation Cost")]
        public decimal? LitigationCost { get; set; }

        [Display(Name = "Litigation Charge")]
        public decimal? LitigationCharge { get; set; }

        [Display(Name = "Regulated Cost")]
        public decimal? RegulatedCost { get; set; }

        [Display(Name = "Regulated Charge")]
        public decimal? RegulatedCharge { get; set; }

        [Display(Name = "Overseas Cost")]
        public decimal? OverseasCost { get; set; }

        [Display(Name = "Overseas Charge")]
        public decimal? OverseasCharge { get; set; }

        [Display(Name = "Total Cost")]
        public decimal? TotalCost { get; set; }

        [Display(Name = "Total Charge")]
        public decimal? TotalCharge { get; set; }

        [Display(Name = "Subscription Profit")]
        public decimal? SubscriptionProfit { get; set; }

        [Display(Name = "Subscription Profit(%)")]
        public decimal? SubscriptionProfitPercet { get; set; }

        [Display(Name = "Member Profit")]
        public decimal? MemberProfit { get; set; }

        [Display(Name = "Member Profit(%)")]
        public decimal? MemberProfitPercent { get; set; }

        [Display(Name = "Private Client Profit")]
        public decimal? PrivateClientProfit { get; set; }

        [Display(Name = "Private Client Profit(%)")]
        public decimal? PrivateClientProfitPercent { get; set; }

        [Display(Name = "Litigation Profit")]
        public decimal? LitigationProfit { get; set; }

        [Display(Name = "Litigation Profit(%)")]
        public decimal? LitigationProfitPercent { get; set; }

        [Display(Name = "Regulated Profit")]
        public decimal? RegulatedProfit { get; set; }

        [Display(Name = "Regulated Profit(%)")]
        public decimal? RegulatedProfitPercent { get; set; }

        [Display(Name = "Overseas Profit")]
        public decimal? OverseasProfit { get; set; }

        [Display(Name = "OverseasProfit(%)")]
        public decimal? OverseasProfitPercent { get; set; }

        [Display(Name = "Charge - Cost")]
        public decimal? ChargeMinusCost { get; set; }

        [Display(Name = "Fee + Charge")]
        public decimal? FeePlusCharge { get; set; }

        [Display(Name = "Overall Profit")]
        public decimal? OverallProfit { get; set; }

        [Display(Name = "Profit(%)")]
        public decimal? ProfitPercent { get; set; }

        //public virtual Client Client { get; set; }
        //public virtual ResetNewMonth ResetNewMonth { get; set; }
    }
}
