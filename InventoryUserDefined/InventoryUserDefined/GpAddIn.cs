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

        // Keep a reference to the EditIVUDef form
        static EditIVUDef EditIVUDefForm;
        
        // Indicate whether the EditIVUDef form should be closed
        public static Boolean CloseEditIVUDefForm = false;

        public void Initialize()
        {
            IVBinToBinTransferInquiryForm.AddMenuHandler(OpenIVUDef1, "User Defined", "N");
            IVBinToBinTransferInquiryForm.CloseAfterOriginal += new EventHandler (WinForm_CloseAfterOriginal);

            IVTransactionInquiryForm.AddMenuHandler(OpenIVUDef2, "User Defined", "N");
            IVTransactionInquiryForm.CloseAfterOriginal += new EventHandler(WinForm_CloseAfterOriginal);

        }
        void OpenIVUDef1(object sender,EventArgs e )
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
        void OpenIVUDef2(object sender, EventArgs e)
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

        void WinForm_CloseAfterOriginal(object sender, EventArgs e)
        {
            // Close the WinForm
            CloseEditIVUDefForm = true;
            EditIVUDefForm.Close();
            EditIVUDefForm = null;
        }
    }
}
