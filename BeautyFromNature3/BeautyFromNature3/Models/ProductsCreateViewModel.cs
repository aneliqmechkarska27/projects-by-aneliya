using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyFromNature3.Models
{
    public class ProductsCreateViewModel
    {
        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name = "Име на продукта")]
        [MaxLength(64, ErrorMessage = "Полето {0} не може да има повече от {1} символа.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name = "Описание")]
        [MaxLength(250, ErrorMessage = "Полето {0} не може да има повече от {1} символа.")]
        public string Description { get; set; }


        [Display(Name = "Снимка")]

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name = "Компания")]
        public int CompanyId { get; set; }
        public string Name { get; set; }

    }
}
