using System;

namespace BankProjekt.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public String Number { get; set; }
        public Decimal Balance { get; set; }

        public virtual Profile Profile { get; set; }
    }
}