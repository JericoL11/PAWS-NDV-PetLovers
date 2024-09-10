﻿using Microsoft.AspNetCore.Mvc;
using PAWS_NDV_PetLovers.Models.Transactions;
using PAWS_NDV_PetLovers.Data;
using PAWS_NDV_PetLovers.ViewModels;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using System.Linq;

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
            if(tvm.Diagnostics != null)
            {
                tvm.Diagnostics.grandTotal = (double)tvm.Diagnostics.totalServicePayment;
            }
     
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
                .Include(d => d.Purchase)
                .ThenInclude(d => d.purchaseDetails)
                .ThenInclude(d => d.product)
                .ThenInclude(d => d.category)
                .FirstOrDefaultAsync(d => d.diagnostic_Id == id);

            var Purchase = await _context.Purchases
                .Include(p => p.purchaseDetails)
                .ThenInclude(p => p.product)
                .ThenInclude(p => p.category)
                .FirstOrDefaultAsync(p => p.diagnosisId_holder == id && p.date == dateOnly);


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
        public async Task<IActionResult> Edit(string customerName, [Bind("Diagnostics")] TransactionsVm tvm)
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

            // List to store individual product totals
            var Purchased = new List<double>();

            //get only the selected ID
            var productIds = diagnostics.Purchase.purchaseDetails
                .Select(p => p.productId).Distinct().ToList();

            // Retrieve the list of products from the context
            var Products = await _context.Products.Where(p => productIds.Contains(p.id)).ToListAsync();

            //only the selected ID will be retrieve
            var productDictionary = Products.ToDictionary(p => p.id);


            // Adding selected products
            if (diagnostics.Purchase.purchaseDetails != null && diagnostics.Purchase.purchaseDetails.Count > 0)
            {

                //update customerName
                diagnostics.Purchase.customerName = customerName;

                foreach (var purchaseDetail in diagnostics.Purchase.purchaseDetails)
                {
                    // Get the price and quantity
                    var price = purchaseDetail.sellingPrice;
                    var quantity = purchaseDetail.quantity;

                    // Calculate the total for this product
                    // the condition quantity > 0 is false, so 1 is used instead.
                    double productTotal = price * (quantity > 0 ? quantity : 1);

                    // Add the total to the Purchased list
                    Purchased.Add(productTotal);



                    // Update product quantity using dictionary for faster lookup
                    if (productDictionary.TryGetValue(purchaseDetail.productId, out var product))
                    {
                        product.quantity -= quantity;
                    }

                }



                // Calculate the total purchase payment
                diagnostics.Purchase.totalProductPayment = Purchased.Sum();

                // Optionally: add the updated PurchaseNav to the context if needed
                _context.Add(diagnostics.Purchase);


                _context.UpdateRange(Products);
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
            diagnostic.grandTotal = (double)grandtotal;
            _context.Update(diagnostic);

            // Update purchase status
            if (purchases != null && purchases.Any())
            {
                foreach (var purchase in purchases)
                {
                    purchase.status = purchasePaymentSuccess;
                }
                #region == temporary remove product == 
                /*   
                         //update product quantity
                      
                             var product = detail.product;
                             if (product != null)
                             {
                                 product.quantity -= detail.quantity; // Reduce quantity
                                 _context.Update(product);
                             }
                         }
                     }

                     _context.UpdateRange(purchases);*/

                #endregion
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
            return RedirectToAction(nameof(PurchaseDetailsView), new { id = diagnosticId });
        }

    }
}