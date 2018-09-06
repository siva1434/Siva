using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class WorkRateViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Rate ID")]
        public int RateId { get; set; }

        [Display(Name = "Rate Type")]
        public string RateType { get; set; }
    }
}
