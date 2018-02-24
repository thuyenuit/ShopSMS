using ShopSMS.Common.Common;
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
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> Search(IDictionary<string, object> dic);
        string AutoGenericCode(int def, List<Product> listProduct);
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {

        public ProductRepository(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }

        public IEnumerable<Product> Search(IDictionary<string, object> dic)
        {
            int productId = Utils.GetInt(dic, "ProductID");
            string productName = Utils.GetString(dic, "ProductName");
            string productCode = Utils.GetString(dic, "ProductCode");
            string productAlias = Utils.GetString(dic, "ProductAlias");
            string productDescription = Utils.GetString(dic, "Description");
            int productQuantity = Utils.GetInt(dic, "Quantity");
            decimal productPrice = Utils.GetDecimal(dic, "Price");
            string keyword = Utils.GetString(dic, "KeyWord");
            int productCategoryID = Utils.GetInt(dic, "ProductCategoryID");
            int status = Utils.GetInt(dic, "Status");

            IEnumerable<Product> query = DbContext.Product;

            if (status == 1)
                query = query.Where(x => x.Status == true);
            else if (status == 2)
                query = query.Where(x => x.Status == false);

            if (productId != 0)
                query = query.Where(x => x.ProductID == productId);
            if (!string.IsNullOrEmpty(productName))
                query = query.Where(x => x.ProductName.ToUpper().Contains(productName.ToUpper()));
            if (!string.IsNullOrEmpty(productCode))
                query = query.Where(x => x.ProductCode.ToUpper().Contains(productCode.ToUpper()));
            if (!string.IsNullOrEmpty(productAlias))
            {
                query = query.Where(x => x.ProductAlias != null)
                             .Where(x => x.ProductAlias.ToUpper().Contains(productAlias.ToUpper()));
            }
            if (!string.IsNullOrEmpty(productDescription))
            {
                query = query.Where(x => x.Description != null)
                             .Where(x => x.Description.ToUpper().Contains(productDescription.ToUpper()));
            }

            if (productCategoryID != 0)
            {
                query = query.Where(x => x.ProductCategoryID.HasValue)
                             .Where(x => x.ProductCategoryID == productCategoryID);
            }

            if (productQuantity != 0)
                query = query.Where(x => x.Quantity == productQuantity);
            if (productPrice != 0)
                query = query.Where(x => x.PriceSell == productPrice);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.ProductCode.ToUpper().Contains(keyword.ToUpper())
                                || x.ProductName.ToUpper().Contains(keyword.ToUpper()));

            return query;
        }

        public string AutoGenericCode(int def, List<Product> listProduct)
        {
            string code = string.Empty;
            int hauto = (listProduct.Count() + 1).ToString().Length;
            int trungto = 6 - hauto;

            for (int i = 0; i < trungto; i++)
            {
                code += "0";
            }
            code = code + (listProduct.Count() + 1 + def);
            code = "SP" + code;
            return code;
        }
    }
}
