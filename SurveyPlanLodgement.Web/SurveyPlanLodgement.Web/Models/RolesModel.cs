using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Models
{
    public class RolesModel : IdentityRole
    {
        public bool isSelected { get; set; }
    }
}
