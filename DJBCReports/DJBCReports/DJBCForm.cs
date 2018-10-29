using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Shell;
using System.Net;
using Microsoft.Reporting.WinForms;


namespace DJBCReports
{
    public partial class DJBCForm : DexUIForm
    {
        public DJBCForm()
        {
            InitializeComponent();
        }

        private void DJBCForm_Load(object sender, EventArgs e)
        {
            //DateTime dt = DateTime.Today;
            //DateTime firstDayOfMonth = new DateTime(dt.Year, dt.Month, 1);
            //DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //this.dtAwal.Value = firstDayOfMonth;
            //this.dtAkhir.Value = lastDayOfMonth;


            // Set the processing mode for the ReportViewer to Remote  
            rptViewer.ProcessingMode = ProcessingMode.Remote;

            ServerReport serverReport = rptViewer.ServerReport;

            // Get a reference to the default credentials  
            //System.Net.ICredentials credentials =
            //    System.Net.CredentialCache.DefaultCredentials;

            // Get a reference to the report server credentials  
            NetworkCredential rsCredentials = new
                NetworkCredential("hendry.leo", "Wingardium1", "VVFINDONESIA");

            // Set the credentials for the server report  
            //rsCredentials.NetworkCredentials = credentials;
            rptViewer.ServerReport.ReportServerCredentials.NetworkCredentials = rsCredentials;

            //// Set the report server URL and report path  
            //serverReport.ReportServerUrl =
            //    new Uri("http://vvfi-db01/reportserver");
            //serverReport.ReportPath =
            //    "/ReportBC/BC_1";

            //// Create the sales order number report parameter  
            //ReportParameter startDate = new ReportParameter();
            //startDate.Name = "startDate";
            //startDate.Values.Add("05/01/2018");

            //ReportParameter endDate = new ReportParameter();
            //endDate.Name = "endDate";
            //endDate.Values.Add("08/31/2018");


            //// Set the report parameters for the report  
            //rptViewer.ServerReport.SetParameters(
            //    new ReportParameter[] { startDate, endDate });

            //// Refresh the report  
            //rptViewer.RefreshReport();

            cboLaporan.SelectedIndex = 0; //this must comes after initializing rptViewer
        }

        private void cboLaporan_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServerReport serverReport = rptViewer.ServerReport;
            // Set the report server URL and report path  
            serverReport.ReportServerUrl = new Uri("http://vvfi-db01/reportserver");

            int index = cboLaporan.SelectedIndex + 1;
            serverReport.ReportPath = "/ReportBC/BC_" + index;


            // Refresh the report  
            rptViewer.RefreshReport();
        }

        private void DJBCForm_Resize(object sender, EventArgs e)
        {
            this.rptViewer.Size = new Size(this.Size.Width - 44, this.Size.Height - 90);
        }
    }
}