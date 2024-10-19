using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;
using System.ComponentModel.DataAnnotations;
using static PAWS_NDV_PetLovers.ViewModels.ReportsVm;

namespace PAWS_NDV_PetLovers.Controllers.PawsReports
{
    public class PawsReportController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public PawsReportController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentsReport()
        {
            return View(await GetAppointments());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AppointmentsReport(string reportType,DateTime? startDate, DateTime? endDate, string? SelectType, string? Status, bool Filtered)
        {

            switch (reportType)
            {
                case "booking":
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
                        var reportData = await GetCustomAppointments(reportType, startDate, endDate, Status, SelectType, true);
                        return View(reportData);  // Pass the filtered data to the view
                    }

                    if (SelectType == "all")
                    {
                        // Fetch all appointments without any date filtering
                        return View(await GetAllAppointments(reportType, startDate, endDate, Status, SelectType, true));
                    }
                    break;


                case "followUp":
                    if (SelectType == "custom")
                    {
                        // Validate the date range
                        if (!startDate.HasValue || !endDate.HasValue)
                        {
                            ModelState.AddModelError("", "Both the start date and end date are required.");
                            return View(await GetFollowUp());
                        }

                        if (startDate > endDate)
                        {
                            ModelState.AddModelError("", "The end date cannot be earlier than the start date.");
                            return View(await GetFollowUp());
                        }

                        // Fetch filtered appointments based on the date range
                        var reportData = await GetCustomFollowUp(reportType, startDate, endDate, Status, SelectType, true);
                        return View(reportData);  // Pass the filtered data to the view
                    }

                    if (SelectType == "all")
                    {
                        // Fetch all appointments without any date filtering
                        return View(await GetAllFollowUp(reportType, startDate, endDate, Status, SelectType, true));
                    }

                    break;

                default:
                    
                    var followUp = await _context.PetFollowUps
                   .Include(p => p.Diagnostics)
                       .ThenInclude(d => d.pet)
                           .ThenInclude(dp => dp.owner)
                   .Include(p => p.Services)
                   .Where(p => p.status != "Cancelled")
                   .ToListAsync();

                
                    var reportVm = new ReportsVm
                    {
                        IPetFollowUps = followUp,
                        IAppointment = await _context.Appointments
                       .Include(a => a.OwnerNav)
                       .Include(a => a.IAppDetails)
                          .ThenInclude(d => d.Services)
                       .Where(a => a.remarks != "Cancelled")
                       .ToListAsync(),
                    };
                    break;
            }
            // Default return in case something is missed
            return View(await GetAppointments());
        }
           


        //Follow Up
        [HttpGet]
        public async Task<IActionResult> FollowUpVisitReport()
        {
            return View(await GetFollowUp());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FollowUpVisitReport(string? reportType, DateTime? startDate, DateTime? endDate, string? SelectType, string? Status, bool Filtered)
        {
            if (SelectType == "custom")
            {
                // Validate the date range
                if (!startDate.HasValue || !endDate.HasValue)
                {
                    ModelState.AddModelError("", "Both the start date and end date are required.");
                    return View(await GetFollowUp());
                }

                if (startDate > endDate)
                {
                    ModelState.AddModelError("", "The end date cannot be earlier than the start date.");
                    return View(await GetFollowUp());
                }

                // Fetch filtered appointments based on the date range
                var reportData = await GetCustomFollowUp(reportType, startDate, endDate, Status, SelectType, true);
                return View(reportData);  // Pass the filtered data to the view
            }

            if (SelectType == "all")
            {
                // Fetch all appointments without any date filtering
                return View(await GetAllFollowUp(reportType, startDate, endDate, Status, SelectType, true));
            }

            // Default return in case something is missed
            return View(await GetAppointments());
        }


        [HttpGet]
        public async Task<IActionResult> ProductMgmtReport()
        {
            return View(await GetProducts());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductMgmtReport(string SelectType, string? Status)
        {
            if (SelectType != "allCategory")
            {
                var checkCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.categoryName == SelectType);

                if (checkCategory != null)
                {
                    switch (Status)
                    {
                        case "Low":
                            return View(await GetLowStockProducts(SelectType, Status, true));

                        case "High":
                            return View(await GetHighStockProducts(SelectType, Status, true));


                        case "All":
                            return View(await GetAllStockProducts(SelectType, Status, true));

                    }
                }
            }
            else
            {
                return View(await GetAllCategoryProducts(SelectType, Status, true));
            }


            return View();
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

        //filtered 
        public async Task<ReportsVm> GetAllAppointments(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)
        {

            var reportVm = new ReportsVm
            {
                IAppointment = await _context.Appointments
                 .Include(a => a.IAppDetails)
                 .ThenInclude(a => a.Services)
                 .Include(a => a.OwnerNav)
                 .Where(d => (!startDate.HasValue || d.date >= startDate)
                        && (!endDate.HasValue || d.date <= endDate)
                        && d.remarks != "Cancelled")
                 .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = Filtered,
                startDate = startDate,
                endDate = endDate,
                reportType = reportType

            };
            return reportVm;
        }

        public async Task<ReportsVm> GetCustomAppointments(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)

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
                    Filtered = Filtered,
                    reportType = reportType
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
                    Filtered = Filtered,
                    reportType = reportType
                };
                return reportVm;
            }
        }
        #endregion

        #region == Functions for Follow Up == 

        public async Task<ReportsVm> GetFollowUp()
        {
            var followUp = await _context.PetFollowUps
                .Include(p => p.Diagnostics)
                    .ThenInclude(d => d.pet)
                        .ThenInclude(dp => dp.owner)
                .Include(p => p.Services)
                .Where(p => p.status != "Cancelled")
                .ToListAsync();

            var reportVm = new ReportsVm
            {
                IPetFollowUps = followUp
            };

            return reportVm;
        }


        public async Task<ReportsVm> GetAllFollowUp(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)
        {
            var reportVm = new ReportsVm
            {
                IPetFollowUps = await _context.PetFollowUps
                .Include(p => p.Diagnostics)
                    .ThenInclude(d => d.pet)
                        .ThenInclude(dp => dp.owner)
                 .Include(a => a.Services)
                 .Where(a => (!startDate.HasValue || a.date >= startDate)
                        && (!endDate.HasValue || a.date <= endDate)
                        && a.status != "Cancelled")
                 .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = Filtered,
                startDate = startDate,
                endDate = endDate,
                reportType = reportType
            };
            return reportVm;
        }

        public async Task<ReportsVm> GetCustomFollowUp(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)

        {
            if (Status == "inProgress")
            {
                var reportVm = new ReportsVm
                {
                    IPetFollowUps = await _context.PetFollowUps
                    .Include(p => p.Diagnostics)
                        .ThenInclude(d => d.pet)
                            .ThenInclude(dp => dp.owner)
                    .Include(a => a.Services)
                    .Where(d => (!startDate.HasValue || d.date >= startDate)
                             && (!endDate.HasValue || d.date <= endDate)
                             && string.IsNullOrEmpty(d.status))
                    .ToListAsync(),
                    Status = Status,
                    startDate = startDate,
                    SelectType = SelectType,
                    endDate = endDate,
                    Filtered = Filtered,
                    reportType = reportType
                };
                return reportVm;
            }
            else
            {
                var reportVm = new ReportsVm
                {
                    IPetFollowUps = await _context.PetFollowUps
                    .Include(p => p.Diagnostics)
                         .ThenInclude(d => d.pet)
                             .ThenInclude(dp => dp.owner)
                   .Include(a => a.Services)
                   .Where(d => (!startDate.HasValue || d.date >= startDate)
                            && (!endDate.HasValue || d.date <= endDate)
                            && d.status != "Cancelled"
                            && !string.IsNullOrEmpty(d.status))
                   .ToListAsync(),
                    Status = Status,
                    startDate = startDate,
                    SelectType = SelectType,
                    endDate = endDate,
                    Filtered = Filtered,
                    reportType = reportType
                };
                return reportVm;
            }
        }


        #endregion

        #region == functions for Product Management == 

        public async Task GetAllCategory()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "categoryName", "categoryName");
        }
        public async Task GetSelectedCategory(string selectType)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "categoryName", "categoryName", selectType);
        }
        public async Task<ReportsVm> GetProducts()
        {
            await GetAllCategory();

            var vm = new ReportsVm
            {
                IProducts = await _context.Products.ToListAsync()
            };
            return vm;

        }

        //stock levels
        public async Task<ReportsVm> GetLowStockProducts(string SelectType, string Status, bool filtered)
        {
            await GetSelectedCategory(SelectType);
            var vm = new ReportsVm
            {
                IProducts = await _context.Products
                .Include(p => p.category)
                .Where(p => p.quantity <= 10 && p.category.categoryName == SelectType)
                .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = filtered
            };
            return vm;

        }
        public async Task<ReportsVm> GetHighStockProducts(string SelectType, string Status, bool filtered)
        {
            await GetSelectedCategory(SelectType);
            var vm = new ReportsVm
            {
                IProducts = await _context.Products
                .Include(p => p.category)
                .Where(p => p.quantity >= 11 && p.category.categoryName == SelectType)
                .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = filtered
            };
            return vm;

        }
        public async Task<ReportsVm> GetAllStockProducts(string SelectType, string Status, bool filtered)
        {
            await GetSelectedCategory(SelectType);
            var vm = new ReportsVm
            {
                IProducts = await _context.Products
                .Include(p => p.category)
                .Where(p => p.category.categoryName == SelectType)
                .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = filtered
            };
            return vm;

        }

        public async Task<ReportsVm> GetAllCategoryProducts(string SelectType, string Status, bool filtered)
        {
            await GetAllCategory();
            var vm = new ReportsVm
            {
                IProducts = await _context.Products
                .Include(p => p.category)
                .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = filtered
            };
            return vm;

            //end of stock levels

            #endregion

        }
    }
}
