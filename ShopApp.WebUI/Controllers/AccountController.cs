using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signınManager;
        private IEmailSender _emailSender;

        public AccountController(SignInManager<ApplicationUser> signınManager, UserManager<ApplicationUser> applicationUser, IEmailSender emailSender)
        {
            _signınManager = signınManager;
            _userManager = applicationUser;
            _emailSender = emailSender;
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                //send email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayın.", $"Lütfen mail hesabınızı onaylamak için linke <a href='https://localhost:44385{callbackUrl}'>tıklayınız.</a> ");

                TempData.Put("message", new ResultMessage()
                {
                    Title="Hesap Onayı",
                    Message ="Eposta adresinize gelen link ile hesabınızı onaylayınız.",
                    Css="warning"
                });
                
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu. Lütfen tekrar deneyiniz");
            return View(model);
        }


        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamış");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı onaylayınız");
                return View(model);

            }
            var result = await _signınManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }
            ModelState.AddModelError("", "Email ya da parola yanlış");


            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signınManager.SignOutAsync();
            TempData.Put("message", new ResultMessage()
            {
                Title = "Oturum kapatıldı.",
                Message = "Hesabınız güvenli bir şekilde sonlandırıldı.",
                Css = "warning"
            });
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new ResultMessage()
                {
                    Title = "Hesap Onayı",
                    Message = "Hesap onayı için bilgileriniz yanlış.",
                    Css = "danger"
                });

                return Redirect("~/");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {

                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız başarıyla onaylanmıştır.",
                        Css = "success"
                    });


                    return RedirectToAction("Login");
                }

            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanamadı.",
                Css = "danger"
            });

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {

                TempData.Put("message", new ResultMessage()
                {
                    Title = "Forgot Password",
                    Message = "Bilgileriniz hatalı.",
                    Css = "danger"
                });

                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = code, email = email },
                    Request.Scheme
                );

                await _emailSender.SendEmailAsync(
                    email,
                    "Reset password",
                    $"Parolanızı yenilemek için <a href='{callbackUrl}'>tıklayınız</a>."
                );
            }

            TempData.Put("message", new ResultMessage()
            {
                Title = "Forgot Password",
                Message = "Eğer email sistemimizde kayıtlıysa, parola yenileme linki gönderilmiştir.",
                Css = "success"
            });

            return View();
        }


        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Home", "Index");
            }
            var model = new ResetPasswordModel { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Home", "Index");
            }

            var results = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (results.Succeeded)
            {
                return RedirectToAction("Login", "Account");

            }
            return View(model);
        }

        public IActionResult Accessdenied()
        {
            return View();
        }
    }
}