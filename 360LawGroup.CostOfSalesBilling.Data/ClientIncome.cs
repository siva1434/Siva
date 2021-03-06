//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _360LawGroup.CostOfSalesBilling.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientIncome
    {
        public System.Guid Id { get; set; }
        public System.Guid ClientId { get; set; }
        public System.Guid ResetNewMonthId { get; set; }
        public long MonNum { get; set; }
        public string Month { get; set; }
        public long Year { get; set; }
        public System.DateTime Date { get; set; }
        public long TotalHours { get; set; }
        public decimal SubscriptionFee { get; set; }
        public decimal SubscriptionCost { get; set; }
        public Nullable<decimal> SubscriptionFee_Cost { get; set; }
        public Nullable<decimal> MemberCost { get; set; }
        public Nullable<decimal> MemberCharge { get; set; }
        public Nullable<decimal> PrivateClientCost { get; set; }
        public Nullable<decimal> PrivateClientCharge { get; set; }
        public Nullable<decimal> LitigationCost { get; set; }
        public Nullable<decimal> LitigationCharge { get; set; }
        public Nullable<decimal> RegulatedCost { get; set; }
        public Nullable<decimal> RegulatedCharge { get; set; }
        public Nullable<decimal> OverseasCost { get; set; }
        public Nullable<decimal> OverseasCharge { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<decimal> TotalCharge { get; set; }
        public Nullable<decimal> SubscriptionProfit { get; set; }
        public Nullable<decimal> SubscriptionProfitPercet { get; set; }
        public Nullable<decimal> MemberProfit { get; set; }
        public Nullable<decimal> MemberProfitPercent { get; set; }
        public Nullable<decimal> PrivateClientProfit { get; set; }
        public Nullable<decimal> PrivateClientProfitPercent { get; set; }
        public Nullable<decimal> LitigationProfit { get; set; }
        public Nullable<decimal> LitigationProfitPercent { get; set; }
        public Nullable<decimal> RegulatedProfit { get; set; }
        public Nullable<decimal> RegulatedProfitPercent { get; set; }
        public Nullable<decimal> OverseasProfit { get; set; }
        public Nullable<decimal> OverseasProfitPercent { get; set; }
        public Nullable<decimal> ChargeMinusCost { get; set; }
        public Nullable<decimal> FeePlusCharge { get; set; }
        public Nullable<decimal> OverallProfit { get; set; }
        public Nullable<decimal> ProfitPercent { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual ResetNewMonth ResetNewMonth { get; set; }
    }
}
