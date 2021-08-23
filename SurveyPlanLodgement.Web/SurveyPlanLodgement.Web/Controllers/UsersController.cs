using Microsoft.AspNetCore.Mvc;
using SurveyPlanLodgement.Web.Models;
using SurveyPlanLodgement.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRolesRepository _rolesRepository;

        public UsersController(IAccountRepository accountRepository, IRolesRepository rolesRepository)
        {
            _accountRepository = accountRepository;
            _rolesRepository = rolesRepository;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        
        {
            var users = await _accountRepository.GetAllApplicationUsers();
            return View(users);
        }

        [Route("roles")]
        public IActionResult GetAllRoles()
        {
            var roles = _accountRepository.GetAllRoles();
            return View(roles);
        }

        [Route("role-create")]
        public IActionResult AddNewRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRole(RolesModel model)
        {
            if (ModelState.IsValid)
            {
                await _rolesRepository.AddRoleAsync(model.Name);
            }


            return RedirectToAction("GetAllRoles");
        }

        [Route("add-user")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost]
        [Route("add-user")]
        public async Task<IActionResult> CreateUser(SignupUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                //Write your code
                var result = await _accountRepository.CreateUserAsync(userModel);

                if (!result.Succeeded)
                {
                    //Get all Error Messages
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View();
                }

                ModelState.Clear();

                return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
            }
            return View();
        }

        [Route("user-roles")]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await _accountRepository.GetUserById(userId);
            ViewBag.UserName = user.FirstName + " " + user.LastName;

            var userRoles = await _accountRepository.GetUserRolesByIdAsync(userId);

            return View(userRoles);
        }

        [HttpPost]
        [Route("user-roles")]
        public async Task<IActionResult> ManageRoles(List<RolesModel> model, string userId)
        {
            var result = await _accountRepository.ManageUserRolesAsync(model, userId);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");

        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            ConfirmEmailModel model = new ConfirmEmailModel
            {
                Email = email
            };
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.IsConfirmed = true;

                    return View(model);
                }

                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong.");
            }

            return View(model);
        }

        [HttpGet]
        [Route("edit-user")]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _accountRepository.GetUserModelAsync(userId);

            return View(user);
        }

        [HttpPost]
        [Route("edit-user")]
        public async Task<IActionResult> EditUser(UpdateUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                //Write your code
                var result = await _accountRepository.EditUserAsync(userModel);

                if (!result.Succeeded)
                {
                    //Get all Error Messages
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View();
                }

                ModelState.Clear();

                return RedirectToAction("Index");
            }
            return View();
        }


    }
}
