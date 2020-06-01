using System;

namespace BankProjekt.Models
{
    public class Credit
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public Decimal Cash { get; set; }
        public Decimal MoneyToPay { get; set; }
        public Decimal MonthlyRepayment { get; set; }
        public int NumberOfMonthsLeft { get; set; }
        public int NumberOfMonths { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}