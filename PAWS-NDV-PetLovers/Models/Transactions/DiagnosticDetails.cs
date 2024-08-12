using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Transactions
{
    public class DiagnosticDetails
    {
        [Key]
        public int diagnosticDet_Id { get; set; }

        [Required]
        public string? diagnosis { get; set; }

        [ForeignKey("Diagnostics")]
        [Display(Name ="Diagnostic ID")]
        public int diagnosticsId { get; set; }

        [ForeignKey("Services")]
        public int serviceId { get; set; }

        [Required]
        [Display(Name = "Service Price")]
        public double servicePrice { get; set; }

        //navigation Property

        public Services? Services { get; set; } 
    }
}