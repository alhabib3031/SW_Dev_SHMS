using Microsoft.AspNetCore.Mvc;

namespace SW_Dev_SHMS.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
