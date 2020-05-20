using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankProjekt.ViewModels
{
    [NotMapped]
    public class TransferPaymentAndPayOffViewModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public String Info { get; set; }

        [Required]
        [Range(0.01, 9999999999)]
        public Decimal Cash { get; set; }
    }
}