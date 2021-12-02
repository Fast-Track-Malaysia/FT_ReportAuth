using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExpress.XtraReports.UI;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Drawing;

namespace FT_ReportAuth.Helpers
{
    public class ExportHelper
    {
        public readonly string pdf = "pdf";
        public readonly string xlsx = "xlsx";
        public readonly string docx = "docx";
        public async Task<byte[]> ExportResult(LoadResult lr, string format)
        {
            XtraReport report = new XtraReport();
            var loadedData = lr;
            //report.DataSource = loadedData.data.Cast<Products>();
            CreateReport(report, new string[] { "ProductName", "CategoryId", "UnitPrice", "UnitsInStock", "UnitsOnOrder", "Discontinued" });
            return await new TaskFactory().StartNew(() => {
                report.CreateDocument();
                using (MemoryStream fs = new MemoryStream())
                {
                    if (format == pdf)
                        report.ExportToPdf(fs);
                    else if (format == xlsx)
                        report.ExportToXlsx(fs);
                    else if (format == docx)
                        report.ExportToDocx(fs);
                    return fs.ToArray();
                }
            });
        }
        public async Task<byte[]> ExportResultDataSet(DataSet loadedData, string format)
        {
            XtraReport report = new XtraReport();

            string[] cols = new string[loadedData.Tables[0].Columns.Count];
            for (int i = 0; i < loadedData.Tables[0].Columns.Count; ++i)
            {
                cols[i] = loadedData.Tables[0].Columns[i].ColumnName;
            }

            report.DataSource = loadedData.Tables[0];
            CreateReport(report, cols);
            return await new TaskFactory().StartNew(() => {
                report.CreateDocument();
                using (MemoryStream fs = new MemoryStream())
                {
                    if (format == pdf)
                        report.ExportToPdf(fs);
                    else if (format == xlsx)
                        report.ExportToXlsx(fs);
                    else if (format == docx)
                        report.ExportToDocx(fs);
                    return fs.ToArray();
                }
            });
        }
        public void CreateReport(XtraReport report, string[] fields)
        {
            PageHeaderBand pageHeader = new PageHeaderBand() { HeightF = 23, Name = "pageHeaderBand" };
            int tableWidth = report.PageWidth - report.Margins.Left - report.Margins.Right;
            XRTable headerTable = XRTable.CreateTable(
                                new Rectangle(0,    // rect X
                                                0,          // rect Y
                                                tableWidth, // width
                                                40),        // height
                                                1,          // table row count
                                                0);         // table column count
            headerTable.Borders = DevExpress.XtraPrinting.BorderSide.All;
            headerTable.BackColor = Color.Gainsboro;
            headerTable.Font = new Font("Verdana", 10, FontStyle.Bold);
            headerTable.Rows.FirstRow.Width = tableWidth;
            headerTable.BeginInit();
            foreach (string field in fields)
            {
                XRTableCell cell = new XRTableCell();
                cell.Width = 100;
                cell.Text = field;
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                headerTable.Rows.FirstRow.Cells.Add(cell);
            }
            headerTable.EndInit();
            headerTable.AdjustSize();
            pageHeader.Controls.Add(headerTable);



            DetailBand detail = new DetailBand() { HeightF = 23, Name = "detailBand" };
            XRTable detailTable = XRTable.CreateTable(
                            new Rectangle(0,    // rect X
                                            0,          // rect Y
                                            tableWidth, // width
                                            40),        // height
                                            1,          // table row count
                                            0);         // table column count



            detailTable.Width = tableWidth;
            detailTable.Rows.FirstRow.Width = tableWidth;
            detailTable.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
            detailTable.BeginInit();
            foreach (string field in fields)
            {
                XRTableCell cell = new XRTableCell();
                ExpressionBinding binding = new ExpressionBinding("BeforePrint", "Text", String.Format("[{0}]", field));
                cell.ExpressionBindings.Add(binding);
                cell.Width = 100;
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                if (field.Contains("Date"))
                    cell.TextFormatString = "{0:MM/dd/yyyy}";
                detailTable.Rows.FirstRow.Cells.Add(cell);
            }
            detailTable.Font = new Font("Verdana", 8F);
            detailTable.EndInit();
            detailTable.AdjustSize();
            detail.Controls.Add(detailTable);
            report.Bands.AddRange(new Band[] { detail, pageHeader });
        }
    }
}
