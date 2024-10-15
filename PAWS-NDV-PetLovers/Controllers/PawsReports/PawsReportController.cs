using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace PAWS_NDV_PetLovers.Controllers.PawsReports
{
    public class PawsReportController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public PawsReportController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }


        //functions
        public async Task<ReportsVm> GetAppointments()
        {
            var reportVm = new ReportsVm
            {
                IAppointment = await _context.Appointments
                     .Include(a => a.OwnerNav)
                     .Include(a => a.IAppDetails)
                        .ThenInclude(d => d.Services)
                     .Where(a => !string.IsNullOrEmpty(a.remarks))
                     .ToListAsync(),
            };
            return reportVm;
        }
        public async Task<ReportsVm> GetAllAppointments(bool filtered,string? selection)
        {
            var reportVm = new ReportsVm
            {
                IAppointment = await _context.Appointments
                     .Include(a => a.IAppDetails)
                     .ThenInclude(a => a.Services)
                     .Include(a => a.OwnerNav)
                     .Where(a => !string.IsNullOrEmpty(a.remarks))
                     .ToListAsync(),
                Filtered = filtered,
                Selection = selection

            };
            return reportVm;
        }

        public async Task<ReportsVm> GetFilteredAppointments(DateTime? startDate, DateTime? endDate, bool filtered, string? selection)

        {
            var reportVm = new ReportsVm
            {
                IAppointment = await _context.Appointments
                    .Include(a => a.IAppDetails)
                     .ThenInclude(a => a.Services)
                    .Include(a => a.OwnerNav)
                    .Where(d => (!startDate.HasValue || d.date >= startDate)
                             && (!endDate.HasValue || d.date <= endDate)
                             && !string.IsNullOrEmpty(d.remarks))
                    .ToListAsync(),
                Filtered = filtered,
                startDate = startDate,
                Selection = selection,
                endDate = endDate 
            };
            return reportVm;
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentsReport()
        {
            return View(await GetAppointments());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AppointmentsReport(DateTime? startDate, DateTime? endDate, string? selection)
        {
            if (selection == "custom")
            {
                // Validate the date range
                if (!startDate.HasValue || !endDate.HasValue)
                {
                    ModelState.AddModelError("", "Please select both Start Date and End Date.");
                    return View(await GetAppointments());
                }

                if (startDate > endDate)
                {
                    ModelState.AddModelError("", "End date must not be before Start Date.");
                    return View(await GetAppointments());
                }

                // Fetch filtered appointments based on the date range
                var reportData = await GetFilteredAppointments(startDate, endDate, true, selection);
                return View(reportData);  // Pass the filtered data to the view
            }
           
            if (selection == "all")
            {
                // Fetch all appointments without any date filtering
                return View(await GetAllAppointments(true, selection));
            }

            // Default return in case something is missed
            return View(await GetAppointments());
        }


        public IActionResult ProductMngmtReport()
        {
            return View();
        }
    }
}
