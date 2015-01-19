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
        static PorReturnsEntryForm PORReturnsEntryForm = Vvf.Forms.PorReturnsEntry;
        static PorInquiryReturnsEntryForm PORInquiryReturnsEntryForm = Vvf.Forms.PorInquiryReturnsEntry;

        // Create a reference to the Receiving Inquiry Alternate window
        public static PopInquiryReceivingsEntryForm.PopInquiryReceivingsEntryWindow POPInquiryReceivingsEntryWindow = POPInquiryReceivingsEntryForm.PopInquiryReceivingsEntry;
        public static PopReceivingsEntryForm.PopReceivingsEntryWindow POPReceivingEntryWindow = POPReceivingEntryForm.PopReceivingsEntry;
        public static PorReturnsEntryForm.PorReturnsEntryWindow PORReturnsEntryWindow = PORReturnsEntryForm.PorReturnsEntry;
        public static PorInquiryReturnsEntryForm.PorInquiryReturnsEntryWindow PORInquiryReturnsEntryWindow = PORInquiryReturnsEntryForm.PorInquiryReturnsEntry;

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
            PORReturnsEntryForm.AddMenuHandler(OpenUserDefined3, "User Defined", "N");
            PORReturnsEntryForm.CloseAfterOriginal += new EventHandler(CloseWinForm);
            PORInquiryReturnsEntryForm.AddMenuHandler(OpenUserDefined4, "User Defined", "N");
            PORInquiryReturnsEntryForm.CloseAfterOriginal += new EventHandler(CloseWinForm);

        }

        void CloseWinForm(object sender, EventArgs e)
        {
            // Close the WinForm
            CloseEditPOPUserDefinedForm = true;
            if (EditPOPUserDefinedForm != null)
            {
                EditPOPUserDefinedForm.Close();
                EditPOPUserDefinedForm = null;
            }
        }

        Boolean userCanEditUDef()
        {
            Boolean canEdit = false;

            string[] targets;
            TableError err;

            err = DataAccessHelper.GetIVRuleTargetsByID(6, out targets);
            if (err == TableError.NoError)
            {
                if (Array.IndexOf(targets, Microsoft.Dexterity.Applications.Dynamics.Globals.UserId.Value) > -1)
                {
                    canEdit = true;
                }
            }
            return canEdit;
        }

        // Method to open the User Defined WinForm
        void OpenUserDefined1(object sender, EventArgs e)
        {
            if (userCanEditUDef())
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
        }

        void OpenUserDefined2(object sender, EventArgs e)
        {
            if (userCanEditUDef())
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

        void OpenUserDefined3(object sender, EventArgs e)
        {
            if (userCanEditUDef())
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
                EditPOPUserDefinedForm.Caller = 3;
                EditPOPUserDefinedForm.Show();
                EditPOPUserDefinedForm.Activate();

                // Set the flag to indicate that the form shouldn't be closed
                CloseEditPOPUserDefinedForm = false;
            }
        }

        void OpenUserDefined4(object sender, EventArgs e)
        {
            if (userCanEditUDef())
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
                EditPOPUserDefinedForm.Caller = 4;
                EditPOPUserDefinedForm.Show();
                EditPOPUserDefinedForm.Activate();

                // Set the flag to indicate that the form shouldn't be closed
                CloseEditPOPUserDefinedForm = false;
            }
        }
    }
}
