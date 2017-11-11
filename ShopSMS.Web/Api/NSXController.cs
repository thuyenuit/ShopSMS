using AutoMapper;
using ShopSMS.Model.Model;
using ShopSMS.Service.Services;
using ShopSMS.Web.Infrastructure.Core;
using ShopSMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopSMS.Web.Api
{
    [RoutePrefix("api/nsx")]
    [Authorize]
    public class NSXController : BaseApiController
    {
        INSXService NSXService;
        public NSXController(IErrorLogService ErrorLogService,
            INSXService NSXService) : base(ErrorLogService)
        {
            this.NSXService = NSXService;
        }

        /// <summary>
        /// Get All Producer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () => {
                var lstNSX = NSXService.GetAll().ToList();
                lstNSX = lstNSX.OrderBy(x => x.ProducerName).ToList();
                var lstResponse = Mapper.Map<List<Producer>, List<ProducerViewModel>>(lstNSX);
                var response = request.CreateResponse(HttpStatusCode.OK, lstResponse);
                return response;
            });
        }

        /// <summary>
        /// Create producer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProducerViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                }
                else
                {
                    try
                    {
                        string msgError = string.Empty;
                        if (string.IsNullOrEmpty(model.ProducerName))
                        {
                            throw new Exception("Vui lòng nhập tên nhà sản xuất!");
                        }
                        Producer objDB = NSXService.GetAll()
                                        .Where(x => x.ProducerName.ToUpper().Equals(model.ProducerName.ToUpper()))
                                        .FirstOrDefault();
                        if (objDB != null)
                        {                          
                            msgError = string.Format("Nhà sản xuất {0} đã tồn tại. Vui lòng kiểm tra lại", model.ProducerName);
                            throw new Exception(msgError);
                            //return request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                        }

                        Producer objNew = new Producer();
                        objNew.ProducerName = model.ProducerName;
                        NSXService.Create(objNew);
                        NSXService.SaveChanges();
                        response = request.CreateResponse(HttpStatusCode.OK, objNew);
                        return response;
                    }
                    catch (Exception ex)
                    {
                        response = request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
                    }

                }
                return response;
            });
        }

        /// <summary>
        /// Update producer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProducerViewModel model)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                }
                else
                {
                    try
                    {
                        string msgError = string.Empty;
                        if (string.IsNullOrEmpty(model.ProducerName))
                        {
                            throw new Exception("Vui lòng nhập tên nhà sản xuất!");
                        }
                        Producer checkValidate = NSXService.GetAll()
                                        .Where(x => x.ProducerName.ToUpper().Equals(model.ProducerName.ToUpper())
                                                && x.ProducerID != model.ProducerID).FirstOrDefault();
                        if (checkValidate != null)
                        {
                            msgError = string.Format("Nhà sản xuất {0} đã tồn tại. Vui lòng kiểm tra lại", model.ProducerName);
                            throw new Exception(msgError);                           
                        }

                        Producer objResult = NSXService.GetSingleById(model.ProducerID);
                        if (objResult != null)
                        {
                            objResult.ProducerName = model.ProducerName;
                            NSXService.Update(objResult);
                            NSXService.SaveChanges();
                            response = request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công!");
                            return response;
                        }
                      
                        response = request.CreateResponse(HttpStatusCode.NotFound, "Xóa thất bại!");
                        return response;
                    }
                    catch (Exception ex)
                    {
                        response = request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
                    }

                }
                return response;
            });
        }

        /// <summary>
        /// Delete producer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    request.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                }
                else
                {
                    try
                    {
                        string msgError = string.Format("Xóa thất bại. Vui lòng kiểm tra lại");
                        if (id <= 0)
                        {
                            // throw new Exception("Có lỗi xả ra!");
                            return request.CreateResponse(HttpStatusCode.NotFound, msgError);
                        }

                        Producer objDB = NSXService.GetSingleById(id);
                        if (objDB == null)
                        {
                            //throw new Exception(msgError);
                            return request.CreateResponse(HttpStatusCode.NotFound, msgError);
                        }

                        NSXService.Delete(objDB.ProducerID);
                        NSXService.SaveChanges();
                        response = request.CreateResponse(HttpStatusCode.OK, "Xóa thành công!");
                        return response;
                    }
                    catch (Exception ex)
                    {
                        response = request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
                    }

                }
                return response;
            });
        }
    }
}
