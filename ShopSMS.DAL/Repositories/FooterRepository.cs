using ShopSMS.Model.Model;
using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL.Infrastructure.Implements;

namespace ShopSMS.DAL.Repositories
{
    public interface IFooterRepository : IRepository<Footer>
    {
       
    }

    public class FooterRepository : RepositoryBase<Footer>, IFooterRepository
    {

        public FooterRepository(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }

       
    }
}
