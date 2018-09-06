using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class ResetNewMonthViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Reset New Month?")]
        public bool IsResetNewMonth { get; set; }

        [Display(Name = "Last Updates")]
        public DateTime LastUpdated { get; set; }
    }
}
