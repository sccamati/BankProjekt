using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankProjekt.ViewModels
{
    [NotMapped]
    public class TransferCreateViewModel
    {
        public int Id{ get; set; }
        public String Info { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Receivers Name")]
        public String ReceiversName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Receivers Number")]
        public String ReceiversNumber { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public String Title { get; set; }
        [Required]
        [Range(0.01, 99999999999)]
        public Decimal Cash { get; set; }
    }
}