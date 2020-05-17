using BankProjekt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BankProjekt.DAL
{
    public class BankContext : DbContext
    {
        public BankContext() : base("DefaultConnection")
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<CreditProposal> CreditProposals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<BankProjekt.ViewModels.TransferCreateViewModel> BankAccountSelects { get; set; }
    }
}