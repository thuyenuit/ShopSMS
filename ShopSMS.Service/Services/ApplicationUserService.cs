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
    public interface IApplicationUserService
    {
        void Create(ApplicationUser user);
        void Update(ApplicationUser user);
        void Delete(int id);
        IEnumerable<ApplicationUser> GetAll();
        IEnumerable<ApplicationUser> GetAllPaging(int page, int pageSize, out int totalRow);
        ApplicationUser GetSingleById(int id);
        ApplicationUser GetSingleByUserCode(string userCode);
        //IEnumerable<ApplicationUser> Search(IDictionary<string, object> dic);
        void SaveChanges();
    }

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public ApplicationUserService(
            IApplicationUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Create(ApplicationUser user)
        {
            userRepository.Add(user);
        }

        public void Delete(int id)
        {
            userRepository.Delete(id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return userRepository.GetAll();
        }

        public IEnumerable<ApplicationUser> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return userRepository.GetMultiPaging(x => !string.IsNullOrEmpty(x.UserCode), out totalRow, page, pageSize);
        }

        public ApplicationUser GetSingleById(int id)
        {
            return userRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public void Update(ApplicationUser user)
        {
            userRepository.Update(user);
        }

        public ApplicationUser GetSingleByUserCode(string userCode)
        {
            return userRepository.GetSingleByUserCode(userCode);
        }
    }
}
