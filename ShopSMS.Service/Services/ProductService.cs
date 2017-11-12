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
        string AutoGenericCode();
        void ExportExcel(string fullPath, string fileTemplatePath, IDictionary<string, object> dic);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            this.productRepository = productRepository;
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

        public string AutoGenericCode()
        {
            return productRepository.AutoGenericCode();
        }

        public void ExportExcel(string fullPath, string fileTemplatePath, IDictionary<string, object> dic)
        {
            FileInfo fileTemplate = new FileInfo(fileTemplatePath);
            FileInfo newFile = new FileInfo(fullPath);

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
                    index++;
                    // STT
                    sheet.Cells[("A" + startRow)].Value = index;
                    // Mã SP
                    sheet.Cells[("B" + startRow)].Value = objProduct.ProductCode;
                    sheet.Cells[("C" + startRow)].Value = objProduct.ProductName;
                    sheet.Cells[("D" + startRow)].Value = objProduct.Quantity;
                    sheet.Cells[("E" + startRow)].Value = objProduct.PriceInput;
                    sheet.Cells[("F" + startRow)].Value = objProduct.PriceSell;
                    sheet.Cells[("G" + startRow)].Value = objProduct.Warranty;
                    sheet.Cells[("H" + startRow)].Value = objProduct.ProductHomeFlag.HasValue && objProduct.ProductHomeFlag == true ? "X" : "";
                    sheet.Cells[("I" + startRow)].Value = objProduct.ProductHotFlag.HasValue && objProduct.ProductHotFlag == true ? "X" : "";
                    sheet.Cells[("J" + startRow)].Value = objProduct.ProductSellingGood.HasValue && objProduct.ProductSellingGood == true ? "X" : "";
                    sheet.Cells[("K" + startRow)].Value = objProduct.ProductNew.HasValue && objProduct.ProductNew == true ? "X" : ""; ;
                    sheet.Cells[("L" + startRow)].Value = objProduct.Status ? "X" : "";
                    sheet.Cells[("M" + startRow)].Value = objProduct.Description;
                    sheet.Cells[("N" + startRow)].Value = objProduct.MetaDescription;
                    sheet.Cells[("O" + startRow)].Value = objProduct.MetaKeyword;

                    if (!objProduct.Status)
                    {
                        sheet.Cells[startRow, 1, startRow, 15].Style.Font.Strike = true;
                        sheet.Cells[startRow, 1, startRow, 15].Style.Font.Color.SetColor(Color.Red);
                        sheet.Cells[startRow, 1, startRow, 15].Style.Locked = true;
                    }
                    else {
                        sheet.Cells[startRow, 1, startRow, 15].Style.Locked = false;
                    }

                    sheet.Cells[startRow, 1, startRow, 15].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[startRow, 1, startRow, 15].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[startRow, 1, startRow, 15].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[startRow, 1, startRow, 15].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                   

                    startRow++;
                }

                sheet.Protection.SetPassword("123456");
                          
                // Lưu file mới
                pck.SaveAs(newFile);             
            }     
        }
    }
}