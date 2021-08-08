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


    }
}
