using Microsoft.EntityFrameworkCore;
using SurveyPlanLodgement.Web.Data;
using SurveyPlanLodgement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupDocs.Viewer;
using GroupDocs.Viewer.Results;
using GroupDocs.Viewer.Options;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SurveyPlanLodgement.Web.Repository
{
    public class LodgementRepository : ILodgementRepository
    {
        private readonly SPLDataContext _context = null;
        private readonly IStatusRepository _statusRepository = null;
        private readonly IAccountRepository _accountRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;

        public LodgementRepository(SPLDataContext context, IStatusRepository statusRepository,
            IAccountRepository accountRepository, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _statusRepository = statusRepository;
            _accountRepository = accountRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<LodgementModel>> GetAllLodgementsAsync()
        {
            return await _context.Lodgements
                .Select(lodgement => new LodgementModel()
                {
                    Id = lodgement.Id,
                    Description = lodgement.Description,
                    ReferenceNumber = lodgement.ReferenceNumber,
                    SurveyorId = lodgement.SurveyorId,
                    Surveyor = _context.Users.Where(s => s.Id == lodgement.SurveyorId).FirstOrDefault(),
                    VerificationOfficerId = lodgement.VerificationOfficerId,
                    SchemePlanUrl = lodgement.SchemePlanUrl,
                    SurveyInstructionUrl = lodgement.SurveyInstructionUrl,
                    SurveyReportUrl = lodgement.SurveyReportUrl,
                    FieldNotesUrl = lodgement.FieldNotesUrl,
                    AreaCalculationSheetUrl = lodgement.AreaCalculationSheetUrl,
                    CoordinateExemptionSheetUrl = lodgement.CoordinateExemptionSheetUrl,
                    CoordinateSheetUrl = lodgement.CoordinateSheetUrl,
                    CreekOffsetSheetUrl = lodgement.CreekOffsetSheetUrl,
                    CalibrationReportUrl = lodgement.CalibrationReportUrl,
                    isCoordinateExempted = lodgement.isCoordinateExempted,
                    Status = _context.Statuses.Where(s => s.Id == lodgement.StatusId).FirstOrDefault().Name,
                    CreatedOn = lodgement.CreatedOn

                }).ToListAsync();
        }

        public async Task<LodgementModel> GetLodgmentByIdAsync(int id)
        {

            return await _context.Lodgements.Where(l => l.Id == id)
                .Select(lodgment => new LodgementModel()
                {
                    Id = lodgment.Id,
                    Description = lodgment.Description,
                    ReferenceNumber = lodgment.ReferenceNumber,
                    SurveyorId = lodgment.SurveyorId,
                    Surveyor = _context.Users.Where(s => s.Id == lodgment.SurveyorId).FirstOrDefault(),
                    VerificationOfficerId = lodgment.VerificationOfficerId,
                    SchemePlanUrl = lodgment.SchemePlanUrl,
                    SurveyInstructionUrl = lodgment.SurveyInstructionUrl,
                    SurveyReportUrl = lodgment.SurveyReportUrl,
                    FieldNotesUrl = lodgment.FieldNotesUrl,
                    AreaCalculationSheetUrl = lodgment.AreaCalculationSheetUrl,
                    CoordinateExemptionSheetUrl = lodgment.CoordinateExemptionSheetUrl,
                    CoordinateSheetUrl = lodgment.CoordinateSheetUrl,
                    CreekOffsetSheetUrl = lodgment.CreekOffsetSheetUrl,
                    CalibrationReportUrl = lodgment.CalibrationReportUrl,
                    isCoordinateExempted = lodgment.isCoordinateExempted,
                    Status = _context.Statuses.Where(s => s.Id == lodgment.StatusId).FirstOrDefault().Name,
                    CreatedOn = lodgment.CreatedOn,
                    UploadedClearanceLetters = lodgment.ClearanceLetters.Select(file => new ClearanceLetterModel()
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Url = file.Url
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<List<LodgementModel>> GetSurveyorLodgementsAsync(string id)
        {
            return await _context.Lodgements
                .Where(l => l.SurveyorId == id)
                .Select(lodgement => new LodgementModel()
                {
                    Id = lodgement.Id,
                    Description = lodgement.Description,
                    ReferenceNumber = lodgement.ReferenceNumber,
                    SurveyorId = lodgement.SurveyorId,
                    Surveyor = _context.Users.Where(s => s.Id == lodgement.SurveyorId).FirstOrDefault(),
                    VerificationOfficerId = lodgement.VerificationOfficerId,
                    SchemePlanUrl = lodgement.SchemePlanUrl,
                    SurveyInstructionUrl = lodgement.SurveyInstructionUrl,
                    SurveyReportUrl = lodgement.SurveyReportUrl,
                    FieldNotesUrl = lodgement.FieldNotesUrl,
                    AreaCalculationSheetUrl = lodgement.AreaCalculationSheetUrl,
                    CoordinateExemptionSheetUrl = lodgement.CoordinateExemptionSheetUrl,
                    CoordinateSheetUrl = lodgement.CoordinateSheetUrl,
                    CreekOffsetSheetUrl = lodgement.CreekOffsetSheetUrl,
                    CalibrationReportUrl = lodgement.CalibrationReportUrl,
                    isCoordinateExempted = lodgement.isCoordinateExempted,
                    Status = _context.Statuses.Where(s => s.Id == lodgement.StatusId).FirstOrDefault().Name,
                    CreatedOn = lodgement.CreatedOn

                }).ToListAsync();
        }

        public async Task<int> AddLodgementAsync(LodgementModel model)
        {
            var status = await _context.Statuses.Where(s => s.Name == "Submitted").FirstOrDefaultAsync();


            var newLodgement = new Lodgement()
            {
                ReferenceNumber = model.ReferenceNumber,
                Description = model.Description,
                SurveyorId = model.SurveyorId,
                VerificationOfficerId = model.VerificationOfficerId,
                SchemePlanUrl = model.SchemePlanUrl,
                SurveyInstructionUrl = model.SurveyInstructionUrl,
                SurveyReportUrl = model.SurveyReportUrl,
                FieldNotesUrl = model.FieldNotesUrl,
                AreaCalculationSheetUrl = model.AreaCalculationSheetUrl,
                CoordinateExemptionSheetUrl = model.CoordinateExemptionSheetUrl,
                CoordinateSheetUrl = model.CoordinateSheetUrl,
                CreekOffsetSheetUrl = model.CreekOffsetSheetUrl,
                CalibrationReportUrl = model.CalibrationReportUrl,
                isCoordinateExempted = model.isCoordinateExempted,
                StatusId = status.Id,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow

            };

            newLodgement.ClearanceLetters = new List<ClearanceLetter>();

            //Uploading multiple files into the database
            foreach (var file in model.UploadedClearanceLetters)
            {
                newLodgement.ClearanceLetters.Add(new ClearanceLetter()
                {
                    Name = file.Name,
                    Url = file.Url,

                });

            }

            await _context.Lodgements.AddAsync(newLodgement);
            await _context.SaveChangesAsync();

            return newLodgement.Id;
        }


        //public async Task<int> UpdateLodgmentStatusAsync(LodgementModel model)
        //{
        //    var lodgement = await _context.Lodgements.Where(l => l.Id == model.Id).FirstOrDefaultAsync();
        //    var status = await _context.Statuses.Where(l => l.Id == model.Status)

        //    lodgement.StatusId
        //}

        // Upload file method
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

