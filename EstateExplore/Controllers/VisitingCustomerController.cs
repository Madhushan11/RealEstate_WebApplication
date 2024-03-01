using EstateExplore.Data;
using EstateExplore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EstateExplore.Controllers
{
    public class VisitingCustomerController : Controller
    {
        private readonly RealEstateContext _context;

        public VisitingCustomerController(RealEstateContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int propertyId)
        {
            // Get the list of visiting customers for a specific property
            var visitingCustomers = await _context.VisitingCustomers
                .Where(c => c.PropertyId == propertyId)
                .ToListAsync();

            return View(visitingCustomers);
        }

        [HttpPost]
        public async Task<IActionResult> RecordVisit(VisitingCustomer visitingCustomer)
        {
            if (ModelState.IsValid)
            {
                visitingCustomer.PropertyId = GetPropertyIdFromContext();

                // Save the visiting customer record to the database
                _context.VisitingCustomers.Add(visitingCustomer);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Property", new { id = visitingCustomer.PropertyId });
            }

            return View(visitingCustomer);
        }

        private int GetPropertyIdFromContext()
        {
            if (RouteData.Values.TryGetValue("propertyId", out var propertyId))
            {
                if (int.TryParse(propertyId.ToString(), out var parsedPropertyId))
                {
                    return parsedPropertyId;
                }
            }

            // Handle the case where PropertyId is not found or not a valid integer.
            // You might want to throw an exception, return a default value, or handle it based on your specific requirements.
            return 0; // Default value if PropertyId is not available.
        }
    }
}
