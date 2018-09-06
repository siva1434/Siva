using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class ClientFileViewModel
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "File")]
        public string Files { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }
    }
}
