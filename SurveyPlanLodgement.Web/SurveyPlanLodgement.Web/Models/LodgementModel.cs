using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Models
{
    public class LodgementModel
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string SurveyorId { get; set; }
        public ApplicationUser Surveyor { get; set; }
        public string VerificationOfficerId { get; set; }
        
        [Required]
        [Display(Name = "Please upload valid Scheme Plan")]
        public IFormFile SchemePlan { get; set; }
        public string SchemePlanUrl { get; set; }

        [Required]
        [Display(Name = "Please upload Survey Instruction")]
        public IFormFile SurveyInstruction { get; set; }
        public string SurveyInstructionUrl { get; set; }

        [Required]
        [Display(Name = "Please upload Survey Report")]
        public IFormFile SurveyReport { get; set; }
        public string SurveyReportUrl { get; set; }

        [Required]
        [Display(Name = "Please upload Field Notes")]
        public IFormFile FieldNotes { get; set; }
        public string FieldNotesUrl { get; set; }

        [Required]
        [Display(Name = "Please upload Area Calculation Sheet")]
        public IFormFile AreaCalculationSheet { get; set; }
        public string AreaCalculationSheetUrl { get; set; }

        [Required]
        [Display(Name = "Do you have a Coordinate Exemption Letter?")]
        public bool isCoordinateExempted { get; set; }

        [Display(Name = "Please upload Coordinate Exemption Letter")]
        public IFormFile CoordinateExemptionSheet { get; set; }
        public string CoordinateExemptionSheetUrl { get; set; }

        [Display(Name = "Please upload Coordinate Sheet")]
        public IFormFile CoordinateSheet { get; set; }
        public string CoordinateSheetUrl { get; set; }

        [Display(Name = "Please upload Creek Offset Sheet")]
        public IFormFile CreekOffsetSheet { get; set; }
        public string CreekOffsetSheetUrl { get; set; }

        [Required]
        [Display(Name = "Please upload Instrument Calibration Report")]
        public IFormFile CalibrationReport { get; set; }
        public string CalibrationReportUrl { get; set; }

        [Display(Name = "Please upload relevant clearance letters")]
        public IFormFileCollection ClearanceLetters { get; set; }

        public int StatusId { get; set; }
        public string Status { get; set; }

        //Property to retrieve documents from database
        public List<ClearanceLetterModel> UploadedClearanceLetters { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }


    }
}
