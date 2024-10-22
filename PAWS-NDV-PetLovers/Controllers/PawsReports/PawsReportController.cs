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


        ///THE VUEW IF THIS REPORST IS LOCATED IN THE VIEWCOMPOENENTS MONITORING
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
                   
                        //validating the inputs
                        var validation = ValidateDateRange(startDate, endDate, reportType, SelectType);

                        if (validation != null)
                        {
                            return RedirectToAction("Dashboard", "Appointments", validation);
                        }

                        // Fetch filtered appointments based on the date range
                        var reportData = await GetCustomAppointments(reportType, startDate, endDate, Status, SelectType, true);

                        return RedirectToAction("Dashboard","Appointments",reportData);  // Pass the filtered data to the view
                    }

                    if (SelectType == "all")
                    {

                        //validating the inputs
                        var validation = ValidateDateRange(startDate, endDate, reportType, SelectType);

                        if (validation != null)
                        {
                            return RedirectToAction("Dashboard", "Appointments", validation);
                        }

                        var result = await GetAllAppointments(reportType, startDate, endDate, Status, SelectType, true);

                        return RedirectToAction("Dashboard", "Appointments", result);
                        
                    }
                    break;


                case "followUp":
                    if (SelectType == "custom")
                    {
                        //validating the inputs
                        var validation = ValidateDateRange(startDate, endDate, reportType, SelectType);

                        if (validation != null)
                        {
                            return RedirectToAction("Dashboard", "Appointments", validation);
                        }

                        // Fetch filtered appointments based on the date range
                        var reportData = await GetCustomFollowUp(reportType, startDate, endDate, Status, SelectType, true);
                        return RedirectToAction("Dashboard", "Appointments",reportData);  // Pass the filtered data to the view
                    }

                    if (SelectType == "all")
                    {

                        //validating the inputs
                        var validation = ValidateDateRange(startDate, endDate, reportType, SelectType);

                        if (validation != null)
                        {
                            return RedirectToAction("Dashboard", "Appointments", validation);
                        }

                        var reportData = await GetAllFollowUp(reportType, startDate, endDate, Status, SelectType, true);

                        // Fetch all appointments without any date filtering
                        return RedirectToAction("Dashboard", "Appointments", reportData);
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


                    var appointment = await _context.Appointments
                          .Include(a => a.OwnerNav)
                          .Include(a => a.IAppDetails)
                             .ThenInclude(d => d.Services)
                          .Where(a => a.remarks != "Cancelled")
                          .ToListAsync();

                    var reportVm = new AppointmentVm
                    {
                        IPetFollowUps = followUp,
                        IEAppointment = appointment
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
                //validating the inputs
                var validation = ValidateDateRange(startDate, endDate, reportType, SelectType);

                if (validation != null)
                {
                    return RedirectToAction("Dashboard", "Appointments", validation);
                }

                // Fetch filtered appointments based on the date range
                var reportData = await GetCustomFollowUp(reportType, startDate, endDate, Status, SelectType, true);
                return RedirectToAction("Dashboard", "Appointments",reportData);  // Pass the filtered data to the view
            }

            if (SelectType == "all")
            {
                //validating the inputs
                var validation = ValidateDateRange(startDate, endDate, reportType, SelectType);

                if (validation != null)
                {
                    return RedirectToAction("Dashboard", "Appointments", validation);
                }

                // Fetch filtered appointments based on the date range
                var reportData = await GetCustomFollowUp(reportType, startDate, endDate, Status, SelectType, true);
                return RedirectToAction("Dashboard", "Appointments", reportData);  // Pass the filtered data to the view
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
                switch (Status)
                {
                    case "Low":
                        return View(await GetAllcategoryLowProducts(SelectType, Status, true));

                    case "High":
                        return View(await GetAllcategoryHighProducts(SelectType, Status, true));

                    case "All":
                        return View(await GetAllCategoryProducts(SelectType, Status, true));

                }
                return View(await GetAllCategoryProducts(SelectType, Status, true));
            }


            return View();
        }


        #region == functions for Appointment Reports ==

        private AppointmentVm ValidateDateRange(DateTime? startDate, DateTime? endDate, string reportType, string SelectType)
        {
            // If either date is null, set null_dateError to true
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return new AppointmentVm
                {
                    reportType = reportType,
                    startDate = startDate?.Date ?? DateTime.MinValue,  // Default to DateTime.MinValue if null
                    endDate = endDate?.Date ?? DateTime.MinValue,
                    SelectType = SelectType,
                    activeAppointTab = AppointmentTab.monitoring,
                    null_dateError = true,
                };
            }

            // If startDate is greater than endDate, set logical_dateError to true
            if (startDate > endDate)
            {
                return new AppointmentVm
                {
                    reportType = reportType,
                    startDate = startDate.Value.Date,
                    endDate = endDate.Value.Date,
                    SelectType = SelectType,
                    activeAppointTab = AppointmentTab.monitoring,
                    logical_dateError = true,
                };
            }

            // If no errors, return null
            return null;
        }
        public async Task<AppointmentVm> GetAppointments()
        {
            var reportVm = new AppointmentVm
            {
                IEAppointment = await _context.Appointments
                     .Include(a => a.OwnerNav)
                     .Include(a => a.IAppDetails)
                        .ThenInclude(d => d.Services)
                     .Where(a => a.remarks != "Cancelled")
                     .ToListAsync(),
            };
            return reportVm;
        }

        //filtered 
        public async Task<AppointmentVm> GetAllAppointments(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)
        {

            var appointment = await _context.Appointments
                 .Include(a => a.IAppDetails)
                 .ThenInclude(a => a.Services)
                 .Include(a => a.OwnerNav)
                 .Where(d => (!startDate.HasValue || d.date >= startDate)
                        && (!endDate.HasValue || d.date <= endDate)
                        && d.remarks != "Cancelled")
                 .ToListAsync();

             return new AppointmentVm
            {
                IEAppointment = appointment,
                Status = Status,
                SelectType = SelectType,
                Filtered = Filtered,
                startDate = startDate,
                endDate = endDate,
                reportType = reportType,
                activeAppointTab = AppointmentTab.monitoring

            };
       
        }

        public async Task<AppointmentVm> GetCustomAppointments(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)

        {
            if (Status == "inProgress")
            {
                return new AppointmentVm
                {
                    IEAppointment = await _context.Appointments
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
                    reportType = reportType,
                    activeAppointTab = AppointmentTab.monitoring
                };
               
            }
            else
            {
                return new AppointmentVm
                {
                    IEAppointment = await _context.Appointments
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
                    reportType = reportType,
                    activeAppointTab = AppointmentTab.monitoring
                };
             
            }
        }
        #endregion

        #region == Functions for Follow Up == 

        public async Task<AppointmentVm> GetFollowUp()
        {
            var followUp = await _context.PetFollowUps
                .Include(p => p.Diagnostics)
                    .ThenInclude(d => d.pet)
                        .ThenInclude(dp => dp.owner)
                .Include(p => p.Services)
                .Where(p => p.status != "Cancelled")
                .ToListAsync();

            var reportVm = new AppointmentVm
            {
                IPetFollowUps = followUp
            };

            return reportVm;
        }


        public async Task<AppointmentVm> GetAllFollowUp(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)
        {
            var reportVm = new AppointmentVm
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
                reportType = reportType,
                activeAppointTab = AppointmentTab.monitoring
            };
            return reportVm;
        }

        public async Task<AppointmentVm> GetCustomFollowUp(string? reportType, DateTime? startDate, DateTime? endDate, string? Status, string? SelectType, bool Filtered)

        {
            if (Status == "inProgress")
            {
                var reportVm = new AppointmentVm
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
                    reportType = reportType,
                    activeAppointTab = AppointmentTab.monitoring
                };
                return reportVm;
            }
            else
            {
                var reportVm = new AppointmentVm
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
                    reportType = reportType,
                    activeAppointTab = AppointmentTab.monitoring
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
        }

        public async Task<ReportsVm> GetAllcategoryLowProducts(string SelectType, string Status, bool filtered)
        {
            await GetAllCategory();
            var vm = new ReportsVm
            {
                IProducts = await _context.Products
                .Where(p => p.quantity <= 10)
                .Include(p => p.category)
                .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = filtered
            };
            return vm;
        }
        public async Task<ReportsVm> GetAllcategoryHighProducts(string SelectType, string Status, bool filtered)
        {
            await GetAllCategory();
            var vm = new ReportsVm
            {
                IProducts = await _context.Products
                .Where(p => p.quantity >= 11)
                .Include(p => p.category)
                .ToListAsync(),
                Status = Status,
                SelectType = SelectType,
                Filtered = filtered
            };
            return vm;
        }


        //end of stock levels
        #endregion
    }
}
