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
    [RoutePrefix("api/supplier")]
    public class SupplierController : BaseApiController
    {

        private readonly ISupplierService supplierService;
        public SupplierController(
            IErrorLogService errorLogService,
            ISupplierService supplierService) : base(errorLogService)
        {
            this.supplierService = supplierService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Getall(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                List<Supplier> lstSupplier = supplierService.GetAll().ToList();
                List<SupplierViewModel> lstResponse = lstSupplier
                .Select(x => new SupplierViewModel
                {
                    SupplierID = x.SupplierID,
                    SupplierName = x.SupplierName,
                }).OrderBy(x=>x.SupplierName).ToList();

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
                var objSupplier = supplierService.GetSingleById(id);
                var objResponse = Mapper.Map<Supplier, SupplierViewModel>(objSupplier);
                var response = request.CreateResponse(HttpStatusCode.OK, objResponse);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, SupplierViewModel model)
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
                        Supplier objDB = supplierService.GetSingleByName(model.SupplierName);
                        if (objDB != null)
                        {
                            string msgError = string.Format("Nhà cung cấp {0} đã tồn tại. Vui lòng kiểm tra lại!", model.SupplierName);
                            response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                        }

                        Supplier objNew = new Supplier();
                        objNew.UpdateSupplier(model);
                        supplierService.Create(objNew);
                        supplierService.SaveChanges();
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
        public HttpResponseMessage Update(HttpRequestMessage request, SupplierViewModel model)
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
                        var objDB = supplierService.GetSingleById(model.SupplierID);
                        if (objDB != null)
                        {
                            var checkResult = supplierService.GetAll()
                                                .Where(x => x.SupplierID != model.SupplierID
                                                && x.SupplierName.ToUpper() == model.SupplierName.ToUpper())
                                                .FirstOrDefault();
                            if (checkResult == null)
                            {
                                objDB.SupplierName = model.SupplierName;
                                supplierService.Update(objDB);
                                supplierService.SaveChanges();
                                response = request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                                return response;
                            }
                            else
                            {
                                string msgError = string.Format("Nhà cung cấp {0} đã tồn tại. Vui lòng kiểm tra lại", model.SupplierName);
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

                var result = supplierService.GetSingleById(id);
                if (result != null)
                {
                    supplierService.Delete(id);
                    supplierService.SaveChanges();
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
