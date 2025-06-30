using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SW_Dev_SHMS.Data.Entities;
using SW_Dev_SHMS.Services.DataServices;
using SW_Dev_SHMS.Services.DataServices.Interfaces;

namespace SW_Dev_SHMS.Controllers
{
    [Authorize(Roles = "Student")]
    public partial class StudentController : Controller
    {
        private readonly IStudentDataService _studentDataService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StudentController> _logger;

        public StudentController(
            IStudentDataService studentDataService,
            UserManager<ApplicationUser> userManager,
            ILogger<StudentController> logger)
        {
            _studentDataService = studentDataService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var student = await _studentDataService.GetByIdAsync(userId);
                if (student == null)
                {
                    _logger.LogWarning($"Student not found for user ID: {userId}");
                    TempData["Error"] = "لم يتم العثور على بيانات الطالب";
                    return RedirectToAction("Index", "Home");
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading student profile");
                TempData["Error"] = "حدث خطأ أثناء تحميل الملف الشخصي";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(
            string studentName, 
            string studentEmail, 
            string studentPhone, 
            string studentAddress)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var student = await _studentDataService.GetByIdAsync(userId);
                if (student == null)
                {
                    TempData["Error"] = "لم يتم العثور على بيانات الطالب";
                    return RedirectToAction("Profile");
                }

                // Update student properties
                if (!string.IsNullOrEmpty(studentName))
                {
                    var nameParts = studentName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    student.FirstName = nameParts.FirstOrDefault() ?? "";
                    student.LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";
                }

                if (!string.IsNullOrEmpty(studentEmail) && studentEmail != student.Email)
                {
                    student.Email = studentEmail;
                    student.UserName = studentEmail; // Keep username in sync with email
                }

                student.PhoneNumber = studentPhone;
               // student.Address = studentAddress;

                // Update in database
                await _studentDataService.UpdateAsync(student);

                // Update Identity user info as well
                var identityUser = await _userManager.FindByIdAsync(userId);
                if (identityUser != null && studentEmail != identityUser.Email)
                {
                    identityUser.Email = studentEmail;
                    identityUser.UserName = studentEmail;
                    await _userManager.UpdateAsync(identityUser);
                }

                TempData["Success"] = "تم تحديث الملف الشخصي بنجاح";
                _logger.LogInformation($"Student profile updated successfully for user ID: {userId}");

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating student profile");
                TempData["Error"] = "حدث خطأ أثناء تحديث الملف الشخصي";
                return RedirectToAction("Profile");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            string currentPassword, 
            string newPassword, 
            string confirmNewPassword)
        {
            try
            {
                // Validate passwords match
                if (newPassword != confirmNewPassword)
                {
                    TempData["Error"] = "كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقتين";
                    return RedirectToAction("Profile");
                }

                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["Error"] = "لم يتم العثور على المستخدم";
                    return RedirectToAction("Profile");
                }

                // Change password using Identity
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (result.Succeeded)
                {
                    TempData["Success"] = "تم تغيير كلمة المرور بنجاح";
                    _logger.LogInformation($"Password changed successfully for user ID: {userId}");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["Error"] = $"فشل في تغيير كلمة المرور: {errors}";
                    _logger.LogWarning($"Failed to change password for user ID: {userId}. Errors: {errors}");
                }

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password");
                TempData["Error"] = "حدث خطأ أثناء تغيير كلمة المرور";
                return RedirectToAction("Profile");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> MyRoom()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var student = await _studentDataService.GetByIdAsync(userId);
                if (student == null)
                {
                    TempData["Error"] = "لم يتم العثور على بيانات الطالب";
                    return RedirectToAction("Profile");
                }

                // Check if student has a room assigned
                if (student.Room == null)
                {
                    TempData["Warning"] = "لم يتم تخصيص غرفة لك بعد. يرجى الانتظار أو التواصل مع الإدارة";
                    return View("NoRoom", student);
                }

                return View(student.Room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading student room");
                TempData["Error"] = "حدث خطأ أثناء تحميل بيانات الغرفة";
                return RedirectToAction("Profile");
            }
        }
    }
}