using Microsoft.AspNetCore.Mvc;

namespace PAWS_NDV_PetLovers.Components.Dashboards
{
    public class DBoard_PurchaseViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
