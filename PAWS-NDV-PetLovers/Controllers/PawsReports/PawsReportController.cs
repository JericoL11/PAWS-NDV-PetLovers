using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Data;

namespace PAWS_NDV_PetLovers.Controllers.PawsReports
{
    public class PawsReportController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public PawsReportController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }
        public IActionResult AppointmentsReport()
        {

            return View();
        }

        public IActionResult ProductMngmtReport()
        {
            return View();
        }
    }
}
