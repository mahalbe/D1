using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ISas.Web.Views.Shared
{
    public partial class ReportViewerMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportDocument rd = new ReportDocument();
            if (Session["CurrentReporttoViewer"] != null)
                rd = Session["CurrentReporttoViewer"] as ReportDocument;

            CrystalReportViewer1.ReportSource = rd;
            CrystalReportViewer1.RefreshReport();
        }
    }
}