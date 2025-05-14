using BeautyFromNature3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BeautyFromNature3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class BeautyUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BeautyUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/BeautyUsers
        public async Task<IActionResult> Index(string searchText, int order)
        {
            var users = _context.Users.Select(b => b);

            if (!string.IsNullOrEmpty(searchText))
            {
                users = users
                    .Where(
                    u => u.UserName.Contains(searchText) ||
                    u.FirstName.Contains(searchText) ||
                    u.LastName.Contains(searchText) ||
                    u.PhoneNumber.Contains(searchText) ||
                    u.Email.Contains(searchText));
            }

            if (order == 0)
            {
                users = users
                    .OrderByDescending(u => u.FirstName)
                    .ThenByDescending(u => u.LastName)
                    .ThenByDescending(u => u.UserName);
            }
            else if (order == 1)
            {
                users = users.OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .ThenBy(u => u.UserName);
            }
            return View(await users.ToListAsync());
        }

        // GET: User/BeautyUsers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/BeautyUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

