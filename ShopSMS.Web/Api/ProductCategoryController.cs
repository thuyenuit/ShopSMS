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
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopSMS.Web.Api
{
    [RoutePrefix("api/productcategory")]
    [Authorize]
    public class ProductCategoryController : BaseApiController
    {
        IProductService productService;
        IProductCategoryService productCategoryService;
        IApplicationUserService userService;
        ICategoryService categoryService;
        public ProductCategoryController(
            IErrorLogService errorLogService,
            IProductCategoryService productCategoryService,
            IApplicationUserService userService,
            ICategoryService categoryService,
            IProductService productService) : base(errorLogService)
        {
            this.productCategoryService = productCategoryService;
            this.userService = userService;
            this.categoryService = categoryService;
            this.productService = productService;
        }

        [Route("getall")]
        [Authorize]
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
                List<ProductCategory> lstProductCategory = productCategoryService.Search(dic).ToList();
                List<ProductCategoryViewModel> lstResponse = lstProductCategory
                .Select(x => new ProductCategoryViewModel
                {
                    ProductCategoryID = x.ProductCategoryID,
                    ProductCategoryName = x.ProductCategoryName,
                    CategoryName = x.Categories.CategoryName,
                    CategoryID = x.CategoryID,
                    CreateBy = x.CreateBy,
                    CreateDate = x.CreateDate,
                    UpdateBy = x.UpdateBy,
                    UpdateDate = x.UpdateDate,
                    DisplayOrder = x.DisplayOrder,
                    Status = x.Status,
                    IntStatusID = x.Status == true ? 1 : 2
                }).ToList();

                /*string StrDate = string.Empty;
                string StrHour = string.Empty;
                string StrUser = string.Empty;
                var a = lstProductCategory.ToList();
                if (lstResponse.Count > 0)
                {
                    string userUpdate = string.Empty;
                    var objPCByUpdate = lstResponse.OrderByDescending(x => x.UpdateDate).FirstOrDefault();
                    var objPCByCreate = lstResponse.OrderByDescending(x => x.CreateDate).FirstOrDefault();

                    if (objPCByUpdate.UpdateDate.HasValue)
                    {
                        DateTime timeUpdate = objPCByUpdate.UpdateDate.Value;
                        DateTime timeCreate = objPCByCreate.CreateDate.Value;
                        if (timeCreate < timeUpdate)
                        {
                            StrHour = string.Format("{0}h{1}", objPCByUpdate.UpdateDate.Value.ToString("HH"),
                                                            objPCByUpdate.UpdateDate.Value.ToString("mm"));
                            StrDate = objPCByUpdate.UpdateDate.Value.ToString("dd/MM/yyy");
                            userUpdate = objPCByUpdate.UpdateBy;
                        }
                        else
                        {
                            StrHour = string.Format("{0}h{1}", objPCByCreate.CreateDate.Value.ToString("HH"),
                                                           objPCByCreate.CreateDate.Value.ToString("mm"));
                            StrDate = objPCByCreate.CreateDate.Value.ToString("dd/MM/yyy");
                            userUpdate = objPCByCreate.CreateBy;
                        }

                    }
                    else
                    {
                        StrHour = string.Format("{0}h{1}", objPCByCreate.CreateDate.Value.ToString("HH"),
                                                           objPCByCreate.CreateDate.Value.ToString("mm"));
                        StrDate = objPCByCreate.CreateDate.Value.ToString("dd/MM/yyy");
                        userUpdate = objPCByCreate.CreateBy;
                    }

                    if (!string.IsNullOrEmpty(userUpdate))
                    {
                        List<string> lstUserCode = new List<string>();
                        lstUserCode = userUpdate.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x).ToList();
                        userUpdate = lstUserCode[(lstUserCode.Count - 1)].ToString();
                        var userResult = userService.GetSingleByUserCode(userUpdate);
                        if (userResult != null)
                            StrUser = userResult.FullName;
                    }
                }*/

                lstResponse = lstResponse.OrderByDescending(x => x.DisplayOrder)
                                        .ThenBy(x => x.ProductCategoryName).ToList();
                int totalRow = lstResponse.Count();
                IEnumerable<ProductCategoryViewModel> lstResult = lstResponse.Skip((page) * pageSize).Take(pageSize);

                var paginationset = new PaginationSet<ProductCategoryViewModel>()
                {
                    Items = lstResult,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    /*StrDate = StrDate,
                    StrHour = StrHour,
                    StrUser = StrUser*/
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationset);
                return response;
            });
        }

        [Route("getallNoPage")]
        [HttpGet]
        public HttpResponseMessage GetallNoPage(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Status", 1);

                List<ProductCategory> lstProductCategory = productCategoryService.Search(dic).ToList();
                List<ProductCategoryViewModel> lstResponse = lstProductCategory
                .Where(x => x.Status == true)
                .Select(x => new ProductCategoryViewModel
                {
                    ProductCategoryID = x.ProductCategoryID,
                    ProductCategoryName = x.ProductCategoryName,
                    DisplayOrder = x.DisplayOrder
                }).OrderByDescending(x => x.DisplayOrder)
                .ThenBy(x => x.ProductCategoryName).ToList();

                var response = request.CreateResponse(HttpStatusCode.OK, lstResponse);
                return response;
            });
        }

        [Route("getbyid")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int categoryId)
        {
            return CreateHttpResponse(request, () =>
            {
                var objProductcategory = productCategoryService.GetSingleById(categoryId);
                var objResponse = Mapper.Map<ProductCategory, ProductCategoryViewModel>(objProductcategory);
                var response = request.CreateResponse(HttpStatusCode.OK, objResponse);
                return response;
            });
        }

        [Route("create")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVM)
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
                    ProductCategory objPC = new ProductCategory();                   
                    objPC.UpdateProductCategory(productCategoryVM);
                    objPC.CreateDate = DateTime.Now;
                    objPC.CreateBy = UserInfoInstance.UserCodeInstance;
                    objPC.Status = true;

                    bool check = productCategoryService.Create(objPC);
                    if (check)
                    {
                        productCategoryService.SaveChanges();
                        string msg = string.Format("Thêm mới thành công");
                        response = request.CreateResponse(HttpStatusCode.OK, msg);
                        return response;
                    }

                    string msgError = string.Format("Thêm mới thất bại! Thể loại {0} đã tồn tại.", productCategoryVM.ProductCategoryName);
                    response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                }

                return response;
            });
        }

        [Route("update")]
        [Authorize]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryVM)
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
                    var objProductCategory = productCategoryService.GetSingleById(productCategoryVM.ProductCategoryID);

                    if (objProductCategory != null)
                    {
                        var checkResult = productCategoryService.GetAll()
                                        .Where(x => x.CategoryID == productCategoryVM.CategoryID
                                                && x.ProductCategoryName.ToUpper() == productCategoryVM.ProductCategoryName.ToUpper()
                                                && x.ProductCategoryID != productCategoryVM.ProductCategoryID)
                                                .FirstOrDefault();
                        if (checkResult == null)
                        {
                            objProductCategory.ProductCategoryName = productCategoryVM.ProductCategoryName;
                            objProductCategory.CategoryID = productCategoryVM.CategoryID;
                            objProductCategory.DisplayOrder = productCategoryVM.DisplayOrder;
                            objProductCategory.UpdateDate = DateTime.Now;
                            objProductCategory.UpdateBy += UserInfoInstance.UserCodeInstance + ",";
                            objProductCategory.Status = productCategoryVM.IntStatusID == 1 ? true : false;
                            productCategoryService.Update(objProductCategory);
                            productCategoryService.SaveChanges();

                            response = request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                            return response;
                        }
                        else
                        {
                            string msgError = string.Format("Cập nhật thất bại! Thể loại {0} đã tồn tại.", productCategoryVM.ProductCategoryName);
                            response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                            return response;
                        }


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
                var result = productCategoryService.GetSingleById(id);
                if (result != null)
                {
                    IDictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("CategoryID", result.CategoryID);
                    Product objPro = productService.Search(dic).FirstOrDefault();

                    if (objPro == null)
                    {
                        productCategoryService.Delete(id);
                        productCategoryService.SaveChanges();
                        response = request.CreateResponse(HttpStatusCode.OK, result);
                        return response;
                    }
                    string msg = string.Format("Xóa thất bại! Thể loại {0} đã được sử dụng.", result.ProductCategoryName);
                    response = request.CreateResponse(HttpStatusCode.BadGateway, msg);
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
                List<int> listId = new JavaScriptSerializer().Deserialize<List<int>>(jsonlistId);

                foreach (var item in listId)
                {
                    productCategoryService.Delete(item);
                }
                productCategoryService.SaveChanges();
                response = request.CreateResponse(HttpStatusCode.OK, listId.Count());

            }
            return response;
        }

    }
}
