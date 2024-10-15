using PAWS_NDV_PetLovers.Models.Appointments;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.ViewModels
{
    public class ReportsVm
    {
        public Appointment Appointment { get; set; }

        public Product Product { get; set; }

        public bool Filtered { get; set; }

        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }

        public string Selection { get; set; }


        public IEnumerable<Appointment> IAppointment { get; set; }
    }
}
