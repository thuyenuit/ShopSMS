using System;

namespace ShopSMS.DAL.Infrastructure.Interfaces
{
    public interface IDbFactory : IDisposable
    {
        ShopSMSDbcontext Init();
    }
}