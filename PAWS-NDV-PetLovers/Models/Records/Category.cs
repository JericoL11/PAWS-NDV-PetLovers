using System.ComponentModel.DataAnnotations;

namespace PAWS_NDV_PetLovers.Models.Records
{
    public class Category
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string? categoryName { get; set; }

        [Required(ErrorMessage ="Description is required")]
        public string? description { get; set;}


        [Required]
        [Display(Name = "Registered Date")]
        [DataType(DataType.Date)]
        public DateTime? registeredDate { get; set; }
        //navigation property 12m
        public ICollection<Product> Products { get; set; }


    }
}
