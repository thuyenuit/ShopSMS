using ShopSMS.DAL.Infrastructure.Interfaces;

namespace ShopSMS.DAL.Infrastructure.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private ShopSMSDbcontext dbContext;

        public UnitOfWork(IDbFactory _dbFactory) {
            this.dbFactory = _dbFactory;
        }

        public ShopSMSDbcontext DbContext
        {
            get { return dbContext ?? ( dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
