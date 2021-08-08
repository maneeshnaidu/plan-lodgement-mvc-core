using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyPlanLodgement.Web.Models;
using SurveyPlanLodgement.Web.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Controllers
{
    public class LodgementController : Controller
    {
        private readonly ILodgementRepository _lodgementRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;

        public LodgementController(ILodgementRepository lodgementRepository, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("apply-lodgement")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("apply-lodgement")]
        public async Task<IActionResult> Create(LodgementModel model)
        {
            if (ModelState.IsValid)
            {
                // If Scheme Plan is available for upload
                if(model.SchemePlan != null)
                {
                    string folder = "uploads/scheme_plan/";
                    model.SchemePlanUrl = await UploadFile(folder, model.SchemePlan);
                }
                // If file is available for upload
                if (model.SurveyInstruction != null)
                {
                    string folder = "uploads/survey_instruction/";
                    model.SurveyInstructionUrl = await UploadFile(folder, model.SurveyInstruction);
                }
                // If file is available for upload
                if (model.SurveyReport != null)
                {
                    string folder = "uploads/survey_report/";
                    model.SurveyReportUrl = await UploadFile(folder, model.SurveyReport);
                }
                // If file is available for upload
                if (model.FieldNotes != null)
                {
                    string folder = "uploads/survey_report/";
                    model.FieldNotesUrl = await UploadFile(folder, model.FieldNotes);
                }
                // If file is available for upload
                if (model.FieldNotes != null)
                {
                    string folder = "uploads/field_notes/";
                    model.FieldNotesUrl = await UploadFile(folder, model.FieldNotes);
                }
                // If file is available for upload
                if (model.AreaCalculationSheet != null)
                {
                    string folder = "uploads/area_calculation_sheet/";
                    model.AreaCalculationSheetUrl = await UploadFile(folder, model.AreaCalculationSheet);
                }
                // If file is available for upload
                if (model.CoordinateExemptionSheet != null)
                {
                    string folder = "uploads/coordinate_exemption_sheet/";
                    model.CoordinateExemptionSheetUrl = await UploadFile(folder, model.CoordinateExemptionSheet);
                }
                // If file is available for upload
                if (model.CoordinateSheet != null)
                {
                    string folder = "uploads/coordinate_sheet/";
                    model.CoordinateSheetUrl = await UploadFile(folder, model.CoordinateSheet);
                }
                // If file is available for upload
                if (model.CreekOffsetSheet != null)
                {
                    string folder = "uploads/creek_offset_sheet/";
                    model.CreekOffsetSheetUrl = await UploadFile(folder, model.CreekOffsetSheet);
                }
                // If file is available for upload
                if (model.CalibrationReport != null)
                {
                    string folder = "uploads/calibration_report/";
                    model.CalibrationReportUrl = await UploadFile(folder, model.CalibrationReport);
                }

                if (model.ClearanceLetters != null)
                {
                    string folder = "uploads/clearance_letters/";
                    model.UploadedClearanceLetters = new List<ClearanceLetterModel>();

                    foreach (var file in model.ClearanceLetters)
                    {
                        var letter = new ClearanceLetterModel()
                        {
                            Name = file.FileName,
                            Url = await UploadFile(folder, file)
                        };
                        model.UploadedClearanceLetters.Add(letter);
                    }

                }

                int id = await _lodgementRepository.AddLodgementAsync(model);

                if(id > 0)
                {
                    return RedirectToAction(nameof(Create), new { isSuccess = true, lodgementId = id });
                }
            }

            return View();
        }

        private async Task<string> UploadFile(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
    }
}
