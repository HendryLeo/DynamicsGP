using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
//using Microsoft.Dexterity.Applications.DynamicsDictionary;
using Microsoft.Dexterity.Applications.VvfDictionary;
using Microsoft.Dexterity.Applications.SmartListDictionary;
using System.Windows.Forms;

namespace JournalEntryWithVendorInfo
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        //for original forms use
        //static GlTransactionEntryForm GLTrxEntryForm = Dynamics.Forms.GlTransactionEntry;
        //static GlJournalEntryInquiryForm GLJournalEntryInquiryForm = Dynamics.Forms.GlJournalEntryInquiry;
        static GlTransactionEntryForm GLTrxEntryForm = Vvf.Forms.GlTransactionEntry;
        static GlJournalEntryInquiryForm GLJEInquiryForm = Vvf.Forms.GlJournalEntryInquiry;

        static GlTransactionEntryForm.GlTransactionEntryWindow GLTrxEntryWindow = GLTrxEntryForm.GlTransactionEntry;
        static GlJournalEntryInquiryForm.GlJournalEntryInquiryWindow GLJEInquiryWindow = GLJEInquiryForm.GlJournalEntryInquiry;

        // Flag to track that a lookup was opened
        public static Boolean ReturnToLookup = false;
        string orIDToSave = "", orNameToSave = "";
        int jrnEntryToUpdate = 0;

        public void Initialize()
        {
            VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalCustomerIdLookup.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(CustomerID_Lookup_BeforeOriginal);
            VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalVendorIdLookup.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(VendorID_Lookup_BeforeOriginal);
            // Select button on the Customers lookup window
            CustomerLookupForm customerLookupForm = SmartList.Forms.CustomerLookup;
            customerLookupForm.CustomerLookup.SelectButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(CustomerSelectButton_ClickBeforeOriginal);
            // Select button on the Vendors lookup window
            VendorLookupForm vendorLookupForm = SmartList.Forms.VendorLookup;
            vendorLookupForm.VendorLookup.SelectButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(VendorSelectButton_ClickBeforeOriginal);

            GLTrxEntryWindow.SaveButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(GLTrxEntryWindow_SaveButton_ClickBeforeOriginal);
            GLTrxEntryWindow.SaveButton.ClickAfterOriginal += new System.EventHandler(GLTrxEntryWindow_SaveButton_ClickAfterOriginal);
            GLTrxEntryWindow.BeforeModalDialog +=  new System.EventHandler<BeforeModalDialogEventArgs> (GLTrxEntryWindow_BeforeModalDialog);
            GLTrxEntryWindow.AfterModalDialog +=  new System.EventHandler<AfterModalDialogEventArgs>(GLTrxEntryWindow_AfterModalDialog);
            GLTrxEntryWindow.PostButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(GLTrxEntryWindow_PostButton_ClickBeforeOriginal);
            GLTrxEntryWindow.JournalEntry.Change +=  new System.EventHandler(GLTrxEntryWindow_JournalEntry_Change);

            GLJEInquiryWindow.JournalEntry.Change += new System.EventHandler(GLJEInquiryWindow_JournalEntry_Change);
        }

        void GLJEInquiryWindow_JournalEntry_Change(object sender, EventArgs e)
        {
            VvfModified.Forms.GlJournalEntryInquiry.GlJournalEntryInquiry.LocalStrVendorId.Value = "";
            VvfModified.Forms.GlJournalEntryInquiry.GlJournalEntryInquiry.LocalStrVendorName.Value = "";
            if (GLJEInquiryWindow.JournalEntry.Value > 0)
            {
                if(GLJEInquiryWindow.LocalFiscalYear.Value == GLJEInquiryForm.Tables.GlAccountTrxHist.HistoryYear.Value.ToString())//history year trx
                {
                    VvfModified.Forms.GlJournalEntryInquiry.GlJournalEntryInquiry.LocalStrVendorId.Value = GLJEInquiryForm.Tables.GlAccountTrxHist.OriginatingMasterId.Value;
                    VvfModified.Forms.GlJournalEntryInquiry.GlJournalEntryInquiry.LocalStrVendorName.Value = GLJEInquiryForm.Tables.GlAccountTrxHist.OriginatingMasterName.Value;
                }
                else if(GLJEInquiryWindow.LocalFiscalYear.Value == GLJEInquiryForm.Tables.GlYtdTrxOpen.OpenYear.Value.ToString())//current ytd trx
                {
                    VvfModified.Forms.GlJournalEntryInquiry.GlJournalEntryInquiry.LocalStrVendorId.Value = GLJEInquiryForm.Tables.GlYtdTrxOpen.OriginatingMasterId.Value;
                    VvfModified.Forms.GlJournalEntryInquiry.GlJournalEntryInquiry.LocalStrVendorName.Value = GLJEInquiryForm.Tables.GlYtdTrxOpen.OriginatingMasterName.Value;
                }
            }
        }

        void GLTrxEntryWindow_JournalEntry_Change(object sender, EventArgs e)
        {
            TableError err;
            string orID = "", orName = "";

            //if (GLTrxEntryWindow.JournalEntry.Value == jrnEntryToUpdate)
            //{
            //    VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value = orIDToSave;
            //    VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorName.Value = orNameToSave;
            //}
            //else
            if (GLTrxEntryWindow.JournalEntry.Value != jrnEntryToUpdate)
            {
                err = DataAccessHelper.GetGLTrxLineByJournalEntry(GLTrxEntryWindow.JournalEntry.Value, out orID, out orName);
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value = orID;
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorName.Value = orName;
            }
       }

        void GLTrxEntryWindow_PostButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult answer;
            TableError err;

            if (VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value.Trim() == String.Empty)
            {
                answer = MessageBox.Show("VendorID/CustomerID tidak diisi, Mau dilanjutkan?", "Vendor/Customer tidak diisi", MessageBoxButtons.YesNo);
                if (answer == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                err = DataAccessHelper.UpdateGLTrxLineByJournalEntry(GLTrxEntryWindow.JournalEntry.Value, VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value.Trim(), VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorName.Value.Trim());
                if (err != TableError.NoError)
                {
                    MessageBox.Show(err.ToString());
                    e.Cancel = true;
                }
            }
        }

        void GLTrxEntryWindow_BeforeModalDialog(object sender, BeforeModalDialogEventArgs e)
        {
            switch (e.Message)
            {
                case "Do you want to save changes or delete this transaction?":
                    GLTrxEntryWindow.BatchNumber.Value = Dynamics.Globals.UserId;
                    jrnEntryToUpdate = GLTrxEntryWindow.JournalEntry.Value;
                    orIDToSave = VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value.Trim();
                    orNameToSave = VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorName.Value.Trim();
                    break;

            }
        }

        void GLTrxEntryWindow_AfterModalDialog(object sender, AfterModalDialogEventArgs e)
        {
            switch (e.Message)
            {
                case "Do you want to save changes or delete this transaction?":
                    switch (e.Response)
                    {
                        case DialogResponse.Button1://save
                            DataAccessHelper.UpdateGLTrxLineByJournalEntry(jrnEntryToUpdate, orIDToSave, orNameToSave);
                            jrnEntryToUpdate = 0;
                            orIDToSave = "";
                            orNameToSave = "";
                            break;
                        case DialogResponse.Button2://delete
                            jrnEntryToUpdate = 0;
                            orIDToSave = "";
                            orNameToSave = "";                        
                            break;
                        case DialogResponse.Button3://cancel
                            break;
                    }
                    break;
            }
        }

        void GLTrxEntryWindow_SaveButton_ClickAfterOriginal(object sender, EventArgs e)
        {
            TableError err;
            if (jrnEntryToUpdate > 0)
            {
                err = DataAccessHelper.UpdateGLTrxLineByJournalEntry(jrnEntryToUpdate, orIDToSave, orNameToSave);
                if (err != TableError.NoError)
                {
                    MessageBox.Show("Vendor/Customer Info not saved");
                }
            }
            jrnEntryToUpdate = 0;
            orIDToSave = "";
            orNameToSave = "";
        }

        void GLTrxEntryWindow_SaveButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult answer;

            if (VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value.Trim() == String.Empty)
            {
                answer = MessageBox.Show("VendorID/CustomerID tidak diisi, Mau dilanjutkan?", "Vendor/Customer tidak diisi", MessageBoxButtons.YesNo);
                if (answer == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                jrnEntryToUpdate = GLTrxEntryWindow.JournalEntry.Value;
                orIDToSave = VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value.Trim();
                orNameToSave = VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorName.Value.Trim();
            }
        }

        void CustomerID_Lookup_BeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Create a reference to the CustomerLookup form
            CustomerLookupForm customerLookup = SmartList.Forms.CustomerLookup;

            // Set the flag indicating that we opened the lookup
            GPAddIn.ReturnToLookup = true;

            // Open the CustomerLookup form
            customerLookup.Open();

            //set the field
            customerLookup.CustomerLookup.CustomerNumber.Value = VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value;
            customerLookup.CustomerLookup.CustomerName.Value = "";
            customerLookup.CustomerLookup.CustomerSortBy.Value = 2; //sort by 1 = customer id, 2 = customer name

            // Call the Initialize procedure to configure the Customer Lookup
            customerLookup.Procedures.Initialize.Invoke(1, 0, "", "", "", "", "", "");
        }

        void VendorID_Lookup_BeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Create a reference to the VendorLookup form
            VendorLookupForm vendorLookup = SmartList.Forms.VendorLookup;

            // Set the flag indicating that we opened the lookup
            GPAddIn.ReturnToLookup = true;

            // Open the VendorLookup form 
            vendorLookup.Open();

            // Set the field values on the lookup window
            vendorLookup.VendorLookup.VendorId.Value = VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value;
            vendorLookup.VendorLookup.VendorName.Value = "";     //Vendor Name 
            vendorLookup.VendorLookup.VendorClassId.Value = "";  //Vendor Class
            vendorLookup.VendorLookup.UserDefined1.Value = "";   //User Defined 1
            vendorLookup.VendorLookup.VendorSortBy.Value = 2;    //Sort by 1 = VendorID, 2 = VendorName

            // Run Validate on the Vendor Sort By to fill the lookup window
            vendorLookup.VendorLookup.VendorSortBy.RunValidate();
        }

        void CustomerSelectButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Run this code only if the Visual Studio Tools add-in opened the lookup.
            if (GPAddIn.ReturnToLookup == true)
            {
                // Retrieve the customer number of the row selected in the scrolling window
                // of the Customers lookup.
                CustomerLookupForm customerLookupForm = SmartList.Forms.CustomerLookup;
                string customerNumber = customerLookupForm.CustomerLookup.CustomerLookupScroll.CustomerNumber.Value;
                string customerName = customerLookupForm.CustomerLookup.CustomerLookupScroll.CustomerName.Value;

                // Display the value retrieved
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value = customerNumber;
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorName.Value = customerName;


                // Clear the flag that indicates a value is to be retrieved from the lookup.
                GPAddIn.ReturnToLookup = false;
                GLTrxEntryWindow.IsChanged = true;
            }
        }

        void VendorSelectButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Run this code only if the Visual Studio Tools add-in opened the lookup.
            if (GPAddIn.ReturnToLookup == true)
            {
                // Retrieve the vendor ID of the row selected in the scrolling window
                // of the Vendors lookup.
                VendorLookupForm vendorLookupForm = SmartList.Forms.VendorLookup;
                string vendorID = vendorLookupForm.VendorLookup.VendorLookupScroll.VendorId;
                string vendorName = vendorLookupForm.VendorLookup.VendorLookupScroll.VendorName;

                // Display the value retrieved
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorId.Value = vendorID;
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrVendorName.Value = vendorName;

                // Clear the flag that indicates a value is to be retrieved from the lookup.
                GPAddIn.ReturnToLookup = false;
                GLTrxEntryWindow.IsChanged = true;
            }
        }
    }
}
