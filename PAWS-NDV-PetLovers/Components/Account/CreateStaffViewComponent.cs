using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Data;

namespace PAWS_NDV_PetLovers.Components.Account
{
    public class CreateStaffViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public CreateStaffViewComponent(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isNull)
        {

            if (isNull)
            {
                ModelState.AddModelError("", "Please fill-up username or password");
           
            }
            return View();
        }
    }
}
