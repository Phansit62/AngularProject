using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using reporting.Models;
namespace reporting
{
    public partial class ReportData1 : System.Web.UI.Page
    {
        private ProjectRoomEntities db = new ProjectRoomEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            var data = db.OrdersData.ToList();
            var rd = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", data); // binding datatable
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/reports/Report1.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rd);
        }
    }
}