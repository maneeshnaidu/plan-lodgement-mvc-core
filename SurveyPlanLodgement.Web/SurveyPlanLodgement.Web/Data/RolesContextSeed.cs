using Microsoft.AspNetCore.Identity;
using SurveyPlanLodgement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Data
{
    public static class RolesContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enum.RolesEnum.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.RolesEnum.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.RolesEnum.Surveyor.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@pau.lands.fj",
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Enum.RolesEnum.Surveyor.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enum.RolesEnum.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enum.RolesEnum.SuperAdmin.ToString());
                }

            }
        }
    }
}
