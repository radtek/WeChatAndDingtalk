using Beisen.AppConnect.Infrastructure.Helper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beisen.AppConnect.ServiceImp.Service
{
    public class ExcelService
    {
        private static readonly ExcelService _Instance = new ExcelService();
        public static ExcelService Instance
        {
            get
            {
                return _Instance;
            }
        }
        public IWorkbook GetExcel(string path)
        {
            IWorkbook workbook = null;
            try
            {
                var fileExt = Path.GetExtension(path);
                var byteData = ExcelHelper.Read(path);
                MemoryStream file = new MemoryStream(byteData);
                if (fileExt == ".xls")
                {
                    workbook = new HSSFWorkbook(file);
                }
                else if (fileExt == ".xlsx")
                {
                    workbook = new NPOI.XSSF.UserModel.XSSFWorkbook(file);
                }
                return workbook;
            }
            catch (Exception e)
            {
                throw new Exception("解析excel文件失败！" + e.ToString());
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRow"></param>
        /// <returns></returns>
        public DataTable ExcelToDataTable(ISheet sheet, int startRow = 2)
        {
            var data = new DataTable();
            try
            {
                if (sheet != null)
                {
                    int cellCount = 2; //一行最后一个cell的编号 即总的列数
                    //给dataTable增加标题
                    for (int i = 0; i <= cellCount; i++)
                    {
                        var columnName = sheet.GetRow(1).GetCell(i).StringCellValue;
                        var column = new DataColumn(columnName);
                        data.Columns.Add(column);
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        DataRow dataRow = data.NewRow();
                        for (int j = 0; j <= cellCount; ++j)
                        {
                            if (row.GetCell(j) != null)
                            {
                                //同理，没有数据的单元格都默认是null
                                dataRow[j] = Convert.ToString(row.GetCell(j)).Trim();
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                    RemoveLastEmptyRow(data);
                }
                else
                    throw new Exception("指定Sheet页不存在");
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
        private DataTable RemoveLastEmptyRow(DataTable source)
        {
            if (source.Rows.Count == 0)
                return source;
            DataRow dr = source.Rows[source.Rows.Count - 1];
            foreach (var cell in dr.ItemArray)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(cell)))
                {
                    return source;
                }
            }
            source.Rows.Remove(dr);
            return source;
        }
    }
}
