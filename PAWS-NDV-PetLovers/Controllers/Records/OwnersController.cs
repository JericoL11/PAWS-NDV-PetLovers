using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Controllers.Records
{
    public class OwnersController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public OwnersController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        // GET: Owners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Owners.ToListAsync());
        }

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .FirstOrDefaultAsync(m => m.id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fname,lname,mname,gender,contact,email,address,Pets")] Owner owner)
        {

            var verifyOwner = _context.Owners
                               .Where(m => m.fname == owner.fname && m.lname == owner.lname).FirstOrDefault();


            if (verifyOwner == null)
            {
                if(owner.contact.Length < 11)
                {
                    ModelState.AddModelError("", "Contact number is invalid");
                    return View(owner);
                }
              
                _context.Add(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
              
            }
            else
            {
                ModelState.AddModelError("", "Owner already exist");

            }

            return View("Create",owner);
        }

            // GET: Owners/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {

            //no id 
            if (id == null)
            {
                return NotFound();
            }

            //mathching the Routed Id to owner Id
            var owner = await _context.Owners.FindAsync(id);

            if (owner == null)
            {
                return NotFound();
            }

            //matching ownerId to Pet
            var pet = await _context.Pets
                .Where(P => P.ownerId == id)
                .Select(p => p).ToListAsync();


            //view Model Instantiations for VIEWS
            var data = new ClientVm
            {
                Owner = owner,
                IPets = pet
            };


            return View(data);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,fname,lname,mname,gender,contact,email,address,Pets")] Owner owner)
        {
            if (id != owner.id)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(owner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
              /*  return RedirectToAction(nameof(Index));
            }*/

            // If we reach this point, something went wrong, redisplay the form with the current model
            var pets = await _context.Pets
                        .Where(p => p.ownerId == id)
                        .ToListAsync();

            var data = new ClientVm
            {
                Owner = owner,
                IPets = pets
            };

            return View(data);
        }

        //edit pet button
        [HttpGet]
        public async Task<IActionResult> EditPet(int? id)
        {

            if(id == null)
            {
                return NotFound();
            }

            // Assuming you want to edit a specific pet in the context of a client
            var clientVm = new ClientVm
            {
                Pet = await _context.Pets.FindAsync(id)
            };

            if (clientVm.Pet == null)
            {
                return NotFound();
            }

            return View(clientVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(int? id, [Bind("id","petName","specie","breed","bdate","color","gender")] Pet pet)
        {
            if (id != pet.id)
            {
                return NotFound(id);
            }

            try
            {
                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!PetExists(pet.id))
                {
                    return NotFound();
                }
            }

            //converting into Client View Models

            //vm instantiation
            var viewModel = new ClientVm
            {
                Pet = pet

            };
           
            return View(viewModel);
        }




        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .FirstOrDefaultAsync(m => m.id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //checking if id exist in the db
        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.id == id);
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.id == id);
        }
    }
}
