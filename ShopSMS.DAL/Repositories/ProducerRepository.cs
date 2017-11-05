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
    public interface IProducerRepository : IRepository<Producer>
    {
    }

    public class ProducerRepository : RepositoryBase<Producer>, IProducerRepository
    {
        public ProducerRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
