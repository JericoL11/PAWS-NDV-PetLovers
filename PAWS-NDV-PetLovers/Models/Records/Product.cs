﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PAWS_NDV_PetLovers.Models.Records
{
    public class Product
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name ="Product Name")]
        public string? productName { get; set; }

        [Required]
        [Display(Name = "Selling Price")]
        public double? sellingPrice { get; set; }

        [Required]
        [Display(Name = "Supplier Price")]
        public double? supplierPrice { get; set; }

        [Required]
        [Display(Name = "Qty On Hand")]
        public int? quantity { get; set; }

        [Required]
        [Display(Name ="Date added")]
        [DataType(DataType.Date)]
        public DateTime? updateDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? expiryDate { get; set; }
       
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        //navigation property
        public Category category { get; set; }
    }
}