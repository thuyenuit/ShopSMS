﻿using ShopSMS.DAL.Infrastructure.Implements;
using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.DAL.Repositories
{

    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {

    }

    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {

        public OrderDetailRepository(IDbFactory dbFactory) :
            base(dbFactory)
        {

        }


    }
}
