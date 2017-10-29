using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL.Repositories;
using ShopSMS.Model.Model;
using System.Collections.Generic;
using System;

namespace ShopSMS.Service.Services
{
    public interface IProductService
    {
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllPaging(int page, int pageSize, out int totalRow);
        Product GetSingleById(int id);
        IEnumerable<Product> Search(IDictionary<string, object> dic);
        void SaveChanges();
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Add(Product product)
        {
            productRepository.Add(product);
        }

        public void Delete(int id)
        {
            productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return productRepository.GetAll();
        }

        public IEnumerable<Product> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return productRepository.GetMultiPaging(x => x.Status == true, out totalRow, page, pageSize);
        }

        public Product GetSingleById(int id)
        {
            return productRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Product> Search(IDictionary<string, object> dic)
        {
            return productRepository.Search(dic);
        }

        public void Update(Product product)
        {
            productRepository.Update(product);
        }
    }
}