using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Controllers.Appointments
{
    public class PetFollowUpController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public PetFollowUpController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        //method
        public async Task<AppointmentVm> GetAllRecords()
        {
            var PetFollowUp = await _context.PetFollowUps
            .Where(p => string.IsNullOrEmpty(p.status))
            .Include(f => f.Diagnostics)
                .ThenInclude(d => d.IdiagnosticDetails)
                    .ThenInclude(d => d.Services)
            .Include(f => f.Diagnostics)
                .ThenInclude(d => d.pet)
                    .ThenInclude(p => p.owner)
            .ToListAsync();

            var vm = new AppointmentVm
            {
                IPetFollowUps = PetFollowUp,
                Services = await _context.Services.ToListAsync()
            };

            return vm;
        }

        public async Task<IActionResult> Index()
        {
           return View(await GetAllRecords());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var PetFollowUp = await _context.PetFollowUps.FindAsync(id);

            if(PetFollowUp == null)
            {
                return NotFound();
            }

            //Update database
            PetFollowUp.status = "Cancelled";

            _context.Update(PetFollowUp);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

       public async Task<IActionResult> Attend(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PetFollowUp = await _context.PetFollowUps.FindAsync(id);

            if (PetFollowUp == null)
            {
                return NotFound();
            }

            //Update database
            PetFollowUp.status = "Attended";

            _context.Update(PetFollowUp);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
