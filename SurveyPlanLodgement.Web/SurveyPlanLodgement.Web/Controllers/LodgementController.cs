using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;
using GroupDocs.Viewer.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SurveyPlanLodgement.Web.Data;
using SurveyPlanLodgement.Web.Models;
using SurveyPlanLodgement.Web.Repository;
using SurveyPlanLodgement.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Controllers
{
    [Authorize]
    public class LodgementController : Controller
    {
        private readonly ILodgementRepository _lodgementRepository = null;
        private readonly IUserService _userService = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;
        private readonly SPLDataContext _context = null;


        public LodgementController(ILodgementRepository lodgementRepository, IWebHostEnvironment webHostEnvironment, 
            IUserService userService, SPLDataContext context)
        {
            _lodgementRepository = lodgementRepository;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [Route("lodgements")]
        public async Task<IActionResult> Index()
        {
            var user = _userService.GetUserId();

            if (User.IsInRole("Admin"))
            {
                var model = await _lodgementRepository.GetAllLodgementsAsync();

                return View(model);
            }
            else
            {
                var model = await _lodgementRepository.GetSurveyorLodgementsAsync(user);

                return View(model);
            }          

            
        }

        [Route("apply-lodgement")]
        public IActionResult Create()
        {
            ViewBag.UserId = _userService.GetUserId();
            return View();
        }

        [HttpPost]
        [Route("apply-lodgement")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LodgementModel model)
        {
            if (ModelState.IsValid)
            {
                model.ReferenceNumber = await GenerateReferenceNumber();



                string folder = "uploads/" + model.ReferenceNumber + "/";

                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

                Directory.CreateDirectory(serverFolder);

                // If file is available for upload
                if(model.SchemePlan != null)
                {
                    model.SchemePlanUrl = await UploadFile(folder, model.SchemePlan);
                }
                // If file is available for upload
                if (model.SurveyInstruction != null)
                {
                    model.SurveyInstructionUrl = await UploadFile(folder, model.SurveyInstruction);
                }
                // If file is available for upload
                if (model.SurveyReport != null)
                {
                    model.SurveyReportUrl = await UploadFile(folder, model.SurveyReport);
                }
                // If file is available for upload
                if (model.FieldNotes != null)
                {
                    model.FieldNotesUrl = await UploadFile(folder, model.FieldNotes);
                }
                // If file is available for upload
                if (model.FieldNotes != null)
                {
                    model.FieldNotesUrl = await UploadFile(folder, model.FieldNotes);
                }
                // If file is available for upload
                if (model.AreaCalculationSheet != null)
                {
                    model.AreaCalculationSheetUrl = await UploadFile(folder, model.AreaCalculationSheet);
                }
                // If file is available for upload
                if (model.CoordinateExemptionSheet != null)
                {
                    model.CoordinateExemptionSheetUrl = await UploadFile(folder, model.CoordinateExemptionSheet);
                }
                // If file is available for upload
                if (model.CoordinateSheet != null)
                {
                    model.CoordinateSheetUrl = await UploadFile(folder, model.CoordinateSheet);
                }
                // If file is available for upload
                if (model.CreekOffsetSheet != null)
                {
                    model.CreekOffsetSheetUrl = await UploadFile(folder, model.CreekOffsetSheet);
                }
                // If file is available for upload
                if (model.CalibrationReport != null)
                {
                    model.CalibrationReportUrl = await UploadFile(folder, model.CalibrationReport);
                }

                if (model.ClearanceLetters != null)
                {
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

        [Route("view-lodgment")]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _lodgementRepository.GetLodgmentByIdAsync(id);

            string folderPath = "uploads/" + model.ReferenceNumber + "/";
            var storagePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            var fileList = new List<string>();

            var files = Directory.GetFiles(storagePath);
            foreach (var file in files)
            {
                fileList.Add(Path.GetFileName(file));
            }

            ViewBag.FileList = fileList;
            ViewBag.StoragePath = storagePath;

            return View(model);
        }

        [HttpPost]
        [Route("view-document")]
        public IActionResult ViewDocument(string fileName, string storagePath)
        {
            int pageCount = 0;
            string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "contents");
            string imageFilesFolder = Path.Combine(outputPath, Path.GetFileName(fileName).Replace(".", "_"));
            if (!Directory.Exists(imageFilesFolder))
            {
                Directory.CreateDirectory(imageFilesFolder);
            }

            string imageFilesPath = Path.Combine(imageFilesFolder, "page-{0}.png");

            using (Viewer viewer = new Viewer(Path.Combine(storagePath, fileName)))
            {
                //Get document info
                ViewInfo info = viewer.GetViewInfo(ViewInfoOptions.ForPngView(false));
                pageCount = info.Pages.Count;
                //Set options and render document
                PngViewOptions options = new PngViewOptions(imageFilesPath);
                viewer.View(options);
            }

            return new JsonResult(pageCount);

        }

        [Route("withdraw-lodgement")]
        public async Task<IActionResult> Withdraw(int id)
        {
            var model = await _lodgementRepository.GetLodgmentByIdAsync(id);
            return View("_Withdraw", model);
        }

        private async Task<string> UploadFile(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;            

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);            

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }

        private async Task<string> GenerateReferenceNumber()
        {
            Random rand = new Random();

            var existingList = await _context.Lodgements.Select(l => l.ReferenceNumber).ToListAsync();

            var year = DateTime.Now.Year.ToString();

            var referenceNumber = "";
            referenceNumber = "PL-" + year + rand.Next(10000000, 99999999).ToString();

            while (existingList.Contains(referenceNumber))
            {
                referenceNumber = "PL-" + year + rand.Next(10000000, 99999999).ToString();
            }

            return referenceNumber;
        }
    }
}
