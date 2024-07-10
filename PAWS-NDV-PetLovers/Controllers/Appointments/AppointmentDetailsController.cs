using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Appointments;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Controllers.Appointments
{
    public class AppointmentDetailsController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public AppointmentDetailsController(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appVm = await GetDetailsNavigationsAsync();

            return View(appVm);
        }

        public async Task<IActionResult> AppointmentHistory()
        {
            var appointDelailsList = _context.AppointmentDetails.Include(a => a.Appointment.Owner).Include(a => a.services).Include(a => a.Pet).Where(a => a.status != null).Select(a => a).ToListAsync();

            var appVm = new AppointmentVm
            {
                IappointmentDetails = await appointDelailsList,

            };

            return View(appVm);
        }

        #region == Functions ==
        public async Task<AppointmentVm> GetCreateAsync()
        {
            // Get appointments
            var registeredOwners = await _context.Appointments.Include(a => a.Owner).ToListAsync();
            var pets = await _context.Pets.ToListAsync();
            var services = await _context.Services.ToListAsync();

            var DeatilsPets = await _context.AppointmentDetails.Include(p => p.Pet).ToListAsync();

            // Object instantiation for VM
            var apps = new AppointmentVm
            {
                IappointmentDetails = DeatilsPets,
                Iappointments = registeredOwners,
                IPets = pets,
                Iservices = services
            };

            return apps;
        }

        public async Task<AppointmentVm> GetDetailsNavigationsAsync()
        {
            var appointDelailsList = _context.AppointmentDetails.Include(a => a.Appointment.Owner).Include(a => a.Pet).Include(a => a.services).Include(a => a.Pet).Where(a => a.status == null).Select(a => a).ToListAsync();



            var appVm = new AppointmentVm
            {
                IappointmentDetails = await appointDelailsList,

            };

            return appVm;
        }


        public async Task GetListAsync()
        {
            var services = await _context.Services.ToListAsync();
            ViewBag.services = new SelectList(services, "serviceId", "serviceName");
        }



        //Used for jquery fetching pets
        [HttpGet]
        public async Task<IActionResult> GetPetsByOwnerId(int ownerId)
        {
            var pets = await _context.Pets.Where(p => p.ownerId == ownerId).ToListAsync();
            return Json(pets.Select(p => new { p.id, p.petName }));
        }


        public async Task<IActionResult> GetOwnerIdByAppointmentId(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Where(a => a.AppointId == appointmentId)
                .Select(a => new { ownerId = a.OwnerId })
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return NotFound();
            }

            return Json(appointment);
        }


        #endregion

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //get selectList
            await GetListAsync();

            //object instantiation
            var appointmentsVm = await GetCreateAsync();

            return View(appointmentsVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointId_details,AppointId,date,time,serviceID,petID")] AppointmentDetails appointmentDetails)
        {
            // Get select list
            await GetListAsync();

            // Verify if the owner exists in the appointment
            var exists = await _context.Appointments.AnyAsync(a => a.AppointId == appointmentDetails.AppointId);
            if (!exists)
            {
                ModelState.AddModelError("AppointId", "The selected appointment does not exist.");
                return View(appointmentDetails);
            }

            // Check if the pet id and owner id are pending
            var details = await _context.AppointmentDetails.AnyAsync(a => a.AppointId == appointmentDetails.AppointId && a.petID == appointmentDetails.petID && a.status == null);
            if (details)
            {

                //get the PET NAME
                var Details = await _context.AppointmentDetails
                                            .Include(a => a.Pet)
                                            .FirstOrDefaultAsync(a => a.AppointId == appointmentDetails.AppointId && a.petID == appointmentDetails.petID && a.status == null);

                if (Details != null)
                {
                    ModelState.AddModelError("", $"Pet Name \"{Details.Pet.petName}\" is on pending.");
                }
                else
                {
                    ModelState.AddModelError("", "The appointment details could not be found.");
                }
                return View(await GetCreateAsync());
            }

            _context.AppointmentDetails.Add(appointmentDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetStatus(int AppointId_details, string statusSelect)
        {
            var appointmentDetails = await _context.AppointmentDetails.FindAsync(AppointId_details);

            if (appointmentDetails == null)
            {
                return NotFound();
            }

            appointmentDetails.status = statusSelect;

            // Update the appointment details in the database
            _context.Update(appointmentDetails);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}