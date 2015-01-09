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
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
//using Microsoft.Dexterity.Applications.DynamicsDictionary;
using Microsoft.Dexterity.Applications.VvfDictionary;

namespace POInquiry
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        //for original Dynamics Form use
        //static PopPoEntryForm POPPOEntryForm = Dynamics.Forms.PopPoEntry;
        static PopPoEntryForm POPPOEntryForm = Vvf.Forms.PopPoEntry;

        static PopPoEntryForm.PopPoEntryWindow POPPOEntryWindow = POPPOEntryForm.PopPoEntry;


        public void Initialize()
        {
            POPPOEntryForm.AddMenuHandler(OpenPOInquiry, "PO Inquiry", "I");

        }

        void OpenPOInquiry(object sender, EventArgs e)
        {
            //sanscript
            //OpenWindow("PO1400200",2,2) of form POP_Inquiry_PO_Entry;
            //sample continuum from Mariano Gomez http://dynamicsgpblogster.blogspot.com/2013/08/accessing-microsoft-dynamics-gp-default.html
            try
            {
                Dynamics.Application gpApp = (Dynamics.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Dynamics.Application"));
                if (gpApp == null)
                    return;
                else
                {
                    string passthrough_code = "";
                    string compile_err;
                    int error_code;

                    passthrough_code += @"OpenWindow(""" + POPPOEntryWindow.PoNumber.Value + @""",2,2) of form POP_Inquiry_PO_Entry;";
                    
                    //not needed in my case, and it will auto adapt to the alternate/modified setup
                    //gpApp.CurrentProductID = 0;

                    error_code = gpApp.ExecuteSanscript(passthrough_code, out compile_err);
                    return;
                }
            }
            catch
            {
                //MessageBox.Show("Failed to initialize gpApp");
                return;
            }
        }
    }
}
