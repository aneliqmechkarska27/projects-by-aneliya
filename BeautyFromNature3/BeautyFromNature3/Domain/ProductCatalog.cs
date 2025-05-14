using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyFromNature3.Domain
{
    public class ProductCatalog
    {
        [Key]
        public int ProductCatalogId { get; set; }

        [Display(Name = "Име на каталога")]
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }

        [Display(Name = "Име на продукта")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
