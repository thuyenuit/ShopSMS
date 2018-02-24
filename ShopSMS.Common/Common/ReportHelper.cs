using OfficeOpenXml;
using OfficeOpenXml.DataValidation.Contracts;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Common.Common
{
    public static class ReportHelper
    {
        /// <summary>
        /// Hàm tạo combobox trong excel
        /// </summary>
        /// <param name="sheet">sheet cần xét</param>
        /// <param name="columnName">Tên cột cần fill</param>
        /// <param name="startRow">Hàng bắt đầu</param>
        /// <param name="totalRow">Tổng số hàng muốn fill</param>
        /// <param name="listValue">Danh sách dữ liệu cần fill</param>
        public static void ListValidationExcel(this ExcelWorksheet sheet, 
            string columnName, int startRow, int totalRow, List<string> listValue)
        {
            IExcelDataValidationList val = null;
            if (startRow > 0 && listValue.Count > 0)
            {
                for (int i = 0; i <= totalRow; i++)
                {
                    val = sheet.DataValidations.AddListValidation((columnName + (startRow + i)));
                    for (int j = 0; j < listValue.Count(); j++)
                    {
                        val.Formula.Values.Add(listValue[j]);
                    }
                    // val.ShowErrorMessage = true;
                    // val.Error = "Chỉ được phép chọn dữ liệu trong danh sách";
                }
            }
        }

        /// <summary>
        /// Hàm kẻ tất cả border trong excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endStart"></param>
        /// <param name="endColumn"></param>
        public static void RenderBorderAll(this ExcelWorksheet sheet, 
            int startRow, int startColumn, int endStart, int endColumn, ExcelBorderStyle style)
        {
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Top.Style = style;
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Left.Style = style;
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Right.Style = style;
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Bottom.Style = style;
        }

        /// <summary>
        ///  Hàm kẻ border top trong excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endStart"></param>
        /// <param name="endColumn"></param>
        /// <param name="style"></param>
        public static void RenderBorderTop(this ExcelWorksheet sheet, 
            int startRow, int startColumn, int endStart, int endColumn, ExcelBorderStyle style)
        {
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Top.Style = style;
        }

        /// <summary>
        /// Hàm kẻ border bottom trong excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endStart"></param>
        /// <param name="endColumn"></param>
        /// <param name="style"></param>
        public static void RenderBorderBottom(this ExcelWorksheet sheet, 
            int startRow, int startColumn, int endStart, int endColumn, ExcelBorderStyle style)
        {
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Bottom.Style = style;
        }

        /// <summary>
        /// Hàm kẻ border left trong excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endStart"></param>
        /// <param name="endColumn"></param>
        /// <param name="style"></param>
        public static void RenderBorderLeft(this ExcelWorksheet sheet, 
            int startRow, int startColumn, int endStart, int endColumn, ExcelBorderStyle style)
        {
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Left.Style = style;
        }

        /// <summary>
        /// Hàm kẻ border right trong excel
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endStart"></param>
        /// <param name="endColumn"></param>
        /// <param name="style"></param>
        public static void RenderBorderRight(this ExcelWorksheet sheet, 
            int startRow, int startColumn, int endStart, int endColumn, ExcelBorderStyle style)
        {
            sheet.Cells[startRow, startColumn, endStart, endColumn].Style.Border.Right.Style = style;
        }

        public static void InsertPicture(this ExcelWorksheet sheet, List<DataImageValidatation> lstDataImage)
        {
            FileInfo file = null;
            DataImageValidatation objData = null;
            for (int i = 0; i < lstDataImage.Count(); i++)
            {
                objData = lstDataImage[i];
                /*file = new FileInfo(objData.PathFile);
                if (objData.PathFile != null && objData.PathFile.Length > 0)
                {
                    var pic = sheet.Drawings.AddPicture("Anh" + i, file);
                    pic.SetPosition(i + 10, 0, 14, 0);
                    pic.SetSize(100);
                }*/

                Bitmap image = new Bitmap(objData.PathFile);
                if (image != null)
                {
                    var excelImage = sheet.Drawings.AddPicture("Debopam Pal", image);
                    excelImage.From.Column = 1;
                    excelImage.From.Row = 2;
                    excelImage.SetSize(100, 100);
                    // 2x2 px space for better alignment
                    excelImage.From.ColumnOff = Pixel2MTU(2);
                    excelImage.From.RowOff = Pixel2MTU(2);
                }
            }
           // return sheet;
        }


        public static int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }

        /*public static void SetValue(this ExcelWorksheet sheet, string column, string value)
        {
            sheet.Cells[column].Value = value;
        }*/

        /*public static void SetValue(this ExcelWorksheet sheet, 
            int startRow, int startColumn, int endRow, int endColumn, string value)
        {
            sheet.Cells[startRow, startColumn, endRow, endRow].Value = value;
        }*/



        public static Task GenerateXls<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    ws.Cells["A1"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }
    }
}
