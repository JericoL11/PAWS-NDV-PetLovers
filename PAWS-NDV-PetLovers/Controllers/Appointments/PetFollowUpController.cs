using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;

namespace PAWS_NDV_PetLovers.Controllers.Appointments
{
    public class PetFollowUpController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public PetFollowUpController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var PetFollowUp = await _context.PetFollowUps
             .Include(f => f.Diagnostics) 
                 .ThenInclude(d => d.IdiagnosticDetails) 
                     .ThenInclude(d => d.Services) 
             .Include(f => f.Diagnostics)
                 .ThenInclude(d => d.pet) 
                     .ThenInclude(p => p.owner) 
             .ToListAsync();

            return View(PetFollowUp);
        }
    }
}
