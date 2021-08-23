using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyPlanLodgement.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Data
{
    public class SPLDataContext : IdentityDbContext<ApplicationUser>
    {
        public SPLDataContext(DbContextOptions<SPLDataContext> options)
            : base(options)
        {

        }

        public DbSet<Lodgement> Lodgements { get; set; }
        public DbSet<ClearanceLetter> ClearanceLetters { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Status> Statuses { get; set; }
    }
}
