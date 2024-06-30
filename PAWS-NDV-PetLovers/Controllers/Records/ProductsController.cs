using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Records;
using System.Drawing;

namespace PAWS_NDV_PetLovers.Controllers.Records
{
    public class ProductsController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public ProductsController(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {

            //including category navigation to display each name
            var categoryInclude = _context.Products.Include(p => p.category);

            return View(await categoryInclude.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            //to fill the selectList for categoryId
            await SetCategoryListAsync();
         
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,productName,supplierPrice,sellingPrice,quantity,updateDate,expiryDate,CategoryId")] Product product)
        {
            if (product == null)
            {
                // Handle the null product scenario
                ModelState.AddModelError("", "Product cannot be null.");
                await SetCategoryListAsync();
                return View(product);
            }


            // if inputs are null, this is the solution
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Created Successfully";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (ProductExistByName(product.productName))
                {
                    await SetCategoryListAsync();
                    ModelState.AddModelError("", $"Product name '{product.productName}' already exists.");

                    await SetCategoryListAsync();
                    return View(product);
                }
            }
            await SetCategoryListAsync();
            return View(product);
      
        }
    

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            await SetCategoryListAsync();

            if (id == null)
            {
                return NotFound();
            }

            var editProduct = await _context.Products.FindAsync(id);

            if(editProduct == null)
            {
                return NotFound();
            }

            return View(editProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,productName,supplierPrice,sellingPrice,quantity,updateDate,lastUpdate,expiryDate,CategoryId")]Product product)
        {
            await SetCategoryListAsync();


            if(id != product.id)
            {
                return NotFound();
            }

            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Updated Successfully";
              

                return View(product);
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!ProductExist(product.id)) 
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        #region == funtions ==
        private async Task SetCategoryListAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryList = new SelectList(categories, "id", "categoryName");
        }
        private bool ProductExistByName(string? productName)
        {
            return _context.Products.Any(p => p.productName == productName);
        }

        private bool ProductExist(int id)
        {
            return _context.Products.Any(p => p.id == id);
        }
        #endregion
    }

}
