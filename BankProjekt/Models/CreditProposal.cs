using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BankProjekt.Models
{
    [Authorize(Roles = "Admin, User")]
    public class CreditProposal
    {
        public int Id { get; set; }

        [Display(Name = "Bank Account")]
        public int BankAccountId { get; set; }

        [Required]
        [Range(0.01, 999999)]
        public Decimal Cash { get; set; }

        [Required]
        [Range(1, 36)]
        [Display(Name = "Number Of Months")]
        public int NumberOfMonths { get; set; }

        public CreditProposalStatus ProposalStatus { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }

    public enum CreditProposalStatus
    {
        Waiting,
        Approved,
        Rejected
    }
}