using SurveyPlanLodgement.Web.Models;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Services
{
    public interface IEmailService
    {
        Task SendEmailConfirmationMail(UserEmailOptions userEmailOptions);
        Task SendForgotPasswordMail(UserEmailOptions userEmailOptions);
        Task SendTestEmail(UserEmailOptions userEmailOptions);
    }
}