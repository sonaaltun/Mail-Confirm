using MailRegisteration.Presentation.Models;
using MailRegisteration.Presentation.MailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MailRegisteration.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, IMailService mailService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _mailService = mailService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, "Password+9");
                if (result.Succeeded)
                {
                    var confirmCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = confirmCode }, protocol: Request.Scheme);
                    var mailbody = $"Lütfen hesabýnýzýn onaylanmasý için linke týklayýnýz <br/> <a href='{url}'>Buraya Týkla</a>";
                    await _mailService.SendMailAsync(model.Email, "Üyelik Ýþlemleri", mailbody);
                    return RedirectToAction("Index", "Home");
                }
                Error();
            }
            return View(model);


        }
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return Error();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
