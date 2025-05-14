using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautyFromNature3.Data;
using BeautyFromNature3.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace BeautyFromNature3.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class ProductCatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BeautyUser> _userManager;

        public ProductCatalogsController(ApplicationDbContext context, UserManager<BeautyUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: User/ProductCatalogs
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);//Get Id na lognatia user

            if (userId == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Unable to load user with ID
            }

            //вземам Id на каталозите на логнатия user и ще повуча List<int>
            List<int> listOfcatalogsId = await GetUsersCatalogId(userId);


            //извличам данните от таблицата ProductCatalogs и получавам  List<ProductCatalog>
            List<ProductCatalog> productCatalogs = await GetListOfProductCatalogs();

            //взема само записите на логнатия user от таблицата ProductCatalogs
            List<ProductCatalog> newProductCatalogs = CreateListOfProductCatalogByUser(listOfcatalogsId, productCatalogs);

            //ще се показва само списъка от каталозите и продуктите в тях на логнатия user
            return View(newProductCatalogs);
        }


        // GET: User/ProductCatalogs/Create
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);//Get Id na lognatia user

            if (userId == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Unable to load user with ID
            }

            var catalogsId = GetUsersCatalogs(userId);
            ViewData["CatalogId"]=new SelectList(catalogsId,"CatalogId", "CatalogName");

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Title");

            return View();
        }

        // POST: User/ProductCatalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCatalogId,CatalogId,ProductId")] ProductCatalog productCatalog)
        {
            var userId = _userManager.GetUserId(User);//Get Id na lognatia user

            if (userId == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Unable to load user with ID
            }

            if (ModelState.IsValid)
            {
                _context.Add(productCatalog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var catalogs = GetUsersCatalogs(userId);
            ViewData["CatalogId"]=new SelectList(catalogs,"CatalogId", "CatalogName", productCatalog.CatalogId);

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Title", productCatalog.ProductId);

            return View(productCatalog);
        }

       

        // GET: User/ProductCatalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductCatalogs == null)
            {
                return NotFound();
            }

            var productCatalog = await _context.ProductCatalogs
                .Include(p => p.Catalog)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductCatalogId == id);
            if (productCatalog == null)
            {
                return NotFound();
            }

            return View(productCatalog);
        }

        // POST: User/ProductCatalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductCatalogs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductCatalogs'  is null.");
            }
            var productCatalog = await _context.ProductCatalogs.FindAsync(id);
            if (productCatalog != null)
            {
                _context.ProductCatalogs.Remove(productCatalog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCatalogExists(int id)
        {
            return _context.ProductCatalogs.Any(e => e.ProductCatalogId == id);
        }


        ///New Metod
        /// <summary>
        /// Creates a list of the ProductCatalogs table records that apply to lofin user
        /// </summary>
        /// <param name="listOfcatalogsId"></param>
        /// <param name="productCatalogs"></param>
        /// <returns> List<ProductCatalog> </returns>

        private static List<ProductCatalog> CreateListOfProductCatalogByUser(List<int> listOfcatalogsId, List<ProductCatalog> productCatalogs)
        {
            List<ProductCatalog> newProductCatalogs = new List<ProductCatalog>();

            foreach (var item in productCatalogs)
            {
                var catalogId = item.CatalogId;
                if (listOfcatalogsId.Contains(catalogId))
                {
                    newProductCatalogs.Add(item);
                }
            }

            return newProductCatalogs;
        }

        ///New Metod
        /// <summary>
        /// Select all records from the ProductCatalogs table
        /// </summary>
        /// <returns></returns>
        private async Task<List<ProductCatalog>> GetListOfProductCatalogs()
        {
            return await _context.ProductCatalogs
                .Include(p => p.Catalog)
                .Include(p => p.Product)
                .ToListAsync();
        }

        ///New Metod
        /// <summary>
        /// Select all records from the Catalogs table and takes the CatalogId that apply to lofin user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<int>> GetUsersCatalogId(string userId)
        {
            return await _context.Catalogs
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .Select(c => c.CatalogId)
                .ToListAsync();
        }


        ///New Metod
        /// <summary>
        /// Select the records from the Catalogs table that apply to lofin user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<Catalog> GetUsersCatalogs(string userId)
        {
            return _context.Catalogs
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .Select(c => c)
                .ToList();
        }
    }
}
