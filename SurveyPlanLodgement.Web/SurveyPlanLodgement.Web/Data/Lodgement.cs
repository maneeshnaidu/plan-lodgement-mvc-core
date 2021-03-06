using SurveyPlanLodgement.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace SurveyPlanLodgement.Web.Data
{
    public class Lodgement
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string SurveyorId { get; set; }
        public string VerificationOfficerId { get; set; }
        public int StatusId { get; set; }
        public string SchemePlanUrl { get; set; }
        public string SurveyInstructionUrl { get; set; }
        public string SurveyReportUrl { get; set; }
        public string FieldNotesUrl { get; set; }
        public string AreaCalculationSheetUrl { get; set; }
        public bool isCoordinateExempted { get; set; }
        public string CoordinateExemptionSheetUrl { get; set; }
        public string CoordinateSheetUrl { get; set; }
        public string CreekOffsetSheetUrl { get; set; }
        public string CalibrationReportUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ICollection<ClearanceLetter> ClearanceLetters { get; set; }
    }
}
