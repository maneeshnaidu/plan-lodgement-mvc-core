using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Data
{
    public class ClearanceLetter
    {
        public int Id { get; set; }
        public int LodgementId { get; set; }
        public string Name { get; set; }        
        public string Url { get; set; }

        public Lodgement Lodgement { get; set; }
    }
}
