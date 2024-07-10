using PAWS_NDV_PetLovers.Models.Records;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Appointments
{
    public class Appontment
    {
        [Key]
        public int AppointId { get; set; }

        [ForeignKey("Owner")]
        [Required]
        public int OwnerId {  get; set; }


        //navigation Prpoerty
        public Owner? Owner { get; set; }

    }
}
