using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyFromNature3.Domain
{
    public class Catalog
    {
        public Catalog()
        {
            Products = new List<ProductCatalog>();
        }
        [Key]
        public int CatalogId { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [MaxLength(64, ErrorMessage = "Полето {0} не може да има повече от {1} символа.")]
        [Display(Name = "Име на каталога")]
        public string CatalogName { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [MaxLength(250, ErrorMessage = "Полето {0} не може да има повече от {1} символа.")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name = "Създаден преди")]
        public DateTime EditDate { get; set; }


        public string UserId { get; set; }
        [Display(Name = "Потребител")]
        public BeautyUser User { get; set; }
        public virtual ICollection<ProductCatalog> Products { get; set; }

    }
}
