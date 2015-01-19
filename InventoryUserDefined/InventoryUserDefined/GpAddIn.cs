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
using Microsoft.Dexterity.Applications.VvfDictionary;

namespace InventoryUserDefined
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        static IvBinToBinTransferInquiryForm IVBinToBinTransferInquiryForm = Dynamics.Forms.IvBinToBinTransferInquiry;
        public static IvBinToBinTransferInquiryForm.IvBinToBinTransferInquiryWindow IVBinToBinTransferInquiryWindow = IVBinToBinTransferInquiryForm.IvBinToBinTransferInquiry;

        static Microsoft.Dexterity.Applications.VvfDictionary.IvTransactionInquiryForm IVTransactionInquiryForm = Vvf.Forms.IvTransactionInquiry;
        public static Microsoft.Dexterity.Applications.VvfDictionary.IvTransactionInquiryForm.IvTransactionInquiryWindow IVTransactionInquiryWindow = IVTransactionInquiryForm.IvTransactionInquiry;

        static Microsoft.Dexterity.Applications.VvfDictionary.IvSetupForm IVSetupForm = Vvf.Forms.IvSetup;
        static Microsoft.Dexterity.Applications.VvfDictionary.IvSetupForm.IvSetupWindow IVSetupWindow = IVSetupForm.IvSetup;
        
        // Keep a reference to the EditIVUDef form
        static EditIVUDef EditIVUDefForm;
        static IVUDefSetup IVUDefSetupForm;
        
        // Indicate whether the EditIVUDef form should be closed
        public static Boolean CloseEditIVUDefForm = false;
        public static Boolean CloseIVUDefSetupForm = false;

        public void Initialize()
        {
            IVBinToBinTransferInquiryForm.AddMenuHandler(OpenIVUDef1, "User Defined", "N");
            IVBinToBinTransferInquiryForm.CloseAfterOriginal += new EventHandler (EditIVUDefForm_CloseAfterOriginal);

            IVTransactionInquiryForm.AddMenuHandler(OpenIVUDef2, "User Defined", "N");
            IVTransactionInquiryForm.CloseAfterOriginal += new EventHandler(EditIVUDefForm_CloseAfterOriginal);

            IVSetupForm.AddMenuHandler(OpenIVUDefSetup, "User Defined Setup", "N");
            IVSetupForm.CloseAfterOriginal += new EventHandler(IVUDefSetupForm_CloseAfterOriginal);
        }

        Boolean userCanEditUDef()
        {
            Boolean canEdit = false;

            string[] targets;
            TableError err;

            err = DataAccessHelper.GetIVRuleTargetsByID(6, out targets);
            if (err == TableError.NoError)
            {
                if (Array.IndexOf(targets, Microsoft.Dexterity.Applications.Dynamics.Globals.UserId.Value.Trim()) > -1)
                {
                    canEdit = true;
                }
            }
            return canEdit;
        }

        void OpenIVUDefSetup(object sender,EventArgs e)
        {
            if (userCanEditUDef())
            {
                if (IVUDefSetupForm == null)
                {
                    try
                    {
                        IVUDefSetupForm = new IVUDefSetup();

                    }
                    catch (Exception ex)
                    {
                        Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(ex.Message);
                    }
                }

                // Always show and activate the WinForm
                IVUDefSetupForm.Show();
                IVUDefSetupForm.Activate();

                // Set the flag to indicate that the form shouldn't be closed
                CloseIVUDefSetupForm = false;
            }
        }

        void OpenIVUDef1(object sender,EventArgs e)
        {
            if (userCanEditUDef())
            {
                if (EditIVUDefForm == null)
                {
                    try
                    {
                        EditIVUDefForm = new EditIVUDef();

                    }
                    catch (Exception ex)
                    {
                        Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(ex.Message);
                    }
                }

                // Always show and activate the WinForm
                EditIVUDefForm.Caller = 1;
                EditIVUDefForm.Show();
                EditIVUDefForm.Activate();

                // Set the flag to indicate that the form shouldn't be closed
                CloseEditIVUDefForm = false;
            }
        }

        void OpenIVUDef2(object sender, EventArgs e)
        {
            if (userCanEditUDef())
            {
                if (EditIVUDefForm == null)
                {
                    try
                    {
                        EditIVUDefForm = new EditIVUDef();

                    }
                    catch (Exception ex)
                    {
                        Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(ex.Message);
                    }
                }

                // Always show and activate the WinForm
                EditIVUDefForm.Caller = 2;
                EditIVUDefForm.Show();
                EditIVUDefForm.Activate();

                // Set the flag to indicate that the form shouldn't be closed
                CloseEditIVUDefForm = false;
            }
        }

        void EditIVUDefForm_CloseAfterOriginal(object sender, EventArgs e)
        {
            // Close the WinForm
            CloseEditIVUDefForm = true;
            if (EditIVUDefForm != null)
            {
                EditIVUDefForm.Close();
                EditIVUDefForm = null;
            }
        }

        void IVUDefSetupForm_CloseAfterOriginal(object sender, EventArgs e)
        {
            // Close the WinForm
            CloseIVUDefSetupForm = true;
            if (EditIVUDefForm != null)
            {
                EditIVUDefForm.Close();
                EditIVUDefForm = null;
            }
        }

    }
}
