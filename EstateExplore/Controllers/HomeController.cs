using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EstateExplore.Data;
using EstateExplore.Models;
using System.Diagnostics;

namespace EstateExplore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RealEstateContext _context;

        public HomeController(ILogger<HomeController> logger, RealEstateContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int pg=1)
        {
            if (_context.Property == null)
            {
                return Problem("Entity set 'pro'  is null.");
            }

            var propertys = from m in _context.Property
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                propertys = propertys.Where(s => s.PropertyName!.Contains(searchString));
            }

            const int pageSize = 12;
            if(pg > 1)
            {
                pg = 1;
            }

            int recsCount = propertys.Count();
            var pager = new Pager(recsCount,pg, pageSize);
            int recSkip = (pg-1) * pageSize;
            var data = propertys.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            //return View(await products.ToListAsync());
            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var property = await _context.Property       
                .FirstOrDefaultAsync(m => m.PropertyID == id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}