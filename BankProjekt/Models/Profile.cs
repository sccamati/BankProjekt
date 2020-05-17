using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankProjekt.Models
{
    [Authorize(Roles = "Admin, User")]
    public class Profile
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Pesel { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public String MothersName { get; set; }


        public virtual Address Address { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}