using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Text)]
        [Display(Name = "AddressesName")]
        public String AddressesName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "AddressesNumber")]
        public String AddressesNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "ReceiversName")]
        public String ReceiversName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "ReceiversNumber")]
        public String ReceiversNumber { get; set; }

        [StringLength(100, ErrorMessage = "Too long title")]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public String Title { get; set;}

        [Range(0.01, 9999999999)]
        public Decimal Cash { get; set;}
        public Decimal AddresseBalance { get; set; }
        public Decimal ReceiverBalance { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
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