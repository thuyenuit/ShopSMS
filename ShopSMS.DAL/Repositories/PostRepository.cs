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
    public interface IPostRepositorycs: IRepository<Post>
    {

    }

    public class PostRepositorycs : RepositoryBase<Post>, IPostRepositorycs
    {

        public PostRepositorycs(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }


    }
}
