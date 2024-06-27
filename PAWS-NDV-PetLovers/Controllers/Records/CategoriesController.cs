using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.ViewModels;

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
        public async Task<IActionResult> Create([Bind("id,categoryName,description,registeredDate,Products")] Category category)
        {

            /* if (category != null)
             {
                 return NotFound();
             }

             if (CategoryExist(category.categoryName))
             {
                 ModelState.AddModelError("", "Category already exist");
                 return View(category);

             }

             *//*check product*//*
             var product = _context.Products.FirstOrDefault(category.);

             if (ProductExist())
             {
                 ModelState.AddModelError("", "Category already exist");
                 return View(category);

             }

             _context.Add(category);
             await _context.SaveChangesAsync();
             TempData["SuccessMessage"] = "Successfully Created!";
             return RedirectToAction(nameof(Index));
             return View("Create", category);*/
            return View("Create", category);
        }


        private bool CategoryExist(string categoryName)
        {
            return _context.Categories.Any(c => c.categoryName == categoryName);

        }
        private bool ProductExist(string productName)
        {
            return _context.Products.Any(c => c.productName == productName);

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

            //retrieve related products
            var updatedCategory = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.id == id);
            
            if(updatedCategory == null)
            {
                return NotFound();
            }

            var data = new RecordsVm
            {
                Category = updatedCategory,
                IProducts = updatedCategory.Products
            };

          
            return View(data);

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

            category.lastUpdate = DateTime.Now;

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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
            return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);   

            if(category == null)
            {
            return NotFound();
            }

            return View(category);

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



    }

}
