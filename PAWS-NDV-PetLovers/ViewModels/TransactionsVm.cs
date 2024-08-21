using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.Models.Transactions;

namespace PAWS_NDV_PetLovers.ViewModels
{
    public class TransactionsVm
    {
        public Diagnostics? Diagnostics { get; set; }
        public DiagnosticDetails? DiagnosticDetails { get; set; }
        public Pet? Pets { get; set; }

        //for Appointment Diagnostic
        public Owner? Owner { get; set; }

        //View display
        public IEnumerable<Diagnostics>? IDiagnostics { get; set; }

        //payment part
        public IEnumerable<Services> Services { get; set; } // Add this property to hold the services list



    }
}


