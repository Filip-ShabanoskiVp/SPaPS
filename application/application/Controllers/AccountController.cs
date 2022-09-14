using application.Data;
using application.Models;
using application.Models.AccountModels;
using DataAccess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Postal;

namespace application.Controllers
{


    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly SPaPSContext _comtext;
        private readonly IEmailSenderEnhance emailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
            SPaPSContext _context, IEmailSenderEnhance emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._comtext = _context;
            this.emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            var result = await signInManager.PasswordSignInAsync(userName: model.email, password: model.password,
                isPersistent: false, lockoutOnFailure: true);

            if (!result.Succeeded || result.IsLockedOut || result.IsNotAllowed)
            {
                ModelState.AddModelError("Error", "Neuspesen obid za logiranje. Ve molam kontaktirajte administrator!");
                return View(model);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);

            if (userExists != null) { 
                ModelState.AddModelError("Error","Korisnikot veke postoi");
                return View(model);
            }
             
            IdentityUser user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var createUser = await userManager.CreateAsync(user, model.Password);

            if (!createUser.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            user = await userManager.FindByEmailAsync(model.Email);

            Client client = new Client()
            {   
                UserId = user.Id,
                Name = model.Name,
                Address = model.Address,
                IdNo = model.IdNo,
                ClientTypeId = model.ClientTypeId,
                CityId = model.CityId,
                CountryId = model.CountryId
            };

           await _comtext.Clients.AddAsync(client);
           await _comtext.SaveChangesAsync();

          /*  EmailSetUp emailSetUp = new EmailSetUp()
           {
                To = model.Email,
                Username = model.Email,
                Password = model.Password,
                Template = "Register",
                RequestPath = emailService.PostalRequest(Request)
            };
            await emailService.SendEmailAsync(emailSetUp);*/

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}
