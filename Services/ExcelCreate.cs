using iTextSharp.text.pdf.qrcode;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class ExcelCreate
    {

        public byte[] CreateNewExcel(DataTable dt)
        {
            byte[] fileBytes = null;
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("sheet");


                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    ws.Cells[1, col + 1].Value = string.IsNullOrEmpty(dt.Columns[col].ColumnName) ? "" : dt.Columns[col].ColumnName.Replace("_", " ");

                    ws.Cells[1, col + 1].Style.Font.Bold = true;
                }


                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    for (int col = 0; col < dt.Columns.Count; col++)
                    {
                        ws.Cells[row + 2, col + 1].Value = dt.Rows[row][col];
                    }
                }

                ws.Cells.AutoFitColumns();

                fileBytes = package.GetAsByteArray();
                
            }
            return fileBytes;
        }
        public void CreateNewExcel(DataTable dtExcel, string strFilePath, string strHeader = "1")
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {

                    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dtExcel.TableName);
                    workSheet.Cells["A1"].LoadFromDataTable(dtExcel, true);
                    if (strHeader == "0")// if 0 then excel will noy have any header
                    {
                        workSheet.DeleteRow(1);
                    }
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void CreateNewExcelDataSet(DataSet dsExcel, string strFilePath, string strHeader = "1")
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    foreach (DataTable dtExcel in dsExcel.Tables)
                    {
                        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dtExcel.TableName);
                        workSheet.Cells["A1"].LoadFromDataTable(dtExcel, true);
                        if (strHeader == "0")// if 0 then excel will noy have any header
                        {
                            workSheet.DeleteRow(1);
                        }
                    }

                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void CreateRewardNewExcel(DataTable dtExcel, string strFilePath, string strHeader)
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {

                    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dtExcel.TableName);
                    workSheet.Cells["A1"].LoadFromDataTable(dtExcel, true);
                    if (strHeader == "0")// if 0 then excel will noy have any header
                    {
                        workSheet.DeleteRow(1);
                    }
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void CreateC360Excel(DataSet dtset, string strFilePath, string strHeader = "1")
        {

            try
            {
                //string[] arroworksheet = { ""};
                string strMobile = "";
                string strPortal = "";
                string strtalisma = "";
                string strMemo = "";
                string strWhatsapp = "";
                string strCallcenter = "";
                DataTable dtAll = new DataTable();
                int count = 0;
                int array = 0;
               
                using (ExcelPackage pck = new ExcelPackage())
                {
                    string[] arroworksheet = { "Mobile", "Portal", "Talisma", "V+ Memo", "Whatsapp", "Call Center" };
                    int loopCounter = 0;
                    foreach (DataTable dtExcel in dtset.Tables)
                    {
                        string strSheetName = arroworksheet[loopCounter];
                        if (dtExcel.Rows.Count > 0)
                        {
                            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(strSheetName);//(dtExcel.TableName);
                            //workSheet.Name = strSheetName;// arroworksheet[count];
                            //count++;
                            workSheet.Cells["A1"].LoadFromDataTable(dtExcel, true);
                            if (strHeader == "0")// if 0 then excel will noy have any header
                            {
                                workSheet.DeleteRow(1);
                            }
                        }
                        loopCounter++;
                    }
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void CreateTextFormatExcel(DataTable dtExcelData, string strFilePath, string strHeader = "1")
        {
            int RowsCount = dtExcelData.Rows.Count + 1;
            int ColumnsCount = dtExcelData.Columns.Count;
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dtExcelData.TableName);

                    using (ExcelRange rng = workSheet.Cells[0 + ":" + RowsCount])
                    {
                        rng.Style.Numberformat.Format = "@";
                    }
                    workSheet.Cells["A1"].LoadFromDataTable(dtExcelData, true);
                    if (strHeader == "0") //if 0 then excel will not have any header
                    {
                        workSheet.DeleteRow(1);
                    }
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void LMSCreateNewExcel(DataTable dtExcel, string strFilePath, string strHeader = "1")
        {
            try
            {
                int Counter = 0;
                DataTable dtFinal = new DataTable();
                int SheetCount = 1;

                using (ExcelPackage pck = new ExcelPackage())
                {

                    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    //ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dtExcel.TableName);
                    ExcelWorksheet workSheet = null;
                    if (dtExcel != null && dtExcel.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtExcel.Columns.Count; k++)
                        {
                            dtFinal.Columns.Add(dtExcel.Columns[k].ColumnName, typeof(string));
                        }
                    }

                    int i = 1;
                    int batchCOunter = 0;
                    i = 1;
                    batchCOunter = 0;
                    bool lastBatch = false;
                    int batchsize = 900000;

                    for (int j = 0; j < dtExcel.Rows.Count; j++)
                    {
                        lastBatch = false;
                        DataRow drowInput = dtFinal.Rows.Add();

                        for (int m = 0; m < dtExcel.Columns.Count; m++)
                        {
                            drowInput[m] = dtExcel.Rows[j][m].ToString();
                        }
                        batchCOunter++;

                        if (batchCOunter == batchsize)
                        {
                            workSheet = pck.Workbook.Worksheets.Add("Sheet" + SheetCount);
                            workSheet.Cells["A1"].LoadFromDataTable(dtFinal, true);
                            //pck.Workbook.Worksheets.Add("Sheet"+SheetCount);
                            SheetCount++;
                            batchCOunter = 0;

                            dtFinal.Rows.Clear();
                            dtFinal.Clear();
                        }
                        i++;
                    }

                    if (lastBatch == false && batchCOunter > 0)
                    {
                        workSheet = pck.Workbook.Worksheets.Add("Sheet" + SheetCount);
                        workSheet.Cells["A1"].LoadFromDataTable(dtFinal, true);
                        dtFinal.Rows.Clear();
                        dtFinal.Clear();
                    }


                    if (strHeader == "0")// if 0 then excel will noy have any header
                    {
                        workSheet.DeleteRow(1);
                    }
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void CreateTCPExcel(DataTable dtExcel, string strFilePath, string strHeader, MyAppSettings appSettings)
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {

                    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dtExcel.TableName);

                    for (int i = 0; i < dtExcel.Rows.Count; i++)
                    {

                        int strblk1ColPosition = dtExcel.Columns["ACC_BLK_CODE_1"].Ordinal + 1;  // fetching column position from datatable
                                                                                                 // int strblk1ColPosition1 = dtExcel.Columns.IndexOf("ACC_BLK_CODE_1");  // another way to fetching column position from datatable
                        int strblkdt1ColPosition = dtExcel.Columns["ACC_BLK_CODE_1_DT"].Ordinal + 1; // fetching column position from datatable
                        string strblk1Col = ExcelAddress.GetAddressCol(strblk1ColPosition).ToString(); // fetching column address from worksheet i.e A,B,C.....XFD
                        string strblkdt1Col = ExcelAddress.GetAddressCol(strblkdt1ColPosition).ToString(); // fetching column address from worksheet i.e A,B,C.....XFD


                        string strBlkCode1 = dtExcel.Rows[i]["ACC_BLK_CODE_1"].ToString(); // geting blkcode1 value from datatable(dtExcel)
                        string strBlkDate1 = dtExcel.Rows[i]["ACC_BLK_CODE_1_DT"].ToString(); // geting blkcodedate1 value from datatable(dtExcel)

                        if (strBlkCode1 == "C" || strBlkCode1 == "E")
                        {
                            int j = i + 2; // adding 1 because datatable count start from 0. adding another 1 because worksheet contains column header as well.
                            string strBlk1Col = strblk1Col + j;
                            string strBlkdt1Col = strblkdt1Col + j;

                            using (ExcelRange rng = workSheet.Cells[strBlk1Col])
                            {
                                rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow); // set background color for blkcode1

                            }
                            using (ExcelRange rng = workSheet.Cells[strBlkdt1Col])
                            {
                                rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow); //// set background color for blkcodedt1

                            }
                        }


                        int strblk2ColPosition = dtExcel.Columns["ACC_BLK_CODE_2"].Ordinal + 1;
                        int strblkdt2ColPosition = dtExcel.Columns["ACC_BLK_CODE_2_DT"].Ordinal + 1;
                        string strblk2Col = ExcelAddress.GetAddressCol(strblk2ColPosition).ToString();
                        string strblkdt2Col = ExcelAddress.GetAddressCol(strblkdt2ColPosition).ToString();


                        string strBlkCode2 = dtExcel.Rows[i]["ACC_BLK_CODE_2"].ToString();
                        string strBlkDate2 = dtExcel.Rows[i]["ACC_BLK_CODE_2_DT"].ToString();

                        if (strBlkCode2 == "C" || strBlkCode2 == "E")
                        {
                            int z = i + 2;
                            string strBlk2Col = strblk2Col + z;
                            string strBlkdt2Col = strblkdt2Col + z;

                            using (ExcelRange rng = workSheet.Cells[strBlk2Col])
                            {
                                rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                            }
                            using (ExcelRange rng = workSheet.Cells[strBlkdt2Col])
                            {
                                rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                            }
                        }

                    }


                    workSheet.Cells["A1"].LoadFromDataTable(dtExcel, true);
                    if (strHeader == "0") //if 0 then excel will not have any header
                    {
                        workSheet.DeleteRow(1);
                    }
                    //workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateRBIReportExcel(DataTable dtExcel, string strFilePath, string strHeader = "0")
        {
            try
            {
                using (ExcelPackage pck = new ExcelPackage())
                {

                    System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(dtExcel.TableName);

                    int strMaxCol = dtExcel.Columns.Count;
                    int strMaxRow = dtExcel.Rows.Count;
                    int strExcelRow = 2;
                    string strLastColumn = ExcelAddress.GetAddressCol(strMaxCol).ToString();
                    string strLastRow = ExcelAddress.GetAddressCol(strMaxRow).ToString();

                    using (ExcelRange rng = workSheet.Cells["A1" + ":" + strLastColumn + "1"])
                    {
                        rng.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AliceBlue);
                        rng.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        rng.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    }

                    for (int i = 0; i < dtExcel.Rows.Count; i++)
                    {
                        string strTitle = dtExcel.Rows[i]["Q_TITLE"].ToString();
                        string strData = dtExcel.Rows[i]["Q_DATA"].ToString();
                        if (strTitle.Trim() == "Customer risk migration")
                        {
                            workSheet.Cells["A" + strExcelRow + ":" + strLastColumn + strExcelRow].Merge = true;
                            workSheet.Cells["A" + strExcelRow + ":" + strLastColumn + strExcelRow].Style.Font.Bold = true;
                            workSheet.Cells["A" + strExcelRow + ":" + strLastColumn + strExcelRow].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + strExcelRow + ":" + strLastColumn + strExcelRow].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(91, 155, 231));
                            workSheet.Cells["A" + strExcelRow + ":" + strLastColumn + strExcelRow].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + strExcelRow + ":" + strLastColumn + strExcelRow].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        }
                        if (strData.Trim() == "No. of Customers")
                        {
                            int u = i + 2;
                            workSheet.Cells["A" + u + ":" + strLastColumn + u].Merge = false;
                            workSheet.Cells["A" + u + ":" + strLastColumn + u].Style.Font.Bold = true;
                            workSheet.Cells["A" + u + ":" + strLastColumn + u].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + u + ":" + strLastColumn + u].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(91, 155, 231));
                            workSheet.Cells["A" + u + ":" + strLastColumn + u].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            workSheet.Cells["A" + u + ":" + strLastColumn + u].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        }

                        if (strTitle.Trim() == "What is the frequency of the periodic review of risk categorisation?")
                        {
                            int j = i + 2;
                            workSheet.Cells["A" + j + ":" + strLastColumn + j].Merge = false;
                            workSheet.Cells["A" + j + ":" + strLastColumn + j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + j + ":" + strLastColumn + j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(221, 235, 247));
                            workSheet.Cells["A" + j + ":" + strLastColumn + j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + j + ":" + strLastColumn + j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strData.Trim() == "Half-Yearly Count")
                        {
                            int c = i + 2;
                            workSheet.Cells["B" + c + ":" + strLastColumn + c].Merge = false;
                            workSheet.Cells["B" + c + ":" + strLastColumn + c].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["B" + c + ":" + strLastColumn + c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["B" + c + ":" + strLastColumn + c].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            workSheet.Cells["B" + c + ":" + strLastColumn + c].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        }

                        if (strTitle.Trim() == "No. of customers whose risk classification was upgraded")
                        {
                            int k = i + 2;
                            workSheet.Cells["A" + k + ":" + strLastColumn + k].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + k + ":" + strLastColumn + k].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                            workSheet.Cells["A" + k + ":" + strLastColumn + k].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + k + ":" + strLastColumn + k].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "High To Medium")
                        {
                            int o = i + 2;
                            workSheet.Cells["A" + o + ":" + strLastColumn + o].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + o + ":" + strLastColumn + o].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + o + ":" + strLastColumn + o].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + o + ":" + strLastColumn + o].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        if (strTitle.Trim() == "High To Low")
                        {
                            int p = i + 2;
                            workSheet.Cells["A" + p + ":" + strLastColumn + p].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + p + ":" + strLastColumn + p].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + p + ":" + strLastColumn + p].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + p + ":" + strLastColumn + p].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        if (strTitle.Trim() == "Medium To Low")
                        {
                            int rr = i + 2;
                            workSheet.Cells["A" + rr + ":" + strLastColumn + rr].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + rr + ":" + strLastColumn + rr].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + rr + ":" + strLastColumn + rr].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + rr + ":" + strLastColumn + rr].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "No. of customers whose risk classification was Downgraded")
                        {
                            int r = i + 2;
                            workSheet.Cells["A" + r + ":" + strLastColumn + r].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + r + ":" + strLastColumn + r].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                            workSheet.Cells["A" + r + ":" + strLastColumn + r].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + r + ":" + strLastColumn + r].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "Low To Medium")
                        {
                            int ss = i + 2;
                            workSheet.Cells["A" + ss + ":" + strLastColumn + ss].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + ss + ":" + strLastColumn + ss].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + ss + ":" + strLastColumn + ss].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + ss + ":" + strLastColumn + ss].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        if (strTitle.Trim() == "Low to High")
                        {
                            int t = i + 2;
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        if (strTitle.Trim() == "Medium To High")
                        {
                            int q = i + 2;
                            workSheet.Cells["A" + q + ":" + strLastColumn + q].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + q + ":" + strLastColumn + q].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + q + ":" + strLastColumn + q].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + q + ":" + strLastColumn + q].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "No. of customers whose risk classification was Unchanged")
                        {
                            int s = i + 2;
                            workSheet.Cells["A" + s + ":" + strLastColumn + s].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + s + ":" + strLastColumn + s].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                            workSheet.Cells["A" + s + ":" + strLastColumn + s].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + s + ":" + strLastColumn + s].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "Unchanged Medium")
                        {
                            int qq = i + 2;
                            workSheet.Cells["A" + qq + ":" + strLastColumn + qq].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + qq + ":" + strLastColumn + qq].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + qq + ":" + strLastColumn + qq].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + qq + ":" + strLastColumn + qq].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "Unchanged High")
                        {
                            int zz = i + 2;
                            workSheet.Cells["A" + zz + ":" + strLastColumn + zz].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + zz + ":" + strLastColumn + zz].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + zz + ":" + strLastColumn + zz].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + zz + ":" + strLastColumn + zz].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "Unchanged Low")
                        {
                            int yy = i + 2;
                            workSheet.Cells["A" + yy + ":" + strLastColumn + yy].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + yy + ":" + strLastColumn + yy].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            workSheet.Cells["A" + yy + ":" + strLastColumn + yy].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + yy + ":" + strLastColumn + yy].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        if (strTitle.Trim() == "Total number of customers on Quarter End")
                        {
                            int t = i + 2;
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 230, 153));
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells["A" + t + ":" + strLastColumn + t].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        if (strTitle.Trim() == "Politically Exposed Persons(PEP)")
                        {
                            if (strData == strData)
                            {
                                int g = i + 2;
                                workSheet.Cells["A" + g + ":" + strLastColumn + g].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                workSheet.Cells["A" + g + ":" + strLastColumn + g].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(221, 235, 247));
                                workSheet.Cells["A" + g + ":" + strLastColumn + g].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                workSheet.Cells["A" + g + ":" + strLastColumn + g].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                workSheet.Cells["A" + g + ":" + strLastColumn + g].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            }

                        }

                        if (strTitle.Trim() == "Politically Exposed Persons(PEP)**")
                        {
                            if (strData == strData)
                            {
                                int h = i + 2;
                                workSheet.Cells["A" + h + ":" + strLastColumn + h].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                workSheet.Cells["A" + h + ":" + strLastColumn + h].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(221, 235, 247));
                                workSheet.Cells["A" + h + ":" + strLastColumn + h].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                workSheet.Cells["A" + h + ":" + strLastColumn + h].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                workSheet.Cells["A" + h + ":" + strLastColumn + h].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }

                        }

                        int m = i + 2;
                        // workSheet.Cells["A" + m + ":" + strLastColumn + m].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        if (i < 16 && i >= 17)
                        {

                            workSheet.Cells["A" + m + ":" + strLastColumn + m].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                        }
                        else if (i >= 18)
                        {
                            workSheet.Cells["A" + m + ":" + strLastColumn + m].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


                        }
                        else
                        {
                            workSheet.Cells["A" + m + ":" + strLastColumn + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        }


                        //// workSheet.Cells["A" + m + ":" + strLastColumn + m].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                    }
                    for (int i = 0; i < strMaxCol; i++)
                    {
                        int n = i + 1;
                        int p = strMaxRow + 1;
                        string strColumn = ExcelAddress.GetAddressCol(n).ToString();
                        workSheet.Cells[strColumn + "0:" + strColumn + p].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    }
                    for (int i = 0; i < strMaxCol; i++)
                    {
                        int n = i + 1;
                        int p = 18 + 1;
                        string strColumn = ExcelAddress.GetAddressCol(n).ToString();
                        workSheet.Cells[strColumn + "18:" + strColumn + p].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
                        workSheet.Cells[strColumn + "18:" + strColumn + p].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);

                    }

                    if (strHeader == "0")// if 0 then excel will noy have any header
                    {
                        workSheet.DeleteRow(1);
                    }

                    workSheet.Cells["A1"].LoadFromDataTable(dtExcel, false);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void CreateRBICSVFile(DataTable dtExcel, string strFilePath)
        {
            try
            {
                int strTotalCount = dtExcel.Rows.Count;
                StringBuilder sb = new StringBuilder();
                string[] columsNames = dtExcel.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();

                sb.AppendLine(string.Join(",", columsNames));

                foreach (DataRow row in dtExcel.Rows)
                {
                    string[] Fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    sb.AppendLine(string.Join(",", Fields));
                }
                File.WriteAllText(strFilePath, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateCCAExcel(DataSet dtset, string strFilePath, string strHeader = "1")
        {

            try
            {
                DataTable dtAll = new DataTable();

                using (ExcelPackage pck = new ExcelPackage())
                {
                    string[] arroworksheet = { "VKYC", "E-Sign", "Document Initiated", "Incomplete Journey" };
                    int loopCounter = 0;
                    foreach (DataTable dtExcel in dtset.Tables)
                    {
                        string strSheetName = arroworksheet[loopCounter];
                        if (dtExcel.Rows.Count > 0)
                        {
                            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            ExcelWorksheet workSheet = pck.Workbook.Worksheets.Add(strSheetName);//(dtExcel.TableName);
                            //workSheet.Name = strSheetName;// arroworksheet[count];
                            //count++;
                            workSheet.Cells["A1"].LoadFromDataTable(dtExcel, true);
                            if (strHeader == "0")// if 0 then excel will noy have any header
                            {
                                workSheet.DeleteRow(1);
                            }
                        }
                        loopCounter++;
                    }
                    pck.SaveAs(new FileInfo(strFilePath));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
