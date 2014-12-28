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
    public partial class IVUDefListSetup : DexUIForm
    {
        private string _IVUDefLabel;

        public string IVUDefLabel
        {
            get { return _IVUDefLabel; }
            set { _IVUDefLabel = value; }
        }

        private byte _IVUDefListIdx;

        public byte IVUDefListIdx
        {
            get { return _IVUDefListIdx; }
            set { _IVUDefListIdx = value; }
        }

        public IVUDefListSetup()
        {
            InitializeComponent();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (txtItem.Text != "")
            {
                lstListItem.Items.Add(txtItem.Text);
                txtItem.Text = "";
                txtItem.Focus();
            }
        }

        private void PopulateForm()
        {
            TableError err;
            List<string> list;
            string[] arrlist;

            txtUDef.Text = _IVUDefLabel;
            txtItem.Text = "";

            lstListItem.Items.Clear();
            err = DataAccessHelper.GetIVUDefListSetup(_IVUDefListIdx, out list);
            
            if (err == TableError.NoError)
            {
                arrlist = list.ToArray();
                foreach (string listItem in arrlist)
                {
                    lstListItem.Items.Add(listItem);
                }
            }
        }

        private void IVUDefListSetup_Load(object sender, EventArgs e)
        {
            PopulateForm();
        }

        private void IVUDefListSetup_Activated(object sender, EventArgs e)
        {
            PopulateForm();
        }

        private void IVUDefListSetup_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Is the form being closed, or just hidden?
            if (IVUDefSetup.CloseIVUDefListSetupForm == false)
            {
                // Do not allow the form to completely close and be removed from memory.
                // Just hide the form and cancel the close operation.
                this.Hide();
                e.Cancel = true;
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            TableError err;
            List<string> list;

            err = new TableError();

            list = new List<string>();
            list.Clear();

            foreach(string obj in lstListItem.Items)
            {
                list.Add(obj);
            }

            err = DataAccessHelper.DeleteIVUDefListValues(_IVUDefListIdx);

            if (err == TableError.NoError | err == TableError.NotFound | err == TableError.EndOfTable)
            {
                //all deleted or not found, now save the new list
                err = DataAccessHelper.SetIVUDefListSetup(_IVUDefListIdx, list);
                if (err != TableError.NoError)
                {
                    //not saved/partial saved
                    Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                //not deleted
                Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(err.ToString());
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            txtItem.Text = "";
            lstListItem.Items.Clear();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //close/hide
            this.Close();
        }
    }
}