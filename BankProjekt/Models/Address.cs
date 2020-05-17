using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankProjekt.Models
{
    public class Address
    {
        [ForeignKey("Profile")]
        public int Id { get; set; }
        public String HouseNumber { get; set; }
        public String Street { get; set; }
        public String PostCode { get; set; }
        public String City { get; set; }

        public virtual Profile Profile { get; set; }

    }
}