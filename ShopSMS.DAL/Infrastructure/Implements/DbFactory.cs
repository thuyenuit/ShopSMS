using ShopSMS.DAL.Infrastructure.Interfaces;

namespace ShopSMS.DAL.Infrastructure.Implements
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ShopSMSDbcontext dbContext;

        public ShopSMSDbcontext Init()
        {
            return dbContext ?? (dbContext = new ShopSMSDbcontext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}