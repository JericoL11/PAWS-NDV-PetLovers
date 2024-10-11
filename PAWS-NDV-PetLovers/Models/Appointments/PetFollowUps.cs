using Microsoft.CodeAnalysis;
using PAWS_NDV_PetLovers.Models.Records;
using PAWS_NDV_PetLovers.Models.Transactions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Appointments
{
    public class PetFollowUps
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Diagnostics")]
        public int DiagnosticsId { get; set; }  // Use int instead of Diagnostics? to make the ID required
        public Diagnostics Diagnostics { get; set; }  // Navigation property for Diagnostics

        [ForeignKey("Service")]  // Reference to the specific service
        public int? serviceId { get; set; }  // Nullable in case no service is selected
        public Services? Service { get; set; }  // Navigation property to the Service

        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime date { get; set; }

        public string? status { get; set; }
    }
}
