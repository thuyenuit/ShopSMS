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

    public interface IPostService
    {
        void Add(Post post);
        void Update(Post post);
        void Delele(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow);
        Post GetSingleById(int id);
        void SaveChanges();
    }

    public class PostService : IPostService
    {
        private readonly IPostRepositorycs postRepository;
        private readonly IUnitOfWork unitOfWork;
        public PostService(
            IPostRepositorycs postRepository,
            IUnitOfWork unitOfWork)
        {
            this.postRepository = postRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Add(Post post)
        {
            postRepository.Add(post);
        }

        public void Delele(int id)
        {
            postRepository.Delete(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return postRepository.GetAll();
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return postRepository.GetMultiPaging(x => x.Status == true, out totalRow, page, pageSize);
        }

        public Post GetSingleById(int id)
        {
            return postRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public void Update(Post post)
        {
            postRepository.Update(post);
        }
    }
}
