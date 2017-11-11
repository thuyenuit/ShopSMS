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
    public interface INSXService
    {
        void Create(Producer obj);
        void Update(Producer obj);
        void Delete(int id);
        IEnumerable<Producer> GetAll();
        Producer GetSingleById(int id);
        IEnumerable<Producer> Search(IDictionary<string, object> dic);
        void SaveChanges();
    }

    public class NSXService : INSXService
    {
        private readonly INCCRepository NCCRepository;
        private readonly IUnitOfWork unitOfWork;

        public NSXService(
            INCCRepository NCCRepository,
            IUnitOfWork unitOfWork)
        {
            this.NCCRepository = NCCRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Create(Producer obj)
        {
            NCCRepository.Create(obj);
        }

        public void Delete(int id)
        {
            NCCRepository.Delete(id);
        }

        public IEnumerable<Producer> GetAll()
        {
            return NCCRepository.GetAll();
        }

        public Producer GetSingleById(int id)
        {
            return NCCRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Producer> Search(IDictionary<string, object> dic)
        {
            return null; //NCCRepository.Search(dic);
        }

        public void Update(Producer obj)
        {
            NCCRepository.Update(obj);
        }
    }
}
