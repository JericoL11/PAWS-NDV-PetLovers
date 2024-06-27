using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.ViewModels
{
    public class RecordsVm
    {
        public Owner Owner { get; set; }
        public Pet Pet { get; set; }


        //Model Pets Ienumerable
        public IEnumerable<Pet> IPets { get; set; }
     
    }
}
