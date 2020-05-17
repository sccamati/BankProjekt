using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankProjekt.Models
{
    [Authorize(Roles = "Admin, User")]
    public class Transfer
    {
        public int Id { get; set; }
        public TransferType TransferType { get; set; }
        public String AddressesName { get; set; }
        public String AddressesNumber { get; set; }
        public String ReceiversName { get; set; }
        public String ReceiversNumber { get; set; }
        public String Title { get; set;}
        public Decimal Cash { get; set;}
        public Decimal AddresseBalance { get; set; }
        public Decimal ReceiverBalance { get; set; }
        public DateTime Date { get; set;}

        public virtual TransferType TransferTypeEnum { get; set; }

    }
    public enum TransferType
    {
    Transfer,
    Payment,
    PayOff
}
}