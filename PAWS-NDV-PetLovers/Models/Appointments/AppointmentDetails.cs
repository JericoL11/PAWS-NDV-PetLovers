using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Appointments
{
    public class AppointmentDetails
    {
        [Key]
        [Display(Name = "Details Id")]
        public int AppointId_details { get; set; }

   
        [ForeignKey("Appointment")]
        [Display(Name = "Appointment Id")]
        public int AppointId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Time")]
        public DateTime time { get; set; }


        [ForeignKey("Services")]
        public int serviceID { get; set; }


        [ForeignKey("Pet")]
        public int petID { get; set; }
         

        [Display(Name = "Status")]
        public string? status { get; set; }

        //navigation Property

        public Pet? Pet { get; set; }
        public Appointment? Appointment {get;set;}
        public Services? services { get; set; }

    }
}
