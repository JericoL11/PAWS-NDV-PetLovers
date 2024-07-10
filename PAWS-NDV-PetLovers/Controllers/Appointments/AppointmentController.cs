using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Appointments;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Controllers.Appointments
{
    public class AppointmentController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public AppointmentController(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {

            //including Owner model
            var appointment = await _context.Appointments.Include(p => p.Owner).ToListAsync();

            return View(appointment);
        }

        #region == function == 

        //for ViewModel Required Object
        public async Task<AppointmentVm> GetOwnersAsync()
        {
            // Assigning IOwners for AppointmentVm
            var owners = await _context.Owners.ToListAsync();

          
            var appointmentOwner = await _context.Appointments.Include(a => a.Owner).ToListAsync();


            var appointmentVm = new AppointmentVm
            {
                IOwner = owners,
                Iappointments = appointmentOwner
               
            };

            return appointmentVm;
        }
      

        #endregion

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ownersViewModel = await GetOwnersAsync();

            return View(ownersViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointId", "OwnerId")] Appointment appointment)
        {
     

            if (appointment == null || appointment.OwnerId == 0)
            {
                ModelState.AddModelError("", "Please select a valid owner.");
          

                return View(appointment);
            }
            
            //check owner if its exist in the database
            var checkOwner = _context.Appointments.Any(o => o.OwnerId == appointment.OwnerId);

            if (checkOwner)
            {
                ModelState.AddModelError("", $"The Owner ID \"{appointment.OwnerId}\" was already registered");


                var ownersViewModel = await GetOwnersAsync();

                return View(ownersViewModel);
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
