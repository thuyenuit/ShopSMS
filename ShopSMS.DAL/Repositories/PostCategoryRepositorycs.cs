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
    public interface IPostCategoryRepositorycs : IRepository<PostCategory>
    {

    }

    public class PostCategoryRepositorycs : RepositoryBase<PostCategory>, IPostCategoryRepositorycs
    {

        public PostCategoryRepositorycs(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }


    }
}
