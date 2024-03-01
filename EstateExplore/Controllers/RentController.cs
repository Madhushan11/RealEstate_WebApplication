using Microsoft.AspNetCore.Mvc;

namespace EstateExplore.Controllers
{
    public class RentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
