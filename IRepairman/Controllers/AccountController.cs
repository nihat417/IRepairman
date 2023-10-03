using IRepairman.Application.Interfaces;
using IRepairman.Application.Models;
using IRepairman.Application.ViewModels;
using IRepairman.Domain.Entities;
using IRepairman.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace IRepairman.Controllers
{
    public class AccountController : Controller
	{
		private readonly IUserRepository userRepository;
		private readonly IEmailService emailService;
		private readonly SignInManager<AppUser> signInManager;
        private readonly ISpecializationRepository specializationRepository;

        public AccountController(IUserRepository userRepository, IEmailService emailService, SignInManager<AppUser> signInManager, ISpecializationRepository specializationRepository)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.specializationRepository = specializationRepository;
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
		public async Task<IActionResult> RegisterMaster()
        {
            var specializations = await specializationRepository.GetAllSpecializationsAsync();
            ViewBag.Specials = new SelectList(specializations, "Id", "Name");
            return View();
        }
		
		[HttpGet]
		public IActionResult RegisterUser() => View();

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var viewModel = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            return View(viewModel);
        }

        [HttpPost]
		public async Task<IActionResult> RegisterUser(UserRegisterVM vm)
		{
			if(ModelState.IsValid)
			{
				var existingEmail = await userRepository.GetUserByEmailAsync(vm.Email);
                if (existingEmail != null)
				{
                    ModelState.AddModelError("Email", "Already taken this email");
                    return View(vm);
                }
				string path = (vm.ImageUrl != null) ? await UploadFileHelper.UploadFile(vm.ImageUrl) : "";
				AppUser user = new()
				{
					FullName = vm.FullName,
					UserName = vm.UserName,
					Email = vm.Email,
					Age = vm.Age,
					ImageUrl = path,
					CreatedTime = DateTime.Now,
				};
				var result = await userRepository.CreateUserAsync(user, vm.Password);
                Log.Information($"User {user} is created");
                if (result)
				{
					var token = await userRepository.GenerateToken(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Confirmation Email Link", confirmationLink!);
                    emailService.SendEmail(message);
                    var nwvm = new UserRegisterVM { UserName = user.UserName };
                    return View("RegisterFinish", nwvm);
                }
            }
            Log.Information("Not Valid Register for User");
            return View(vm);
		}

        [HttpPost]
        public async Task<IActionResult> RegisterMaster(MasterRegisterVM vm)
        {
            if(ModelState.IsValid)
            {
                var existingEmail = await userRepository.GetUserByEmailAsync(vm.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "Already taken this email");
                    return View(vm);
                }
                string path = (vm.ImageUrl != null) ? await UploadFileHelper.UploadFile(vm.ImageUrl) : "";
                Master master = new()
                {
                    UserName = vm.UserName,
                    FullName = vm.FullName,
                    Email = vm.Email,
                    ImageUrl = path,
                    Age = vm.Age,
                    CreatedTime = DateTime.Now,
                    WorkExperience = vm.WorkExperience,
                    About = vm.About,
                    SpecializationId=vm.SpecializationId,
                    Role =Domain.Enums.Role.Master
                };
                Log.Information($"Master {master} is created");
                var result = await userRepository.CreateUserAsync(master, vm.Password);
                if (result)
                {
                    var addToRoleResult = await userRepository.AddUserToRoleAsync(master, "Master");
                    if (!addToRoleResult)
                    {
                        ModelState.AddModelError("Role", "Failed to assign the 'Master' role.");
                        return View(vm);
                    }
                    var token = await userRepository.GenerateToken(master);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = master.Email }, Request.Scheme);
                    var message = new Message(new string[] { master.Email }, "Confirmation Email Link", confirmationLink!);
                    emailService.SendEmail(message);
                    var nwvm = new MasterRegisterVM { UserName = master.UserName };
                    return View("RegisterFinish", nwvm);
                }
            }
            Log.Information("Not Valid Register for User");
            var specializations = await specializationRepository.GetAllSpecializationsAsync();
            ViewBag.Specials = new SelectList(specializations, "Id", "Name");
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
								ViewBag.ImageUrl = string.IsNullOrEmpty(user.ImageUrl)
			                    ? "/Images/adminphoto.jpg"
			                    : user.ImageUrl;

								if (await userRepository.IsInRoleAsync(user, "Admin"))
                                {
                                    Log.Information($"{user} is Logged");
                                    return Redirect("/foradmin");
                                }
                                if (await userRepository.IsInRoleAsync(user, "Master"))
                                {
                                    Log.Information($"{user} is Logged");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await userRepository.GetUserByEmailAsync(vm.Email);
                if (user != null)
                {
                    var token = await userRepository.GenerateToken(user);
                    var newlink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email }, "Forgot password link", newlink!);
                    emailService.SendEmail(message);
                    return View("ForgotPasswordConfirmation");
                }
                ModelState.AddModelError(string.Empty, "User not found");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await userRepository.GetUserByEmailAsync(vm.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                    return View(vm);
                }

                var result = await userRepository.ResetPasswordsAsync(user, vm.Token, vm.Password);

                if (result.Succeeded)
                {
                    return View("ResetPasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(vm);
        }


        #region Email Config

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

        #endregion
    }
}
