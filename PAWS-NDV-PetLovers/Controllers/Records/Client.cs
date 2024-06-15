using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Controllers.Records
{
    public class Client : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public Client(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }

        [HttpGet]

        //uses async to avoid thread problem
        public async Task<IActionResult> Index()
        {
            return View(await _context.Owners.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientVm clientVm)
        {
            //save to db for OWNER
            _context.Owners.Add(clientVm.Owner);
            await _context.SaveChangesAsync();

            //Assign to created owner ID
            clientVm.Pet.ownerId = clientVm.Owner.id;

            //save pet
            _context.Pets.Add(clientVm.Pet);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}


//proceed nakas crud, edit,delete, UG DISPLAY SA UPDATE NGA DAPAT NAA ANG LIST SA IYANG PETS UG MAKA UPDATE