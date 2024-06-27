using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.Controllers.Records
{
    public class CategoriesController : Controller
    { 
        //dependency Injection
        private readonly PAWS_NDV_PetLoversContext _context;

        public CategoriesController(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }

        //Get
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,categoryName,description,registeredDate")] Category category)
        {

            if (category != null)
            {
                if (CategoryExist(category.categoryName))
                {
                    ModelState.AddModelError("", "Category already exist");
                    return View(category);

                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Successfully Created!";
                return RedirectToAction(nameof(Index));
            }

            return View("Create",category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            //find the category information
            var category = await _context.Categories.FindAsync(id);

            if(category == null)
            {
                return NotFound();
            }

            //if exist
            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("id,categoryName,description,registeredDate,lastUpdate")]Category category)
        {
            //id check
            if(id != category.id)
            {
                return NotFound();
            }

            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Successfully Updated";
            }
            catch(DbUpdateConcurrencyException)
            {
                //verify to the database
                if (!CategoryExist(category.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View(category);

        }
        //check if Id exist

        private bool CategoryExist(int id)
        {
            return _context.Categories.Any(c => c.id == id);
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {

            if(id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.id == id);
            
            if(category == null)
            {
                return NotFound();
            }

            return View(category);


        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CategoryExist(string categoryName)
        {
            return _context.Categories.Any(c => c.categoryName == categoryName);

        }

    }

}
