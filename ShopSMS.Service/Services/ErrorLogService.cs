using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL.Repositories;
using ShopSMS.Model.Model;

namespace ShopSMS.Service.Services
{
    public interface IErrorLogService
    {
        void Create(ErrorLog error);

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

        public void Create(ErrorLog error)
        {
            errorLogRepository.Create(error);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }
    }
}