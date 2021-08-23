using SurveyPlanLodgement.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public interface IStatusRepository
    {
        Task<int> AddNewStatus(StatusModel model);
        Task<int> DeleteStatus(int id);
        Task<int> EditStatus(StatusModel model);
        Task<List<StatusModel>> GetAllStatusesAsync();
        Task<StatusModel> GetStatusById(int id);
        Task<string> GetStatusNameById(int id);
    }
}