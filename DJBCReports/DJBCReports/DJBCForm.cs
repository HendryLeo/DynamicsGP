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
            System.Net.ICredentials credentials = System.Net.CredentialCache.DefaultCredentials;
            
            // testing user login credentials
            // Ensure Directory Security settings for default web site in IIS is "Windows Authentication".
            string url = "http://vvfi-db01/Reports";
            // Create a 'HttpWebRequest' object with the specified url. 
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            // Assign the credentials of the logged in user or the user being impersonated.
            myHttpWebRequest.Credentials = credentials;
            // Send the 'HttpWebRequest' and wait for response.
            try
            {
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            }
            catch (System.Net.WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    rptViewer.Visible = false;
                    cboLaporan.Visible = false;
                    rptViewer.Enabled = false;
                    cboLaporan.Enabled = false;
                    this.Text += " Unauthorized";
                    //maybe we can ask for user cred here, but for now, just disable the form
                    //ICredentials credentials = new NetworkCredential("user", "pass", "domain");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                rptViewer.Visible = false;
                cboLaporan.Visible = false;
                rptViewer.Enabled = false;
                cboLaporan.Enabled = false;
                this.Text += " ERROR";
            }
            
            // Set the credentials for the server report  
            if (rptViewer.Enabled) rptViewer.ServerReport.ReportServerCredentials.NetworkCredentials = credentials;

            if (cboLaporan.Enabled) cboLaporan.SelectedIndex = 0; //this must comes after initializing rptViewer
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