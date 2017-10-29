using ShopSMS.Service.Services;
using ShopSMS.Web.Infrastructure.Core;
using ShopSMS.Web.Provider;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ShopSMS.Web.Api
{
    [RoutePrefix("api/home")]
    public class HomeController : BaseApiController
    {
        public static ClaimsIdentity _ClaimsInstance = new ClaimsIdentity();

        private IErrorLogService _errorLogService;

        public HomeController(IErrorLogService errorLogService) : base(errorLogService)
        {
            this._errorLogService = errorLogService;
        }

        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Xin chào SMS";
        }

        [Route("getMenuPer")]
        [HttpGet]
        public HttpResponseMessage GetMenuPer(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var lstMenu = UserInfoInstance.ListGroupMenu;
                var response = request.CreateResponse(HttpStatusCode.OK, lstMenu);
                return response;
            });
        }
    }
}