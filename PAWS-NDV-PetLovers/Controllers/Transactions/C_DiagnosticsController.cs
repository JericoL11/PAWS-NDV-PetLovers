using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Models.Transactions;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Diagnostics")] TransactionsVm tvm)
        {

            //update granda
        /*    if(tvm.Diagnostics != null)
            {
                tvm.Diagnostics.grandTotal = (double)tvm.Diagnostics.totalServicePayment;
            }
     */
            _context.Add(tvm.Diagnostics);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Billing");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            // Extract only the date part (ignoring time)
      
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
                .Include(d => d.Purchase)
                .ThenInclude(d => d.purchaseDetails)
                .ThenInclude(d => d.product)
                .ThenInclude(d => d.category)
                .FirstOrDefaultAsync(d => d.diagnostic_Id == id);

            var Purchase = await _context.Purchases
                .Include(p => p.purchaseDetails)
                .ThenInclude(p => p.product)
                .ThenInclude(p => p.category)
                .FirstOrDefaultAsync(p => p.diagnosisId_holder == id);

            //VIEW Cart

            var PurchaseItems = await _context.PurchaseDetails
                .Include(p => p.product)
                .ThenInclude(p => p.category)
                .Include(p => p.Purchase)
                .Where(p => p.Purchase.diagnosisId_holder == id && string.IsNullOrEmpty(p.Purchase.status))
                .ToListAsync();

            // Calculate total price
            double purchasePayment = PurchaseItems.Sum(item => (double)item.product.sellingPrice * (int)item.quantity);

           

            TransactionsVm tvm = new TransactionsVm
            {
                Diagnostics = firstAsync,

                Purchase = Purchase,

                Services = await _context.Services.ToListAsync(),

                IProducts = await _context.Products.Include(p => p.category).ToListAsync(),

                IPurchaseDetails = PurchaseItems,
                totalPurchasePayment = purchasePayment

            };

            return View(tvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("Diagnostics")] TransactionsVm tvm)
        {
            double totalPurchase = 0;

            // Extract the Diagnostics object from the view model
            var diagnostics = tvm.Diagnostics;
  

            // Referenced from view for Update detail Results
            foreach (var detail in diagnostics.IdiagnosticDetails)
            {
                // FindAsync the DetailID
                var existingDetail = await _context.DiagnosticDetails.FindAsync(detail.diagnosticDet_Id);

                // Update the diagnosis details
                if (existingDetail != null)
                {
                    existingDetail.details = detail.details;
                    
                }
            }

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


            //for Diagnostic Bill Display

            var diagnostic = await _context.Diagnostics
               .Include(d => d.Purchase)
                   .ThenInclude(p => p.purchaseDetails)
                   .ThenInclude(p => p.product)
               .Include(d => d.pet)
                   .ThenInclude(p => p.owner)
               .Include(d => d.IdiagnosticDetails)
                   .ThenInclude(dd => dd.Services)
               .Where(d => string.IsNullOrEmpty(d.status))
               .ToListAsync();

            var purchase = await _context.Purchases
                .Include(p => p.purchaseDetails).ToListAsync();

            TransactionsVm vm = new TransactionsVm
            {
                IDiagnostics = diagnostic,
                IPurchase = purchase
            };

            return RedirectToAction("Index" , "BillingDashboard"); // Redirect to a different action after saving
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchase(Purchase purchase)
        {
            // Check if the purchase object or purchase details are null or empty
            if (purchase == null || purchase.purchaseDetails == null || !purchase.purchaseDetails.Any())
            {
                // Fetch the original data for the view
                var diagnostics = await _context.Diagnostics
                    .Include(d => d.pet)
                    .ThenInclude(p => p.owner)
                    .Include(d => d.IdiagnosticDetails)
                    .ThenInclude(dd => dd.Services)
                    .Include(d => d.Purchase)
                    .ThenInclude(d => d.purchaseDetails)
                    .ThenInclude(pd => pd.product)
                    .ThenInclude(p => p.category)
                    .FirstOrDefaultAsync(d => d.diagnostic_Id == purchase.diagnosisId_holder);

                var purchaseItems = await _context.PurchaseDetails
                    .Include(p => p.product)
                    .ThenInclude(p => p.category)
                    .Include(p => p.Purchase)
                    .Where(p => p.Purchase.diagnosisId_holder == purchase.diagnosisId_holder && string.IsNullOrEmpty(p.Purchase.status))
                    .ToListAsync();

                // Calculate the total price again
                double purchasePayment = purchaseItems.Sum(item => (double)item.product.sellingPrice * (int)item.quantity);

                // Reload the view model with the original data and an error message
                var viewModel = new TransactionsVm
                {
                    Diagnostics = diagnostics,
                    Purchase = await _context.Purchases
                        .Include(p => p.purchaseDetails)
                        .ThenInclude(p => p.product)
                        .ThenInclude(p => p.category)
                        .FirstOrDefaultAsync(p => p.diagnosisId_holder == purchase.diagnosisId_holder),
                    Services = await _context.Services.ToListAsync(),
                    IProducts = await _context.Products.Include(p => p.category).ToListAsync(),
                    IPurchaseDetails = purchaseItems,
                    totalPurchasePayment = purchasePayment
                 
                };

                // Add an error message to the ViewData to display on the view
                ViewData["ErrorMessage"] = "No products were selected. Please select at least one product to proceed.";

                // Return the Edit view with the original data and error message
                return View("Edit", viewModel);
            }

            // Storage for calculated totals
            var Purchased = new List<double>();

            // Selected Product Ids
            var ProductIds = purchase.purchaseDetails
                .Select(P => P.productId)
                .Distinct()
                .ToList();

            // Get products from the database based on selected product Ids
            var Products = await _context.Products.Where(p => ProductIds.Contains(p.id)).ToListAsync();

            // Convert the list of products to a dictionary for easy lookup
            var ProductDictionary = Products.ToDictionary(p => p.id);

            // Process each purchase detail and update total price
            foreach (var details in purchase.purchaseDetails)
            {
                var price = details.sellingPrice;
                var quantity = details.quantity;

                // Calculate total price (if quantity is 0, it defaults to 1)
                var total = price * (quantity > 0 ? quantity : 1);

                // Add total to the list of totals
                Purchased.Add(total);

                // Update product quantity in the database
                if (ProductDictionary.TryGetValue(details.productId, out var product))
                {
                    product.quantity -= quantity;
                }
            }

            // Sum all the product totals and update the purchase
            /*purchase.totalProductPayment = Purchased.Sum();*/

            // Add the purchase and update the products in the database
            _context.Add(purchase);
            _context.UpdateRange(Products);

            // Attempt to save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(purchase.diagnosisId_holder))
                {
                    // Fetch the original data for the view
                    var diagnostics = await _context.Diagnostics
                        .Include(d => d.pet)
                        .ThenInclude(p => p.owner)
                        .Include(d => d.IdiagnosticDetails)
                        .ThenInclude(dd => dd.Services)
                        .Include(d => d.Purchase)
                        .ThenInclude(d => d.purchaseDetails)
                        .ThenInclude(pd => pd.product)
                        .ThenInclude(p => p.category)
                        .FirstOrDefaultAsync(d => d.diagnostic_Id == purchase.diagnosisId_holder);

                    var purchaseItems = await _context.PurchaseDetails
                        .Include(p => p.product)
                        .ThenInclude(p => p.category)
                        .Include(p => p.Purchase)
                        .Where(p => p.Purchase.diagnosisId_holder == purchase.diagnosisId_holder && string.IsNullOrEmpty(p.Purchase.status))
                        .ToListAsync();

                    // Calculate the total price again
                    double purchasePayment = purchaseItems.Sum(item => (double)item.product.sellingPrice * (int)item.quantity);

                    // Reload the view model with the original data
                    var viewModel = new TransactionsVm
                    {
                        Diagnostics = diagnostics,
                        Purchase = await _context.Purchases
                            .Include(p => p.purchaseDetails)
                            .ThenInclude(p => p.product)
                            .ThenInclude(p => p.category)
                            .FirstOrDefaultAsync(p => p.diagnosisId_holder == purchase.diagnosisId_holder),
                        Services = await _context.Services.ToListAsync(),
                        IProducts = await _context.Products.Include(p => p.category).ToListAsync(),
                        IPurchaseDetails = purchaseItems,
                        totalPurchasePayment = purchasePayment
                    };

                    ViewData["ErrorMessage"] = "The purchase does not exist.";
                    return View("Edit", viewModel);
                }
                else
                {
                    throw;
                }
            }

            // Fetch the updated diagnostic data and purchase details for the Edit view
            var updatedDiagnostics = await _context.Diagnostics
                .Include(d => d.pet)
                .ThenInclude(p => p.owner)
                .Include(d => d.IdiagnosticDetails)
                .ThenInclude(dd => dd.Services)
                .Include(d => d.Purchase)
                .ThenInclude(d => d.purchaseDetails)
                .ThenInclude(pd => pd.product)
                .ThenInclude(p => p.category)
                .FirstOrDefaultAsync(d => d.diagnostic_Id == purchase.diagnosisId_holder);

            var updatedPurchaseItems = await _context.PurchaseDetails
                .Include(p => p.product)
                .ThenInclude(p => p.category)
                .Include(p => p.Purchase)
                .Where(p => p.Purchase.diagnosisId_holder == purchase.diagnosisId_holder && string.IsNullOrEmpty(p.Purchase.status))
                .ToListAsync();

            // Calculate the total price again
            double updatedPurchasePayment = updatedPurchaseItems.Sum(item => (double)item.product.sellingPrice * (int)item.quantity);

            // Reload the view model with the updated data
            var updatedTvm = new TransactionsVm
            {
                Diagnostics = updatedDiagnostics,
                Purchase = await _context.Purchases
                    .Include(p => p.purchaseDetails)
                    .ThenInclude(p => p.product)
                    .ThenInclude(p => p.category)
                    .FirstOrDefaultAsync(p => p.diagnosisId_holder == purchase.diagnosisId_holder),
                Services = await _context.Services.ToListAsync(),
                IProducts = await _context.Products.Include(p => p.category).ToListAsync(),
                IPurchaseDetails = updatedPurchaseItems,
                totalPurchasePayment = updatedPurchasePayment
            };

            // Return the updated Edit view with the current data
            return View("Edit", updatedTvm);
        }



        #region == Functions == 
        private bool DiagnosticExists(int id)
        {
            return _context.Diagnostics.Any(e => e.diagnostic_Id == id);
        }

        private bool PurchaseExists(int? id)
        {
            return _context.Purchases.Any(p => p.purchaseId == id);
        }
        #endregion

        /*public async Task<IActionResult> Billing()
        {
            // Load diagnostics including related entities
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
            var purchases = await _context.Purchases.Include(p => p.purchaseDetails).ToListAsync();
            
            

            var tvm = new TransactionsVm
            {
                IDiagnostics = diagnostics,
                IPurchase = purchases,
            };

            return View(tvm);
        }
*/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetRemarks(int diagnosticId, double? grandtotal)
        {
            var diagnostic = await _context.Diagnostics.FindAsync(diagnosticId);

            if (diagnostic == null)
            {
                return NotFound();
            }

            var purchases = await _context.Purchases
                .Include(p => p.purchaseDetails) // Include purchase details
                .ThenInclude(pd => pd.product) // Include product
                .Where(p => p.diagnosisId_holder == diagnosticId)
                .ToListAsync();

            var successful = "Completed";
            var purchasePaymentSuccess = "Paid";

            // Update diagnostic status
            diagnostic.status = successful;
            /*diagnostic.grandTotal = (double)grandtotal;*/
            _context.Update(diagnostic);

            // Update purchase status
            if (purchases != null && purchases.Any())
            {
                foreach (var purchase in purchases)
                {
                    purchase.status = purchasePaymentSuccess;
                }
           
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Billing");
        }


        [HttpGet]
        public async Task<IActionResult>PurchaseDetailsView(int? id)
        {
            var PurchaseItems = await _context.PurchaseDetails
                .Include(p => p.product)
                .ThenInclude(p => p.category)
                .Include(p => p.Purchase)
                .Where(p => p.Purchase.diagnosisId_holder == id && string.IsNullOrEmpty(p.Purchase.status))
                .ToListAsync();



            // Calculate total price
            double purchasePayment = PurchaseItems.Sum(item => (double)item.product.sellingPrice * (int)item.quantity);

            TransactionsVm tvm = new TransactionsVm
            {
                IPurchaseDetails = PurchaseItems,
                totalPurchasePayment = purchasePayment
            };  
            return View(tvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProductFromPurchase(int purchaseId, int productId, int quantity,int diagnosticId)
        {
 
            var purchaseDetail = await _context.PurchaseDetails
                .Include(pd => pd.product)
                .FirstOrDefaultAsync(pd => pd.purchaseId == purchaseId && pd.product.id == productId);

            if (purchaseDetail != null)
            {
                // Restore product quantity
                var product = purchaseDetail.product;
                product.quantity += quantity;
                _context.Update(product);

                // Remove the purchase detail entry
                _context.PurchaseDetails.Remove(purchaseDetail);

                await _context.SaveChangesAsync();
            }

            // Redirect to the PurchaseDetailsView with the relevant purchase->diagnosticId
            return RedirectToAction(nameof(Edit), new { id = diagnosticId});
        }
        public async Task<IActionResult> DiagnosticHistory()
        {
            var tvm = new TransactionsVm
            {
                IDiagnostics = await _context.Diagnostics.Include(p => p.IdiagnosticDetails).ThenInclude(p => p.Services).Include(p => p.pet).ThenInclude(p => p.owner).Where(p => !string.IsNullOrEmpty(p.status)).ToListAsync()
            };

            return View(tvm);
        }

        public async Task<IActionResult> PurchaseHistory()
        {
            var tvm = new TransactionsVm
            {
                IPurchase = await _context.Purchases.Include(p => p.purchaseDetails).Where(p => !string.IsNullOrEmpty(p.status)).ToListAsync()
            };

            return View(tvm);
        }
    }
}