using PAWS_NDV_PetLovers.Models.Appointments;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.ViewModels
{
    public class AppointmentVm
    {
        public Appointment? Appointment {  get; set; }

        public AppointmentDetails? AppointmentDetails { get; set; }

        public Services? Services { get; set; }


        //Enumerables for index View

        public IEnumerable<Appointment>? Iappointments { get; set; }

        public IEnumerable<AppointmentDetails>? IappointmentDetails { get; set; }

        public IEnumerable<Owner>? IOwner { get; set; }

        public IEnumerable<Pet>? IPets { get; set; }

        public IEnumerable<Services>? Iservices { get; set; }


    }
}
