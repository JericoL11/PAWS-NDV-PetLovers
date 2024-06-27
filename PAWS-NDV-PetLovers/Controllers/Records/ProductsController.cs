using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;

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
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult Create() 
        {
            return View();
        }
    }
}
