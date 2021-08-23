using SurveyPlanLodgement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public interface ILodgementRepository
    {
        Task<int> AddLodgementAsync(LodgementModel model);
        Task<List<LodgementModel>> GetAllLodgementsAsync();
        Task<List<LodgementModel>> GetSurveyorLodgementsAsync(string id);
        Task<LodgementModel> GetLodgmentByIdAsync(int id);
    }
}