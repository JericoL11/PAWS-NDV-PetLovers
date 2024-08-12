using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Transactions
{
    public class Purchase
    {

        [Key]
        [Display(Name = "Purchace ID")]
        public int purchaseId { get; set; }

        [ForeignKey("PurchaseDetails")]
        public int purchaseDetId { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime? date { get; set; }

        [Required]
        [Display(Name = "Total Payment")]
        public double? totalPayment { get; set; }

        //Navigation Property
        public ICollection<PurchaseDetails> purchaseDetails { get; set; }
    }
}
