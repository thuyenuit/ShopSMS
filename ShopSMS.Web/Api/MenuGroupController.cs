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
    public class MenuGroupController : BaseApiController
    {
        IMenuGroupService menuGroupService;

        public MenuGroupController(IErrorLogService errorService, IMenuGroupService menuGroupService) :
            base(errorService)
        {
            this.menuGroupService = menuGroupService;
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {             
            return CreateHttpResponse(request, () =>
            {
                var lstMenuGroup = menuGroupService.GetAll();              
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, lstMenuGroup);
                return response;
            });
        }
    }
}
