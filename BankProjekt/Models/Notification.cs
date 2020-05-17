using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankProjekt.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public String Message { get; set; }
        public DateTime Date { get; set; }
        

        public virtual Profile Profile { get; set; }
    }
}