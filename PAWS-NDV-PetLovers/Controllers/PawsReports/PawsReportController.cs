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


        //start of functions
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

        //filtered 
        public async Task<ReportsVm> GetAllAppointments(string? Status, string? SelectType, bool Filtered)
        {
            var reportVm = new ReportsVm
            {
                IAppointment = await _context.Appointments
                     .Include(a => a.IAppDetails)
                     .ThenInclude(a => a.Services)
                     .Include(a => a.OwnerNav)
                     .Where(a => a.remarks != "Cancelled")
                     .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = Filtered

            };
            return reportVm;
        }

        public async Task<ReportsVm> GetCustomAppointments(DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)

        {
            if (Status == "inProgress")
            {
                var reportVm = new ReportsVm
                {
                    IAppointment = await _context.Appointments
                    .Include(a => a.IAppDetails)
                     .ThenInclude(a => a.Services)
                    .Include(a => a.OwnerNav)
                    .Where(d => (!startDate.HasValue || d.date >= startDate)
                             && (!endDate.HasValue || d.date <= endDate)
                             && string.IsNullOrEmpty(d.remarks))
                    .ToListAsync(),
                    Status = Status,
                    startDate = startDate,
                    SelectType = SelectType,
                    endDate = endDate,
                    Filtered = Filtered
                };
                return reportVm;
            }
            else
            {
                var reportVm = new ReportsVm
                {
                    IAppointment = await _context.Appointments
                   .Include(a => a.IAppDetails)
                    .ThenInclude(a => a.Services)
                   .Include(a => a.OwnerNav)
                   .Where(d => (!startDate.HasValue || d.date >= startDate)
                            && (!endDate.HasValue || d.date <= endDate)
                            && d.remarks != "Cancelled"
                            && !string.IsNullOrEmpty(d.remarks))
                   .ToListAsync(),
                    Status = Status,
                    startDate = startDate,
                    SelectType = SelectType,
                    endDate = endDate,
                    Filtered = Filtered
                };
                return reportVm;
            }

            
        }

        // end of functions

        [HttpGet]
        public async Task<IActionResult> AppointmentsReport()
        {
            return View(await GetAppointments());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AppointmentsReport(DateTime? startDate, DateTime? endDate, string? SelectType, string? Status, bool Filtered)
        {
            if (SelectType == "custom")
            {
                // Validate the date range
                if (!startDate.HasValue || !endDate.HasValue)
                {
                    ModelState.AddModelError("", "Both the start date and end date are required.");
                    return View(await GetAppointments());
                }

                if (startDate > endDate)
                {
                    ModelState.AddModelError("", "The end date cannot be earlier than the start date.");
                    return View(await GetAppointments());
                }

                // Fetch filtered appointments based on the date range
                var reportData = await GetCustomAppointments(startDate, endDate, Status, SelectType, true);
                return View(reportData);  // Pass the filtered data to the view
            }
           
            if (SelectType == "all")
            {
                // Fetch all appointments without any date filtering
                return View(await GetAllAppointments(Status, SelectType, true));
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
