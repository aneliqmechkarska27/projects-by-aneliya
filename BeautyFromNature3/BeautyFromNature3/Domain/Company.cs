using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyFromNature3.Domain
{
    public class Company
    {
        public Company()
        {
            Products = new List<Product>();
        }
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name ="Име на компанията")]
        [MaxLength(64, ErrorMessage = "Полето {0} не може да има повече от {1} символа.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name = "Описание")]
        [MaxLength(250, ErrorMessage = "Полето {0} не може да има повече от {1} символа.")]
        public string  Description{ get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name = "Местоположение в България")]
        [MaxLength(50, ErrorMessage = "Полето {0} не може да има повече от {1} символа.")]
        public string Location { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}
