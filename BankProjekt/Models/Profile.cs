using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BankProjekt.Models
{
    [Authorize(Roles = "Admin, User")]
    public class Profile
    {
        public int Id { get; set; }

        [StringLength(50)]
        public String Name { get; set; }

        [StringLength(50)]
        public String LastName { get; set; }

        public String Email { get; set; }

        [StringLength(11)]
        public String Pesel { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(50)]
        public String MothersName { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}