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
    public interface INCCRepository : IRepository<Producer>
    {

    }

    public class NCCRepository : RepositoryBase<Producer>, INCCRepository
    {
        public NCCRepository(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }
    }
}
