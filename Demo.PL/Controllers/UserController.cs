using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }




        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                var users = await _userManager.Users.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    FName = u.FName,
                    Email = u.Email,
                    LName = u.LName,
                    Roles = _userManager.GetRolesAsync(u).Result
                }).ToListAsync();
                return View(users);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var mappedUser = new UserViewModel()
                {
                    Id = user.Id,
                    FName = user.FName,
                    Email = user.Email,
                    LName = user.LName,
                    Roles = _userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserViewModel> { mappedUser });
            }
            return View(Enumerable.Empty<UserViewModel>());
        }


        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(viewName, mappedUser);
        }



        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest();
            if (!ModelState.IsValid) return View(updatedUser);
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                user.FName = updatedUser.FName;
                user.LName = updatedUser.LName;

                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(updatedUser);
        }



        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete( string id)
        {
            try
            {
                var user =await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
