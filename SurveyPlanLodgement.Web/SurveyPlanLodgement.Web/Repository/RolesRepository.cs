using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public class RolesRepository : IRolesRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            //Method to return list of Roles
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task AddRoleAsync(string role)
        {
            //Method to add new Role
            if (!string.IsNullOrEmpty(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role.Trim()));
            }
        }



    }
}
