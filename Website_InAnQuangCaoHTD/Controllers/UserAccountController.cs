using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website_InAnQuangCaoHTD.Data;
using Website_InAnQuangCaoHTD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Security.Cryptography;
using Website_InAnQuangCaoHTD.Helpers;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Website_InAnQuangCaoHTD.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ReCaptchaSettings _reCaptchaSettings;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private const string ResetCodeSessionKey = "ResetCode";
        private const string ResetEmailSessionKey = "ResetEmail";
     
        public UserAccountController(ApplicationDbContext context, IOptions<ReCaptchaSettings> reCaptchaSettings, HttpClient httpClient, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _reCaptchaSettings = reCaptchaSettings.Value;
            _httpClient = httpClient;
            _configuration = configuration;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                // Kiểm tra xem email đã tồn tại chưa
                var existingUser = await _context.Users
                    .SingleOrDefaultAsync(u => u.UserEmail == model.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email đã tồn tại.");
                    return View(model);
                }

                var hashedPassword = HashPassword(model.Password);

                var user = new UsersModel
                {
                    UserName = model.Username,
                    UserEmail = model.Email,
                    Password = hashedPassword,
                    RoleId = 2 // Gán vai trò mặc định là 2 (Khách hàng)
                };

                // Thêm người dùng vào cơ sở dữ liệu
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Đăng nhập người dùng ngay lập tức sau khi đăng ký (nếu cần)
                var role = await _context.Roles.FindAsync(2); // Lấy vai trò có RoleId là 2
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.UserEmail),
                        new Claim(ClaimTypes.Role, role.RoleName)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, "UserCookie");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("UserCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Login", "UserAccount");
            }

            return View(model);
        }
        // Xác nhận Email trước khi đăng ký thành công
       /* private async Task SendEmailConfirmationAsync(string email, string confirmationCode)
        {
            var subject = "Mã xác nhận đăng ký tài khoản";
            var message = $"Mã xác nhận của bạn là: {confirmationCode}";

            // Gọi hàm gửi email ở đây
            await _emailService.SendEmailAsync(email, subject, message);
        }
        private string GenerateConfirmationCode()
        {
            // Tạo mã xác nhận ngẫu nhiên (có thể là một chuỗi ngẫu nhiên)
            return new Random().Next(100000, 999999).ToString(); // Mã xác nhận 6 chữ số
        }
        public IActionResult ConfirmEmail(string email)
        {
            var model = new EmailConfirmationViewModel
            {
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationViewModel model)
        {
            var sessionEmail = HttpContext.Session.GetString("ConfirmationEmail");
            var sessionCode = HttpContext.Session.GetString("ConfirmationCode");

            if (sessionEmail == model.Email && sessionCode == model.ConfirmationCode)
            {
                // Nếu mã xác nhận hợp lệ, tiến hành tạo tài khoản
                var hashedPassword = HashPassword(model.Password); // Bạn cần chắc chắn rằng mật khẩu đã có trong view model

                var user = new UsersModel
                {
                    UserName = model.Username,
                    UserEmail = model.Email,
                    Password = hashedPassword,
                    RoleId = 2 // Gán vai trò mặc định là 2 (Khách hàng)
                };

                // Thêm người dùng vào cơ sở dữ liệu
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Đăng nhập người dùng ngay lập tức sau khi đăng ký (nếu cần)
                var role = await _context.Roles.FindAsync(2); // Lấy vai trò có RoleId là 2
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.UserEmail),
            new Claim(ClaimTypes.Role, role.RoleName)
        };

                var claimsIdentity = new ClaimsIdentity(claims, "UserCookie");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("UserCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

                // Xóa thông tin mã xác nhận và email từ session
                HttpContext.Session.Remove("ConfirmationCode");
                HttpContext.Session.Remove("ConfirmationEmail");

                return RedirectToAction("Login", "UserAccount");
            }

            ModelState.AddModelError(string.Empty, "Mã xác nhận không hợp lệ.");
            return View(model);
        }
*/
        public IActionResult Login()
        {
            var model = new LoginViewModel
            {
                SiteKey = _configuration["ReCaptcha:SiteKey"]
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            var isRecaptchaValid = await VerifyRecaptcha(recaptchaResponse);
            if (ModelState.IsValid)
            {
                if (!isRecaptchaValid)
                {
                    ModelState.AddModelError(string.Empty, "Vui lòng xác thực Recaptcha.");
                    return View(model);
                }

                var user = _context.Users
                                    .Include(u => u.Role)
                                    .SingleOrDefault(u => u.UserEmail == model.Email && u.Role.RoleName == "Khách hàng");

                if (user != null && VerifyPassword(model.Password, user.Password))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

                    HttpContext.Session.SetString("Username", user.UserName);
                    HttpContext.Session.SetString("Usergmail", user.UserEmail);
                    HttpContext.Session.SetInt32("UserId", user.UserId);

                    var claimsIdentity = new ClaimsIdentity(claims, "UserCookie");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync("UserCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập thất bại");
                }
            }

            return View(model);
        }


        private async Task<bool> VerifyRecaptcha(string recaptchaResponse)
        {
            var requestUrl = $"https://www.google.com/recaptcha/api/siteverify";
            var requestData = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("secret", _configuration["ReCaptcha:SecretKey"]),
        new KeyValuePair<string, string>("response", recaptchaResponse)
    });

            var response = await _httpClient.PostAsync(requestUrl, requestData);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.Linq.JObject.Parse(jsonResponse);
            return (bool)result["success"];
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserCookie");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
        [HttpGet]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SendResetCode(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == model.Email);
                if (user != null)
                {   // Tạo mã xác nhận
                    var resetCode = new Random().Next(100000, 999999).ToString();

                    // Gửi mã xác nhận qua email
                    var emailSent = await _emailService.SendEmailAsync(model.Email, "Mã xác nhận quên mật khẩu", $"Mã xác nhận của bạn là: {resetCode}");

                    if (emailSent)
                    {
                        TempData["Message"] = "Mã xác nhận đã được gửi đến email của bạn.";
                        HttpContext.Session.SetString(ResetCodeSessionKey, resetCode);
                        HttpContext.Session.SetString(ResetEmailSessionKey, model.Email);
                        return RedirectToAction("VerifyResetCode");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Gửi email thất bại. Vui lòng thử lại.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email của khách hàng không tồn tại trong hệ thống !");
                }
               
            }

            return View("ForgotPassword", model);
        }
        [HttpGet]
        public IActionResult VerifyResetCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyResetCode(string ResetCode)
        {
            var storedResetCode = HttpContext.Session.GetString(ResetCodeSessionKey);
            var storedEmail = HttpContext.Session.GetString(ResetEmailSessionKey);

            if (storedResetCode != null && storedEmail != null)
            {
                if (ResetCode == storedResetCode)
                {
                    // Mã xác nhận hợp lệ, hiển thị form thay đổi mật khẩu
                    return RedirectToAction("ResetPassword");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Mã xác nhận không hợp lệ.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không có yêu cầu đặt lại mật khẩu nào được lưu trữ.");
            }

            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = HttpContext.Session.GetString(ResetEmailSessionKey);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);

                if (user != null)
                {
                    // Đặt lại mật khẩu
                    user.Password = HashPassword(model.NewPassword);

                    // Cập nhật thông tin người dùng trong cơ sở dữ liệu
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    // Xóa thông tin mã xác nhận và email từ session
                    HttpContext.Session.Remove(ResetCodeSessionKey);
                    HttpContext.Session.Remove(ResetEmailSessionKey);

                    ViewBag.SuccessMessage = "Mật khẩu đã được đặt lại thành công.";
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                ModelState.AddModelError(string.Empty, "Không tìm thấy người dùng với địa chỉ email này.");
            }

            return View(model);
        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


    }

}
