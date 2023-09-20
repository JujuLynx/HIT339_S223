using Microsoft.AspNetCore.Mvc;

namespace e_corp.Controllers
{
    public class Training : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
