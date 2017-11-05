using ShopSMS.Common.Common;
using ShopSMS.DAL.Infrastructure.Implements;
using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.DAL.Repositories
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        IEnumerable<Supplier> Search(IDictionary<string, object> dic);
        //Supplier GetSingleByName(string name);
    }

    public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        public SupplierRepository(IDbFactory dbFactory) : base(dbFactory)
        {}

        /*public Supplier GetSingleByName(string name)
        {
            Supplier obj = DbContext.Supplier.Where(x => x.SupplierName.ToUpper().Equals(name.ToUpper()))
                                    .FirstOrDefault();
            return obj;
        }*/

        public IEnumerable<Supplier> Search(IDictionary<string, object> dic)
        {
            IEnumerable<Supplier> lstQuery = DbContext.Supplier;
            string supplierName = Utils.GetString(dic, "SupplierName");

            if (!string.IsNullOrEmpty(supplierName))
                lstQuery = lstQuery.Where(x => x.SupplierName.ToUpper().Contains(supplierName.ToUpper()));

            return lstQuery;
        }
    }
}
