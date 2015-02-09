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
using Microsoft.Dexterity.Applications.PurchaseRequisitionDictionary;

namespace PCFHack
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        static PrListComparisonForm PRListComparisonForm = PurchaseRequisition.Forms.PrListComparison;

        static PrListComparisonForm.PrListComparisonWindow PRListComparisonWindow = PRListComparisonForm.PrListComparison;

        static PrEntryHdrWorkTable PREntryHdrWorkTable = PurchaseRequisition.Tables.PrEntryHdrWork;
        static PrEntryLineWorkTable PREntryLineWorkTable = PurchaseRequisition.Tables.PrEntryLineWork;

        public void Initialize()
        {
            PRListComparisonForm.AddMenuHandler(resetUser, "Reset PCF User", "R");
        }

        void resetUser(object sender, EventArgs e)
        {
            TableError lastError;
            List<string> approvedPRList = new List<string>();
            

            lastError = PREntryLineWorkTable.GetFirst();
            while (lastError == TableError.NoError) //loop the entire PR_Entry_Line_Work Table (PRHS007) looking for PR_Status = 4
            {
                if(PREntryLineWorkTable.PrStatus.Value == 4) //PR_Status = 4
                {
                    approvedPRList.Add(PREntryLineWorkTable.PrNumber.Value);
                }
                lastError = PREntryLineWorkTable.GetNext();
            }

            PREntryLineWorkTable.Close();

            approvedPRList.Sort();

            foreach (string approvedPR in approvedPRList)
            {
                PREntryHdrWorkTable.Key = 1;
                PREntryHdrWorkTable.RangeClear();
                PREntryHdrWorkTable.Clear();
                PREntryHdrWorkTable.IntercompanyId.Value = Dynamics.Globals.IntercompanyId.Value;
                PREntryHdrWorkTable.PrNumber.Value = approvedPR;
                PREntryHdrWorkTable.RangeStart();

                PREntryHdrWorkTable.Fill();
                PREntryHdrWorkTable.IntercompanyId.Value = Dynamics.Globals.IntercompanyId.Value;
                PREntryHdrWorkTable.PrNumber.Value = approvedPR;
                PREntryHdrWorkTable.RangeEnd();

                lastError = PREntryHdrWorkTable.Change();
                if (lastError == TableError.NoError)
                {
                    PREntryHdrWorkTable.PrUserCreatorPo.Value = Dynamics.Globals.UserId.Value;
                    PREntryHdrWorkTable.Save();
                }
                
                PREntryHdrWorkTable.Close();
            }
        }
    }
}
