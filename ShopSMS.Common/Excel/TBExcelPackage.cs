using OfficeOpenXml;
using OfficeOpenXml.DataValidation.Contracts;
using ShopSMS.Common.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ShopSMS.Common.Excel
{
    public interface ITBExcelPackage
    {
        ExcelWorksheet FillComboboxToExcel(ExcelWorksheet sheet, string columnName, int startRow, int totalRow, List<string> listValue);
    }
     
    public class TBExcelPackage : ITBExcelPackage
    {
        /// <summary>
        /// Hàm tạo combobox trong excel
        /// </summary>
        /// <param name="sheet">sheet cần xét</param>
        /// <param name="columnName">Tên cột cần fill</param>
        /// <param name="startRow">Hàng bắt đầu</param>
        /// <param name="totalRow">Tổng số hàng muốn fill</param>
        /// <param name="listValue">Danh sách dữ liệu cần fill</param>
        public ExcelWorksheet FillComboboxToExcel(ExcelWorksheet sheet, string columnName, int startRow, int totalRow, List<string> listValue)
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
                }
            }
            return sheet;
        }

        public ExcelWorksheet InsertPicture(ExcelWorksheet sheet, List<DataImageValidatation> lstDataImage)
        {
            FileInfo file = null;
            DataImageValidatation objData = null;
            for (int i = 0; i < lstDataImage.Count(); i++)
            {
                objData = lstDataImage[i];
                file = new FileInfo(objData.PathFile);
                /*if (objData.ByteImage != null && objData.ByteImage.Count() > 0)
                {
                    var pic = sheet.Drawings.AddPicture("Anh", file);
                    pic.SetPosition(i + 10, 0, 14, 0);
                    pic.SetSize(100);
                }*/
            }

            return sheet;
        }
    }
}
