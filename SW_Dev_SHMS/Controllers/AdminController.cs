using Microsoft.AspNetCore.Mvc;

namespace SW_Dev_SHMS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
