using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EstateExplore.Data;
using EstateExplore.Models;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace EstateExplore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertysController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly RealEstateContext _context;

        public PropertysController(RealEstateContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString, int pg=1)
        {
            if (_context.Property == null)
            {
                return Problem("Entity set 'product'  is null.");
            }

            var propertys = from m in _context.Property
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                propertys = propertys.Where(s => s.PropertyName!.Contains(searchString));
            }

            const int pageSize = 12;
            if (pg > 1)
            {
                pg = 1;
            }

            int recsCount = propertys.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = propertys.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            //return View(await products.ToListAsync());
            return View(data);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var product = await _context.Property
                .FirstOrDefaultAsync(m => m.PropertyID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        private string? UploadedFile(Property product)
        {
            string? uniqueFileName = null;

            if (product.PropertyImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() +"_" + product.PropertyImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.PropertyImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropertyID,PropertyName,Owner,Description,Price,PopertyLocation,PropertyImage")] Property product)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = UploadedFile(product);
                product.ImageUrl = uniqueFileName;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var product = await _context.Property.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.ImageUrl = Url.Content("../images/" + product.ImageUrl);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PropertyID,PropertyName,Owner,Description,Price,PopertyLocation,PropertyImage")] Property product)
        {
            if (id != product.PropertyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string? uniqueFileName = UploadedFile(product);
                    product.ImageUrl = uniqueFileName;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.PropertyID))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var product = await _context.Property
                .FirstOrDefaultAsync(m => m.PropertyID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Property == null)
            {
                return Problem("Entity set 'RentalSystemContext.Product'  is null.");
            }
            var product = await _context.Property.FindAsync(id);
            if (product != null)
            {
                _context.Property.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Property?.Any(e => e.PropertyID == id)).GetValueOrDefault();
        }
    }
}
