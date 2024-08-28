using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Transactions
{
    public class Purchase
    {

        [Key]
        [Display(Name = "Purchace ID")]
        public int purchaseId { get; set; }

        [Display(Name = "Date")]
        public DateTime? date { get; set; }

        [Display(Name = "Status")]
        public string?  status { get; set; }


        [Display(Name = "Total Payment")]
        public double? totalProductPayment { get; set; }


        //temporary Fk for Diagnosis id Holder
        public int diagnosisId { get; set; }

        //Navigation Property
        public ICollection<PurchaseDetails> purchaseDetails { get; set; }
    }
}
