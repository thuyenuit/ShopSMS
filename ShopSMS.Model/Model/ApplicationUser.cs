using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    public class ApplicationUser: IdentityUser
    {
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string UserCode { get; set; }

        [MaxLength(200)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }
            
        public DateTime? BirthDay { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
