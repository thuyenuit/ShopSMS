using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL.Repositories;
using ShopSMS.Model.Model;

namespace ShopSMS.Service.Services
{
    public interface IErrorLogService
    {
        void Add(ErrorLog error);

        void SaveChanges();
    }

    internal class ErrorLogService : IErrorLogService
    {
        private readonly IErrorLogRepository errorLogRepository;
        private readonly IUnitOfWork unitOfWork;

        public ErrorLogService(
            IErrorLogRepository errorLogRepository,
            IUnitOfWork unitOfWork)
        {
            this.errorLogRepository = errorLogRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Add(ErrorLog error)
        {
            errorLogRepository.Add(error);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }
    }
}