using Microsoft.AspNetCore.Mvc;

namespace SW_Dev_SHMS.Controllers;

public class NotificationController : Controller
{
    public IActionResult Index()
    {
        var userRole = User.IsInRole("Admin") ? "Admin" : "Student";

        // اجلب الإشعارات بناءً على الدور
        var notifications = GetNotificationsForRole(userRole);

        return View("~/Views/Shared/Notifications.cshtml", notifications);
    }

    private List<string> GetNotificationsForRole(string role)
    {
        // هذا مثال - استخدم قاعدة البيانات بدلاً منه
        if (role == "Admin")
            return new List<string> { "تنبيه للنظام", "تقرير جديد" };
        else
            return new List<string> { "تمت الموافقة على طلبك", "تحديث في بيانات السكن" };
    }
}
