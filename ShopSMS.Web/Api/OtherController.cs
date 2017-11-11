using ShopSMS.Service.Services;
using ShopSMS.Web.Infrastructure.Core;
using ShopSMS.Web.Provider;
using ShopSMS.Web.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Web.Http;

namespace ShopSMS.Web.Api
{
    [RoutePrefix("api/other")]
    [Authorize]
    public class OtherController : BaseApiController
    {
        public OtherController(IErrorLogService errorLogService) 
            : base(errorLogService)
        {
        }

        [Route("getListStatus")]
        [HttpGet]
        public HttpResponseMessage GetListStatus(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var listStatus = UserInfoInstance.ListStatus;
                var response = request.CreateResponse(HttpStatusCode.OK, listStatus);
                return response;
            });
        }

        [Route("getListResources")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetListResources(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                ResourceSet resources =
                    ResourceVI.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

                Dictionary<string, string> dic = new Dictionary<string, string>();

                foreach (DictionaryEntry item in resources)
                {
                    dic.Add(item.Key.ToString(), item.Value.ToString());
                }

                var response = request.CreateResponse(HttpStatusCode.OK, dic);
                return response;
            });
        }
    }
}
