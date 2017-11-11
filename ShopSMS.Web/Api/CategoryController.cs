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
    [RoutePrefix("api/category")]
    [Authorize]
    public class CategoryController : BaseApiController
    {
        ICategoryService categoryService;
        IProductCategoryService productCategoryService;
        public CategoryController(
            IErrorLogService errorLogService,
            ICategoryService categoryService,
            IProductCategoryService productCategoryService) : base(errorLogService)
        {
            this.categoryService = categoryService;
            this.productCategoryService = productCategoryService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request,
            int page, int pageSize, string keyWord, int status)
        {
            return CreateHttpResponse(request, () =>
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("KeyWord", keyWord);
                dic.Add("Status", status);
                List<Category> lstProductCategory = categoryService.Search(dic).ToList();
                List<CategoryViewModel> lstResponse = lstProductCategory
                .Select(x => new CategoryViewModel
                {
                    CategoryID = x.CategoryID,
                    CategoryName = x.CategoryName,
                    CreateBy = x.CreateBy,
                    CreateDate = x.CreateDate,
                    UpdateBy = x.UpdateBy,
                    UpdateDate = x.UpdateDate,
                    DisplayOrder = x.DisplayOrder,
                    Status = x.Status
                }).OrderByDescending(x => x.DisplayOrder)
                .ThenBy(x => x.CategoryName).ToList();

                int totalRow = lstResponse.Count();
                IEnumerable<CategoryViewModel> lstResult = lstResponse.Skip((page) * pageSize).Take(pageSize);

                var paginationset = new PaginationSet<CategoryViewModel>()
                {
                    Items = lstResult,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
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
                List<Category> lstProductCategory = categoryService.GetAll().ToList() ;
                List<CategoryViewModel> lstResponse = lstProductCategory
                .Where(x => x.Status == true)
                .Select(x => new CategoryViewModel
                {
                    CategoryID = x.CategoryID,
                    CategoryName = x.CategoryName,
                    DisplayOrder = x.DisplayOrder,
                    IntStatusID = x.Status == true ? 1 : 2
                }).OrderByDescending(x => x.DisplayOrder)
                .ThenBy(x => x.CategoryName).ToList();
             
                var response = request.CreateResponse(HttpStatusCode.OK, lstResponse);
                return response;
            });
        }      

        [Route("getbyid")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int categoryId)
        {
            return CreateHttpResponse(request, () =>
            {
                var objCategory = categoryService.GetSingleById(categoryId);
                var objResponse = Mapper.Map<Category, CategoryViewModel>(objCategory);
                var response = request.CreateResponse(HttpStatusCode.OK, objResponse);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, CategoryViewModel categoryVM)
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
                        Category objDB = categoryService.GetSingleByName(categoryVM.CategoryName);
                        if (objDB != null)
                        {
                            string msgError = string.Format("Thêm mới thất bại! Danh mục {0} đã tồn tại.", categoryVM.CategoryName);
                            response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                        }

                        Category objCate = new Category();
                        objCate.UpdateCategory(categoryVM);
                        objCate.CreateDate = DateTime.Now;
                        objCate.CreateBy = UserInfoInstance.UserCode;
                        objCate.Status = true;
                        if (objCate.DisplayOrder == null)
                        {
                            objCate.DisplayOrder = categoryService.GetAll().ToList().Count() + 1;
                        }

                        categoryService.Create(objCate);
                        categoryService.SaveChanges();
                        response = request.CreateResponse(HttpStatusCode.OK, objCate);
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
        public HttpResponseMessage Update(HttpRequestMessage request, CategoryViewModel categoryVM)
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
                        var objDB = categoryService.GetSingleById(categoryVM.CategoryID);
                        if (objDB != null)
                        {
                            var checkResult = categoryService.GetAll().Where(x => x.CategoryID != categoryVM.CategoryID
                                                                            && x.CategoryName.ToUpper() == categoryVM.CategoryName.ToUpper())
                                                                    .FirstOrDefault();
                            if (checkResult == null)
                            {
                                objDB.CategoryName = categoryVM.CategoryName;
                                objDB.UpdateDate = DateTime.Now;
                                objDB.UpdateBy += UserInfoInstance.UserCode + ",";
                                categoryService.Update(objDB);
                                categoryService.SaveChanges();

                                response = request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                                return response;
                            }
                            else
                            {
                                string msgError = string.Format("Cập nhật thất bại! Danh mục {0} đã tồn tại.", categoryVM.CategoryName);
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
                if(id <= 0)
                    response = request.CreateResponse(HttpStatusCode.NotFound);

                var result = categoryService.GetSingleById(id);
                if (result != null)
                {
                    var objProductCategory = productCategoryService.GetByCategoryId(result.CategoryID).FirstOrDefault();
                    if (objProductCategory != null)
                    {
                        string msgError = string.Format("Xóa thất bại! Danh mục {0} đã được sử dụng", result.CategoryName);
                        response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                        return response;
                    }

                    categoryService.Delete(id);
                    categoryService.SaveChanges();
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
                List<int> listId = new JavaScriptSerializer().Deserialize<List<int>>(jsonlistId);

                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("lstCategoryID", listId);

                List<int> lstCategoryID = productCategoryService.Search(dic)
                    .Select(x=> x.Categories.CategoryID).Distinct().ToList();

                if (lstCategoryID.Count > 0)
                {
                    dic["lstCategoryID"] = lstCategoryID;
                    List<Category> lstCategory = categoryService.Search(dic).ToList();
                    string name = string.Empty;
                    for (int i = 0 ; i < lstCategory.Count; i++)
                    {
                        if (i < lstCategory.Count -1)
                            name += lstCategory[i].CategoryName + ", " ;
                        else if(i == lstCategory.Count -1)
                            name += lstCategory[i].CategoryName;
                    }

                    string msgError = string.Format("Xóa thất bại! Danh mục {0} đã được khai báo. Vui lòng kiểm tra lại", name);
                    response = request.CreateResponse(HttpStatusCode.BadGateway, msgError);
                    return response;
                }

                foreach (var item in listId)
                {
                    categoryService.Delete(item);
                }
                categoryService.SaveChanges();
                response = request.CreateResponse(HttpStatusCode.OK, listId.Count());

            }
            return response;
        }

    }
}
