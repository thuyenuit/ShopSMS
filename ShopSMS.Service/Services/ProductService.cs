using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL.Repositories;
using ShopSMS.Model.Model;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.DataValidation.Contracts;
using ShopSMS.Common.Common;

namespace ShopSMS.Service.Services
{
    public interface IProductService
    {
        void Create(Product product);
        void Update(Product product);
        void Delete(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllPaging(int page, int pageSize, out int totalRow);
        Product GetSingleById(int id);
        IEnumerable<Product> Search(IDictionary<string, object> dic);
        void SaveChanges();
        string AutoGenericCode(int def, List<Product> listProduct);
        void ExportExcel(string fullPath, string fileTemplatePath, IDictionary<string, object> dic);
        void InsertOrUpdateFromExcel(List<Product> lstFromExcel, List<Product> lstProductDB, int ProductCategoryID);
        void DownLoadTemplate(string fullPath, string fileTemplatePath);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISupplierRepository supplierRepository;
        private readonly INSXService NSXService;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository,
            INSXService NSXService,
            IUnitOfWork unitOfWork)
        {
            this.productRepository = productRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.categoryRepository = categoryRepository;
            this.supplierRepository = supplierRepository;
            this.NSXService = NSXService;
            this.unitOfWork = unitOfWork;
        }

        public void Create(Product product)
        {
            productRepository.Create(product);
        }

        public void Delete(int id)
        {
            productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return productRepository.GetAll();
        }

        public IEnumerable<Product> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return productRepository.GetMultiPaging(x => x.Status == true, out totalRow, page, pageSize);
        }

        public Product GetSingleById(int id)
        {
            return productRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Product> Search(IDictionary<string, object> dic)
        {
            return productRepository.Search(dic);
        }

        public void Update(Product product)
        {
            productRepository.Update(product);
        }

        public string AutoGenericCode(int def, List<Product> listProduct)
        {
            return productRepository.AutoGenericCode(def, listProduct);
        }

        public void InsertOrUpdateFromExcel(List<Product> lstFromExcel, List<Product> lstProductDB, int productCategoryID)
        {        
            Product objCheckCode = null;
            int index = 0;
            for (int i = 0; i < lstFromExcel.Count(); i++)
            {
                var obj = lstFromExcel[i];
                obj.ProductCategoryID = productCategoryID;

                // Cập nhật
                if (!string.IsNullOrEmpty(obj.ProductCode))
                {
                    var objResult = lstProductDB.FirstOrDefault(x => x.ProductCode.ToUpper().Equals(obj.ProductCode.ToUpper()));
                    if (objResult != null && !string.IsNullOrEmpty(obj.ProductName))
                    {
                        objResult.ProductCategoryID = obj.ProductCategoryID;
                        objResult.ProductName = obj.ProductName;
                        objResult.Quantity = obj.Quantity;
                        objResult.PriceSell = obj.PriceSell;
                        objResult.PriceInput = obj.PriceInput;
                        objResult.Warranty = obj.Warranty;
                        objResult.ProductHomeFlag = obj.ProductHomeFlag;
                        objResult.ProductHotFlag = obj.ProductHotFlag;
                        objResult.ProductSellingGood = obj.ProductSellingGood;
                        objResult.ProductNew = obj.ProductNew;
                        objResult.Status = obj.Status;
                        objResult.Description = obj.Description;
                        objResult.MetaDescription = obj.MetaDescription;
                        objResult.MetaKeyword = obj.MetaKeyword;
                        objResult.UpdateDate = obj.UpdateDate;
                        objResult.UpdateBy = objResult.UpdateBy + ", " + obj.UpdateBy;

                        productRepository.Update(objResult);
                    }
                    else if(objResult == null)
                    {
                        productRepository.Create(obj);
                    }
                }
                else
                {
                    index++;
                    obj.ProductCode = AutoGenericCode(index, lstProductDB);
                    objCheckCode = lstProductDB.Where(x => x.ProductCode == obj.ProductCode).FirstOrDefault();

                    while (objCheckCode != null)
                    {
                        index++;
                        obj.ProductCode = AutoGenericCode(index, lstProductDB);
                        objCheckCode = lstProductDB.Where(x => x.ProductCode == obj.ProductCode).FirstOrDefault();
                    };

                    if (objCheckCode == null)
                    {
                        objCheckCode = lstFromExcel.Where(x=>x.ProductCode != null)
                                                    .Where(x => x.ProductCode == obj.ProductCode)
                                                    .FirstOrDefault();
                        while (objCheckCode != null)
                        {
                            index++;
                            obj.ProductCode = AutoGenericCode(index, lstProductDB);
                            objCheckCode = lstProductDB.Where(x => x.ProductCode == obj.ProductCode).FirstOrDefault();
                        };

                    }
                
                    productRepository.Create(obj);               
                }
            }
        }

        public void ExportExcel(string fullPath, string fileTemplatePath, IDictionary<string, object> dic)
        {
            FileInfo fileTemplate = new FileInfo(fileTemplatePath);
            FileInfo newFile = new FileInfo(fullPath);

            List<Producer> lstNSX = NSXService.GetAll().ToList();
            Producer objProducer = null;
            List<Product> lstResult = Search(dic).ToList();
            Product objProduct = null;
                      
            using (ExcelPackage pck = new ExcelPackage(newFile, fileTemplate))
            {
                // Lấy sheet thứ 1
                ExcelWorksheet sheet = pck.Workbook.Worksheets[1];



                int startRow = 9;
                int index = 0;
           
                for (int i = 0; i < lstResult.Count; i++)
                {
                    objProduct = lstResult[i];
                    if (objProduct.ProducerID.HasValue && objProduct.ProducerID.Value > 0)
                    {
                        objProducer = lstNSX.Where(x => x.ProducerID == objProduct.ProducerID).FirstOrDefault();
                    }
                    
                    index++;
                    // STT
                    sheet.SetValue(("A" + startRow), index);
                    // Mã SP
                    sheet.SetValue(("B" + startRow), objProduct.ProductCode);
                    sheet.SetValue(("C" + startRow), objProduct.ProductName);
                    sheet.SetValue(("D" + startRow), objProduct.Quantity);
                    sheet.SetValue(("E" + startRow), objProduct.PriceInput.ToString("N0"));
                    sheet.SetValue(("F" + startRow), objProduct.PriceSell.ToString("N0"));
                    sheet.SetValue(("G" + startRow), objProducer != null ? objProducer.ProducerName : "");
                    sheet.SetValue(("H" + startRow), objProduct.Warranty);
                    sheet.SetValue(("I" + startRow), objProduct.ProductHomeFlag.HasValue && objProduct.ProductHomeFlag == true ? "X" : "");
                    sheet.SetValue(("J" + startRow), objProduct.ProductHotFlag.HasValue && objProduct.ProductHotFlag == true ? "X" : "");
                    sheet.SetValue(("K" + startRow), objProduct.ProductSellingGood.HasValue && objProduct.ProductSellingGood == true ? "X" : "");
                    sheet.SetValue(("L" + startRow), objProduct.ProductNew.HasValue && objProduct.ProductNew == true ? "X" : "");
                    sheet.SetValue(("M" + startRow), objProduct.Status ? "X" : "");
                    sheet.SetValue(("N" + startRow), objProduct.Description);
                    sheet.SetValue(("O" + startRow), objProduct.MetaDescription);
                    sheet.SetValue(("P" + startRow), objProduct.MetaKeyword);

                    if (!objProduct.Status)
                    {
                        sheet.Cells[startRow, 1, startRow, 15].Style.Font.Strike = true;
                        sheet.Cells[startRow, 1, startRow, 15].Style.Font.Color.SetColor(Color.Red);
                        //sheet.Cells[startRow, 1, startRow, 15].Style.Locked = true;
                    }
                    else {
                        //sheet.Cells[startRow, 1, startRow, 15].Style.Locked = false;
                    }

                    objProducer = null;
                    startRow++;
                }

                List<string> lstNSXName = lstNSX.Select(x => x.ProducerName).ToList();
               
                sheet.ListValidationExcel("G", 9, 500, lstNSXName);
                sheet.RenderBorderAll(8, 1, 509, 16, ExcelBorderStyle.Thin);
                List<DataImageValidatation> lstDataImage = new List<DataImageValidatation>();
                lstDataImage.Add(new DataImageValidatation {PathFile = @"F:\Troll Pictures\Troll Pictures\44645_316025975209315_1696254844_n.jpg", Row =1, Column =2 });
                //lstDataImage.Add(new DataImageValidatation {PathFile = @"F:\Troll Pictures\Troll Pictures\294935_537302912984840_1891609347_n.jpg", Row = 2, Column = 2 });
                sheet.InsertPicture(lstDataImage);
                //InsertPicture(this ExcelWorksheet sheet, List < DataImageValidatation > lstDataImage)

                //sheet.Protection.SetPassword("123456");                        
                // Lưu file mới
                pck.SaveAs(newFile);             
            }     
        }

        public void DownLoadTemplate(string fullPath, string fileTemplatePath)
        {
            FileInfo fileTemplate = new FileInfo(fileTemplatePath);
            FileInfo newFile = new FileInfo(fullPath);

            using (ExcelPackage pck = new ExcelPackage(newFile, fileTemplate))
            {
                // Lấy sheet thứ 1
                ExcelWorksheet sheet = pck.Workbook.Worksheets[1];          

                List<string> lstNSXName = NSXService.GetAll().Select(x => x.ProducerName).ToList();

                if (lstNSXName.Count() > 0)
                {
                    sheet.ListValidationExcel("G", 9, 500, lstNSXName);
                }                        
                // Lưu file mới
                pck.SaveAs(newFile);
            }
        }
    }
}