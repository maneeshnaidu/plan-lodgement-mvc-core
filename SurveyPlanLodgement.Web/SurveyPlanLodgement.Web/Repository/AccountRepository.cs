using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SurveyPlanLodgement.Web.Models;
using SurveyPlanLodgement.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<ApplicationUser>> GetAllApplicationUsers()
        {
            var users = _userManager.Users;

            var userList = new List<ApplicationUser>();

            foreach (var user in users)
            {
                userList.Add(new ApplicationUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = await GetUserRoles(user)

                });
            }

            return userList;
        }

        public List<RolesModel> GetAllRoles()
        {
            var roles = _roleManager.Roles;

            var rolesList = new List<RolesModel>();

            foreach (var role in roles)
            {
                rolesList.Add(new RolesModel
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }

            return rolesList;
        }

        

        public async Task<IdentityResult> CreateUserAsync(SignupUserModel userModel)
        {
            var user = new ApplicationUser()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                DateOfBirth = userModel.DateOfBirth,
                Email = userModel.Email,
                UserName = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                //Add role "Surveyor" to user by default
                await _userManager.AddToRoleAsync(user, Enum.RolesEnum.Surveyor.ToString());
                await GenerateEmailConfirmationTokenAsync(user);
            }

            return result;
        }


        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationMail(user, token);
            }
        }

        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordMail(user, token);
            }
        }

        public async Task<SignInResult> PasswordLoginAsync(LoginUserModel loginUserModel)
        {
            var result = await _signInManager.PasswordSignInAsync(
                loginUserModel.Email,
                loginUserModel.Password,
                loginUserModel.RememberMe,
                false
                );

            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        public async Task<List<RolesModel>> GetUserRolesByIdAsync(string userId)
        {
            var user = await GetUserById(userId);

            var rolesList = new List<RolesModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesVM = new RolesModel
                {
                    Id = role.Id,
                    Name = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesVM.isSelected = true;
                }
                else
                {
                    userRolesVM.isSelected = false;
                }

                    rolesList.Add(userRolesVM);
            }

            return rolesList;

        }

        public async Task<IdentityResult> ManageUserRolesAsync(List<RolesModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            
            result = await _userManager.AddToRolesAsync(user, model.Where(r => r.isSelected).Select(u => u.Name));

            return result;
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        private async Task SendEmailConfirmationMail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                Placeholders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{Username}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token))
                }

            };

            await _emailService.SendEmailConfirmationMail(options);
        }

        private async Task SendForgotPasswordMail(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                Placeholders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{Username}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token))
                }

            };

            await _emailService.SendForgotPasswordMail(options);
        }


    }
}
