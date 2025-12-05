using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class ExcelRead
    {
        public DataTable ProgrammingFilesEPPLUSExcelRead(string path, bool hasHeader = true)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }
                    var ws = pck.Workbook.Worksheets.First();
                    DataTable tbl = new DataTable();
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(firstRowCell.Text, typeof(string));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, tbl.Columns.Count];//ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text.ToString();
                        }
                    }
                    ws.Dispose();
                    pck.Dispose();
                    tbl = tbl.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field =>
                                      field is System.DBNull || string.Compare((field as string).Trim(),
                                      string.Empty) == 0)).CopyToDataTable();
                    return tbl;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
