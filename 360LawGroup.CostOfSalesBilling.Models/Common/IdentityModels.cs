using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.    
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool? IsSendAnEmailIntro { get; set; }
        public string Address { get; set; }
        public string AddresslLine1 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? PinCode { get; set; }
        public string ConsultantCompany { get; set; }
        public string EmailAddress_360BusinessLaw { get; set; }
        public string EmailAddress_PrivateClientLaw { get; set; }
        public string EmailAddress_360LawServices { get; set; }
        public string PrivateEmailAddress { get; set; }
        public string AreaofLaw { get; set; }
        public string BusinessPhone { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string JurisdictionsCovered { get; set; }
        public string Notes { get; set; }
        public string Attachments { get; set; }
        public decimal? SubscriptionHourlyRate { get; set; }
        public decimal? MemberHourlyRate { get; set; }
        public decimal? PrivateClientHourlyRate { get; set; }
        public decimal? LitigationHourlyRate { get; set; }
        public decimal? RegulatedHourlyRate { get; set; }
        public decimal? OverseasHourlyRate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string ConsultantUserId { get; set; }
        public Guid? ClientId { get; set; }
        public string UserStatus { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(string conString)
            : base(conString, throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext(IdentityHelper.ProviderConnectionString);
        }
    }

    public static class IdentityHelper
    {
        private static string _providerConString;

        public static string ProviderConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_providerConString))
                {
                    var connString = ConfigurationManager.ConnectionStrings["DataEntities"].ConnectionString;
                    var builder = new EntityConnectionStringBuilder(connString);
                    _providerConString = builder.ProviderConnectionString;
                }
                return _providerConString;
            }
        }

        public static string UserId
        {
            get
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    return System.Web.HttpContext.Current.User.Identity.GetUserId();
                return string.Empty;
            }
        }
    }
}