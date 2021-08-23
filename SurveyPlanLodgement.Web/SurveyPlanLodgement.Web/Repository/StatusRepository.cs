using Microsoft.EntityFrameworkCore;
using SurveyPlanLodgement.Web.Data;
using SurveyPlanLodgement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly SPLDataContext _context = null;

        public StatusRepository(SPLDataContext context)
        {
            _context = context;
        }

        public async Task<List<StatusModel>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Select(status => new StatusModel()
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description
                }).ToListAsync();
        }

        public async Task<StatusModel> GetStatusById(int id)
        {
            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == id);

            var statusVM = new StatusModel()
            {
                Id = status.Id,
                Name = status.Name,
                Description = status.Description,
            };

            return statusVM;
        }

        public async Task<string> GetStatusNameById(int id)
        {
            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == id);

            var statusVM = new StatusModel()
            {
                Id = status.Id,
                Name = status.Name,
                Description = status.Description,
            };

            return statusVM.Name;
        }

        public async Task<int> AddNewStatus(StatusModel model)
        {
            var newStatus = new Status()
            {
                Name = model.Name,
                Description = model.Description
            };

            await _context.Statuses.AddAsync(newStatus);
            await _context.SaveChangesAsync();

            return newStatus.Id;
        }

        public async Task<int> EditStatus(StatusModel model)
        {
            var status = await _context.Statuses.FirstOrDefaultAsync(t => t.Id == model.Id);

            if (status != null)
            {
                status.Name = model.Name;
                status.Description = model.Description;
            }

            _context.Statuses.Update(status);

            await _context.SaveChangesAsync();

            return status.Id;
        }
        public async Task<int> DeleteStatus(int id)
        {
            var status = await _context.Statuses.FirstOrDefaultAsync(t => t.Id == id);

            _context.Statuses.Remove(status);

            await _context.SaveChangesAsync();

            return status.Id;
        }

    }
}
