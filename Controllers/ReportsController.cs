using PIMS.DataAccess;
using PIMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;
using System.Data;


namespace PIMS.Controllers
{
    public class ReportsController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        #region Index
        //
        // GET: /Reports/

        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Export Data
        public ActionResult ExportData()
        {
            //GridView gv = new GridView();
            //gv.DataSource = db.Studentrecord.ToList();
            //gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("StudentDetails");
        }
        #endregion

        #region Report: List Of Suppliers report of usage

        [Authorize(Roles = "SuperUser")]
        public ActionResult ReportForSupplier(string StartDate, string EndDate, string Company, string VatCharged)
        {
            DateTime startDate = DateTime.Today.AddYears(-1);
            DateTime endDate = DateTime.Today;
            if (StartDate != null)
            { startDate = DateTime.Parse(StartDate); }

            if (EndDate != null)
            {
                endDate = DateTime.Parse(EndDate);
            }

            if (startDate != null && endDate != null && !string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(VatCharged))
            {
                IQueryable<PhotocopierDetails> photocopyPerCo = (from pd in db.PhotocopierDetails
                                                                 where (pd.Company.Contains(Company)) &&
                                                                 (pd.Obsolete == false)
                                                                 select pd);
                var usageAndPriceReport = new List<ReportDetails>();
                var runningTotalOfPrice = 0F;
                var runningTotalOfUsage = 0F;
                var runningTotalOfVat = 0F;
                var Vat = float.Parse(VatCharged);
                foreach (var p in photocopyPerCo)
                {
                    IQueryable<MeterReading> meterReadingsAsc = (from mr in db.MeterReading
                                                                 where (mr.PhotocopierId == p.PhotocopierId)
                                                                 && (mr.DateOfReading >= startDate)
                                                                 && (mr.DateOfReading <= endDate)
                                                                 orderby mr.DateOfReading ascending
                                                                 select mr);
                    if (meterReadingsAsc.Count() != 0)
                    {
                        int firstMeterReading = meterReadingsAsc.FirstOrDefault().Reading;

                        IQueryable<MeterReading> meterReadingsDsc = (from mr in db.MeterReading
                                                                     where (mr.PhotocopierId == p.PhotocopierId)
                                                                      && (mr.DateOfReading >= startDate)
                                                                        && (mr.DateOfReading <= endDate)
                                                                     orderby mr.DateOfReading descending
                                                                     select mr);

                        int latestMeterReading = meterReadingsDsc.FirstOrDefault().Reading;
                        ReportDetails reportDetail = new ReportDetails();


                        var totalUsage = latestMeterReading - firstMeterReading;
                        var totalPrice = p.PricePerCopy * totalUsage;
                        Vat = (Vat / 100 * 1) * totalPrice;
                        float totalPriceWithVat = Vat + totalPrice;
                        runningTotalOfPrice = totalPrice + reportDetail.RunningTotalOfPrice;
                        runningTotalOfUsage = totalUsage + reportDetail.RunningTotalOfUsage;
                        runningTotalOfVat = totalPriceWithVat + reportDetail.RunningTotalOfVat;
                        reportDetail.TotalUsage = totalUsage.ToString();
                        reportDetail.TotalPrice = totalPrice.ToString();
                        reportDetail.TotalPriceWithVat = totalPriceWithVat.ToString();
                        reportDetail.SerialNumber = p.SerialNumber;
                        reportDetail.AreaLocation = p.AreaLocation;
                        reportDetail.Make = p.Make;
                        reportDetail.Model = p.Model;
                        reportDetail.RunningTotalOfPrice = runningTotalOfPrice;
                        reportDetail.RunningTotalOfUsage = runningTotalOfUsage;
                        reportDetail.RunningTotalOfVat = runningTotalOfVat;


                        usageAndPriceReport.Add(reportDetail);

                        var usageTotal = 0F;
                        double priceTotal = 0F;
                        double vatTotal = 0F;

                        foreach (var item in usageAndPriceReport)
                        {
                            usageTotal += item.RunningTotalOfUsage;
                            priceTotal += item.RunningTotalOfPrice;
                            vatTotal += item.RunningTotalOfVat;

                        }

                        Convert.ToDecimal(priceTotal);
                        priceTotal.ToString("c2"); // 2dp currenc
                        priceTotal = Math.Round(priceTotal, 2);

                        Convert.ToDecimal(vatTotal);
                        vatTotal.ToString("c2"); // 2dp currenc
                        vatTotal = Math.Round(vatTotal, 2);

                        ViewBag.UsageTotal = usageTotal;
                        ViewBag.PriceTotal = priceTotal;
                        ViewBag.VatTotal = vatTotal;


                    }
                }

                return View(usageAndPriceReport);
            }
            else
            {
                TempData["Error"] = "Please fill in all Fields";
            }

             return View();
        }

        [Authorize(Roles = "SuperUser")]
        public ActionResult ExportReportForSupplier(string StartDate, string EndDate, string Company, string VatCharged)
        {
            DateTime startDate = DateTime.Today.AddYears(-1);
            DateTime endDate = DateTime.Today;
            if (StartDate != null)
            { startDate = DateTime.Parse(StartDate); }

            if (EndDate != null)
            {
                endDate = DateTime.Parse(EndDate);
            }

            if (startDate != null && endDate != null && !string.IsNullOrEmpty(Company))
            {
                IQueryable<PhotocopierDetails> photocopyPerCo = (from pd in db.PhotocopierDetails
                                                                 where (pd.Company.Contains(Company)) &&
                                                                 (pd.Obsolete == false)
                                                                 select pd);

                var usageAndPriceReport = new List<ReportDetails>();
                var runningTotalOfPrice = 0F;
                var runningTotalOfUsage = 0F;
                var runningTotalOfVat = 0F;
                var Vat = float.Parse(VatCharged);
                foreach (var p in photocopyPerCo)
                {
                    IQueryable<MeterReading> meterReadingsAsc = (from mr in db.MeterReading
                                                                 where (mr.PhotocopierId == p.PhotocopierId)
                                                                 && (mr.DateOfReading >= startDate)
                                                                 && (mr.DateOfReading <= endDate)
                                                                 orderby mr.DateOfReading ascending
                                                                 select mr);
                    if (meterReadingsAsc.Count() != 0)
                    {
                        int firstMeterReading = meterReadingsAsc.FirstOrDefault().Reading;

                        IQueryable<MeterReading> meterReadingsDsc = (from mr in db.MeterReading
                                                                     where (mr.PhotocopierId == p.PhotocopierId)
                                                                      && (mr.DateOfReading >= startDate)
                                                              && (mr.DateOfReading <= endDate)
                                                                     orderby mr.DateOfReading descending
                                                                     select mr);
                        int latestMeterReading = meterReadingsDsc.FirstOrDefault().Reading;
                        ReportDetails reportDetail = new ReportDetails();

                        var totalUsage = latestMeterReading - firstMeterReading;
                        var totalPrice = p.PricePerCopy * totalUsage;
                        Vat = (Vat / 100 * 1) * totalPrice;
                        var totalPriceWithVat = Vat + totalPrice;
                        runningTotalOfPrice = totalPrice + reportDetail.RunningTotalOfPrice;
                        runningTotalOfUsage = totalUsage + reportDetail.RunningTotalOfUsage;
                        runningTotalOfVat = totalPriceWithVat + reportDetail.RunningTotalOfVat;
                        reportDetail.TotalUsage = totalUsage.ToString();
                        reportDetail.TotalPrice = totalPrice.ToString();
                        reportDetail.TotalPriceWithVat = totalPriceWithVat.ToString();
                        reportDetail.SerialNumber = p.SerialNumber;
                        reportDetail.AreaLocation = p.AreaLocation;
                        reportDetail.Make = p.Make;
                        reportDetail.Model = p.Model;
                        reportDetail.RunningTotalOfPrice = runningTotalOfPrice;
                        reportDetail.RunningTotalOfUsage = runningTotalOfUsage;
                        reportDetail.RunningTotalOfVat = runningTotalOfVat;

                        usageAndPriceReport.Add(reportDetail);
                    }
                }

                var usageTotal = 0F;
                double priceTotal = 0F;
                double vatTotal = 0F;
                DataTable dt = new DataTable();
                dt.Columns.Add("Serial Number");
                dt.Columns.Add("Area / Location");
                dt.Columns.Add("Make");
                dt.Columns.Add("Model");
                dt.Columns.Add("Total Usage");
                dt.Columns.Add("Total Cost");
                dt.Columns.Add("Total Cost With VAT");

                

                foreach (var item in usageAndPriceReport)
                {
                    usageTotal += item.RunningTotalOfUsage;
                    priceTotal += item.RunningTotalOfPrice;
                    vatTotal += item.RunningTotalOfVat;
                    DataRow dr = dt.NewRow();
                    dr["Serial Number"] = item.SerialNumber;
                    dr["Area / Location"] = item.AreaLocation;
                    dr["Make"] = item.Make;
                    dr["Model"] = item.Model ;
                    dr["Total Usage"] = item.TotalUsage ;
                    dr["Total Cost"] = item.TotalPrice ;
                    dr["Total Cost With VAT"] = item.TotalPriceWithVat;
                    dt.Rows.Add(dr);
                }

                Convert.ToDecimal(priceTotal);
                priceTotal.ToString("c2"); // 2dp currenc
                priceTotal = Math.Round(priceTotal, 2);

                Convert.ToDecimal(vatTotal);
                vatTotal.ToString("c2"); // 2dp currenc
                vatTotal = Math.Round(vatTotal, 2);

                DataRow usageRow = dt.NewRow();
                usageRow["Serial Number"] = "Totals:";
                usageRow["Area / Location"] = "";
                usageRow["Make"] = "";
                usageRow["Model"] = "";
                usageRow["Total Usage"] = usageTotal;
                usageRow["Total Cost"] = priceTotal;
                usageRow["Total Cost With VAT"] = vatTotal ;
                dt.Rows.Add(usageRow);

                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SupplierReportFor_ " + Company + startDate.ToShortDateString() + endDate.ToShortDateString() + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View(usageAndPriceReport);
            }

            return View();
        }


        public PhotocopierDetails ValidateCompany(PhotocopierDetails pd)
        {
                if (pd.Company == null)
                {
                    TempData["ErrorCostCentre"] = "The cost centre must be Entered";
                }
                return pd;
            
            
        }
        #endregion

        #region Report: List of Photocopiers Over 5 Years Old Still In Use

        [Authorize(Roles = "Administrator")]
        public ActionResult PhotocopierOver5YearsOld()
        {
            DateTime fiveYearsAgo = DateTime.Today.AddYears(-5);
            IQueryable<PhotocopierDetails> photocopiersOverFiveYearsOld = (from pd in db.PhotocopierDetails
                                                                           where (pd.DateOfSupply < fiveYearsAgo)
                                                                           && (pd.Obsolete == false)
                                                                           select pd);
            return View(photocopiersOverFiveYearsOld);
        }
       

        public ActionResult ExportPhotocopiersOver5YearsOld()
        {
            DateTime fiveYearsAgo = DateTime.Today.AddYears(-5);
            IQueryable<PhotocopierDetails> photocopiersOverFiveYearsOld = (from pd in db.PhotocopierDetails
                                                                           where (pd.DateOfSupply < fiveYearsAgo)
                                                                           && (pd.Obsolete == false)
                                                                           select pd);
            GridView gv = new GridView();
            gv.DataSource = photocopiersOverFiveYearsOld.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=PhotocopiersOver5YearsOld_ " + DateTime.Today.ToShortDateString() + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View(photocopiersOverFiveYearsOld);
        }
        #endregion

        #region Report: List Of Photocopiers Added Between Dates

        [Authorize(Roles = "SuperUser")]
        public ActionResult PhotocopiersAddedBetweenDates(string StartDate, string EndDate, string Company)
        {
            DateTime startDate = DateTime.Today.AddYears(-1);
            DateTime endDate = DateTime.Today;
            if (StartDate != null)
            { startDate = DateTime.Parse(StartDate); }
            
            if (EndDate != null)
            {
                endDate = DateTime.Parse(EndDate);
            }
            if (startDate != null && endDate != null && !string.IsNullOrEmpty(Company) )
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply >= startDate)
                                                                            && (pd.DateOfSupply <= endDate)
                                                                            && (pd.Company.Contains(Company))
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);


                return View(photocopiersAddedThisYear);
            }
            if (startDate != null && endDate != null)
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply >= startDate)
                                                                            && (pd.DateOfSupply <= endDate)
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);
                return View(photocopiersAddedThisYear);
            }


            if (startDate != null && endDate == null)
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply > startDate)
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);
      
                return View(photocopiersAddedThisYear);
                
                
            }

            if (startDate == null && endDate != null)
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply < endDate)
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);

                return View(photocopiersAddedThisYear);
            }
//removed when no dates entered as these details are available elsewhere
            return View();
        }

        public ActionResult ExportPhotocopiersAddedBetweenDates(string StartDate, string EndDate, string Company)
        {
            DateTime startDate = DateTime.Today.AddYears(-1);
            DateTime endDate = DateTime.Today;
            if (StartDate != null)
            { startDate = DateTime.Parse(StartDate); }

            if (EndDate != null)
            {
                endDate = DateTime.Parse(EndDate);
            }
            if (startDate != null && endDate != null && !string.IsNullOrEmpty(Company))
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply >= startDate)
                                                                            && (pd.DateOfSupply <= endDate)
                                                                            && (pd.Company.Contains(Company))
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);
                GridView gv = new GridView();
                gv.DataSource = photocopiersAddedThisYear.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=PhotocopiersAddedBetween_ " + startDate.ToShortDateString() + endDate.ToShortDateString() + Company + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View(photocopiersAddedThisYear);
            }
            if (startDate != null && endDate != null)
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply >= startDate)
                                                                            && (pd.DateOfSupply <= endDate)
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);

                GridView gv = new GridView();
                gv.DataSource = photocopiersAddedThisYear.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=PhotocopiersAddedBetween_ " + startDate.ToShortDateString() + endDate.ToShortDateString() + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View(photocopiersAddedThisYear);
            }


            if (startDate != null && endDate == null)
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply > startDate)
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);
                GridView gv = new GridView();
                gv.DataSource = photocopiersAddedThisYear.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename==PhotocopiersAddedBetween_ " + startDate.ToShortDateString() + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View(photocopiersAddedThisYear);
            }

            if (startDate == null && endDate != null)
            {
                IQueryable<PhotocopierDetails> photocopiersAddedThisYear = (from pd in db.PhotocopierDetails
                                                                            where (pd.DateOfSupply < endDate)
                                                                            orderby pd.DateOfSupply descending
                                                                            select pd);
                GridView gv = new GridView();
                gv.DataSource = photocopiersAddedThisYear.ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename==PhotocopiersAddedBetween_ " + endDate.ToShortDateString() + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return View(photocopiersAddedThisYear);

            }
            //removed when no dates entered as these details are available elsewhere
            return View();
        }
        #endregion

        #region Report: List of PhotocopiersWithoutSerialNo

        [Authorize(Roles = "Administrator")]
        public ActionResult PhotocopiersWithoutSerialNo()
        {
            var blank = ' ';
            var blankString = blank.ToString();
            IQueryable<PhotocopierDetails> photocopiersWithoutSerialNo = (from pd in db.PhotocopierDetails
                                                                           where (pd.SerialNumber == blankString)
                                                                           select pd);
            return View(photocopiersWithoutSerialNo);
        }
        
        public ActionResult ExportPhotocopiersWithoutSerialNo()
        {
            var blank = ' ';
            var blankString = blank.ToString();
            IQueryable<PhotocopierDetails> photocopiersWithoutSerialNo = (from pd in db.PhotocopierDetails
                                                                          where (pd.SerialNumber == blankString)
                                                                          select pd);
            


            GridView gv = new GridView();
            gv.DataSource = photocopiersWithoutSerialNo.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=NoSerialNumbers-" + DateTime.Today.ToShortDateString() + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View(photocopiersWithoutSerialNo);
        }
        #endregion

        #region Report: List of PhotocopiersWithoutLastReading

        [Authorize(Roles = "Administrator")]
        public ActionResult PhotocopiersWithoutLastReading()
        {
            var blank = ' ';
            var blankString = blank.ToString();
            IQueryable<PhotocopierDetails> photocopiersWithoutLastReading = (from pd in db.PhotocopierDetails
                                                                          where (pd.LastReading == 0)
                                                                          select pd);

            return View(photocopiersWithoutLastReading);
        }

        public ActionResult ExportPhotocopiersWithoutLastReading()
        {
            var blank = ' ';
            var blankString = blank.ToString();
            IQueryable<PhotocopierDetails> photocopiersWithoutLastReading = (from pd in db.PhotocopierDetails
                                                                             where (pd.LastReading == 0)
                                                                             select pd);
            GridView gv = new GridView();
            gv.DataSource = photocopiersWithoutLastReading.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=PhotocopierWithoutLastReading_ " + DateTime.Today.ToShortDateString() + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View(photocopiersWithoutLastReading);
        }
        #endregion

        #region Report: List of PhotocopiersThatAreObsolete

        [Authorize(Roles = "Administrator")]
        public ActionResult PhotocopiersThatAreObsolete()
        {
            
            IQueryable<PhotocopierDetails> photocopiersThatAreObsolete = (from pd in db.PhotocopierDetails
                                                                          where (pd.Obsolete == true)
                                                                          select pd);
            return View(photocopiersThatAreObsolete);
        }

        public ActionResult ExportPhotocopiersThatAreObsolete()
        {
            IQueryable<PhotocopierDetails> photocopiersThatAreObsolete = (from pd in db.PhotocopierDetails
                                                                          where (pd.Obsolete == true)
                                                                          select pd);

            
            GridView gv = new GridView();
            gv.DataSource = photocopiersThatAreObsolete.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ObsoleteMachines_" + DateTime.Today.ToShortDateString() + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View(photocopiersThatAreObsolete);
        }
        #endregion
    }
}
