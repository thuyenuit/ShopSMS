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
    public interface ICategoryService
    {
        void Add(Category category);
        void Update(Category productCategory);
        void Delete(int id);
        IEnumerable<Category> Search(IDictionary<string, object> dic);
        IEnumerable<Category> GetAll();
        IEnumerable<Category> GetAllPaging(int page, int pageSize, out int totalRow);
        Category GetSingleById(int id);
        Category GetSingleByName(string name);
        void SaveChanges();
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;
        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Add(Category category)
        {
            categoryRepository.Add(category);
        }

        public void Update(Category category)
        {
            categoryRepository.Update(category);
        }

        public void Delete(int id)
        {
            categoryRepository.Delete(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return categoryRepository.GetAll();
        }

        public IEnumerable<Category> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return categoryRepository.GetMultiPaging(x => x.Status == true, out totalRow, page, pageSize);
        }

        public Category GetSingleById(int id)
        {
            return categoryRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Category> Search(IDictionary<string, object> dic)
        {
            string keyWord = Utils.GetString(dic, "KeyWord");
            int status = Utils.GetInt(dic, "Status");
            List<int> lstCategoryID = Utils.GetListInt(dic, "lstCategoryID");

            IEnumerable<Category> lstQuery = GetAll();

            if(status == 1)
                lstQuery = lstQuery.Where(x => x.Status == true);
            else if(status == 2)
                lstQuery = lstQuery.Where(x => x.Status == false);

            if (lstCategoryID.Count > 0)
            {
                lstQuery = lstQuery.Where(x => lstCategoryID.Contains(x.CategoryID));
            }

            if (!string.IsNullOrEmpty(keyWord))
            {
                keyWord = keyWord.ToUpper();
                lstQuery = lstQuery.Where(x => x.CategoryName.ToUpper().Contains(keyWord));
            }
            return lstQuery;
        }

        public Category GetSingleByName(string name)
        {
            return GetAll().Where(x=>x.CategoryName.ToUpper() == name.ToUpper()).FirstOrDefault();
        }
    }
}
