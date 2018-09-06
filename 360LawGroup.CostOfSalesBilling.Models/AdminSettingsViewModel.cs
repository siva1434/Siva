using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class AdminSettingsViewModel
    {
        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Name")]
        public string ParamName { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Title")]
        public string ParamTitle { get; set; }

        [Required(ErrorMessage = Common.RequiredMsg)]
        [Display(Name = "Value")]
        public string ParamValue { get; set; }

        public string ParamType { get; set; }
    }
}
