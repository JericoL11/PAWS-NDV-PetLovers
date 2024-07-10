using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Appointments
{
    public class Appointment
    {
        [Key]
        public int AppointId { get; set; }


        [Required(ErrorMessage = "Please select an owner")]
        [ForeignKey("Owner")]
        public int OwnerId {  get; set; }


        //navigation Prpoerty
        public Owner? Owner { get; set; }


    }
}
