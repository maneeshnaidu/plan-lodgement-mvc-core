using Microsoft.AspNetCore.Mvc;
using SurveyPlanLodgement.Web.Models;
using SurveyPlanLodgement.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusRepository _statusRepository = null;

        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _statusRepository.GetAllStatusesAsync();
            return View(model);
        }

        [Route("add-status")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("add-status")]
        public async Task<IActionResult> Create(StatusModel model)
        {
            if (ModelState.IsValid)
            {
                int id = await _statusRepository.AddNewStatus(model);

                if (id > 0)
                {
                    return RedirectToAction(nameof(Create));
                }
            }

            //Send custom error message to validation summary
            ModelState.AddModelError("", "This is a custom error message");


            return View();
        }

        [Route("edit-status")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _statusRepository.GetStatusById(id);
            return View(model);
        }

        [HttpPost]
        [Route("edit-status")]
        public async Task<IActionResult> Edit(StatusModel model)
        {
            if (ModelState.IsValid)
            {
                //Write your code
                var result = await _statusRepository.EditStatus(model);

                if (result < 0)
                {

                    return View();
                }

                ModelState.Clear();

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
