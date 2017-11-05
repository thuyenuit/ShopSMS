using ShopSMS.Model.Model;
using ShopSMS.Service.Services;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopSMS.Web.Infrastructure.Core
{
    public class BaseApiController : ApiController
    {
        private IErrorLogService errorLogService;

        public BaseApiController(IErrorLogService errorLogService)
        {
            this.errorLogService = errorLogService;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage reponse = null;

            try
            {
                reponse = function.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has thư following validation error.");

                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"Property: \"{ve.PropertyName}\" in state \"{ve.ErrorMessage}\"");
                    }
                }

                LogError(ex);
                reponse = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
            catch (DbUpdateException dbEx)
            {
                LogError(dbEx);
                reponse = requestMessage.CreateResponse(HttpStatusCode.BadRequest, dbEx.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                reponse = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            return reponse;
        }

        private void LogError(Exception ex)
        {
            try
            {
                ErrorLog erroLog = new ErrorLog();
                erroLog.ErrorLogMessage = ex.Message;
                erroLog.ErrorLogCreateDate = DateTime.Now;
                erroLog.ErrorLogStackTrace = ex.StackTrace;

                errorLogService.Create(erroLog);
                errorLogService.SaveChanges();
            }
            catch
            {
                throw new Exception("Có lỗi xảy ra");
            }
        }
    }
}