using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SW_Dev_SHMS.Data;
using SW_Dev_SHMS.Data.Entities;

namespace SW_Dev_SHMS.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == user.Id);
            
            if (student == null)
            {
                return NotFound("Student profile not found");
            }

            var requests = await _context.Requests
                .Where(r => r.Student.Id == student.Id)
                .OrderByDescending(r => r.ApplicationDate)
                .ToListAsync();

            ViewBag.Hostels = await _context.Hostels.ToListAsync();
            ViewBag.CurrentStudent = student;

            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string academicYear, int preferredHostelId, string? remarks)
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == user.Id);
            
            if (student == null)
            {
                TempData["Error"] = "ملف الطالب غير موجود";
                return RedirectToAction("Index");
            }

            var hostel = await _context.Hostels.FindAsync(preferredHostelId);
            if (hostel == null)
            {
                TempData["Error"] = "السكن المحدد غير صالح";
                return RedirectToAction("Index");
            }

            var request = new Request
            {
                AcademicYear = academicYear,
                Remarks = remarks,
                Status = ApplicationStatus.Draft,
                ApplicationDate = DateTime.UtcNow,
                Student = student,
                PreferredHostel = hostel
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم تقديم طلب السكن بنجاح!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                TempData["Error"] = "الطلب غير موجود";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (request.Student.Id != user.Id)
            {
                TempData["Error"] = "غير مصرح لك بهذا الإجراء";
                return RedirectToAction("Index");
            }

            if (request.Status == ApplicationStatus.Draft)
            {
                request.Status = ApplicationStatus.PendingPayment;
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم تقديم الطلب، يرجى المتابعة لدفع الرسوم";
            }

            return RedirectToAction("Index");
        }
    }
}