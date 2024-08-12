using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Models.Transactions;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.Controllers.Transactions
{

    public class C_DiagnosticsController : Controller
    {
        private readonly PAWS_NDV_PetLoversContext _context;

        public C_DiagnosticsController(PAWS_NDV_PetLoversContext context)
        {
            this._context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {

            //id handling if null
            if (id == null)
            {
                return NotFound();
            }

            var firstPet = await _context.Pets.Include(p => p.owner).FirstOrDefaultAsync(p => p.id == id);

            if (firstPet == null)
            {
                return NotFound();
            }

            //vm instantiationn
            TransactionsVm tVm = new TransactionsVm
            {
                Pets = firstPet,
                //assigning fk column PetId based on routed id
                Diagnostics = new Diagnostics { petId = (int)id },
                Services = await _context.Services.ToListAsync()
            };

            return View(tVm);
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("Diagnostics")] TransactionsVm tvm)
        {
            _context.Add(tvm.Diagnostics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DiagnosticBill));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firstAsync = await _context.Diagnostics
                .Include(d => d.pet)
                .ThenInclude(d => d.owner)
                .Include(d => d.IdiagnosticDetails)
                .ThenInclude(dd => dd.Services)
                .FirstOrDefaultAsync(d => d.diagnostic_Id == id);

            TransactionsVm tvm = new TransactionsVm
            {
                Diagnostics = firstAsync,
                Services = await _context.Services.ToListAsync()

            };

            return View(tvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Diagnostics")] TransactionsVm tvm)
        {
            // Extract the Diagnostics object from the view model
            var diagnostics = tvm.Diagnostics;

            //referenced from view
            foreach (var detail in diagnostics.IdiagnosticDetails)
            {
                //FindAsync the DetailID
                //updator,when matched inside the if condition
                var existingDetail = await _context.DiagnosticDetails.FindAsync(detail.diagnosticDet_Id);


                if (existingDetail != null)
                {
                    //update the diagnosis remarks
                    existingDetail.diagnosis = detail.diagnosis;

                }
            }

            // Attempt to save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //check if Details Id exist
                if (!DiagnosticExists(diagnostics.diagnostic_Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(DiagnosticBill)); // Redirect to a different action after saving
        }

        #region == Functions == 
        private bool DiagnosticExists(int id)
        {
            return _context.Diagnostics.Any(e => e.diagnostic_Id == id);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> DiagnosAppointment()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> DiagnosticBill()
        {
            var diagnostics = await _context.Diagnostics
                .Include(d => d.pet)
                .ThenInclude(d => d.owner)
                .Include(d => d.IdiagnosticDetails)
                .ThenInclude(d => d.Services)
            .Where(d => string.IsNullOrEmpty(d.remarks)).ToListAsync();

            return View(diagnostics);
        }

        [HttpPost]
        public async Task<IActionResult> SetRemarks(int diagnosticId)
        {
            var FindAsync = await _context.Diagnostics.FindAsync(diagnosticId);

            var successful = "Completed";

            //update remarks
            if (FindAsync != null)
            {
                FindAsync.remarks = successful;
                _context.Update(FindAsync);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(DiagnosticBill));
        }
    }
}
