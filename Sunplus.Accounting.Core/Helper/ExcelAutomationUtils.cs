using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Helper
{
    /// <summary>
    /// Summary description for ExcelAutomationUtils
    /// </summary>
    public class ExcelAutomationUtils
    {
        #region ColorCode

        public enum ColorCode
        {
            White = 2,
            Black = 1,
            Red = 3,
            Green = 4,
            Blue = 5,
            Grey = 16
        }

        #endregion

        #region CleanExcelProcess

        public void CleanExcelProcess()
        {
            try
            {
                Process[] processesByName = Process.GetProcessesByName("EXCEL");
                for (int i = 0; i < processesByName.Length; i++)
                {
                    Process.GetProcessById(processesByName[i].Id).Kill();
                }
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }

        #endregion

        #region OpenWorksheetFromTemplate

        public Workbook OpenWorksheetFromTemplate(string path, ref Worksheet ws, string sheetName)
        {
            Workbook excelWorkbook;
            try
            {
                Application excelApp = new Application();
                excelWorkbook = excelApp.Workbooks.Open(path,
                                                        0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0,
                                                        true, false, false);
                ws = (Worksheet)excelWorkbook.Sheets[sheetName];
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw;
            }
            return excelWorkbook;
        }

        #endregion

        #region SetValueRangeStyle

        public Range SetValueRangeStyle(Worksheet ws, int firstRow, int firstColumn, int lastRow, int lastColumn,
                                        object value, bool isBold, ColorCode fontColorCode, ColorCode bgColorCode,
                                        XlHAlign xlHAlign, XlVAlign xlVAlign, bool isMerge)
        {
            Range range = ws.get_Range(ws.Cells[firstRow, firstColumn], ws.Cells[lastRow, lastColumn]);
            range.MergeCells = isMerge;
            range.Value2 = value;
            range.Font.Bold = isBold;
            range.Font.ColorIndex = fontColorCode;
            range.Interior.ColorIndex = bgColorCode;
            range.HorizontalAlignment = xlHAlign;
            range.VerticalAlignment = xlVAlign;
            range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, "0");

            return range;
        }

        #endregion

        #region SetValueAtCellStyle

        public Range SetValueAtCellStyle(Worksheet ws, int rowIndex, int columnIndex, object value, bool isBold,
                                         ColorCode fontColorCode, ColorCode bgColorCode, XlHAlign xlHAlign,
                                         XlVAlign xlVAlign)
        {
            var range = (Range)ws.Cells[rowIndex, columnIndex];
            range.Value2 = value;
            range.Font.Bold = isBold;
            range.Font.ColorIndex = fontColorCode;
            range.Interior.ColorIndex = bgColorCode;
            range.HorizontalAlignment = xlHAlign;
            range.VerticalAlignment = xlVAlign;
            range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, "0");
            return range;
        }

        #endregion

        #region SetValueAtCellStyleNumber

        public Range SetValueAtCellStyleNumber(Worksheet ws, int rowIndex, int columnIndex, object value, bool isBold,
                                               ColorCode fontColorCode, ColorCode bgColorCode, XlHAlign xlHAlign,
                                               XlVAlign xlVAlign, string symbol)
        {
            var range = (Range)ws.Cells[rowIndex, columnIndex];
            range.Value2 = value;
            range.Font.Bold = isBold;
            range.Font.ColorIndex = fontColorCode;
            range.Interior.ColorIndex = bgColorCode;
            range.HorizontalAlignment = xlHAlign;
            range.VerticalAlignment = xlVAlign;
            range.NumberFormat = "#,##0.00" + symbol + "_);(#,##0.00)" + symbol;
            range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, "0");
            return range;
        }

        #endregion
    }
}
