using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyFromNature3.Data;
using BeautyFromNature3.Domain;
using Microsoft.AspNetCore.Hosting;
using BeautyFromNature3.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BeautyFromNature3.Areas.User.Controllers
{
    [Area("User")]
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
       

        public ProductsController(ApplicationDbContext context )
        {
            _context = context;
            
        }


        // GET: User/Products
        public async Task<IActionResult> Index()
        {
            var productsList = await _context.Products.Include(p => p.Company).ToListAsync();
            return View(productsList);
        }

        // GET: User/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
