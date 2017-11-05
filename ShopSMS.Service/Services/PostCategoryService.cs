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
    public interface IPostCategoryService
    {
        void Create(PostCategory postCategory);
        void Update(PostCategory postCategory);
        void Delele(int id);
        IEnumerable<PostCategory> GetAll();
        IEnumerable<PostCategory> GetAllPaging(int page, int pageSize, out int totalRow);
        PostCategory GetSingleById(int id);
        void SaveChanges();
    }

    public class PostCategoryService : IPostCategoryService
    {
        private readonly IPostCategoryRepositorycs postCategoryRepository;
        private readonly IUnitOfWork unitOfWork;
        public PostCategoryService(
            IPostCategoryRepositorycs postCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            this.postCategoryRepository = postCategoryRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Create(PostCategory postCategory)
        {
            postCategoryRepository.Create(postCategory);
        }

        public void Delele(int id)
        {
            postCategoryRepository.Delete(id);
        }

        public IEnumerable<PostCategory> GetAll()
        {
            return postCategoryRepository.GetAll();
        }

        public IEnumerable<PostCategory> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return postCategoryRepository.GetMultiPaging(x => x.Status == true, out totalRow, page, pageSize);
        }

        public PostCategory GetSingleById(int id)
        {
            return postCategoryRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public void Update(PostCategory postCategory)
        {
            postCategoryRepository.Update(postCategory);
        }
    }
}
