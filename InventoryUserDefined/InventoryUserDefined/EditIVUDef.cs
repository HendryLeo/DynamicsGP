/* The MIT License (MIT)
 * 
 * Copyright (c) 2014 HendryLeo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

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
    public partial class EditIVUDef : DexUIForm
    {
        private Int16 _caller;

        public Int16 Caller
        {
            get { return _caller; }
            set { _caller = value; }
        }
        public EditIVUDef()
        {
            InitializeComponent();
            
            TableError err;
            string[] UserDefLabels, arrUserDefSetups;
            List<string> UserDefSetups;

            //byte err;

            //Get the UserDefined Label from IVUDefSetup
            err = DataAccessHelper.GetIVUDefSetup(out UserDefLabels);
            if (err == TableError.NoError)
            {
                lblUserDef1.Text = UserDefLabels[0];
                lblUserDef2.Text = UserDefLabels[1];
                lblUserDef3.Text = UserDefLabels[2];
                lblUserDef4.Text = UserDefLabels[3];
                lblUserDef5.Text = UserDefLabels[4];
                lblUserDef6.Text = UserDefLabels[5];
                lblUserDef7.Text = UserDefLabels[6];
                lblUserDef8.Text = UserDefLabels[7];
                lblUserDef9.Text = UserDefLabels[8];
                lblUserDef10.Text = UserDefLabels[9];
                lblUserDef11.Text = UserDefLabels[10];
                lblUserDef12.Text = UserDefLabels[11];
                lblUserDef13.Text = UserDefLabels[12];
                lblUserDef14.Text = UserDefLabels[13];
                lblUserDef15.Text = UserDefLabels[14];
                lblUserDef16.Text = UserDefLabels[15];
                lblUserDef17.Text = UserDefLabels[16];
                lblUserDef18.Text = UserDefLabels[17];
                lblUserDef19.Text = UserDefLabels[18];
                lblUserDef20.Text = UserDefLabels[19];
                lblUserDef21.Text = UserDefLabels[20];
                lblUserDef22.Text = UserDefLabels[21];
                lblUserDef23.Text = UserDefLabels[22];
                lblUserDef24.Text = UserDefLabels[23];
                lblUserDef25.Text = UserDefLabels[24];
                lblUserDef26.Text = UserDefLabels[25];
                lblUserDef27.Text = UserDefLabels[26];
                lblUserDef28.Text = UserDefLabels[27];
                lblUserDef29.Text = UserDefLabels[28];
                lblUserDef30.Text = UserDefLabels[29];
                lblUserDef31.Text = UserDefLabels[30];
                lblUserDef32.Text = UserDefLabels[31];
                lblUserDef33.Text = UserDefLabels[32];
                lblUserDef34.Text = UserDefLabels[33];
                lblUserDef35.Text = UserDefLabels[34];
            }
            //Get the UserDefined Values from IVUDefListSetup
            err = DataAccessHelper.GetIVUDefListSetup(1, out UserDefSetups);
            cboUserDefList1.Items.Add("");
            if (err == TableError.NoError)
            {
                arrUserDefSetups = UserDefSetups.ToArray();
                foreach (string UserDefSetup in arrUserDefSetups)
                {
                    cboUserDefList1.Items.Add(UserDefSetup);
                }
            }
            err = DataAccessHelper.GetIVUDefListSetup(2, out UserDefSetups);
            cboUserDefList2.Items.Add("");
            if (err == TableError.NoError)
            {
                arrUserDefSetups = UserDefSetups.ToArray();
                foreach (string UserDefSetup in arrUserDefSetups)
                {
                    cboUserDefList2.Items.Add(UserDefSetup);
                }
            }
            err = DataAccessHelper.GetIVUDefListSetup(3, out UserDefSetups);
            cboUserDefList3.Items.Add("");
            if (err == TableError.NoError)
            {
                arrUserDefSetups = UserDefSetups.ToArray();
                foreach (string UserDefSetup in arrUserDefSetups)
                {
                    cboUserDefList3.Items.Add(UserDefSetup);
                }
            }
            err = DataAccessHelper.GetIVUDefListSetup(4, out UserDefSetups);
            cboUserDefList4.Items.Add("");
            if (err == TableError.NoError)
            {
                arrUserDefSetups = UserDefSetups.ToArray();
                foreach (string UserDefSetup in arrUserDefSetups)
                {
                    cboUserDefList4.Items.Add(UserDefSetup);
                }
            }
            err = DataAccessHelper.GetIVUDefListSetup(5, out UserDefSetups);
            cboUserDefList5.Items.Add("");
            if (err == TableError.NoError)
            {
                arrUserDefSetups = UserDefSetups.ToArray();
                foreach (string UserDefSetup in arrUserDefSetups)
                {
                    cboUserDefList5.Items.Add(UserDefSetup);
                }
            }
        }

        private void EditIVUDef_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Is the form being closed, or just hidden?
            if (GPAddIn.CloseEditIVUDefForm == false)
            {
                // Do not allow the form to completely close and be removed from memory.
                // Just hide the form and cancel the close operation.
                this.Hide();
                e.Cancel = true;
            }
        }

        private void EditIVUDef_Load(object sender, EventArgs e)
        {
            TableError err;
            string[] UserDefinedStrings;
            DateTime[] UserDefinedDates;

            switch (_caller)
            {
                case 1:
                    {
                        txtDocNumber.Text = GPAddIn.IVBinToBinTransferInquiryWindow.BinTransferDocumentNumber.Value;
                        txtDocType.Text = GPAddIn.IVBinToBinTransferInquiryWindow.DocumentType.Value.ToString();
                        break;
                    }
                case 2:
                    {
                        txtDocNumber.Text = GPAddIn.IVTransactionInquiryWindow.DocumentNumber.Value;
                        txtDocType.Text = GPAddIn.IVTransactionInquiryWindow.IvDocumentType.Value.ToString();
                        break;
                    }
            }

            err = DataAccessHelper.GetIVUDefValues(txtDocNumber.Text, txtDocType.Text, out UserDefinedStrings, out UserDefinedDates);
            if (err == TableError.NoError | err == TableError.NotFound)
            {
                cboUserDefList1.SelectedItem = UserDefinedStrings[0];
                cboUserDefList2.SelectedItem = UserDefinedStrings[1];
                cboUserDefList3.SelectedItem = UserDefinedStrings[2];
                cboUserDefList4.SelectedItem = UserDefinedStrings[3];
                cboUserDefList5.SelectedItem = UserDefinedStrings[4];

                txtUserDefText1.Text = UserDefinedStrings[5];
                txtUserDefText2.Text = UserDefinedStrings[6];
                txtUserDefText3.Text = UserDefinedStrings[7];
                txtUserDefText4.Text = UserDefinedStrings[8];
                txtUserDefText5.Text = UserDefinedStrings[9];
                txtUserDefText6.Text = UserDefinedStrings[10];
                txtUserDefText7.Text = UserDefinedStrings[11];
                txtUserDefText8.Text = UserDefinedStrings[12];
                txtUserDefText9.Text = UserDefinedStrings[13];
                txtUserDefText10.Text = UserDefinedStrings[14];

                dtUserDefDate1.Value = UserDefinedDates[0];
                dtUserDefDate2.Value = UserDefinedDates[1];
                dtUserDefDate3.Value = UserDefinedDates[2];
                dtUserDefDate4.Value = UserDefinedDates[3];
                dtUserDefDate5.Value = UserDefinedDates[4];
                dtUserDefDate6.Value = UserDefinedDates[5];
                dtUserDefDate7.Value = UserDefinedDates[6];
                dtUserDefDate8.Value = UserDefinedDates[7];
                dtUserDefDate9.Value = UserDefinedDates[8];
                dtUserDefDate10.Value = UserDefinedDates[9];
                dtUserDefDate11.Value = UserDefinedDates[10];
                dtUserDefDate12.Value = UserDefinedDates[11];
                dtUserDefDate13.Value = UserDefinedDates[12];
                dtUserDefDate14.Value = UserDefinedDates[13];
                dtUserDefDate15.Value = UserDefinedDates[14];
                dtUserDefDate16.Value = UserDefinedDates[15];
                dtUserDefDate17.Value = UserDefinedDates[16];
                dtUserDefDate18.Value = UserDefinedDates[17];
                dtUserDefDate19.Value = UserDefinedDates[18];
                dtUserDefDate20.Value = UserDefinedDates[19];

            }
        }

        private void EditIVUDef_Activated(object sender, EventArgs e)
        {
            TableError err;
            string[] UserDefinedStrings;
            DateTime[] UserDefinedDates;

            switch (_caller)
            {
                case 1:
                    {
                        txtDocNumber.Text = GPAddIn.IVBinToBinTransferInquiryWindow.BinTransferDocumentNumber.Value;
                        txtDocType.Text = GPAddIn.IVBinToBinTransferInquiryWindow.DocumentType.Value.ToString();
                        break;
                    }
                case 2:
                    {
                        txtDocNumber.Text = GPAddIn.IVTransactionInquiryWindow.DocumentNumber.Value;
                        txtDocType.Text = GPAddIn.IVTransactionInquiryWindow.IvDocumentType.Value.ToString();
                        break;
                    }
            }

            err = DataAccessHelper.GetIVUDefValues(txtDocNumber.Text, txtDocType.Text, out UserDefinedStrings, out UserDefinedDates);
            if (err == TableError.NoError | err == TableError.NotFound)
            {
                cboUserDefList1.SelectedItem = UserDefinedStrings[0];
                cboUserDefList2.SelectedItem = UserDefinedStrings[1];
                cboUserDefList3.SelectedItem = UserDefinedStrings[2];
                cboUserDefList4.SelectedItem = UserDefinedStrings[3];
                cboUserDefList5.SelectedItem = UserDefinedStrings[4];

                txtUserDefText1.Text = UserDefinedStrings[5];
                txtUserDefText2.Text = UserDefinedStrings[6];
                txtUserDefText3.Text = UserDefinedStrings[7];
                txtUserDefText4.Text = UserDefinedStrings[8];
                txtUserDefText5.Text = UserDefinedStrings[9];
                txtUserDefText6.Text = UserDefinedStrings[10];
                txtUserDefText7.Text = UserDefinedStrings[11];
                txtUserDefText8.Text = UserDefinedStrings[12];
                txtUserDefText9.Text = UserDefinedStrings[13];
                txtUserDefText10.Text = UserDefinedStrings[14];

                dtUserDefDate1.Value = UserDefinedDates[0];
                dtUserDefDate2.Value = UserDefinedDates[1];
                dtUserDefDate3.Value = UserDefinedDates[2];
                dtUserDefDate4.Value = UserDefinedDates[3];
                dtUserDefDate5.Value = UserDefinedDates[4];
                dtUserDefDate6.Value = UserDefinedDates[5];
                dtUserDefDate7.Value = UserDefinedDates[6];
                dtUserDefDate8.Value = UserDefinedDates[7];
                dtUserDefDate9.Value = UserDefinedDates[8];
                dtUserDefDate10.Value = UserDefinedDates[9];
                dtUserDefDate11.Value = UserDefinedDates[10];
                dtUserDefDate12.Value = UserDefinedDates[11];
                dtUserDefDate13.Value = UserDefinedDates[12];
                dtUserDefDate14.Value = UserDefinedDates[13];
                dtUserDefDate15.Value = UserDefinedDates[14];
                dtUserDefDate16.Value = UserDefinedDates[15];
                dtUserDefDate17.Value = UserDefinedDates[16];
                dtUserDefDate18.Value = UserDefinedDates[17];
                dtUserDefDate19.Value = UserDefinedDates[18];
                dtUserDefDate20.Value = UserDefinedDates[19];
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            TableError err;

            err = DataAccessHelper.DeleteIVUDefValues(txtDocNumber.Text, txtDocType.Text);
            if (err != TableError.NoError)
            {
                //some error, not deleted
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());

            }
            else
            {
                //deleted
                this.Close();
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            TableError err;
            string DocNumber,DocType;
            string[] UserDefinedStrings = new string[15] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            DateTime[] UserDefinedDates = new DateTime[20]
                {new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1)};

            DocNumber = txtDocNumber.Text;
            DocType = txtDocType.Text;
            UserDefinedStrings[0] = cboUserDefList1.SelectedItem.ToString();
            UserDefinedStrings[1] = cboUserDefList2.SelectedItem.ToString();
            UserDefinedStrings[2] = cboUserDefList3.SelectedItem.ToString();
            UserDefinedStrings[3] = cboUserDefList4.SelectedItem.ToString();
            UserDefinedStrings[4] = cboUserDefList5.SelectedItem.ToString();

            UserDefinedStrings[5] = txtUserDefText1.Text;
            UserDefinedStrings[6] = txtUserDefText2.Text;
            UserDefinedStrings[7] = txtUserDefText3.Text;
            UserDefinedStrings[8] = txtUserDefText4.Text;
            UserDefinedStrings[9] = txtUserDefText5.Text;
            UserDefinedStrings[10] = txtUserDefText6.Text;
            UserDefinedStrings[11] = txtUserDefText7.Text;
            UserDefinedStrings[12] = txtUserDefText8.Text;
            UserDefinedStrings[13] = txtUserDefText9.Text;
            UserDefinedStrings[14] = txtUserDefText10.Text;

            UserDefinedDates[0] = dtUserDefDate1.Value;
            UserDefinedDates[1] = dtUserDefDate2.Value;
            UserDefinedDates[2] = dtUserDefDate3.Value;
            UserDefinedDates[3] = dtUserDefDate4.Value;
            UserDefinedDates[4] = dtUserDefDate5.Value;
            UserDefinedDates[5] = dtUserDefDate6.Value;
            UserDefinedDates[6] = dtUserDefDate7.Value;
            UserDefinedDates[7] = dtUserDefDate8.Value;
            UserDefinedDates[8] = dtUserDefDate9.Value;
            UserDefinedDates[9] = dtUserDefDate10.Value;
            UserDefinedDates[10] = dtUserDefDate11.Value;
            UserDefinedDates[11] = dtUserDefDate12.Value;
            UserDefinedDates[12] = dtUserDefDate13.Value;
            UserDefinedDates[13] = dtUserDefDate14.Value;
            UserDefinedDates[14] = dtUserDefDate15.Value;
            UserDefinedDates[15] = dtUserDefDate16.Value;
            UserDefinedDates[16] = dtUserDefDate17.Value;
            UserDefinedDates[17] = dtUserDefDate18.Value;
            UserDefinedDates[18] = dtUserDefDate19.Value;
            UserDefinedDates[19] = dtUserDefDate20.Value;

            err = DataAccessHelper.SetIVUDefValues(DocNumber, DocType, UserDefinedStrings, UserDefinedDates);
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

        private void EditIVUDef_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}