using BankProjekt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BankProjekt.DAL
{
    public class BankInitializer : DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));
            roleManager.Create(new IdentityRole("Worker"));

            var user = new ApplicationUser { UserName = "email@wp.pl" };
            string password = "Password1.";
            userManager.Create(user, password);
            userManager.AddToRole(user.Id, "User");

            var user2 = new ApplicationUser { UserName = "email2@wp.pl" };
            string password2 = "Password2.";
            userManager.Create(user2, password2);
            userManager.AddToRole(user2.Id, "User");

            var user3 = new ApplicationUser { UserName = "admin@wp.pl" };
            string password3 = "Admin1.";
            userManager.Create(user3, password3);
            userManager.AddToRole(user3.Id, "Admin");

            var worker = new ApplicationUser { UserName = "worker@wp.pl" };
            string workerpass = "Worker1.";
            userManager.Create(worker, workerpass);
            userManager.AddToRole(worker.Id, "Worker");

            var addresses = new List<Address>
            {
                new Address {HouseNumber = "8A", Street = "Warszawska", City = "Siedlce",  PostCode = "02-312"},
                new Address {HouseNumber = "3", Street = "Dobra", City = "Warszawa",  PostCode = "04-322"},
                new Address {HouseNumber = "51C", Street = "Fajna", City = "Siedlce",  PostCode = "03-112"}
            };

            addresses.ForEach(a => context.Addresses.Add(a));

            var profiles = new List<Profile>
            {
                new Profile { Address = addresses[0], Name = "Marcin", LastName = "Fajny", Email = user.UserName, BirthDate = DateTime.Parse("1998-01-03"), MothersName = "Magda", Pesel = "12312312312"},
                new Profile { Address = addresses[1], Name = "Paweł", LastName = "Kowalski", Email = user2.UserName, BirthDate = DateTime.Parse("1999-02-03"), MothersName = "Aleksandra", Pesel = "12312312313"},
                new Profile { Address = addresses[2], Name = "Adam", LastName = "Dobry", Email = user3.UserName, BirthDate = DateTime.Parse("2000-05-03"), MothersName = "Marysia", Pesel = "12312312314"},
            };

            profiles.ForEach(p => context.Profiles.Add(p));
            context.SaveChanges();

            var bankAccounts = new List<BankAccount>
            {
                new BankAccount {Number = "1" , Balance = 0, Profile = profiles[0]},
                new BankAccount {Number = "2" , Balance = 0, Profile = profiles[1]},
                new BankAccount {Number = "3" , Balance = 0, Profile = profiles[2]}
            };

            bankAccounts.ForEach(b => context.BankAccounts.Add(b));
            context.SaveChanges();

            var Transfers = new List<Transfer>
            {
                new Transfer { TransferType = TransferType.Transfer, AddressesNumber = "1", ReceiversNumber = "2", AddressesName = "Pawel", ReceiversName = "Marek", Title = "za zakupy", Cash = 20, Date = DateTime.Parse("2020-05-03")},
                new Transfer { TransferType = TransferType.Transfer, AddressesNumber = "2", ReceiversNumber = "1", AddressesName = "Maciek", ReceiversName = "Robert", Title = "za zakupy", Cash = 35, Date = DateTime.Parse("2020-05-03")},
                new Transfer { TransferType = TransferType.Transfer, AddressesNumber = "3", ReceiversNumber = "1", AddressesName = "Tomek", ReceiversName = "Marek", Title = "za zakupy", Cash = 40, Date = DateTime.Parse("2020-05-03")},
                new Transfer { TransferType = TransferType.Transfer, AddressesNumber = "3", ReceiversNumber = "2", AddressesName = "Tomek", ReceiversName = "Marek", Title = "za zakupy", Cash = 40, Date = DateTime.Parse("2020-05-03")}
            };

            Transfers.ForEach(n => context.Transfers.Add(n));
            context.SaveChanges();
        }
    }
}