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
    // định nghĩa thêm các method cần thêm không có sẵn trong RepositoryBase
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        //IQueryable<ProductCategory> Search(IDictionary<string, object> dic);
        //IEnumerable<ProductCategory> GetByAlias(string alias);
    }

    public class ProductCategoryRepository: RepositoryBase<ProductCategory>, IProductCategoryRepository
    {

        public ProductCategoryRepository(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }

        /*public IEnumerable<ProductCategory> GetAllProductCategory()
        {
            return DbContext.ProductCategory;
        }*/      
    }
}
