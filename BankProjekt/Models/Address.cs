using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankProjekt.Models
{
    public class Address
    {
        [ForeignKey("Profile")]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "House Number")]
        public String HouseNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Street")]
        public String Street { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Post Code")]
        public String PostCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public String City { get; set; }

        public virtual Profile Profile { get; set; }
    }
}