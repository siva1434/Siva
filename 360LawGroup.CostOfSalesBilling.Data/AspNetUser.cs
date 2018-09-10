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
    
    public partial class AspNetUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetUser()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.AspNetUsers1 = new HashSet<AspNetUser>();
            this.AspNetUsers11 = new HashSet<AspNetUser>();
            this.AspNetUsers12 = new HashSet<AspNetUser>();
            this.Clients = new HashSet<Client>();
            this.Clients1 = new HashSet<Client>();
            this.ConsultantHours = new HashSet<ConsultantHour>();
            this.ConsultantHours1 = new HashSet<ConsultantHour>();
            this.ConsultantHours2 = new HashSet<ConsultantHour>();
            this.Matters = new HashSet<Matter>();
            this.Matters1 = new HashSet<Matter>();
            this.AspNetRoles = new HashSet<AspNetRole>();
            this.Matters2 = new HashSet<Matter>();
        }
    
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public Nullable<bool> IsSendAnEmailIntro { get; set; }
        public string Address { get; set; }
        public string AddresslLine1 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> PinCode { get; set; }
        public string FullName { get; set; }
        public string ConsultantCompany { get; set; }
        public string EmailAddress_360BusinessLaw { get; set; }
        public string EmailAddress_PrivateClientLaw { get; set; }
        public string EmailAddress_360LawServices { get; set; }
        public string PrivateEmailAddress { get; set; }
        public string AreaofLaw { get; set; }
        public string BusinessPhone { get; set; }
        public string HomePhone { get; set; }
        public string JurisdictionsCovered { get; set; }
        public string Notes { get; set; }
        public string Attachments { get; set; }
        public Nullable<decimal> SubscriptionHourlyRate { get; set; }
        public Nullable<decimal> MemberHourlyRate { get; set; }
        public Nullable<decimal> PrivateClientHourlyRate { get; set; }
        public Nullable<decimal> LitigationHourlyRate { get; set; }
        public Nullable<decimal> RegulatedHourlyRate { get; set; }
        public Nullable<decimal> OverseasHourlyRate { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string ConsultantName { get; set; }
        public string ConsultantUserId { get; set; }
        public Nullable<System.Guid> ClientId { get; set; }
        public string UserStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual Client Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers1 { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers11 { get; set; }
        public virtual AspNetUser AspNetUser2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers12 { get; set; }
        public virtual AspNetUser AspNetUser3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client> Clients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Client> Clients1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsultantHour> ConsultantHours { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsultantHour> ConsultantHours1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConsultantHour> ConsultantHours2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matter> Matters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matter> Matters1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matter> Matters2 { get; set; }
    }
}
