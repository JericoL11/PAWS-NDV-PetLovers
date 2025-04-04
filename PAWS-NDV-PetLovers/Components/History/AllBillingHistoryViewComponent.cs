﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Components.History
{
    public class AllBillingHistoryViewComponent:ViewComponent
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public AllBillingHistoryViewComponent( PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var Ibillings = await _context.Billings
               .Include(b => b.diagnostics)
               .ThenInclude(d => d.IdiagnosticDetails)
               .ThenInclude(dd => dd.Services)
               .Include(b => b.diagnostics.pet.owner)
               .Include(b => b.purchase)
               .ThenInclude(p => p.purchaseDetails)
               .ThenInclude(p => p.product)
               .ToListAsync();


            var tvm = new TransactionsVm
            {
                IBilling = Ibillings
            };
            return View(tvm);
    
        }
    }
}
