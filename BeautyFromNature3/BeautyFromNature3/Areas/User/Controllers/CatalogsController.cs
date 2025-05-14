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
    public class CatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BeautyUser> _userManager;

        public CatalogsController(ApplicationDbContext context, UserManager<BeautyUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: User/Catalogs
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);//взима id на логнатия потребител

            if (userId == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Не може да се зареди потребител с ID
            }

            var catalogs = _context.Catalogs //каталозите на логнатия потребител
                .Where(u => u.UserId == userId)
                .Select(c => c)
                .OrderByDescending(c => c.EditDate);


            return View(await catalogs.ToListAsync());

        }

        // GET: User/Catalogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userManager.GetUserId(User);

            if (user == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Unable to load user with ID
            }

            var catalog = await _context.Catalogs
                .Include(c => c.User)
                .Include(c => c.Products)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(c => c.CatalogId == id);

            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);

        }

        // GET: User/Catalogs/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: User/Catalogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Catalog catalog)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Не може да се зареди потребител с ID
            }


            ////EditDate is never null since is from type DateTime
            if (ModelState.IsValid)
            {
                catalog.UserId = userId;

                _context.Add(catalog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));



            }

            return View(catalog);
        }

        // GET: User/Catalogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Unable to load user with ID
            }

            var catalog = await _context.Catalogs
                .Where(c => c.UserId == userId)
                .FirstOrDefaultAsync(c => c.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }
            return View(catalog);
        }

        // POST: User/Catalogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Catalog catalog)
        {
            if (id != catalog.CatalogId)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'."); //Unable to load user with ID
            }

            if (ModelState.IsValid)
            {
                try
                {


                    catalog.UserId = userId;
                    catalog.EditDate = DateTime.Now;

                    _context.Update(catalog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogExists(catalog.CatalogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(catalog);
        }

        // GET: User/Catalogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Catalogs == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CatalogId == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // POST: User/Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Catalogs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Catalogs'  is null.");
            }
            var catalog = await _context.Catalogs.FindAsync(id);
            if (catalog != null)
            {
                _context.Catalogs.Remove(catalog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogExists(int id)
        {
            return _context.Catalogs.Any(e => e.CatalogId == id);
        }
    }
}
