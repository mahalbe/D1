using CrystalDecisions.CrystalReports.Engine;
using ISas.Entities.CommonEntities;
using ISas.Entities.FeesEntities;
using ISas.Repository.DashboardRepository.IRepository;
using ISas.Repository.Interface;
using ISas.Web.Models;
using PdfSharp;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Configuration;
using ISas.Repository.FeeModuleRepo.IRepository;
using System.Collections.Generic;
using paytm;
using Paytm;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using ISas.Repository.FeeModuleRepo.Repository;
using System.Web;



namespace ISas.Web.Controllers.Dashboard
{
    [Authorize]
    [ExceptionHandler]
    public class StudentFeeDetailsController : Controller
    {

        
        private IStudentFeeDetailsRepo _studentFeeDetailsRepo;
        private IFee_PaymentGatewayMasterRepo _paymentGatewayMasterRepo;
        private ICommonRepo _commonRepo;
        string merchantId;
        string Merchant_KEY;
        string responseURL;
        string transactionURL;
        string statusAPIURL;
        //      string userId;
        string _freereciptmode = ConfigurationManager.AppSettings["FeeReceiptMode"];
        string _OnlinePaymentURL = ConfigurationManager.AppSettings["onlinePaymentURL"];
        string studentid = "";
        onlinePayment _onlinePayment = new onlinePayment();
        

        public StudentFeeDetailsController(IStudentFeeDetailsRepo feeDetails, ICommonRepo commonRepo, IFee_PaymentGatewayMasterRepo paymentGatewayMasterRepo)
        {

            this._studentFeeDetailsRepo = feeDetails;
            _commonRepo = commonRepo;
            this._paymentGatewayMasterRepo = paymentGatewayMasterRepo;
            Fee_PaymentGatewayMasterModel model = _paymentGatewayMasterRepo.PaymentGatewayMasterTranasction("FEE", "GetPaymentGateway");
            merchantId = model.merchantId;
            Merchant_KEY = model.merchant_KEY;
            responseURL = model.responseURL;
            transactionURL = model.transactionURL;
            statusAPIURL = model.statusAPIURL;
            DataTable dt = new DataTable();
            dt = _paymentGatewayMasterRepo.GetPaymentGateway("FEE");
            if (dt.Rows.Count > 0)
            {
                _onlinePayment.paymentGateWay.gatewayName = dt.Rows[0]["gatewayName"].ToString();
                _onlinePayment.paymentGateWay.merchantId = dt.Rows[0]["merchantId"].ToString();
                _onlinePayment.paymentGateWay.merchant_KEY = dt.Rows[0]["merchant_KEY"].ToString();
                _onlinePayment.paymentGateWay.responseURL = dt.Rows[0]["responseURL"].ToString();
                _onlinePayment.paymentGateWay.transactionURL = dt.Rows[0]["transactionURL"].ToString();
                _onlinePayment.paymentGateWay.encodedURL = dt.Rows[0]["encodedURL"].ToString();
                _onlinePayment.paymentGateWay.statusAPIURL = dt.Rows[0]["statusAPIURL"].ToString();
                _onlinePayment.paymentGateWay.bankId = dt.Rows[0]["bankId"].ToString();
                _onlinePayment.paymentGateWay.loginId = dt.Rows[0]["loginId"].ToString();
                _onlinePayment.paymentGateWay.loginPassword = dt.Rows[0]["loginPassword"].ToString();
                _onlinePayment.paymentGateWay.requestHashKey = dt.Rows[0]["requestHashKey"].ToString();
                _onlinePayment.paymentGateWay.responseHashKey = dt.Rows[0]["responseHashKey"].ToString();
                _onlinePayment.paymentGateWay.customerCode = dt.Rows[0]["customerCode"].ToString();
                _onlinePayment.paymentGateWay.clientCode = dt.Rows[0]["clientCode"].ToString();
                _onlinePayment.paymentGateWay.clientProductId = dt.Rows[0]["clientProductId"].ToString();
                _onlinePayment.paymentGateWay.requestIVSet = dt.Rows[0]["requestIVSet"].ToString();
                _onlinePayment.paymentGateWay.responseIVSet = dt.Rows[0]["responseIVSet"].ToString();
                _onlinePayment.paymentGateWay.aesEncryptRequestKey = dt.Rows[0]["aesEncryptRequestKey"].ToString();
                _onlinePayment.paymentGateWay.aesEncryptResponseKey = dt.Rows[0]["aesEncryptResponseKey"].ToString();
            }
        }

        //GET:/StudentFeeDetails/
        public ActionResult StudentFeeDetailsMainPage()
        {
            return View(_studentFeeDetailsRepo.GetFeeStatusDetailsList_StudDash(Session["LoginStudentERPNo"].ToString(), Convert.ToInt32(Session["SessionID"])));
        }

        public PartialViewResult _ReceiptDetails(string erpno)
        {
            return PartialView(this._studentFeeDetailsRepo.GetFeeStatusDetailsList_StudDash(erpno, Convert.ToInt32(Session["SessionID"])));
        }

        public PartialViewResult _StudentLedger(string erpno)
        {
            ReportHeaderEntities model = _commonRepo.ReportHeaderDetails(" ");
            if (System.IO.File.Exists(Server.MapPath("~/" + model.LogoURL + "")))
                ViewBag.ImageData = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("~/" + model.LogoURL + ""))));
            else
                ViewBag.ImageData = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(System.IO.File.ReadAllBytes(Server.MapPath("~/Images/System/loginBgleft.jpg")))); //defaultLogo.png

            ViewBag.HeaderInfo = model;
            return PartialView(this._studentFeeDetailsRepo.GetFeeStatusDetailsList_StudDash1(erpno, Convert.ToInt32(Session["SessionID"])));
        }

        public PartialViewResult _FeeDetails(string DueDate)
        {
            return PartialView(this._studentFeeDetailsRepo.GetFeeDetailsListByDueDate(Session["UserID"].ToString(), DueDate, "RECEIPT"));
        }

        [EncryptedActionParameter]
        public ActionResult BillingInfo(string ERPNo, string DueDate)
        {
            return View(_studentFeeDetailsRepo.GetFeeBillingInfo(ERPNo,DueDate, Session["SessionId"].ToString(),Session["UserId"].ToString()));
        }
        [EncryptedActionParameter]
        public ActionResult BillingInfo_V1(string ERPNo, string DueDate)
        {
            return View(_studentFeeDetailsRepo.GetFeeBillingInfo(ERPNo, DueDate, Session["SessionId"].ToString(), Session["UserId"].ToString()));
        }
        [EncryptedActionParameter]
        public ActionResult GetFeeDocument(string TransRefNo, string Mode, string erpno, string MonthName = "REC")
        {
            ReportDocument rd = GetREC_INV(TransRefNo, Mode, erpno, MonthName);
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "" + Mode.ToUpper() + "_" + MonthName.ToUpper() + ".pdf");
        }

        private ReportDocument GetREC_INV(string TransRefNo, string Mode, string erpno, string MonthName = "REC")
        {
            string reportName = Mode == "RECEIPT" ? "FeeDetailsReport.rpt" : Mode == "INVOICE" ? "FeeDetailsReport_Invoice.rpt" : "FeeBookReport.rpt";
            DataSet feeDetails = this._studentFeeDetailsRepo.GetFeeDetails_ForReport(erpno, TransRefNo, Mode, Convert.ToInt32(Session["SessionId"]));

            feeDetails.Tables[0].TableName = "ReportHeader";
            feeDetails.Tables[1].TableName = "StudentInformation";
            feeDetails.Tables[2].TableName = "Duration";
            feeDetails.Tables[3].TableName = "HeadDetails";
            if (Mode == "RECEIPT")
            {
                string imgPath = feeDetails.Tables[0].Rows[0][4].ToString();
                if (System.IO.File.Exists(Server.MapPath("~/" + imgPath + "")))
                    feeDetails.Tables[0].Rows[0][4] = Server.MapPath("~/" + imgPath + "");

                //feeDetails.Tables[4].TableName = "BalanceDetails";

                if (feeDetails.Tables.Count > 5)
                {
                    feeDetails.Tables[5].TableName = "ReportHeader1";
                    feeDetails.Tables[6].TableName = "StudentInformation1";
                    feeDetails.Tables[7].TableName = "Duration1";
                    feeDetails.Tables[8].TableName = "HeadDetails1";
                    feeDetails.Tables[9].TableName = "BalanceDetails1";
                }
            }
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), reportName));
            rd.SetDataSource(feeDetails);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            return rd;
        }

        [EncryptedActionParameter]
        public ActionResult GetFeeDocument_View_HtmlReport(string TransRefNo, string Mode, string erpno, string MonthName = "REC", string PrintType = "DEFAULT")
        {
            // string reportName = Mode == "RECEIPT" ? "FeeDetailsReport.rpt" : Mode == "INVOICE" ? "FeeDetailsReport_Invoice.rpt" : "FeeBookReport.rpt";
            DataSet ds = _studentFeeDetailsRepo.GetFeeDetails_ForReport(erpno, TransRefNo, Mode, Convert.ToInt32(Session["SessionId"]));
            Fee_ReportHtmlModel model = new Fee_ReportHtmlModel();

            if (_freereciptmode == "HTML")
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.ReportHeader.Header1 = ds.Tables[0].Rows[0][0].ToString();
                    model.ReportHeader.Header2 = ds.Tables[0].Rows[0][1].ToString();
                    model.ReportHeader.Header3 = ds.Tables[0].Rows[0][2].ToString();
                    model.ReportHeader.Header4 = ds.Tables[0].Rows[0][3].ToString();
                    model.ReportHeader.LogoURL = ds.Tables[0].Rows[0][4].ToString();
                    model.ReportHeader.ReportName = ds.Tables[0].Rows[0][5].ToString();
                }

                model.StudentInfo.ReceiptNo = ds.Tables[1].Rows[0][0].ToString();
                model.StudentInfo.ReceiptDate = ds.Tables[1].Rows[0][1].ToString();
                model.StudentInfo.ERP = ds.Tables[1].Rows[0][2].ToString();
                model.StudentInfo.DOA = ds.Tables[1].Rows[0][3].ToString();
                model.StudentInfo.AdmNo = ds.Tables[1].Rows[0][4].ToString();
                model.StudentInfo.RollNo = ds.Tables[1].Rows[0][5].ToString();
                model.StudentInfo.Student = ds.Tables[1].Rows[0][6].ToString();
                model.StudentInfo.Class = ds.Tables[1].Rows[0][7].ToString();
                model.StudentInfo.Gender = ds.Tables[1].Rows[0][8].ToString();
                model.StudentInfo.DOB = ds.Tables[1].Rows[0][9].ToString();
                model.StudentInfo.Father = ds.Tables[1].Rows[0][10].ToString();
                model.StudentInfo.Mother = ds.Tables[1].Rows[0][11].ToString();
                model.StudentInfo.Address = ds.Tables[1].Rows[0][12].ToString();
                model.StudentInfo.FMobileNo = ds.Tables[1].Rows[0][13].ToString();
                model.StudentInfo.MMobileNo = ds.Tables[1].Rows[0][14].ToString();
                model.StudentInfo.SMSNo = ds.Tables[1].Rows[0][15].ToString();
                model.StudentInfo.AlternateNumber = ds.Tables[1].Rows[0][16].ToString();
                //model.StudentInfo.TransRefNo = ds.Tables[1].Rows[0][17].ToString();
                //model.StudentInfo.TransMode = ds.Tables[1].Rows[0][18].ToString();
                //model.StudentInfo.TransBank = ds.Tables[1].Rows[0][19].ToString();
                //model.StudentInfo.TransBranch = ds.Tables[1].Rows[0][20].ToString();
                //model.StudentInfo.TransRefNo = ds.Tables[1].Rows[0][21].ToString();
                //model.StudentInfo.TransDate = ds.Tables[1].Rows[0][22].ToString();
                //model.StudentInfo.FeeType = ds.Tables[1].Rows[0][23].ToString();
                //model.StudentInfo.TransportRefNo = ds.Tables[1].Rows[0][24].ToString();
                //model.StudentInfo.ClassId = ds.Tables[1].Rows[0][25].ToString();
                //model.StudentInfo.ERPNo1 = ds.Tables[1].Rows[0][26].ToString();
                model.StudentInfo.PaymentMode = ds.Tables[1].Rows[0][17].ToString();
                model.StudentInfo.AmountInWord = ds.Tables[1].Rows[0][18].ToString();
                model.Duration = ds.Tables[1].Rows[0][19].ToString();
                // model.Duration = ds.Tables[2].Rows[0][0].ToString();

                model.HeadDetails = ds.Tables[2].AsEnumerable().Select(r => new HeadDetailsModel
                {
                    ERPNo = r.Field<string>("ERPNo"),
                    Due = r.Field<int>("Due"),
                    Duration = r.Field<string>("Duration"),
                    HeadName = r.Field<string>("HeadName"),
                    Paid = r.Field<int>("Paid"),
                    dueAmount = r.Field<int>("dueAmount"),
                    creditNote = r.Field<int>("creditNote"),
                    paid = r.Field<int>("paid"),
                    PrintOrder = r.Field<short>("PrintOrder"),
                }).ToList();

                if (ds.Tables.Count > 3)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["TotalDue"].ToString()))
                        model.TotalDue = Convert.ToInt32(ds.Tables[3].Rows[0]["TotalDue"]);

                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["ExemptedAmount"].ToString()))
                        model.Exempted = Convert.ToInt32(ds.Tables[3].Rows[0]["ExemptedAmount"]);

                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["AmounttoPay"].ToString()))
                        model.AmounttoPay = Convert.ToInt32(ds.Tables[3].Rows[0]["AmounttoPay"]);

                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["PaidAmount"].ToString()))
                        model.TotalPaid = Convert.ToInt32(ds.Tables[3].Rows[0]["PaidAmount"]);

                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["Balance"].ToString()))
                        model.Balance = Convert.ToInt32(ds.Tables[3].Rows[0]["Balance"]);

                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["Excess"].ToString()))
                        model.Excess = Convert.ToInt32(ds.Tables[3].Rows[0]["Excess"]);

                    model.ReceiptStatus = ds.Tables[3].Rows[0]["ReceiptStatus"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["Advance"].ToString()))
                        model.AdvanceAmount = Convert.ToInt32(ds.Tables[3].Rows[0]["Advance"]);

                    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["creditNote"].ToString()))
                        model.creditNote = Convert.ToInt32(ds.Tables[3].Rows[0]["creditNote"]);
                }

                ViewBag.PrintOnly = PrintType;

                return View(model);
            }

            else {

                ds.Tables[0].TableName = "ReportHeader";
                ds.Tables[1].TableName = "StudentInformation";
                ds.Tables[2].TableName = "HeadDetails";
                ds.Tables[3].TableName = "BalanceDetails";
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "NewFeeReceipt.rpt"));
                rd.SetDataSource(ds);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                GC.Collect();
                return File(stream, "application/pdf");
            }
            //if (ds.Tables.Count > 3)
            //{
            //    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0][0].ToString()))
            //        model.TotalDue = Convert.ToInt32(ds.Tables[3].Rows[0][0]);

            //    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0][1].ToString()))
            //        model.Exempted = Convert.ToInt32(ds.Tables[3].Rows[0][1]);

            //    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0][2].ToString()))
            //        model.AmounttoPay = Convert.ToInt32(ds.Tables[3].Rows[0][2]);

            //    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0][3].ToString()))
            //        model.TotalPaid = Convert.ToInt32(ds.Tables[3].Rows[0][3]);

            //    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0][4].ToString()))
            //        model.Balance = Convert.ToInt32(ds.Tables[3].Rows[0][4]);

            //    if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0][5].ToString()))
            //        model.Excess = Convert.ToInt32(ds.Tables[3].Rows[0][5]);

            //    model.ReceiptStatus = ds.Tables[3].Rows[0][6].ToString();
            //}
            //byte[] pdfContent = null;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    string html = CommonHelpers.RenderPartialToString(this, "GetFeeDocument_View_HtmlReport", model);
            //    var pdf = PdfGenerator.GeneratePdf(html, PageSize.A4);
            //    for (int i = 0; i < pdf.Pages.Count; i++)
            //        pdf.Pages[i].Orientation = PageOrientation.Landscape;

            //    pdf.Save(ms);
            //    pdfContent = ms.ToArray();
            //}
            //return File(pdfContent, "application/pdf");

            //if (pdfContent == null)
            //    return null;

            //var contentDispositionHeader = new System.Net.Mime.ContentDisposition
            //{
            //    Inline = true,
            //    FileName = "someFilename.pdf"
            //};
            //Response.Headers.Add("Content-Disposition", contentDispositionHeader.ToString());
            //return File(pdfContent, System.Net.Mime.MediaTypeNames.Application.Pdf);

            //if (PrintType == "DOWNLOAD")
            //{
            //    return new Rotativa.ViewAsPdf
            //    {
            //        FileName = "FeeReceipt_"+model.StudentInfo.ReceiptNo +".pdf",
            //        Model = model,
            //        PageSize = Rotativa.Options.Size.A4,
            //        PageOrientation = Rotativa.Options.Orientation.Landscape,
            //        IsGrayScale = false,
            //        // PageMargins = { Left = 1, Right = 1 }
            //    };
            //}
            //var actionPDF = new Rotativa.ViewAsPdf
            //{
            //    //FileName = "FeeReceipt.pdf",
            //    Model = model,
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    IsGrayScale = false,
            //    // PageMargins = { Left = 1, Right = 1 }
            //};
            //return actionPDF;
            //return File("", "application/pdf");
        }

        [EncryptedActionParameter]
        public ActionResult NodueReceipt(string erpno)
        {
            DataSet ds = _studentFeeDetailsRepo.NoDuesRecept(erpno, Session["SessionId"].ToString());
            Fee_ReportHtmlModel model = new Fee_ReportHtmlModel();
            ds.Tables[0].TableName = "ReportHeader";
            ds.Tables[1].TableName = "StudentInformation";
            ds.Tables[2].TableName = "HeadDetails";
            ds.Tables[3].TableName = "BalanceDetails";
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "NoDueReceipt.rpt"));
            rd.SetDataSource(ds);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf");
        }

        [EncryptedActionParameter]
        public ActionResult GetFeeDocument_FeeBook_HtmlReport(string TransRefNo, string Mode, string erpno, string MonthName = "REC", string PrintOnly = "NO")
        {
            DataSet ds = _studentFeeDetailsRepo.GetFeeDetails_ForReport(erpno, TransRefNo, Mode, Convert.ToInt32(Session["SessionId"]));
            Fee_ReportHtmlModel model = new Fee_ReportHtmlModel();

            model.ReportHeader.Header1 = ds.Tables[0].Rows[0][0].ToString();
            model.ReportHeader.Header2 = ds.Tables[0].Rows[0][1].ToString();
            model.ReportHeader.Header3 = ds.Tables[0].Rows[0][2].ToString();
            model.ReportHeader.Header4 = ds.Tables[0].Rows[0][3].ToString();
            model.ReportHeader.LogoURL = ds.Tables[0].Rows[0][4].ToString();
            //model.ReportHeader.ReportName = ds.Tables[0].Rows[0][5].ToString();

            if (ds.Tables[1].Rows.Count > 0)
            {
                model.StudentInfo.ERP = ds.Tables[1].Rows[0][0].ToString();
                model.StudentInfo.DOA = ds.Tables[1].Rows[0][1].ToString();
                model.StudentInfo.AdmNo = ds.Tables[1].Rows[0][2].ToString();
                model.StudentInfo.RollNo = ds.Tables[1].Rows[0][3].ToString();
                model.StudentInfo.Student = ds.Tables[1].Rows[0][4].ToString();
                model.StudentInfo.Class = ds.Tables[1].Rows[0][5].ToString();
                model.StudentInfo.Gender = ds.Tables[1].Rows[0][6].ToString();
                model.StudentInfo.DOB = ds.Tables[1].Rows[0][7].ToString();
                model.StudentInfo.Father = ds.Tables[1].Rows[0][8].ToString();
                model.StudentInfo.Mother = ds.Tables[1].Rows[0][9].ToString();
                model.StudentInfo.Address = ds.Tables[1].Rows[0][10].ToString();
                model.StudentInfo.FMobileNo = ds.Tables[1].Rows[0][11].ToString();
                model.StudentInfo.MMobileNo = ds.Tables[1].Rows[0][12].ToString();
                model.StudentInfo.SMSNo = ds.Tables[1].Rows[0][13].ToString();
                model.StudentInfo.AlternateNumber = ds.Tables[1].Rows[0][14].ToString();
            }
            if (ds.Tables[2].Rows.Count > 0)
                model.Duration = ds.Tables[2].Rows[0][0].ToString();

            model.HeadDetails = ds.Tables[3].AsEnumerable().Select(r => new HeadDetailsModel
            {
                HeadName = r.Field<string>("HeadName"),
                Due = r.Field<int>("Due"),
                Paid = r.Field<int>("Paid"),
                ReceiptNo = r.Field<string>("E6"),
                InvoiceNo = r.Field<string>("E5"),
                Period = r.Field<string>("E4"),
            }).ToList();
            ViewBag.PrintOnly = PrintOnly;
            return View(model);
        }


        //[EncryptedActionParameter]
        //public ActionResult Makepayment(string paymentMode, string erpno, decimal amount)
        //{


        //    Guid OrderID = Guid.NewGuid();
        //    string tnxId = "";
        //    tnxId = OrderID.ToString() + DateTime.Now.ToString("yyyymmddHHmmss");    //RandomString(50);// 
        //    Tuple<int, string> res = _paymentGatewayMasterRepo.GatewayTransaction_CRUD(tnxId, erpno, Session["UserId"].ToString(), amount, paymentMode,"Pending");
        //    //return Json(new { status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        //    return View();
        //}

        //public JsonResult MakeOnlinepayment(string paymentMode, string erpno, string amount)
        //{

        //    Guid OrderID = Guid.NewGuid();
        //    string tnxId = "";
        //    tnxId = OrderID.ToString() + DateTime.Now.ToString("yyyymmddHHmmss");    //RandomString(50);// 
        //    Tuple<int, string> res = _paymentGatewayMasterRepo.GatewayTransaction_CRUD(tnxId, erpno, Session["UserId"].ToString(), Convert.ToDecimal(amount), paymentMode, "Pending");
        //    return Json(new { transactionNo = tnxId, erpNumber = erpno, status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult MakeOnlinepayment(string paymentMode, string erpno, string amount)
        //{

        //    if (transactionURL == null)
        //    {
        //        return Json(new { status = "failed", Msg = "Payment gateway is not integrted. ", Color = "Warning" }, JsonRequestBehavior.AllowGet);
        //    }

        //    else
        //    {
        //        Guid OrderID = Guid.NewGuid();
        //        string tnxId = "";
        //        tnxId = OrderID.ToString() + DateTime.Now.ToString("yyyymmddHHmmss");    //RandomString(50);// 
        //        Tuple<int, string> res = _paymentGatewayMasterRepo.GatewayTransaction_CRUD(tnxId, erpno, Session["UserId"].ToString(), Convert.ToDecimal(amount), paymentMode, "Pending");
        //        if (res.Item1 == 1)
        //        {
        //            paytmEntities paytmEntities = MakePaymentByPaytm(tnxId, amount);

        //            return Json(new { tokenNo = paytmEntities.tnxId, orderId = tnxId, status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //            return Json(new { status = "failed", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public paytmEntities MakePaymentByPaytm(string tnxId, string amount)
        //{

        //    paytmEntities paytmEntities = new paytmEntities();

        //    try
        //    {
        //        Dictionary<string, object> body = new Dictionary<string, object>();
        //        Dictionary<string, string> head = new Dictionary<string, string>();
        //        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        //        Dictionary<string, string> txnAmount = new Dictionary<string, string>();
        //        txnAmount.Add("value", amount);
        //        txnAmount.Add("currency", "INR");
        //        Dictionary<string, string> userInfo = new Dictionary<string, string>();
        //        userInfo.Add("custId", Session["UserId"].ToString());
        //        body.Add("requestType", "Payment");
        //        body.Add("mid", merchantId);
        //        body.Add("websiteName", "WEBSTAGING");
        //        body.Add("orderId", tnxId);
        //        body.Add("txnAmount", amount);
        //        body.Add("userInfo", userInfo);
        //        body.Add("callbackUrl", responseURL);

        //        /*
        //        * Generate checksum by parameters we have in body
        //        * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys 
        //        */
        //        string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(body), Merchant_KEY);
        //        head.Add("signature", paytmChecksum);
        //        requestBody.Add("body", body);
        //        requestBody.Add("head", head);
        //        string post_data = JsonConvert.SerializeObject(requestBody);
        //        //For  Staging
        //        string url = "https://securegw-stage.paytm.in/theia/api/v1/initiateTransaction?mid=" + merchantId + "&orderId=" + tnxId;





        //        //For  Production 
        //        //string  url  =  "https://securegw.paytm.in/theia/api/v1/initiateTransaction?mid=YOUR_MID_HERE&orderId=ORDERID_98765";

        //        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //        webRequest.Method = "POST";
        //        webRequest.ContentType = "application/json";
        //        webRequest.ContentLength = post_data.Length;
        //        using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
        //        {
        //            requestWriter.Write(post_data);
        //        }

        //        string responseData = string.Empty;

        //        using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
        //        {
        //            responseData = responseReader.ReadToEnd();
        //            dynamic stuff = JsonConvert.DeserializeObject(responseData);
        //            paytmEntities.tnxId = stuff.body.txnToken;
        //            paytmEntities.orderId = tnxId;
        //            paytmEntities.amount = Convert.ToDecimal(amount);

        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        //    return RedirectToAction("Failed", "StudentFeeDetails", new { Orderid = tnxId });
        //    }

        //    return paytmEntities;
        //}

        ////public ActionResult MakePayment(string tnxId, string amount)
        ////{

        ////    if (transactionURL == null)
        ////    {
        ////        return RedirectToAction("OnlineFee", "FeeSubmit");
        ////    }
        ////    else
        ////    {
        ////        try
        ////        {
        ////            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
        ////            paytmParams.Add("MID", merchantId);
        ////            paytmParams.Add("WEBSITE", "WEBSTAGING");
        ////            paytmParams.Add("INDUSTRY_TYPE_ID", "Retail");
        ////            paytmParams.Add("CHANNEL_ID", "WEB");
        ////            paytmParams.Add("ORDER_ID", tnxId);

        ////            paytmParams.Add("CUST_ID", Session["UserId"].ToString());
        ////            paytmParams.Add("MOBILE_NO", "");
        ////            paytmParams.Add("EMAIL", "");
        ////            //paytmParams.Add("TXN_AMOUNT", Convert.ToString(getPaymentGateway.Amount)+".0");
        ////            paytmParams.Add("TXN_AMOUNT", Convert.ToString(amount));
        ////            paytmParams.Add("CALLBACK_URL", responseURL);
        ////            paytmParams.Add("MERC_UNQ_REF", "");
        ////            String checksum = paytm.CheckSum.generateCheckSum(Merchant_KEY, paytmParams);
        ////            string Url_Paytmgateway = null;
        ////            Url_Paytmgateway += transactionURL;
        ////            foreach (string key in paytmParams.Keys)
        ////            {
        ////                Url_Paytmgateway += key + "=" + paytmParams[key] + "&";
        ////            }
        ////            Url_Paytmgateway += "CHECKSUMHASH=" + checksum;

        ////            //return (Url_Paytmgateway);

        ////            string outputHTML = "<html>";
        ////            outputHTML += "<head>";
        ////            outputHTML += "<title>Merchant Check Out Page</title>";
        ////            outputHTML += "</head>";
        ////            outputHTML += "<body>";
        ////            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
        ////            outputHTML += "<form method='post' action='" + transactionURL + "' name='f1'>";
        ////            outputHTML += "<table border='1'>";
        ////            outputHTML += "<tbody>";

        ////            foreach (string key in paytmParams.Keys)
        ////            {
        ////                outputHTML += "<input type='hidden' name='" + key + "' value='" + paytmParams[key] + "'>";
        ////            }

        ////            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
        ////            outputHTML += "</tbody>";
        ////            outputHTML += "</table>";
        ////            outputHTML += "<script type='text/javascript'>";
        ////            outputHTML += "document.f1.submit();";
        ////            outputHTML += "</script>";
        ////            outputHTML += "</form>";
        ////            outputHTML += "</body>";
        ////            outputHTML += "</html>";

        ////            ViewBag.htmlData = outputHTML;


        ////            return View();
        ////        }
        ////        catch (Exception e)
        ////        {
        ////            return RedirectToAction("OnlineFee", "FeeSubmit");
        ////        }

        ////    }
        ////}

        //public ActionResult MakePayment1(string tnxId, string amount)
        //{

        //    if (transactionURL == null)
        //    {
        //        return RedirectToAction("OnlineFee", "FeeSubmit");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Dictionary<string, object> body = new Dictionary<string, object>();
        //            Dictionary<string, string> head = new Dictionary<string, string>();
        //            Dictionary<string, object> requestBody = new Dictionary<string, object>();
        //            Dictionary<string, string> txnAmount = new Dictionary<string, string>();
        //            txnAmount.Add("value", amount);
        //            txnAmount.Add("currency", "INR");
        //            Dictionary<string, string> userInfo = new Dictionary<string, string>();
        //            userInfo.Add("custId", Session["UserId"].ToString());
        //            body.Add("requestType", "Payment");
        //            body.Add("mid", merchantId);
        //            body.Add("websiteName", "WEBSTAGING");
        //            body.Add("orderId", tnxId);
        //            body.Add("txnAmount", amount);
        //            body.Add("userInfo", userInfo);
        //            body.Add("callbackUrl", responseURL);

        //            /*
        //            * Generate checksum by parameters we have in body
        //            * Find your Merchant Key in your Paytm Dashboard at https://dashboard.paytm.com/next/apikeys 
        //            */
        //            string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(body), Merchant_KEY);
        //            head.Add("signature", paytmChecksum);
        //            requestBody.Add("body", body);
        //            requestBody.Add("head", head);
        //            string post_data = JsonConvert.SerializeObject(requestBody);
        //            //For  Staging
        //            string url = "https://securegw-stage.paytm.in/theia/api/v1/initiateTransaction?mid=" + merchantId + "&orderId=" + tnxId;
        //            //For  Production 
        //            //string  url  =  "https://securegw.paytm.in/theia/api/v1/initiateTransaction?mid=YOUR_MID_HERE&orderId=ORDERID_98765";

        //            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //            webRequest.Method = "POST";
        //            webRequest.ContentType = "application/json";
        //            webRequest.ContentLength = post_data.Length;
        //            using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
        //            {
        //                requestWriter.Write(post_data);
        //            }

        //            string responseData = string.Empty;

        //            using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
        //            {
        //                responseData = responseReader.ReadToEnd();
        //                paytmEntities paytmEntities = new paytmEntities();
        //                //Console.WriteLine(responseData);
        //                dynamic stuff = JsonConvert.DeserializeObject(responseData);
        //                paytmEntities.tnxId = stuff.body.txnToken;
        //                paytmEntities.orderId = tnxId;
        //                paytmEntities.amount = Convert.ToDecimal(amount);
        //                return View(paytmEntities);
        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            return RedirectToAction("OnlineFee", "FeeSubmit");
        //        }

        //    }
        //}
        ////public ActionResult MakePayment2(string tnxId, string amount)
        ////{

        ////    if (transactionURL == null)
        ////    {
        ////        return RedirectToAction("OnlineFee", "FeeSubmit");
        ////    }
        ////    else
        ////    {
        ////        try
        ////        {
        ////            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
        ////            paytmParams.Add("MID", merchantId);
        ////            paytmParams.Add("WEBSITE", "WEBSTAGING");
        ////            paytmParams.Add("INDUSTRY_TYPE_ID", "Retail");
        ////            paytmParams.Add("CHANNEL_ID", "WEB");
        ////            paytmParams.Add("ORDER_ID", tnxId);
        ////            paytmParams.Add("CUST_ID", Session["UserId"].ToString());
        ////            paytmParams.Add("MOBILE_NO", "");
        ////            paytmParams.Add("EMAIL", "");
        ////            //paytmParams.Add("TXN_AMOUNT", Convert.ToString(getPaymentGateway.Amount)+".0");
        ////            paytmParams.Add("TXN_AMOUNT", Convert.ToString(amount));
        ////            paytmParams.Add("CALLBACK_URL", responseURL);
        ////            paytmParams.Add("MERC_UNQ_REF", "");


        ////            String checksum = paytm.CheckSum.generateCheckSum(Merchant_KEY, paytmParams);
        ////            string Url_Paytmgateway = null;
        ////            Url_Paytmgateway += transactionURL;
        ////            foreach (string key in paytmParams.Keys)
        ////            {
        ////                Url_Paytmgateway += key + "=" + paytmParams[key] + "&";
        ////            }
        ////            Url_Paytmgateway += "CHECKSUMHASH=" + checksum;
        ////            //return (Url_Paytmgateway);
        ////            string outputHTML = "<html>";
        ////            outputHTML += "<head>";
        ////            outputHTML += "<title>Show Payment Page</title>";
        ////            outputHTML += "</head>";
        ////            outputHTML += "<body>";
        ////            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
        ////            //outputHTML += "<form method='post' action='" + transactionURL + "' name='f1'>";
        ////            transactionURL = transactionURL.Replace("[MID]", merchantId);
        ////            transactionURL = transactionURL.Replace("[OrderI]", tnxId);
        ////            outputHTML += "<form method='post' action='" + transactionURL + "' name='paytm'>";
        ////            outputHTML += "<table border='1'>";
        ////            outputHTML += "<tbody>";

        ////            foreach (string key in paytmParams.Keys)
        ////            {
        ////                outputHTML += "<input type='hidden' name='" + key + "' value='" + paytmParams[key] + "'>";
        ////            }

        ////            outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
        ////            outputHTML += "</tbody>";
        ////            outputHTML += "</table>";
        ////            outputHTML += "<script type='text/javascript'>";
        ////            outputHTML += "document.paytm.submit();";
        ////            outputHTML += "</script>";
        ////            outputHTML += "</form>";
        ////            outputHTML += "</body>";
        ////            outputHTML += "</html>";

        ////            ViewBag.htmlData = outputHTML;


        ////            return View();
        ////        }
        ////        catch (Exception e)
        ////        {
        ////            return RedirectToAction("OnlineFee", "FeeSubmit");
        ////        }

        ////    }
        ////}

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult Response_(Responsedata data)
        //{
        //    String paytmChecksum = "";
        //    string CHECKSUMHASH = Request.QueryString["CHECKSUMHASH"];
        //    /* Create a Dictionary from the parameters received in POST */
        //    Dictionary<String, String> paytmParams = new Dictionary<String, String>();
        //    foreach (string key in Request.Form.Keys)
        //    {
        //        if (key.Equals("CHECKSUMHASH"))
        //        {
        //            paytmChecksum = Request.Form[key];
        //        }
        //        else
        //        {
        //            paytmParams.Add(key.Trim(), Request.Form[key].Trim());
        //        }
        //    }

        //    try
        //    {
        //        bool isValidChecksum = CheckSum.verifyCheckSum(Merchant_KEY, paytmParams, paytmChecksum);
        //        if (isValidChecksum)
        //        {
        //            paytmEntities paytmEntities = new paytmEntities();
        //            paytmEntities.paymentStatus = data.STATUS;

        //            if (data.STATUS == "Failed")
        //            {
        //                paytmEntities.failureMsg = data.RESPMSG;
        //            }

        //            paytmEntities.paymentSuccessOn = Convert.ToDateTime(data.TXNDATE);
        //            paytmEntities.tnxId = data.ORDERID;
        //            paytmEntities.trackingId = data.TXNID;
        //            paytmEntities.bankRefNo = data.BANKTXNID;
        //            paytmEntities.amount = Convert.ToDecimal(data.TXNAMOUNT);
        //            paytmEntities.paymentMode = data.PAYMENTMODE;
        //            paytmEntities.isManualResponse = false;
        //            paytmEntities.statusCode = data.RESPMSG;
        //            paytmEntities.currency = data.CURRENCY;
        //            paytmEntities.sRemark = data.GATEWAYNAME;
        //            paytmEntities.cardName = data.GATEWAYNAME;
        //            paytmEntities.statusCode = data.RESPCODE;
        //            paytmEntities.statusMsg = data.STATUS;
        //            //paytmEntities.UpdatedOn = DateTime.Now;

        //            if (paytmEntities.paymentStatus == "TXN_SUCCESS")
        //            {
        //                Tuple<int, string> res = _paymentGatewayMasterRepo.GatewayTransaction_CRUD(paytmEntities);
        //                return RedirectToAction("SuccessPayment", "StudentFeeDetails", new { Orderid = data.ORDERID });
        //            }
        //            else
        //            {
        //                return RedirectToAction("Failed", "StudentFeeDetails", new { Orderid = data.ORDERID });
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Failed", "StudentFeeDetails", new { Orderid = data.ORDERID });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return RedirectToAction("Failed", "StudentFeeDetails", new { Orderid = data.ORDERID });
        //    }


        //}

        ////public ActionResult SuccessPayment(string Orderid)
        ////{
        ////    paytmEntities paytmEntities = _paymentGatewayMasterRepo.GatewayTransaction_Transaction(Orderid, "SuccessPayment");
        ////    return View(paytmEntities);
        ////}

        ////public ActionResult Failed(string Orderid)
        ////{
        ////    paytmEntities paytmEntities = _paymentGatewayMasterRepo.GatewayTransaction_Transaction(Orderid, "Failed");
        ////    return View(paytmEntities);
        ////}

        //#region Status API for Check Status After Some time that Payment is Success or fail
        //[HttpGet]
        //public JsonResult StatusAPI(string Orderid)
        //{
        //    string Status = null;
        //    //var getPaymentGateway = obnydb.GatewayTransactions.Where(p => p.TnxId == Orderid).FirstOrDefault();

        //    if (statusAPIURL != null)
        //    {
        //        Dictionary<String, String> paytmParams = new Dictionary<String, String>();
        //        paytmParams.Add("MID", merchantId);
        //        paytmParams.Add("ORDERID", Orderid);
        //        String checksum = paytm.CheckSum.generateCheckSum(Merchant_KEY, paytmParams);
        //        paytmParams.Add("CHECKSUMHASH", checksum);
        //        StatusResponsedata obj = new StatusResponsedata();
        //        string apiUrl = statusAPIURL; ;
        //        var input = new
        //        {
        //            MID = merchantId,
        //            ORDERID = Orderid,
        //            CHECKSUMHASH = checksum,
        //        };
        //        //string inputJson = (new Newtonsoft.Json.JsonSerializer()).Serialize(input);
        //        var inputJson = Newtonsoft.Json.JsonConvert.SerializeObject(input);
        //        WebClient client = new WebClient();
        //        client.Headers["Content-type"] = "application/json";
        //        client.Encoding = Encoding.UTF8;
        //        string json = client.UploadString(apiUrl, inputJson);

        //        //List<StatusResponsedata> customers = (new JavaScriptSerializer()).Deserialize<List<StatusResponsedata>>(json);


        //        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
        //        StatusResponsedata item____ = json_serializer.Deserialize<StatusResponsedata>(json);
        //        StatusResponsedata item_ = json_serializer.Deserialize<StatusResponsedata>(json);
        //        paytmEntities paytmEntities = new paytmEntities();
        //        if (obj != null)
        //        {
        //            var firsandefault = item_;

        //            paytmEntities.paymentStatus = firsandefault.STATUS;

        //            if (firsandefault.STATUS == "Failed")
        //            {
        //                paytmEntities.failureMsg = firsandefault.RESPMSG;
        //            }
        //            //Status = getPaymentGateway.PaymentStatus;
        //            paytmEntities.tnxId = firsandefault.ORDERID;
        //            paytmEntities.paymentSuccessOn = Convert.ToDateTime(firsandefault.TXNDATE);
        //            paytmEntities.trackingId = firsandefault.TXNID;
        //            paytmEntities.bankRefNo = firsandefault.BANKTXNID;
        //            paytmEntities.amount = Convert.ToDecimal(firsandefault.TXNAMOUNT);
        //            paytmEntities.paymentMode = firsandefault.PAYMENTMODE;
        //            paytmEntities.isManualResponse = false;
        //            paytmEntities.statusMsg = firsandefault.STATUS;
        //            paytmEntities.currency = firsandefault.CURRENCY;
        //            paytmEntities.sRemark = firsandefault.GATEWAYNAME;
        //            paytmEntities.cardName = firsandefault.BANKNAME;
        //            paytmEntities.statusCode = firsandefault.RESPCODE;
        //            //getPaymentGateway.UpdatedOn = DateTime.Now;


        //            if (paytmEntities.paymentStatus == "TXN_SUCCESS")
        //            {
        //                Status = "Success";
        //                Tuple<int, string> res = _paymentGatewayMasterRepo.GatewayTransaction_CRUD(paytmEntities);
        //                //Fee Receipt logic here
        //            }
        //        }
        //    }
        //    return Json(Status, JsonRequestBehavior.AllowGet);
        //}


        //---ATOM Payment Gateway---

        public JsonResult MakeOnlinepayment(string paymentMode, string erpno, string amount, string duedate)
        {
            onlinePayment _onlinePayment = new onlinePayment();
            DataTable dt = new DataTable();

            dt = _paymentGatewayMasterRepo.GetPaymentGateway("FEE");
            if (dt.Rows.Count > 0)
            {
                _onlinePayment.paymentGateWay.gatewayName = dt.Rows[0]["gatewayName"].ToString();
                _onlinePayment.paymentGateWay.merchantId = dt.Rows[0]["merchantId"].ToString();
                _onlinePayment.paymentGateWay.merchant_KEY = dt.Rows[0]["merchant_KEY"].ToString();
                _onlinePayment.paymentGateWay.responseURL = dt.Rows[0]["responseURL"].ToString();
                _onlinePayment.paymentGateWay.transactionURL = dt.Rows[0]["transactionURL"].ToString();
                _onlinePayment.paymentGateWay.encodedURL = dt.Rows[0]["encodedURL"].ToString();
                _onlinePayment.paymentGateWay.statusAPIURL = dt.Rows[0]["statusAPIURL"].ToString();
                _onlinePayment.paymentGateWay.bankId = dt.Rows[0]["bankId"].ToString();
                _onlinePayment.paymentGateWay.loginId = dt.Rows[0]["loginId"].ToString();
                _onlinePayment.paymentGateWay.loginPassword = dt.Rows[0]["loginPassword"].ToString();
                _onlinePayment.paymentGateWay.requestHashKey = dt.Rows[0]["requestHashKey"].ToString();
                _onlinePayment.paymentGateWay.responseHashKey = dt.Rows[0]["responseHashKey"].ToString();
                _onlinePayment.paymentGateWay.customerCode = dt.Rows[0]["customerCode"].ToString();
                _onlinePayment.paymentGateWay.clientCode = dt.Rows[0]["clientCode"].ToString();
                _onlinePayment.paymentGateWay.clientProductId = dt.Rows[0]["clientProductId"].ToString();
                _onlinePayment.paymentGateWay.requestIVSet = dt.Rows[0]["requestIVSet"].ToString();
                _onlinePayment.paymentGateWay.responseIVSet = dt.Rows[0]["responseIVSet"].ToString();
                _onlinePayment.paymentGateWay.aesEncryptRequestKey = dt.Rows[0]["aesEncryptRequestKey"].ToString();
                _onlinePayment.paymentGateWay.aesEncryptResponseKey = dt.Rows[0]["aesEncryptResponseKey"].ToString();
            }
            if (String.IsNullOrEmpty(_onlinePayment.paymentGateWay.transactionURL))
            {
                return Json(new { status = "failed", Msg = "Payment gateway is not integrted, Please contact to system administrator. ", Color = "Warning" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Guid OrderID = Guid.NewGuid();
                string tnxId = "";
                tnxId = erpno + DateTime.Now.ToString("yyyymmddHHmmss");
                Tuple<int, string> res = _paymentGatewayMasterRepo.GatewayTransaction_CRUD(tnxId, erpno, Session["UserId"].ToString(), Convert.ToDecimal(amount), paymentMode, "Pending", duedate);
                if (res.Item1 == 1)
                {
                    string url = "";
                    if (_onlinePayment.paymentGateWay.gatewayName == "ATOM")
                    {
                        url = TransferFund(_onlinePayment.paymentGateWay.loginId, _onlinePayment.paymentGateWay.loginPassword,
                        null, _onlinePayment.paymentGateWay.clientProductId, _onlinePayment.paymentGateWay.customerCode, _onlinePayment.paymentGateWay.clientCode,
                        amount, "INR", "0", tnxId, DateTime.Now.ToString("yyyymmddHHmmss"),
                        _onlinePayment.paymentGateWay.bankId, _onlinePayment.paymentGateWay.responseURL, _onlinePayment.paymentGateWay.transactionURL, _onlinePayment.paymentGateWay.encodedURL, _onlinePayment.paymentGateWay.requestHashKey, _onlinePayment.paymentGateWay.aesEncryptRequestKey, _onlinePayment.paymentGateWay.requestIVSet);
                        return Json(new { url = url, orderId = tnxId, status = "success", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { status = "failed", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { status = "failed", Msg = res.Item2, Color = res.Item1 == 1 ? "Success" : "Warning" }, JsonRequestBehavior.AllowGet);
            }
        }
        public static string byteToHexString(byte[] byData)
        {
            StringBuilder sb = new StringBuilder((byData.Length * 2));
            for (int i = 0; (i < byData.Length); i++)
            {
                int v = (byData[i] & 255);
                if ((v < 16))
                {
                    sb.Append('0');
                }

                sb.Append(v.ToString("X"));

            }

            return sb.ToString();
        }
        public string TransferFund(string MerchantLogin, string MerchantPass, string MerchantDiscretionaryData, string ProductID, string ClientCode, string CustomerAccountNo, string TransactionAmount, string TransactionCurrency, string TransactionServiceCharge, string TransactionID, string TransactionDateTime, string BankID, string RetrunURL, string transactionURL, string encodedURL, string requestHashKey, string requestEncryptionKey, string salt)
        {
            string strClientCode, strClientCodeEncoded, finalURL;
            byte[] b;
            string TransactionType = "NBFundtransfer";
            string encdata = "";
            //string reqHashKey = "KEY123657234";
            string signature = "";
            try
            {
                b = Encoding.UTF8.GetBytes(ClientCode);
                strClientCode = Convert.ToBase64String(b);
                strClientCodeEncoded = HttpUtility.UrlEncode(strClientCode);
                //strURL = transactionURL;
                //encodedURL = "login=[MerchantLogin]pass=[MerchantPass]ttype=[TransactionType]prodid=[ProductID]amt=[TransactionAmount]txncurr=[TransactionCurrency]txnscamt=[TransactionServiceCharge]clientcode=[ClientCode]txnid=[TransactionID]date=[TransactionDateTime]custacc=[CustomerAccountNo]ru=[ru]signature=[signature]";
                transactionURL = transactionURL.Replace("[MerchantLogin]", MerchantLogin + "&");
                //strURL = transactionURL;
                encodedURL = encodedURL.Replace("[MerchantLogin]", MerchantLogin + "&");
                encodedURL = encodedURL.Replace("[MerchantPass]", MerchantPass + "&");
                encodedURL = encodedURL.Replace("[TransactionType]", TransactionType + "&");
                encodedURL = encodedURL.Replace("[ProductID]", ProductID + "&");
                encodedURL = encodedURL.Replace("[TransactionAmount]", TransactionAmount + "&");
                encodedURL = encodedURL.Replace("[TransactionCurrency]", TransactionCurrency + "&");
                encodedURL = encodedURL.Replace("[TransactionServiceCharge]", TransactionServiceCharge + "&");
                encodedURL = encodedURL.Replace("[ClientCode]", strClientCodeEncoded + "&");
                encodedURL = encodedURL.Replace("[TransactionID]", TransactionID + "&");
                encodedURL = encodedURL.Replace("[TransactionDateTime]", TransactionDateTime + "&");
                encodedURL = encodedURL.Replace("[CustomerAccountNo]", CustomerAccountNo + "&");
                encodedURL = encodedURL.Replace("[MerchantDiscretionaryData]", MerchantDiscretionaryData + "&");
                encodedURL = encodedURL.Replace("[BankID]", BankID + "&");
                encodedURL = encodedURL.Replace("[ru]", RetrunURL + "&");// Remove on Production

                string strsignature = MerchantLogin + MerchantPass + TransactionType + ProductID + TransactionID + TransactionAmount + TransactionCurrency;
                //byte[] bytes = Encoding.UTF8.GetBytes(reqHashKey);
                byte[] bytes = Encoding.UTF8.GetBytes(requestHashKey);
                byte[] bt = new System.Security.Cryptography.HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(strsignature));
                signature = byteToHexString(bt).ToLower();

                encodedURL = encodedURL.Replace("[signature]", signature);

                //string passphrase = "8E41C78439831010F81F61C344B7BFC7";
                //string salt = "8E41C78439831010F81F61C344B7BFC7";
                byte[] IV = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                int iterations = 65536;
                AtomAES atomAES = new AtomAES();
                encdata = atomAES.Encrypt(encodedURL, requestEncryptionKey, salt, IV, iterations);
                //encdata = atomAES.Encrypt(encodedURL, passphrase, salt, IV, iterations);
                //  string reqHashKey = requestkey;
                finalURL = transactionURL + "encdata=" + encdata;
                return finalURL;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AtomResponse()
        {
            string decdata = "", passphrase, salt;
            Uri uri;
            string strURL = _onlinePayment.paymentGateWay.transactionURL;
            Dictionary<String, String> atomParams = new Dictionary<String, String>();
            foreach (string key in Request.Form.Keys)
            {
                atomParams.Add(key.Trim(), Request.Form[key].Trim());
            }

            //string passphrase = "8E41C78439831010F81F61C344B7BFC7";
            //string salt = "8E41C78439831010F81F61C344B7BFC7";
            passphrase = _onlinePayment.paymentGateWay.aesEncryptResponseKey;
            salt = _onlinePayment.paymentGateWay.responseIVSet;
            byte[] IV = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            int iterations = 65536;
            AtomAES atomAES = new AtomAES();
            strURL = strURL.Replace("[MerchantLogin]", atomParams["login"] + "&");
            decdata = atomAES.decrypt(atomParams["encdata"], passphrase, salt, IV, iterations);
            //strURL = strURL.Replace("[MerchantLogin]", atomParams["login"] + "&");
            ///decdata = atomAES.decrypt(atomParams["encdata"], passphrase, salt, IV, iterations);
            uri = new Uri(strURL + decdata);
            _onlinePayment.paymentResponse.mmp_txn = HttpUtility.ParseQueryString(uri.Query).Get("mmp_txn");
            _onlinePayment.paymentResponse.mer_txn = HttpUtility.ParseQueryString(uri.Query).Get("mer_txn");
            _onlinePayment.paymentResponse.amt = HttpUtility.ParseQueryString(uri.Query).Get("amt");
            _onlinePayment.paymentResponse.prod = HttpUtility.ParseQueryString(uri.Query).Get("prod");
            _onlinePayment.paymentResponse.date = HttpUtility.ParseQueryString(uri.Query).Get("date");
            _onlinePayment.paymentResponse.bank_txn = HttpUtility.ParseQueryString(uri.Query).Get("bank_txn");
            _onlinePayment.paymentResponse.f_code = HttpUtility.ParseQueryString(uri.Query).Get("f_code");
            _onlinePayment.paymentResponse.clientcode = HttpUtility.ParseQueryString(uri.Query).Get("clientcode");
            _onlinePayment.paymentResponse.bank_name = HttpUtility.ParseQueryString(uri.Query).Get("bank_name");
            _onlinePayment.paymentResponse.discriminator = HttpUtility.ParseQueryString(uri.Query).Get("discriminator");
            _onlinePayment.paymentResponse.desc = HttpUtility.ParseQueryString(uri.Query).Get("desc");
            _onlinePayment.paymentResponse.udf1 = HttpUtility.ParseQueryString(uri.Query).Get("udf1");
            _onlinePayment.paymentResponse.udf2 = HttpUtility.ParseQueryString(uri.Query).Get("udf2");
            _onlinePayment.paymentResponse.udf3 = HttpUtility.ParseQueryString(uri.Query).Get("udf3");
            _onlinePayment.paymentResponse.udf4 = HttpUtility.ParseQueryString(uri.Query).Get("udf4");
            _onlinePayment.paymentResponse.udf5 = HttpUtility.ParseQueryString(uri.Query).Get("udf5");
            _onlinePayment.paymentResponse.udf6 = HttpUtility.ParseQueryString(uri.Query).Get("udf6");
            _onlinePayment.paymentResponse.udf9 = HttpUtility.ParseQueryString(uri.Query).Get("udf9");
            _onlinePayment.paymentResponse.udf10 = HttpUtility.ParseQueryString(uri.Query).Get("udf10");
            _onlinePayment.paymentResponse.udf11 = HttpUtility.ParseQueryString(uri.Query).Get("udf11");
            _onlinePayment.paymentResponse.ipg_txn_id = HttpUtility.ParseQueryString(uri.Query).Get("ipg_txn_id");
            _onlinePayment.paymentResponse.surcharge = HttpUtility.ParseQueryString(uri.Query).Get("surcharge");
            _onlinePayment.paymentResponse.CardNumber = HttpUtility.ParseQueryString(uri.Query).Get("CardNumber");
            _onlinePayment.paymentResponse.signature = HttpUtility.ParseQueryString(uri.Query).Get("signature");
            //orderid = _onlinePayment.paymentResponse.mer_txn;
            paymentResponse _paymentResponse = _onlinePayment.paymentResponse;
            if (_onlinePayment.paymentResponse.mer_txn != null)
            {
                //string respHashKey = "KEYRESP123657234";
                string respHashKey = _onlinePayment.paymentGateWay.responseHashKey;
                string ressignature = "";
                string strsignature = _onlinePayment.paymentResponse.mmp_txn + _onlinePayment.paymentResponse.mer_txn + _onlinePayment.paymentResponse.f_code + _onlinePayment.paymentResponse.prod + _onlinePayment.paymentResponse.discriminator + _onlinePayment.paymentResponse.amt + _onlinePayment.paymentResponse.bank_txn;
                //string strsignature = postingmmp_txn + postingmer_txn1 + postingf_code + postingprod + discriminator + postinamount + postingbank_txn;
                byte[] bytes = Encoding.UTF8.GetBytes(respHashKey);
                byte[] b = new System.Security.Cryptography.HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(strsignature));
                ressignature = byteToHexString(b).ToLower();
                //atomResponse atomResponse = new atomResponse();
                
                Tuple<int, string> res = _paymentGatewayMasterRepo.GatewayTransaction_CRUD(_paymentResponse);
                if (_onlinePayment.paymentResponse.signature == ressignature && _onlinePayment.paymentResponse.f_code == "Ok")
                {
                    return RedirectToAction("SuccessPayment", "StudentFeeDetails", new { transactionid= _paymentResponse.mer_txn,  paymentmode= _paymentResponse.discriminator, bankrefrenceno= _paymentResponse.bank_txn, banktransid= _paymentResponse.ipg_txn_id, amount= _onlinePayment.paymentResponse.amt, status=  "Successful transaction", transdatetime = _paymentResponse.date,paymentmessage = _paymentResponse.desc});
                }
                else
                {
                    return RedirectToAction("Failed", "StudentFeeDetails", new { transactionid = _paymentResponse.mer_txn, paymentmode = _paymentResponse.discriminator, bankrefrenceno = _paymentResponse.ipg_txn_id, amount = _onlinePayment.paymentResponse.amt, status = _onlinePayment.paymentResponse.f_code, transdatetime = _paymentResponse.date, paymentmessage = _paymentResponse.desc });
                }
            }
            else
            {
                return RedirectToAction("Failed", "StudentFeeDetails", new { transactionid = _paymentResponse.mer_txn, paymentmode = _paymentResponse.discriminator, bankrefrenceno = _paymentResponse.ipg_txn_id, amount = _onlinePayment.paymentResponse.amt, status = _onlinePayment.paymentResponse.f_code, transdatetime = _paymentResponse.date, paymentmessage = _paymentResponse.desc });
            }

        }
        public ActionResult SuccessPayment(string transactionid, string paymentmode, string bankrefrenceno,string banktransid, string amount, string status,  string transdatetime,string paymentmessage)
        {
            paymentStatus _paymentStatus = new paymentStatus();
            _paymentStatus.transactionId = transactionid;
            _paymentStatus.paymentMode = paymentmode;
            _paymentStatus.bankRefrenceNo = bankrefrenceno;
            _paymentStatus.bankTransactionId = banktransid;
            _paymentStatus.amount = amount;
            _paymentStatus.status = status;
            _paymentStatus.transactionDateTime = transdatetime;
            _paymentStatus.paymentMessage = paymentmessage;
            //paytmEntities paytmEntities = _paymentGatewayMasterRepo.GatewayTransaction_Transaction(_paymentStatus, "SuccessPayment");
            return View(_paymentStatus);
        }
        public ActionResult Failed(string transactionid, string paymentmode, string bankrefrenceno, string banktransid, string amount, string status, string transdatetime, string paymentmessage)
        {
            paymentStatus _paymentStatus = new paymentStatus();
            _paymentStatus.transactionId = transactionid;
            _paymentStatus.paymentMode = paymentmode;
            _paymentStatus.bankRefrenceNo = bankrefrenceno;
            _paymentStatus.bankTransactionId = banktransid;
            _paymentStatus.amount = amount;
            if (status == "F")
                _paymentStatus.status = "Transaction is Failed.";
            else if (status == "C")
                _paymentStatus.status = "Transaction is Cancelled.";
            else
                _paymentStatus.status = status;
            _paymentStatus.transactionDateTime = transdatetime;
            _paymentStatus.paymentMessage = paymentmessage;

            //paytmEntities paytmEntities = _paymentGatewayMasterRepo.GatewayTransaction_Transaction(Orderid, "Failed");
            return View(_paymentStatus);
        }
        //#endregion






    }


}
