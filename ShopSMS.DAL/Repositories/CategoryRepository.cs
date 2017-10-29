using ShopSMS.DAL.Infrastructure.Implements;
using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.DAL.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        //IEnumerable<ProductCategory> GetByAlias(string alias);
    }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {

        public CategoryRepository(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }

        /*public IEnumerable<Category> GetByAlias(string alias)
        {
            return DbContext.c.Where(x => x.ProductCategoryAlias == alias);
        }*/
    }
}
