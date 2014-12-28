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

namespace InventoryUserDefined
{
    public partial class IVUDefSetup : DexUIForm
    {
        static IVUDefListSetup IVUDefListSetupForm;
        public static Boolean CloseIVUDefListSetupForm = false;

        public IVUDefSetup()
        {
            InitializeComponent();
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtUDefLabel1.Text = "";
            txtUDefLabel2.Text = "";
            txtUDefLabel3.Text = "";
            txtUDefLabel4.Text = "";
            txtUDefLabel5.Text = "";
            txtUDefLabel6.Text = "";
            txtUDefLabel7.Text = "";
            txtUDefLabel8.Text = "";
            txtUDefLabel9.Text = "";
            txtUDefLabel10.Text = "";
            txtUDefLabel11.Text = "";
            txtUDefLabel12.Text = "";
            txtUDefLabel13.Text = "";
            txtUDefLabel14.Text = "";
            txtUDefLabel15.Text = "";
            txtUDefLabel16.Text = "";
            txtUDefLabel17.Text = "";
            txtUDefLabel18.Text = "";
            txtUDefLabel19.Text = "";
            txtUDefLabel20.Text = "";
            txtUDefLabel21.Text = "";
            txtUDefLabel22.Text = "";
            txtUDefLabel23.Text = "";
            txtUDefLabel24.Text = "";
            txtUDefLabel25.Text = "";
            txtUDefLabel26.Text = "";
            txtUDefLabel27.Text = "";
            txtUDefLabel28.Text = "";
            txtUDefLabel29.Text = "";
            txtUDefLabel30.Text = "";
            txtUDefLabel31.Text = "";
            txtUDefLabel32.Text = "";
            txtUDefLabel33.Text = "";
            txtUDefLabel34.Text = "";
            txtUDefLabel35.Text = "";

            cmdList1.Enabled = false;
            cmdList2.Enabled = false;
            cmdList3.Enabled = false;
            cmdList4.Enabled = false;
            cmdList5.Enabled = false;


        }

        private TableError DoSave()
        {
            string[] IVUDefLabels;
            TableError err;

            IVUDefLabels = new string[35] 
                {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                 "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                 "", "", "", "", ""};

            IVUDefLabels[0] = txtUDefLabel1.Text;
            IVUDefLabels[1] = txtUDefLabel2.Text;
            IVUDefLabels[2] = txtUDefLabel3.Text;
            IVUDefLabels[3] = txtUDefLabel4.Text;
            IVUDefLabels[4] = txtUDefLabel5.Text;
            IVUDefLabels[5] = txtUDefLabel6.Text;
            IVUDefLabels[6] = txtUDefLabel7.Text;
            IVUDefLabels[7] = txtUDefLabel8.Text;
            IVUDefLabels[8] = txtUDefLabel9.Text;
            IVUDefLabels[9] = txtUDefLabel10.Text;
            IVUDefLabels[10] = txtUDefLabel11.Text;
            IVUDefLabels[11] = txtUDefLabel12.Text;
            IVUDefLabels[12] = txtUDefLabel13.Text;
            IVUDefLabels[13] = txtUDefLabel14.Text;
            IVUDefLabels[14] = txtUDefLabel15.Text;
            IVUDefLabels[15] = txtUDefLabel16.Text;
            IVUDefLabels[16] = txtUDefLabel17.Text;
            IVUDefLabels[17] = txtUDefLabel18.Text;
            IVUDefLabels[18] = txtUDefLabel19.Text;
            IVUDefLabels[19] = txtUDefLabel20.Text;
            IVUDefLabels[20] = txtUDefLabel21.Text;
            IVUDefLabels[21] = txtUDefLabel22.Text;
            IVUDefLabels[22] = txtUDefLabel23.Text;
            IVUDefLabels[23] = txtUDefLabel24.Text;
            IVUDefLabels[24] = txtUDefLabel25.Text;
            IVUDefLabels[25] = txtUDefLabel26.Text;
            IVUDefLabels[26] = txtUDefLabel27.Text;
            IVUDefLabels[27] = txtUDefLabel28.Text;
            IVUDefLabels[28] = txtUDefLabel29.Text;
            IVUDefLabels[29] = txtUDefLabel30.Text;
            IVUDefLabels[30] = txtUDefLabel31.Text;
            IVUDefLabels[31] = txtUDefLabel32.Text;
            IVUDefLabels[32] = txtUDefLabel33.Text;
            IVUDefLabels[33] = txtUDefLabel34.Text;
            IVUDefLabels[34] = txtUDefLabel35.Text;

            err = DataAccessHelper.SetIVUDefSetup(IVUDefLabels);
            return err;
        }


        private void cmdSave_Click(object sender, EventArgs e)
        {
            TableError err;
            err = DoSave();

            if (err != TableError.NoError)
            {
                //notsaved
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
            }
            else
            {
                //saved
                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopulateForm()
        {
            TableError err;
            string[] IVUDefLabels;

            err = DataAccessHelper.GetIVUDefSetup(out IVUDefLabels);
            if (err == TableError.NoError | err == TableError.NotFound)
            {
                txtUDefLabel1.Text = IVUDefLabels[0];
                cmdList1.Enabled = !(IVUDefLabels[0] == "");
                txtUDefLabel2.Text = IVUDefLabels[1];
                cmdList2.Enabled = !(IVUDefLabels[1] == "");
                txtUDefLabel3.Text = IVUDefLabels[2];
                cmdList3.Enabled = !(IVUDefLabels[2] == "");
                txtUDefLabel4.Text = IVUDefLabels[3];
                cmdList4.Enabled = !(IVUDefLabels[3] == "");
                txtUDefLabel5.Text = IVUDefLabels[4];
                cmdList5.Enabled = !(IVUDefLabels[4] == "");
                txtUDefLabel6.Text = IVUDefLabels[5];
                txtUDefLabel7.Text = IVUDefLabels[6];
                txtUDefLabel8.Text = IVUDefLabels[7];
                txtUDefLabel9.Text = IVUDefLabels[8];
                txtUDefLabel10.Text = IVUDefLabels[9];
                txtUDefLabel11.Text = IVUDefLabels[10];
                txtUDefLabel12.Text = IVUDefLabels[11];
                txtUDefLabel13.Text = IVUDefLabels[12];
                txtUDefLabel14.Text = IVUDefLabels[13];
                txtUDefLabel15.Text = IVUDefLabels[14];
                txtUDefLabel16.Text = IVUDefLabels[15];
                txtUDefLabel17.Text = IVUDefLabels[16];
                txtUDefLabel18.Text = IVUDefLabels[17];
                txtUDefLabel19.Text = IVUDefLabels[18];
                txtUDefLabel20.Text = IVUDefLabels[19];
                txtUDefLabel21.Text = IVUDefLabels[20];
                txtUDefLabel22.Text = IVUDefLabels[21];
                txtUDefLabel23.Text = IVUDefLabels[22];
                txtUDefLabel24.Text = IVUDefLabels[23];
                txtUDefLabel25.Text = IVUDefLabels[24];
                txtUDefLabel26.Text = IVUDefLabels[25];
                txtUDefLabel27.Text = IVUDefLabels[26];
                txtUDefLabel28.Text = IVUDefLabels[27];
                txtUDefLabel29.Text = IVUDefLabels[28];
                txtUDefLabel30.Text = IVUDefLabels[29];
                txtUDefLabel31.Text = IVUDefLabels[30];
                txtUDefLabel32.Text = IVUDefLabels[31];
                txtUDefLabel33.Text = IVUDefLabels[32];
                txtUDefLabel34.Text = IVUDefLabels[33];
                txtUDefLabel35.Text = IVUDefLabels[34];
            }
        }

        private void IVUDefSetup_Load(object sender, EventArgs e)
        {
            PopulateForm();
        }

        private void IVUDefSetup_Activated(object sender, EventArgs e)
        {
            PopulateForm();
        }

        private void IVUDefSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Is the form being closed, or just hidden?
            if (GPAddIn.CloseIVUDefSetupForm  == false)
            {
                // Do not allow the form to completely close and be removed from memory.
                // Just hide the form and cancel the close operation.
                this.Hide();
                e.Cancel = true;
            }
            else
            {
                CloseIVUDefListSetupForm = true;
                IVUDefListSetupForm.Close();
                IVUDefListSetupForm = null;
            }

        }

        private void txtUDefLabel1_TextChanged(object sender, EventArgs e)
        {
            cmdList1.Enabled = !(txtUDefLabel1.Text == "");
        }

        private void txtUDefLabel2_TextChanged(object sender, EventArgs e)
        {
            cmdList2.Enabled = !(txtUDefLabel2.Text == "");
        }

        private void txtUDefLabel3_TextChanged(object sender, EventArgs e)
        {
            cmdList3.Enabled = !(txtUDefLabel3.Text == "");
        }

        private void txtUDefLabel4_TextChanged(object sender, EventArgs e)
        {
            cmdList4.Enabled = !(txtUDefLabel4.Text == "");
        }

        private void txtUDefLabel5_TextChanged(object sender, EventArgs e)
        {
            cmdList5.Enabled = !(txtUDefLabel5.Text == "");
        }

        private void callIVUDefListSetupForm(string label, byte idx)
        {
            if (IVUDefListSetupForm == null)
            {
                try
                {
                    IVUDefListSetupForm = new IVUDefListSetup();

                }
                catch (Exception ex)
                {
                    Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(ex.Message);
                }
            }

            IVUDefListSetupForm.IVUDefLabel = label;
            IVUDefListSetupForm.IVUDefListIdx = idx;
            // Always show and activate the WinForm
            IVUDefListSetupForm.Show();
            IVUDefListSetupForm.Activate();

            CloseIVUDefListSetupForm = false;
        }

        private void cmdList1_Click(object sender, EventArgs e)
        {
            TableError err;
            err = DoSave();
            if (err == TableError.NoError)
            {
                callIVUDefListSetupForm(txtUDefLabel1.Text, 1);
            }
            else
            {
                //notsaved
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
            }
        }

        private void cmdList2_Click(object sender, EventArgs e)
        {
            TableError err;
            err = DoSave();
            if (err == TableError.NoError)
            {
                callIVUDefListSetupForm(txtUDefLabel2.Text, 2);
            }
            else
            {
                //notsaved
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
            }
        }

        private void cmdList3_Click(object sender, EventArgs e)
        {
            TableError err;
            err = DoSave();
            if (err == TableError.NoError)
            {
                callIVUDefListSetupForm(txtUDefLabel3.Text, 3);
            }
            else
            {
                //notsaved
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
            }
        }

        private void cmdList4_Click(object sender, EventArgs e)
        {
            TableError err;
            err = DoSave();
            if (err == TableError.NoError)
            {
                callIVUDefListSetupForm(txtUDefLabel4.Text, 4);
            }
            else
            {
                //notsaved
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
            }
        }

        private void cmdList5_Click(object sender, EventArgs e)
        {
            TableError err;
            err = DoSave();
            if (err == TableError.NoError)
            {
                callIVUDefListSetupForm(txtUDefLabel5.Text, 5);
            }
            else
            {
                //notsaved
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
            }
        }
    }
}