namespace ShopSMS.DAL.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShopSMS.DAL.ShopSMSDbcontext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShopSMS.DAL.ShopSMSDbcontext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            CreateUser(context);           
        }     

        private void CreateUser(ShopSMSDbcontext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ShopSMSDbcontext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ShopSMSDbcontext()));

            var user = new ApplicationUser()
            {
                UserCode = "ADMIN",
                UserName = "admin",
                Email = "thuyennv1004@gmail.com",
                EmailConfirmed = true,        
                FullName = "Thuyền Bự",
                BirthDay = DateTime.Now,
                PhoneNumber = "0167.2102.464",
                PhoneNumberConfirmed = true,  
            };

            if (manager.Users.Count(x => x.UserName == "admin") == 0)
            {
                manager.Create(user, "123456a@");

                if (!roleManager.Roles.Any())
                {
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                    roleManager.Create(new IdentityRole { Name = "Staff" });
                    roleManager.Create(new IdentityRole { Name = "User" });
                }

                var adminUser = manager.FindByEmail("thuyennv1004@gmail.com");
                manager.AddToRoles(adminUser.Id, new string [] {"Admin", "Staff", "User" });
            }

        }
    }
}
