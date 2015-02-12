using System;
using System.Collections.Generic;
using System.Text;
using System.IO; //remove later
using System.Windows.Forms;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;
//using Microsoft.Dexterity.Applications.VatDictionary;

namespace VATinIDR
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        static PmTransactionEntryForm PMTrxEntryForm = Dynamics.Forms.PmTransactionEntry;
        static PmBatchEntryForm PMBatchEntryForm = Dynamics.Forms.PmBatchEntry;

        static PmTransactionEntryForm.PmTransactionEntryWindow PMTrxEntryWindow = PMTrxEntryForm.PmTransactionEntry;
        static PmTransactionEntryForm.PmTransactionEntryTaxDetailsWindow PMTrxEntryTaxDetailWindow = PMTrxEntryForm.PmTransactionEntryTaxDetails;
        static PmBatchEntryForm.PmBatchEntryWindow PMBatchEntryWindow = PMBatchEntryForm.PmBatchEntry;
        
        public void Initialize()
        {
            PMTrxEntryForm.AddMenuHandler(test, "sendkeys");
            PMTrxEntryForm.AddMenuHandler(test2, "postbatch");
            PMTrxEntryForm.AddMenuHandler(test3, "createbatch");
            PMTrxEntryForm.AddMenuHandler(test4, "create payable header");
            PMTrxEntryForm.AddMenuHandler(test5, "create payable tax");
            PMTrxEntryForm.AddMenuHandler(test6, "create payable distribution");

        }

        void test6(object sender, EventArgs e)
        {
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 10, 44, 2, "14030297", 2/11/2015, "IDR", 0.00000, 0.00000, 4800000.00000, 4800000.00000, 0.0000000
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 20, 25, 10, "14030297", 2/11/2015, "IDR", 4800000.00000, 4800000.00000, 0.00000, 0.00000, 0.0000000
            //'TW_AUTO_POST_PAYABLE', "VATIN1502008", 2/11/2015
            string batchNumber = "LEOTEST";
            DateTime trxDate = new DateTime(2015, 2, 28);
            decimal amount = 999999;
            decimal amount0 = 0;
            int distIdx1 = 10;
            int distIdx2 = 20;
            int actIdx1 = 44;//need to check how to get this dynamically
            int actIdx2 = 25;//need to check how to get this dynamically
            short distType1 = 2;
            short distType2 = 10;
            string currencyID = "IDR";
            string vendorID = "14030297";

            //                                          1        2  3   4     5            6        7      8        9           10             11           12
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 10, 44, 2, "14030297", 2/11/2015, "IDR", 0.00000, 0.00000, 4800000.00000, 4800000.00000, 0.0000000
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 20, 25, 10, "14030297", 2/11/2015, "IDR", 4800000.00000, 4800000.00000, 0.00000, 0.00000, 0.0000000

            FieldReadOnly<string> inParam1 = batchNumber;
            FieldReadOnly<int> inParam2a = distIdx1;
            FieldReadOnly<int> inParam3a = actIdx1;
            FieldReadOnly<short> inParam4a = distType1;
            FieldReadOnly<int> inParam2b = distIdx2;
            FieldReadOnly<int> inParam3b = actIdx2;
            FieldReadOnly<short> inParam4b = distType2;
            FieldReadOnly<string> inParam5 = vendorID;
            FieldReadOnly<DateTime> inParam6 = trxDate;
            FieldReadOnly<string> inParam7 = currencyID;
            FieldReadOnly<decimal> inParam8a = amount0;
            FieldReadOnly<decimal> inParam9a = amount0;
            FieldReadOnly<decimal> inParam10a = amount;
            FieldReadOnly<decimal> inParam11a = amount;
            FieldReadOnly<decimal> inParam8b = amount;
            FieldReadOnly<decimal> inParam9b = amount;
            FieldReadOnly<decimal> inParam10b = amount0;
            FieldReadOnly<decimal> inParam11b = amount0;
            FieldReadOnly<decimal> inParam12 = amount0;

            //                                          1        2  3   4     5            6        7      8        9           10             11           12
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 10, 44, 2, "14030297", 2/11/2015, "IDR", 0.00000, 0.00000, 4800000.00000, 4800000.00000, 0.0000000
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 20, 25, 10, "14030297", 2/11/2015, "IDR", 4800000.00000, 4800000.00000, 0.00000, 0.00000, 0.0000000

            Microsoft.Dexterity.Applications.Vat.Procedures.TwCreatePayableDistribution.Invoke(inParam1, inParam2a, inParam3a, inParam4a, inParam5, inParam6, inParam7, inParam8a, inParam9a, inParam10a, inParam11a, inParam12);

            //                                          1        2  3   4     5            6        7      8        9           10             11           12
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 20, 25, 10, "14030297", 2/11/2015, "IDR", 4800000.00000, 4800000.00000, 0.00000, 0.00000, 0.0000000

            Microsoft.Dexterity.Applications.Vat.Procedures.TwCreatePayableDistribution.Invoke(inParam1, inParam2b, inParam3b, inParam4b, inParam5, inParam6, inParam7, inParam8b, inParam9b, inParam10b, inParam11b, inParam12);

        }

        void test5(object sender, EventArgs e)
        {
            //                                1            2          3              4              5       6         7           8    9
            //'TW_CREATE_PAYABLE_TAX', "VATIN1502008", "VAT IN", "14030297", "VATIN1502008", 4800000.00000, 25, 48000000.00000, "IDR", 1
            string batchNumber = "LEOTEST";
            decimal purchaseAmount = 9999990;
            decimal taxAmount = 999999;
            string currencyID = "IDR";
            string vendorID = "14030297";
            short unknown1 = 1; //unknown
            string vatID = "VAT IN"; //how to check, maybe from VAT IN form or table
            int actIdx = 25; //need to check how to get this dynamically

            FieldReadOnly<string> inParam1 = batchNumber;
            FieldReadOnly<string> inParam2 = vatID;
            FieldReadOnly<string> inParam3 = vendorID;
            FieldReadOnly<string> inParam4 = batchNumber;
            FieldReadOnly<decimal> inParam5 = taxAmount;
            FieldReadOnly<int> inParam6 = actIdx;
            FieldReadOnly<decimal> inParam7 = purchaseAmount;
            FieldReadOnly<string> inParam8 = currencyID;
            FieldReadOnly<short> inParam9 = unknown1;

            Microsoft.Dexterity.Applications.Vat.Procedures.TwCreatePayableTax.Invoke(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6, inParam7, inParam8, inParam9);
        }

        void test4(object sender, EventArgs e)
        {
            //'TW_CREATE_PAYABLE_TAX', "VATIN1502008", "VAT IN", "14030297", "VATIN1502008", 4800000.00000, 25, 48000000.00000, "IDR", 1
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 10, 44, 2, "14030297", 2/11/2015, "IDR", 0.00000, 0.00000, 4800000.00000, 4800000.00000, 0.0000000
            //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 20, 25, 10, "14030297", 2/11/2015, "IDR", 4800000.00000, 4800000.00000, 0.00000, 0.00000, 0.0000000
            //'TW_AUTO_POST_PAYABLE', "VATIN1502008", 2/11/2015
            string batchNumber = "LEOTEST";
            DateTime trxDate = new DateTime(2015,2,28);
            decimal amount = 999999;
            string currencyID = "IDR";
            string vendorID = "14030297";
            decimal purchaseAmount = 0; //purchase amount
            decimal unknown2 = 0; //unknown
            string addressID = "HO";
            string trxDesc = "INVOICE VAT LEO";
            decimal unknown3 = 0;//unknown
            string unknown4 = "";//unused
            string unknown5 = "";//unused
            short unknown6 = 1;


            //                                    1           2            3          4        5               6             7              8 
            //'TW_CREATE_PAYABLE_HEADER', "VATIN1502008", 2/11/2015, 4800000.00000, "IDR", "VATIN1502008", "14030297", 4800000.00000, 4800000.00000
            //    9        10         11       12       13         14        15  16  17
            //, 0.00000, 0.00000, 2/11/2015, "HO", "INVOICE VAT", 0.0000000, "", "", 1
            FieldReadOnly<string> inParam1 = batchNumber;
            FieldReadOnly<DateTime> inParam2 = trxDate;
            FieldReadOnly<decimal> inParam3 = amount;
            FieldReadOnly<string> inParam4 = currencyID;
            FieldReadOnly<string> inParam5 = batchNumber;
            FieldReadOnly<string> inParam6 = vendorID;
            FieldReadOnly<decimal> inParam7 = amount; //critical
            FieldReadOnly<decimal> inParam8 = amount;
            FieldReadOnly<decimal> inParam9 = purchaseAmount;
            FieldReadOnly<decimal> inParam10 = unknown2;
            FieldReadOnly<DateTime> inParam11 = trxDate;
            FieldReadOnly<string> inParam12 = addressID;
            FieldReadOnly<string> inParam13 = trxDesc;
            FieldReadOnly<decimal> inParam14 = unknown3;
            FieldReadOnly<string> inParam15 = unknown4;
            FieldReadOnly<string> inParam16 = unknown5;
            FieldReadOnly<short> inParam17 = unknown6;


            Microsoft.Dexterity.Applications.Vat.Procedures.TwCreatePayableHeader.Invoke(inParam1, inParam2, inParam3, inParam4, inParam5, inParam6, inParam7, inParam8, inParam9, inParam10, inParam11, inParam12, inParam13, inParam14, inParam15, inParam16, inParam17);
        }

        void test3(object sender, EventArgs e)
        {

            TableError lastError;

            BatchHeadersTable BatchHeadersTable = Dynamics.Tables.BatchHeaders;
            BatchHeadersTable.Clear();
            BatchHeadersTable.BatchSource.Value = "PM_Trxent";
            BatchHeadersTable.BatchNumber.Value = "LEO2";
            BatchHeadersTable.Series.Value = 4;
            BatchHeadersTable.BatchFrequency.Value = 1;
            BatchHeadersTable.UserId.Value = Dynamics.Globals.UserId.Value;
            BatchHeadersTable.CreatedDate.Value = DateTime.Now;
            //BatchHeadersTable.NoteIndex.Value = 
            BatchHeadersTable.Origin.Value = 1;

            BatchHeadersTable.BatchComment.Value = "LEO";
            BatchHeadersTable.CheckbookId.Value = "ANZ IDR"; //change this to the first checkbook master id
            lastError = BatchHeadersTable.Save();
            BatchHeadersTable.Close();
        }
        void test2(object sender, EventArgs e)
        {
            //DateTime date = new DateTime(1900,1,1);
            //bool param = false;
            //bool paramfalse = false;
            //string[] param9 = { "FFFFFFFFFFFFFFFFFFFF" };
            //short number = 2;
            ////'Post_PM_Batch', 2, "PM_Trxent", "TEST2", 0, 0, 0, 0/0/0000, 0/0/0000, "FFFFF"
            //Dynamics.Procedures.PostPmBatch.Invoke(number, "PM_Trxent", "LEO", paramfalse, paramfalse, ref param, date, date, param9);
            
            FieldReadOnly<short> param1 = 2;
            FieldReadOnly<string> param2 = "PM_Trxent";
            FieldReadOnly<string> param3 = "LEO";
            FieldReadOnly<bool> param4 = false;
            FieldReadOnly<bool> param5 = false;
            Field<bool> param6  = false;
            FieldReadOnly<DateTime> param7 = DateTime.MinValue;
            FieldReadOnly<DateTime> param8 = DateTime.MinValue;
            string[] p9 = { "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F", "F" };
            FieldArrayReadOnly<string> param9 = new FieldArrayReadOnly<string>(p9);


            Dynamics.Procedures.PostPmBatch.Invoke(param1, param2, param3, param4, param5, ref param6, param7, param8, param9);
        }
        void test(object sender, EventArgs e)
        {

            
            CmCheckbookMstrTable CMCheckbookMstrTable = Dynamics.Tables.CmCheckbookMstr;
            string batchNumber = "";

            CMCheckbookMstrTable.Key = 1;
            CMCheckbookMstrTable.GetFirst();

            ////prepare the batch
            batchNumber = Path.GetRandomFileName().ToString().Replace(".", ""); //change to VAT DocNumber
            PMBatchEntryWindow.Open(); 
            PMBatchEntryWindow.BatchComment.Value = PMBatchEntryWindow.BatchNumber.Value = "LEO";
            PMBatchEntryWindow.BatchNumber.RunValidate();

            PMBatchEntryWindow.Origin.Value = 1;
            PMBatchEntryWindow.Origin.RunValidate();
            PMBatchEntryWindow.CheckbookId.Value = CMCheckbookMstrTable.CheckbookId.Value; //not needed by vat but needed by PM Batch
            PMBatchEntryWindow.CheckbookId.RunValidate();

                       
            ////save the batch
            PMBatchEntryWindow.SaveButton.RunValidate();
            CMCheckbookMstrTable.Close();
            PMBatchEntryWindow.Close();

            //prepare the invoice
            //PMTrxEntryWindow.LocalOverrideDefaultVoucherNumber.Value = true;
            PMTrxEntryWindow.VoucherNumberWork.Focus();
            SendKeys.SendWait(batchNumber + "{TAB}");
            //PMTrxEntryWindow.VoucherNumberWork.Value = batchNumber; 
            //PMTrxEntryWindow.VoucherNumberWork.RunValidate();
            //PMTrxEntryWindow.LocalOriginalVoucherNumberWork.Value = batchNumber;
            //PMTrxEntryWindow.LocalOriginalVoucherNumberWork.RunValidate();
            
            PMTrxEntryWindow.DocumentType.Value = 1; //invoice
            PMTrxEntryWindow.DocumentType.RunValidate();

            //PMTrxEntryWindow.TransactionDescription.Focus();
            PMTrxEntryWindow.TransactionDescription.Value = "INVOICE VAT";

            PMTrxEntryWindow.BatchNumber.Value = "LEO";
            PMTrxEntryWindow.BatchNumber.RunValidate();

            PMTrxEntryWindow.VendorId.Value = "14030297"; //change to VAT Vendor
            PMTrxEntryWindow.VendorId.RunValidate();

            PMTrxEntryWindow.DocumentNumber.Value = batchNumber;

            PMTrxEntryWindow.MiscChargesAmount.Focus();
            SendKeys.SendWait("{TAB}{TAB}"); //move to tax amount as tab order 
            SendKeys.SendWait("100000000{TAB}"); //fill tax  amount and tab to open tax detail
            SendKeys.SendWait("VAT IN{TAB}"); //change to match VAT WINDOW
            SendKeys.SendWait("100000000{TAB}");
            SendKeys.SendWait("{TAB}");
            SendKeys.SendWait("{TAB}"); //3 tabs to finish the line scroll
            SendKeys.SendWait("{ENTER}"); //close the tax detail


            //PMTrxEntryWindow.TaxAmount.Value = PMTrxEntryTaxDetailWindow.TotalTaxAmount.Value;
            //PMTrxEntryTaxDetailWindow.OkButton.RunValidate();
            
            //PMTrxEntryWindow.TaxAmount.RunValidate();

            //save the invoice
            PMTrxEntryWindow.SaveButton.RunValidate();
            //post the invoice
            //PMTrxEntryWindow.PostButton.RunValidate();


            //PMBatchEntryWindow.Open();
            //PMBatchEntryWindow.BatchNumber.Value = batchNumber;
            //PMBatchEntryWindow.BatchNumber.RunValidate();
            //PMBatchEntryWindow.Origin.Value = 1;
            //PMBatchEntryWindow.Origin.RunValidate();
            //PMBatchEntryWindow.PostButton.RunValidate();

            //PMBatchEntryWindow.Close();

            
        }
    }
}
