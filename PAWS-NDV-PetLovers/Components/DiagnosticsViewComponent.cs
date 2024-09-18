using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Component
{
    public class DiagnosticsViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public DiagnosticsViewComponent(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var diagnostics = await _context.Diagnostics
                            .Include(d => d.Purchase)
                            .ThenInclude(p => p.purchaseDetails)
                            .ThenInclude(p => p.product)
                            .Include(d => d.pet)
                            .ThenInclude(p => p.owner)
                            .Include(d => d.IdiagnosticDetails)
                            .ThenInclude(dd => dd.Services)
                            .Where(d => string.IsNullOrEmpty(d.status))
                            .ToListAsync();
            // Optionally, fetch all purchases if needed for other purposes
            var purchases = await _context.Purchases.Include(p => p.purchaseDetails).ThenInclude(p => p.product ).ToListAsync();
            var vm = new TransactionsVm
            {
                IDiagnostics = diagnostics,
                IPurchase = purchases
            };

            return View(vm);
        }
    }
}
