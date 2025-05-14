using BeautyFromNature3.Domain;

namespace BeautyFromNature3.Models
{
    public class ProductCatalogViewModel
    {
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
