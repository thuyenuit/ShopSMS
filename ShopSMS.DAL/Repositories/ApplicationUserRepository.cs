using ShopSMS.DAL.Infrastructure.Implements;
using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.DAL.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser GetSingleByUserCode(string userCode);
        
    }

    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {

        public ApplicationUserRepository(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }

        public ApplicationUser GetSingleByUserCode(string userCode)
        {
            return DbContext.Users.Where(x=>x.UserCode.ToUpper() == userCode.ToUpper()).FirstOrDefault();
        }
    }
}
