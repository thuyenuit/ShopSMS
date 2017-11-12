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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        IProductService productService;
        INSXService NSXService;
        IProductCategoryService productCategoryService;
        public ProductController(IErrorLogService errorLogService,
            IProductService productService,
            INSXService NSXService,
            IProductCategoryService productCategoryService) : base(errorLogService)
        {
            this.productService = productService;
            this.NSXService = NSXService;
            this.productCategoryService = productCategoryService;
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
                List<Producer> lstProducer = NSXService.GetAll().ToList();
                List<ProductCategory> lstProCategory = productCategoryService.GetAll().ToList();

                IDictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("KeyWord", keyWord);
                if(categoryID.HasValue)
                    dic.Add("CategoryID", categoryID);
                if(status.HasValue)
                    dic.Add("Status", status);

                List<Product> lstProduct = productService.Search(dic).ToList();

                List<ProductViewModel> lstResponse = lstProduct
                .Select(x => new ProductViewModel
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
                    ProductCategoryName = "(Chưa có)",
                    ProducerName = "(Chưa có)" // x.ProducerID.HasValue ? (lstProducer.FirstOrDefault(p => p.ProducerID == x.ProducerID) != null ? lstProducer.FirstOrDefault(p => p.ProducerID == x.ProducerID).ProducerName : string.Empty) : string.Empty,
                }).ToList();

                foreach (var item in lstResponse)
                {
                    if (item.ProducerID.HasValue)
                    {
                        var objProducer = lstProducer.FirstOrDefault(p => p.ProducerID == item.ProducerID);
                        if (objProducer != null)
                            item.ProducerName = objProducer.ProducerName;
                    }

                    if (item.ProductCategoryID.HasValue)
                    {
                        var objProCategory = lstProCategory.FirstOrDefault(p => p.ProductCategoryID == item.ProductCategoryID);
                        if (objProCategory != null)
                            item.ProductCategoryName = objProCategory.ProductCategoryName;
                    }
                }

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

        [Route("importExcel")]
        [HttpPost]
        public async Task<HttpResponseMessage> ImportExcel()
        {
            // HttpResponseMessage response = null;
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

            int categoryId = 0;
            int.TryParse(result.FormData["categoryId"], out categoryId);

            // băm file thành nhiều phần
            foreach (MultipartFileData fileData in result.FileData)
            {
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "File dữ liệu không hợp lệ");
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

                var lstProduct = this.GetDataFromExcel(fullPath);

            }

            return Request.CreateResponse(HttpStatusCode.OK, "Import thành công!");
        }

        private List<Product> GetDataFromExcel(string fullPath)
        {
            List<Product> lstProduct = new List<Product>();
            Product objNew = null;

            List<ListError> lstListError = new List<ListError>();
            ListError objError = null;

            ExcelPackage package = new ExcelPackage(new FileInfo(fullPath));
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];

            List<string> lstProductCode = new List<string>();
            List<string> lstProductName = new List<string>();
            string productCode = string.Empty;
            string productName = string.Empty;
            int quantity = 0;
            decimal priceInput = 0;
            decimal priceSell = 0;

            int endRow = sheet.Dimension.End.Row;

            for (int i = 9; i <= endRow; i++)
            {
                objNew = new Product();

                if (sheet.Cells[i, 2].Value == null && sheet.Cells[i, 3].Value == null)
                {
                    continue;
                }

                #region // mã sản phẩm
                if (sheet.Cells[i, 2].Value != null)
                {
                    // mã sản phẩm
                    productCode = sheet.Cells[i, 2].Value.ToString();
                    lstProductCode.Add(productCode);
                    if (lstProductCode.Where(x => x.ToUpper() == productCode.ToUpper()).ToList().Count() > 1)
                    {
                        // trùng mã sản phẩm
                        objError = new ListError();
                        objError.ProductCode = productCode;
                        objError.Description = "Mã sản phẩm trong file bị trùng lập";
                        lstListError.Add(objError);
                    }
                    else
                        objNew.ProductCode = productCode;
                }
                #endregion

                #region // tên sản phẩm
                if (sheet.Cells[i, 3].Value != null)
                {
                    // tên sản phẩm
                    productName = sheet.Cells[i, 3].Value.ToString();
                    lstProductName.Add(productName);
                    if (lstProductCode.Where(x => x.ToUpper() == productCode.ToUpper()).ToList().Count() > 1)
                    {
                        // trùng tên sản phẩm
                        objError = new ListError();
                        objError.ProductCode = productCode;
                        objError.Description = "Tên sản phẩm trong file bị trùng lập";
                        objError.ProductName = productName;
                        lstListError.Add(objError);
                    }
                    else
                        objNew.ProductName = productCode;
                }
                #endregion

                #region // Số lượng
                if (sheet.Cells[i, 4].Value != null)
                {
                    if (Utils.IsInt32(sheet.Cells[i, 4].Value.ToString()))
                    {
                        Int32.TryParse(sheet.Cells[i, 4].Value.ToString(), out quantity);
                        if (quantity < 0 || quantity > 999999)
                        {
                            // Không phải kiểu Int
                            objError = new ListError();
                            objError.ProductCode = productCode;
                            objError.Description = "Số lượng không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999]";
                            lstListError.Add(objError);
                        }
                        else
                        {
                            objNew.Quantity = quantity;
                        }
                        quantity = 0;
                    }
                    else
                    {
                        // Không phải kiểu Int
                        objError = new ListError();
                        objError.ProductCode = productCode;
                        objError.Description = "Số lượng không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999]";
                        lstListError.Add(objError);
                    }
                }
                #endregion

                #region // Giá nhập
                if (sheet.Cells[i, 5].Value != null)
                {
                    if (Utils.IsDecimal(sheet.Cells[i, 5].Value.ToString()))
                    {
                        Decimal.TryParse(sheet.Cells[i, 5].Value.ToString(), out priceInput);
                        if (priceInput < 0 || priceInput > 9999999999999999)
                        {
                            objError = new ListError();
                            objError.ProductCode = productCode;
                            objError.ProductName = productName;
                            objError.Description = "Giá nhập không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999,9999,999,999]";
                            lstListError.Add(objError);
                        }
                        else
                        {
                            objNew.PriceInput = priceInput;
                        }
                        priceInput = 0;
                    }
                    else
                    {
                        // Không phải kiểu Decimal
                        objError = new ListError();
                        objError.ProductCode = productCode;
                        objError.ProductName = productName;
                        objError.Description = "Giá nhập không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999,9999,999,999]";
                        lstListError.Add(objError);
                    }
                }
                #endregion

                #region // Giá bán
                if (sheet.Cells[i, 6].Value != null)
                {
                    if (Utils.IsDecimal(sheet.Cells[i, 6].Value.ToString()))
                    {
                        Decimal.TryParse(sheet.Cells[i, 6].Value.ToString(), out priceSell);
                        if (priceSell < 0 || priceSell > 9999999999999999)
                        {
                            objError = new ListError();
                            objError.ProductCode = productCode;
                            objError.ProductName = productName;
                            objError.Description = "Giá bán không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999,9999,999,999]";
                            lstListError.Add(objError);
                        }
                        else
                        {
                            objNew.PriceSell = priceSell;
                        }
                        priceSell = 0;
                    }
                    else
                    {
                        // Không phải kiểu Decimal
                        objError = new ListError();
                        objError.ProductCode = productCode;
                        objError.ProductName = productName;
                        objError.Description = "Giá bán không hợp lệ. Giá trị là số nguyên dương trong khoảng [0 - 999,999,9999,999,999]";
                        lstListError.Add(objError);
                    }
                }
                #endregion

                lstProduct.Add(objNew);
            }

            return lstProduct;
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
            string fileTemplatePath = HttpContext.Current.Server.MapPath(ParameterFileInfo.TemplateFolder +  "TemplateProduct.xlsx");

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
                if(productCategoryId.HasValue)
                    dic.Add("ProductCategoryID", productCategoryId);
                if(statusID.HasValue)
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
             
    }
}
