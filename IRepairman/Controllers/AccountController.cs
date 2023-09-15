﻿using IRepairman.Application.Interfaces;
using IRepairman.Application.Models;
using IRepairman.Application.ViewModels;
using IRepairman.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Controllers
{
    public class AccountController : Controller
	{
		private readonly IUserRepository userRepository;
		private readonly IEmailService emailService;
		private readonly SignInManager<AppUser> signInManager;

        public AccountController(IUserRepository userRepository, IEmailService emailService, SignInManager<AppUser> signInManager)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.signInManager = signInManager;
        }

        [HttpGet]
		public IActionResult Login(string ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			return View();
		}

		[HttpGet]
		public IActionResult ChooseRole() => View();

		[HttpGet]
		public IActionResult RegisterMaster() => View();
		
		[HttpGet]
		public IActionResult RegisterUser() => View();

		[HttpPost]
		public async Task<IActionResult> RegisterUser(UserRegisterVM vm)
		{
			if(ModelState.IsValid)
			{
				var existingEmail = await userRepository.GetUserByEmailAsync(vm.Email);
				if(existingEmail != null)
				{
                    ModelState.AddModelError("Email", "Already taken this email");
                    return View(vm);
                }
                AppUser user = new()
				{
					FullName = vm.FullName,
					UserName = vm.UserName,
					Email = vm.Email,
					Age = vm.Age,
					CreatedTime = DateTime.Now,
				};
				var result = await userRepository.CreateUserAsync(user, vm.Password);
                if(result)
				{
					var token = await userRepository.GenerateToken(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Confirmation Email Link", confirmationLink!);
                    emailService.SendEmail(message);
                    var nwvm = new UserRegisterVM { UserName = user.UserName };//bax mastervm
                    return View("RegisterFinish", nwvm);
                }
			}
			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel vm, string? ReturnUrl)
		{
			if(ModelState.IsValid)
			{
				var user = await userRepository.GetUserByEmailAsync(vm.Email);
				if(user != null)
				{
                    if (await userRepository.IsEmailConfirmedAsync(user))
					{
						if(user.LockoutEnabled == true)
						{
                            var result = await signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
							if(result.Succeeded)
							{
                                if (await userRepository.IsInRoleAsync(user, "Admin"))
                                {
                                    return Redirect("/foradmin");
                                }
                                if (await userRepository.IsInRoleAsync(user, "Master"))
                                {
                                    return Redirect("/formaster");
                                }
                                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                                    return Redirect(ReturnUrl);
                                return Redirect("/");
                            }
                            else if (result.IsLockedOut)
                                ModelState.AddModelError("All", "Lockout");
                        }
                        else { return View("BlockPage"); }
                    }
                    else
                        ModelState.AddModelError("All", "Mail has not confired yet!!");
                }
                else
                    ModelState.AddModelError("login", "Incorrect username or password");
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "" });
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userRepository.GetUserByEmailAsync(email);
            if (user != null)
            {
                var result = await userRepository.ConfirmEmailAsync(user, token);
                if (result)
                {
                    var vm = new UserRegisterVM { UserName = user.UserName };//bax sonra mastervm
                    return View("SuccessPage", vm);
                }
            }
            return View();
        }
    }
}
