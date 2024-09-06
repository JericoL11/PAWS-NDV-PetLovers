using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Transactions
{
    public class Diagnostics
    {
        [Key]
        [Display(Name = "Diagnosis ID")]
        public int diagnostic_Id { get; set; }

        [ForeignKey("Pet")]
        [Display(Name = "Pet ID")]
        public int petId { get; set; }

        [Display(Name = "Weight (kg)")]
        [StringLength(5)]
        public string? weight { get; set; }



        [Required]
        [Display(Name = "Service Total")]
        public double? totalServicePayment { get; set; }

        public double grandTotal { get; set; }

        [Required]
        [Display(Name = "Diagnosis Date")]
        [DataType(DataType.Date)]
        public DateTime? date { get; set; }

        public string? status { get; set; }


        //Navigation Property
        public Pet? pet { get; set; }
        public IList<DiagnosticDetails> IdiagnosticDetails { get; set; }

        public Purchase? Purchase { get; set; }


    }
}