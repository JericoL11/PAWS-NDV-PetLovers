﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Components.Appointments
{
    public class MonitoringViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public MonitoringViewComponent(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }


        //adding of parameters naka, when viewd, all data will be gone
        public async Task<IViewComponentResult> InvokeAsync( bool? logical_dateError, bool? null_dateError)
        {

            if (null_dateError == true)
            {
                ModelState.AddModelError("", "The end date cannot be earlier than the start date.");

            }
            if (logical_dateError == true)
            {
                ModelState.AddModelError("", "Both the start date and end date are required.");
            }
         
            return View(await GetAppointments());
        }


        #region == functions for Appointment Reports ==
        public async Task<ReportsVm> GetAppointments()
        {
            var reportVm = new ReportsVm
            {
                IAppointment = await _context.Appointments
                     .Include(a => a.OwnerNav)
                     .Include(a => a.IAppDetails)
                        .ThenInclude(d => d.Services)
                     .Where(a => a.remarks != "Cancelled")
                     .ToListAsync(),
            };
            return reportVm;
        }
        #endregion
    }
}
