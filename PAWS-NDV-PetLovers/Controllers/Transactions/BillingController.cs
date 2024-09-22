using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.Models.Transactions;
using PAWS_NDV_PetLovers.ViewModels;

namespace PAWS_NDV_PetLovers.Controllers.Transactions
{
    public class BillingController : Controller
    {
        private PAWS_NDV_PetLoversContext _context;
        public BillingController(PAWS_NDV_PetLoversContext context)
        {
            _context = context;
        }

        public IActionResult Index(TransactionsVm vcm)

        {
            if (vcm.activeBoardTab == null)
            {
                vcm.activeBoardTab = DBoardTab.DBoard_Diagnostics;
            }
            return View(vcm);
        }

        
        public  IActionResult SwitchToDboardTab(string? tabName)
        {
            var vm = new TransactionsVm();

            switch (tabName)
            {
                case "DBoard_Diagnostics":
                    vm.activeBoardTab = DBoardTab.DBoard_Diagnostics;
                    break;


                case "DBoard_Purchase":
                    vm.activeBoardTab = DBoardTab.DBoard_Purchase;
                    break;

                default:
                    vm.activeBoardTab = DBoardTab.DBoard_Diagnostics;
                    break;
            }
            return RedirectToAction(nameof(Index), vm);
        }

        public IActionResult SwitchToTab(string? tabName)
        {
            var vm = new TransactionsVm();

            switch (tabName)
            {
                case "Diagnostics":
                    vm.activeBillingTab = BillingTab.Diagnosis;
                    break;

                case "Purchase":
                    vm.activeBillingTab = BillingTab.Purchase;
                    break;

                case "Billing":
                    vm.activeBillingTab = BillingTab.Billing;
                    break;

                default:
                    vm.activeBillingTab = BillingTab.Diagnosis;
                    break;
            }

            return RedirectToAction(nameof(Index), vm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, bool? errorMessage, TransactionsVm tvm)
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
                .FirstOrDefaultAsync(d => d.diagnostic_Id == id);


            if (tvm.activeBillingTab == null)
            {
                tvm.activeBillingTab = BillingTab.Diagnosis;
            }

            TransactionsVm vm = new TransactionsVm
            {
                Diagnostics = firstAsync,
                activeBillingTab = tvm.activeBillingTab,
                errorMessage = errorMessage
            };

            return View(vm);

        }


        //billing switch Tabs
         public IActionResult SwitchToBillingTab(int? id, string? tabName)
        {

            //object instantiation
            var vm = new TransactionsVm();

            switch (tabName)
            {
                case "Diagnosis":
                    vm.activeBillingTab = BillingTab.Diagnosis;
                break;

                case "Purchase":
                    vm.activeBillingTab = BillingTab.Purchase;
                    break;

                case "Billing":
                    vm.activeBillingTab = BillingTab.Billing;
                    break;
            }

            return RedirectToAction(nameof(Edit), new { id = id, vm.activeBillingTab});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Diagnostics")] TransactionsVm tvm)
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

            return RedirectToAction("Index", "BillingDashboard"); // Redirect to a different action after saving
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchase(int? id,Purchase purchase)
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
                    .FirstOrDefaultAsync(d => d.diagnostic_Id == id);

                var purchaseItems = await _context.PurchaseDetails
                    .Include(p => p.product)
                    .ThenInclude(p => p.category)
                    .Include(p => p.Purchase)
                    .Where(p => p.Purchase.diagnosisId_holder == id && string.IsNullOrEmpty(p.Purchase.status))
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
                        .FirstOrDefaultAsync(p => p.diagnosisId_holder == id),
                    Services = await _context.Services.ToListAsync(),
                    IProducts = await _context.Products.Include(p => p.category).ToListAsync(),
                    IPurchaseDetails = purchaseItems,
                    totalPurchasePayment = purchasePayment,
                    activeBillingTab = BillingTab.Purchase

                };

                // Add an error message to the ViewData to display on the view
                ViewData["ErrorMessage"] = "No products were selected. Please select at least one product to proceed.";


                // Return the Edit view with the original data and error message
                return RedirectToAction("Edit", new { id = id,  errorMessage = true , viewModel.activeBillingTab } );
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
            purchase.totalProductPayment = Purchased.Sum();

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
                        .FirstOrDefaultAsync(d => d.diagnostic_Id == id);

                    var purchaseItems = await _context.PurchaseDetails
                        .Include(p => p.product)
                        .ThenInclude(p => p.category)
                        .Include(p => p.Purchase)
                        .Where(p => p.Purchase.diagnosisId_holder == id && string.IsNullOrEmpty(p.Purchase.status))
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
                            .FirstOrDefaultAsync(p => p.diagnosisId_holder == id),
                        Services = await _context.Services.ToListAsync(),
                        IProducts = await _context.Products.Include(p => p.category).ToListAsync(),
                        IPurchaseDetails = purchaseItems,
                        totalPurchasePayment = purchasePayment,
                        activeBillingTab = BillingTab.Purchase
                    };

                    ViewData["ErrorMessage"] = "The purchase does not exist.";
                    return RedirectToAction("Edit", new { id = id, viewModel.activeBillingTab });
                }
                else
                {
                    throw;
                }
            }

     
            var updatedPurchaseItems = await _context.PurchaseDetails
                .Include(p => p.product)
                .ThenInclude(p => p.category)
                .Include(p => p.Purchase)
                .Where(p => p.Purchase.diagnosisId_holder == id && string.IsNullOrEmpty(p.Purchase.status))
                .ToListAsync();



            // Reload the view model with the updated data
            var updatedTvm = new TransactionsVm
            {
                Diagnostics = await _context.Diagnostics.FirstOrDefaultAsync(d => d.diagnostic_Id == id),
                Purchase = await _context.Purchases
                    .Include(p => p.purchaseDetails)
                    .ThenInclude(p => p.product)
                    .ThenInclude(p => p.category)
                    .FirstOrDefaultAsync(p => p.diagnosisId_holder == purchase.diagnosisId_holder),
                Services = await _context.Services.ToListAsync(),
                IProducts = await _context.Products.Include(p => p.category).ToListAsync(),
                activeBillingTab = BillingTab.Purchase
       
            };

            // Return the updated Edit view with the current data
            return RedirectToAction("Edit", new { id = id, updatedTvm.activeBillingTab });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProductFromPurchase(int purchaseId, int productId, int quantity, int diagnosticId)
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
            return RedirectToAction(nameof(Edit), new { id = diagnosticId });
        }

    }
}
