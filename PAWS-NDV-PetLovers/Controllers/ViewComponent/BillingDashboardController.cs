using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Controllers.ViewComponent
{
    public class BillingDashboardController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public BillingDashboardController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }
        public IActionResult Index(TransactionsVm tvm)
        {
            if (tvm == null)
            {
                var vm = new TransactionsVm
                {
                    activeTab = Tab.Diagnostics
                };
                
            }
            return View(tvm);
        }


        public IActionResult SwitchToTab(string? tabName)
        {
            var vm = new TransactionsVm();

            switch (tabName)
            {
                case "Diagnostics":
                    vm.activeTab = Tab.Diagnostics;
                    break;

                case "Purchase":
                    vm.activeTab = Tab.Purchase;
                    break;

                case "Billing":
                    vm.activeTab = Tab.Billing;
                    break;
               
                default:
                    vm.activeTab = Tab.Diagnostics;
                    break;
            }

            return RedirectToAction(nameof(Index), vm);
        }
    }
}
