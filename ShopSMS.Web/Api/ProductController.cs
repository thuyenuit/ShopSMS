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
                    ProductQuantity = x.ProductQuantity,
                    ProductPrice =x.ProductPrice,
                    ProductCode = x.ProductCode,
                    ProductImage = x.ProductImage,
                    ProductPromotionPrice = x.ProductPromotionPrice,
                    ProductViewCount = x.ProductViewCount,
                    ProductWarranty = x.ProductWarranty,                    
                    ProductDescription = x.ProductDescription,
                    // = x.Categories.CategoryName,
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
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVM)
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
                    Product objPG = new Product();
                    objPG.UpdateProduct(productVM);
                    objPG.CreateDate = DateTime.Now;

                    productService.Add(objPG);
                    productService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.Created, objPG);
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
