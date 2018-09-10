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
    
    public partial class ConsultantHour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsultantHour()
        {
            this.ConsultantCosts = new HashSet<ConsultantCost>();
        }
    
        public System.Guid Id { get; set; }
        public string ConsultantId { get; set; }
        public System.Guid MatterId { get; set; }
        public System.Guid ClientId { get; set; }
        public Nullable<System.DateTime> WorkDate { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<decimal> WorkHours { get; set; }
        public Nullable<bool> CurrentMonth { get; set; }
        public Nullable<bool> CostUpdated { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> WorkRateId { get; set; }
        public Nullable<bool> Agreed { get; set; }
        public Nullable<System.Guid> ResetNewMonthId { get; set; }
        public string DisbursementDescription { get; set; }
        public Nullable<decimal> DisbursementAmount { get; set; }
        public Nullable<decimal> Hours { get; set; }
        public Nullable<decimal> Minutes { get; set; }
        public Nullable<decimal> SubscriptionHours { get; set; }
        public Nullable<decimal> MemberHours { get; set; }
        public Nullable<decimal> PrivateClientHours { get; set; }
        public Nullable<decimal> LitigationHours { get; set; }
        public Nullable<decimal> RegulatedHours { get; set; }
        public Nullable<decimal> OverseasHours { get; set; }
        public Nullable<System.DateTime> LastDate { get; set; }
        public Nullable<int> MonNum { get; set; }
        public string MonthName { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<decimal> TotalHours_excl_Sub { get; set; }
        public string Month { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        public virtual AspNetUser AspNetUser2 { get; set; }
        public virtual Client Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsultantCost> ConsultantCosts { get; set; }
        public virtual Matter Matter { get; set; }
        public virtual ResetNewMonth ResetNewMonth { get; set; }
    }
}
