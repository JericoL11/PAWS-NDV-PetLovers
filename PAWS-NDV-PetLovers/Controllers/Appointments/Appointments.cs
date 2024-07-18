using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Appointments;
using PAWS_NDV_PetLovers.ViewModels;
using System.Runtime.CompilerServices;

namespace PAWS_NDV_PetLovers.Controllers.Appointments
{
    public class Appointments : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public Appointments(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            var appDetails = await GetAllAsync();

            return View(appDetails);
        }

        #region == Functions == 
           public async Task<AppointmentVm> GetAllAsync()
            {

            // Fetch appointment details from the database, including related Services and Appointment entities.
            var appDetails = await _context.AppointmentDetails
                .Include(a => a.Services)    
                .Include(a => a.Appointment)  
                .ToListAsync();           

            // Group the fetched appointment details by AppointmentId to ensure each appointment is processed only once.
            var groupedAppointments = appDetails
                .GroupBy(ad => ad.Appointment.AppointId)   // Group by the AppointmentId property
                .Select(g => new AppointmentGroup          // Project each group into a new AppointmentGroup object
                {
                    Appointment = g.First().Appointment,   // Use the first AppointmentDetail in the group to get the Appointment
                    Details = g.ToList()                  
                })
                .ToList();                                


            var vm = new AppointmentVm
            {
                AppointmentGrouping = groupedAppointments  
            };

            return vm;

        }


        public async Task GetServices()
        {
            var services = await _context.Services.ToListAsync();

            ViewBag.Services = new SelectList(services, "serviceId", "serviceName");
        }

        #endregion

        public async Task<IActionResult> Create()
        {
            // Populate ViewBag.Services or other necessary data
            await GetServices();
            return View();
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("AppointId,fname,mname,lname,contact,date,time,Owner,IAppDetails")]Appointment appointments)
         {
             await GetServices();


            #region === string hashing == 
            /*
                        var serviceHash = new HashSet<string>();

                        foreach(var service in appointments.IAppDetails)
                        {
                            if (!serviceHash.Add(service.Services.serviceName))
                            {
                                ModelState.AddModelError("", "Duplicate Appointments is not valid");
                                return View(appointments);
                            }
                        }
            */
            #endregion

            if (ModelState.IsValid)
             {
                 _context.Add(appointments);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }

             return View();
         }
    }
}

