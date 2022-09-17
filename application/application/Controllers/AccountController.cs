    using application.Data;
using application.Helpers;
using application.Models;
using application.Models.AccountModels;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace application.Controllers
{


    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly SPaPSContext _context;
        private readonly IEmailSenderEnhance emailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
            SPaPSContext _context, IEmailSenderEnhance emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._context = _context;
            this.emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", Convert.ToString(TempData["Success"]));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await signInManager.PasswordSignInAsync(userName: model.email, password: model.password,
               isPersistent: false, lockoutOnFailure: true);

            if (!result.Succeeded || result.IsLockedOut || result.IsNotAllowed)
            {
                ModelState.AddModelError("Error", "Неуспешен обид за логирање. Ве молам контактирај те администратор!");
                return View(model);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var types = _context.References.Where(x => x.ReferenceTypeId == 1);
            var cities = _context.References.Where(x => x.ReferenceTypeId == 2);
            var countries = _context.References.Where(x => x.ReferenceTypeId == 3);


            ViewData["types"] = new SelectList(types.ToList(), "ReferenceId", "Description");
            ViewData["cities"] = new SelectList(cities.ToList(), "ReferenceId", "Description");
            ViewData["countries"] = new SelectList(countries.ToList(), "ReferenceId", "Description",7);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                var userExists = await userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                ModelState.AddModelError("Error", "Корисникот веќе постои!");
                return View(model);
            }

            IdentityUser user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var newPassword = Shared.GeneratePassword(8);

            var createUser = await userManager.CreateAsync(user, newPassword);

            if (!createUser.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

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

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var callback = Url.Action(action:"ResetPassword",controller:"Account", values: new {token, email = user.Email}, HttpContext.Request.Scheme);

            EmailSetUp emailSetUp = new EmailSetUp()
            {
                To = user.Email,
                Template = "Register",
                Username = user.Email,
                Callback = callback,
                Token = token,
                RequestPath = emailService.PostalRequest(Request)
            }; 

            await emailService.SendEmailAsync(emailSetUp);

            TempData["Success"] = "Успешно креиран корисник";


            return RedirectToAction(nameof(Login));
        }

       
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Error", "Корисникот не постои!");
                return View(model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var callback = Url.Action(action: "ResetPassword", controller: "Account", values: new { token, email = user.Email }, HttpContext.Request.Scheme);

            EmailSetUp emailSetUp = new EmailSetUp()
            {
                To = user.Email,
                Template = "ResetPassword",
                Username = user.Email,
                Callback = callback,
                Token = token,
                RequestPath = emailService.PostalRequest(Request)
            };

            await emailService.SendEmailAsync(emailSetUp);


            TempData["Success"] = "Ве молиме проверете го вашето влезно сандаче!";


            return RedirectToAction(nameof(Login));
         }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            var resetPassword = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            
            if (!resetPassword.Succeeded)
            {
                ModelState.AddModelError("Error", "Се случи грешка. Обидете се повторно!");
                return View();
            }

            TempData["Success"] = "Успешно променета лозинка!";

            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loggedInUserEmail = User.Identity.Name;

            var user = await userManager.FindByEmailAsync(loggedInUserEmail);

            var changePassword = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePassword.Succeeded)
            {
                ModelState.AddModelError("Error", "Се случи грешка. Обидете се повторно!");
                return View();
            }

            ModelState.AddModelError("Success", "Успешно променета лозинка!");

            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeUserInfo()
        {

            var types = _context.References.Where(x => x.ReferenceTypeId == 1);
            var cities = _context.References.Where(x => x.ReferenceTypeId == 2);
            var countries = _context.References.Where(x => x.ReferenceTypeId == 3);


            ViewData["Changertypes"] = new SelectList(types.ToList(), "ReferenceId", "Description");
            ViewData["changedcities"] = new SelectList(cities.ToList(), "ReferenceId", "Description");
            ViewData["Changedcountries"] = new SelectList(countries.ToList(), "ReferenceId", "Description", 7);

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeUserInfo(RegisterModel model)
        {
          
            var loggedInUserEmail = User.Identity.Name;

            var user = await userManager.FindByEmailAsync(loggedInUserEmail);

            user.PhoneNumber = model.PhoneNumber;
            await userManager.UpdateAsync(user);

            Client client =  _context.Clients.Where(x=>x.UserId == user.Id).FirstOrDefault();

            client.Name = model.Name;
            client.Address = model.Address;
            client.IdNo = model.IdNo;
            client.ClientTypeId = model.ClientTypeId;
            client.CityId = model.CityId;
            client.CountryId = model.CountryId;
            client.UpdatedOn = DateTime.Now;

            _context.Update(client);

            await _context.SaveChangesAsync();

            ModelState.AddModelError("Success", "Успешнa измена!");

            return RedirectToAction(nameof(UserInfos));
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserInfos()
        {

            var loggedInUserEmail = User.Identity.Name;

            var user = await userManager.FindByEmailAsync(loggedInUserEmail);

            Client client = _context.Clients.Where(x => x.UserId == user.Id).FirstOrDefault();

            var type = _context.References.Where(x => x.ReferenceId == client.ClientTypeId).FirstOrDefault();
                //.Where(x => x.ReferenceTypeId == 1).ToList();
            var city = _context.References.Where(x => x.ReferenceId == client.CityId).FirstOrDefault();
            var countrie = _context.References.Where(x => x.ReferenceId == client.CountryId).FirstOrDefault();





            ChangeUserInfoModel infoModel = new ChangeUserInfoModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = client.Name,
                Address = client.Address,
                IdNo = client.IdNo,
                ClientTypeId = client.ClientTypeId,
                CityId = client.CityId,
                CountryId = client.CountryId
            };

            ViewData["descType"] = type.Description;
            ViewData["descCity"] = city.Description;
            ViewData["descCountry"] = countrie.Description;
            ViewData["infos"] = infoModel;
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
