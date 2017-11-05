using AutoMapper;
using ShopSMS.Model.Model;
using ShopSMS.Service.Services;
using ShopSMS.Web.Infrastructure.Core;
using ShopSMS.Web.Infrastructure.Extensions;
using ShopSMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopSMS.Web.Api
{
    public class ProducerController : BaseApiController
    {
        IProducerService producerService;
        public ProducerController(IErrorLogService errorLogService,
            IProducerService producerService)
            : base(errorLogService)
        {
            this.producerService = producerService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Getall(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                List<Producer> lstProducer = producerService.GetAll().ToList();
                List<ProducerViewModel> lstResponse = lstProducer
                .Select(x => new ProducerViewModel
                {
                    ProducerID = x.ProducerID,
                    ProducerName = x.ProducerName,
                }).OrderBy(x => x.ProducerName).ToList();

                var response = request.CreateResponse(HttpStatusCode.OK, lstResponse);
                return response;
            });
        }

        [Route("getbyid")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var objSupplier = producerService.GetSingleById(id);
                var objResponse = Mapper.Map<Producer, ProducerViewModel>(objSupplier);
                var response = request.CreateResponse(HttpStatusCode.OK, objResponse);
                return response;
            });
        }

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
                        Producer objDB = producerService.GetSingleByName(model.ProducerName);
                        if (objDB != null)
                        {
                            string msgError = string.Format("Nhà sản xuất {0} đã tồn tại. Vui lòng kiểm tra lại!", model.ProducerName);
                            response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                        }

                        Producer objNew = new Producer();
                        objNew.UpdateProducer(model);
                        producerService.Create(objNew);
                        producerService.SaveChanges();
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
                        var objDB = producerService.GetSingleById(model.ProducerID);
                        if (objDB != null)
                        {
                            var checkResult = producerService.GetAll()
                                                .Where(x => x.ProducerID != model.ProducerID
                                                && x.ProducerName.ToUpper() == model.ProducerName.ToUpper())
                                                .FirstOrDefault();
                            if (checkResult == null)
                            {
                                objDB.ProducerName = model.ProducerName;
                                producerService.Update(objDB);
                                producerService.SaveChanges();
                                response = request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                                return response;
                            }
                            else
                            {
                                string msgError = string.Format("Nhà sản xuất {0} đã tồn tại. Vui lòng kiểm tra lại", model.ProducerName);
                                response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                                return response;
                            }
                        }
                        else
                            response = request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    catch (Exception ex)
                    {
                        response = request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
                    }

                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            HttpResponseMessage response = null;
            if (!ModelState.IsValid)
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                if (id <= 0)
                    response = request.CreateResponse(HttpStatusCode.NotFound);

                var result = producerService.GetSingleById(id);
                if (result != null)
                {
                    producerService.Delete(id);
                    producerService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            return response;
        }
    }
}
