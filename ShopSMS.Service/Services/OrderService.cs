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
    public interface IOrderService
    {
        void Add(Order order);
        void Update(Order order);
        void Delete(int id);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetAllPaging(int page, int pageSize, out int totalRow);
        Order GetSingleById(int id);
        void SaveChanges();
    }

    public class OrderService: IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Add(Order order)
        {
            orderRepository.Add(order);
        }

        public void Delete(int id)
        {
            orderRepository.Delete(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return orderRepository.GetAll();
        }

        public IEnumerable<Order> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return orderRepository.GetMultiPaging(x => true, out totalRow, page, pageSize);
        }

        public Order GetSingleById(int id)
        {
            return orderRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public void Update(Order order)
        {
            orderRepository.Update(order);
        }
    }
}
