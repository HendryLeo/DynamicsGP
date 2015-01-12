using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
//using Microsoft.Dexterity.Applications.DynamicsDictionary;
using Microsoft.Dexterity.Applications.VvfIdInHouseCustomizationDictionary;
using Microsoft.Dexterity.Applications.VvfDictionary;
using Microsoft.Dexterity.Applications.PurchaseRequisitionDictionary;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Data.Odbc;


namespace InventoryRules
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface

        static IvTransactionEntryForm IVTrxEntryForm = Vvf.Forms.IvTransactionEntry;
        static PopReceivingsEntryForm POPReceivingEntryForm = PurchaseRequisition.Forms.PopReceivingsEntry;
        static Microsoft.Dexterity.Applications.VvfDictionary.PorReturnsEntryForm PORReturnsEntryForm = Vvf.Forms.PorReturnsEntry;
        //static Microsoft.Dexterity.Applications.DynamicsDictionary.PopReceiptLookupForm POPReceiptLookupForm = Microsoft.Dexterity.Applications.Dynamics.Forms.PopReceiptLookup;

        static IvTransactionEntryForm.IvTransactionEntryWindow IVTrxEntryWindow = IVTrxEntryForm.IvTransactionEntry;
        static PopReceivingsEntryForm.PopReceivingsEntryWindow POPReceivingEntryWindow = POPReceivingEntryForm.PopReceivingsEntry;
        static Microsoft.Dexterity.Applications.VvfDictionary.PorReturnsEntryForm.PorReturnsEntryWindow PORReturnsEntryWindow = PORReturnsEntryForm.PorReturnsEntry;
        //static Microsoft.Dexterity.Applications.DynamicsDictionary.PopReceiptLookupForm.PopReceiptLookupWindow POPReceiptLookupWindow = POPReceiptLookupForm.PopReceiptLookup;

        public static Boolean POPReceivingEntryWindow_openSavedReceipt;//Saved and not user defined
        public static Boolean PORReturnsEntryWindow_openSavedReturn;//Saved and not user defined

        Boolean safeToEditReceipt(string Receiptnumber)
        {
            DateTime defaultGPDate = new DateTime(1900,1,1);
            TableError err;
            Boolean canEdit = false;
            string[] text;
            DateTime[] date;
            err = DataAccessHelper.GetPOPUserDefinedValues(Receiptnumber, out text, out date);
            
            switch(err)
            {
                case TableError.NotFound:
                    {
                        //row not found, clear to edit
                        canEdit = true;
                        break;
                    }
                case TableError.NoError:
                    {
                        //row found 
                        if (text[0].Trim() == String.Empty & text[5].Trim() == String.Empty & date[0] == defaultGPDate)
                        {
                            //
                            //MessageBox.Show("Receipt No: " + Receiptnumber + " has already been reportedto Custom with Registration No: " + text[5].Trim());
                            canEdit = true;
                        }
                        else
                        {
                            canEdit = false;
                        }
                        break;
                    }
                default:
                    {
                        canEdit = false;
                        //other tabel error operation
                        //MessageBox.Show("Table Operation Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
            }
            return canEdit;
        }

        DateTime GetNetworkTime()
        {
            DateTime sqlDate = new DateTime(1900,1,1);

            //const string ntpServer = "vvfi-dc01";
            //var ntpData = new byte[48];
            //ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            //var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            //var ipEndPoint = new IPEndPoint(addresses[0], 123);
            //var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            ////Stops code hang if NTP is blocked
            //socket.ReceiveTimeout = 3000;
            //socket.Connect(ipEndPoint);

            //socket.Send(ntpData);
            //socket.Receive(ntpData);
            //socket.Close();

            //ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            //ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            //var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            //var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            //return networkDateTime.ToLocalTime();
            
            string connectionString = "DSN=" + Microsoft.Dexterity.Applications.Dynamics.Globals.SqlDataSourceName + ";UID=GPDateGetter;PWD=5Rp6p_R4e8b4j^K;";
            try
            {
                using (OdbcConnection ODBC = new OdbcConnection(connectionString))
                {
                    ODBC.Open();
                    using (OdbcCommand com = new OdbcCommand("SELECT GETDATE()", ODBC))
                    {
                        using (OdbcDataReader reader = com.ExecuteReader())
                        {
                            reader.Read();
                            sqlDate = reader.GetDateTime(0);
                        }
                    }
                }
            }
            catch (System.Data.Odbc.OdbcException)
            {
                
            }
            
            return sqlDate;
        }

        public void Initialize()
        {
            IVTrxEntryWindow.PostButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(IVTrxEntryWindow_PostButton_ClickBeforeOriginal);
            IVTrxEntryWindow.SaveButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(IVTrxEntryWindow_SaveButton_ClickBeforeOriginal);
            IVTrxEntryWindow.SaveButton.ClickAfterOriginal += new System.EventHandler(IVTrxEntryWindow_SaveButton_ClickAfterOriginal);
            IVTrxEntryWindow.OpenAfterOriginal += new System.EventHandler(IVTrxEntryWindow_OpenAfterOriginal);

            POPReceivingEntryWindow.OpenAfterOriginal += new System.EventHandler(POPReceivingEntryWindow_OpenAfterOriginal);
            POPReceivingEntryWindow.SaveButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(POPReceivingEntryWindow_SaveButton_ClickBeforeOriginal);
            POPReceivingEntryWindow.BeforeModalDialog += new System.EventHandler<BeforeModalDialogEventArgs>(POPReceivingEntryWindow_BeforeModalDialog);
            POPReceivingEntryWindow.ReceiptDate.LeaveBeforeOriginal += new System.ComponentModel.CancelEventHandler(POPReceivingEntryWindow_ReceiptDate_LeaveBeforeOriginal);
            POPReceivingEntryWindow.PopReceiptNumber.Change += new System.EventHandler(POPReceivingEntryWindow_PopReceiptNumber_Change);
            POPReceivingEntryWindow.LocalAutoRcvButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(POPReceivingEntryWindow_LocalAutoRcvButton_ClickBeforeOriginal);
            POPReceivingEntryWindow.LineScroll.LineDeleteBeforeOriginal += POPReceivingEntryWindow_LineScroll_LineDeleteBeforeOriginal;
            POPReceivingEntryWindow.LineScroll.LineFillBeforeOriginal += POPReceivingEntryWindow_LineScroll_LineFillBeforeOriginal;
            POPReceivingEntryWindow.LineScroll.LineInsertBeforeOriginal += POPReceivingEntryWindow_LineScroll_LineInsertBeforeOriginal;


            PORReturnsEntryWindow.OpenAfterOriginal += new System.EventHandler(PORReturnsEntryWindow_OpenAfterOriginal);
            PORReturnsEntryWindow.SaveButton.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(PORReturnsEntryWindow_SaveButton_ClickBeforeOriginal);
            PORReturnsEntryWindow.BeforeModalDialog += new System.EventHandler<BeforeModalDialogEventArgs>(PORReturnsEntryWindow_BeforeModalDialog);
            PORReturnsEntryWindow.ReceiptDate.LeaveBeforeOriginal += new System.ComponentModel.CancelEventHandler(PORReturnsEntryWindow_ReceiptDate_LeaveBeforeOriginal);
            PORReturnsEntryWindow.PopReceiptNumber.Change += new System.EventHandler(PORReturnsEntryWindow_PopReceiptNumber_Change);
            PORReturnsEntryWindow.ReturnsScroll.LineFillBeforeOriginal += new System.ComponentModel.CancelEventHandler(PORReturnsEntryWindow_ReturnsScroll_LineFillBeforeOriginal);
            PORReturnsEntryWindow.ReturnsScroll.LineInsertBeforeOriginal += new System.ComponentModel.CancelEventHandler(PORReturnsEntryWindow_ReturnsScroll_LineInsertBeforeOriginal);
            PORReturnsEntryWindow.ReturnsScroll.LineDeleteBeforeOriginal += new System.ComponentModel.CancelEventHandler(PORReturnsEntryWindow_ReturnsScroll_LineDeleteBeforeOriginal);
            PORReturnsEntryWindow.LookupButton4.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(LookupButton4_ClickBeforeOriginal);
            PORReturnsEntryWindow.ReplaceReturnedGoods.ValidateBeforeOriginal += new System.ComponentModel.CancelEventHandler(ReplaceReturnedGoods_ValidateBeforeOriginal);
            PORReturnsEntryWindow.InvoiceExpectedReturns.ValidateBeforeOriginal += new System.ComponentModel.CancelEventHandler(InvoiceExpectedReturns_ValidateBeforeOriginal);
        }

        void PORReturnsEntryWindow_ReceiptDate_LeaveBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateTime serverDate;
            if (IVDateRule())
            {
                serverDate = GetNetworkTime().Date;
                if (PORReturnsEntryWindow.ReceiptDate.Value.Date != serverDate)
                {
                    MessageBox.Show("Document Date cannot be in the past, please re-enter.", "enforceServerDate", MessageBoxButtons.OK);
                    PORReturnsEntryWindow.ReceiptDate.Value = serverDate;
                    PORReturnsEntryWindow.ReceiptDate.Focus();
                    e.Cancel = true;
                }
            }

        }

        void PORReturnsEntryWindow_BeforeModalDialog(object sender, BeforeModalDialogEventArgs e)
        {
            DateTime serverDate;
            switch (e.Message)
            {
                case "Do you want to save or delete the document?":
                    {
                        if (IVDateRule())
                        {
                            serverDate = GetNetworkTime().Date;
                            if (PORReturnsEntryWindow.ReceiptDate.Value.Date != serverDate)
                            {
                                MessageBox.Show("Document Date cannot be in the past, please re-enter.", "enforceServerDate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //PORReturnsEntryWindow.ReceiptDate.Value = serverDate;
                                PORReturnsEntryWindow.ReceiptDate.Focus();
                                e.Response = DialogResponse.Button3;
                            }
                        }
                        break;
                    }
            }

        }

        void PORReturnsEntryWindow_SaveButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateTime serverDate;
            if (IVDateRule())
            {
                serverDate = GetNetworkTime().Date;
                if (PORReturnsEntryWindow.ReceiptDate.Value.Date != serverDate)
                {
                    MessageBox.Show("Document Date cannot be in the past, please re-enter.", "enforceServerDate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //PORReturnsEntryWindow.ReceiptDate.Value = serverDate;
                    PORReturnsEntryWindow.ReceiptDate.Focus();
                    e.Cancel = true;
                }
            }
        }

        void PORReturnsEntryWindow_OpenAfterOriginal(object sender, EventArgs e)
        {
            DateTime serverDate;
            if (IVDateRule())
            {
                serverDate = GetNetworkTime().Date;
                PORReturnsEntryWindow.ReceiptDate.Value = serverDate;
                PORReturnsEntryWindow.ReceiptDate.Lock();
                PORReturnsEntryWindow.ExpansionButton1.Lock();
            }

        }

        void InvoiceExpectedReturns_ValidateBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PORReturnsEntryWindow_openSavedReturn)
            {
                e.Cancel = true;
                PORReturnsEntryWindow.IsChanged = false;
            }
        }

        void ReplaceReturnedGoods_ValidateBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PORReturnsEntryWindow_openSavedReturn)
            {
                e.Cancel = true;
                PORReturnsEntryWindow.IsChanged = false;
            }
        }

        void LookupButton4_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PORReturnsEntryWindow_openSavedReturn) e.Cancel = true;
        }

        void PORReturnsEntryWindow_ReturnsScroll_LineDeleteBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PORReturnsEntryWindow_openSavedReturn) e.Cancel = true;
        }

        void PORReturnsEntryWindow_ReturnsScroll_LineInsertBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PORReturnsEntryWindow_openSavedReturn) e.Cancel = true;
        }

        void PORReturnsEntryWindow_ReturnsScroll_LineFillBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PORReturnsEntryWindow_openSavedReturn) e.Cancel = true;
        }

        void PORReturnsEntryWindow_PopReceiptNumber_Change(object sender, EventArgs e)
        {
            if (PORReturnsEntryWindow.PopReceiptNumber.Value.Trim() != String.Empty)
            {
                if (IVDateRule())
                {
                    if (safeToEditReceipt(PORReturnsEntryWindow.PopReceiptNumber.Value.Trim()))
                    {
                        //save to edit
                        PORReturnsEntryWindow_openSavedReturn = false;
                        //enable receiptdate
                        //PORReturnsEntryWindow.ReceiptDate.Enable();
                        //PORReturnsEntryWindow.ExpansionButton1.Enable();
                        PORReturnsEntryWindow.VendorDocumentNumber.Enable();
                        //enable record manipulation fields
                        PORReturnsEntryWindow.SaveButton.Enable();
                        PORReturnsEntryWindow.DeleteButton.Enable();
                        PORReturnsEntryWindow.PostButton.Enable();
                        PORReturnsEntryWindow.LocalReturnType.Enable();
                        PORReturnsEntryWindow.VendorName.Enable();
                        PORReturnsEntryWindow.CurrencyId.Enable();
                        PORReturnsEntryWindow.ExpansionButton3.Enable();
                        PORReturnsEntryWindow.ReplaceReturnedGoods.Enable();
                        PORReturnsEntryWindow.InvoiceExpectedReturns.Enable();
                        PORReturnsEntryWindow.LookupButton5.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.PoNumber.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.VendorItemNumber.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.ItemNumber.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.ReceiptReturnNumber.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.UOfM.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.QtyReserved.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.UnitCost.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.VendorItemDescription.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.ItemDescription.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.InventoryAccount.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.UnrealizedPurchasePriceVarianceAccount.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.CommentId.Enable();
                        PORReturnsEntryWindow.ReturnsScroll.LookupButton3.Enable();
                        PORReturnsEntryWindow.DistributionsButton.Enable();
                        PORReturnsEntryWindow.LookupButton4.Enable();
                        PORReturnsEntryWindow.LookupButton6.Enable();
                        PORReturnsEntryWindow.LookupButton7.Enable();
                        PORReturnsEntryWindow.LookupButton8.Enable();
                    }
                    else
                    {
                        //calling saved Receipt that has been reported
                        MessageBox.Show("You are opening a saved Document that is not allowed to be changed" + System.Environment.NewLine + "Opening Inquiry Instead", "enforceServerDate", MessageBoxButtons.OK);
                        PORReturnsEntryWindow_openSavedReturn = true;
                        //PORReturnsEntryWindow.ReceiptDate.Disable();
                        //PORReturnsEntryWindow.ExpansionButton1.Disable();
                        PORReturnsEntryWindow.VendorDocumentNumber.Disable();
                        //disable record manipulation fields
                        PORReturnsEntryWindow.SaveButton.Disable();
                        PORReturnsEntryWindow.DeleteButton.Disable();
                        PORReturnsEntryWindow.PostButton.Disable();
                        PORReturnsEntryWindow.LocalReturnType.Disable();
                        PORReturnsEntryWindow.VendorName.Disable();
                        PORReturnsEntryWindow.CurrencyId.Disable();
                        PORReturnsEntryWindow.ExpansionButton3.Disable();
                        PORReturnsEntryWindow.ReplaceReturnedGoods.Disable();
                        PORReturnsEntryWindow.InvoiceExpectedReturns.Disable();
                        PORReturnsEntryWindow.LookupButton5.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.PoNumber.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.VendorItemNumber.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.ItemNumber.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.ReceiptReturnNumber.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.UOfM.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.QtyReserved.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.UnitCost.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.VendorItemDescription.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.ItemDescription.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.InventoryAccount.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.UnrealizedPurchasePriceVarianceAccount.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.CommentId.Disable();
                        PORReturnsEntryWindow.ReturnsScroll.LookupButton3.Disable();
                        PORReturnsEntryWindow.DistributionsButton.Disable();
                        PORReturnsEntryWindow.LookupButton4.Disable();
                        PORReturnsEntryWindow.LookupButton6.Disable();
                        PORReturnsEntryWindow.LookupButton7.Disable();
                        PORReturnsEntryWindow.LookupButton8.Disable();

                        //call inquiry window
                        //sanscript
                        //'OpenWindow() of form POR_Inquiry_Returns_Entry', 0, "RTS1400011", 4, 2, 2
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

                                passthrough_code += @"OpenWindow(""" + PORReturnsEntryWindow.PopReceiptNumber.Value + @""",4,2,2) of form POR_Inquiry_Returns_Entry;";

                                //not needed in my case, and it will auto adapt to the alternate/modified setup
                                //gpApp.CurrentProductID = 0;

                                error_code = gpApp.ExecuteSanscript(passthrough_code, out compile_err);
                            }
                        }
                        catch
                        {
                            //MessageBox.Show("Failed to initialize gpApp");
                            //return;
                        }
                    }
                }
            }
        }

        void POPReceivingEntryWindow_LocalAutoRcvButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (POPReceivingEntryWindow_openSavedReceipt) e.Cancel = true;
        }

        void POPReceivingEntryWindow_LineScroll_LineInsertBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(POPReceivingEntryWindow_openSavedReceipt) e.Cancel = true;
        }

        void POPReceivingEntryWindow_LineScroll_LineFillBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (POPReceivingEntryWindow_openSavedReceipt) e.Cancel = true;
        }

        void POPReceivingEntryWindow_LineScroll_LineDeleteBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (POPReceivingEntryWindow_openSavedReceipt) e.Cancel = true;
        }

        void POPReceivingEntryWindow_PopReceiptNumber_Change(object sender, EventArgs e)
        {
            if (POPReceivingEntryWindow.PopReceiptNumber.Value.Trim() != String.Empty)
            {
                if (IVDateRule())
                {
                    if (safeToEditReceipt(POPReceivingEntryWindow.PopReceiptNumber.Value.Trim()))
                    {
                        //save to edit
                        POPReceivingEntryWindow_openSavedReceipt = false;
                        //enable receiptdate
                        //POPReceivingEntryWindow.ReceiptDate.Enable();
                        //enable record manipulation fields
                        POPReceivingEntryWindow.SaveButton.Enable();
                        POPReceivingEntryWindow.DeleteButton.Enable();
                        POPReceivingEntryWindow.VoidButtonI.Enable();
                        POPReceivingEntryWindow.PostButton.Enable();
                        POPReceivingEntryWindow.LocalAutoRcvButton.Enable();
                        POPReceivingEntryWindow.PopType.Enable();
                        POPReceivingEntryWindow.VendorName.Enable();
                        POPReceivingEntryWindow.CurrencyId.Enable();
                        POPReceivingEntryWindow.LookupButton5.Enable();
                        POPReceivingEntryWindow.LineScroll.PoNumber.Enable();
                        POPReceivingEntryWindow.LineScroll.VendorItemNumber.Enable();
                        POPReceivingEntryWindow.LineScroll.ItemNumber.Enable();
                        POPReceivingEntryWindow.LineScroll.QtyShipped.Enable();
                        POPReceivingEntryWindow.LineScroll.UnitCost.Enable();
                        POPReceivingEntryWindow.LineScroll.UOfM.Enable();
                        POPReceivingEntryWindow.LineScroll.LocationCode.Enable();
                        POPReceivingEntryWindow.LineScroll.QtyOrdered.Enable();
                        POPReceivingEntryWindow.LineScroll.QtyInvoiced.Enable();
                        POPReceivingEntryWindow.LineScroll.ExtendedCost.Enable();
                        POPReceivingEntryWindow.LineScroll.ExtendedCost.Lock();
                        POPReceivingEntryWindow.LineScroll.VendorItemDescription.Enable();
                        POPReceivingEntryWindow.LineScroll.ItemDescription.Enable();
                        POPReceivingEntryWindow.LineScroll.Remarks.Enable();
                        POPReceivingEntryWindow.LineScroll.QtyPrevShipped.Enable();
                        POPReceivingEntryWindow.LineScroll.QtyPrevInvoiced.Enable();
                        POPReceivingEntryWindow.LocalLandedCostButton.Enable();
                        POPReceivingEntryWindow.DistributionsButton.Enable();
                        POPReceivingEntryWindow.UserDefinedButton.Enable();
                        POPReceivingEntryWindow.LookupButton4.Enable();
                        POPReceivingEntryWindow.LookupButton6.Enable();
                        POPReceivingEntryWindow.LookupButton8.Enable();
                        POPReceivingEntryWindow.LookupButton9.Enable();
                        POPReceivingEntryWindow.VendorDocumentNumber.Enable();
                    }
                    else
                    {
                        //calling saved Receipt that has been reported
                        MessageBox.Show("You are opening a saved Document that is not allowed to be changed" + System.Environment.NewLine + "Opening Inquiry Instead", "enforceServerDate", MessageBoxButtons.OK);
                        POPReceivingEntryWindow_openSavedReceipt = true;
                        //disable record manipulation fields
                        POPReceivingEntryWindow.SaveButton.Disable();
                        POPReceivingEntryWindow.DeleteButton.Disable();
                        POPReceivingEntryWindow.VoidButtonI.Disable();
                        POPReceivingEntryWindow.PostButton.Disable();
                        POPReceivingEntryWindow.LocalAutoRcvButton.Disable();
                        POPReceivingEntryWindow.PopType.Disable();
                        POPReceivingEntryWindow.VendorName.Disable();
                        POPReceivingEntryWindow.CurrencyId.Disable();
                        POPReceivingEntryWindow.LookupButton5.Disable();
                        POPReceivingEntryWindow.LineScroll.PoNumber.Disable();
                        POPReceivingEntryWindow.LineScroll.VendorItemNumber.Disable();
                        POPReceivingEntryWindow.LineScroll.ItemNumber.Disable();
                        POPReceivingEntryWindow.LineScroll.QtyShipped.Disable();
                        POPReceivingEntryWindow.LineScroll.UnitCost.Disable();
                        POPReceivingEntryWindow.LineScroll.UOfM.Disable();
                        POPReceivingEntryWindow.LineScroll.LocationCode.Disable();
                        POPReceivingEntryWindow.LineScroll.QtyOrdered.Disable();
                        POPReceivingEntryWindow.LineScroll.QtyInvoiced.Disable();
                        POPReceivingEntryWindow.LineScroll.ExtendedCost.Disable();
                        POPReceivingEntryWindow.LineScroll.ExtendedCost.Lock();
                        POPReceivingEntryWindow.LineScroll.VendorItemDescription.Disable();
                        POPReceivingEntryWindow.LineScroll.ItemDescription.Disable();
                        POPReceivingEntryWindow.LineScroll.Remarks.Disable();
                        POPReceivingEntryWindow.LineScroll.QtyPrevShipped.Disable();
                        POPReceivingEntryWindow.LineScroll.QtyPrevInvoiced.Disable();
                        POPReceivingEntryWindow.LocalLandedCostButton.Disable();
                        POPReceivingEntryWindow.DistributionsButton.Disable();
                        POPReceivingEntryWindow.UserDefinedButton.Disable();
                        POPReceivingEntryWindow.LookupButton4.Disable();
                        POPReceivingEntryWindow.LookupButton6.Disable();
                        POPReceivingEntryWindow.LookupButton8.Disable();
                        POPReceivingEntryWindow.LookupButton9.Disable();
                        POPReceivingEntryWindow.VendorDocumentNumber.Disable();

                        //call inquiry window
                        //sanscript
                        //'OpenWindow() of form POP_Inquiry_Receivings_Entry', 0, "GRN1400953", 2, 3, 2
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

                                passthrough_code += @"OpenWindow(""" + POPReceivingEntryWindow.PopReceiptNumber.Value + @""",2,4,2) of form POP_Inquiry_Receivings_Entry;";

                                //not needed in my case, and it will auto adapt to the alternate/modified setup
                                //gpApp.CurrentProductID = 0;

                                error_code = gpApp.ExecuteSanscript(passthrough_code, out compile_err);
                            }
                        }
                        catch
                        {
                            //MessageBox.Show("Failed to initialize gpApp");
                            //return;
                        }
                    }
                }
            }
        }

        void POPReceivingEntryWindow_ReceiptDate_LeaveBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateTime serverDate;
            if (IVDateRule())
            {
                serverDate = GetNetworkTime().Date;
                if (POPReceivingEntryWindow.ReceiptDate.Value.Date != serverDate)
                {
                    MessageBox.Show("Document Date cannot be in the past, please re-enter.", "enforceServerDate", MessageBoxButtons.OK);
                    POPReceivingEntryWindow.ReceiptDate.Value = serverDate;
                    POPReceivingEntryWindow.ReceiptDate.Focus();
                    e.Cancel = true;
                }
            }
        }

        void POPReceivingEntryWindow_BeforeModalDialog(object sender, BeforeModalDialogEventArgs e)
        {
            DateTime serverDate;
            switch (e.Message)
            {
                case "Do you want to save or delete the document?":
                    {
                        if (IVDateRule())
                        {
                            serverDate = GetNetworkTime().Date;
                            if (POPReceivingEntryWindow.ReceiptDate.Value.Date != serverDate)
                            {
                                MessageBox.Show("Document Date cannot be in the past, please re-enter.", "enforceServerDate", MessageBoxButtons.OK,MessageBoxIcon.Error);
                                //POPReceivingEntryWindow.ReceiptDate.Value = serverDate;
                                POPReceivingEntryWindow.ReceiptDate.Focus();
                                e.Response = DialogResponse.Button3;
                            }
                        }
                        break;
                    }
            }
        }

        void POPReceivingEntryWindow_SaveButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateTime serverDate;
            if (IVDateRule())
            {
                serverDate = GetNetworkTime().Date;
                if (POPReceivingEntryWindow.ReceiptDate.Value.Date != serverDate)
                {
                    MessageBox.Show("Document Date cannot be in the past, please re-enter.", "enforceServerDate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //POPReceivingEntryWindow.ReceiptDate.Value = serverDate;
                    POPReceivingEntryWindow.ReceiptDate.Focus();
                    e.Cancel = true;
                }
            }
        }

        void POPReceivingEntryWindow_OpenAfterOriginal(object sender, EventArgs e)
        {
            DateTime serverDate;
            if (IVDateRule())
            {
                serverDate = GetNetworkTime().Date;
                POPReceivingEntryWindow.ReceiptDate.Value = serverDate;
                POPReceivingEntryWindow.ReceiptDate.Lock();
                POPReceivingEntryWindow.ExpansionButton1.Lock();
            }
        }

        void IVTrxEntryWindow_SaveButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult answer;
            if (UserDefaults1())
            {
                if (IVTrxEntryWindow.VvfAdjtype == 1)
                {
                    answer = MessageBox.Show("Anda yakin akan menyimpan Adjustment Type = IN", "Adjustment IN", MessageBoxButtons.YesNo);
                    if (answer == DialogResult.No) e.Cancel = true;
                }
            }
        }

        void IVTrxEntryWindow_OpenAfterOriginal(object sender, EventArgs e)
        {
            if (UserDefaults1())
            {
                IVTrxEntryWindow.LocalDefaultSite.Value = "STORE";
                IVTrxEntryWindow.VvfAdjtype.Value = 2;
                IVTrxEntryWindow.BatchNumber.Value = "ISSUE SEMENTARA";
                IVTrxEntryWindow.Changed.Value = false;
            }
        }

        void IVTrxEntryWindow_SaveButton_ClickAfterOriginal(object sender, EventArgs e)
        {
            if(UserDefaults1())
            {
                IVTrxEntryWindow.LocalDefaultSite.Value = "STORE";
                IVTrxEntryWindow.VvfAdjtype.Value = 2;
                IVTrxEntryWindow.BatchNumber.Value = "ISSUE SEMENTARA";
                IVTrxEntryWindow.Changed.Value = false;
            }
        }

        void IVTrxEntryWindow_PostButton_ClickBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(UserCanPost()))
            {
                MessageBox.Show("Jangan Post dari sini, Silahkan Post dari Series Post", "Rule 1", MessageBoxButtons.OK);
                e.Cancel = true;
            }
        }

        Boolean IVDateRule()
        {
            Boolean enforceServerDate = true;
            string[] targets;
            TableError err;

            err = DataAccessHelper.GetIVRuleTargetsByID(3, out targets);
            if (err == TableError.NoError)
            {
                if (Array.IndexOf(targets, Microsoft.Dexterity.Applications.Dynamics.Globals.UserId) == -1)
                {
                    enforceServerDate = true;
                }
            }

            return enforceServerDate;
        }

        Boolean UserDefaults1()
        {
            Boolean useDefault = false;
            string[] targets;
            TableError err;

            err = DataAccessHelper.GetIVRuleTargetsByID(2, out targets);
            if (err == TableError.NoError)
            {
                if (Array.IndexOf(targets, Microsoft.Dexterity.Applications.Dynamics.Globals.UserId) == -1) 
                {
                    useDefault = true;
                }
            }
            return useDefault;
        }

        Boolean UserCanPost()
        {
            Boolean userCanPost = false;
            string[] targets;
            TableError err;

            err = DataAccessHelper.GetIVRuleTargetsByID(1, out targets);
            if (err == TableError.NoError)
            {
                if (Array.IndexOf(targets, Microsoft.Dexterity.Applications.Dynamics.Globals.UserId) == -1) 
                {
                    userCanPost = true;
                }
            }
            return userCanPost;
        }
    }
}
