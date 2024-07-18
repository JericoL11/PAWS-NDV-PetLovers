using Microsoft.AspNetCore.Mvc;

namespace PAWS_NDV_PetLovers.Controllers.Appointments
{
    public class AppointmentsDetails : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
