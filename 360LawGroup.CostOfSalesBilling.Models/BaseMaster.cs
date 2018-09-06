using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class BaseViewModel<IDType>
    {
        public IDType Id { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [StringLength(128)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modify On")]
        public DateTime? ModifiedOn { get; set; }

        [Display(Name = "User Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Created by")]
        public virtual string AspNetUserFullName { get; set; }

        [Display(Name = "Modify by")]
        public virtual string AspNetUser1FullName { get; set; }
    }
}
