using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BankProjekt.Models
{
    
    public class Transfer
    {
        public int Id { get; set; }
        public TransferType TransferType { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Addresses Name")]
        public String AddressesName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Addresses Number")]
        public String AddressesNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Receivers Name")]
        public String ReceiversName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Receivers Number")]
        public String ReceiversNumber { get; set; }

        [StringLength(100, ErrorMessage = "Too long title")]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public String Title { get; set; }

        [Range(0.01, 9999999999)]
        public Decimal Cash { get; set; }

        [Display(Name = "Addresse Balance")]
        public Decimal AddresseBalance { get; set; }

        [Display(Name = "Receiver Balance")]
        public Decimal ReceiverBalance { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }

    public enum TransferType
    {
        Transfer,
        Payment,
        PayOff,
        CreditPayment,
        Credit
    }
}