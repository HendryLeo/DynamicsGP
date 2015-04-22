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
using Microsoft.Dexterity.Applications.VatDictionary;

namespace VATinIDR
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface

        static VatInForm VATInForm = Vat.Forms.VatIn;

        static VatInForm.VatInWindow VATInWindow = VATInForm.VatIn;

        static McSetpTable MCSetupTable = Vat.Tables.McSetp;

        string batchNumber = "";

        public void Initialize()
        {

            VATInWindow.PostButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(PostButton_ClickBeforeOriginal);
            VATInWindow.PostButton.ClickAfterOriginal += new EventHandler(PostButton_ClickAfterOriginal);
            //VATInWindow.VatTotalPpn.Change += VatTotalPpn_Change;
            //VATInWindow.TotalTax.Change += TotalTax_Change;
        }

        /* void TotalTax_Change(object sender, EventArgs e)
        {
            MessageBox.Show(VATInWindow.DocumentAmount.Value.ToString());
            MessageBox.Show(VATInWindow.TotalTax.Value.ToString());
        }

        void VatTotalPpn_Change(object sender, EventArgs e)
        {
            MessageBox.Show("TotalPpn" + VATInWindow.VatTotalPpn.Value.ToString());
        } */

        void PostButton_ClickAfterOriginal(object sender, EventArgs e)
        {
            TableError lastError;
            string trxsource = "", functionalCurrency = "";;
            DateTime postedDate = new DateTime(1900,1,1);
            short functionalCurrencyIndex = 0;

            PmTransactionOpenTable PMTrxOpenTable = Vat.Tables.PmTransactionOpen;
            PmTransactionWorkTable PMTrxWorkTable = Vat.Tables.PmTransactionWork;
            PmTaxWorkTable PMTaxWorkTable = Vat.Tables.PmTaxWork;
            PmDistributionWorkOpenTable PMDistributionWorkOpenTable = Vat.Tables.PmDistributionWorkOpen;
            PmKeyMstrTable PMKeyMstrTable = Vat.Tables.PmKeyMstr;
            BatchHeadersTable BatchHeadersTable = Vat.Tables.BatchHeaders;

            if(batchNumber != "") //get relevant data to update PM10100 if batchNumber is set
            {
                PMTrxOpenTable.Key = 2;
                PMTrxOpenTable.Clear();
                PMTrxOpenTable.DocumentType.Value = 1;
                PMTrxOpenTable.VoucherNumber.Value = batchNumber;
                lastError = PMTrxOpenTable.Get();
                if (lastError == TableError.NoError)
                {
                    postedDate = PMTrxOpenTable.PostedDate.Value;
                    trxsource = PMTrxOpenTable.TrxSource.Value;
                }
                PMTrxOpenTable.Close();

			    MCSetupTable.Key = 1;
                lastError = MCSetupTable.GetFirst(); //this table is only 1 row
		        if (lastError == TableError.NoError)
                {                
                    functionalCurrency = MCSetupTable.FunctionalCurrency.Value;
                    functionalCurrencyIndex = MCSetupTable.FunctionalCurrencyIndex.Value;
                }
                MCSetupTable.Close();
            }

            if (batchNumber != "" && trxsource != "" && functionalCurrencyIndex != 0)//prepare to cleanup when batchnumber is set and relevant data is retrievable
            {

                //PM10000
                PMTrxWorkTable.Key = 1;
                PMTrxWorkTable.Clear();
                PMTrxWorkTable.BatchSource.Value = "PM_Trxent";
                PMTrxWorkTable.BatchNumber.Value = batchNumber;
                PMTrxWorkTable.VoucherNumberWork.Value = batchNumber;
                PMTrxWorkTable.Change();
                PMTrxWorkTable.Remove();

                //PM10500
                PMTaxWorkTable.Key = 1;
                PMTaxWorkTable.RangeClear();
                PMTaxWorkTable.Clear();
                PMTaxWorkTable.VoucherNumber.Value = batchNumber;
                PMTaxWorkTable.TrxSource.Value = "";
                PMTaxWorkTable.RangeStart();
                
                PMTaxWorkTable.Fill();
                PMTaxWorkTable.VoucherNumber.Value = batchNumber;
                PMTaxWorkTable.TrxSource.Value = "";
                PMTaxWorkTable.RangeEnd();

                PMTaxWorkTable.RangeRemove();

                //PM10100
                PMDistributionWorkOpenTable.Key = 5;
                PMDistributionWorkOpenTable.RangeClear();

                PMDistributionWorkOpenTable.Clear();
                PMDistributionWorkOpenTable.VoucherNumber.Value = batchNumber;
                PMDistributionWorkOpenTable.RangeStart();

                PMDistributionWorkOpenTable.Fill();
                PMDistributionWorkOpenTable.VoucherNumber.Value = batchNumber;
                PMDistributionWorkOpenTable.RangeEnd();

                lastError = PMDistributionWorkOpenTable.ChangeFirst();
                while (lastError == TableError.NoError)
                {
                    PMDistributionWorkOpenTable.PostingStatus.Value = 1;
                    PMDistributionWorkOpenTable.TrxSource.Value = trxsource;
                    PMDistributionWorkOpenTable.PostingDate.Value = postedDate;
                    PMDistributionWorkOpenTable.CurrencyId.Value = functionalCurrency;
                    PMDistributionWorkOpenTable.CurrencyIndex.Value = functionalCurrencyIndex;
                    PMDistributionWorkOpenTable.OriginatingCreditAmount.Value = 0;
                    PMDistributionWorkOpenTable.OriginatingDebitAmount.Value = 0;
                    PMDistributionWorkOpenTable.ExchangeRate.Value = 1;
                    
                    PMDistributionWorkOpenTable.Save();
                    lastError = PMDistributionWorkOpenTable.ChangeNext();
                }

                //PM00400
                PMKeyMstrTable.Key = 1;
                PMKeyMstrTable.Clear();
                PMKeyMstrTable.ControlType.Value = 0;
                PMKeyMstrTable.ControlNumber.Value = batchNumber;
                PMKeyMstrTable.Change();
                PMKeyMstrTable.DocumentStatus.Value = 2;
                PMKeyMstrTable.Save();

                //SY00500
                BatchHeadersTable.Key = 1;
                BatchHeadersTable.Clear();
                BatchHeadersTable.BatchSource.Value = "PM_Trxent";
                BatchHeadersTable.BatchNumber.Value = batchNumber;
                BatchHeadersTable.Change();
                BatchHeadersTable.Remove();

                PMTrxWorkTable.Close();
                PMTaxWorkTable.Close();
                PMDistributionWorkOpenTable.Close();
                PMKeyMstrTable.Close();
                BatchHeadersTable.Close();
            }

            batchNumber = ""; //clear batchNumber unconditionally
        }


        void PostButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            VatInHdrTable VATInHdrTable = Vat.Tables.VatInHdr;
            VatInTaxDtlIdTable VATInTaxDtlIdTable = Vat.Tables.VatInTaxDtlId;
            VatInDistributionTable VATInDistributionTable = Vat.Tables.VatInDistribution;
            
            TableError err;

            DateTime trxDate = new DateTime();
            decimal headerTaxAmount = 0;
            string functionalCurrency = "";
            short functionalCurrencyIndex = 0;
            string vendorID = "";
            //decimal unknown2 = 0; //unknown
            string addressCode = "";
            string trxDesc = "INVOICE VAT";
            //decimal unknown3 = 0;//unknown
            string unknown4 = "";//unused
            string unknown5 = "";//unused
            short unknown6 = 1;

            decimal taxDetailPurchaseAmount = 0;
            List<decimal> taxDetailTaxAmount = new List<decimal>();
            short unknown1 = 1; //unknown
            List<string> taxID = new List<string>();

            decimal amount0 = 0;
            List<int> distLine = new List<int>();
            List<int> actIdx = new List<int>();
            List<short> distType = new List<short>();
            List<decimal> distDebitAmount = new List<decimal>(), distCreditAmount = new List<decimal>();

            MCSetupTable.Key = 1;
            err = MCSetupTable.GetFirst(); //this table is only 1 row

            if (err == TableError.NoError)
            {
                functionalCurrency = MCSetupTable.FunctionalCurrency.Value;
                functionalCurrencyIndex = MCSetupTable.FunctionalCurrencyIndex.Value;
                MCSetupTable.Close();

                if ((VATInWindow.CurrencyId.Value != functionalCurrency) && (VATInWindow.DocumentNumber.Value != ""))//not in functional currency, intercept
                {
                    batchNumber = VATInWindow.DocumentNumber.Value;
                    //save the form data
                    VATInWindow.SaveRecord.RunValidate();
                    
                    VATInHdrTable.Key = 1;
                    VATInHdrTable.DocumentNumber.Value = batchNumber;
                    err = VATInHdrTable.Get();
                    if (err == TableError.NoError) //found in VAT_IN_HDR
                    {
                        vendorID = VATInHdrTable.VendorId.Value;
                        addressCode = VATInHdrTable.AddressCode.Value;
                        trxDate = VATInHdrTable.VatTglTerimaFaktur.Value; //what if date change? i believe it is already saved and disabled in form
                        //the following data is not yet updated in VAT_IN_HDR
                        taxDetailPurchaseAmount = VATInHdrTable.VatTaxDocumentAmount.Value;
                        headerTaxAmount = VATInHdrTable.VatTotalPpn.Value;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    VATInHdrTable.Close();
                    if (e.Cancel)//no use to continue if VAT_IN_HDR data is not found
                    {
                        batchNumber = "";
                        return; 
                    }

                    //get all taxID from VAT_IN_TAX_DTL_ID table
                    VATInTaxDtlIdTable.Key = 1;
                    VATInTaxDtlIdTable.RangeClear();

                    VATInTaxDtlIdTable.Clear();
                    VATInTaxDtlIdTable.VatNoFaktur.Value = batchNumber;
                    VATInTaxDtlIdTable.RangeStart();

                    VATInTaxDtlIdTable.Fill();
                    VATInTaxDtlIdTable.VatNoFaktur.Value = batchNumber;
                    VATInTaxDtlIdTable.RangeEnd();

                    err = VATInTaxDtlIdTable.GetFirst();

                    if (err == TableError.NoError)//found
                    {
                        while (err == TableError.NoError)//loop to get all taxID
                        {
                            taxID.Add(VATInTaxDtlIdTable.TaxDetailId.Value);

                            err = VATInTaxDtlIdTable.GetNext();
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }

                    VATInTaxDtlIdTable.Close();
                    if (e.Cancel) //no use to continue if VAT_IN_TAX_DTL_ID data is not found
                    {
                        batchNumber = "";
                        return;
                    }

                    //get all Distribution from VAT_IN_Distribution table
                    VATInDistributionTable.Key = 1;
                    VATInDistributionTable.RangeClear();
                    VATInDistributionTable.Clear();
                    VATInDistributionTable.VatNoFaktur.Value = batchNumber;
                    VATInDistributionTable.RangeStart();

                    VATInDistributionTable.Fill();
                    VATInDistributionTable.VatNoFaktur.Value = batchNumber;
                    VATInDistributionTable.RangeEnd();

                    err = VATInDistributionTable.GetFirst();

                    if (err == TableError.NoError)//found
                    {
                        while (err == TableError.NoError)//loop to get all taxID
                        {
                            distLine.Add(VATInDistributionTable.LineItemSequence.Value);
                            actIdx.Add(VATInDistributionTable.AccountIndex.Value);
                            distType.Add(VATInDistributionTable.DistributionType.Value);
                            distDebitAmount.Add(VATInDistributionTable.DebitAmount);
                            distCreditAmount.Add(VATInDistributionTable.CreditAmount.Value);
                            taxDetailTaxAmount.Add(VATInDistributionTable.DebitAmount.Value + VATInDistributionTable.CreditAmount.Value);

                            err = VATInDistributionTable.GetNext();
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }

                    VATInDistributionTable.Close();
                    if (e.Cancel) //no use to continue if VAT_IN_Distribution data is not found
                    {
                        batchNumber = "";
                        return;
                    }
                    
                    //                                    1           2            3          4        5               6             7              8 
                    //'TW_CREATE_PAYABLE_HEADER', "VATIN1502008", 2/11/2015, 4800000.00000, "IDR", "VATIN1502008", "14030297", 4800000.00000, 4800000.00000
                    //    9        10         11       12       13         14        15  16  17
                    //, 0.00000, 0.00000, 2/11/2015, "HO", "INVOICE VAT", 0.0000000, "", "", 1
                    FieldReadOnly<string> step1InParam1 = batchNumber;
                    FieldReadOnly<DateTime> step1InParam2 = trxDate;
                    FieldReadOnly<decimal> step1InParam3 = headerTaxAmount;
                    FieldReadOnly<string> step1InParam4 = functionalCurrency;
                    FieldReadOnly<string> step1InParam5 = batchNumber;
                    FieldReadOnly<string> step1InParam6 = vendorID;
                    FieldReadOnly<decimal> step1InParam7 = headerTaxAmount; //critical
                    FieldReadOnly<decimal> step1InParam8 = headerTaxAmount;
                    FieldReadOnly<decimal> step1InParam9 = amount0;
                    FieldReadOnly<decimal> step1InParam10 = amount0;
                    FieldReadOnly<DateTime> step1InParam11 = trxDate;
                    FieldReadOnly<string> step1InParam12 = addressCode;
                    FieldReadOnly<string> step1InParam13 = trxDesc;
                    FieldReadOnly<decimal> step1InParam14 = amount0;
                    FieldReadOnly<string> step1InParam15 = unknown4;
                    FieldReadOnly<string> step1InParam16 = unknown5;
                    FieldReadOnly<short> step1InParam17 = unknown6;

                    Vat.Procedures.TwCreatePayableHeader.Invoke(step1InParam1, step1InParam2, step1InParam3, step1InParam4, step1InParam5, step1InParam6, step1InParam7, step1InParam8, step1InParam9, step1InParam10, step1InParam11, step1InParam12, step1InParam13, step1InParam14, step1InParam15, step1InParam16, step1InParam17);

                    //                                1            2          3              4              5       6         7           8    9
                    //'TW_CREATE_PAYABLE_TAX', "VATIN1502008", "VAT IN", "14030297", "VATIN1502008", 4800000.00000, 25, 48000000.00000, "IDR", 1
                    FieldReadOnly<string> step2InParam1 = batchNumber;
                    FieldReadOnly<string> step2InParam2;
                    FieldReadOnly<string> step2InParam3 = vendorID;
                    FieldReadOnly<string> step2InParam4 = batchNumber;
                    FieldReadOnly<decimal> step2InParam5;
                    FieldReadOnly<int> step2InParam6;
                    FieldReadOnly<decimal> step2InParam7 = taxDetailPurchaseAmount;
                    FieldReadOnly<string> step2InParam8 = functionalCurrency;
                    FieldReadOnly<short> step2InParam9 = unknown1;
                    for (int i = 0; i < taxID.Count; i++)
                    {
                        step2InParam2 = taxID[i];
                        step2InParam5 = taxDetailTaxAmount[i+1];
                        step2InParam6 = actIdx[i+1];

                        Vat.Procedures.TwCreatePayableTax.Invoke(step2InParam1, step2InParam2, step2InParam3, step2InParam4, step2InParam5, step2InParam6, step2InParam7, step2InParam8, step2InParam9);
                    }

                    //                                          1        2  3   4     5            6        7      8        9           10             11           12
                    //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 10, 44, 2, "14030297", 2/11/2015, "IDR", 0.00000, 0.00000, 4800000.00000, 4800000.00000, 0.0000000
                    //'TW_CREATE_PAYABLE_DISTRIBUTION', "VATIN1502008", 20, 25, 10, "14030297", 2/11/2015, "IDR", 4800000.00000, 4800000.00000, 0.00000, 0.00000, 0.0000000

                    FieldReadOnly<string> step3InParam1 = batchNumber;
                    FieldReadOnly<int> step3InParam2;
                    FieldReadOnly<int> step3InParam3;
                    FieldReadOnly<short> step3InParam4;
                    FieldReadOnly<string> step3InParam5 = vendorID;
                    FieldReadOnly<DateTime> step3InParam6 = trxDate;
                    FieldReadOnly<string> step3InParam7 = functionalCurrency;
                    FieldReadOnly<decimal> step3InParam8;
                    FieldReadOnly<decimal> step3InParam9;
                    FieldReadOnly<decimal> step3InParam10;
                    FieldReadOnly<decimal> step3InParam11;
                    FieldReadOnly<decimal> step3InParam12 = amount0;

                    for (int i = 0; i < distLine.Count; i++)
                    {
                        step3InParam2 = distLine[i];
                        step3InParam3 = actIdx[i];
                        step3InParam4 = distType[i];
                        step3InParam8 = distDebitAmount[i];
                        step3InParam9 = distDebitAmount[i];
                        step3InParam10 = distCreditAmount[i];
                        step3InParam11 = distCreditAmount[i];

                        Vat.Procedures.TwCreatePayableDistribution.Invoke(step3InParam1, step3InParam2, step3InParam3, step3InParam4, step3InParam5, step3InParam6, step3InParam7, step3InParam8, step3InParam9, step3InParam10, step3InParam11, step3InParam12);
                    }

                    //posting
                    //'TW_AUTO_POST_PAYABLE', "VATIN1502008", 2/11/2015

                    FieldReadOnly<string> step4InParam1 = batchNumber;
                    FieldReadOnly<DateTime> step4InParam2 = trxDate;

                    Vat.Procedures.TwAutoPostPayable.Invoke(step4InParam1, step4InParam2);
                }
            }
            else
            {
                MessageBox.Show("Can not get functional currency", "VAT in IDR");
                e.Cancel = true;
            }
            MCSetupTable.Close();//double close??should be fine
        }
    }
}
