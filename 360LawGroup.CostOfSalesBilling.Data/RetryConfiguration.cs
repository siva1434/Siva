using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Runtime.Remoting.Messaging;

namespace _360LawGroup.CostOfSalesBilling.Data
{
    public class RetryConfiguration : DbConfiguration
    {
        public RetryConfiguration()
        {
            SetExecutionStrategy(
                "System.Data.SqlClient",
                () => new SqlAzureExecutionStrategy(5, TimeSpan.FromSeconds(30)));
        }
    }
}
