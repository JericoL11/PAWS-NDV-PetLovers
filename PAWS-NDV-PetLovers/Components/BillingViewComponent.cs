using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Data;

namespace PAWS_NDV_PetLovers.Components
{
    public class BillingViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public BillingViewComponent(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }


        public async Task <IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
