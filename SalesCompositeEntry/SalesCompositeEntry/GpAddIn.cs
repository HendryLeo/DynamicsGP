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
// I am using an Modified Alternate/3rd party Form
using Microsoft.Dexterity.Applications.VatDictionary;

namespace SalesCompositeEntry
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface

        static VatSalesCompositeEntryForm VATSCEForm = Vat.Forms.VatSalesCompositeEntry;
        static VatSalesCompositeEntryForm.VatSalesCompositeEntryWindow VATSCEWindow = VATSCEForm.VatSalesCompositeEntry;
        
        public void Initialize()
        {
            VATSCEWindow.ExchangeRate.Change += new EventHandler(Rate_Change);
        }

        void Rate_Change(object sender, EventArgs e)
        {
            try
            {
                VatModified.Forms.VatSalesCompositeEntry.VatSalesCompositeEntry.LocalXchRate.Value = VATSCEWindow.ExchangeRate.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + System.Environment.NewLine + "Are you using the Modified Window?");
            }
        }
    }
}
