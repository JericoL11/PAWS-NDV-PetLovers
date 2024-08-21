using PAWS_NDV_PetLovers.Models.Appointments;
using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.ViewModels
{
    public class AppointmentVm
    {
        public Appointment? Appointment {  get; set; }

        public AppointmentDetails? AppointmentDetails { get; set; }

        public List<Services>? IlistServices { get; set; }

        //for  CREATE View
        public IEnumerable<Owner> IOwner { get; set; }




        //List for index View

        public List<AppointmentDetails> IAppDetails { get; set; }

        //GROUP FOR INDEX UNIQUE ID
        public List<AppointmentGroup> AppointmentGrouping { get; set; }

         public List<Appointment> IAppointments { get; set; }


        public List<string> AvailableAM {  get; set; }
        public List<string> AvailablePM { get; set; }
    }


    //grouping technique to display 1 ID distinctly.
    public class AppointmentGroup
    {
        public Appointment Appointment { get; set; }

        public List<AppointmentDetails> Details { get; set; }

        public IEnumerable<Owner>? Owner { get; set; } // for related Owners
    }
}
