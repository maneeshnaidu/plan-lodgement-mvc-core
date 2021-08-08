namespace SurveyPlanLodgement.Web.Services
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}