using SurveyPlanLodgement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public interface ILodgementRepository
    {
        Task<int> AddLodgementAsync(LodgementModel model);
        Task<List<LodgementModel>> GetLodgementsAsync();
    }
}