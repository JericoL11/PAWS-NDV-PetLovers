using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Components.ProductMgmt
{
    public class StockAdjustmentsViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public StockAdjustmentsViewComponent(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }


        public async Task <IViewComponentResult> InvokeAsync()
        {
            
            return View(await GetAllStockAdjustments());
        }

        //function
        public async Task<ReportsVm> GetAllStockAdjustments()
        {
            return new ReportsVm
            {
                IstockAdjustment = await _context.StockAdjustments.Include(s => s.productsNav).ToListAsync()
            };
        }
    }
}
