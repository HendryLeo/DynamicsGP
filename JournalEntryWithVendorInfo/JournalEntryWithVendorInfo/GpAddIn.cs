using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
//using Microsoft.Dexterity.Applications.DynamicsDictionary;
using Microsoft.Dexterity.Applications.VvfDictionary;
using Microsoft.Dexterity.Applications.SmartListDictionary;


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
            customerLookup.CustomerLookup.CustomerNumber.Value = VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrCustomerId.Value;
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
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrCustomerId.Value = customerNumber;
                VvfModified.Forms.GlTransactionEntry.GlTransactionEntry.LocalStrCustomerName.Value = customerName;


                // Clear the flag that indicates a value is to be retrieved from the lookup.
                GPAddIn.ReturnToLookup = false;
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
            }
        }
    }
}
