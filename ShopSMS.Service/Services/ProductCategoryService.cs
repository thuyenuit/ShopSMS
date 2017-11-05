using ShopSMS.Common.Common;
using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL.Repositories;
using ShopSMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Service.Services
{
    public interface IProductCategoryService
    {
        bool Create(ProductCategory productCategory);
        void Update(ProductCategory productCategory);
        void Delete(int id);
        IEnumerable<ProductCategory> Search(IDictionary<string, object> dic);
        IEnumerable<ProductCategory> GetAll();
        IEnumerable<ProductCategory> GetAllPaging(int page, int pageSize, out int totalRow);
        ProductCategory GetSingleById(int id);
        IEnumerable<ProductCategory> GetByCategoryId(int categoryID);
        void SaveChanges();
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IUnitOfWork unitOfWork;
        public ProductCategoryService(
            IProductCategoryRepository productCategoryRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.unitOfWork = unitOfWork;
        }

        public bool Create(ProductCategory obj)
        {
            List<ProductCategory> lstPC = productCategoryRepository.GetAll().ToList();

            string productName = obj.ProductCategoryName.Trim().ToUpper().ToString();
            var check = lstPC.Where(x => x.ProductCategoryName.ToUpper() == productName).FirstOrDefault();
            if (check != null)
                return false;

            int? orderBy = obj.DisplayOrder;
            obj.DisplayOrder = orderBy == null ? (lstPC.Count() + 1) : orderBy;
            productCategoryRepository.Create(obj);
            return true;
        }

        public void Update(ProductCategory productCategory)
        {
            productCategoryRepository.Update(productCategory);
        }

        public void Delete(int id)
        {
            productCategoryRepository.Delete(id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return productCategoryRepository.GetAll();
            /*IEnumerable<ProductCategory> queryPC = productCategoryRepository.GetAll();

            string _keyword = keyWord.ToUpper();

            if (!string.IsNullOrEmpty(_keyword))
            {
                queryPC = queryPC.Where(x => x.ProductCategoryName.ToUpper().Contains(_keyword)
                                           //|| x.ProductCategoryAlias.ToUpper().Contains(_keyword)
                                           //|| x.MetaDescription.ToUpper().Contains(_keyword)
                                           //|| x.MetaKeyword.ToUpper().Contains(_keyword)
                                           || x.MetaDescription.ToUpper().Contains(_keyword)
                                           || x.Categories.CategoryName.ToUpper().Contains(_keyword)).ToList();
            }

            IEnumerable<Category> queryC = categoryRepository.GetAll();

            var query = queryPC.Join(queryC,
                        a => a.CategoryID,
                        b => b.CategoryID,
                        (a, b) => new {a}).Select(x=>x.a);

            return query;*/
        }

        public IEnumerable<ProductCategory> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return productCategoryRepository.GetMultiPaging(x => x.Status == true, out totalRow, page, pageSize);
        }

        public ProductCategory GetSingleById(int id)
        {
            return productCategoryRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<ProductCategory> Search(IDictionary<string, object> dic)
        {
            string keyWord = Utils.GetString(dic, "KeyWord");
            int categoryID = Utils.GetInt(dic, "CategoryID");
            int status = Utils.GetInt(dic, "Status");
            List<int> lstCategoryID = Utils.GetListInt(dic, "lstCategoryID");

            IEnumerable<ProductCategory> lstQuery = GetAll();

            if (status == 1)
                lstQuery = lstQuery.Where(x => x.Status == true);
            else if (status == 2)
                lstQuery = lstQuery.Where(x => x.Status == false);
            if (categoryID > 0)
                lstQuery = lstQuery.Where(x => x.CategoryID == categoryID);
            if (lstCategoryID.Count > 0)
                lstQuery = lstQuery.Where(x => lstCategoryID.Contains(x.CategoryID));
            if (!string.IsNullOrEmpty(keyWord))
            {
                keyWord = keyWord.ToUpper();
                lstQuery = lstQuery.Where(x => x.ProductCategoryName.ToUpper().Contains(keyWord));
            }
            IEnumerable<Category> lstCategory = categoryRepository.GetAll();
            var abc = lstQuery.ToList();
            lstQuery = lstQuery.Join(lstCategory,
                        a => a.CategoryID,
                        b => b.CategoryID,
                        (a, b) => new { a }).Select(x => x.a);

            return lstQuery;
        }

        public IEnumerable<ProductCategory> GetByCategoryId(int categoryID)
        {
            IEnumerable<ProductCategory> lstQuery = GetAll().Where(x=>x.CategoryID == categoryID);
            return lstQuery;
        }
    }
}
