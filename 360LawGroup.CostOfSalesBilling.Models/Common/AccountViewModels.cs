using System.Collections.Generic;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }

        public string ResponseType { get; set; }

        public string ClientId { get; set; }

        public string Provider { get; set; }
    }

    public class DashboardCounters
    {
        public int TotalRegistrations { get; set; }
        public int TotalTodaysRegistrations { get; set; }
        public int TotalEmailConfirmations { get; set; }
        public int TotalTodaysEmailConfirmations { get; set; }
    }
}
