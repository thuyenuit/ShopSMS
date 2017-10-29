using ShopSMS.Service.Services;
using ShopSMS.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopSMS.Web.Api
{
    [RoutePrefix("api/order")]
    public class OrderController : BaseApiController
    {
        IOrderService orderService;
        public OrderController(IErrorLogService errorLogService,
            IOrderService orderService) : base(errorLogService)
        {
            this.orderService = orderService;
        }
    }
}
