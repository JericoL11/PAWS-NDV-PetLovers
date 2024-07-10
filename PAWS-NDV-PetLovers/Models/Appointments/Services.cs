using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Appointments
{
    public class Services
    {
        [Key]
        public int serviceId { get; set; }

        [Required]
        [Display(Name ="Service Name")]
        public string? serviceName {  get; set; }

        [Required]
        [Display(Name = "Service Charge")]
        public double serviceCharge { get; set; }


    }
}
