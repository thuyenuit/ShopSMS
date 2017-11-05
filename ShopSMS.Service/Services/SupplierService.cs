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
    public interface ISupplierService
    {
        void Create(Supplier obj);
        void Update(Supplier obj);
        void Delete(int id);
        IEnumerable<Supplier> GetAll();       
        Supplier GetSingleById(int id);
        Supplier GetSingleByName(string name);
        IEnumerable<Supplier> Search(IDictionary<string, object> dic);
        void SaveChanges();
    }

    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository supplierRepository;
        private readonly IUnitOfWork unitOfWork;

        public SupplierService(
            ISupplierRepository supplierRepository,
            IUnitOfWork unitOfWork)
        {
            this.supplierRepository = supplierRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Create(Supplier obj)
        {
            supplierRepository.Create(obj);
        }

        public void Delete(int id)
        {
            supplierRepository.Delete(id);
        }

        public IEnumerable<Supplier> GetAll()
        {
            return supplierRepository.GetAll();
        }

        public Supplier GetSingleById(int id)
        {
            return supplierRepository.GetSingleById(id);
        }

        public Supplier GetSingleByName(string name)
        {
            return GetAll().Where(x => x.SupplierName.ToUpper().Equals(name.ToUpper()))
                            .FirstOrDefault();
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Supplier> Search(IDictionary<string, object> dic)
        {
            return supplierRepository.Search(dic);
        }

        public void Update(Supplier obj)
        {
            supplierRepository.Update(obj);
        }
    }
}
