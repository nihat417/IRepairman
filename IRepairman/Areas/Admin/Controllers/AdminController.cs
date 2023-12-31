﻿using IRepairman.Application.Interfaces;
using IRepairman.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace IRepairman.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly IUserRepository userRepository;
        private readonly ISpecializationRepository specializationRepository;
        private readonly SignInManager<AppUser> signInManager;
        public AdminController(IUserRepository userRepository , ISpecializationRepository specializationRepository, SignInManager<AppUser> signInManager)
        {
            this.userRepository = userRepository;
			this.specializationRepository = specializationRepository;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> AdminPage()
		{
			var users = await userRepository.GetAllUsersAsync();
			ViewBag.Users = users;
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Specializations()
		{
			var specializations = await specializationRepository.GetAllSpecializationsAsync();
			ViewBag.Specializations = specializations;
			return View();
		}

        public async Task<IActionResult> DeleteSpecialization(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid specialization ID");
            }
            await specializationRepository.DeleteSpecializationAsync(id);
            Log.Information($"Specialization with {id} deleted");
            return RedirectToAction("Specializations");
        }

        [HttpPost]
        public async Task<IActionResult> Specializations(Specialization specialization)
        {
        	if(ModelState.IsValid)
        	{
        		var res = await specializationRepository.GetSpecializationByNameAsync(specialization.Name);
				if(res != null)
				{
					ModelState.AddModelError("Name", "NAME is already extist");
					return View(res);
				}

                await specializationRepository.CreateSpecializationAsync(specialization);
                Log.Information($"{specialization} is created");
                return RedirectToAction("Specializations");
        	}
        	return BadRequest();
        }

        #region User Control
        public async Task<IActionResult> LockUser(string Id)
        {
            var user = await userRepository.GetUserByIdAsync(Id);
            if (user != null)
            {
                user.LockoutEnabled = false;
                await userRepository.UpdateAsync(user);
            }
            return RedirectToAction("AdminPage");
        }

        public async Task<IActionResult> UnlockUser(string id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                user.LockoutEnabled = true;
                await userRepository.UpdateAsync(user);
            }
            return RedirectToAction("AdminPage");
        }

        #endregion
    }
}
