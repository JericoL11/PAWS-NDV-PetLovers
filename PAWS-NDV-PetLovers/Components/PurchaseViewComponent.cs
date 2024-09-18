using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;

namespace PAWS_NDV_PetLovers.Components
{
    public class PurchaseViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public PurchaseViewComponent(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Purchase = await _context.Purchases
                .Include(p => p.purchaseDetails)
                .ThenInclude(pd => pd.product)
                .ThenInclude(pd => pd.category)
                .ToListAsync();

            return View(Purchase);
        }
    }
}
