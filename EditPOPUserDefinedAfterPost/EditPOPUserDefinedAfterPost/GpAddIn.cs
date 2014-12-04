using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;


namespace EditPOPUserDefinedAfterPost
{
    public class GPAddIn : IDexterityAddIn
    {
        // Keep a reference to the EditPOPUserDefined WinForm.
        static EditPOPUserDefined EditPOPUserDefinedForm;

        // Create a reference to the Receiving Inquiry Alternate Form
        static PopInquiryReceivingsEntryForm POPInquiryReceivingsEntryForm = PurchaseRequisition.Forms.PopInquiryReceivingsEntry;

       
        // Create a reference to the Receiving Inquiry Alternate window
        public static PopInquiryReceivingsEntryForm.PopInquiryReceivingsEntryWindow POPInquiryReceivingsEntryWindow = POPInquiryReceivingsEntryForm.PopInquiryReceivingsEntry;

        // Indicate whether the EditPOPUserDefined form should be closed
        public static Boolean CloseEditPOPUserDefinedForm = false;

        
        // IDexterityAddIn interface

        public void Initialize()
        {
            // Add the menu item to open the WinForm
            POPInquiryReceivingsEntryForm.AddMenuHandler(OpenUserDefined, "User Defined", "N");
            POPInquiryReceivingsEntryForm.CloseAfterOriginal += new EventHandler(POPInquiryReceivingsEntryForm_CloseAfterOriginal);

            // Watch when the POP Number changes
            //POPInquiryReceivingsEntryWindow.PopReceiptNumber.Change += new EventHandler(POPReceipt_Change);
        }


        void POPInquiryReceivingsEntryForm_CloseAfterOriginal(object sender, EventArgs e)
        {
            // Close the WinForm
            CloseEditPOPUserDefinedForm = true;
            EditPOPUserDefinedForm.Close();
            EditPOPUserDefinedForm = null;
        }

        //void POPReceipt_Change(object sender, EventArgs e)
        //{
        //    // If the Winform is open, update the document number
        //    // and clear the other controls
        //    if (EditPOPUserDefinedForm.Created == true)
        //    {
        //        EditPOPUserDefinedForm. .Text = SOPEntryWindow.SopNumber.Value;
        //        EditPOPUserDefinedForm.textBoxEstimatedFreight.Clear();
        //        EditPOPUserDefinedForm.textBoxTotalWeight.Clear();
        //    }
        //}

        // Method to open the User Defined WinForm
        static void OpenUserDefined(object sender, EventArgs e)
        {
            if (EditPOPUserDefinedForm == null)
            {
                try
                {
                    EditPOPUserDefinedForm = new EditPOPUserDefined();
                    
                }
                catch (Exception ex)
                {
                    Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(ex.Message);
                }
            }

            // Always show and activate the WinForm
            EditPOPUserDefinedForm.Show();
            EditPOPUserDefinedForm.Activate();

            // Set the flag to indicate that the form shouldn't be closed
            CloseEditPOPUserDefinedForm = false;



        }
    }
}
