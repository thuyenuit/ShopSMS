using AutoMapper;
using OfficeOpenXml;
using ShopSMS.Common.Common;
using ShopSMS.Model.Model;
using ShopSMS.Service.Services;
using ShopSMS.Web.Infrastructure.Core;
using ShopSMS.Web.Infrastructure.Extensions;
using ShopSMS.Web.Models;
using ShopSMS.Web.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopSMS.Web.Api
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : BaseApiController
    {
        private IProductService productService;
        private INSXService NSXService;
        private IProductCategoryService productCategoryService;
        private ICategoryService categoryService;

        public ProductController(IErrorLogService errorLogService,
            IProductService productService,
            INSXService NSXService,
            IProductCategoryService productCategoryService,
            ICategoryService categoryService) : base(errorLogService)
        {
            this.productService = productService;
            this.NSXService = NSXService;
            this.productCategoryService = productCategoryService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Get All Paging
        /// </summary>
        /// <param name="request"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <param name="categoryID"></param>
        /// <param name="status"></param>
        /// <returns> List Product</returns>
        [Route("getallpaging")]
        [HttpGet]
        public HttpResponseMessage GetAllPaging(HttpRequestMessage request,
           int page, int pageSize, string keyWord, int? categoryID, int? status)
        {
            return CreateHttpResponse(request, () =>
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("KeyWord", keyWord);
                if (categoryID.HasValue)
                    dic.Add("ProductCategoryID", categoryID);
                if (status.HasValue)
                    dic.Add("Status", status);

                List<Product> lstProduct = productService.Search(dic).ToList();

                List<Producer> lstProducer = NSXService.GetAll().ToList();
                List<Items> lstItemsProducer = lstProducer.Select(x => new Items { Value = x.ProducerID, Text = x.ProducerName }).ToList();
                List<ProductCategory> lstProCategory = productCategoryService.GetAll().ToList();
                List<Items> lstItemsPC = lstProCategory.Select(x => new Items { Value = x.ProductCategoryID, Text = x.ProductCategoryName }).ToList();

                List<ProductViewModel> lstResponse = lstProduct.Select(x => new ProductViewModel
                {
                    ProductID = x.ProductID,
                    ProductName = x.ProductName,
                    ProductAlias = x.ProductAlias,
                    Quantity = x.Quantity,
                    PriceSell = x.PriceSell,
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
                    Status = x.Status,
                    ProductCategoryID = x.ProductCategoryID,
                    ProducerID = x.ProducerID,
                    ProductCategoryName = x.ProductCategoryID.HasValue ? (
                                        lstItemsPC.FirstOrDefault(p => p.Value == x.ProductCategoryID) != null ?
                                        lstItemsPC.FirstOrDefault(p => p.Value == x.ProductCategoryID).Text : Res.Get("Not_Yet"))
                                        : Res.Get("Not_Yet"),
                    ProducerName = x.ProducerID.HasValue ? (
                                        lstItemsProducer.FirstOrDefault(p => p.Value == x.ProducerID) != null ?
                                        lstItemsProducer.FirstOrDefault(p => p.Value == x.ProducerID).Text : Res.Get("Not_Yet"))
                                        : Res.Get("Not_Yet"),
                }).ToList();

                lstResponse = lstResponse.OrderByDescending(x => x.ProductCode)
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
                        throw new Exception(Res.Get("Please_Enter_Product_Name"));
                    }

                    Product objCheckProName = productService.GetAll()
                                            .Where(x => x.ProductName.ToUpper().Equals(model.ProductName.ToUpper()))
                                            .FirstOrDefault();
                    if (objCheckProName != null)
                    {
                        throw new Exception(string.Format("Tên sản phẩm {0} đã tồn tại. Vui lòng kiểm tra lại!", model.ProductName));
                    }

                    model.ProductCode = productService.AutoGenericCode(0, productService.GetAll().ToList());

                    Product objNew = new Product();
                    objNew.UpdateProduct(model);
                    objNew.CreateDate = DateTime.Now;
                    objNew.CreateBy = UserInfoInstance.UserCode;
                    objNew.Status = true;
                    productService.Create(objNew);
                    productService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.Created, Res.Get("Create_Success"));
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
                        throw new Exception(Res.Get("Please_Enter_Product_Name"));
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
                    response = request.CreateResponse(HttpStatusCode.Created, Res.Get("Update_Success"));
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

        [Route("importExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ImportExcel()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Định dạng không được hỗ trợ!");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadFiles/Excels");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            int categoryID = Int32.Parse(result.FormData["categoryID"]);
            int productCategoryID = Int32.Parse(result.FormData["productCategoryID"]);

            var objCategory = categoryService.GetSingleById(categoryID);
            if (objCategory == null || (objCategory != null && !objCategory.Status))
            {
                string message = Res.Get("Category") + " " + Res.Get("Does_Not_Exist").ToLower();
                return Request.CreateResponse(HttpStatusCode.NotFound, message);
            }

            var objProductCategory = productCategoryService.GetSingleById(productCategoryID);
            if (objProductCategory == null || (objProductCategory != null && !objProductCategory.Status))
            {
                string message = Res.Get("Product_Category") + " " + Res.Get("Does_Not_Exist").ToLower();
                return Request.CreateResponse(HttpStatusCode.NotFound, message);
            }

            List<Product> lstProductFromExcel = new List<Product>();
            List<ListError> lstListError = new List<ListError>();
            List<Product> lstProductDB = productService.GetAll().ToList();

            // băm file thành nhiều phần
            foreach (MultipartFileData fileData in result.FileData)
            {
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, Res.Get("File_Data_Not_Invalid"));
                }

                string fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"/"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                var fullPath = Path.Combine(root, fileName);
                File.Copy(fileData.LocalFileName, fullPath, true);

                try
                {
                    lstProductFromExcel = this.GetDataFromExcel(fullPath, lstProductDB, ref lstListError);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            if (lstListError.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, lstListError);
            }
            else
            {
                /* if (lstProductFromExcel.Count == 0)
                 {
                     return Request.CreateResponse(HttpStatusCode.NotAcceptable, Res.Get("Import_Excel_Success"));
                 }*/

                productService.InsertOrUpdateFromExcel(lstProductFromExcel, lstProductDB, objProductCategory.ProductCategoryID);
                productService.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK, Res.Get("Import_Excel_Success"));
        }

        private List<Product> GetDataFromExcel(string fullPath, List<Product> lstProductDB, ref List<ListError> lstListError)
        {
            ExcelPackage package = new ExcelPackage(new FileInfo(fullPath));
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];

            var listNSX = NSXService.GetAll().ToList();

            List<Product> lstProduct = new List<Product>();
            Product objNew = null;

            List<string> lstProductCode = new List<string>();
            List<string> lstProductName = new List<string>();
            string productCode = string.Empty;
            string productName = string.Empty;
            int quantity = 0;
            decimal priceInput = 0;
            decimal priceSell = 0;
            int warranty = 0;
            int endRow = sheet.Dimension.End.Row;

            string error = string.Empty;

            for (int i = 9; i <= endRow; i++)
            {
                // chỉ cho phép lưu tối đa 500 bản ghi
                if (i >= (500 + 9))
                    break;

                objNew = new Product();

                if ((sheet.Cells[i, 2].Value == null && sheet.Cells[i, 3].Value == null)
                    || sheet.Cells[i, 3].Value == null)
                {
                    break;
                }

                #region // Getdata

                #region // Mã sản phẩm

                if (sheet.Cells[i, 2].Value != null)
                {
                    productCode = sheet.Cells[i, 2].Value.ToString();
                    lstProductCode.Add(productCode);
                    if (lstProductCode.Where(x => x.ToUpper() == productCode.ToUpper()).ToList().Count() > 1)
                    {
                        // trùng mã sản phẩm
                        lstListError.Add(MassageError(productCode, "", "Mã sản phẩm bị trùng lặp"));
                    }
                    else
                    {
                        objNew.ProductCode = productCode;
                    }
                }

                #endregion // Mã sản phẩm

                #region // tên sản phẩm

                if (sheet.Cells[i, 3].Value != null)
                {
                    // tên sản phẩm
                    productName = sheet.Cells[i, 3].Value.ToString();
                    lstProductName.Add(productName);
                    if (lstProductName.Where(x => x.ToUpper() == productName.ToUpper()).ToList().Count() > 1)
                    {
                        // trùng tên sản phẩm
                        lstListError.Add(MassageError(productCode, productName, "Tên sản phẩm bị trùng lập"));
                    }
                    else
                    {
                        if (productName.Length > 100)
                        {
                            lstListError.Add(MassageError(productCode, productName, "Tên sản phẩm không được vượt quá 100 ký tự"));
                        }
                        else
                        {
                            objNew.ProductName = productName;
                        }
                    }
                }

                #endregion // tên sản phẩm

                #region // Số lượng

                if (sheet.Cells[i, 4].Value != null)
                {
                    if (Utils.IsInt32(sheet.Cells[i, 4].Value.ToString()))
                    {
                        Int32.TryParse(sheet.Cells[i, 4].Value.ToString(), out quantity);
                        if (quantity < 0 || quantity > 999)
                        {
                            error = "Số lượng không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999]";
                            lstListError.Add(MassageError(productCode, productName, error));
                        }
                        else
                        {
                            objNew.Quantity = quantity;
                        }
                        quantity = 0;
                    }
                    else
                    {
                        error = "Số lượng không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999]";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Số lượng

                #region // Giá nhập

                if (sheet.Cells[i, 5].Value != null)
                {
                    if (Utils.IsDecimal(sheet.Cells[i, 5].Value.ToString()))
                    {
                        Decimal.TryParse(sheet.Cells[i, 5].Value.ToString(), out priceInput);
                        if (priceInput < 0 || priceInput > 999999999)
                        {
                            error = "Giá nhập không hợp lệ.Giá trị là số nguyên dương trong khoảng[0 - 999,999,999]";
                            lstListError.Add(MassageError(productCode, productName, error));
                        }
                        else
                        {
                            objNew.PriceInput = priceInput;
                        }
                        priceInput = 0;
                    }
                    else
                    {
                        error = "Giá nhập không hợp lệ.Giá trị là số nguyên dương trong khoảng[0 - 999,999,999]";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Giá nhập

                #region // Giá bán

                if (sheet.Cells[i, 6].Value != null)
                {
                    if (Utils.IsDecimal(sheet.Cells[i, 6].Value.ToString()))
                    {
                        Decimal.TryParse(sheet.Cells[i, 6].Value.ToString(), out priceSell);
                        if (priceSell < 0 || priceSell > 999999999)
                        {
                            error = "Giá bán không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999,999]";
                            lstListError.Add(MassageError(productCode, productName, error));
                        }
                        else
                        {
                            objNew.PriceSell = priceSell;
                        }
                        priceSell = 0;
                    }
                    else
                    {
                        error = "Giá bán không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999,999]";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Giá bán

                #region // Nhà sản xuất

                if (sheet.Cells[i, 7].Value != null && sheet.Cells[i, 7].Value != "")
                {
                    string nxs = sheet.Cells[i, 7].Value.ToString();
                    var objNXS = listNSX.Where(x => x.ProducerName == nxs).FirstOrDefault();
                    if (objNXS != null)
                    {
                        objNew.ProducerID = objNXS.ProducerID;
                    }
                    else
                    {
                        error = string.Format("Nhà sản xuất [{0}] không tồn tại", nxs);
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Nhà sản xuất

                #region // Thời gian bảo hành

                if (sheet.Cells[i, 8].Value != null)
                {
                    if (Utils.IsInt32(sheet.Cells[i, 8].Value.ToString()))
                    {
                        Int32.TryParse(sheet.Cells[i, 8].Value.ToString(), out warranty);
                        if (warranty < 0 || warranty > 999)
                        {
                            error = "Thời gian bảo hành không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999]";
                            lstListError.Add(MassageError(productCode, productName, error));
                        }
                        else
                        {
                            objNew.Warranty = warranty;
                        }
                        warranty = 0;
                    }
                    else
                    {
                        error = "Thời gian bảo hành không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999]";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Thời gian bảo hành

                #region // Hiển thị website

                if (sheet.Cells[i, 9].Value != null && sheet.Cells[i, 9].Value != "")
                {
                    if ((sheet.Cells[i, 9].Value.ToString() == "X" || sheet.Cells[i, 9].Value.ToString() == "x")
                        && sheet.Cells[i, 9].Value.ToString().Length == 1)
                    {
                        objNew.ProductHomeFlag = true;
                    }
                    else
                    {
                        error = "Hiển thị ra Website. Có nhập X, không để trống";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }
                else
                {
                    objNew.ProductHomeFlag = false;
                }

                #endregion // Hiển thị website

                #region // Hàng nổi bật

                if (sheet.Cells[i, 10].Value != null && sheet.Cells[i, 10].Value != "")
                {
                    if ((sheet.Cells[i, 10].Value.ToString() == "X" || sheet.Cells[i, 10].Value.ToString() == "x")
                        && sheet.Cells[i, 10].Value.ToString().Length == 1)
                    {
                        objNew.ProductHotFlag = true;
                    }
                    else
                    {
                        error = "Hàng nổi bật. Có nhập X, không để trống";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }
                else
                {
                    objNew.ProductHotFlag = false;
                }

                #endregion // Hàng nổi bật

                #region // Hàng bán chạy

                if (sheet.Cells[i, 11].Value != null && sheet.Cells[i, 11].Value != "")
                {
                    if ((sheet.Cells[i, 11].Value.ToString() == "X" || sheet.Cells[i, 11].Value.ToString() == "x")
                        && sheet.Cells[i, 11].Value.ToString().Length == 1)
                    {
                        objNew.ProductSellingGood = true;
                    }
                    else
                    {
                        error = "Hàng bán chạy. Có nhập X, không để trống";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }
                else
                {
                    objNew.ProductSellingGood = false;
                }

                #endregion // Hàng bán chạy

                #region // Hàng mới

                if (sheet.Cells[i, 12].Value != null && sheet.Cells[i, 12].Value != "")
                {
                    if ((sheet.Cells[i, 12].Value.ToString() == "X" || sheet.Cells[i, 12].Value.ToString() == "x")
                        && sheet.Cells[i, 12].Value.ToString().Length == 1)
                    {
                        objNew.ProductNew = true;
                    }
                    else
                    {
                        error = "Hàng mới. Có nhập X, không để trống";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }
                else
                {
                    objNew.ProductNew = false;
                }

                #endregion // Hàng mới

                #region // Trạng thái

                if (sheet.Cells[i, 13].Value != null && sheet.Cells[i, 13].Value != "")
                {
                    if ((sheet.Cells[i, 13].Value.ToString() == "X" || sheet.Cells[i, 13].Value.ToString() == "x")
                        && sheet.Cells[i, 13].Value.ToString().Length == 1)
                    {
                        objNew.Status = true;
                    }
                    else
                    {
                        error = "Trạng thái. Kinh doanh nhập X, ngừng kinh doanh để trống";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }
                else
                {
                    objNew.Status = false;
                }

                #endregion // Trạng thái

                #region // Mô tả

                if (sheet.Cells[i, 14].Value != null)
                {
                    if (sheet.Cells[i, 14].Value.ToString().Length <= 500)
                    {
                        objNew.Description = sheet.Cells[i, 14].Value.ToString();
                    }
                    else
                    {
                        error = "Mô tả không được vượt quá 500 ký tự";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Mô tả

                #region // Thông tin mô tả

                if (sheet.Cells[i, 15].Value != null)
                {
                    if (sheet.Cells[i, 15].Value.ToString().Length <= 100)
                    {
                        objNew.MetaDescription = sheet.Cells[i, 15].Value.ToString();
                    }
                    else
                    {
                        error = "Thông tin mô tả không được vượt quá 100 ký tự";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Thông tin mô tả

                #region // Tags

                if (sheet.Cells[i, 16].Value != null)
                {
                    if (sheet.Cells[i, 16].Value.ToString().Length <= 100)
                    {
                        objNew.MetaKeyword = sheet.Cells[i, 16].Value.ToString();
                    }
                    else
                    {
                        error = "Tags không được vượt quá 100 ký tự";
                        lstListError.Add(MassageError(productCode, productName, error));
                    }
                }

                #endregion // Tags

                #endregion // Getdata

                productCode = string.Empty;
                productName = string.Empty;

                objNew.CreateBy = UserInfoInstance.UserCode;
                objNew.CreateDate = DateTime.Now;
                objNew.UpdateBy = UserInfoInstance.UserCode;
                objNew.UpdateDate = DateTime.Now;
                lstProduct.Add(objNew);
            }

            return lstProduct;
        }

        private ListError MassageError(string productCode, string productName, string description)
        {
            ListError obj = new ListError();
            obj.ProductCode = productCode;
            obj.ProductName = productName;
            obj.Description = description;
            return obj;
        }

        [Route("exportExcelNoTemplate")]
        [HttpGet]
        public async Task<HttpResponseMessage> ExportExcelNoTemplate(HttpRequestMessage request)
        {
            string fileName = string.Concat("Product_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var lstResult = productService.GetAll().ToList();
                await ReportHelper.GenerateXls(lstResult, fullPath);
                string path = Path.Combine(folderReport, fileName);

                return request.CreateErrorResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("exportExcel")]
        [HttpGet]
        public async Task<HttpResponseMessage> exportExcel(HttpRequestMessage request, string keyword, int? productCategoryId, int? statusID)
        {
            // Tên file
            string fileName = string.Format("DanhSachSanPham_{0}.xlsx", DateTime.Now.ToString("yyyyMMddhhmmsss"));
            // Nơi chứa file Report
            string filePath = HttpContext.Current.Server.MapPath(ParameterFileInfo.ReportFolder);
            // Nơi chứa file Template
            string fileTemplatePath = HttpContext.Current.Server.MapPath(ParameterFileInfo.TemplateFolder + "TemplateProduct.xlsx");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                FileInfo fileTemplate = new FileInfo(fileTemplatePath);
                FileInfo newFile = new FileInfo(fullPath);

                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Keyword", keyword);
                if (productCategoryId.HasValue)
                    dic.Add("ProductCategoryID", productCategoryId);
                if (statusID.HasValue)
                    dic.Add("Status", statusID);

                productService.ExportExcel(fullPath, fileTemplatePath, dic);

                string newfullPath = Path.Combine(ParameterFileInfo.ReportFolder, fileName);
                return request.CreateErrorResponse(HttpStatusCode.OK, newfullPath);
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("downloadTemplate")]
        [HttpGet]
        public HttpResponseMessage downloadTemplate(HttpRequestMessage request)
        {
            // Tên file
            string fileName = "FileMau_Import_DanhSachSanPham.xlsx";
            // Nơi chứa file Report
            string filePath = HttpContext.Current.Server.MapPath(ParameterFileInfo.ReportFolder);
            // Nơi chứa file Template
            string fileTemplatePath = HttpContext.Current.Server.MapPath(ParameterFileInfo.TemplateFolder + "TemplateProduct.xlsx");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                FileInfo fileTemplate = new FileInfo(fileTemplatePath);
                FileInfo newFile = new FileInfo(fullPath);

                productService.DownLoadTemplate(fullPath, fileTemplatePath);

                string newfullPath = Path.Combine(ParameterFileInfo.ReportFolder, fileName);
                return request.CreateErrorResponse(HttpStatusCode.OK, newfullPath);
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}