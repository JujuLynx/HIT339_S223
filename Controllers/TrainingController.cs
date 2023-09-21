using Microsoft.AspNetCore.Mvc;

namespace e_corp.Controllers
{
    public class TrainingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
