using BankProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankProjekt.ViewModels
{
    public class TransferCreateViewModel
    {
        public int Id { get; set; }
        public String Info { get; set; }
        public String ReceiversName { get; set; }
        public String ReceiversNumber { get; set; }
        public String Title { get; set; }
        public Decimal Cash { get; set; }
        public DateTime Date { get; set; }
    }
}