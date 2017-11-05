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
    public interface IProducerService
    {
        void Create(Producer obj);
        void Update(Producer obj);
        void Delete(int id);
        IEnumerable<Producer> GetAll();
        Producer GetSingleById(int id);
        Producer GetSingleByName(string name);
        void SaveChanges();

    }

    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository producerRepository;
        private readonly IUnitOfWork unitOfWork;
        public ProducerService(
            ProducerRepository producerRepository,
            IUnitOfWork unitOfWork) {
            this.producerRepository = producerRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Create(Producer obj)
        {
            producerRepository.Create(obj);
        }

        public void Delete(int id)
        {
            producerRepository.Delete(id);
        }

        public IEnumerable<Producer> GetAll()
        {
            return producerRepository.GetAll();
        }

        public Producer GetSingleById(int id)
        {
            return producerRepository.GetSingleById(id);
        }

        public Producer GetSingleByName(string name)
        {
            return GetAll().Where(x => x.ProducerName.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public void Update(Producer obj)
        {
            producerRepository.Update(obj);
        }
    }
}
