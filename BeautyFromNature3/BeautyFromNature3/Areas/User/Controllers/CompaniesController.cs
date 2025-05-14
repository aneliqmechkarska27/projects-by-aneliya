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

namespace BeautyFromNature3.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User/Companies
        public async Task<IActionResult> Index()
        {
              return View(await _context.Companies.ToListAsync());
        }

     
        private bool CompanyExists(int id)
        {
          return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
