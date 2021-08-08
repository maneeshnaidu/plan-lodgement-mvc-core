using Microsoft.EntityFrameworkCore;
using SurveyPlanLodgement.Web.Data;
using SurveyPlanLodgement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public class LodgementRepository : ILodgementRepository
    {
        private readonly SPLDataContext _context = null;

        public LodgementRepository(SPLDataContext context)
        {
            _context = context;
        }

        public async Task<List<LodgementModel>> GetLodgementsAsync()
        {
            return await _context.Lodgements
                .Select(lodgement => new LodgementModel()
                {
                    Id = lodgement.Id,
                    Description = lodgement.Description,
                    SurveyorId = lodgement.SurveyorId,
                    VerificationOfficerId = lodgement.VerificationOfficerId

                }).ToListAsync();
        }

        public async Task<int> AddLodgementAsync(LodgementModel model)
        {
            var newLodgement = new Lodgement()
            {
                Description = model.Description,
                SurveyorId = model.SurveyorId,
                VerificationOfficerId = (int)(model.VerificationOfficerId.HasValue ? model.VerificationOfficerId : 0),
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
    }
}
