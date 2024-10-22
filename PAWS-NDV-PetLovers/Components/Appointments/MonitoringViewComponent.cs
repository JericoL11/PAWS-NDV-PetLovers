using Microsoft.AspNetCore.Mvc;
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



        public async Task<IViewComponentResult> InvokeAsync()
        {

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
