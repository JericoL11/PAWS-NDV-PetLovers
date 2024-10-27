using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Components.PawsReports
{
    public class TransacSummaryViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;
        public TransacSummaryViewComponent(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(ReportsVm vcm)
        {
            if(vcm.logical_dateError == true)
            {
                ModelState.AddModelError("", "The end date cannot be earlier than the start date.");
                return View(vcm);
            }

            //ELSE

            var billing = await _context.Billings
               .Where(d => (!vcm.startDate.HasValue || d.date >= vcm.startDate)
                        && (!vcm.endDate.HasValue || d.date <= vcm.endDate))
               .Include(b => b.purchase)
               .Include(b => b.diagnostics)
               .ThenInclude(d => d.pet)
               .ThenInclude(p => p.owner)
               .ToListAsync(); 

            return View(new ReportsVm
            {
                IBilling = billing,
                startDate = vcm.startDate,
                endDate = vcm.endDate,
                Filtered = vcm.Filtered,
                activeReportTab = ReportTab.transacSum
            });

        }
    }
}
