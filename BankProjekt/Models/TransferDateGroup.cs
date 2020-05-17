using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankProjekt.Models
{
    public class TransferDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public int TransferCount { get; set; }
    }
}