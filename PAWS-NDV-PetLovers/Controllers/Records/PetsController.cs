using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;



namespace PAWS_NDV_PetLovers.Controllers.Records
{
    public class PetsController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public PetsController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        // GET: Pets
        public async Task<IActionResult> Index()
        {
            var pAWS_NDV_PetLoversContext = _context.Pets.Include(p => p.owner);
            return View(await pAWS_NDV_PetLoversContext.ToListAsync());
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.owner)
                .FirstOrDefaultAsync(m => m.id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pets/Create
        public IActionResult Create()
        {
            ViewData["ownerId"] = new SelectList(_context.Owners, "id", "address");
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,petName,species,breed,bdate,age,color,gender,registeredDate,ownerId")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ownerId"] = new SelectList(_context.Owners, "id", "address", pet.ownerId);
            return View(pet);
        }

        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            var date = DateTime.Now;
            var today = date.Date;
            //instantiation
            var pets = new Pet
            {
                petName = pet.petName,
                age = pet.age,
                bdate = date,
                color = pet.color,
                ownerId = pet.ownerId,
                species = pet.species,
                gender = pet.gender,
                breed = pet.breed,
                owner = pet.owner,
                registeredDate = today
            };
/*
            ViewData["ownerId"] = new SelectList(_context.Owners, "id", "address", pet.ownerId);*/
            return View(pets);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,petName,species,breed,bdate,age,color,gender,registeredDate,ownerId")] Pet pet)
        {
            if (id != pet.id)
            {
                return NotFound();
            }

          
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
/*            ViewData["ownerId"] = new SelectList(_context.Owners, "id", "id", pet.ownerId);*/
         
        }

        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.owner)
                .FirstOrDefaultAsync(m => m.id == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.id == id);
        }
    }
}
