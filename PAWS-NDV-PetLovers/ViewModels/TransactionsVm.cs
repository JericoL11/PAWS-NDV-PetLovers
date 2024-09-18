using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.Models.Transactions;

namespace PAWS_NDV_PetLovers.ViewModels
{
    public class TransactionsVm
    {
        public Diagnostics? Diagnostics { get; set; }
        public DiagnosticDetails? DiagnosticDetails { get; set; }
        public Pet? Pets { get; set; }

        public Purchase? Purchase { get; set; }


        //selected services from appointment
        public List<int>? SelectedServices { get; set; }

        //for Appointment Diagnostica
        public Owner? Owner { get; set; }

        //FOR EDIT diagnostic Add-On
        public IEnumerable<PurchaseDetails>? IPurchaseDetails { get; set; }


        public IEnumerable<Purchase>? IPurchase { get; set; }

        //View display
        public IEnumerable<Diagnostics>? IDiagnostics { get; set; }

        //payment part
        public IEnumerable<Services> Services { get; set; } // Add this property to hold the services list


        //add on Product

        public IEnumerable<Product>? IProducts { get; set; }

        public double? grandTotal { get; set; }

        public double? totalServicePayment { get; set; }

        public double? totalPurchasePayment { get; set; }


        //for view Components
        public Tab activeTab { get; set; }

    }

        public enum Tab
        {
            Diagnostics,
            Purchase,
            Billing
        };
}


