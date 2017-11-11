using AutoMapper;
using ShopSMS.Model.Model;
using ShopSMS.Service.Services;
using ShopSMS.Web.Infrastructure.Core;
using ShopSMS.Web.Infrastructure.Extensions;
using ShopSMS.Web.Models;
using ShopSMS.Web.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopSMS.Web.Api
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : BaseApiController
    {
        IProductService productService;
        public ProductController(IErrorLogService errorLogService,
            IProductService productService) : base(errorLogService) {
            this.productService = productService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request,
           int page, int pageSize, string keyWord, int categoryID, int status)
        {
            return CreateHttpResponse(request, () =>
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("KeyWord", keyWord);
                dic.Add("CategoryID", categoryID);
                dic.Add("Status", status);
                List<Product> lstProduct = productService.Search(dic).ToList();
                List<ProductViewModel> lstResponse = lstProduct
                .Select(x => new ProductViewModel
                {
                    ProductID = x.ProductID,
                    ProductName = x.ProductName,
                    ProductAlias = x.ProductAlias,
                    Quantity = x.Quantity,
                    PriceSell =x.PriceSell,
                    PriceInput = x.PriceInput,
                    ProductCode = x.ProductCode,
                    Avatar = x.Avatar,
                    PromotionPrice = x.PromotionPrice,
                    ProductViewCount = x.ProductViewCount,
                    Warranty = x.Warranty,                    
                    Description = x.Description,
                    CreateBy = x.CreateBy,
                    CreateDate = x.CreateDate,
                    UpdateBy = x.UpdateBy,
                    UpdateDate = x.UpdateDate,
                    Status = x.Status
                }).ToList();

                lstResponse = lstResponse.OrderBy(x => x.ProductCode)
                                        .ThenBy(x => x.ProductName).ToList();
                int totalRow = lstResponse.Count();
                IEnumerable<ProductViewModel> lstResult = lstResponse.Skip((page) * pageSize).Take(pageSize);

                var paginationset = new PaginationSet<ProductViewModel>()
                {
                    Items = lstResult,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationset);
                return response;
            });
        }

        [Route("getbykeyword")]
        [HttpGet]
        public HttpResponseMessage GetByKeyword(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("ProductCode", "SP2017000001");
                var lstProduct = productService.Search(dic);
               // var lstResponse = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProduct);

                var response = request.CreateResponse(HttpStatusCode.OK, lstProduct);
                return response;
            });
        }

        [Route("getbyid")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int categoryId)
        {
            return CreateHttpResponse(request, () =>
            {
                var objProduct = productService.GetSingleById(categoryId);
                var objResponse = Mapper.Map<Product, ProductViewModel>(objProduct);
                var response = request.CreateResponse(HttpStatusCode.OK, objResponse);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel model)
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
                    if (string.IsNullOrEmpty(model.ProductName))
                    {
                        throw new Exception("Vui lòng nhập tên sản phẩm");
                    }

                    Product objCheckProName = productService.GetAll()
                                            .Where(x => x.ProductName.ToUpper().Equals(model.ProductName.ToUpper()))
                                            .FirstOrDefault();
                    if (objCheckProName != null)
                    {
                        throw new Exception(string.Format("Tên sản phẩm {0} đã tồn tại. Vui lòng kiểm tra lại!", model.ProductName));
                    }

                    model.ProductCode = productService.AutoGenericCode();

                    Product objNew = new Product();
                    objNew.UpdateProduct(model);
                    objNew.CreateDate = DateTime.Now;
                    objNew.CreateBy = UserInfoInstance.UserCode;
                    objNew.Status = true;
                    productService.Create(objNew);
                    productService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.Created, "Thêm mới thành công!");
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVM)
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
                    var productDB = productService.GetSingleById(productVM.ProductID);

                    if (productDB != null)
                    {
                        productVM.UpdateDate = DateTime.Now;

                        productDB.UpdateProduct(productVM);
                        productService.Update(productDB);
                        productService.SaveChanges();

                        response = request.CreateResponse(HttpStatusCode.Created, productDB);
                    }
                    else
                        response = request.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Route("updateNameAndPriceSell")]
        [HttpPut]
        public HttpResponseMessage UpdateNameAndPriceSell(HttpRequestMessage request, ProductViewModel model)
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
                    if (model.ProductID <= 0)
                    {
                        throw new Exception("Cập nhật thất bại");
                    }

                    if (string.IsNullOrEmpty(model.ProductName))
                    {
                        throw new Exception("Vui lòng nhập tên sản phẩm");
                    }

                    Product objCheckProName = productService.GetAll()
                                            .Where(x => x.ProductName.ToUpper().Equals(model.ProductName.ToUpper())
                                            && x.ProductID != model.ProductID)
                                            .FirstOrDefault();
                    if (objCheckProName != null)
                    {
                        throw new Exception(string.Format("Tên sản phẩm {0} đã tồn tại. Vui lòng kiểm tra lại!", model.ProductName));
                    }

                    Product objResult = productService.GetSingleById(model.ProductID);
                    objResult.ProductName = model.ProductName;
                    objResult.PriceSell = model.PriceSell;
                    productService.Update(objResult);
                    productService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.Created, "Cập nhật thành công!");
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
                var result = productService.GetSingleById(id);
                if (result != null)
                {
                    productService.Delete(id);
                    productService.SaveChanges();
                    response = request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            return response;
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string jsonlistId)
        {
            HttpResponseMessage response = null;
            if (!ModelState.IsValid)
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                var listLicense = new JavaScriptSerializer().Deserialize<List<int>>(jsonlistId);

                foreach (var item in listLicense)
                {
                    productService.Delete(item);
                }
                productService.SaveChanges();
                response = request.CreateResponse(HttpStatusCode.OK, listLicense.Count());

            }
            return response;
        }
    }
}
