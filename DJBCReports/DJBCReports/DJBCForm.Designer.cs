namespace DJBCReports
{
    partial class DJBCForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboLaporan = new System.Windows.Forms.ComboBox();
            this.rptViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // cboLaporan
            // 
            this.cboLaporan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboLaporan.FormattingEnabled = true;
            this.cboLaporan.Items.AddRange(new object[] {
            "Laporan Pemasukan Barang",
            "Laporan Pengeluaran Barang",
            "Laporan Posisi WIP",
            "Laporan Mutasi Bahan Baku",
            "Laporan Mutasi Barang Jadi",
            "Laporan Mutasi Mesin/Peralatan",
            "Laporan Mutasi Barang Reject/Scrap"});
            this.cboLaporan.Location = new System.Drawing.Point(12, 12);
            this.cboLaporan.Name = "cboLaporan";
            this.cboLaporan.Size = new System.Drawing.Size(281, 21);
            this.cboLaporan.TabIndex = 2;
            this.cboLaporan.SelectedIndexChanged += new System.EventHandler(this.cboLaporan_SelectedIndexChanged);
            // 
            // rptViewer
            // 
            this.rptViewer.Location = new System.Drawing.Point(12, 39);
            this.rptViewer.Name = "rptViewer";
            this.rptViewer.ServerReport.BearerToken = null;
            this.rptViewer.Size = new System.Drawing.Size(680, 319);
            this.rptViewer.TabIndex = 0;
            // 
            // DJBCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 370);
            this.ControlArea = false;
            this.Controls.Add(this.cboLaporan);
            this.Controls.Add(this.rptViewer);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(924, 409);
            this.Name = "DJBCForm";
            this.StatusArea = false;
            this.Text = "DJBC Reports";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DJBCForm_Load);
            this.Resize += new System.EventHandler(this.DJBCForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cboLaporan;
        private Microsoft.Reporting.WinForms.ReportViewer rptViewer;
    }
}

