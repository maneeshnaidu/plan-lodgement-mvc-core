using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public interface IRolesRepository
    {
        Task AddRoleAsync(string role);
        Task<List<IdentityRole>> GetAllRolesAsync();
    }
}