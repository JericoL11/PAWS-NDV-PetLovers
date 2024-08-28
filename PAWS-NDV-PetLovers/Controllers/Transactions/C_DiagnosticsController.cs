using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Models.Transactions;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

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
                Services = await _context.Services.Where(s => string.IsNullOrEmpty(s.status)).ToListAsync()
            };

            return View(tVm);
        }



        [HttpGet]
        public async Task<IActionResult> DiagnosAppointment(int? id, List<int> serviceId)
        {
            var getOwner = await _context.Owners.Include(o => o.Pets)
                .FirstOrDefaultAsync(o => o.id == id);


            TransactionsVm tvm = new TransactionsVm
            {
                Owner = getOwner,
                SelectedServices = serviceId,
                Services = await _context.Services.Where(s => string.IsNullOrEmpty(s.status)).ToListAsync() //status can be updated when service is deleted
            };

            return View(tvm);
        }



        //Owner Diagnosis
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Diagnostics")] TransactionsVm tvm)
        {


            _context.Add(tvm.Diagnostics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DiagnosticBill));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, DateTime date)
        {


            // Extract only the date part (ignoring time)
            DateTime dateOnly = date.Date;
            if (id == null)
            {
                return NotFound();
            }


            //fetched the data in database
            var firstAsync = await _context.Diagnostics
                .Include(d => d.pet)
                .ThenInclude(d => d.owner)
                .Include(d => d.IdiagnosticDetails)
                .ThenInclude(dd => dd.Services)
                .Include(d => d.PurchaseNav)
                .ThenInclude(d => d.purchaseDetails)
                .ThenInclude(d => d.product)
                .ThenInclude(d => d.category)
                .FirstOrDefaultAsync(d => d.diagnostic_Id == id);

            var Purchase = await _context.Purchases
                .Include(p => p.purchaseDetails)
                .ThenInclude(p => p.product)
                .ThenInclude(p => p.category)
                .FirstOrDefaultAsync(p => p.diagnosisId == id && p.date == dateOnly);

            TransactionsVm tvm = new TransactionsVm
            {
                Diagnostics = firstAsync,

                Purchase = Purchase,

                Services = await _context.Services.ToListAsync(),

                IProducts = await _context.Products.Include(p => p.category).ToListAsync()

            };

            return View(tvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Diagnostics")] TransactionsVm tvm)
        {
            double totalPurchase = 0;

            // Extract the Diagnostics object from the view model
            var diagnostics = tvm.Diagnostics;

            // Referenced from view
            foreach (var detail in diagnostics.IdiagnosticDetails)
            {
                // FindAsync the DetailID
                var existingDetail = await _context.DiagnosticDetails.FindAsync(detail.diagnosticDet_Id);

                // Update the diagnosis remarks
                if (existingDetail != null)
                {
                    existingDetail.details = detail.details;
                }
            }

            // List to store individual product totals
            var Purchased = new List<double>();

            // Adding selected products
            if (diagnostics.PurchaseNav.purchaseDetails.Count > 0)
            {
                foreach (var purchaseDetail in diagnostics.PurchaseNav.purchaseDetails)
                {
                    // Get the price and quantity
                    var price = purchaseDetail.sellingPrice;
                    var quantity = purchaseDetail.quantity;

                    // Calculate the total for this product
                    // the condition quantity > 0 is false, so 1 is used instead.
                    double productTotal = price * (quantity > 0 ? quantity : 1);

                    // Add the total to the Purchased list
                    Purchased.Add(productTotal);
                }

                // Calculate the total purchase payment
                diagnostics.PurchaseNav.totalProductPayment = Purchased.Sum();

                // Optionally: add the updated PurchaseNav to the context if needed
                _context.Add(diagnostics.PurchaseNav);
            }

            // Attempt to save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
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


        public async Task<IActionResult> DiagnosticBill()
        {
            // Load diagnostics
            var diagnostics = await _context.Diagnostics
                .Include(d => d.pet)
                .ThenInclude(d => d.owner)
                .Include(d => d.IdiagnosticDetails)
                .ThenInclude(d => d.Services)
                .Where(d => string.IsNullOrEmpty(d.status)).ToListAsync();

            // Load purchases
            var purchases = await _context.Purchases.ToListAsync();

            // Manually link the purchases to diagnostics
            foreach (var diag in diagnostics)
            {
                var relatedPurchase = purchases.FirstOrDefault(p => p.diagnosisId == diag.diagnostic_Id);
                if (relatedPurchase != null)
                {
                    diag.PurchaseNav = relatedPurchase; // Manually associate the purchase with the diagnostic
                }
            }

            // Create the view model
            var tvm = new TransactionsVm
            {
                IDiagnostics = diagnostics,
                IPurchase = purchases,
            };

            return View(tvm);
        }

        [HttpPost]
        public async Task<IActionResult> SetRemarks(int diagnosticId)
        {
            var diagnostic = await _context.Diagnostics.FindAsync(diagnosticId);

            if (diagnostic == null)
            {
                // Handle the case where the diagnostic is not found
                return NotFound();
            }

            var purchases = await _context.Purchases.Where(p => p.diagnosisId == diagnosticId).ToListAsync();

            var successful = "Completed";
            var purchasePayment = "Paid";

            // Update diagnostic status
            diagnostic.status = successful;
            _context.Update(diagnostic);

            // Update purchase status
            if (purchases != null && purchases.Any())
            {
                foreach (var purchase in purchases)
                {
                    purchase.status = purchasePayment;
                }

                _context.UpdateRange(purchases);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(DiagnosticBill));
        }

        [HttpGet]
        public async Task<IActionResult>PurchaseDetailsView(int? id)
        {
            var PurchaseItems = await _context.PurchaseDetails
                .Include(p => p.product)
                .ThenInclude(p => p.category)
                .Include(p => p.Purchase)
                .Where(p => p.Purchase.diagnosisId == id && string.IsNullOrEmpty(p.Purchase.status))
                .ToListAsync();



            // Calculate total price
            double grandTotal = PurchaseItems.Sum(item => (double)item.product.sellingPrice * (int)item.quantity);

            TransactionsVm tvm = new TransactionsVm
            {
                IPurchaseDetails = PurchaseItems,
                totalPurchasePayment = grandTotal
            };  
            return View(tvm);
        }
    }
}