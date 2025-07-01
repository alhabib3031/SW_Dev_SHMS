using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SW_Dev_SHMS.Data.Entities;
using SW_Dev_SHMS.Services.DataServices.Interfaces;

namespace SW_Dev_SHMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class PaymentController : Controller
    {
        private readonly IStudentDataService _studentDataService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IStudentDataService studentDataService,
            UserManager<ApplicationUser> userManager,
            ILogger<PaymentController> logger)
        {
            _studentDataService = studentDataService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
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
                    return RedirectToAction("Profile", "Student");
                }

                // Check if student already has a completed payment
                var existingPayment = student.Payments?.FirstOrDefault();
                if (existingPayment?.IsCompleted == true)
                {
                    TempData["Info"] = "تم دفع رسوم التسجيل بالفعل";
                    return RedirectToAction("Status");
                }

                ViewBag.AmountDue = 500; // Set the amount due
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading payment page");
                TempData["Error"] = "حدث خطأ أثناء تحميل صفحة الدفع";
                return RedirectToAction("Profile", "Student");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BankTransfer()
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
                    return RedirectToAction("Index");
                }

                // Create payment record for bank transfer
                var payment = new Payment
                {
                    PaymentMethod = PaymentMethod.BankTransfer,
                    IsCompleted = false, // Bank transfers need admin confirmation
                    PaymentDate = DateTime.UtcNow,
                    Student = student,
                    Request = student.Request
                };

                student.Payments ??= new List<Payment>();
                student.Payments.Add(payment);

                await _studentDataService.UpdateAsync(student);

                TempData["Success"] = "تم إرسال طلب الدفع عبر التحويل المصرفي. سيتم مراجعته من قِبل الإدارة خلال 24-48 ساعة";
                _logger.LogInformation($"Bank transfer payment created for student {userId}");

                return RedirectToAction("Status");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating bank transfer payment");
                TempData["Error"] = "حدث خطأ أثناء إنشاء طلب الدفع";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Online(string cardNumber, string expiryDate, string cvv, bool agreeTerms)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                if (!agreeTerms)
                {
                    TempData["Error"] = "يجب الموافقة على الشروط والأحكام للمتابعة";
                    return RedirectToAction("Index");
                }

                // Basic validation
                if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Replace(" ", "").Length < 16)
                {
                    TempData["Error"] = "رقم البطاقة غير صحيح";
                    return RedirectToAction("Index");
                }

                var student = await _studentDataService.GetByIdAsync(userId);
                if (student == null)
                {
                    TempData["Error"] = "لم يتم العثور على بيانات الطالب";
                    return RedirectToAction("Index");
                }

                // Simulate payment processing delay
                await Task.Delay(2000);

                // Create payment record (PayPal represents online payment)
                var payment = new Payment
                {
                    PaymentMethod = PaymentMethod.PayPal,
                    IsCompleted = true, // Online payments are completed immediately
                    PaymentDate = DateTime.UtcNow,
                    Student = student,
                    Request = student.Request
                };

                student.Payments ??= new List<Payment>();
                student.Payments.Add(payment);

                await _studentDataService.UpdateAsync(student);

                TempData["Success"] = "تم الدفع بنجاح! تم تأكيد دفع رسوم التسجيل";
                _logger.LogInformation($"Online payment completed for student {userId}");

                return RedirectToAction("Status", new { paymentId = payment.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing online payment");
                TempData["Error"] = "حدث خطأ أثناء معالجة الدفع الإلكتروني";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cash()
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
                    return RedirectToAction("Index");
                }

                // Create payment record for cash
                var payment = new Payment
                {
                    PaymentMethod = PaymentMethod.BankTransfer, // Using as closest to cash
                    IsCompleted = false, // Cash payments need admin confirmation
                    PaymentDate = DateTime.UtcNow,
                    Student = student,
                    Request = student.Request
                };

                student.Payments ??= new List<Payment>();
                student.Payments.Add(payment);

                await _studentDataService.UpdateAsync(student);

                TempData["Success"] = "تم إرسال إشعار بالدفع النقدي. سيتم التحقق من الدفع وتأكيده من قِبل الإدارة";
                _logger.LogInformation($"Cash payment notification sent for student {userId}");

                return RedirectToAction("Status");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while confirming cash payment");
                TempData["Error"] = "حدث خطأ أثناء إرسال إشعار الدفع النقدي";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Status()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                _logger.LogInformation($"Loading payment status for user: {userId}");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var student = await _studentDataService.GetByIdAsync(userId);
                if (student == null)
                {
                    TempData["Error"] = "لم يتم العثور على بيانات الطالب";
                    return RedirectToAction("Profile", "Student");
                }

                _logger.LogInformation($"Student found. Payments count: {student.Payments?.Count ?? 0}");

                var payments = student.Payments?.OrderByDescending(p => p.PaymentDate).ToList() ?? new List<Payment>();
        
                _logger.LogInformation($"Returning {payments.Count} payments to view");
        
                return View(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading payment status");
                TempData["Error"] = "حدث خطأ أثناء تحميل حالة الدفع";
                return RedirectToAction("Profile", "Student");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Success(int paymentId)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var student = await _studentDataService.GetByIdAsync(userId);
                var payment = student?.Payments?.FirstOrDefault(p => p.Id == paymentId);

                if (payment == null)
                {
                    TempData["Error"] = "لم يتم العثور على معاملة الدفع";
                    return RedirectToAction("Status");
                }

                return View(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading payment success page");
                TempData["Error"] = "حدث خطأ أثناء تحميل صفحة نجاح الدفع";
                return RedirectToAction("Status");
            }
        }
    }
}