using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Data
{
    public class Journal
    {
        public int Id { get; set; }
        public int LodgementId { get; set; }
        public int OfficerId { get; set; }
        public DateTime CheckedOutOn { get; set; }
    }
}
