using System;
using System.Collections.Generic;
using System.Drawing;
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


        //pet area
        #region == Pets code area || EDIT > ADD NEW PET == 

        public async Task<IActionResult> addNewPet([Bind("id,petName,species,breed,bdate,age,color,gender,registeredDate,ownerId")] Pet pet)
        {
            /*
                        var checkPet = _context.Pets.FirstOrDefault(e => e.petName == pet.petName &&
                                        e.bdate == pet.bdate && e.age == pet.age && e.breed == pet.breed && e.ownerId == pet.ownerId);*/


            if(pet == null)
            {
                return View(pet);
            }

            if (PetExists(pet.petName, pet.species ,pet.breed, pet.ownerId))
            {
                ModelState.AddModelError("", "Pet exist Already");

                //for jquery code
                return Json(new { success = false, message = "Pet Exist Already" });
            }

            if (pet != null)
            {
                //for modal

                _context.Add(pet);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Pet added successfully!";
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid data" });


        }


        private bool PetExists(string petName, string specie,string breed, int ownerId)
        {
            return _context.Pets.Any(e => e.petName == petName && e.species == specie  && e.breed == breed &&
           e.ownerId == ownerId);
            /* return _context.Pets.Any(e => e.id == pet.id && e.petName == pet.petName &&
             e.bdate == pet.bdate && e.age == pet.age && e.breed == pet.breed && e.ownerId == pet.ownerId);*/
        }
        private bool PetExistsById(int id)
        {
            return _context.Pets.Any(e => e.id == id);
        }

        #endregion


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

            var date = DateTime.Now;
            var today = date.Date;
           

            var owner = new Owner
            {
                registeredDate = today
            };

            return View(owner);
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fname,lname,mname,gender,contact,email,address,registeredDate,Pets")] Owner owner)
        {
            var verifyOwner = _context.Owners
                               .Where(m => m.fname == owner.fname && m.lname == owner.lname)
                               .FirstOrDefault();

            if (verifyOwner == null)
            {
                if (owner.contact.Length < 11)
                {
                    ModelState.AddModelError("", "Contact number is invalid");
                    return View(owner);
                }

                // Use a HashSet to track pet names and check for duplicates
                var petNames = new HashSet<string>();

                foreach (var pet in owner.Pets)
                {
                    if (!petNames.Add(pet.petName))
                    {
                        ModelState.AddModelError("", $"Duplicate pet name '{pet.petName}' is invalid");
                        return View(owner);
                    }
                }

                _context.Add(owner);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Created Successfully";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Owner already exists");
            }

            return View("Create", owner);
        }


        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }

            // Retrieve related pets
            var updatedOwner = await _context.Owners
                                    .Include(o => o.Pets)
                                    .FirstOrDefaultAsync(o => o.id == id);



            //view Model Instantiations for VIEWS
            var data = new RecordsVm
            {
                Owner = updatedOwner,
                IPets = updatedOwner.Pets
            };


            return View(data);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,fname,lname,mname,gender,contact,email,address,registeredDate,lastUpdate,Pets")] Owner owner)
        {
            if (id != owner.id)
            {
                return NotFound();
            }

            if(string.IsNullOrEmpty(owner.contact) || owner.contact.Length < 11)
            {
                ModelState.AddModelError("","Please input a contact number correctly");

                // Retrieve the updated data to redisplay the form
                var owners = await _context.Owners
                                        .Include(o => o.Pets)
                                        .FirstOrDefaultAsync(o => o.id == id);

                var contactCheck = new RecordsVm
                {
                    Owner = owners,
                    IPets = owners.Pets
                };
                return View(contactCheck);
            }

            // Update the lastUpdate column
            owner.lastUpdate = DateTime.Now;

            try
            {
                _context.Update(owner);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Update Successfully";
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

            // Retrieve the updated data to redisplay the form
            var updatedOwner = await _context.Owners
                                    .Include(o => o.Pets)
                                    .FirstOrDefaultAsync(o => o.id == id);

            var data = new RecordsVm
            {
                Owner = updatedOwner,
                IPets = updatedOwner.Pets
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
            var clientVm = new RecordsVm
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
        public async Task<IActionResult> EditPet(int? id, [Bind("id","petName","specie","breed","bdate","color",
            "registeredDate","gender")] Pet pet)
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
                if (!PetExistsById(pet.id))
                {
                    return NotFound();
                }
            }

            //converting into Client View Models

            //vm instantiation
            var viewModel = new RecordsVm
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

    }
}

