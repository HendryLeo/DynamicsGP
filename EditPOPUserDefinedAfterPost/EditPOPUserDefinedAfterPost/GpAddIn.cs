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
		// I am using an Alternate Form hereby
		// If you are using Original GP Form change to
		// static PopInquiryReceivingsEntryForm POPInquiryReceivingsEntryForm = Dynamics.Forms.PopInquiryReceivingsEntry;
        //static PopReceivingsEntryForm POPReceivingEntryForm = Dynamics.Forms.PopReceivingsEntry;

        static PopInquiryReceivingsEntryForm POPInquiryReceivingsEntryForm = PurchaseRequisition.Forms.PopInquiryReceivingsEntry;
        static PopReceivingsEntryForm POPReceivingEntryForm = PurchaseRequisition.Forms.PopReceivingsEntry;
       
        // Create a reference to the Receiving Inquiry Alternate window
        public static PopInquiryReceivingsEntryForm.PopInquiryReceivingsEntryWindow POPInquiryReceivingsEntryWindow = POPInquiryReceivingsEntryForm.PopInquiryReceivingsEntry;
        public static PopReceivingsEntryForm.PopReceivingsEntryWindow POPReceivingEntryWindow = POPReceivingEntryForm.PopReceivingsEntry;

        // Indicate whether the EditPOPUserDefined form should be closed
        public static Boolean CloseEditPOPUserDefinedForm = false;

        
        // IDexterityAddIn interface

        public void Initialize()
        {
            // Add the menu item to open the WinForm
            POPInquiryReceivingsEntryForm.AddMenuHandler(OpenUserDefined1, "User Defined", "N");
            POPInquiryReceivingsEntryForm.CloseAfterOriginal += new EventHandler(CloseWinForm);
            POPReceivingEntryForm.AddMenuHandler(OpenUserDefined2, "User Defined", "N");
            POPReceivingEntryForm.CloseAfterOriginal += new EventHandler(CloseWinForm);

            // Watch when the POP Number changes
            //POPInquiryReceivingsEntryWindow.PopReceiptNumber.Change += new EventHandler(POPReceipt_Change);
        }

        void POPReceivingEntryForm_CloseAfterOriginal(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        void CloseWinForm(object sender, EventArgs e)
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
        static void OpenUserDefined1(object sender, EventArgs e)
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
            EditPOPUserDefinedForm.Caller = 1;
            EditPOPUserDefinedForm.Show();
            EditPOPUserDefinedForm.Activate();

            // Set the flag to indicate that the form shouldn't be closed
            CloseEditPOPUserDefinedForm = false;



        }
        static void OpenUserDefined2(object sender, EventArgs e)
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
            EditPOPUserDefinedForm.Caller = 2;
            EditPOPUserDefinedForm.Show();
            EditPOPUserDefinedForm.Activate();

            // Set the flag to indicate that the form shouldn't be closed
            CloseEditPOPUserDefinedForm = false;



        }
    }
}
